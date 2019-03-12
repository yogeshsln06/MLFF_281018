using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;
using VaaaN.MLFF.Libraries.CommonLibrary.MSMQ;

namespace MLFFWebUI.Models
{
    public class HelperAsync
    {
        static System.Messaging.MessageQueue dashQueue;
        public string StartProcessing(CancellationToken cancellationToken, string strstarttime)
        {
            string result = "";
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string Det = JsonConvert.SerializeObject(TransactionBLL.StackChartData(strstarttime, strstarttime), Formatting.Indented);
                if (!string.IsNullOrEmpty(Det))
                {
                    #region Send data to MSMQ
                    Message m = new Message();
                    m.Formatter = new BinaryMessageFormatter();
                    m.Body = Det;
                    m.Recoverable = true;
                    dashQueue = Queue.Create(Queue.webChartDashboardMessageQueue);
                    if (dashQueue != null)
                        dashQueue.Purge();
                        dashQueue.Send(m);
                    #endregion
                }
                //Thread.Sleep(50000);
            }
            catch (Exception)
            {
                ProcessCancellation();
            }
            return result;
        }
        private void ProcessCancellation()
        {
            Thread.Sleep(10000);
        }
    }
}