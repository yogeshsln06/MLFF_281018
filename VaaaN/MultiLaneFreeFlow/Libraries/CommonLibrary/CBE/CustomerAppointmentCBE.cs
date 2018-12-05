using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class CustomerAppointmentCBE
    {
        
        private Int32 tmsId;
        private Int32 customerAppointmentId;
        private Int32 accountId;
        private String appointmentLocation;
        private String appointmentDate;
        private Int32 appointedbyId;
        private Int32 attendedbyId;
        private Int32 status;
        private Int32 modifierId;
        private DateTime creationDate;
        private DateTime modificationDate;

        public CustomerAppointmentCBE()
        {
            this.tmsId = 1;
            this.customerAppointmentId = 0;
            this.accountId = 0;
            this.appointmentLocation = String.Empty;
            this.appointmentDate = DateTime.Now.ToString();
            this.appointedbyId = 0;
            this.attendedbyId = 0;
            this.status = 0;
            this.modifierId = 0;
            this.creationDate = DateTime.Now;
            this.modificationDate = DateTime.Now;
        }
        public int TmsId
        {
            get
            {
                return this.tmsId;
            }

            set
            {
                this.tmsId = value;
            }
        }

        public int CustomerAppointmentId
        {
            get
            {
                return this.customerAppointmentId;
            }

            set
            {
                this.customerAppointmentId = value;
            }
        }

        public int AccountId
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

        public string AppointmentLocation
        {
            get
            {
                return this.appointmentLocation;
            }

            set
            {
                this.appointmentLocation = value;
            }
        }

        public string AppointmentDate
        {
            get
            {
                return this.appointmentDate;
            }

            set
            {
                this.appointmentDate = value;
            }
        }

        public int AppointedById
        {
            get
            {
                return this.appointedbyId;
            }

            set
            {
                this.appointedbyId = value;
            }
        }

        public int AttendedbyId
        {
            get
            {
                return this.attendedbyId;
            }

            set
            {
                this.attendedbyId = value;
            }
        }

       

        public int Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
            }
        }

        public int ModifierId
        {
            get
            {
                return this.modifierId;
            }

            set
            {
                this.modifierId = value;
            }
        }

        public DateTime CreationDate
        {
            get
            {
                return this.creationDate;
            }

            set
            {
                this.creationDate = value;
            }
        }

        public DateTime ModificationDate
        {
            get
            {
                return this.modificationDate;
            }

            set
            {
                this.modificationDate = value;
            }
        }
    }

    public class CustomerAppointmentCollection : CollectionBase
    {
        public CustomerAppointmentCollection()
        {
        }
        public CustomerAppointmentCBE this[int index]
        {
            get { return (CustomerAppointmentCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(CustomerAppointmentCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(CustomerAppointmentCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, CustomerAppointmentCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(CustomerAppointmentCBE value)
        {
            List.Remove(value);
        }
    }
}
