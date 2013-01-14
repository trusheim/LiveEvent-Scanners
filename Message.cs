using System;

using System.Collections.Generic;
using System.Text;

namespace SU_MT2000_SUIDScanner
{
    class Message
    {
        public short id = 0;
        public string big_message = "";
        public string small_message = "";

        public Message(short id, string big_message, string small_message)
        {
            this.id = id;
            this.big_message = big_message;
            this.small_message = small_message;
        }

    }

    class Messages : SortedList<short, Message> { }
}
