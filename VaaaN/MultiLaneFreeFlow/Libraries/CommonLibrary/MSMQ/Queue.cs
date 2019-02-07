using System;
using System.Messaging;

namespace VaaaN.MLFF.Libraries.CommonLibrary.MSMQ
{
    public class Queue
    {
        public static string inBoxQueueName = ".\\Private$\\inBoxQueue";
        //public static string inBoxQueueNameIKE = ".\\Private$\\inBoxQueueIKE";
        //public static string inBoxQueueNameANPR = ".\\Private$\\inBoxQueueANPR";

        public static string failedQueueName = ".\\Private$\\failedQueue";
        //public static string failedQueueNameIKE = ".\\Private$\\failedQueueIKE";
        //public static string failedQueueNameANPR = ".\\Private$\\failedQueueANPR";

        public static string eventQueue = ".\\Private$\\eventQueue";
        public static TimeSpan eventQueueTimeOut = new TimeSpan(0, 0, 0, 2);

        public static string localEventQueue = ".\\Private$\\localEventQueue";
        public static TimeSpan localEventQueueTimeOut = new TimeSpan(1, 0, 10);

        public static string hardwareStatusEventQueue = ".\\Private$\\hardwareStatusEventQueue";
        public static TimeSpan hardwareStatusEventQueueTimeOut = new TimeSpan(1, 0, 10);

        public static string vmsMessageQueue = ".\\Private$\\vmsMessageQueue";
        public static TimeSpan vmsMessageQueueTimeOut = new TimeSpan(0, 0, 10);

        public static string smsMessageQueue = ".\\Private$\\smsMessageQueue";
        public static TimeSpan smsMessageQueueTimeOut = new TimeSpan(0, 0, 10);

        public static string webDashboardMessageQueue = ".\\Private$\\webDashboardStatusQueue";

        public static MessageQueue Create(string queueName)
        {
            MessageQueue mq;

            //Check if MS Message Queue is installed or not

            if (!MessageQueue.Exists(queueName))
            {
                mq = MessageQueue.Create(queueName);
                mq.DefaultPropertiesToSend.Recoverable = true;
                mq.Formatter = new BinaryMessageFormatter();
                mq.ResetPermissions();
                mq.SetPermissions("Administrator", MessageQueueAccessRights.FullControl);
                mq.SetPermissions("EveryOne", MessageQueueAccessRights.FullControl);
            }
            else
            {
                mq = new MessageQueue(queueName);
                mq.Formatter = new BinaryMessageFormatter();
            }

            return mq;
        }
    }
}
