using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class CustomerSmsNotificationCBE
    {
        private Int32 entryId;
   
     
        private Int32 accountId;
        private String contactNumber;
        private String emailId;
        private String messageCategory;
        private String mediaType;
        private String message;
        private String moduleName;
        private String sentStatus;
        private String serialNo;
        private String deliveryStatus;
        private Int32 attemptCount;
        private String customerName;
        private DateTime creationDate;
        private DateTime sentDate;
        private DateTime deliveryDate;

        public CustomerSmsNotificationCBE()
        {
            this.entryId = 0;
          
            this.accountId = 0;
            this.attemptCount = 0;
            this.contactNumber = String.Empty;
            this.emailId = String.Empty;
            this.messageCategory = String.Empty;
            this.mediaType = String.Empty;
            this.message = String.Empty;
            this.moduleName = string.Empty;
            this.sentStatus = String.Empty;
            this.serialNo = String.Empty;
            this.deliveryStatus = String.Empty;
            this.customerName = String.Empty;
            this.creationDate = DateTime.Now;
            this.sentDate = DateTime.Now;
            this.deliveryDate = DateTime.Now;
        }
        public Int32 EntryId
        {
            get { return this.entryId; }
            set { this.entryId = value; }
        }
     
        public int AccountId
        {
            get { return this.accountId; }
            set { this.accountId = value; }
        }
        public int AttemptCount
        {
            get { return this.attemptCount; }
            set { this.attemptCount = value; }
        }
        public string ContactNumber
        {
            get { return this.contactNumber; }
            set { this.contactNumber = value; }
        }

        public string EmailId
        {
            get { return this.emailId; }
            set { this.emailId = value; }
        }

        public string MessageCategory
        {
            get { return this.messageCategory; }
            set { this.messageCategory = value; }
        }

        public string MediaType
        {
            get { return this.mediaType; }
            set { this.mediaType = value; }
        }

        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }

        public string ModuleName
        {
            get { return this.moduleName; }
            set { this.moduleName = value; }
        }

        public string SentStatus
        {
            get { return this.sentStatus; }
            set { this.sentStatus = value; }
        }
        public string SerialNo
        {
            get { return this.serialNo; }
            set { this.serialNo = value; }
        }
        public string DeliveryStatus
        {
            get { return this.deliveryStatus; }
            set { this.deliveryStatus = value; }
        }

        public string CustomerName
        {
            get { return this.customerName; }
            set { this.customerName = value; }
        }

        public DateTime CreationDate
        {
            get { return this.creationDate; }
            set { this.creationDate = value; }
        }

        public DateTime SentDate
        {
            get { return this.sentDate; }
            set { this.sentDate = value; }
        }

        public DateTime DeliveryDate
        {
            get { return this.deliveryDate; }
            set { this.deliveryDate = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("entryId = " + this.entryId + Environment.NewLine);
        
            sb.Append("accountId = " + this.accountId + Environment.NewLine);
            sb.Append("attemptCount = " + this.attemptCount + Environment.NewLine);
            sb.Append("contactNumber = " + this.contactNumber + Environment.NewLine);
            sb.Append("emailId = " + this.emailId + Environment.NewLine);
            sb.Append("messageCategory = " + this.messageCategory + Environment.NewLine);
            sb.Append("mediaType = " + this.mediaType + Environment.NewLine);
            sb.Append("message = " + this.message + Environment.NewLine);
            sb.Append("moduleName = " + this.moduleName + Environment.NewLine);
            sb.Append("sentStatus = " + this.sentStatus + Environment.NewLine);
            sb.Append("deliveryStatus = " + this.deliveryStatus + Environment.NewLine);
            sb.Append("customerName = " + this.customerName + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("sentDate = " + this.sentDate + Environment.NewLine);
            sb.Append("serialNo = " + this.serialNo + Environment.NewLine);
            sb.Append("deliveryDate = " + this.deliveryDate + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class CustomerSmsNotificationCollection : CollectionBase
    {
        public CustomerSmsNotificationCollection()
        {
        }
        public CustomerSmsNotificationCBE this[int index]
        {
            get { return (CustomerSmsNotificationCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(CustomerSmsNotificationCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(CustomerSmsNotificationCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, CustomerSmsNotificationCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(CustomerSmsNotificationCBE value)
        {
            List.Remove(value);
        }
    }


}
