using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification
{
    public abstract class SMSGatewayBase
    {
        public abstract VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE SendSMS(VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE sms);
        public abstract List<VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail> ReadSMS();
    }
}
