using System;

using System.Collections.Generic;
using System.Text;

using System.IO;

namespace SU_MT2000_SUIDScanner
{
    class AdmitList
    {
        private const string InputFileName = "\\Application Data\\SU_MT2000_SUIDScanner\\admit-list.dat";
        private const string OutputFileName = "\\Application Data\\SU_MT2000_SUIDScanner\\output-{0:ddd}-{0:HH}-{0:mm}.csv";

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
                throw new Exception(String.Format("Could not read the admit list. Ensure {{0}} exists",filename));
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

    }
}
