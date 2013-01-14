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

        public static Message InvalidMessage = new Message(-1, "INVALID", "Not a current student ID.");
        public static Message RepeatMessage = new Message(-1, "REPEAT", "ID was already scanned.");
        public static Message UnknownMessage = new Message(-1, "ERROR", "An unknown error occurred.");
    }

    class Messages : SortedList<short, Message> { }
}
