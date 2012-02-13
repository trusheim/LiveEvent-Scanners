using System;

using System.Collections.Generic;
using System.Text;

namespace SU_MT2000_SUIDScanner
{

    enum AdmitFlags : short
    {
        ADMIT = -1,
        ERR_NOT_INVITED = 10,
        ERR_NOT_ID = 1,
        ERR_EPGY = 2,
        ERR_ALUMNI = 3,
        ERR_UNDER21 = 11,
        ERR_ONLEAVE = 12,
        ERR_SENIORNOWAIVER = 20,
        ERR_FRESHMAN = 30,
        ERR_SOPHOMORE = 31,
        ERR_JUNIOR = 32,
        ERR_SENIOR = 33,
        ERR_GRAD = 34
    }

    enum Flags : short 
    {
        NONE = 0,
        FORCE_CHECKED = 1
    }

    class SUID
    {
        public string id = "";
        //public bool is_21 = true; // TODO: Add is_21 flag
        public AdmitFlags admit_flag = 0;
        public Flags flags = 0;
        public bool admitted = false;

        public SUID(string barcode_id, AdmitFlags admit_flag, Flags flags)
        {
            this.id = barcode_id;
            this.admit_flag = admit_flag;
            this.flags = flags;
        }
    }

    class SUIDs : SortedList<string,SUID> { }
}
