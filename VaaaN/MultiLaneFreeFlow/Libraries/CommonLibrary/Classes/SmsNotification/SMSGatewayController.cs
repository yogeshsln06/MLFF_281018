using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification
{
    public class SMSGatewayController
    {
        SMSGatewayBase smsGatewayBase;

        public SMSGatewayController()
        {
            // Get from configuration
            
            smsGatewayBase = new MobisuiteSMSGateway();
           
            //smsGatewayBase = new GoSMSGateway();
        }

        public SMSCommunicationHistoryCBE SendSMS(SMSCommunicationHistoryCBE sms)
        {
            return smsGatewayBase.SendSMS(sms);
        }

        public List<SMSDetail> ReadSMS()
        {
            return smsGatewayBase.ReadSMS();
        }

        private void LogMessage(string message)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(message, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.InboundSMS);
        }
    }
}
