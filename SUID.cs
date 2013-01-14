using System;

using System.Collections.Generic;
using System.Text;

namespace SU_MT2000_SUIDScanner
{

    enum Flags : short 
    {
        NONE = 0,
        UNRECOGNIZED = 1,
        FORCED = 2,
        REPEAT = 3
    }

    class SUID
    {
        public string id = "";
        public bool over_21 = false;
        public bool admit_flag = false;
        public Flags flags = 0;
        public short messageId = 0;
        public DateTime admit_time = DateTime.Now;


        public SUID(string barcode_id, bool over_21, bool admit_flag, Flags flags, short messageId)
        {
            this.id = barcode_id;
            this.over_21 = over_21;
            this.admit_flag = admit_flag;
            this.flags = flags;
            this.messageId = messageId;
        }

        public void setAdmitTime(DateTime admit_time)
        {
            this.admit_time = admit_time;
        }

    }

    class SUIDs : SortedList<string,SUID> { }
}
