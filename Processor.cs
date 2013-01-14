using System;

using System.Collections.Generic;
using System.Text;

using System.IO;

namespace SU_MT2000_SUIDScanner
{
    class Processor
    {
        public static string eventName = "";
        private const string InputFileName = "\\Application\\0-SUID\\admit-list.dat";
        private const string OutputFileName = "\\Application\\0-SUID\\output-{0:ddd}-{0:HH}-{0:mm}.csv";

        public static SUIDs SUIDs = new SUIDs();
        public static Messages Messages = new Messages();
        public static SUIDs admitted = new SUIDs();

        public bool Load()
        {
            return true;

            try
            {
                StreamReader file = new StreamReader(InputFileName);
                string line;
                short current_mode = -1;

                while ((line = file.ReadLine()) != null)
                {
                    line = line.TrimEnd();

                    if (line[0] == '#')
                    {
                        continue;
                    }
                    else if (line == "!!!MESSAGES")
                    {
                        current_mode = 0;
                        continue;
                    }
                    else if (line == "!!!IDCARDS")
                    {
                        current_mode = 1;
                        continue;
                    }

                    if (current_mode == 0)
                    {
                        AddMessageLine(line);
                    }
                    else if (current_mode == 1)
                    {
                        AddSUIDLine(line);
                    }

                }
                return true;
            } catch (Exception) {
                throw new Exception("Could not load the admit list. Ensure 0-SUID\\admit-list.dat exists.");
            }
        }

        private void AddMessageLine(string line)
        {
            string[] fields = line.Split(',');
            try
            {
                short id = Convert.ToInt16(fields[0]);
                AddMessage(id, fields[1], fields[2]);
            }
            catch (Exception)
            {
                // pass this over
            }
        }

        private void AddMessage(short id, string largeMessage, string smallMessage)
        {
            if (Messages.ContainsKey(id))
            {
                return;
            }
            Messages.Add(id, new Message(id, largeMessage, smallMessage));
        }

        private void AddSUIDLine(string line)
        {
            string[] fields = line.Split(',');
            Boolean over_21 = fields[1].Equals("1");
            Boolean admit = fields[2].Equals("1");

            short message = 0;
            try {
                message = Convert.ToInt16(fields[3]);
            } catch (Exception) {
                message = 0;
            }

            AddSUID(fields[0], (bool)over_21, (bool)admit, Flags.NONE, message);
        }

        private void AddSUID(string barcode_id, bool over_21, bool admit_flag, Flags flags, short messageId) {
            if (SUIDs.ContainsKey(barcode_id)) {
                SUIDs.Remove(barcode_id);
            }
            SUIDs.Add(barcode_id, new SUID(barcode_id, over_21, admit_flag, flags, messageId));
        }

        public bool ShouldAdmit(string barcode_id)
        {
            if (SUIDs.ContainsKey(barcode_id) && SUIDs[barcode_id].admit_flag)
            {
                return true;
            }
            return false;
        }

        public bool Admit(string barcode_id)
        {
            if (!SUIDs.ContainsKey(barcode_id)) {
                AddSUID(barcode_id, false, false, Flags.UNRECOGNIZED,0);
            }

            if (!admitted.ContainsKey(barcode_id))
            {
                admitted.Add(barcode_id, SUIDs[barcode_id]);
                admitted[barcode_id].setAdmitTime(DateTime.Now);
            }
            return true;
        }

        public bool IsRepeat(string barcode_id) {
            if (admitted.ContainsKey(barcode_id) && admitted[barcode_id].admit_time < DateTime.Now.Subtract(new TimeSpan(0, 0, 30)))
            {
                return true;
            }
            return false;
        }

        public string[] GetRepeatMessage()
        {
            return new string[] { "REPEAT", "ID was already scanned" };
        }

        public string[] GetMessages(string barcode_id)
        {
            string[] messages = new string[2];
            if (!SUIDs.ContainsKey(barcode_id))
            {
                messages[0] = "INVALID";
                messages[1] = "Not a current student ID.";
                return messages;
            }
            short messageId = SUIDs[barcode_id].messageId;

            if (!Messages.ContainsKey(messageId))
            {
                messages[0] = "Unrecognized";
                messages[1] = "Unrecognized error.";
                return messages;
            }

            Message message_entry = Messages[SUIDs[barcode_id].messageId];
            messages[0] = message_entry.big_message;
            messages[1] = message_entry.small_message;

            return messages;
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
            return result;
        }

    }
}
