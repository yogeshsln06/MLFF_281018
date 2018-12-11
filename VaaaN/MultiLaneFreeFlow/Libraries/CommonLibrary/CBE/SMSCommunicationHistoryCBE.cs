﻿using System;
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
        private DateTime creationDate;
        private DateTime modificationDate;
        private Int32 modifiedBy;

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
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
            this.modifiedBy = 0;
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("entryId: " + this.entryId + Environment.NewLine);
            sb.Append("tmsId: " + this.tmsId + Environment.NewLine);
            sb.Append("customerAccountId: " + this.customerAccountId + Environment.NewLine);
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
