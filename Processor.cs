using System;

using System.Collections.Generic;
using System.Text;

using System.IO;

namespace SU_MT2000_SUIDScanner
{
    class Processor
    {
        private const string InputFileName = "\\Application\\0-SUID\\admit-list.csv";
        private const string OutputFileName = "\\Application\\0-SUID\\output-{0:ddd}-{0:HH}-{0:mm}.csv";

        public static SUIDs SUIDs = new SUIDs();

        public bool Load()
        {
            StreamReader file = new StreamReader(InputFileName);
            string line;

            while ((line = file.ReadLine()) != null) {
                string[] fields = line.Split(',');
                AddSUID(fields[0], (AdmitFlags)Enum.Parse(typeof(AdmitFlags), fields[2],true), Flags.NONE);
            }
            return true;
        }

        private void AddSUID(string barcode_id, AdmitFlags admit_flag, Flags flags) {
            if (SUIDs.ContainsKey(barcode_id)) {
                SUIDs.Remove(barcode_id);
            }
            SUIDs.Add(barcode_id, new SUID(barcode_id, admit_flag, flags));
        }

        public bool ShouldAdmit(string barcode_id)
        {
            if (SUIDs.ContainsKey(barcode_id) && SUIDs[barcode_id].admit_flag == AdmitFlags.ADMIT)
            {
                return true;
            }
            return false;
        }

        public bool Admit(string barcode_id)
        {
            if (!SUIDs.ContainsKey(barcode_id)) {
                AddSUID(barcode_id, AdmitFlags.ERR_NOT_INVITED, Flags.FORCE_CHECKED);
            }

            SUIDs[barcode_id].admitted = true;
            return true;
        }

        public AdmitFlags GetDenyCode(string barcode_id)
        {
            if (!SUIDs.ContainsKey(barcode_id))
            {
                AddSUID(barcode_id, AdmitFlags.ERR_NOT_ID, Flags.FORCE_CHECKED);
            }
            return SUIDs[barcode_id].admit_flag;
        }

        public static bool Save()
        {
            StreamWriter writer = null;
            char separator = ',';
            bool result = true;

            // write the inventory to the text file
            try
            {
                string outputFile = String.Format(OutputFileName, DateTime.Now);
                FileInfo fileInfo = new FileInfo(outputFile);
                writer = fileInfo.CreateText();
                foreach (KeyValuePair<string, SUID> ids in SUIDs)
                {
                    SUID id = ids.Value;
                    writer.WriteLine(id.id + separator + id.admitted + separator + (short)id.flags);
                }
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
            return result;
        }

    }
}
