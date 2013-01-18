using System;

using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SU_MT2000_SUIDScanner
{
    class ScanLog
    {

        private const string SaveFolder = "\\Application Data\\SU_MT2000_SUIDScanner\\scan-logs\\";
        private const string FileNameFormat = "{0}-{1}.csv";
        private const string HeaderFormat = "!!!LID///DIF2-O///{0}///{1}///\n!!! LiveID Scan log file for {0} started {1}";
        private const string LineFormat = "{0},{1}";

        private string eventName;
        private string eventId;
        protected StreamWriter oStream;

        public ScanLog(CardProcessor p)
        {
            this.eventName = p.EventName;
            this.eventId = p.EventId;

            //Find unix timestamp
            long ticks = DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
            ticks /= 10000000; //Convert windows ticks to seconds
            string timestamp = ticks.ToString();

            try
            {
                string filename = SaveFolder + String.Format(FileNameFormat, this.eventId, timestamp);
                FileInfo fileInfo = new FileInfo(filename);
                oStream = fileInfo.CreateText();
                oStream.WriteLine(String.Format(HeaderFormat, eventId, timestamp));
                oStream.Flush();
            }
            catch (IOException e)
            {
                oStream = null;
                throw e;
            }
        }

        ~ScanLog()
        {
            if (oStream != null)
            {
                oStream.Close();
                oStream = null;
            }
        }

        public void Log(string barcode_id, Message m) {
            if (oStream == null) return;

            oStream.WriteLine(String.Format(LineFormat, barcode_id, m.id.ToString()));
            oStream.Flush();
        }

    }
}
