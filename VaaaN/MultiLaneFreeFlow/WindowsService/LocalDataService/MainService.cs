using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using System.Collections;
using System.Threading;
using System.IO;

namespace VaaaN.MLFF.WindowsServices
{
    public partial class MainService : ServiceBase
    {
        #region Variables
        private Queue logQueue = new Queue();
        private Thread loggerThread;
        private volatile Boolean stopLoggerThread = false;

        VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection lanes;
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection hardwares;
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCollection plazas;
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection customerAccounts;
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection customerVehicles;
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection tollRates;
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection vehicleClasses;

        private MessageQueue failedQueue;
        private MessageQueue inBoxQueue;
        private MessageQueue smsMessageQueue;
        private MessageQueue eventQueue;
        int ctpEntryId = 0;
        int nfpEntryId = 0;
        int currentTMSId = -1;

        DateTime countStartTime = DateTime.MinValue;
        int motorCycleCount = 0;
        int smallCount = 0;
        int mediumCount = 0;
        int bigCount = 0;
        string counterString = string.Empty;
        #endregion

        #region Constructor
        public MainService()
        {
            InitializeComponent();

            //dont forget to comment this line
            //OnStart(new string[] { "sd" }); //<===================================================================== only for debugging

            //tollRates = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TollRateBLL.GetAll();
            //DateTime dt = DateTime.ParseExact("13/10/2018 23:24:25.111", "dd/MM/yyyy HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
            //GetTollRate(1, 1, dt, 1);
        }

        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MainService()
            };
            ServiceBase.Run(ServicesToRun);
        }
        #endregion

        #region Events
        protected override void OnStart(string[] args)
        {
            currentTMSId = VaaaN.MLFF.Libraries.CommonLibrary.Constants.GetCurrentTMSId();

            countStartTime = System.DateTime.Now;
            motorCycleCount = 0;
            smallCount = 0;
            mediumCount = 0;
            bigCount = 0;

            try
            {
                LogMessage("Starting TDM service logger thread...");

                loggerThread = new Thread(new ThreadStart(this.LoggerThreadFunction));
                loggerThread.IsBackground = true;
                loggerThread.Start();

                LogMessage("The TDM Service logger has been started.");
            }
            catch (Exception ex)
            {
                LogMessage("Error in starting TDM Service logger thread function. TDM Service cannot be started. " + ex.ToString());
            }

            try
            {
                LogMessage("Getting collections of plazas, lanes, hardwares...");

                lanes = VaaaN.MLFF.Libraries.CommonLibrary.BLL.LaneBLL.GetAll();
                plazas = VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.GetAllAsCollection();
                hardwares = VaaaN.MLFF.Libraries.CommonLibrary.BLL.HardwareBLL.GetAll();
                customerAccounts = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsCollection();
                customerVehicles = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetAllAsCollection();
                tollRates = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TollRateBLL.GetAll();
                vehicleClasses = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAllAsCollection();

                LogMessage("Required collections has been accessed.");
            }
            catch (Exception ex)
            {
                LogMessage("Exception in accessing collections. " + ex.ToString());
            }

            try
            {
                LogMessage("Creating reference for SMS queue...");

                this.smsMessageQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.smsMessageQueue);

                LogMessage("Reference for SMS queue has been created successfully.");
            }
            catch (Exception ex)
            {
                LogMessage("Failed to create reference for SMS queue. " + ex.Message);
            }

            try
            {
                LogMessage("Creating reference for event queue queue...");

                this.eventQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.eventQueue);

                LogMessage("Reference for event queue has been created successfully.");
            }
            catch (Exception ex)
            {
                LogMessage("Failed to create reference for event queue. " + ex.Message);
            }

            try
            {
                LogMessage("Trying to start LDS service...");
                this.inBoxQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.inBoxQueueName);
                inBoxQueue.PeekCompleted += new PeekCompletedEventHandler(InBoxQueue_PeekCompleted);
                inBoxQueue.BeginPeek();

                this.failedQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.failedQueueName);
                LogMessage("LDS service started successfully.");
            }
            catch (Exception ex)
            {
                LogMessage("Failed to start LDS service." + ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                inBoxQueue.PeekCompleted -= new PeekCompletedEventHandler(InBoxQueue_PeekCompleted);
            }
            catch (Exception ex)
            {
                LogMessage("Failed to process OnStop..." + ex.Message);
            }
            finally
            {
                base.OnStop();
            }

            try
            {
                stopLoggerThread = true;
                Thread.Sleep(100);
                if (loggerThread != null)
                {
                    if (loggerThread.IsAlive == true)
                    {
                        loggerThread.Abort();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        void InBoxQueue_PeekCompleted(object sender, PeekCompletedEventArgs e)
        {
            bool receiveRecord = false;
            Stopwatch sw = new Stopwatch();
            MessageQueue mq = (MessageQueue)sender;

            try
            {
                sw.Start();

                Message m = (Message)mq.EndPeek(e.AsyncResult);
                m.Formatter = new BinaryMessageFormatter();
                LogMessage("Processing InBoxQueue message.");

                ProcessQueueMessage(m);
                receiveRecord = true;
            }
            catch (Exception ex)
            {
                LogMessage("Error in peeking inbox queue. " + ex.ToString());
                receiveRecord = false;
            }
            finally
            {
                //if (receiveRecord)
                //{
                    mq.Receive();
                    sw.Stop();
                //}
                //else
                //{
                    //receive and send to failed queue
                //}

                inBoxQueue.BeginPeek();
            }
        }
        #endregion

        #region Helper Methods

        private void ProcessQueueMessage(Message m)
        {
            if (m != null)
            {
                m.Formatter = new BinaryMessageFormatter();

                if (m.Body != null)
                {
                    #region Processing packets
                    if (m.Body is VaaaN.MLFF.Libraries.CommonLibrary.Common.CrossTalkPacket)
                    {
                        #region CrossTalk packet
                        LogMessage("==CROSSTALK PACKET==");

                        VaaaN.MLFF.Libraries.CommonLibrary.Common.CrossTalkPacket crossTalkPacket = (VaaaN.MLFF.Libraries.CommonLibrary.Common.CrossTalkPacket)m.Body;

                        if (crossTalkPacket.Payload != null)
                        {
                            if (crossTalkPacket.Payload is VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE)
                            {
                                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE ctp = (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE)crossTalkPacket.Payload;

                                #region parsing tagid
                                ctp.ObjectId = ctp.ObjectId.Trim(); //otherwise trailing and leading spaces create problems
                                TagStructure ts = ParseEPC(ctp.ObjectId);

                                int eviClass = -1;
                                string eviVRN = "";
                                if (ts != null)
                                {
                                    eviClass = ts.ClassId;
                                    eviVRN = ts.VRN;

                                    LogMessage("Checking the tag exists in the system or not. " + ctp.ObjectId);
                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE associatedCVCT = DoesTagExist(ctp.ObjectId);
                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE associatedCACT = null;
                                    if (associatedCVCT != null)
                                    {
                                        LogMessage("Tag exists.");
                                        associatedCACT = GetCustomerAccountById(associatedCVCT.AccountId);

                                        LogMessage("The associated accoount is: " + associatedCACT.FirstName + " " + associatedCACT.LastName);

                                        #region Update some fields in the CBE
                                        try
                                        {
                                            LogMessage("Trying to update TMSId, Creation Date etc. in the CBE...");
                                            ctp.TMSId = currentTMSId;
                                            ctp.TimeStamp = ConversionDateTime(ctp.TimeStamp, "crosstalk");

                                            //in case of crosstalk not giving lane id and plaza id peek laneid and plazaid by hardwareid. 
                                            //hardwareid is assigned to specific lane of specific plaza.
                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane = GetLaneDetailByHardwareId(Convert.ToInt32(ctp.LocationId)); //locationid is hardwareid? (satya said)

                                            if (lane != null)
                                            {
                                                ctp.PlazaId = lane.PlazaId; //<== need to update as per device location
                                                ctp.LaneId = lane.LaneId; //<== need to update as per device location
                                            }
                                            else
                                            {
                                                LogMessage("No lane detail found against the hardware id: " + ctp.LocationId);
                                                //future processing should be closed here
                                            }

                                            ctp.CreationDate = System.DateTime.Now;
                                            ctp.ModifierId = 1;
                                            ctp.ModificationDate = System.DateTime.Now;
                                            LogMessage("Crosstalk CBE updated successfully.");

                                            try
                                            {
                                                LogMessage("Trying to push crosstalk event to event queue...");
                                                //plaza id, plaza name, lane id, lane name, vrn, class, timestamp
                                                Message crosstalkEventMessage = new Message();
                                                crosstalkEventMessage.Formatter = new BinaryMessageFormatter();
                                                crosstalkEventMessage.TimeToBeReceived = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.eventQueueTimeOut;

                                                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkEvent ctEvent = new Libraries.CommonLibrary.CBE.CrossTalkEvent();

                                                ctEvent.Timestamp = Convert.ToDateTime(ConversionDateTime(ctp.TimeStamp, "crosstalk"));
                                                ctEvent.PlazaId = ctp.PlazaId;
                                                ctEvent.PlazaName = GetPlazaNameById(ctp.PlazaId);
                                                ctEvent.LaneName = GetLaneNameById(ctp.LaneId);
                                                ctEvent.VehicleClassName = associatedCVCT.VehicleClassName;
                                                ctEvent.VRN = associatedCVCT.VehRegNo;

                                                crosstalkEventMessage.Body = ctEvent;

                                                eventQueue.Send(crosstalkEventMessage);
                                                LogMessage("Crosstalk event pushed to event queue successfully.");
                                            }
                                            catch (Exception ex)
                                            {
                                                LogMessage("Exception in pushing crosstalk event to event queue. " + ex.ToString());
                                            }


                                        }
                                        catch (Exception ex)
                                        {
                                            LogMessage("Exception in updating some fields. " + ex.ToString());
                                        }
                                        #endregion

                                        //does recent transactions contains this tag? it may be reported repetatively. 
                                        //check in tbl_crosstalk_packet's most recent transactions of the same plaza
                                        #region Check in recent crosstalk packets
                                        LogMessage("Checking already inserted..." + ctp.PlazaId + ", " + ctp.ObjectId + ", " + ctp.TimeStamp);
                                        if (!DoesExistInRecentCrossTalkPackets(ctp.PlazaId, ctp.ObjectId, Convert.ToDateTime(ctp.TimeStamp)))
                                        {
                                            #region Step 2: Send to local database
                                            try
                                            {
                                                LogMessage("Sending crosstalk packet to local database. " + ctp.ObjectId);
                                                ctpEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CrossTalkBLL.Insert(ctp);
                                                LogMessage("Crosstalk packet inserted successfully.");
                                            }
                                            catch (Exception ex)
                                            {
                                                #region Send data to Failed queue
                                                LogMessage("Failed to insert crosstalk packet." + ex.Message);
                                                try
                                                {
                                                    LogMessage("Trying to send to failed queue...");

                                                    m.Recoverable = true;
                                                    failedQueue.Send(m);

                                                    LogMessage("Message sent to failed queue.");
                                                }
                                                catch (Exception exc)
                                                {
                                                    LogMessage("***DATA LOST*** Failed to send to failed queue. Crosstalk packet Transaction is " + ctp.ToString() + exc.ToString());
                                                }
                                                #endregion
                                            }
                                            #endregion

                                            //is the associated VRN is already inserted in the transaction table by nodeflux front or nodeflux rear camera?
                                            //if inserted look by associated vrn and update, if not create a new transaction
                                            LogMessage("Searching associated transaction exists in the transaction table or not...");
                                            DateTime ctpDateTime = Convert.ToDateTime(ctp.TimeStamp);
                                            LogMessage("Search criteria: " + ctp.TMSId + ", " + ctp.PlazaId + ", " + ctpDateTime.ToString("dd/MM/yyyy hh:mm:ss.fff tt") + ", " + associatedCVCT.VehRegNo);
                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans1 = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetCorrespondingTransactionInNodeFlux(ctp.TMSId, ctp.PlazaId, ctpDateTime, associatedCVCT.VehRegNo);

                                            if (trans1.Count > 0)
                                            {
                                                if (trans1.Count == 1)
                                                {
                                                    #region Update in main transaction table
                                                    try
                                                    {
                                                        #region Update CTP section in main transaction table
                                                        LogMessage("Updating in main transaction table...");
                                                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateCrossTalkSection(trans1[0], ctpEntryId);//, eviVehicleClassId, eviVRN);
                                                        LogMessage("Record updated successfully.");
                                                        #endregion

                                                        #region Get vehicle class matced to VRN front and rear
                                                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nfPacketFront;
                                                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nfPacketRear;
                                                        Int32 vehicleClassIdFront = -1;
                                                        Int32 vehicleClassIdRear = -1;
                                                        if (trans1[0].NodefluxEntryIdFront > 0)
                                                        {
                                                            nfPacketFront = VaaaN.MLFF.Libraries.CommonLibrary.DAL.NodeFluxDAL.GetByEntryId(trans1[0].NodefluxEntryIdFront);
                                                            if (nfPacketFront != null)
                                                            {
                                                                vehicleClassIdFront = nfPacketFront.VehicleClassId;
                                                            }
                                                        }
                                                        if (trans1[0].NodefluxEntryIdRear > 0)
                                                        {
                                                            nfPacketRear = VaaaN.MLFF.Libraries.CommonLibrary.DAL.NodeFluxDAL.GetByEntryId(trans1[0].NodefluxEntryIdRear);
                                                            if (nfPacketRear != null)
                                                            {
                                                                vehicleClassIdRear = nfPacketRear.VehicleClassId;
                                                            }
                                                        }
                                                        #endregion

                                                        //does the EVI class and AVC class matched? if not,  mark it as violation and leave it for manual review.
                                                        //else deduct the balance
                                                        #region Charging and SMSing
                                                        //if anyone is matched, do the financial operation, no double charging
                                                        if ((associatedCVCT.VehicleClassId == vehicleClassIdFront) || (associatedCVCT.VehicleClassId == vehicleClassIdRear))
                                                        {
                                                            if (trans1[0].IsViolation == -1) //0 for normal, 1 for violtion, by default -1 (this means this is not not updated)
                                                            {
                                                                if (trans1[0].IsBalanceUpdated == -1) //0 for balance not updated, 1 means balance updated
                                                                {
                                                                    if (associatedCACT != null)
                                                                    {
                                                                        //financial operation here
                                                                        FinancialProcessing(associatedCVCT, associatedCACT, trans1[0]);

                                                                        //notification operation here
                                                                        NotificationProcessing(associatedCVCT, associatedCACT, trans1[0]);
                                                                    }
                                                                    else
                                                                    {
                                                                        LogMessage("Associated customer account of this EVI id is found null.");
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.MarkAsViolation(trans1[0]);

                                                            //violation vms message
                                                        }
                                                        #endregion
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        LogMessage("Failed to insert crosstalk packet in main transaction table." + ex.Message);
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    LogMessage("Abnormal case: Multiple entries found in the transaction table for this nodeflux packet in the specified time window (1 minute).");
                                                }
                                            }
                                            else
                                            {
                                                #region Insert into main transaction table
                                                try
                                                {
                                                    LogMessage("No associated transaction found in transaction table. Inserting into main transaction table...");
                                                    VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByCTP(ctp, ctpEntryId);//, eviVehicleClassId, eviVRN);
                                                    LogMessage("Crosstalk packet inserted successfully.");
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogMessage("Failed to insert crosstalk packet in main transaction table." + ex.Message);
                                                }
                                                #endregion
                                            }
                                        }
                                        else
                                        {
                                            LogMessage("Repeated crosstalk reporting. Discarded.");
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        LogMessage("Tag " + ctp.ObjectId + " does not exist in the system. So discarded. Such type of vehicles will be detected by ANPR cameras.");
                                    }
                                }
                                else
                                {
                                    LogMessage("Invalid tag. " + ctp.ObjectId);
                                }
                                #endregion
                            }
                            else
                            {
                                LogMessage("Payload is not of crosstalk packet type.");
                            }
                        }
                        else
                        {
                            LogMessage("Crosstalk packet's payload is null.");
                        }
                        #endregion
                    }
                    else if (m.Body is VaaaN.MLFF.Libraries.CommonLibrary.Common.NodeFluxPacket)
                    {
                        #region NodeFlux packet
                        LogMessage("==NODEFLUX PACKET==");

                        VaaaN.MLFF.Libraries.CommonLibrary.Common.NodeFluxPacket nfpBody = (VaaaN.MLFF.Libraries.CommonLibrary.Common.NodeFluxPacket)m.Body;
                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nfp = null;

                        if (nfpBody.Payload != null)
                        {
                            if (nfpBody.Payload is VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE)
                            {
                                nfp = (VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE)nfpBody.Payload;

                                if (nfp != null)
                                {
                                    if (!string.IsNullOrEmpty(nfp.PlateNumber))
                                    {
                                        nfp.PlateNumber = nfp.PlateNumber.Trim(); //leading or trailing spaces create problem
                                    }
                                    else
                                    {
                                        nfp.PlateNumber = "";
                                        LogMessage("Plate number is comming as blank!");
                                    }

                                    #region Update some fields in the CBE
                                    try
                                    {
                                        LogMessage("Trying to update TMSId, CreationDate etc. in nodeflux packet..");
                                        nfp.TMSId = currentTMSId;

                                        LogMessage("Camera id is: " + nfp.CameraId);

                                        //in case of nodeflux not giving lane id and plaza id peek laneid and plazaid by hardwareid. 
                                        //hardwareid is assigned to specific lane of specific plaza.
                                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane = GetLaneDetailByHardwareId(nfp.CameraId);

                                        if (lane != null)
                                        {
                                            nfp.GantryId = lane.PlazaId;
                                            nfp.LaneId = lane.LaneId;
                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE camera = GetHardwareById(nfp.CameraId);
                                            if (camera != null)
                                            {
                                                nfp.CameraPosition = camera.HardwarePosition.ToString(); //1 for front, 2 for rear
                                            }
                                            else
                                            {
                                                LogMessage("No camera found with id: " + nfp.CameraId);
                                            }
                                            //nodeflux is not giving vehicle class id, but name, so we have to get it by name
                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE nfVehicleClass = GetVehicleClassByName(nfp.VehicleClassName);
                                            if (nfVehicleClass != null)
                                            {
                                                nfp.VehicleClassId = nfVehicleClass.Id;
                                            }
                                            else
                                            {
                                                LogMessage("Vehicle class name might be wrong in nodeflux packet. (motorcycle, small, medium, big)" + nfp.VehicleClassName);
                                            }
                                        }
                                        else
                                        {
                                            LogMessage("No lane detail found against the lane id: " + nfp.LaneId);
                                        }

                                        nfp.CreationDate = System.DateTime.Now;
                                        nfp.ModifierId = -1;
                                        nfp.ModificationDate = System.DateTime.Now;
                                        LogMessage("Nodeflux CBE updated successfully");

                                        #region event queue
                                        try
                                        {
                                            LogMessage("Trying to push nodeflux event to event queue...");
                                            //plaza id, plaza name, lane id, lane name, vrn, class, timestamp
                                            Message nodeFluxEventMessage = new Message();
                                            nodeFluxEventMessage.Formatter = new BinaryMessageFormatter();
                                            nodeFluxEventMessage.TimeToBeReceived = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.eventQueueTimeOut;

                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxEvent nfEvent = new Libraries.CommonLibrary.CBE.NodeFluxEvent();
                                            nfEvent.Timestamp = Convert.ToDateTime(nfp.TimeStamp);
                                            nfEvent.PlazaId = nfp.GantryId;
                                            nfEvent.PlazaName = GetPlazaNameById(nfp.GantryId);
                                            nfEvent.LaneName = lane.LaneName;
                                            nfEvent.VehicleClassName = nfp.VehicleClassName;
                                            nfEvent.VRN = nfp.PlateNumber;
                                            nfEvent.VideoURL = nfp.VideoURL;
                                            nfEvent.NumberPlatePath = nfp.PlateThumbnail;
                                            nfEvent.VehiclePath = nfp.VehicleThumbnail;
                                            if (nfp.CameraPosition == "1")
                                            {
                                                nfEvent.CameraLocation = "Front";
                                            }
                                            else if (nfp.CameraPosition == "2")
                                            {
                                                nfEvent.CameraLocation = "Rear";
                                            }
                                            else
                                            {
                                                nfEvent.CameraLocation = "Undefined";
                                            }

                                            nodeFluxEventMessage.Body = nfEvent;

                                            eventQueue.Send(nodeFluxEventMessage);
                                            LogMessage("NodeFlux event pushed to event queue successfully.");
                                        }
                                        catch (Exception ex)
                                        {
                                            LogMessage("Exception in pushing nodeflux event to event queue. " + ex.ToString());
                                        }
                                        #endregion

                                    }
                                    catch (Exception ex)
                                    {
                                        LogMessage("Exception in updating nodeflux fields. " + ex.Message);
                                    }
                                    #endregion

                                    //whether the vrn exists or does not exist, we have to push it to local table after checking the case of
                                    //multiple reporting
                                    LogMessage("Checking existing nodeflux records for: " + nfp.PlateNumber);
                                    if (string.IsNullOrEmpty(nfp.PlateNumber))
                                    {
                                        #region Send to local nodeflux database
                                        try
                                        {
                                            LogMessage("VRN is blank. Sending to local nodeflux table...");
                                            nfpEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.NodeFluxBLL.Insert(nfp);
                                            LogMessage("Nodeflux record with blank VRN inserted successfully.");
                                        }
                                        catch (Exception ex)
                                        {
                                            #region Send data to Failed queue
                                            LogMessage("Failed to insert nodeflux packet. " + ex.Message);

                                            try
                                            {
                                                LogMessage("Trying to send to failed queue...");
                                                m.Recoverable = true;
                                                failedQueue.Send(m);
                                                LogMessage("Message sent to failed queue.");
                                            }
                                            catch (Exception exc)
                                            {
                                                LogMessage("***DATA LOST*** Failed to send to failed queue. nodeflux packet  is " + nfp.ToString() + exc.ToString());
                                            }
                                            #endregion
                                        }
                                        #endregion

                                        #region Create a transaction Main transaction table 
                                        try
                                        {
                                            LogMessage("VRN is blank. Trying to insert into transaction table...");

                                            if (nfp.CameraPosition == "1") //1 means front, 2 means rear
                                            {
                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPFront(nfp, nfpEntryId);
                                                LogMessage("Transaction inserted by nf entry id front.");
                                            }
                                            else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
                                            {
                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPRear(nfp, nfpEntryId);
                                                LogMessage("Transaction inserted by nf entry id rear.");
                                            }
                                            else
                                            {
                                                LogMessage("Invalid camera position. " + nfp.CameraPosition);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            LogMessage("Exception in inserting in main transaction table.");
                                        }
                                        #endregion
                                    }
                                    else if (!DoesExistInRecentNodeFluxPackets(nfp.GantryId, nfp.PlateNumber, Convert.ToDateTime(nfp.TimeStamp), Convert.ToInt32(nfp.CameraPosition)))
                                    {
                                        LogMessage("Does not exist in local nodeflux table.");

                                        #region Send to local nodeflux database
                                        try
                                        {
                                            LogMessage("Sending to local nodeflux table. Plate number is: " + nfp.PlateNumber);
                                            nfpEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.NodeFluxBLL.Insert(nfp);
                                            LogMessage("NodeFlux packet inserted successfully.");
                                        }
                                        catch (Exception ex)
                                        {
                                            #region Send data to Failed queue
                                            LogMessage("Failed to insert nodeflux packet. " + ex.Message);

                                            try
                                            {
                                                LogMessage("Trying to send to failed queue...");

                                                m.Recoverable = true;
                                                failedQueue.Send(m);

                                                LogMessage("Message sent to failed queue.");
                                            }
                                            catch (Exception exc)
                                            {
                                                LogMessage("***DATA LOST*** Failed to send to failed queue. nodeflux packet  is " + nfp.ToString() + exc.ToString());
                                            }
                                            #endregion
                                        }
                                        #endregion

                                        //is the VRN actually exist in the system?
                                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE associatedCVNF = DoesVRNExist(nfp.PlateNumber);
                                        if (associatedCVNF != null)
                                        {
                                            #region Checking VRN in recent transactions in tbl_transaction
                                            LogMessage("VRN exists in the system, checking whether it is in recent transactions or not...");
                                            DateTime nfpDateTime = Convert.ToDateTime(nfp.TimeStamp);
                                            LogMessage("Search criteria: " + nfp.TMSId + ", " + nfp.GantryId + ", " + nfpDateTime.ToString("dd/MM/yyyy hh:mm:ss.fff tt") + " " + nfp.PlateNumber);
                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection associatedCrossTalkTrans = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetCorrespondingTransactionInCrossTalk(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber);
                                            #endregion

                                            if (associatedCrossTalkTrans.Count > 0)
                                            {
                                                #region Complete transaction, balance update and SMS
                                                if (associatedCrossTalkTrans.Count == 1)
                                                {
                                                    LogMessage("Transaction found to update...");
                                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = associatedCrossTalkTrans[0];


                                                    #region Get customer vehicle and customer account
                                                    //Get vehicle details of the associated tagid (VRN, Customer Account id etc)
                                                    LogMessage("Getting vehicle details...");
                                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicleInfo = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetByTansactionCrosstalkEntryId(transaction.CrosstalkEntryId);

                                                    //Get customer details of the associated tagid
                                                    LogMessage("Getting customer details...");
                                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccountInfo = GetCustomerAccountById(customerVehicleInfo.AccountId);
                                                    #endregion

                                                    #region Existing transaction update
                                                    //in the main transaction table, if found update the nodeflux related fields. this is normal transaction
                                                    if (nfp.CameraPosition == "1") //1 means front, 2 means rear
                                                    {
                                                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateNodefluxSectionFront(transaction, nfpEntryId);
                                                        LogMessage("Transaction updated by nf entry id front.");
                                                    }
                                                    else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
                                                    {
                                                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateNodefluxSectionRear(transaction, nfpEntryId);
                                                        LogMessage("Transaction updated by nf entry id rear.");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Invalid camera position. " + nfp.CameraPosition);
                                                    }
                                                    #endregion

                                                    #region charging and notification
                                                    //if the transactionno marked as violation before?
                                                    if (associatedCrossTalkTrans[0].IsViolation == -1) //0 for normal, 1 for violtion, by default -1 (not updated)
                                                    {
                                                        LogMessage("Transaction is not marked as violation previously. Going to check violation...");
                                                        if (customerVehicleInfo.VehicleClassId == nfp.VehicleClassId)
                                                        {
                                                            LogMessage("Tag class and NF class matched. Going to financial and notification processing...");
                                                            if (associatedCrossTalkTrans[0].IsBalanceUpdated == -1) //0 for balance not updated, 1 means balance updated
                                                            {
                                                                //financial operation here
                                                                FinancialProcessing(customerVehicleInfo, customerAccountInfo, transaction);
                                                                LogMessage("Financial processing has been done.");

                                                                //notification operation here
                                                                NotificationProcessing(customerVehicleInfo, customerAccountInfo, transaction);
                                                                LogMessage("Notification processing has been done.");
                                                            }
                                                            else
                                                            {
                                                                LogMessage("Balance is already updated.");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            LogMessage("Tag class and NF class does not match. Transaction will be marked as violation.");
                                                            //mark the transaction as violation and leave it for manual review
                                                            VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.MarkAsViolation(associatedCrossTalkTrans[0]);
                                                            LogMessage("Transaction is marked as violation.");
                                                        }
                                                    }
                                                    #endregion


                                                }
                                                else
                                                {
                                                    LogMessage("Abnormal case: Multiple entries found in the transaction table for this nodeflux packet in the specified time window (1 minute).");
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                //no associated crosstalk transaction found
                                                #region Create/ Update a transaction Main transaction table 
                                                try
                                                {
                                                    LogMessage("VRN exists but no associated crosstalk transaction found in transaction table. Checking associated nodeflux transaction...");

                                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection associatedNodeFluxTrans = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetCorrespondingTransactionInNodeFlux(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber);

                                                    if (associatedNodeFluxTrans.Count > 0)
                                                    {
                                                        if (associatedNodeFluxTrans.Count == 1)
                                                        {
                                                            LogMessage("Transaction found to update...");
                                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = associatedNodeFluxTrans[0];

                                                            if (nfp.CameraPosition == "1") //1 means front, 2 means rear
                                                            {
                                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPFront(transaction, nfpEntryId);
                                                                LogMessage("nf entry id front has been updated in transaction.");
                                                            }
                                                            else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
                                                            {
                                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPRear(transaction, nfpEntryId);
                                                                LogMessage("nf entry id rear has been updated in transaction.");
                                                            }
                                                            else
                                                            {
                                                                LogMessage("Invalid camera position. " + nfp.CameraPosition);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            LogMessage("Abnormal case.");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (nfp.CameraPosition == "1") //1 means front, 2 means rear
                                                        {
                                                            VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPFront(nfp, nfpEntryId);
                                                            LogMessage("Transaction inserted by nf entry id front.");
                                                        }
                                                        else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
                                                        {
                                                            VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPRear(nfp, nfpEntryId);
                                                            LogMessage("Transaction inserted by nf entry id rear.");
                                                        }
                                                        else
                                                        {
                                                            LogMessage("Invalid camera position. " + nfp.CameraPosition);
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogMessage("Exception in inserting/updating in main transaction table. " + ex.ToString());
                                                }
                                                #endregion
                                            }
                                        }
                                        else
                                        {
                                            //***if exists in tbl_transaction updat it, otherwise insert***//
                                            //the vrn does not exists two cases may arrise
                                            //A. The vrn is a erroneous detection by nodeflux 
                                            //B. the vehicle is new and not exist in our customervehicle list
                                            #region Insert into main transaction table 
                                            try
                                            {
                                                if (nfp.CameraPosition == "1") // 1 means front, 2 means rear
                                                {
                                                    LogMessage("Inserting NFP (front) into main transaction table...");
                                                    VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPFront(nfp, nfpEntryId);
                                                    LogMessage("Record inserted successfully.");

                                                    //we cannot do financial operation as because this VRN does not exist in the system, will be reviewed in manual review
                                                }
                                                else if (nfp.CameraPosition == "2") // 1 means front, 2 means rear
                                                {
                                                    LogMessage("Inserting NFP (rear) into main transaction table...");
                                                    VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPRear(nfp, nfpEntryId);
                                                    LogMessage("Record inserted successfully.");

                                                    //we cannot do financial operation as because this VRN does not exist in the system, will be reviewed in manual review
                                                }
                                                else
                                                {
                                                    LogMessage("Invalid camera position. " + nfp.CameraPosition);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                LogMessage("Failed to insert nodeflux packet in main transaction table." + ex.Message);
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        LogMessage("Repeated nodeflux reporting. Discarded. Plate Number is: " + nfp.PlateNumber);
                                    }
                                }
                                else
                                {
                                    LogMessage("nfp is null.");
                                }
                            }
                            else
                            {
                                LogMessage("Payload is not of nodeflux packet type.");
                            }
                        }
                        else
                        {
                            LogMessage("Nodeflux packet's payload is null.");
                        }
                        #endregion                       
                    }
                    else
                    {
                        LogMessage("Current object is not valid packet. " + m.Body.ToString());
                    }
                    #endregion
                }
                else
                {
                    LogMessage("Message body is null.");
                }
            }
            else
            {
                LogMessage("Message is null.");
            }
        }

        private void FinancialProcessing(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicleInfo, VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccountInfo, VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            //All these to be converted into a ORACLE transaction -- CJS
            //calculation of deduct amount
            #region LaneType and TollRate Section
            decimal tollToDeduct = -1;
            try
            {
                LogMessage("Finding out LaneType and toll rate to deduct...");
                int laneTypeId = GetLaneTypeByLaneId(transaction.LaneId);
                LogMessage("LaneType is: " + laneTypeId);
                tollToDeduct = GetTollRate(currentTMSId, laneTypeId, transaction.TransactionDateTime, customerVehicleInfo.VehicleClassId);
                LogMessage("Toll to deduct is (for motorcycle it may be 0.00): " + tollToDeduct);
            }
            catch (Exception ex)
            {
                LogMessage("Exception in finding out lane type and toll to deduct. " + ex.ToString());
                tollToDeduct = -1;
            }
            #endregion

            if (tollToDeduct > -1)
            {
                #region Account History Section
                try
                {
                    LogMessage("Trying to record in account history table...");
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.AccountHistoryCBE accountHistory = new Libraries.CommonLibrary.CBE.AccountHistoryCBE();
                    accountHistory.TMSId = transaction.TMSId;
                    //accountHistory.EntryId = 0;//this  is the auto incremented and primery key of table
                    accountHistory.AccountId = customerAccountInfo.AccountId;
                    accountHistory.CustomerVehicleEntryId = customerVehicleInfo.EntryId; //<============================= 
                    accountHistory.TransactionTypeId = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransactionType.LaneDebit;
                    accountHistory.TransactionId = transaction.TransactionId;
                    accountHistory.Amount = tollToDeduct;
                    accountHistory.IsSMSSent = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent; //will be updated later on
                    accountHistory.IsEmailSent = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.EmailSentStatus.Unsent; ;//will be updated later on
                                                                                                                            //accountHistory.ModifierId = 1;//will be updated later on
                    accountHistory.CreationDate = DateTime.Now;
                    accountHistory.ModificationDate = DateTime.Now;
                    accountHistory.TransferStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred;
                    VaaaN.MLFF.Libraries.CommonLibrary.BLL.AccountHistoryBLL.Insert(accountHistory);
                    LogMessage("Recorded in account history table successfully.");
                }
                catch (Exception ex)
                {
                    LogMessage("Exception in recording in the Account History table. " + ex.ToString());
                }
                #endregion

                #region Update Balance Section
                try
                {
                    LogMessage("Trying to update balance in customer account table...");
                    //should be by by trigger defined in TBL_ACCOUNT_HISTORY
                    VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.UpdateBalance(customerAccountInfo, (-1 * tollToDeduct));
                    LogMessage("Balance updated successfully in the customer account.");
                }
                catch (Exception ex)
                {
                    LogMessage("Exception in updating customer's account balance. " + ex.ToString());
                }
                #endregion

                #region Mark transaction as balance updated
                try
                {
                    LogMessage("Trying to update isBalanceUpdated field in transaction table...");
                    VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.MarkAsBalanceUpdated(transaction);
                    LogMessage("Transaction is marked as balance updated.");
                }
                catch (Exception ex)
                {
                    LogMessage("Exception in marking the transaction as balance updated. " + ex.ToString());
                }
                #endregion
            }
            else
            {
                LogMessage("Toll to deduct is -1.00. There is some error somewhere.");
            }
        }

        private void NotificationProcessing(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicleInfo, VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccountInfo, VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction)
        {
            try
            {
                LogMessage("Trying to push SMS to MSMQ...");
                Message smsMessage = new Message();
                smsMessage.Formatter = new BinaryMessageFormatter();
                VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail smsDetail = new Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail();
                //smsDetail.SMSMessage = "Your account has been deducted for Vehicle " + customerVehicleInfo.VehRegNo + " at Location: " + transaction.PlazaId + " at Time: " + transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.DATETIME_FORMAT);
                //Akun anda telah dipotong untuk bertransaksi nomor kendaraan hr36k3032 anda di tempat gantry 1 pada 13 / 10 / 2018 5: 50: 30 pm.
                smsDetail.SMSMessage = "Akun anda telah dipotong untuk bertransaksi nomor kendaraan " + customerVehicleInfo.VehRegNo + " anda di tempat " + GetPlazaNameById(transaction.PlazaId) + " pada " + transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.DATETIME_FORMAT) + ".";
                LogMessage(smsDetail.SMSMessage);
                smsDetail.AccountId = customerAccountInfo.AccountId;
                smsDetail.CustomerName = customerAccountInfo.FirstName + " " + customerAccountInfo.LastName;
                smsDetail.SenderMobileNumber = customerAccountInfo.MobileNo;

                smsMessage.Body = smsDetail;
                smsMessageQueue.Send(smsMessage);
            }
            catch (Exception ex)
            {
                LogMessage("Exception in pushing SMS to SMS MSMQ. " + ex.ToString());
            }
        }

        private int GetVehicleClassIdByTagId(string tagId)
        {
            int result = -1;

            try
            {
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE cv in customerVehicles)
                {
                    if (cv.TagId.ToLower() == tagId.ToLower())
                    {
                        result = cv.VehicleClassId;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }

        private string GetVRNByTagId(string tagId)
        {
            string result = string.Empty;

            try
            {
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE cv in customerVehicles)
                {
                    if (cv.TagId.ToLower() == tagId.ToLower())
                    {
                        result = cv.VehRegNo;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Empty;
            }

            return result;
        }

        private VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE DoesVRNExist(string vrn)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE result = null;

            try
            {
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE cvc in customerVehicles)
                {
                    if (cvc.VehRegNo.ToLower() == vrn.ToLower())
                    {
                        result = cvc;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception in DoesVRNExist() function. " + ex.ToString());
            }

            return result;
        }

        private VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE DoesTagExist(string tagId)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE result = null;

            try
            {
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE cvc in customerVehicles)
                {
                    if (cvc.TagId.ToLower() == tagId.ToLower())
                    {
                        result = cvc;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception in DoesVRNExist() function. " + ex.ToString());
            }

            return result;
        }

        private bool DoesExistInRecentCrossTalkPackets(Int32 plazaId, string tagId, DateTime tagReportingTime)
        {
            bool result = false;
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCollection ctPackets = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CrossTalkBLL.GetRecent(plazaId, tagId, tagReportingTime);

                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE ctp in ctPackets)
                {
                    if (ctp.ObjectId.ToLower() == tagId.ToLower())
                    {
                        result = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception in finding recent crosstalk packets." + ex.Message);
                result = false;
            }
            return result;
        }

        private bool DoesExistInRecentNodeFluxPackets(Int32 plazaId, string vrn, DateTime nodeFluxReportingTime, int cameraPosition)
        {
            bool result = false;
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection nfPackets = VaaaN.MLFF.Libraries.CommonLibrary.BLL.NodeFluxBLL.GetRecent(plazaId, vrn, nodeFluxReportingTime, cameraPosition);

                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nfp in nfPackets)
                {
                    if (nfp.PlateNumber.ToLower() == vrn.ToLower())
                    {
                        result = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception in finding recent nodeflux packets." + ex.Message);
                result = false;
            }
            return result;
        }

        private decimal GetTollRate(int plazaId, int laneType, DateTime transactionTime, int vehicleClass)
        {
            decimal result = -1;

            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection currentTimeTollRates = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection();

                DateTime currentStartDate = new DateTime();
                DateTime currentEndDate = new DateTime();
                DateTime actualEndDate = new DateTime(); //CJS

                // Get toll rate as per transaction time
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE tr in tollRates)
                {
                    DateTime currentDate = transactionTime;

                    // Get Start hour and minute
                    int startHour = Convert.ToInt32(tr.StartTime.Substring(0, 2));
                    int startMinute = Convert.ToInt32(tr.StartTime.Substring(3, 2));

                    int endHour = Convert.ToInt32(tr.EndTime.Substring(0, 2));
                    int endMinute = Convert.ToInt32(tr.EndTime.Substring(3, 2));

                    Console.WriteLine(startHour + ", " + startMinute + " -> " + endHour + ", " + endMinute);

                    currentStartDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, startHour, startMinute, currentDate.Second);
                    currentEndDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, endHour, endMinute, currentDate.Second);

                    if (startHour > endHour)// Cross day
                    {
                        actualEndDate = currentEndDate.AddDays(1); //this value need to be assigned to another vehicle CJS
                    }
                    else
                    {
                        actualEndDate = currentEndDate; //CJS
                    }

                    if (currentDate > currentStartDate && currentDate < actualEndDate)
                    {
                        currentTimeTollRates.Add(tr);
                    }
                }

                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE tr in currentTimeTollRates)
                {
                    if (tr.PlazaId == plazaId && tr.LaneTypeId == laneType && tr.VehicleClassId == vehicleClass)
                    {
                        result = tr.Amount;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Failed to get toll rate." + ex.Message);
                result = -1;
            }

            return result;
        }

        private int GetLaneTypeByLaneId(int laneId)
        {
            int result = -1;

            try
            {
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane in lanes)
                {
                    if (lane.LaneId == laneId)
                    {
                        result = lane.LaneTypeId;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }

        private VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE GetHardwareById(int hardwareId)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE result = null;

            try
            {
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE hardware in hardwares)
                {
                    if (hardware.HardwareId == hardwareId)
                    {
                        result = hardware;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        private VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE GetLaneDetailByHardwareId(int hardwareId)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE result = null;
            try
            {
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane in lanes)
                {
                    if ((lane.CameraIdFront == hardwareId) || (lane.CameraIdRear == hardwareId) || (lane.AntennaIdFront == hardwareId) || (lane.AntennaIdRear == hardwareId))
                    {
                        result = lane; //it should be lane module business logic that one assigned hardware id cannot be assigned to other lane.
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception is GetLaneDetailByHardwareId() function. " + ex.ToString());
            }

            return result;
        }

        private VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE GetLaneById(int laneId)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE result = null;
            try
            {
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane in lanes)
                {
                    if (lane.LaneId == laneId)
                    {
                        result = lane;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception is GetLaneById() function. " + ex.ToString());
            }

            return result;
        }

        private string GetLaneNameById(int laneId)
        {
            string result = string.Empty;
            try
            {
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane in lanes)
                {
                    if (lane.LaneId == laneId)
                    {
                        result = lane.LaneName;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception is GetLaneById() function. " + ex.ToString());
                result = string.Empty;
            }

            return result;
        }

        private VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE GetCustomerAccountById(Int32 accountId)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE result = null;
            try
            {
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE ca in customerAccounts)
                {
                    if (ca.AccountId == accountId)
                    {
                        result = ca;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception is GetCustomerAccountById() function. " + ex.ToString());
            }

            return result;
        }

        private VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE GetVehicleClassByName(string vehicleClassName)
        {
            LogMessage("Getting vehicle class by name...");
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE result = null;

            try
            {
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicleClasses)
                {
                    if (vc.Name.ToLower() == vehicleClassName.ToLower())
                    {
                        result = vc;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        private string GetPlazaNameById(int plazaId)
        {
            string result = string.Empty;

            try
            {
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE plaza in plazas)
                {
                    if (plaza.PlazaId == plazaId)
                    {
                        result = plaza.PlazaName;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception in getting plaza name for plazaId " + plazaId);
                result = string.Empty;
            }

            return result;
        }



        private TagStructure ParseEPC(string epcInput)
        {
            //string epc = "024231323358595AFC730000";
            string epc = epcInput.Trim();

            TagStructure result = null;

            try
            {
                int classId = Convert.ToInt32(epc.Substring(0, 2));


                string vrnPart = epc.Substring(2);
                string temp1 = string.Empty;
                List<byte> bytes = new List<byte>();

                for (Int32 i = 0; i < vrnPart.Length; i = i + 2)
                {
                    temp1 = vrnPart[i].ToString() + vrnPart[i + 1].ToString();
                    if (temp1 == "FC")
                    {
                        break;
                    }
                    else
                    {
                        bytes.Add(Convert.ToByte(temp1, 16));
                    }
                }

                string vrn = System.Text.Encoding.ASCII.GetString(bytes.ToArray());

                result = new TagStructure();
                result.ClassId = classId;
                result.VRN = vrn;

                //MessageBox.Show(vrn);
            }
            catch (Exception ex)
            {
                result = null;
                LogMessage("Error in parsing EPC string. epcString = " + epc);
            }
            return result;
        }

        //private void LiveQueueUpdate(int gantryId, DateTime counterStartTime, int mcCount, int smallCount, int mediumCount, int bigCount)
        //{
        //    try
        //    {
        //        LogMessage("Trying to update live queue...");

        //        if (liveCountQueue.)
        //        {
        //            Message m = (Message)liveCountQueue.Receive();
        //            m.Formatter = new BinaryMessageFormatter();
        //        }

        //        LogMessage("Live queue has been updated successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        LogMessage("Exception in updating live queue. " + ex.ToString());

        //    }
        //}

        #endregion

        #region Convert Timestamp to date time
        private string ConversionDateTime(string timestamp, string SourceFilter)
        {
            try
            {
                if (!string.IsNullOrEmpty(timestamp))
                {
                    if (SourceFilter.ToLower() == "crosstalk")
                    {
                        double Dtimestamp = Convert.ToDouble(timestamp);
                        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Dtimestamp / 1000d)).ToLocalTime();
                        return dt.ToString(Libraries.CommonLibrary.Constants.dateTimeFormat24H);
                    }
                    else if (SourceFilter.ToLower() == "nodeflux")
                    {
                        return Convert.ToDateTime(timestamp).ToString(Libraries.CommonLibrary.Constants.dateTimeFormat24H);

                    }
                    else
                    {
                        return timestamp;
                    }
                }
                else
                {
                    return timestamp;
                }
            }
            catch (Exception)
            {

                return timestamp;
            }
        }

        private Int32 DateTimeToUnixTimestamp(DateTime dt)
        {
            Int32 result = 0;

            result = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            return result;
        }
        #endregion

        #region Service Logger
        private void LogMessage(String message)
        {
            //logQueue.Enqueue(Environment.NewLine + DateTime.Now.ToString("hh:mm:ss.FFFF tt") + ": " + message);
            logQueue.Enqueue(Environment.NewLine + message);
        }

        private void LoggerThreadFunction()
        {
            stopLoggerThread = false;

            while (!stopLoggerThread)
            {
                try
                {
                    WriteMessageFromQueue();
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    Thread.Sleep(50);
                }
            }
        }

        private void WriteMessageFromQueue()
        {
            if (logQueue.Count > 0)
            {
                String message = (String)logQueue.Dequeue();

                VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(message, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.LDS);
            }
        }
        #endregion
    }

    public class TagStructure
    {
        int classId = -1;
        string vrn = string.Empty;

        public Int32 ClassId
        {
            get
            {
                return this.classId;
            }
            set
            {
                this.classId = value;
            }
        }

        public String VRN
        {
            get
            {
                return this.vrn;
            }
            set
            {
                this.vrn = value;
            }
        }
    }




}

