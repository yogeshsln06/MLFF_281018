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
        public class DashBoardDataCount
        {
            public Int32 TotalRegisterVehicleCount { get; set; }
            public Int32 VehiclePassedCount { get; set; }
            public FrontIKEDataCount FrontIKE { get; set; }
            public RearIKEDataCount RearIKE { get; set; }

            public FrontANPRDataCount FrontANPR { get; set; }
            public RearANPRDataCount RearANPR { get; set; }
        }
        public class FrontIKEDataCount
        {
            public Int32 MoterCycleCount { get; set; }
            public Int32 SmallCount { get; set; }
            public Int32 MediumCount { get; set; }
            public Int32 BigCount { get; set; }

        }
        public class RearIKEDataCount
        {
            public Int32 MoterCycleCount { get; set; }
            public Int32 SmallCount { get; set; }
            public Int32 MediumCount { get; set; }
            public Int32 BigCount { get; set; }

        }
        public class FrontANPRDataCount
        {
            public Int32 MoterCycleCount { get; set; }
            public Int32 SmallCount { get; set; }
            public Int32 MediumCount { get; set; }
            public Int32 BigCount { get; set; }
            public Int32 Register { get; set; }

        }
        public class RearANPRDataCount
        {
            public Int32 MoterCycleCount { get; set; }
            public Int32 SmallCount { get; set; }
            public Int32 MediumCount { get; set; }
            public Int32 BigCount { get; set; }

            public Int32 Register { get; set; }
        }
        #endregion

    }

   
}
