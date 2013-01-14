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
        public bool EnableScanLog = false;

        protected SUIDs SUIDs = new SUIDs();
        protected Messages Messages = new Messages();

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

        public bool ShouldAdmit(string barcode_id)
        {
            if (SUIDs.ContainsKey(barcode_id) && SUIDs[barcode_id].admit)
            {
                return true;
            }
            return false;
        }

        public void DoAdmit(string barcode_id)
        {
            if (SUIDs.ContainsKey(barcode_id))
            {
                SUIDs[barcode_id].admitted = true;
                SUIDs[barcode_id].setAdmitTime(DateTime.Now);
            }

            LogAdmit(barcode_id);
        }

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
            return info;
        }

        public bool IsRepeat(string barcode_id)
        {
            if (SUIDs.ContainsKey(barcode_id) && SUIDs[barcode_id].admit_time < DateTime.Now.Subtract(new TimeSpan(0, 0, 30)))
            {
                return true;
            }
            return false;
        }

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

        protected void LogAdmit(string barcode_id)
        {
            // nothing yet
        }
    }

    struct AdmitInfo
    {
        public AdmitStatus status;
        public Message message;
    }
}
