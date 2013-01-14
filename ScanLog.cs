using System;

using System.Collections.Generic;
using System.Text;

namespace SU_MT2000_SUIDScanner
{
    class ScanLog
    {
        public static bool Save()
        {
            /*StreamWriter writer = null;
            char separator = ',';
            bool result = true;

            // write the inventory to the text file
            try
            {
                string outputFile = String.Format(OutputFileName, DateTime.Now);
                FileInfo fileInfo = new FileInfo(outputFile);
                writer = fileInfo.CreateText();
                foreach (KeyValuePair<string, SUID> ids in admitted)
                {
                    SUID id = ids.Value;
                    writer.WriteLine(id.id + separator + (short)id.flags);
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
            return result;*/
            return false;
        }
    }
}
