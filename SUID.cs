using System;

using System.Collections.Generic;
using System.Text;

namespace SU_MT2000_SUIDScanner
{

    enum Flags : short 
    {
        NONE = 0,
        UNRECOGNIZED = 1,
        FOLLOWUP = 2,
        REPEAT = 3
    }

    class SUID
    {
        public string id = "";
        public bool over_21 = false;
        public bool admit = false;
        public Flags flags = Flags.NONE;
        public short messageId = 0;
        public DateTime admit_time = DateTime.MinValue;
        public bool admitted = false;


        public SUID(string barcode_id, bool over_21, bool admit, short messageId)
        {
            this.id = barcode_id;
            this.over_21 = over_21;
            this.admit = admit;
            this.messageId = messageId;
        }

        public void setAdmitTime(DateTime admit_time)
        {
            this.admit_time = admit_time;
        }

    }

    class SUIDs : SortedList<string,SUID> { }
}
