using System;

using System.Collections.Generic;
using System.Text;

using System.IO;

namespace SU_MT2000_SUIDScanner
{
    class AdmitList
    {
        private const string AdmitListFolder = "\\Application Data\\SU_MT2000_SUIDScanner\\admit-lists\\";
        private const string FIRST_LINE_REGEX = "^!!!LID///DIF2///(.+)///([0-9]+)///([0-9]+)///$";

        public static string[] GetAllAdmitListFilenames()
        {
            return System.IO.Directory.GetFiles(AdmitListFolder, "*.dat");
        }

        public static AdmitListInfo[] GetAllAdmitLists()
        {
            List<AdmitListInfo> admitLists = new List<AdmitListInfo>();
            string[] filenames = GetAllAdmitListFilenames();
            foreach (string filename in filenames)
            {
                System.Diagnostics.Debug.WriteLine(filename);
                admitLists.Add(GetAdmitListInfo(filename));
            }
            return admitLists.ToArray();
        }

        public static AdmitListInfo GetAdmitListInfo(string filename)
        {
            StreamReader file = new StreamReader(filename);
            string line = file.ReadLine();
            file.Close();

            if (!System.Text.RegularExpressions.Regex.IsMatch(line,FIRST_LINE_REGEX)) {
                throw new Exception(String.Format("The file {0} did not define an admit list file.", filename));
            }

            try {
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(line,FIRST_LINE_REGEX);
                string eventName = match.Groups[1].ToString();
                string dataDateStr = match.Groups[2].ToString();
                string exportDateStr = match.Groups[3].ToString();
                DateTime dataDate = UnixTimeStampToDateTime(Convert.ToDouble(dataDateStr));
                DateTime exportDate = UnixTimeStampToDateTime(Convert.ToDouble(exportDateStr));

                AdmitListInfo info = new AdmitListInfo();
                info.filePath = filename;
                info.eventName = eventName;
                info.dataDate = dataDate;
                info.exportDate = exportDate;

                return info;
            } catch (Exception e) {
                throw new Exception(String.Format("Could not process the file {0}",filename), e);
            }
        }

        /// <summary>
        /// http://stackoverflow.com/questions/249760/how-to-convert-unix-timestamp-to-datetime-and-vice-versa
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0);
            dtDateTime = dtDateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
            return dtDateTime;
        }

        #region Card Processor setup functions
        public static void SetupProcessorFromFile(string filename, CardProcessor cardProcessor)
        {
            try
            {
                StreamReader file = new StreamReader(filename);
                string line;
                short current_mode = -1;

                while ((line = file.ReadLine()) != null)
                {
                    line = line.TrimEnd();

                    if (line[0] == '#')
                    {
                        continue;
                    }
                    else if (line == "!!!MESSAGES")
                    {
                        current_mode = 0;
                        continue;
                    }
                    else if (line == "!!!IDCARDS")
                    {
                        current_mode = 1;
                        continue;
                    }

                    if (current_mode == 0)
                    {
                        AddMessageLine(cardProcessor,line);
                    }
                    else if (current_mode == 1)
                    {
                        AddSUIDLine(cardProcessor,line);
                    }

                }
            } catch (IOException) {
                throw new Exception(String.Format("Could not read the admit list. Ensure {0} exists", filename));
            }
        }

        private static void AddMessageLine(CardProcessor p, string line)
        {
            string[] fields = line.Split(',');
            try
            {
                short id = Convert.ToInt16(fields[0]);
                p.AddMessage(new Message(id, fields[1], fields[2]));
            }
            catch (Exception)
            {
                // pass this line over
            }
        }

        private static void AddSUIDLine(CardProcessor p, string line)
        {
            string[] fields = line.Split(',');
            Boolean over_21 = fields[1].Equals("1");
            Boolean admit = fields[2].Equals("1");

            short message = 0;
            try {
                message = Convert.ToInt16(fields[3]);
            } catch (Exception) {
                message = 0;
            }

            p.AddSUID(new SUID(fields[0], over_21, admit, message));
        }

        #endregion

    }

    struct AdmitListInfo {
        public string eventName;
        public string filePath;
        public DateTime dataDate;
        public DateTime exportDate;
    }
}
