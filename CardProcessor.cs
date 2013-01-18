using System;

using System.Collections.Generic;
using System.Text;

namespace SU_MT2000_SUIDScanner
{

    enum AdmitStatus {OKAY,REPEAT,NO};

    class CardProcessor
    {
        #region Data & management methods
        public string EventName = "";
        public string EventId = "";
        public bool EnableScanLog = false;

        public static TimeSpan REPEAT_OK_TIME = new TimeSpan(0, 0, 30);

        protected SUIDs SUIDs = new SUIDs();
        protected Messages Messages = new Messages();

        protected ScanLog log;

        public CardProcessor(string eventName, string eventId)
        {
            this.EventId = eventId;
            this.EventName = eventName;
            this.log = new ScanLog(this);
        }

        public void AddMessage(Message message)
        {
            if (Messages.ContainsKey(message.id))
            {
                return;
            }
            Messages.Add(message.id, message);
        }

        public void AddSUID(SUID card)
        {
            if (SUIDs.ContainsKey(card.id))
            {
                SUIDs.Remove(card.id);
            }
            SUIDs.Add(card.id, card);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns true if the supplied barcode_id has the "admit" flag set to true.
        /// </summary>
        /// <param name="barcode_id"></param>
        /// <returns></returns>
        public bool ShouldAdmit(string barcode_id)
        {
            if (SUIDs.ContainsKey(barcode_id) && SUIDs[barcode_id].admit)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the supplied barcode_id to reflect admission at DateTime.Now.
        /// </summary>
        /// <param name="barcode_id"></param>
        public void DoAdmit(string barcode_id)
        {
            if (SUIDs.ContainsKey(barcode_id))
            {
                SUIDs[barcode_id].admitted = true;
                SUIDs[barcode_id].setAdmitTime(DateTime.Now);
            }
        }

        /// <summary>
        /// Attempts to admit the person, or returns an appropriate error message. This is the main method
        /// for most applications that don't have special requirements.
        /// 
        /// Also logs the scan, if the scan log is enabled.
        /// </summary>
        /// <param name="barcode_id"></param>
        /// <returns></returns>
        public AdmitInfo TryAdmit(string barcode_id)
        {
            AdmitInfo info = new AdmitInfo();

            if (!ShouldAdmit(barcode_id)) {
                info.status = AdmitStatus.NO;
                info.message = GetMessage(barcode_id);
            }
            else if (ShouldAdmit(barcode_id) && !IsRepeat(barcode_id))
            {
                DoAdmit(barcode_id);
                info.status = AdmitStatus.OKAY;
                info.message = GetMessage(barcode_id);
            }
            else if (ShouldAdmit(barcode_id) && IsRepeat(barcode_id))
            {
                info.status = AdmitStatus.REPEAT;
                info.message = Message.RepeatMessage;
            }

            DoLog(barcode_id, GetMessage(barcode_id));

            return info;
        }

        /// <summary>
        /// Returns true if the ID was scanned already, and if that scan was more than REPEAT_OK_TIME ago.
        /// </summary>
        /// <param name="barcode_id"></param>
        /// <returns></returns>
        public bool IsRepeat(string barcode_id)
        {
            if (SUIDs.ContainsKey(barcode_id) && SUIDs[barcode_id].admitted && SUIDs[barcode_id].admit_time < DateTime.Now.Subtract(REPEAT_OK_TIME))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the message that is appropriate for this ID according to the SUIDs list. Does not
        /// return a repeat message if the ID has already been scanned. Will return Message.InvalidMessage
        /// if the barcode_id is not in the SUIDs list.
        /// </summary>
        /// <param name="barcode_id"></param>
        /// <returns></returns>
        public Message GetMessage(string barcode_id)
        {
            if (!SUIDs.ContainsKey(barcode_id))
            {
                return Message.InvalidMessage;
            }

            short messageId = SUIDs[barcode_id].messageId;

            if (!Messages.ContainsKey(messageId))
            {
                return Message.UnknownMessage;
            }

            Message message = Messages[SUIDs[barcode_id].messageId];
            return message;
        }
        #endregion

        protected void DoLog(string barcode_id, Message m)
        {
            log.Log(barcode_id, m);
        }
    }

    struct AdmitInfo
    {
        public AdmitStatus status;
        public Message message;
    }
}
