using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class CustomerQueueCBE
    {
        private Int32 tmsId;
        private Int32 customerQueueId;
        private String firstName;
        private String lastName;
        private String mobNumber;
        private String emailId;
        private String description;
        private String address;
        private Int32 status;
        private String appointmentLocation;
        private DateTime appointmentDate;
        private String customerImagePath;
        private String scannedDocsPath1;
        private String scannedDocsPath2;
        private String scannedDocsPath3;
        private String scannedDocsPath4;
        private String vehicleImagePathFront;
        private String vehicleImagePathRear;
        private Int32 isDocVerified;
        private Int32 modifierId;
        private DateTime creationDate;
        private DateTime modificationDate;
        public CustomerQueueCBE()
        {
            this.tmsId = 0;
            this.customerQueueId = 0;
            this.firstName = String.Empty;
            this.lastName = String.Empty;
            this.mobNumber = String.Empty;
            this.emailId = String.Empty;
            this.description = String.Empty;
            this.address = String.Empty;
            this.status = 0;
            this.appointmentLocation = String.Empty;
            this.appointmentDate = DateTime.Now;
            this.customerImagePath = String.Empty;
            this.scannedDocsPath1 = String.Empty;
            this.scannedDocsPath2 = String.Empty;
            this.scannedDocsPath3 = String.Empty;
            this.scannedDocsPath4 = String.Empty;
            this.vehicleImagePathFront = String.Empty;
            this.vehicleImagePathRear = String.Empty;
            this.isDocVerified = 0;
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

        public int CustomerQueueId
        {
            get
            {
                return this.customerQueueId;
            }

            set
            {
                this.customerQueueId = value;
            }
        }

        public string FirstName
        {
            get
            {
                return this.firstName;
            }

            set
            {
                this.firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }

            set
            {
                this.lastName = value;
            }
        }

        public string MobNumber
        {
            get
            {
                return this.mobNumber;
            }

            set
            {
                this.mobNumber = value;
            }
        }


        public string EmailId
        {
            get
            {
                return this.emailId;
            }

            set
            {
                this.emailId = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value;
            }
        }

        public string Address
        {
            get
            {
                return this.address;
            }

            set
            {
                this.address = value;
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

        public DateTime AppointmentDate
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

        public string CustomerImagePath
        {
            get
            {
                return this.customerImagePath;
            }

            set
            {
                this.customerImagePath = value;
            }
        }

        public string ScannedDocsPath1
        {
            get
            {
                return this.scannedDocsPath1;
            }

            set
            {
                this.scannedDocsPath1 = value;
            }
        }

        public string ScannedDocsPath2
        {
            get
            {
                return this.scannedDocsPath2;
            }

            set
            {
                this.scannedDocsPath2 = value;
            }
        }

        public string ScannedDocsPath3
        {
            get
            {
                return this.scannedDocsPath3;
            }

            set
            {
                this.scannedDocsPath3 = value;
            }
        }

        public string ScannedDocsPath4
        {
            get
            {
                return this.scannedDocsPath4;
            }

            set
            {
                this.scannedDocsPath4 = value;
            }
        }

        public string VehicleImagePathFront
        {
            get
            {
                return this.vehicleImagePathFront;
            }

            set
            {
                this.vehicleImagePathFront = value;
            }
        }

        public string VehicleImagePathRear
        {
            get
            {
                return this.vehicleImagePathRear;
            }

            set
            {
                this.vehicleImagePathRear = value;
            }
        }

        public int IsDocVerified
        {
            get
            {
                return this.isDocVerified;
            }

            set
            {
                this.isDocVerified = value;
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

    public class CustomerQueueCollection : CollectionBase
    {
        public CustomerQueueCollection()
        {
        }
        public CustomerQueueCBE this[int index]
        {
            get { return (CustomerQueueCBE)List[index]; }
            set { List[index] = value; }
        }
        public int Add(CustomerQueueCBE value)
        {
            return (List.Add(value));
        }
        public int IndexOf(CustomerQueueCBE value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, CustomerQueueCBE value)
        {
            List.Insert(index, value);
        }
        public void Remove(CustomerQueueCBE value)
        {
            List.Remove(value);
        }
    }
}
