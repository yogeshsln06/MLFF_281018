using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaaaN.MLFF.Libraries.CommonLibrary
{
    [Serializable]
    public class WebDashboardStatusClass
    {
        #region Variables
        private List<WebDashboardATCCStatus> atccStatusList;
        private List<WebDashboardECBStatus> ecbStatusList;
        #endregion

        #region Constructor
        public WebDashboardStatusClass()
        {
            this.atccStatusList = new List<WebDashboardATCCStatus>();
            this.ecbStatusList = new List<WebDashboardECBStatus>();
        }
        #endregion

        #region Properties
        public List<WebDashboardATCCStatus> AtccStatusList
        {
            get { return atccStatusList; }
            set { atccStatusList = value; }
        }

        public List<WebDashboardECBStatus> EcbStatusList
        {
            get { return ecbStatusList; }
            set { ecbStatusList = value; }
        }
        #endregion
    }

    [Serializable]
    public class WebDashboardATCCStatus
    {
        #region Variables
        Int32 atccId;
        Int32 controlRoomId = 0;
        Int32 deviceCategoryId = 0;
        String ipAddress;
        Int32 currentPingStatus;
        DateTime statusDateTime;
        #endregion

        #region Constructor

        public WebDashboardATCCStatus()
        {
            this.ipAddress = String.Empty;
            this.currentPingStatus = 0;
            this.deviceCategoryId = 5;
            this.atccId = 0;
            this.controlRoomId = 0;
            this.statusDateTime = DateTime.Now;
        }

        #endregion

        #region Properties
        public Int32 AtccId
        {
            get { return atccId; }
            set { atccId = value; }
        }

        public String IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public Int32 CurrentPingStatus
        {
            get { return currentPingStatus; }
            set { currentPingStatus = value; }
        }

        public Int32 ControlRoomId
        {
            get { return controlRoomId; }
            set { controlRoomId = value; }
        }

        public Int32 DeviceCategoryId
        {
            get { return deviceCategoryId; }
        }

        public DateTime StatusDateTime
        {
            get { return statusDateTime; }
            set { statusDateTime = value; }
        }
        #endregion
    }

    [Serializable]
    public class WebDashboardECBStatus
    {
        #region Variables
        Int32 ecbId;
        Int32 controlRoomId = 0;
        Int32 deviceCategoryId = 0;
        String ipAddress;
        Int32 pingStatus;
        DateTime statusDateTime;
        #endregion

        #region Constructor

        public WebDashboardECBStatus()
        {
            this.ipAddress = String.Empty;
            this.pingStatus = 0;
            this.deviceCategoryId = 1;
            this.ecbId = 0;
            this.controlRoomId = 0;
            this.statusDateTime = DateTime.Now;
        }

        #endregion

        #region Properties
        public Int32 EcbId
        {
            get { return ecbId; }
            set { ecbId = value; }
        }

        public String IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public Int32 PingStatus
        {
            get { return pingStatus; }
            set { pingStatus = value; }
        }

        public Int32 ControlRoomId
        {
            get { return controlRoomId; }
            set { controlRoomId = value; }
        }

        public Int32 DeviceCategoryId
        {
            get { return deviceCategoryId; }
        }

        public DateTime StatusDateTime
        {
            get { return statusDateTime; }
            set { statusDateTime = value; }
        }
        #endregion
    }
}
