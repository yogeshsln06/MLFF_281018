using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class SMSCommunicationHistoryCBE
    {
        private Int32 entryId;
        private Int32 tmsId;
        private Int32 customerAccountId;
        private Int32 customerVehicleId;
        private String customerName;
        private String mobileNumber;
        private Int32 messageDirection;//INCOMING=1,OUTGOING=2
        private String messageBody;
        private Int32 sentStatus;//UNKNOWN=0,UNSENT=1,SENT=2,IGNORED=3
        private Int32 receivedProcessStatus;//UNKNOWN=0,UNPROCESSED=1,PROCESSED=2
        private DateTime messageSendDateTime;
        private DateTime messageReceiveTime;
        private Int32 messageDeliveryStatus;//UNKNOWN=0,DELIVERED=1,UNDELIVERED=2
        private Int32 attemptCount;
        private Int32 operatorAttemptCount;
        private DateTime creationDate;
        private DateTime modificationDate;
        private Int32 modifiedBy;
        private string transactionId;
        private string gatewayResponse;
        private Int32 responseCode;
        private Int32 operatorResponseCode;
        private Int32 accountHistoryId;
        private Int32 transactionType;
        private string subject;
        private string emailid;
        private string smsTransactionId;
        private string referenceNo;
        private String vehicleRCNumber;

        public SMSCommunicationHistoryCBE()
        {
            this.entryId = 0;
            this.tmsId = 0;
            this.customerAccountId = 0;
            this.customerVehicleId = 0;
            this.customerName = String.Empty;
            this.mobileNumber = String.Empty;
            this.messageDirection = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDirection.Incoming;//INCOMING=1,OUTGOING=2
            this.messageBody = String.Empty;
            this.sentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent;//UNSENT=1,SENT=2,IGNORED=3
            this.receivedProcessStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSReceivedMessageProcessStatus.UnProcessed;//UNPROCESSED=1,PROCESSED=2
            this.messageSendDateTime = DateTime.Now;
            this.messageReceiveTime = DateTime.Now;
            this.messageDeliveryStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDeliveryStatus.UnDelivered; //DELIVERED=1,UNDELIVERED=2
            this.attemptCount = 0;
            this.operatorAttemptCount = 0;
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
            this.modifiedBy = 0;
            this.responseCode = 0;
            this.operatorResponseCode = 0;
            this.accountHistoryId = 0;
            this.transactionType = 0;
            this.subject = string.Empty;
            this.emailid = string.Empty;
            this.smsTransactionId = string.Empty;
            this.referenceNo = string.Empty;
            this.vehicleRCNumber = string.Empty;
        }
        public Int32 EntryId
        {
            get { return this.entryId; }
            set { this.entryId = value; }
        }

        public Int32 TmsId
        {
            get { return this.tmsId; }
            set { this.tmsId = value; }
        }

        public Int32 CustomerAccountId
        {
            get { return this.customerAccountId; }
            set { this.customerAccountId = value; }
        }
        public Int32 CustomerVehicleId
        {
            get { return this.customerVehicleId; }
            set { this.customerVehicleId = value; }
        }
        public String CustomerName
        {
            get { return this.customerName; }
            set { this.customerName = value; }
        }

        public string MobileNumber
        {
            get { return this.mobileNumber; }
            set { this.mobileNumber = value; }
        }

        public Int32 MessageDirection
        {
            get { return this.messageDirection; }
            set { this.messageDirection = value; }
        }

        public String MessageBody
        {
            get { return this.messageBody; }
            set { this.messageBody = value; }
        }

        public Int32 SentStatus
        {
            get { return this.sentStatus; }
            set { this.sentStatus = value; }
        }

        public Int32 ReceivedProcessStatus
        {
            get { return this.receivedProcessStatus; }
            set { this.receivedProcessStatus = value; }
        }

        public DateTime MessageSendDateTime
        {
            get { return this.messageSendDateTime; }
            set { this.messageSendDateTime = value; }
        }

        public DateTime MessageReceiveTime
        {
            get { return this.messageReceiveTime; }
            set { this.messageReceiveTime = value; }
        }

        public Int32 MessageDeliveryStatus
        {
            get { return this.messageDeliveryStatus; }
            set { this.messageDeliveryStatus = value; }
        }

        public Int32 AttemptCount
        {
            get { return this.attemptCount; }
            set { this.attemptCount = value; }
        }

        public Int32 OperatorAttemptCount
        {
            get { return this.operatorAttemptCount; }
            set { this.operatorAttemptCount = value; }
        }

        public DateTime CreationDate
        {
            get { return this.creationDate; }
            set { this.creationDate = value; }
        }

        public DateTime ModificationDate
        {
            get { return this.modificationDate; }
            set { this.modificationDate = value; }
        }

        public Int32 ModifiedBy
        {
            get { return this.modifiedBy; }
            set { this.modifiedBy = value; }
        }

        public string TransactionId
        {
            get { return this.transactionId; }
            set { this.transactionId = value; }
        }

        public string GatewayResponse
        {
            get { return this.gatewayResponse; }
            set { this.gatewayResponse = value; }
        }

        public Int32 ResponseCode
        {
            get { return this.responseCode; }
            set { this.responseCode = value; }
        }

        public Int32 OperatorResponseCode
        {
            get { return this.operatorResponseCode; }
            set { this.operatorResponseCode = value; }
        }

        public Int32 AccountHistoryId
        {
            get { return this.accountHistoryId; }
            set { this.accountHistoryId = value; }
        }

        public Int32 TransactionType
        {
            get { return this.transactionType; }
            set { this.transactionType = value; }
        }

        public string Subject
        {
            get { return this.subject; }
            set { this.subject = value; }
        }

        public string EmailId
        {
            get { return this.emailid; }
            set { this.emailid = value; }
        }

        public string SMSTransactionId
        {
            get { return this.smsTransactionId; }
            set { this.smsTransactionId = value; }
        }

        public string ReferenceNo
        {
            get { return this.referenceNo; }
            set { this.referenceNo = value; }
        }

        public String VehicleRCNumber
        {
            get
            {
                return this.vehicleRCNumber;
            }

            set
            {
                this.vehicleRCNumber = value;
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("entryId: " + this.entryId + Environment.NewLine);
            sb.Append("tmsId: " + this.tmsId + Environment.NewLine);
            sb.Append("customerAccountId: " + this.customerAccountId + Environment.NewLine);
            sb.Append("customerVehicleId: " + this.customerVehicleId + Environment.NewLine);
            sb.Append("customerName: " + this.customerName + Environment.NewLine);
            sb.Append("mobileNumber: " + this.mobileNumber + Environment.NewLine);
            sb.Append("messageDirection: " + this.messageDirection + Environment.NewLine);
            sb.Append("messageBody: " + this.messageBody + Environment.NewLine);
            sb.Append("sentStatus: " + this.sentStatus + Environment.NewLine);
            sb.Append("receivedProcessStatus: " + this.receivedProcessStatus + Environment.NewLine);
            sb.Append("messageSendDateTime: " + this.messageSendDateTime + Environment.NewLine);
            sb.Append("messageReceiveTime: " + this.messageReceiveTime + Environment.NewLine);
            sb.Append("messageDeliveryStatus: " + this.messageDeliveryStatus + Environment.NewLine);
            sb.Append("attemptCount: " + this.attemptCount + Environment.NewLine);
            sb.Append("creationDate: " + this.creationDate + Environment.NewLine);
            sb.Append("modificationDate: " + this.modificationDate + Environment.NewLine);
            sb.Append("modifiedBy: " + this.modifiedBy + Environment.NewLine);
            sb.Append("modifiedBy: " + this.modifiedBy + Environment.NewLine);
            sb.Append("accountHistoryId: " + this.accountHistoryId + Environment.NewLine);
            return sb.ToString();
        }

    }
    public class SMSCommunicationHistoryCollection : CollectionBase
    {
        public SMSCommunicationHistoryCollection()
        {
        }
        public SMSCommunicationHistoryCBE this[int index]
        {
            get { return (SMSCommunicationHistoryCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(SMSCommunicationHistoryCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(SMSCommunicationHistoryCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, SMSCommunicationHistoryCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(SMSCommunicationHistoryCBE value)
        {
            List.Remove(value);
        }
    }
}
