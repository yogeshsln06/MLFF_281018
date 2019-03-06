using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VaaaN.MLFF.Libraries.CommonLibrary.CBE
{
    [Serializable]
    public class DashboardEventCBE
    {
        #region Property
        [Serializable]
        public class DashBoardDataCount
        {
            public Int32 TotalRegisterVehicleCount { get; set; }
            public string EventDate { get; set; }

            public RegisterChargedVehcileCount RegisterCharged { get; set; }

            public RegisterVehcileCount Register { get; set; }

            public UnRegisterVehcileCount UnRegister { get; set; }

            public UnIdentifiedVehcileCount UnIdentified { get; set; }

        }
       

        [Serializable]
        public class RegisterChargedVehcileCount
        {
            public FrontIKEDataCount ChargedClass { get; set; }
           

        }

        [Serializable]
        public class RegisterVehcileCount
        {
            public FrontIKEDataCount FrontIKE { get; set; }
            public RearIKEDataCount RearIKE { get; set; }
            public FrontANPRDataCount FrontANPR { get; set; }
            public RearANPRDataCount RearANPR { get; set; }

        }

        [Serializable]
        public class UnRegisterVehcileCount
        {

            public FrontANPRDataCount FrontANPR { get; set; }
            public RearANPRDataCount RearANPR { get; set; }

        }

        [Serializable]
        public class UnIdentifiedVehcileCount
        {

            public FrontANPRDataCount FrontANPR { get; set; }
            public RearANPRDataCount RearANPR { get; set; }

        }

        [Serializable]
        public class FrontIKEDataCount
        {
            private Int32 moterCycleCount;
            private Int32 smallCount;
            private Int32 mediumCount;
            private Int32 bigCount;

            public FrontIKEDataCount()
            {
                this.moterCycleCount = 0;
                this.smallCount = 0;
                this.mediumCount = 0;
                this.bigCount = 0;

            }

            public Int32 MoterCycleCount
            {
                get { return this.moterCycleCount; }
                set { this.moterCycleCount = value; }
            }
            public Int32 SmallCount
            {
                get { return this.smallCount; }
                set { this.smallCount = value; }
            }
            public Int32 MediumCount
            {
                get { return this.mediumCount; }
                set { this.mediumCount = value; }
            }
            public Int32 BigCount
            {
                get { return this.bigCount; }
                set { this.bigCount = value; }
            }

        }
        [Serializable]
        public class RearIKEDataCount
        {

            private Int32 moterCycleCount;
            private Int32 smallCount;
            private Int32 mediumCount;
            private Int32 bigCount;

            public RearIKEDataCount()
            {
                this.moterCycleCount = 0;
                this.smallCount = 0;
                this.mediumCount = 0;
                this.bigCount = 0;

            }

            public Int32 MoterCycleCount
            {
                get { return this.moterCycleCount; }
                set { this.moterCycleCount = value; }
            }
            public Int32 SmallCount
            {
                get { return this.smallCount; }
                set { this.smallCount = value; }
            }
            public Int32 MediumCount
            {
                get { return this.mediumCount; }
                set { this.mediumCount = value; }
            }
            public Int32 BigCount
            {
                get { return this.bigCount; }
                set { this.bigCount = value; }
            }

        }
        [Serializable]
        public class FrontANPRDataCount
        {

            private Int32 moterCycleCount;
            private Int32 smallCount;
            private Int32 mediumCount;
            private Int32 bigCount;

            public FrontANPRDataCount()
            {
                this.moterCycleCount = 0;
                this.smallCount = 0;
                this.mediumCount = 0;
                this.bigCount = 0;

            }

            public Int32 MoterCycleCount
            {
                get { return this.moterCycleCount; }
                set { this.moterCycleCount = value; }
            }
            public Int32 SmallCount
            {
                get { return this.smallCount; }
                set { this.smallCount = value; }
            }
            public Int32 MediumCount
            {
                get { return this.mediumCount; }
                set { this.mediumCount = value; }
            }
            public Int32 BigCount
            {
                get { return this.bigCount; }
                set { this.bigCount = value; }
            }

            

        }
        [Serializable]
        public class RearANPRDataCount
        {
            private Int32 moterCycleCount;
            private Int32 smallCount;
            private Int32 mediumCount;
            private Int32 bigCount;

            public RearANPRDataCount()
            {
                this.moterCycleCount = 0;
                this.smallCount = 0;
                this.mediumCount = 0;
                this.bigCount = 0;

            }

            public Int32 MoterCycleCount
            {
                get { return this.moterCycleCount; }
                set { this.moterCycleCount = value; }
            }
            public Int32 SmallCount
            {
                get { return this.smallCount; }
                set { this.smallCount = value; }
            }
            public Int32 MediumCount
            {
                get { return this.mediumCount; }
                set { this.mediumCount = value; }
            }
            public Int32 BigCount
            {
                get { return this.bigCount; }
                set { this.bigCount = value; }
            }

           
        }
        #endregion

    }


}
