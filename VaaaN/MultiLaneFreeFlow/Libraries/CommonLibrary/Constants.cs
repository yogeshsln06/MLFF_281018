
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.ServiceProcess;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace VaaaN.MLFF.Libraries.CommonLibrary
{
    public class Constants
    {
        #region Variable
        public static string DATETIME_FORMAT = "dd/MM/yyyy hh:mm:ss tt";
        public static string DATETIME_FORMAT_WITHOUT_SECONDS = "dd/MM/yyyy hh:mm tt";
        public static string dateTimeFormat24H = "yyyy-MM-dd HH:mm:ss.fff";
        public static string dateTimeFormat24HForFileName = "yyyy-MM-dd-HH-mm-ss-fff";
        public static string dateTimeFormat24HsqlServer = "yyyy-MM-dd HH:mm:ss.fff";
        public static string dateTimeFormat24HOracleQuery = "dd/MM/yyyy HH:mm:ss";
        public static char hardwareStatusMessageSeparator = '≠';
        public static string driveLetter = "C";
        public static string atmsConfigDirectory = driveLetter + @":\MLFF\Config\";
        public static string companyLogoPath = driveLetter + @":\Freeflow\images\company_logo.jpg";
        public static string companyLogoPath2 = driveLetter + @":\Freeflow\images\company_logo2.jpg";

        public static string atmsECBCallHistoryBackupProcessedFolder = driveLetter + @":\ATMS\Backup\ECB\Processed\";
        public static string atmsECBCallHistoryBackupUnProcessedFolder = driveLetter + @":\ATMS\Backup\ECB\UnProcessed\";

        public static string vmsMMFName = @"Global\myVmsStatuses";//Memory mapped file name

        //Module Name resource key
        public static String masterModuleNameResourceKey = "CMN_LBL_MasterConfigurationModule";
        public static String dashboardModuleNameResourceKey = "CMN_LBL_DashboardModule";
        public static String ecbOperatorTerminalModuleNameResourceKey = "CMN_LBL_ECBOperatorTerminal";
        public static String atccModuleNameResourceKey = "CMN_LBL_ATCCController";
        public static String metControllerModuleNameResourceKey = "CMN_LBL_METController";
        public static String vidsControllerModuleNameResourceKey = "CMN_LBL_VIDSController";
        public static String vmsControllerModuleNameResourceKey = "CMN_LBL_VMSController";
        public static String reportModuleNameResourceKey = "CMN_LBL_ReportModule";


        //For encryption and decryption
        public static string passPhrase = "Pas5pr@se";        // can be any string
        public static string saltValue = "s@1tValue";        // can be any string
        public static string hashAlgorithm = "SHA1";             // can be "MD5"
        public static int passwordIterations = 2;                  // can be any number
        public static string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
        public static int keySize = 256;                // can be 192 or 128


        public static int CCRGSPortNumber = 8010;//Control Room General Service
        public static int emsPortNumber = 8011;//Event messenger service


        public static char degree = (Char)176;

        public static string oraclePackagePrefix = "MLFF_PACKAGE.";
        public static string oraclePackageReportPrefix = "ATMS_PACKAGE_REPORTS.";


        /// <summary>
        /// Possible transfer Status.
        /// </summary>

       
        public enum ReviewTransactionCategory
        {
            [Display(Name = "--Must Review--")]
            AllTransaction = 0,

            [Display(Name = "ANPR data missing")]
            OnlyEVI = 1,

            [Display(Name = "EVI data missing")]
            OnlyANPR = 2,

            [Display(Name = "EVI & ANPR Vehicle Class Mismatch")]
            EVI_ANPRClassMisMatch = 3,

            [Display(Name = "Reviewed Transaction")]
            Audited = 4,

            //[Display(Name = "EVI & ANPR data missmatch")]
            //EVI_ANPRMissMatch = 3,
            //[Display(Name = "OCR Missing")]
            //OCRMissing = 5,
            //[Display(Name = "Un- Transaction")]
            //Unaudited = 7,

        }

        public enum ManualReviewTransactionCategory
        {
            [Display(Name = "--Must Review--")]
            AllTransaction = 0,

            [Display(Name = "IKE Data")]
            OnlyCrossTalk = 1,

            [Display(Name = "ANPR Front")]
            OnlyNFFront = 2,

            [Display(Name = "ANPR Rear")]
            OnlyNFRear = 3,

        
            //[Display(Name = "EVI & ANPR data missmatch")]
            //EVI_ANPRMissMatch = 3,
            //[Display(Name = "OCR Missing")]
            //OCRMissing = 5,
            //[Display(Name = "Un- Transaction")]
            //Unaudited = 7,

        }


        public enum ReportTransactionCategory
        {
            [Display(Name = "--All Transactions--")]
            AllTransaction = 0,

            [Display(Name = "ANPR data missing")]
            OnlyEVI = 1,

            [Display(Name = "EVI data missing")]
            OnlyANPR = 2,

            [Display(Name = "EVI & ANPR Vehicle Class Mismatch")]
            EVI_ANPRClassMisMatch = 3,

            [Display(Name = "Reviewed Transaction")]
            Audited = 4,

            [Display(Name = "Unreviewed Transaction")]
            Unaudited = 5,

        }

        //public static string[] TransactionCategoryName = new string[]
        //{
        //    "Select Transaction Category",//0
        //    "Both EVI & ANPR",
        //    "Only EVI",
        //    "Only ANPR",
        //    "EVI & ANPR Miss Match",
        //    "EVI & ANPR Class Miss Match",
        //    "OCR Missing"
        //};

        public enum TransferStatus
        {
            NotTransferred = 1,
            Transferred,
            FailedToTransfer,
        }

        /// <summary>
        ///SubmoduleActivity
        /// </summary>
        public enum Activity
        {
            View = 1,
            Add,
            Edit,
            Delete,
            Control,
        };

        public enum ControlRoom
        {
            ControlRoom1 = 1,
            ControlRoom2 = 2,
            ControlRoom3 = 3,
        }
        public enum TransactionType
        {
            Sale = 1,
            Recharge,
            Refund,
            LaneDebit
        }

        public static string[] TransactionTypeName = new string[]
        {
            "Sale",//0
            "Recharge",
            "Refund"
        };

        /// <summary>
        /// SubmoduleActivityName
        /// </summary>
        public static string[] ActivityName = new string[]
        {
            "CMN_LBL_View",//0
            "CMN_LBL_Add",
            "CMN_LBL_Edit",
            "CMN_LBL_Delete",
            "CMN_LBL_Control",
        };

        public const int GRID_PAGE_SIZE = 20;

        //Show or hide the submodules in the master configuration module. It also include report modules
        public static int[] hiddenSubmoduleList = new int[] { };

        /// <summary>
        /// Possible login result.
        /// </summary>
        public enum LoginResult
        {
            Successful,
            InvalidUser,
            CannotLogin,
            DatabaseError,
            AccountExpired,
            InvalidPassword,
            Cancelled,
            FingerPrintNotMatched,
            DatabaseServerNotConnected
        }

        public enum ATCCAlarmType
        {
            BeamsDegraded = 1,
            BeamsUnbalanced,
            BeamsLost,
            PowerReceiverExtSupply,
            PowerTransmitterExtSupply,
            Logspace,
            Temprature,
            TimeDiscontineouty,
            TimeNetSRVConnectivity,
            TimeAccuracy,
            MobileService,
            RTCBattery,
            GPSReceiption,
            MobilePacket,
            HareNetwork
        }
        public static string[] ATCCAlarmDescription = new string[]
        {
            "Beams Degraded",// 0,
            "Beams Unbalanced",
            "Beams Lost",
            "Receiver ext power supply" ,
            "Transmitter Ext power supply",
            "Log space",
            "Temprature",
            "Time Discontineouty",
            "Time Net SRV Connectivity",
            "Time Accuracy",
            "Mobile Service",
            "RTC Battery",
            "GPS Receiption",
            "Mobile Packet",
            "Hare Network"
        };

        public enum HardwarePosition
        {
            [Display(Name = "--Select--")]
            NoPosition = 0,

            [Display(Name = "Front")]
            Front = 1,

            [Display(Name = "Rear")]
            Rear = 2,
        }

        public enum HardwareType
        {
            [Display(Name = "ANPR Camera")]
            ANPRCamera = 1,
            [Display(Name = "RFID Reader")]
            RFIDReader=2
        }

        public static string[] HardwareTypeName = new string[]
        {
            "ANPR Camera",// 1,
            "RFID Reader",
        };
        public enum METAlarmType
        {
            LowBattery,
            SensorBadData,
            NetworkCommunicationError
        }

        public static string[] METAlarmDescription = new string[]
        {
            "Low Battery",// 0,
            "Sensor Bad Data",
            "Network Communication Error",
        };
        public enum ECBAlarmType
        {
            LowBattery,
            DoorOpen,
            NetworkCommunicationError,
            Vendalisim
        }
        public static string[] ECBAlarmDescription = new string[]
        {
            "Low Battery",// 0,
            "Door Open",
            "Network Communication Error",
            "Vendalisim"
        };
        public enum CCTVAlarmType
        {
            NetworkCommunicationError,
            Vendalisim
        }

        public static string[] CCTVAlarmDescription = new string[]
        {
            "Network Communication Error",
            "Vendalisim"
        };

        public enum VMSAlarmType
        {
            LowBattery,
            NetworkCommunicationError,
            MessageFailed,
            PCBFailure,
            HighInsideTemprature,
            ServiceDueDate_
        }

        public enum EventCategory
        {
            Alarms = 1,
            Situation,
            Notification
        }

        public enum ATMSDeviceCategory
        {
            ECB = 1,
            VMS,
            MET,
            VIDS,
            ATCC,
            CCTV
        }

        public static string[] ATMSDeviceCategoryDescription = new string[]
        {
            "ECB",
            "VMS",
            "MET",
            "VIDS",
            "ATCC",
            "CCTV"
        };

        //Unknown, Vehicle collision, breakdown, vehicle fire, VIP movement, Sports event, Device not working
        public enum IncidentReasonId
        {
            Unknown = 0,
            Accident,
            VehcileBreakDown,
            VehicleFire,
            VIPMovement,
            SportsEvent,
            DeviceNotWorking,
            DeviceNotPinging
        }

        public static string[] IncidentReasonName = new string[]
        {
            "Unknown",
            "Accident",
            "Vehicle Breakdown",
            "Vehicle Fire",
            "VIP Movement",
            "Sports Event",
            "Device Not Working"
        };

        public enum IncidentStatus
        {
            Unknown = 0,
            Handled,
            Unhandled,
        }

        public static string[] IncidentStatusName = new string[]
        {
            "Unknown",
            "Handled",
            "Unhandled"
        };

        public enum IncidentType
        {
            Unknown = 0,
            Genuine,
            Fake,
        }

        public static string[] IncidentTypeName = new string[]
        {
            "Unknown",
            "Genuine",
            "Fake"
        };

        public enum IncidentSource
        {
            Unknown = 0,
            AutoGenerated,
            ReportedByPhone,
            RegisteredByMobileApp
        }

        public static string[] IncidentSourceName = new string[]
        {
            "Unknown",
            "Auto Generated",
            "Reported By phone",
            "Registered By Mobile App"
        };

        public enum EventPriority
        {
            High = 1,
            Medium,
            Low
        }

        public static string[] EventPriorityDescription = new string[]
        {
            "High",
            "Medium",
            "Low"
        };

        public enum EventStatus
        {
            Pending = 0,
            Close
        }
        public static string[] EventStatusDescription = new string[]
        {
            "Pending",
            "Close"
        };

        public enum ECBStatus
        {
            Idle = 0,//On Hook
            Busy,//Off Hook
            Ringing,
            RingStop,
            MissedCall,
            Active,
            Inactive
        }
        public enum CustomerQueueStatus 
        {
            Unknown = 0,
            Open,
            Arranged,
            Processed
        };
        public static string[] ECBStatusDescription = new string[]
        {
            "Idle",//0
            "Busy",
            "Ringing",
            "Ring Stop",
            "MissedCall",
            "Active",
            "Inactive"
        };

        public enum OFCBasedECBStatus
        {
            Idle = 0,
            Incoming,
            Outgoing,
            HookOff,
            HookOn,
            MissedCall,
            Received,
            RecordedSoundPath,
            Outgoing_Ended,
            Vandal,
            MissedCallWhileBusy
        }
        public static string[] OFCBasedECBStatusDescription = new string[]
        {
           "Idle",
           "Incoming",
           "Outgoing",
           "HookOff",
           "HookOn",
           "Missed Call",
           "Received",
           "RecordedSoundPath",
           "Outgoing_Ended",
           "Vandal",
           "Missed Call While Busy"
        };

        public enum IpBasedECBStatus
        {
            Idle = 0,
            Missed,
            Incoming,
            Outgoing,
            Connect,
            Waiting,
            Received,
            DoorStatus,
            BatteryStatus,
            MediaSwitchig,
            SolarPanel,
            ECBState
        }
        public static string[] IpBasedECBStatusDescription = new string[]
        {
            "Idle",
            "Missed",
            "Incoming",
            "Outgoing",
            "Connect",
            "Waiting",
            "Received",
            "DoorStatus",
            "BatteryStatus",
            "MediaSwitchig",
            "SolarPanel",
            "ECBState"
        };

        public enum ECBPacketType
        {
            DefaultValue = 0,
            CallStatus = 1,
            NetworkStatus
        }

        public static string[] ECBPacketTypeDescription = new string[]
        {
            "DefaultValue",
            "CallType",
            "NetworkStatus"
        };

        public enum HardwareStatus
        {
            Inactive = 0,
            Active
        }

        public static string[] HardwareStatusDescription = new string[]
        {
            "Inactive",
            "Active"
        };

        public enum LaneDirection
        {
            NorthToSouth = 1,
            SouthToNorth

        }
        public enum ECBPositionDirection
        {
            LeftSide = 1,
            RightSide

        }
        public static string[] ECBPositionDirectionName = new string[]
        {
            "Left Side",//0
            "Right Side",
        };
        public enum ECBCallType
        {
            MissedCall = 1,
            Received,
            Outgoing
        }
        public static string[] ECBCallTypeName = new string[]
        {
            "MissedCall",//0
            "ReceivedCall",
            "Outgoing",
        };

        public enum DispositionCategory
        {
            Accident = 0,
            FuelFinish,
            CraneRequest,
            HoaxCall,
            AmbulanceRequest,
            Others
        }
        public static string[] DispositionCategoryName = new string[]
        {
            "Accident",//0
            "Fuel Finish",
            "Crane Request",
            "Hoax Call",
            "Ambulance Request",
            "Others"
        };
        public enum Line
        {
            Line1 = 0,
            Line2,
            Line3,
            Line4
        }
        public static string[] LineName = new string[]
        {
            "Line1",//0
            "Line2",
            "Line3",
            "Line4",
        };

        public enum AuditStatus
        {
            Reviewed = 1,
            NotReviewed
        }
        public static string[] AuditStatusName = new string[]
        {
            "Reviewed",//0
            "NotReviewed",
        };

        public enum CameraMake
        {
            Pelco = 1,
            Hikvision,
            Viovotek,
            Samsung,
            LG,
            Sony
        }
        public static string[] CameraMakeName = new string[]
        {
            "Pelco",//1
            "Hikvision",
            "Viovotek",
            "Samsung",
            "LG",
            "Sony"
        };

        public enum MetMake
        {
            Sutron = 1,
            DataTakerDT80
        }
        public static string[] MetMakeName = new string[]
        {
            "Sutron",//1
            "DataTakerDT80"
        };

        public enum MetInfoType
        {
            AirTemp = 1,
            Humidity,
            Visibility,
            RoadTemp,
            WindDirection,
            WindSpeed,
            Rain,
            Rain24Hrs,
            Battery,
            RelativeHumidity,
            AirTempMin,
            AirTempMax
        }

        public static string[] MetInfoTypeName = new string[]
        {
            "Air Temperatue",  //1,
            "Humidity",
            "Visiblity",
            "Road Temp",
            "Wind Direction",
            "Wind Speed",
            "Rain",
            "Rain 24 Hrs",
            "Battary",
            "Min. Temperature",
            "Max. Temperature"
        };

        public enum GraphDuration
        {
            Today = 1,
            Week = 90,
            Month = 90
        };
        public static string[] GraphDurationName = new string[]
        {
            "Today",
            "Week",
            "Month"
        };
        public enum MetCloudType
        {
            Clear = 1,
            PartialyCloudy,
            Cloudy
        };
        public static string[] MetCloudDescription = new String[]
        {
            "Clear",
            "Partialy Cloudy",
            "Cloudy"
        };

        public enum WindDirecDegree
        {
            N = 1,   //348.75 - 11.25
            NNE,     //11.25 - 33.75
            NE,      //33.75 - 56.25
            ENE,     //56.25 - 78.75
            E,       //78.75 - 101.25
            ESE,     //101.25 - 123.75
            SE,      //123.75 - 146.25
            SSE,     //146.25 - 168.75
            S,       //168.75 - 191.25
            SSW,     //191.25 - 213.75
            SW,      //213.75 - 236.25
            WSW,     //236.25 - 258.75
            W,       //258.75 - 281.25
            WNW,     //281.25 - 303.75
            NW,      //303.75 - 326.25          
            NNW      //326.25 - 348.75
        };

        public static string[] WindDirecDegreeDesc = new String[]
        {
            "N-North",                 //348.75 - 11.25
            "NNE-North northeast",     //11.25 - 33.75
            "NE-Northeast",            //33.75 - 56.25
            "ENE-East northeast",      //56.25 - 78.75
            "E-East",                  //78.75 - 101.25
            "ESE-East southeast",      //101.25 - 123.75
            "SE-southeast",            //123.75 - 146.25
            "SSE-South southeast",     //146.25 - 168.75
            "S-South",                 //168.75 - 191.25
            "SSW-South southwest",     //191.25 - 213.75
            "SW-Southwest",            //213.75 - 236.25
            "WSW-West southwest",      //236.25 - 258.75
            "W-West",                  //258.75 - 281.25
            "WNW-West northwest",      //281.25 - 303.75
            "NW-Northwest",            //303.75 - 326.25          
            "NNW-North northwest"      //326.25 - 348.75
        };

        public enum RainType
        {
            Slight_rain = 1,          //Less than 0.5 mm per hour. 
            Moderate_rain,          //Greater than 0.5 mm per hour, but less than 4.0 mm per hour.
            Heavy_rain,             // Greater than 4 mm per hour, but less than 8 mm per hour. 
            Very_heavy_rain         // Greater than 8 mm per hour.
        };

        public static string[] RainTypeDescription = new String[]
        {
            "No Rain",
            "Slight Rain",            //Less than 0.5 mm per hour. 
            "Moderate Rain",          //Greater than 0.5 mm per hour, but less than 4.0 mm per hour.
            "Heavy Rain",             // Greater than 4 mm per hour, but less than 8 mm per hour. 
            "Very Heavy Rain",        // Greater than 8 mm per hour.
            "Violent Shower"          // Greater than 50 mm per hour.
        };

        public enum WindSpeedRange
        {
            Calm = 1,               //<1 km/h
            Light_Air,              //1-5 km/h	    
            Light_Breeze,           //6-11 km/h	
            Gentle_Breeze,          //12-19 km/h	
            Moderate_Breeze,        //20-29	km/h
            Fresh_Breeze,           //30-39 km/h	
            Strong_Breeze,          //40-50 km/h	
            Near_gale,              //50-61 km/h	
            Gale,                   //61-74 km/h	
            Strong_gale,            //74-88 km/h	
            Storm,                  //88-102	km/h
            Violent_Storm,          //103-118 km/h	
            Hurricane               //>118 km/h	
        };

        public static string[] WindSpeedRangeDesc = new String[]
        {
            "Calm",                       //<1 km/h
            "Light Air",                  //1-5 km/h	    
            "Light Breeze",               //5-11 km/h	
            "Gentle Breeze",              //11-19 km/h	
            "Moderate Breeze",            //19-29	km/h
            "Fresh Breeze",               //29-39 km/h	
            "Strong Breeze",              //39-50 km/h	
            "Near Gale",                  //50-61 km/h	
            "Gale",                       //61-74 km/h	
            "Strong Gale",                //74-88 km/h	
            "Storm",                      //88-102	km/h
            "Violent Storm",              //102-118 km/h	
            "Hurricane"                   //>118 km/h	
        };
        public static string[] VisibiltyScale = new String[]
        {
            "Very Poor/Fog",               //<1 km
            "Poor/Mist",                   //1-2 km	    
            "Moderate/Haze",               //2-5 km	
            "Good"                         //>5km
        };

        //Will be assosciated to Template Category
        public enum VMSMessageCategory
        {
            RoadWorks = 1,
            Accident,
            RoadInformation,
            Weather,
            Emergency
        }

        public static string[] VMSMessageCategoryName = new String[]
        {
            "Road Works",
            "Accident",
            "Road Information",
            "Weather",
            "Emergency"
        };

        //Will be assosciated to Message Detail
        public enum VMSCustomMessageType
        {
            None = 0,
            Rain = 1,
            WindDirection,
            WindSpeed,
            AirTemp,
            RelativeHumidity,
            Visiblity,
            DateAndTime,
            Date,
            Time
        }

        public static string[] VMSCustomMessageTypeName = new String[]
        {
            "None",
            "Rain",
            "Wind Direction",
            "Wind Speed",
            "Air Temperature",
            "Humidity",
            "Visiblity",
            "Date And Time",
            "Date",
            "Time"
        };

        public static string[] VMSubCategoryMessageName = new string[]
        {
            "",
            "Rain §§",
            "WiDr §§",
            "WS §§",
            "TEMP §§",
            "HUM §§",
            "VIS §§",
            "DateTime §§",
            "Date §§",
            "Time §§"
        };

        public enum VMSMessageType
        {
            Single = 1,
            Multiple
        }

        public static string[] VMSMessageTypeName = new String[]
        {
            "Single Message",
            "Multiple Messages"
        };

        /// <summary>
        ///ATMS device incident Category. These categories are mapped with database.
        /// </summary>
        public enum DeviceIncidentCategory
        {
            // ECB
            ECB_BATTERY_STATUS = 1,
            ECB_MASTER_DOOR_OPEN = 2,
            ECB_SLAVE_DOOR_OPEN = 3,
            ECB_SOLAR_PANEL_STATUS = 4,
            ECB_CONNECT_STATUS = 15,

            // VMS
            VMS_CONNECTION_STATUS = 5,
            VMS_DOOR_OPEN = 6,

            // MET
            MET_CONNECTION_STATUS = 14,

            // VIDS
            VIDS_CONNECTION_STATUS = 7,
            VIDS_WRONG_DIRECTION = 8,
            VIDS_WRONG_PARKING = 9,
            VIDS_WATER_LOGGING = 10,
            VIDS_RED_LIGHT_VIOLATION = 11,
            VIDS_SPEED_VOILATION = 12,
            VIDS_CONGESSION = 13,

            // ATCC
            ATCC_CONNECTION_STATUS = 16,

            // CCTV
            CCTV_CONNECTION_STATUS = 17
        };

        /// <summary>
        /// IncidentCategory
        /// </summary>
        public enum IncidentCategory
        {
            Unknown = 0,
            Accident = 1,
            RoadMaintenance = 2,
            DeviceStatus
        }
        public static string[] IncidentCategoryName = new String[]
        {
            "Unknown",
            "Accident",
            "Road Maintenence",
            "Device Status"
        };

        /// <summary>
        /// Asset Category
        /// </summary>
        public enum AssetCategory
        {
            CCTV = 1,
            ATCC,
            ECB,
            MET,
            VIDS,
            VMS

        }


        public static string[] AssetCategoryName = new string[]
        {
            "CCTV",
            "ATCC",
            "ECB",
            "MET",
            "VIDS",
            "VMS"
        };

        /// <summary>
        /// Status of Assets
        /// </summary>
        public enum AssetStatus
        {
            Working = 1,
            NotWorking,
            Lost,
            Damaged,
            NotInstalled,
            Theft
        }

        public static string[] AssetStatusName = new string[]
        {
            "Working",
            "Not Working",
            "Lost",
            "Damaged",
            "Not Installed",
            "Theft"
        };
        public enum AssetMaintenanceTypeID
        {
            Repair = 1,
            Replace
        }

        public static string[] AssetMaintenanceType = new string[]
        {
            "Repair",
            "Replace"
        };

        /// <summary>
        /// VMS type
        /// </summary>
        public enum VMSType
        {
            ChainZone = 1,
            PhotonPlay = 2
        }
        public static string[] VMSTypeName = new String[]
        {
            "Chain Zone",
            "Photon Play"
        };


        public enum ECBType
        {
            GSM = 1,
            VOID = 2,
            Aragon = 3
        }
        public static string[] ECBTypeName = new String[]
        {
            "GSM",
            "VOID",
            "Aragon"
        };


        public enum CCTVType
        {
            HIKVISION = 1,
            Pelco = 2,
            AXIS = 3,
            CPPlus = 4,
            Vivotek = 5
        }
        public static string[] CCTVTypeName = new String[]
        {
            "HIKVISION",
            "Pelco",
            "AXIS",
            "Cp Plus",
            "Vivotek"
        };

        public enum ATCCType
        {
            Tirtel = 1,
            Diamond = 2
        }
        public static string[] ATCCTypeName = new String[]
        {
            "Tirtel",
            "Diamond"
        };

        /// <summary>
        ///EventSeverity
        /// </summary>
        public enum EventSeverity
        {
            Critical = 1,
            Urgent,
            High,
            Normal,
            Low
        };

        public static string[] EventSeverityName = new String[]
        {
            "Urgent",
            "High",
            "Normal",
            "Low"
        };

        /// <summary>
        ///VMSMessageLanguage
        /// </summary>
        public enum VMSMessageLanguage
        {
            English = 1,
            Hindi
        };

        /// <summary>
        /// VMSMessageLanguageName
        /// </summary>
        public static string[] VMSMessageLanguageName = new string[]
        {
            "English",
            "Hindi"
        };

        public enum SMSDirection
        {
            Incoming = 1,
            Outgoing
        };

        public static string[] SMSDirectionName = new string[]
        {
            "Incoming",
            "Outgoing"
        };
        public enum SMSSentStatus
        {
            Unsent = 1,
            Sent,
            Ignored
        };
        public enum EmailSentStatus
        {
            Unsent = 1,
            Sent,
            Ignored
        };

        public static string[] SMSSentStatusName = new string[]
        {
            "Unsent",
            "Sent",
            "Ignored"
        };

        public enum SMSReceivedMessageProcessStatus
        {
            UnProcessed = 1,
            Processed
        };

        public static string[] SMSReceiveProcessStatusName = new string[]
        {
            "UnProcessed",
            "Processed"
        };

        public enum SMSDeliveryStatus
        {
            UnDelivered = 1,
            Delivered
        };

        public static string[] SMSDeliveryStatusName = new string[]
        {
            "UnDelivered",
            "Delivered"
        };

        public enum LaneType
        {
            [Display(Name = "Normal")]
            Normal = 1,
            [Display(Name = "Trans Jakarta")]
            TransJakarta
        };

        public static string[] LaneTypeName = new string[]
        {
            "Normal",
            "TransJakarta"
        };

        #endregion

        #region Methods
        public static int GetCurrentTMSId()
        {
            return 1;
        }
        public static int GetCurrentPlazaId()
        {
            return 1;
        }

        public static int GetControlRoomId()
        {
            return 1;
        }

        public static Span GetModuleNameForApplicationHeader(string moduleName)
        {
            Span span = new Span();
            //Paragraph span = new Paragraph();

            span.Inlines.Add(new Run("VaaaN-ATMS"));
            //span.Inlines.Add(new Bold(new Italic(new Underline(new Run("M")))));
            //span.Inlines.Add(new Run("ax"));
            Run r = new Run("®");
            r.BaselineAlignment = System.Windows.BaselineAlignment.Superscript;
            r.FontSize = 15.0;
            span.Inlines.Add(r);
            span.Inlines.Add(new Bold(new Run(moduleName)));
            span.Inlines.Add(new Bold(new Run(" Version: " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString())));

            return span;
        }

        public static String GetDevelopedByName()
        {
            string result = "VaaaN Infra";

            try
            {
                //    VaaaN.ATMS.Libraries.CommonLibrary.CBE.ATMSConfigurationCBE atmsConfiguration = new Libraries.CommonLibrary.CBE.ATMSConfigurationCBE();
                //    atmsConfiguration = VaaaN.ATMS.Libraries.CommonLibrary.BLL.ATMSConfigurationBLL.GetConfiguration();
                // result = atmsConfiguration.DevelopedBy;
            }
            catch (Exception)
            {
                result = "VaaaN Infra";
            }

            return result;
            //return " VaaaN Infra, 2013 INDIA";
        }

        public static String GetCompanyName()
        {
            string result = "VaaaN Infra";

            try
            {
                //VaaaN.ATMS.Libraries.CommonLibrary.CBE.ATMSConfigurationCBE atmsConfiguration = new Libraries.CommonLibrary.CBE.ATMSConfigurationCBE();
                //atmsConfiguration = VaaaN.ATMS.Libraries.CommonLibrary.BLL.ATMSConfigurationBLL.GetConfiguration();
                //result = atmsConfiguration.CompanyName;
            }
            catch (Exception)
            {
                result = "VaaaN Infra";
            }

            return result;

            //return "N A G P U R - B E T U L    E X P R E S S     W A Y";
        }

        public static bool IsPingSuccessful(string ipAddress)
        {
            bool result = true;

            try
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();

                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                options.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 50;

                PingReply reply = pingSender.Send(ipAddress, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public static bool CheckIPValid(string strIP)
        {
            IPAddress result = null;
            return
                !String.IsNullOrEmpty(strIP) &&
                IPAddress.TryParse(strIP, out result);
        }

        public static int DeleteLogFile()
        {
            int count = 0;

            try
            {
                //Get all files with full path
                string[] filePaths = Directory.GetFiles(driveLetter + @":\ATMS\log\", "*.*", SearchOption.AllDirectories);

                //Iterate thru each file
                foreach (string path in filePaths)
                {
                    FileInfo fi = new FileInfo(path);

                    //Delete file if size is greater than 500 MB it happens some time if there is no connectivity and log file is written in thread loop
                    // Delete 20 days older log files
                    if (fi.Length >= 524288000 || (DateTime.Now - fi.CreationTime).TotalDays > 20)
                    {
                        fi.Delete();
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return count;
        }

        public static void StartWindowsService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);

            TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);
        }

        #endregion

        #region Regex

        /// <summary>
        /// for digits with decimal. 7 digits before decimal & 3digits after decimal.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDecimalNumber(string input)
        {
            bool result = false;

            if (Regex.Match(input, @"^\d{1,7}(\.\d{0,3})?$").Success)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// for only digits with length of 4.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDigit(string input)
        {
            bool result = false;

            if (Regex.Match(input, @"^\d{1,4}$").Success)
            {
                result = true;
            }

            return result;
        }


        /// <summary>
        /// for only digits.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsADigit(string input)
        {
            bool result = false;

            if (Regex.Match(input, @"^\d{1,10}$").Success)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// for alphabets with space.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAlpha(string input)
        {
            bool result = false;

            if (Regex.Match(input, @"^([a-zA-Z\s])*$").Success)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// for IP address.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIP(string input)
        {
            bool result = false;

            if (Regex.Match(input, @"^(([01]?\d\d?|2[0-4]\d|25[0-5])\.){3}([01]?\d\d?|25[0-5]|2[0-4]\d)$").Success)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// for Email.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            bool result = false;

            if (Regex.Match(input, @"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$").Success)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// for CellNo.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsCellNO(string input)
        {
            bool result = false;

            if (Regex.Match(input, @"^([0-9]{10})$").Success)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// for Special Chararcters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsSpecialChar(string input)
        {
            bool result = false;

            if (Regex.Match(input, @"^[a-zA-Z0-9]*$").Success)
            {
                result = true;
            }

            return result;
        }
        #endregion
    }
}