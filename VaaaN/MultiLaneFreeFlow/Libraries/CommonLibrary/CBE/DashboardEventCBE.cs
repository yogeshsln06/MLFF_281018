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
            public ChargedFrontIKEDataCount ChargedClass { get; set; }
           

        }

        [Serializable]
        public class RegisterVehcileCount
        {
            public RegFrontIKEDataCount FrontIKE { get; set; }
            public RegRearIKEDataCount RearIKE { get; set; }
            public RegFrontANPRDataCount FrontANPR { get; set; }
            public RegRearANPRDataCount RearANPR { get; set; }

        }

        [Serializable]
        public class UnRegisterVehcileCount
        {

            public UnRegFrontANPRDataCount FrontANPR { get; set; }
            public UnRegRearANPRDataCount RearANPR { get; set; }

        }

        [Serializable]
        public class UnIdentifiedVehcileCount
        {

            public UnIdentFrontANPRDataCount FrontANPR { get; set; }
            public UnIdentRearANPRDataCount RearANPR { get; set; }

        }

        [Serializable]
        public class ChargedFrontIKEDataCount
        {
            private Int32 moterCycleCount;
            private Int32 smallCount;
            private Int32 mediumCount;
            private Int32 bigCount;

            public ChargedFrontIKEDataCount()
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
        public class RegFrontIKEDataCount
        {
            private Int32 moterCycleCount;
            private Int32 smallCount;
            private Int32 mediumCount;
            private Int32 bigCount;

            public RegFrontIKEDataCount()
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

        //[Serializable]
        //public class FrontIKEDataCount
        //{
        //    private Int32 moterCycleCount;
        //    private Int32 smallCount;
        //    private Int32 mediumCount;
        //    private Int32 bigCount;

        //    public FrontIKEDataCount()
        //    {
        //        this.moterCycleCount = 0;
        //        this.smallCount = 0;
        //        this.mediumCount = 0;
        //        this.bigCount = 0;

        //    }

        //    public Int32 MoterCycleCount
        //    {
        //        get { return this.moterCycleCount; }
        //        set { this.moterCycleCount = value; }
        //    }
        //    public Int32 SmallCount
        //    {
        //        get { return this.smallCount; }
        //        set { this.smallCount = value; }
        //    }
        //    public Int32 MediumCount
        //    {
        //        get { return this.mediumCount; }
        //        set { this.mediumCount = value; }
        //    }
        //    public Int32 BigCount
        //    {
        //        get { return this.bigCount; }
        //        set { this.bigCount = value; }
        //    }

        //}

        [Serializable]
        public class RegRearIKEDataCount
        {

            private Int32 moterCycleCount;
            private Int32 smallCount;
            private Int32 mediumCount;
            private Int32 bigCount;

            public RegRearIKEDataCount()
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

        //[Serializable]
        //public class RearIKEDataCount
        //{

        //    private Int32 moterCycleCount;
        //    private Int32 smallCount;
        //    private Int32 mediumCount;
        //    private Int32 bigCount;

        //    public RearIKEDataCount()
        //    {
        //        this.moterCycleCount = 0;
        //        this.smallCount = 0;
        //        this.mediumCount = 0;
        //        this.bigCount = 0;

        //    }

        //    public Int32 MoterCycleCount
        //    {
        //        get { return this.moterCycleCount; }
        //        set { this.moterCycleCount = value; }
        //    }
        //    public Int32 SmallCount
        //    {
        //        get { return this.smallCount; }
        //        set { this.smallCount = value; }
        //    }
        //    public Int32 MediumCount
        //    {
        //        get { return this.mediumCount; }
        //        set { this.mediumCount = value; }
        //    }
        //    public Int32 BigCount
        //    {
        //        get { return this.bigCount; }
        //        set { this.bigCount = value; }
        //    }

        //}

        [Serializable]
        public class RegFrontANPRDataCount
        {

            private Int32 moterCycleCount;
            private Int32 smallCount;
            private Int32 mediumCount;
            private Int32 bigCount;

            public RegFrontANPRDataCount()
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
        public class UnRegFrontANPRDataCount
        {

            private Int32 moterCycleCount;
            private Int32 smallCount;
            private Int32 mediumCount;
            private Int32 bigCount;

            public UnRegFrontANPRDataCount()
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
        public class UnIdentFrontANPRDataCount
        {

            private Int32 moterCycleCount;
            private Int32 smallCount;
            private Int32 mediumCount;
            private Int32 bigCount;

            public UnIdentFrontANPRDataCount()
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
        //[Serializable]
        //public class FrontANPRDataCount
        //{

        //    private Int32 moterCycleCount;
        //    private Int32 smallCount;
        //    private Int32 mediumCount;
        //    private Int32 bigCount;

        //    public FrontANPRDataCount()
        //    {
        //        this.moterCycleCount = 0;
        //        this.smallCount = 0;
        //        this.mediumCount = 0;
        //        this.bigCount = 0;

        //    }

        //    public Int32 MoterCycleCount
        //    {
        //        get { return this.moterCycleCount; }
        //        set { this.moterCycleCount = value; }
        //    }
        //    public Int32 SmallCount
        //    {
        //        get { return this.smallCount; }
        //        set { this.smallCount = value; }
        //    }
        //    public Int32 MediumCount
        //    {
        //        get { return this.mediumCount; }
        //        set { this.mediumCount = value; }
        //    }
        //    public Int32 BigCount
        //    {
        //        get { return this.bigCount; }
        //        set { this.bigCount = value; }
        //    }
        //}


        //[Serializable]
        //public class RearANPRDataCount
        //{
        //    private Int32 moterCycleCount;
        //    private Int32 smallCount;
        //    private Int32 mediumCount;
        //    private Int32 bigCount;

        //    public RearANPRDataCount()
        //    {
        //        this.moterCycleCount = 0;
        //        this.smallCount = 0;
        //        this.mediumCount = 0;
        //        this.bigCount = 0;

        //    }

        //    public Int32 MoterCycleCount
        //    {
        //        get { return this.moterCycleCount; }
        //        set { this.moterCycleCount = value; }
        //    }
        //    public Int32 SmallCount
        //    {
        //        get { return this.smallCount; }
        //        set { this.smallCount = value; }
        //    }
        //    public Int32 MediumCount
        //    {
        //        get { return this.mediumCount; }
        //        set { this.mediumCount = value; }
        //    }
        //    public Int32 BigCount
        //    {
        //        get { return this.bigCount; }
        //        set { this.bigCount = value; }
        //    }


        //}

        [Serializable]
        public class RegRearANPRDataCount
        {
            private Int32 moterCycleCount;
            private Int32 smallCount;
            private Int32 mediumCount;
            private Int32 bigCount;

            public RegRearANPRDataCount()
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
        public class UnRegRearANPRDataCount
        {
            private Int32 moterCycleCount;
            private Int32 smallCount;
            private Int32 mediumCount;
            private Int32 bigCount;

            public UnRegRearANPRDataCount()
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
        public class UnIdentRearANPRDataCount
        {
            private Int32 moterCycleCount;
            private Int32 smallCount;
            private Int32 mediumCount;
            private Int32 bigCount;

            public UnIdentRearANPRDataCount()
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
