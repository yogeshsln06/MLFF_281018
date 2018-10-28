using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Classes.VMS
{
    public class VMSController
    {
        VMSBase vmsBase;

        public VMSController()
        {
            // Get from configuration
            LogMessage("Using TeleHouseVMS for sending message.");
            vmsBase = new TeleHouseVMS();
        }

        public bool SendMessage(VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection tollRates)
        {
            return vmsBase.SendTollRateMessage(tollRates);
        }

        private void LogMessage(string message)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(message, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.VMS);
        }
    }
}
