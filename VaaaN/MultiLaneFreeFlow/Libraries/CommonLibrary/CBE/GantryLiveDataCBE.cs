using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    public class GantryLiveDataCBE
    {
        int gantryId;
        DateTime counterStartTime;
        int motorCycleCount = 0;
        int smallCount = 0;
        int mediumCount = 0;
        int bigCount = 0;
         
        public Int32 GantryId
        {
            get
            {
                return this.gantryId;
            }
            set
            {
                this.gantryId = value;
            }
        }

        public DateTime CounterStartTime
        {
            get
            {
                return this.counterStartTime;
            }
            set
            {
                this.counterStartTime = value;
            }
        }

        public Int32 MotorCycleCount
        {
            get
            {
                return this.motorCycleCount;
            }
            set
            {
                this.motorCycleCount = value;
            }
        }

        public Int32 SmallCount
        {
            get
            {
                return this.smallCount;
            }
            set
            {
                this.smallCount = value;
            }
        }

        public Int32 MediumCount
        {
            get
            {
                return this.mediumCount;
            }
            set
            {
                this.mediumCount = value;
            }
        }

        public Int32 BigCount
        {
            get
            {
                return this.bigCount;
            }
            set
            {
                this.bigCount = value;
            }
        }

        public class GantryLiveDataCBECollection : CollectionBase
        {
            public GantryLiveDataCBECollection()
            {
            }
            public GantryLiveDataCBE this[int index]
            {
                get { return (GantryLiveDataCBE)List[index]; }
                set { List[index] = value; }
            }
            public int Add(GantryLiveDataCBE value)
            {
                return (List.Add(value));
            }
            public int IndexOf(GantryLiveDataCBE value)
            {
                return (List.IndexOf(value));
            }
            public void Insert(int index, GantryLiveDataCBE value)
            {
                List.Insert(index, value);
            }
            public void Remove(GantryLiveDataCBE value)
            {
                List.Remove(value);
            }
        }
    }
}
