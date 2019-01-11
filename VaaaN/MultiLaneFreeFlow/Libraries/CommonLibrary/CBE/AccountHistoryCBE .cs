using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Web;


namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class AccountHistoryCBE
    {
        private Int32 tmsId;
        private Int32 entryId;
        private Int32 accountId;
        private Int32 cutomerVehicleEntryId;
        private Int32 transationType;
        private Int64 transactionId;
        private Decimal amount;
        private Int32 isSMSSent;
        private Int32 isEmailSent;
        private Int32 modifierId;
        private DateTime creationDate;
        private DateTime modificationDate;
        private Int32 transferStatus;
        private Decimal openingBalance;
        private Decimal closingBalance;


        public AccountHistoryCBE()
        {
            this.tmsId = 0;
            this.accountId = 0;
            this.entryId = 0;
            this.cutomerVehicleEntryId = 0;
            this.transationType = 0;
            this.transactionId = 0;
            this.amount = 0;
            this.isSMSSent = 0;
            this.isEmailSent = 0;
            this.modifierId = 0;
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
            this.transferStatus = 0;
            this.openingBalance = 0;
            this.closingBalance = 0;
        }

        public Int32 TMSId
        {
            get { return this.tmsId; }
            set { this.tmsId = value; }
        }
        public Int32 AccountId
        {
            get
            {
                return this.accountId;
            }

            set
            {
                this.accountId = value;
            }
        }

        public Int32 EntryId
        {
            get { return this.entryId; }
            set { this.entryId = value; }
        }

        public Int32 CustomerVehicleEntryId
        {
            get { return this.cutomerVehicleEntryId; }
            set { this.cutomerVehicleEntryId = value; }
        }
        public Int32 TransactionTypeId
        {
            get { return this.transationType; }
            set { this.transationType = value; }
        }
        public Int64 TransactionId
        {
            get { return this.transactionId; }
            set { this.transactionId = value; }
        }

        public Decimal Amount
        {
            get { return this.amount; }
            set { this.amount = value; }
        }

        public Int32 IsSMSSent
        {
            get { return this.isSMSSent; }
            set { this.isSMSSent = value; }
        }

        public Int32 IsEmailSent
        {
            get { return this.isEmailSent; }
            set { this.isEmailSent = value; }
        }

        public Int32 ModifierId
        {
            get { return this.modifierId; }
            set { this.modifierId = value; }
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
        public Int32 TransferStatus
        {
            get { return this.transferStatus; }
            set { this.transferStatus = value; }
        }

        public Decimal OpeningBalance
        {
            get { return this.openingBalance; }
            set { this.openingBalance = value; }
        }

        public Decimal ClosingBalance
        {
            get { return this.closingBalance; }
            set { this.closingBalance = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("tmsId = " + this.tmsId + Environment.NewLine);
            sb.Append("entryId = " + this.entryId + Environment.NewLine);
            sb.Append("transactionId = " + this.transactionId + Environment.NewLine);
            sb.Append("amount = " + this.amount + Environment.NewLine);
            sb.Append("isSMSSent = " + this.isSMSSent + Environment.NewLine);
            sb.Append("isEmailSent = " + this.isEmailSent + Environment.NewLine);
            sb.Append("modifierId = " + this.modifierId + Environment.NewLine);
            sb.Append("creationDate = " + this.creationDate + Environment.NewLine);
            sb.Append("modificationDate = " + this.modificationDate + Environment.NewLine);
            sb.Append("transferStatus = " + this.transferStatus + Environment.NewLine);

            return sb.ToString();
        }
    }

    public class AccountHistoryCollection : CollectionBase
    {
        public AccountHistoryCollection()
        {
        }
        public AccountHistoryCBE this[int index]
        {
            get { return (AccountHistoryCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(AccountHistoryCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(AccountHistoryCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, AccountHistoryCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(AccountHistoryCBE value)
        {
            List.Remove(value);
        }
    }
}
