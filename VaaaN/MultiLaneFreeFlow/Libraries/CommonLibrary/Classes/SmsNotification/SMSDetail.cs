using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification
{
    [Serializable]
    public class SMSDetail
    {
        public int AccountId { get; set; }
        public String CustomerName { get; set; }

        public int MessageIndex { get; set; }
        public String Status { get; set; }
        public String SenderMobileNumber { get; set; }
        public DateTime SMSReceiveTime { get; set; }
        public String SMSMessage { get; set; }
        public String RawData { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("AccountId: " + AccountId);
            sb.AppendLine("CustomerName: " + CustomerName);
            sb.AppendLine("MessageIndex: " + MessageIndex);
            sb.AppendLine("Status: " + Status);
            sb.AppendLine("SenderMobileNumber: " + SenderMobileNumber);
            sb.AppendLine("SMSReceiveTime: " + SMSReceiveTime);
            sb.AppendLine("SMSBody: " + SMSMessage);
            sb.AppendLine("RawData: " + RawData);

            return sb.ToString();
        }
    }
}
