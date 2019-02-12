using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using System.Messaging;
using System.Collections;
using System.Threading;
using System.Globalization;
using VaaaN.MLFF.Libraries.CommonLibrary;

namespace VaaaN.MLFF.WindowsServices
{
    public partial class MainService : ServiceBase
    {
        #region Variables
        private Queue logQueue = new Queue();
        private Thread loggerThread;
        private volatile Boolean stopLoggerThread = false;

        DateTime lastCollectionUpdateTime = DateTime.MinValue;

        //convert these collections into Dictionary<int id, class> in future for performance - CJS
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection lanes;
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection hardwares;
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCollection plazas;
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection customerAccounts;
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection customerVehicles;
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection tollRates;
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection vehicleClasses;

        private MessageQueue failedQueue;

        private MessageQueue inBoxQueue;

        private MessageQueue smsMessageQueue;
        private MessageQueue eventQueue;

        int ctpEntryId = 0;
        int nfpEntryId = 0;
        int currentTMSId = -1;
        Double TotalDistance = 30;
        int Minutes = 2;

        DateTime countStartTime = DateTime.MinValue;
        int motorCycleCount = 0;
        int smallCount = 0;
        int mediumCount = 0;
        int bigCount = 0;
        string counterString = string.Empty;
        string ActiveAnpr = string.Empty;

        List<IkePktData> rfidRecentDataList = new List<IkePktData>();
        List<ANPRPktData> anprRecentDataList = new List<ANPRPktData>();
        List<TranscationData> transcationDataList = new List<TranscationData>();

        List<TranscationData> filteredTransactionList = new List<TranscationData>();
        List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE> TollRateFilteredList = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE>();

        VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.GeneralConfiguration generalFileConfig;

        Thread collectionUpdaterThread;

        volatile Boolean stopCollectionUpdatingThread = false;
        #endregion

        #region Constructor
        public MainService()
        {
            InitializeComponent();

            //dont forget to comment this line
            //OnStart(new string[] { "sd" }); //<===================================================================== only for debugging
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
            try
            {
                generalFileConfig = VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.GeneralConfiguration.Deserialize();
                TotalDistance = Convert.ToDouble(generalFileConfig.Distance);
            }
            catch (Exception ex)
            {
                LogMessage("Unable to get distance from : " + ex);
            }
            currentTMSId = VaaaN.MLFF.Libraries.CommonLibrary.Constants.GetCurrentTMSId();


            countStartTime = System.DateTime.Now;
            motorCycleCount = 0;
            smallCount = 0;
            mediumCount = 0;
            bigCount = 0;

            try
            {
                LogMessage("Starting LDS logger thread...");

                loggerThread = new Thread(new ThreadStart(this.LoggerThreadFunction));
                loggerThread.IsBackground = true;
                loggerThread.Start();

                LogMessage("The LDS logger has been started.");
            }
            catch (Exception ex)
            {
                LogMessage("Error in starting LDS logger thread function. LDS cannot be started. " + ex.ToString());
            }

            try
            {
                LogMessage("Getting collections of plazas, lanes, hardwares...");

                lanes = VaaaN.MLFF.Libraries.CommonLibrary.BLL.LaneBLL.GetAll();
                plazas = VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.GetAllAsCollection();
                hardwares = VaaaN.MLFF.Libraries.CommonLibrary.BLL.HardwareBLL.GetAll();
                customerAccounts = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsCollection();
                customerVehicles = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetAllAsCollection();
                vehicleClasses = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAllAsCollection();
                tollRates = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TollRateBLL.GetAll();
                ActiveAnpr = VaaaN.MLFF.Libraries.CommonLibrary.BLL.HardwareBLL.GetActiveANPR();
                lastCollectionUpdateTime = DateTime.Now;

                LogMessage("Required collections has been accessed.");
            }
            catch (Exception ex)
            {
                LogMessage("Exception in accessing collections. " + ex.ToString());
            }

            try
            {
                LogMessage("Starting collection updater thread...");

                collectionUpdaterThread = new Thread(new ThreadStart(this.CollectionUpdatingThreadFunction));
                collectionUpdaterThread.IsBackground = true;
                collectionUpdaterThread.Start();

                LogMessage("Collection updater thread has been started.");

            }
            catch (Exception ex)
            {
                LogMessage("Exception in starting collection updater thread. " + ex.ToString());
            }

            try
            {
                LogMessage("Creating reference for required MSMMQs...");

                this.smsMessageQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.smsMessageQueue);
                this.eventQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.eventQueue);
                this.failedQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.failedQueueName);

                LogMessage("Reference for MSMQs has been created successfully.");
            }
            catch (Exception ex)
            {
                LogMessage("Failed to create reference for SMS queue. " + ex.Message);
            }

            try
            {
                LogMessage("Trying to start LDS...");


                #region Inbox Queue
                try
                {
                    LogMessage("Attaching listener to inbox queue...");

                    this.inBoxQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.inBoxQueueName);
                    inBoxQueue.PeekCompleted += new PeekCompletedEventHandler(InBoxQueue_PeekCompleted);
                    inBoxQueue.BeginPeek();

                    LogMessage("Inbox queue listener has been attached.");
                }
                catch (Exception)
                {
                    throw;
                }
                #endregion

                LogMessage("LDS service started successfully.");
            }
            catch (Exception ex)
            {
                LogMessage("Failed to start LDS. " + ex.Message);
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

            }

            try
            {
                stopCollectionUpdatingThread = true;
                Thread.Sleep(2000);
                if (collectionUpdaterThread != null)
                {
                    if (collectionUpdaterThread.IsAlive == true)
                    {
                        collectionUpdaterThread.Abort();
                    }
                }
            }
            catch (Exception ex)
            {

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

            base.OnStop();
        }


        void InBoxQueue_PeekCompleted(object sender, PeekCompletedEventArgs e)
        {
            MessageQueue mq = (MessageQueue)sender;

            try
            {
                Message m = (Message)mq.EndPeek(e.AsyncResult);
                m.Formatter = new BinaryMessageFormatter();

                //LogMessage("Going to process InBoxQueue message...");

                ProcessQueueMessage(m);

            }
            catch (Exception ex)
            {
                LogMessage("Error in peeking inbox queue. " + ex.ToString());
            }
            finally
            {
                mq.Receive();
                inBoxQueue.BeginPeek();
            }
        }
        #endregion

        #region Helper Methods


        int packetCounter = 0;
        private void ProcessQueueMessage(Message m)
        {
            if (m != null)
            {
                m.Formatter = new BinaryMessageFormatter();

                //delete old records from lists------------------------
                packetCounter = packetCounter + 1;
                if (packetCounter > 100)
                {
                    //keep only last 3 minutes data
                    rfidRecentDataList.RemoveAll(e => e.currentDateTime < DateTime.Now.AddMinutes(-3));
                    anprRecentDataList.RemoveAll(e => e.currentDateTime < DateTime.Now.AddMinutes(-3));
                    transcationDataList.RemoveAll(e => e.CurrentDateTime < DateTime.Now.AddMinutes(-3));
                    packetCounter = 0;
                }
                //-----------------------------------------------------

                if (m.Body != null)
                {
                    #region Processing packets
                    if (m.Body is VaaaN.MLFF.Libraries.CommonLibrary.Common.CrossTalkPacket)
                    {
                        #region CrossTalk packet
                        LogMessage("==RFID==");

                        VaaaN.MLFF.Libraries.CommonLibrary.Common.CrossTalkPacket crossTalkPacket = (VaaaN.MLFF.Libraries.CommonLibrary.Common.CrossTalkPacket)m.Body;

                        if (crossTalkPacket.Payload != null)
                        {
                            if (crossTalkPacket.Payload is VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE)
                            {
                                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE ctp = (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE)crossTalkPacket.Payload;

                                ctp.ObjectId = ctp.ObjectId.Trim(); //otherwise trailing and leading spaces create problems

                                LogMessage("Validity checking. tag:" + ctp.ObjectId);

                                #region Check EPC exists or not in DB
                                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE associatedCVCT = DoesTagExist(ctp.ObjectId);
                                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE associatedCACT = null;
                                if (associatedCVCT != null)
                                {
                                    associatedCACT = GetCustomerAccountById(associatedCVCT.AccountId);
                                    LogMessage("The associated accoount is: " + associatedCACT.FirstName + " tag: " + ctp.ObjectId);

                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane = null;
                                    #region Update some fields in the CBE
                                    try
                                    {
                                        ctp.TMSId = currentTMSId;
                                        lane = GetLaneDetailByHardwareId(Convert.ToInt32(ctp.LocationId)); //locationid is hardwareid? (satya said)
                                        if (lane != null)
                                        {
                                            ctp.PlazaId = lane.PlazaId; //<== need to update as per device location
                                            ctp.LaneId = lane.LaneId; //<== need to update as per device location
                                        }
                                        else
                                        {
                                            LogMessage("No lane detail found against the hardware id: " + ctp.LocationId + " tag: " + ctp.ObjectId);
                                        }
                                        ctp.PlateNumber = associatedCVCT.VehRegNo;
                                        ctp.VehicleClassId = associatedCVCT.VehicleClassId;
                                        ctp.CreationDate = System.DateTime.Now;
                                        ctp.ModifierId = 1;
                                        ctp.ModificationDate = System.DateTime.Now;
                                        try
                                        {
                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE reader = GetHardwareById(Convert.ToInt32(ctp.LocationId));
                                            ctp.ReaderPosition = Convert.ToInt32(reader.HardwarePosition.ToString());
                                        }
                                        catch (Exception)
                                        {
                                            ctp.ReaderPosition = 1;
                                        }


                                    }
                                    catch (Exception ex)
                                    {
                                        LogMessage("Exception in updating some fields. " + ex.ToString() + " tag: " + ctp.ObjectId);
                                    }
                                    #endregion

                                    #region Insert into event queue

                                    try
                                    {
                                        Message crosstalkEventMessage = new Message();
                                        crosstalkEventMessage.Formatter = new BinaryMessageFormatter();
                                        crosstalkEventMessage.TimeToBeReceived = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.eventQueueTimeOut;

                                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkEvent ctEvent = new Libraries.CommonLibrary.CBE.CrossTalkEvent();

                                        ctEvent.Timestamp = Convert.ToDateTime(ctp.TimeStamp);
                                        ctEvent.TagId = ctp.ObjectId;
                                        if (lane != null)
                                        {
                                            ctEvent.PlazaId = ctp.PlazaId;
                                            ctEvent.PlazaName = GetPlazaNameById(ctp.PlazaId);
                                            ctEvent.LaneId = ctp.LaneId;
                                            ctEvent.LaneName = GetLaneNameById(ctp.LaneId);

                                        }
                                        else
                                        {
                                            ctEvent.PlazaId = 0;
                                            ctEvent.PlazaName = "NA";
                                            ctEvent.LaneId = 0;
                                            ctEvent.LaneName = "NA";
                                        }
                                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE VehicleClass = GetVehicleClassById(associatedCVCT.VehicleClassId);
                                        if (VehicleClass != null)
                                        {
                                            ctEvent.VehicleClassName = VehicleClass.Name;
                                        }
                                        else
                                        {
                                            LogMessage("Vehicle class name might be wrong in IKE packet. (1, 2, 3, 4)" + associatedCVCT.VehicleClassId + " tag: " + ctp.ObjectId);
                                        }
                                        ctEvent.ReaderPosition = ctp.ReaderPosition;
                                        ctEvent.VRN = associatedCVCT.VehRegNo;

                                        crosstalkEventMessage.Body = ctEvent;

                                        eventQueue.Send(crosstalkEventMessage);
                                        // LogMessage("Crosstalk event pushed to event queue successfully.");
                                    }
                                    catch (Exception ex)
                                    {
                                        LogMessage("Exception in pushing crosstalk event to event queue. " + ex.ToString() + " tag: " + ctp.ObjectId);
                                    }

                                    #endregion

                                    //check most recent transactions of the same plaza, same tag, same hardware id (location id)
                                    #region Check in recent crosstalk packets
                                    if (!DoesExistInRecentCrossTalkPackets(ctp.PlazaId, ctp.ObjectId, Convert.ToDateTime(ctp.TimeStamp), ctp.LocationId, ctp.ReaderPosition))
                                    {
                                        #region Send to local database 
                                        try
                                        {
                                            ctpEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CrossTalkBLL.Insert(ctp);
                                            rfidRecentDataList.Add(new IkePktData { LocationId = ctp.LocationId, PktId = ctpEntryId, PacketTimeStamp = Convert.ToDateTime(ctp.TimeStamp), VehicleClassId = associatedCVCT.VehicleClassId, ObjectId = ctp.ObjectId, PlazaId = ctp.PlazaId, currentDateTime = DateTime.Now, ReaderPosition = ctp.ReaderPosition });

                                            LogMessage("Crosstalk packet inserted successfully. tag: " + ctp.ObjectId + " Location: " + ctp.LocationId + " at CTid :" + ctpEntryId.ToString());
                                        }
                                        catch (Exception ex)
                                        {
                                            #region Send data to failed queue
                                            try
                                            {
                                                m.Recoverable = true;
                                                failedQueue.Send(m);
                                                LogMessage("Message sent to failed queue." + ex.ToString() + " tag: " + ctp.ObjectId);
                                            }
                                            catch (Exception exc)
                                            {
                                                LogMessage("***DATA LOST*** Failed to send to failed queue. Crosstalk packet Transaction is " + ctp.ToString() + exc.ToString());
                                            }
                                            #endregion
                                        }
                                        #endregion

                                        #region Procession for Transcation
                                        //is the associated VRN is already inserted in the transaction table by nodeflux front or nodeflux rear camera?
                                        //if inserted look by associated vrn and update, if not create a new transaction
                                        // LogMessage("Searching associated transaction exists in the transaction table or not...");
                                        DateTime ctpDateTime = Convert.ToDateTime(ctp.TimeStamp);
                                        LogMessage("Search criteria: " + ctp.TMSId + ", " + ctp.PlazaId + ", " + ctpDateTime.ToString("dd/MM/yyyy hh:mm:ss.fff tt") + ", " + associatedCVCT.VehRegNo);
                                        filteredTransactionList = GetAssociatedTransactions(ctp.TMSId, ctp.PlazaId, ctpDateTime, associatedCVCT.VehRegNo, ctp.VehicleClassId, "ANPR");
                                        if (filteredTransactionList.Count > 0)
                                        {
                                            if (filteredTransactionList.Count == 1)
                                            {
                                                #region Update in main transaction table
                                                try
                                                {
                                                    #region Update CTP section in main transaction table
                                                    //LogMessage("Updating in main transaction table...");
                                                    TranscationData Filtertransaction = filteredTransactionList[0];
                                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
                                                    transaction.CrosstalkEntryIdFront = Convert.ToInt32(Filtertransaction.IKEFId);
                                                    transaction.CrosstalkEntryIdRear = Convert.ToInt32(Filtertransaction.IKERId);
                                                    transaction.NodefluxEntryIdFront = Convert.ToInt32(Filtertransaction.AnprFId);
                                                    transaction.NodefluxEntryIdRear = Convert.ToInt32(Filtertransaction.AnprRId);
                                                    transaction.TMSId = Filtertransaction.TMSId;
                                                    transaction.PlazaId = Filtertransaction.PlazaId;
                                                    transaction.LaneId = Filtertransaction.LaneId;
                                                    transaction.TransactionId = Filtertransaction.TranscationId;
                                                    transaction.TransactionDateTime = Filtertransaction.TransactionDateTime;
                                                    if (ctp.ReaderPosition == 1)
                                                    {
                                                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateCrossTalkSection(transaction, ctpEntryId);//, eviVehicleClassId, eviVRN);
                                                        var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                        if (obj != null)
                                                        {
                                                            obj.IKEFId = ctpEntryId;
                                                            obj.IKEFrontVehicleClassId = ctp.VehicleClassId;
                                                        }

                                                        #region Get vehicle class matched to VRN front and rear

                                                        Int32 vehicleClassIdFront = -1;
                                                        Int32 vehicleClassIdRear = -1;
                                                        if (transaction.NodefluxEntryIdFront > 0)
                                                        {

                                                            List<ANPRPktData> ANPRPktDataDetails = anprRecentDataList.Where(trans => (trans.PktId == transaction.NodefluxEntryIdFront)).OrderBy(o => o.PacketTimeStamp).ToList();
                                                            if (ANPRPktDataDetails.Count > 0)
                                                            {
                                                                vehicleClassIdFront = ANPRPktDataDetails[0].VehicleClassId;
                                                            }
                                                        }
                                                        if (transaction.NodefluxEntryIdRear > 0)
                                                        {

                                                            List<ANPRPktData> ANPRPktDataDetails = anprRecentDataList.Where(trans => (trans.PktId == transaction.NodefluxEntryIdRear)).OrderBy(o => o.PacketTimeStamp).ToList();
                                                            if (ANPRPktDataDetails.Count > 0)
                                                            {
                                                                vehicleClassIdRear = ANPRPktDataDetails[0].VehicleClassId;
                                                            }
                                                        }
                                                        #endregion

                                                        //does the EVI class and AVC class matched? if not,  mark it as violation and leave it for manual review.
                                                        //else deduct the balance
                                                        #region Charging and SMS
                                                        //if anyone is matched, do the financial operation, no double charging
                                                        // if IKE Pakect vehcile class not matched with ANPR and Dataabse than balance never deducated and mark Violation
                                                        if (associatedCVCT.QueueStatus == 3 && ctp.VehicleClassId == associatedCVCT.VehicleClassId && (ctp.VehicleClassId == vehicleClassIdFront) || (ctp.VehicleClassId == vehicleClassIdRear))
                                                        {
                                                            //updated on 26 Dec, 2018 following if removed
                                                            //if (transaction.IsViolation == -1) //0 for normal, 1 for violtion, by default -1 (this means this is not not updated)
                                                            //{ 
                                                            if (transaction.IsBalanceUpdated == -1) //0 for balance not updated, 1 means balance updated
                                                            {
                                                                if (associatedCACT != null)
                                                                {
                                                                    //update the local list
                                                                    if (obj != null)
                                                                    {
                                                                        obj.IsBalanceUpdated = 1;
                                                                    }

                                                                    //do financial transaction
                                                                    FinancialProcessing(associatedCVCT, associatedCACT, transaction);

                                                                    //do notification
                                                                    //NotificationProcessing(associatedCVCT, associatedCACT, transaction);
                                                                }
                                                                else
                                                                {
                                                                    LogMessage("Associated customer account of this tag id is found null.");
                                                                }
                                                            }
                                                            //}
                                                        }
                                                        else
                                                        {
                                                            //update local list
                                                            if (obj != null)
                                                            {
                                                                if (obj.IsBalanceUpdated != 1) //updated on 26 Dec, 2018
                                                                {
                                                                    obj.IsViolation = 1;
                                                                }
                                                            }
                                                            //update in database
                                                            if (transaction.IsBalanceUpdated != 1) //updated on 26 Dec, 2018
                                                            {
                                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.MarkAsViolation(transaction);
                                                                LogMessage("Transaction is marked as violation.");
                                                            }
                                                            //else
                                                            //{
                                                            //    LogMessage("Transaction cannot be marked as violation as balance has been already updated.");
                                                            //}

                                                            //violation vms message
                                                        }
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateCrossTalkSectionRear(transaction, ctpEntryId);//, eviVehicleClassId, eviVRN);
                                                        var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                        if (obj != null)
                                                        {
                                                            obj.IKERId = ctpEntryId;
                                                            obj.IKERearVehicleClassId = ctp.VehicleClassId;
                                                        }
                                                        #region Get vehicle class matched to VRN front and rear

                                                        Int32 vehicleClassIdFront = -1;
                                                        Int32 vehicleClassIdRear = -1;
                                                        if (transaction.NodefluxEntryIdFront > 0)
                                                        {

                                                            List<ANPRPktData> ANPRPktDataDetails = anprRecentDataList.Where(trans => (trans.PktId == transaction.NodefluxEntryIdFront)).OrderBy(o => o.PacketTimeStamp).ToList();
                                                            if (ANPRPktDataDetails.Count > 0)
                                                            {
                                                                vehicleClassIdFront = ANPRPktDataDetails[0].VehicleClassId;
                                                            }
                                                        }
                                                        if (transaction.NodefluxEntryIdRear > 0)
                                                        {

                                                            List<ANPRPktData> ANPRPktDataDetails = anprRecentDataList.Where(trans => (trans.PktId == transaction.NodefluxEntryIdRear)).OrderBy(o => o.PacketTimeStamp).ToList();
                                                            if (ANPRPktDataDetails.Count > 0)
                                                            {
                                                                vehicleClassIdRear = ANPRPktDataDetails[0].VehicleClassId;
                                                            }
                                                        }
                                                        #endregion

                                                        //does the EVI class and AVC class matched? if not,  mark it as violation and leave it for manual review.
                                                        //else deduct the balance
                                                        #region Charging and SMSing
                                                        //if anyone is matched, do the financial operation, no double charging
                                                        // if IKE Pakect vehcile class not matched with ANPR and Dataabse than balance never deducated and mark Violation
                                                        if (associatedCVCT.QueueStatus == 3 && ctp.VehicleClassId == associatedCVCT.VehicleClassId && (ctp.VehicleClassId == vehicleClassIdFront) || (ctp.VehicleClassId == vehicleClassIdRear))
                                                        {
                                                            //updated on 26 Dec, 2018 following if removed
                                                            //if (transaction.IsViolation == -1) //0 for normal, 1 for violtion, by default -1 (this means this is not not updated)
                                                            //{ 
                                                            if (transaction.IsBalanceUpdated == -1) //0 for balance not updated, 1 means balance updated
                                                            {
                                                                if (associatedCACT != null)
                                                                {
                                                                    //update the local list
                                                                    if (obj != null)
                                                                    {
                                                                        obj.IsBalanceUpdated = 1;
                                                                    }

                                                                    //do financial transaction
                                                                    FinancialProcessing(associatedCVCT, associatedCACT, transaction);

                                                                    //do notification
                                                                    //NotificationProcessing(associatedCVCT, associatedCACT, transaction);
                                                                }
                                                                else
                                                                {
                                                                    LogMessage("Associated customer account of this tag id is found null.");
                                                                }
                                                            }
                                                            //}
                                                        }
                                                        else
                                                        {
                                                            //update local list
                                                            if (obj != null)
                                                            {
                                                                if (obj.IsBalanceUpdated != 1) //updated on 26 Dec, 2018
                                                                {
                                                                    obj.IsViolation = 1;
                                                                }
                                                            }
                                                            //update in database
                                                            if (transaction.IsBalanceUpdated != 1) //updated on 26 Dec, 2018
                                                            {
                                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.MarkAsViolation(transaction);
                                                                LogMessage("Transaction is marked as violation.");
                                                            }
                                                            else
                                                            {
                                                                LogMessage("Transaction cannot be marked as violation as balance has been already updated.");
                                                            }

                                                            //violation vms message
                                                        }
                                                        #endregion
                                                    }


                                                    // LogMessage("RFID transcation updated successfully.");
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
                                            filteredTransactionList = GetAssociatedTransactions(ctp.TMSId, ctp.PlazaId, ctpDateTime, associatedCVCT.VehRegNo, ctp.VehicleClassId, "RFID");
                                            if (filteredTransactionList.Count > 0)
                                            {
                                                #region No ANPR Found Check for releted RFID
                                                if (filteredTransactionList.Count == 1)
                                                {
                                                    #region Update in main transaction table
                                                    try
                                                    {
                                                        #region Update CTP section in main transaction table
                                                        //LogMessage("Updating in main transaction table...");
                                                        TranscationData Filtertransaction = filteredTransactionList[0];
                                                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
                                                        transaction.CrosstalkEntryIdFront = Convert.ToInt32(Filtertransaction.IKEFId);
                                                        transaction.CrosstalkEntryIdRear = Convert.ToInt32(Filtertransaction.IKERId);
                                                        transaction.NodefluxEntryIdFront = Convert.ToInt32(Filtertransaction.AnprFId);
                                                        transaction.NodefluxEntryIdRear = Convert.ToInt32(Filtertransaction.AnprRId);
                                                        transaction.TMSId = Filtertransaction.TMSId;
                                                        transaction.PlazaId = Filtertransaction.PlazaId;
                                                        transaction.LaneId = Filtertransaction.LaneId;
                                                        transaction.TransactionId = Filtertransaction.TranscationId;
                                                        transaction.TransactionDateTime = Filtertransaction.TransactionDateTime;
                                                        if (ctp.ReaderPosition == 1)
                                                        {
                                                            VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateCrossTalkSection(transaction, ctpEntryId);//, eviVehicleClassId, eviVRN);
                                                            var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                            if (obj != null)
                                                            {
                                                                obj.IKEFId = ctpEntryId;
                                                                obj.IKEFrontVehicleClassId = ctp.VehicleClassId;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateCrossTalkSectionRear(transaction, ctpEntryId);//, eviVehicleClassId, eviVRN);
                                                            var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                            if (obj != null)
                                                            {
                                                                obj.IKERId = ctpEntryId;
                                                                obj.IKERearVehicleClassId = ctp.VehicleClassId;
                                                            }
                                                        }
                                                        //LogMessage("RFID transcation updated successfully.");
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
                                                #endregion
                                            }
                                            else
                                            {
                                                #region No related data Found Insert into main transaction table
                                                try
                                                {
                                                    // LogMessage("No associated transaction found in transaction table. Inserting into main transaction table...");
                                                    if (ctp.ReaderPosition == 1)
                                                    {
                                                        Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByCTP(ctp, ctpEntryId); //, eviVehicleClassId, eviVRN);
                                                        transcationDataList.Add(new TranscationData { TranscationId = TranscationId, LaneId = ctp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = associatedCVCT.VehRegNo, PlazaId = ctp.PlazaId, IKEFId = ctpEntryId, CameraPosition = 0, TransactionDateTime = Convert.ToDateTime(ctp.TimeStamp), CurrentDateTime = DateTime.Now, IKEFrontVehicleClassId = ctp.VehicleClassId });
                                                    }
                                                    else {
                                                        Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByCTPRear(ctp, ctpEntryId); //, eviVehicleClassId, eviVRN);
                                                        transcationDataList.Add(new TranscationData { TranscationId = TranscationId, LaneId = ctp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = associatedCVCT.VehRegNo, PlazaId = ctp.PlazaId, IKERId = ctpEntryId, CameraPosition = 0, TransactionDateTime = Convert.ToDateTime(ctp.TimeStamp), CurrentDateTime = DateTime.Now, IKERearVehicleClassId = ctp.VehicleClassId });
                                                    }
                                                    //LogMessage("No associated transaction found in transaction table. Inserting into main transaction table, Crosstalk packet inserted successfully.");
                                                    //LogMessage("Crosstalk packet inserted successfully.");
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogMessage("Failed to insert crosstalk packet in main transaction table." + ex.Message);
                                                }
                                                #endregion
                                            }


                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        LogMessage("Repeated crosstalk reporting. Discarded. tag: " + ctp.ObjectId);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    LogMessage("tag: " + ctp.ObjectId + " does not exist in the system. So discarded. Such type of vehicles will be detected by ANPR cameras.");
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
                        LogMessage("==ANPR==");

                        VaaaN.MLFF.Libraries.CommonLibrary.Common.NodeFluxPacket nfpBody = (VaaaN.MLFF.Libraries.CommonLibrary.Common.NodeFluxPacket)m.Body;
                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nfp = null;

                        if (nfpBody.Payload != null)
                        {
                            if (nfpBody.Payload is VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE)
                            {
                                nfp = (VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE)nfpBody.Payload;

                                if (nfp != null)
                                {
                                    nfp.PlateNumber = nfp.PlateNumber.Trim();
                                    LogMessage("Validity checking. tag:" + nfp.PlateNumber);

                                    #region Update some fields in the CBE
                                    try
                                    {
                                        nfp.TMSId = currentTMSId;
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
                                        #region event queue
                                        try
                                        {
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
                                    //LogMessage("Checking existing nodeflux records for: " + nfp.PlateNumber);
                                    if (nfp.PlateNumber.ToLower() == "not detected")
                                    {
                                        #region Send to local nodeflux database
                                        try
                                        {
                                            nfpEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.NodeFluxBLL.Insert(nfp);
                                        }
                                        catch (Exception ex)
                                        {
                                            #region Send data to Failed queue
                                            LogMessage("Failed to insert nodeflux packet. " + ex.Message);

                                            try
                                            {
                                                m.Recoverable = true;
                                                failedQueue.Send(m);
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
                                            if (nfp.CameraPosition == "1") //1 means front, 2 means rear
                                            {
                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPFront(nfp, nfpEntryId, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.VRNRegistred.NotRegistered);
                                            }
                                            else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
                                            {
                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPRear(nfp, nfpEntryId, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.VRNRegistred.NotRegistered);
                                            }
                                            else
                                            {
                                                LogMessage("Invalid camera position. " + nfp.CameraPosition);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            LogMessage("Exception in inserting in main transaction table. " + ex.Message);
                                        }
                                        #endregion
                                    }
                                    else if (!DoesExistInRecentNodeFluxPackets(nfp.GantryId, nfp.PlateNumber, Convert.ToDateTime(nfp.TimeStamp), Convert.ToInt32(nfp.CameraPosition)))
                                    {
                                        #region ANPR Data Found 
                                        #region Send to local nodeflux database
                                        try
                                        {
                                            nfpEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.NodeFluxBLL.Insert(nfp);
                                            anprRecentDataList.Add(new ANPRPktData { VehicleClassId = nfp.VehicleClassId, PktId = nfpEntryId, PacketTimeStamp = Convert.ToDateTime(nfp.TimeStamp), VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, cameraPosition = Convert.ToInt32(nfp.CameraPosition), currentDateTime = DateTime.Now });
                                            LogMessage("NodeFlux packet inserted successfully. VRN " + nfp.PlateNumber + " Id: " + nfpEntryId);
                                        }
                                        catch (Exception ex)
                                        {
                                            #region Send data to Failed queue
                                            LogMessage("Failed to insert nodeflux packet. " + ex.Message);
                                            try
                                            {
                                                m.Recoverable = true;
                                                failedQueue.Send(m);
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
                                        DateTime nfpDateTime = Convert.ToDateTime(nfp.TimeStamp);
                                        if (associatedCVNF != null)
                                        {
                                            #region Checking VRN in recent transactions in tbl_transaction
                                            //LogMessage("Search criteria: " + nfp.TMSId + ", " + nfp.GantryId + ", " + nfpDateTime.ToString("dd/MM/yyyy hh:mm:ss.fff tt") + " " + nfp.PlateNumber);

                                            filteredTransactionList = GetAssociatedTransactions(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber, nfp.VehicleClassId, "RFID");
                                            #endregion

                                            if (filteredTransactionList.Count > 0)
                                            {
                                                #region Complete transaction, balance update and SMS
                                                if (filteredTransactionList.Count == 1)
                                                {
                                                    //LogMessage("Transaction is found to update...");
                                                    // VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = associatedCrossTalkTrans[0];
                                                    TranscationData filteredTransaction = filteredTransactionList[0];
                                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
                                                    transaction.CrosstalkEntryIdFront = Convert.ToInt32(filteredTransaction.IKEFId);
                                                    transaction.CrosstalkEntryIdRear = Convert.ToInt32(filteredTransaction.IKERId);
                                                    transaction.NodefluxEntryIdFront = Convert.ToInt32(filteredTransaction.AnprFId);
                                                    transaction.NodefluxEntryIdRear = Convert.ToInt32(filteredTransaction.AnprRId);
                                                    transaction.TMSId = filteredTransaction.TMSId;
                                                    transaction.PlazaId = filteredTransaction.PlazaId;
                                                    transaction.LaneId = nfp.LaneId;
                                                    transaction.TransactionId = filteredTransaction.TranscationId;
                                                    transaction.TransactionDateTime = filteredTransaction.TransactionDateTime;

                                                    transaction.IsBalanceUpdated = filteredTransaction.IsBalanceUpdated; //26 Dec, 2018

                                                    #region Get customer vehicle and customer account

                                                    //VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicleInfo = DoesTagExist(ObjectId);

                                                    //VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicleInfo = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetByTansactionCrosstalkEntryId(transaction.CrosstalkEntryId);

                                                    //Get customer details of the associated tagid
                                                    //LogMessage("Getting customer details...");
                                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccountInfo = GetCustomerAccountById(associatedCVNF.AccountId);
                                                    #endregion

                                                    #region Existing transaction update
                                                    //in the main transaction table, if found update the nodeflux related fields. this is normal transaction
                                                    if (nfp.CameraPosition == "1") //1 means front, 2 means rear
                                                    {
                                                        if (ActiveAnpr.ToLower() == "hikvision")
                                                        {
                                                            transaction.VehicleSpeed = nfp.VehicleSpeed;
                                                        }
                                                        else
                                                        {
                                                            if (transaction.NodefluxEntryIdRear > 0)
                                                            {
                                                                List<ANPRPktData> VehicleTimeStamp = anprRecentDataList.Where(trans => (trans.PlazaId == transaction.PlazaId && trans.PktId == transaction.NodefluxEntryIdRear)).OrderByDescending(o => o.PacketTimeStamp).ToList();
                                                                if (VehicleTimeStamp.Count > 0)
                                                                {
                                                                    transaction.VehicleSpeed = CalculateSpeed(nfpDateTime, VehicleTimeStamp[0].PacketTimeStamp);
                                                                }
                                                            }
                                                        }
                                                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPFront(transaction, nfpEntryId);
                                                        //VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateNodefluxSectionFront(transaction, nfpEntryId);
                                                        //LogMessage("Transaction updated by nf entry id front.");
                                                        var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == filteredTransaction.TranscationId);
                                                        if (obj != null) obj.AnprFId = nfpEntryId;
                                                    }
                                                    else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
                                                    {
                                                        if (ActiveAnpr.ToLower() == "hikvision")
                                                        {
                                                            transaction.VehicleSpeed = nfp.VehicleSpeed;
                                                        }
                                                        else
                                                        {
                                                            if (transaction.NodefluxEntryIdFront > 0)
                                                            {
                                                                List<ANPRPktData> VehicleTimeStamp = anprRecentDataList.Where(trans => (trans.PlazaId == transaction.PlazaId && trans.PktId == transaction.NodefluxEntryIdFront)).OrderByDescending(o => o.PacketTimeStamp).ToList();
                                                                if (VehicleTimeStamp.Count > 0)
                                                                {
                                                                    transaction.VehicleSpeed = CalculateSpeed(nfpDateTime, VehicleTimeStamp[0].PacketTimeStamp);
                                                                }
                                                            }
                                                        }
                                                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPRear(transaction, nfpEntryId);
                                                        //VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateNodefluxSectionRear(transaction, nfpEntryId);
                                                        var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == filteredTransaction.TranscationId);
                                                        if (obj != null) obj.AnprRId = nfpEntryId;
                                                        //LogMessage("Transaction updated by nf entry id rear.");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Invalid camera position. " + nfp.CameraPosition);
                                                    }
                                                    #endregion

                                                    #region charging and notification
                                                    //if the transaction marked as violation before?
                                                    //26 Dec, 2012 the following if has been removed
                                                    //if (Filtertransaction.IsViolation == -1) //0 for normal, 1 for violtion, by default -1 (not updated)
                                                    //{
                                                    //LogMessage("Transaction is not marked as violation previously. Going to check violation...");
                                                    if (associatedCVNF.QueueStatus == 3 && (filteredTransaction.IKEFrontVehicleClassId == nfp.VehicleClassId || filteredTransaction.IKERearVehicleClassId == nfp.VehicleClassId) && associatedCVNF.VehicleClassId == nfp.VehicleClassId)
                                                    {
                                                        //LogMessage("Tag class and NF class matched. Going to financial and notification processing...");
                                                        if (filteredTransaction.IsBalanceUpdated == -1) //0 for balance not updated, 1 means balance updated
                                                        {
                                                            var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == filteredTransaction.TranscationId);
                                                            if (obj != null) obj.IsBalanceUpdated = 1;
                                                            //financial operation here
                                                            FinancialProcessing(associatedCVNF, customerAccountInfo, transaction);
                                                            // LogMessage("Financial processing has been done.");

                                                            //notification operation here
                                                            // NotificationProcessing(customerVehicleInfo, customerAccountInfo, transaction);
                                                            //LogMessage("Notification processing has been done.");
                                                        }
                                                        else
                                                        {
                                                            LogMessage("Balance is already updated.");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //LogMessage("Tag class and NF class does not match. Transaction will be marked as violation.");
                                                        //mark the transaction as violation and leave it for manual review
                                                        var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == filteredTransaction.TranscationId);
                                                        if (obj != null)
                                                        {
                                                            if (obj.IsBalanceUpdated != 1) //26 Dec, 2018
                                                            {
                                                                obj.IsViolation = 1;
                                                            }
                                                        }
                                                        if (transaction.IsBalanceUpdated != 1) //26 Dec, 2018
                                                        {
                                                            VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.MarkAsViolation(transaction);
                                                            LogMessage("Transaction is marked as violation.");
                                                        }
                                                        else
                                                        {
                                                            LogMessage("Transaction cannot be marked as violation as balance has been already updated.");
                                                        }
                                                    }
                                                    //}
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
                                                    //LogMessage("VRN exists but no associated crosstalk transaction found in transaction table. Checking associated nodeflux transaction...");

                                                    //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection associatedNodeFluxTrans = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetCorrespondingTransactionInNodeFlux(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber);
                                                    //1. Jan 2019 --> 
                                                    //filteredTransactionList = GetAssociatedTransactions(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber, "ANPR");
                                                    filteredTransactionList = GetAssociatedTransactionsANPR(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber, nfp.VehicleClassId, Convert.ToInt32(nfp.CameraPosition));
                                                    if (filteredTransactionList.Count > 0)
                                                    {
                                                        #region Update Tran Table if ANPR found
                                                        if (filteredTransactionList.Count == 1)
                                                        {
                                                            //LogMessage("Transaction found to update...");
                                                            //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = associatedNodeFluxTrans[0];
                                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
                                                            TranscationData Filtertransaction = filteredTransactionList[0];
                                                            transaction.NodefluxEntryIdFront = Convert.ToInt32(Filtertransaction.AnprFId);
                                                            transaction.NodefluxEntryIdRear = Convert.ToInt32(Filtertransaction.AnprRId);
                                                            transaction.TMSId = Filtertransaction.TMSId;
                                                            transaction.PlazaId = Filtertransaction.PlazaId;
                                                            transaction.LaneId = nfp.LaneId;
                                                            transaction.TransactionId = Filtertransaction.TranscationId;
                                                            transaction.TransactionDateTime = Filtertransaction.TransactionDateTime;
                                                            if (nfp.CameraPosition == "1") //1 means front, 2 means rear
                                                            {
                                                                if (ActiveAnpr.ToLower() == "hikvision")
                                                                {
                                                                    transaction.VehicleSpeed = nfp.VehicleSpeed;
                                                                }
                                                                else
                                                                {
                                                                    // update the VEHICLE speed
                                                                    if (transaction.NodefluxEntryIdRear > 0)
                                                                    {
                                                                        List<ANPRPktData> VehicleTimeStamp = anprRecentDataList.Where(trans => (trans.PlazaId == transaction.PlazaId && trans.PktId == transaction.NodefluxEntryIdRear)).OrderByDescending(o => o.PacketTimeStamp).ToList();
                                                                        if (VehicleTimeStamp.Count > 0)
                                                                        {
                                                                            transaction.VehicleSpeed = CalculateSpeed(nfpDateTime, VehicleTimeStamp[0].PacketTimeStamp);
                                                                        }
                                                                    }
                                                                }
                                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPFront(transaction, nfpEntryId);
                                                                //LogMessage("nf entry id front has been updated in transaction.");
                                                                var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                                if (obj != null) obj.AnprFId = nfpEntryId;
                                                            }
                                                            else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
                                                            {
                                                                if (ActiveAnpr.ToLower() == "hikvision")
                                                                {
                                                                    transaction.VehicleSpeed = nfp.VehicleSpeed;
                                                                }
                                                                else
                                                                {
                                                                    // update the VEHICLE speed
                                                                    if (transaction.NodefluxEntryIdFront > 0)
                                                                    {
                                                                        List<ANPRPktData> VehicleTimeStamp = anprRecentDataList.Where(trans => (trans.PlazaId == transaction.PlazaId && trans.PktId == transaction.NodefluxEntryIdFront)).OrderByDescending(o => o.PacketTimeStamp).ToList();
                                                                        if (VehicleTimeStamp.Count > 0)
                                                                        {
                                                                            transaction.VehicleSpeed = CalculateSpeed(nfpDateTime, VehicleTimeStamp[0].PacketTimeStamp);
                                                                        }
                                                                    }
                                                                }
                                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPRear(transaction, nfpEntryId);
                                                                var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                                if (obj != null) obj.AnprRId = nfpEntryId;
                                                                //LogMessage("nf entry id rear has been updated in transaction.");
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
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region No Associatied Found
                                                        if (nfp.CameraPosition == "1") //1 means front, 2 means rear
                                                        {
                                                            Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPFront(nfp, nfpEntryId, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.VRNRegistred.Registered);
                                                            transcationDataList.Add(new TranscationData { AnprFrontVehicleClassId = nfp.VehicleClassId, TranscationId = TranscationId, LaneId = nfp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, AnprFId = nfpEntryId, CameraPosition = 1, TransactionDateTime = Convert.ToDateTime(nfp.TimeStamp), CurrentDateTime = DateTime.Now });
                                                            //LogMessage("Transaction inserted by nf entry id front.");
                                                        }
                                                        else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
                                                        {
                                                            Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPRear(nfp, nfpEntryId, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.VRNRegistred.Registered);
                                                            transcationDataList.Add(new TranscationData { AnprRearVehicleClassId = nfp.VehicleClassId, TranscationId = TranscationId, LaneId = nfp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, AnprFId = nfpEntryId, CameraPosition = 2, TransactionDateTime = Convert.ToDateTime(nfp.TimeStamp), CurrentDateTime = DateTime.Now });
                                                            //LogMessage("Transaction inserted by nf entry id rear.");
                                                        }
                                                        else
                                                        {
                                                            LogMessage("Invalid camera position. " + nfp.CameraPosition);
                                                        }
                                                        #endregion
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
                                            //no associated vrn found in Database
                                            #region Create/ Update a transaction Main transaction table 
                                            try
                                            {
                                                //LogMessage("VRN does not exist. Checking associated nodeflux transaction...");

                                                //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection associatedNodeFluxTrans = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetCorrespondingTransactionInNodeFlux(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber);
                                                //2. Jan 2019 -->
                                                //filteredTransactionList = GetAssociatedTransactions(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber, "ANPR");
                                                filteredTransactionList = GetAssociatedTransactionsANPR(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber, nfp.VehicleClassId, Convert.ToInt32(nfp.CameraPosition));
                                                if (filteredTransactionList.Count > 0)
                                                {
                                                    #region Associate ANPR Found
                                                    if (filteredTransactionList.Count == 1)
                                                    {
                                                        #region ANPR Associated Data found in ANPR
                                                        TranscationData Filtertransaction = filteredTransactionList[0];
                                                        //LogMessage("Transaction found to update...");
                                                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
                                                        transaction.CrosstalkEntryIdFront = Convert.ToInt32(Filtertransaction.IKEFId);
                                                        transaction.CrosstalkEntryIdRear = Convert.ToInt32(Filtertransaction.IKERId);
                                                        transaction.NodefluxEntryIdFront = Convert.ToInt32(Filtertransaction.AnprFId);
                                                        transaction.NodefluxEntryIdRear = Convert.ToInt32(Filtertransaction.AnprRId);
                                                        transaction.TMSId = Filtertransaction.TMSId;
                                                        transaction.PlazaId = Filtertransaction.PlazaId;
                                                        transaction.LaneId = nfp.LaneId;
                                                        transaction.TransactionId = Filtertransaction.TranscationId;
                                                        transaction.TransactionDateTime = Filtertransaction.TransactionDateTime;
                                                        if (nfp.CameraPosition == "1") //1 means front, 2 means rear
                                                        {
                                                            if (ActiveAnpr.ToLower() == "hikvision")
                                                            {
                                                                transaction.VehicleSpeed = nfp.VehicleSpeed;
                                                            }
                                                            else
                                                            {
                                                                // update the VEHICLE speed
                                                                if (transaction.NodefluxEntryIdRear > 0)
                                                                {
                                                                    List<ANPRPktData> VehicleTimeStamp = anprRecentDataList.Where(trans => (trans.PlazaId == transaction.PlazaId && trans.PktId == transaction.NodefluxEntryIdRear)).OrderByDescending(o => o.PacketTimeStamp).ToList();
                                                                    if (VehicleTimeStamp.Count > 0)
                                                                    {
                                                                        transaction.VehicleSpeed = CalculateSpeed(nfpDateTime, VehicleTimeStamp[0].PacketTimeStamp);
                                                                    }
                                                                }
                                                            }
                                                            VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPFront(transaction, nfpEntryId);
                                                            var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                            if (obj != null) obj.AnprFId = nfpEntryId;
                                                            //LogMessage("nf entry id front has been updated in transaction.");
                                                        }
                                                        else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
                                                        {
                                                            if (ActiveAnpr.ToLower() == "hikvision")
                                                            {
                                                                transaction.VehicleSpeed = nfp.VehicleSpeed;
                                                            }
                                                            else
                                                            {
                                                                // update the VEHICLE speed
                                                                if (transaction.NodefluxEntryIdFront > 0)
                                                                {
                                                                    List<ANPRPktData> VehicleTimeStamp = anprRecentDataList.Where(trans => (trans.PlazaId == transaction.PlazaId && trans.PktId == transaction.NodefluxEntryIdFront)).OrderByDescending(o => o.PacketTimeStamp).ToList();
                                                                    if (VehicleTimeStamp.Count > 0)
                                                                    {
                                                                        transaction.VehicleSpeed = CalculateSpeed(VehicleTimeStamp[0].PacketTimeStamp, nfpDateTime);
                                                                    }
                                                                }
                                                            }
                                                            VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPRear(transaction, nfpEntryId);
                                                            var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                            if (obj != null) obj.AnprRId = nfpEntryId;

                                                            //LogMessage("nf entry id rear has been updated in transaction.");
                                                        }
                                                        else
                                                        {
                                                            LogMessage("Invalid camera position. " + nfp.CameraPosition);
                                                        }
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Abnormal case.");
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region No Transcation Found
                                                    if (nfp.CameraPosition == "1") //1 means front, 2 means rear
                                                    {
                                                        Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPFront(nfp, nfpEntryId, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.VRNRegistred.NotRegistered);
                                                        //LogMessage("Transaction inserted by nf entry id front.");
                                                        transcationDataList.Add(new TranscationData { AnprFrontVehicleClassId = nfp.VehicleClassId, TranscationId = TranscationId, LaneId = nfp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, AnprFId = nfpEntryId, CameraPosition = 1, TransactionDateTime = Convert.ToDateTime(nfp.TimeStamp), CurrentDateTime = DateTime.Now });
                                                    }
                                                    else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
                                                    {
                                                        Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPRear(nfp, nfpEntryId, (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.VRNRegistred.NotRegistered);
                                                        transcationDataList.Add(new TranscationData { AnprRearVehicleClassId = nfp.VehicleClassId, TranscationId = TranscationId, LaneId = nfp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, AnprRId = nfpEntryId, CameraPosition = 2, TransactionDateTime = Convert.ToDateTime(nfp.TimeStamp), CurrentDateTime = DateTime.Now });
                                                        //LogMessage("Transaction inserted by nf entry id rear.");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Invalid camera position. " + nfp.CameraPosition);
                                                    }
                                                    #endregion
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                LogMessage("Exception in inserting/updating in main transaction table. " + ex.ToString());
                                            }
                                            #endregion

                                            //***if exists in tbl_transaction updat it, otherwise insert***//


                                        }
                                        #endregion
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
                        LogMessage("Current object is not valid crosstalk/ nodeflux packet. " + m.Body.ToString());
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
            decimal tollToDeduct = -1; //if no toll rate found it will be returned as -1
            try
            {
                LogMessage("Finding out LaneType and toll rate to deduct...");
                int laneTypeId = GetLaneTypeByLaneId(transaction.LaneId);
                LogMessage("LaneType is: " + laneTypeId);
                if (customerVehicleInfo.ExceptionFlag == 2)
                {
                    tollToDeduct = 0;
                    LogMessage("Toll to deduct is (EXCEPTION FLAG NOT CHARGED): " + tollToDeduct);
                }
                else
                {
                    tollToDeduct = GetTollRate(currentTMSId, laneTypeId, transaction.TransactionDateTime, customerVehicleInfo.VehicleClassId);
                    LogMessage("Toll to deduct is (for motorcycle it may be 0.00): " + tollToDeduct);
                }

                //if (tollToDeduct == -1)
                //{
                //    tollToDeduct = 0;
                //}
            }
            catch (Exception ex)
            {
                LogMessage("Exception in finding out lane type and toll to deduct. " + ex.ToString());
                tollToDeduct = -1;
            }
            #endregion

            if (tollToDeduct > -1)
            {
                Decimal currentAccountBalance = customerVehicleInfo.AccountBalance;
                Decimal afterDeduction = currentAccountBalance - tollToDeduct;
                int entryId = 0;
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
                    accountHistory.IsEmailSent = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.EmailSentStatus.Unsent;//will be updated later on//accountHistory.ModifierId = 1;//will be updated later on
                    accountHistory.CreationDate = DateTime.Now;
                    accountHistory.ModificationDate = DateTime.Now;
                    accountHistory.TransferStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred;
                    accountHistory.OpeningBalance = currentAccountBalance;
                    accountHistory.ClosingBalance = afterDeduction;
                    entryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.AccountHistoryBLL.Insert(accountHistory);
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
                    //customerVehicleInfo.AccountBalance += -1 * tollToDeduct;
                    customerVehicleInfo.AccountBalance = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.UpdateVehiclebalance(customerVehicleInfo, (-1 * tollToDeduct));
                    //VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.UpdateBalance(customerAccountInfo, (-1 * tollToDeduct));

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
                    NotificationProcessing(customerVehicleInfo, customerAccountInfo, transaction, tollToDeduct, afterDeduction, entryId);
                    LogMessage("Transaction is marked as balance updated.");
                }
                catch (Exception ex)
                {
                    LogMessage("Exception in marking the transaction as balance updated. " + ex.ToString());
                }
                #endregion

                //if (afterDeduction > 0)
                //{

                //}
                //else
                //{
                //    LogMessage("Transaction has been declined due to insufficient balance.");
                //    NotificationProcessing(customerVehicleInfo, customerAccountInfo, transaction, tollToDeduct, afterDeduction);
                //}
            }
            else
            {
                LogMessage("Toll to deduct is -1.00. There is some error somewhere.");
            }
        }

        private void NotificationProcessing(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicleInfo, VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccountInfo, VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction, Decimal tollToDeduct, Decimal AfterDeduction, int entryId)
        {
            try
            {
                //smsFileConfig = VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.SMSFileConfiguration.Deserialize();
                LogMessage("Trying to push SMS to MSMQ...");

                Message smsMessage = new Message();
                smsMessage.Formatter = new BinaryMessageFormatter();
                VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail smsDetail = new Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail();
                //smsDetail.SMSMessage = "Akun anda telah dipotong untuk bertransaksi nomor kendaraan " + customerVehicleInfo.VehRegNo + " anda di tempat " + GetPlazaNameById(transaction.PlazaId) + " pada " + transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.dateTimeFormat24H) + ".";
                CultureInfo culture = new CultureInfo("id-ID");
                string RechareDate = transaction.TransactionDateTime.AddDays(4).ToString("dd-MMM-yyyy") + " 23:59:59";
                if (AfterDeduction >= 0)
                {
                    string AFTERDEDUCTION = Constants.AfterDeduction;
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[tolltodeduct]", Decimal.Parse(tollToDeduct.ToString()).ToString("C", culture));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[vehregno]", customerVehicleInfo.VehRegNo);
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[transactiondatetime]", transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.DATETIME_FORMAT_WITHOUT_SECONDSForSMS));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[plazaid]", GetPlazaNameById(transaction.PlazaId));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[laneid]", transaction.LaneId.ToString());
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[balance]", Decimal.Parse(AfterDeduction.ToString()).ToString("C", culture));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[tid]", transaction.TransactionId.ToString());
                    smsDetail.SMSMessage = AFTERDEDUCTION;
                }
                else
                {
                    string NOTIFICATION = Constants.AfterDeductionInsufficientBalance;
                    NOTIFICATION = NOTIFICATION.Replace("[tolltodeduct]", Decimal.Parse(tollToDeduct.ToString()).ToString("C", culture));
                    NOTIFICATION = NOTIFICATION.Replace("[vehregno]", customerVehicleInfo.VehRegNo);
                    NOTIFICATION = NOTIFICATION.Replace("[transactiondatetime]", transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.DATETIME_FORMAT_WITHOUT_SECONDSForSMS));
                    NOTIFICATION = NOTIFICATION.Replace("[plazaid]", GetPlazaNameById(transaction.PlazaId));
                    NOTIFICATION = NOTIFICATION.Replace("[laneid]", transaction.LaneId.ToString());
                    NOTIFICATION = NOTIFICATION.Replace("[recharedate]", RechareDate);
                    NOTIFICATION = NOTIFICATION.Replace("[liability]", Decimal.Parse(Math.Abs(AfterDeduction).ToString()).ToString("C", culture));
                    NOTIFICATION = NOTIFICATION.Replace("[tid]", transaction.TransactionId.ToString());
                    smsDetail.SMSMessage = NOTIFICATION;
                    //"Pelanggan Yth, Saldo SJBE anda saat ini tidak mencukupi untuk dilakukan pemotongan senilai Rp " + Decimal.Parse(tollToDeduct.ToString()).ToString("C", culture).Replace("Rp", "") + " atas transaksi kendaraan " + customerVehicleInfo.VehRegNo + " pada " + transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.DATETIME_FORMAT_WITHOUT_SECONDSForSMS) + " di Gantry - Medan Merdeka Barat 1. Silahkan melakukan pengisian ulang saldo SJBE anda sebelum " + RechareDate + ". Keterlambatan pengisian ulang saldo akan dikenakan denda sebesar Rp 1.000.000,00. Sisa saldo SJBE anda saat ini Rp " + Decimal.Parse((AfterDeduction + tollToDeduct).ToString()).ToString("C", culture).Replace("Rp", "") + " Ref: [" + transaction.TransactionId.ToString() + "]";
                }

                LogMessage(smsDetail.SMSMessage);
                smsDetail.AccountId = customerAccountInfo.AccountId;
                smsDetail.CustomerName = customerAccountInfo.FirstName + " " + customerAccountInfo.LastName;
                smsDetail.SenderMobileNumber = customerAccountInfo.MobileNo;
                smsDetail.AccountHistoryId = entryId;
                smsMessage.Body = smsDetail;

                LogMessage("Detail:" + smsDetail.ToString());
                smsMessageQueue.Send(smsMessage);
                LogMessage("Message pushed successfully to MSMQ.");
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
                lock (customerVehicles)
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
                lock (customerVehicles)
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
                lock (customerVehicles)
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
                //VaaaN.MLFF.Libraries.CommonLibrary.BLL.RoleBLL.GetAll().Select(x => new SelectListItem { Text = x.RoleName, Value = x.RoleId.ToString() });
                //var carsWithIds = customerVehicles.select(a => new { a.id, a.cars.color, a.cars.model })
                lock (customerVehicles)
                {
                    foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE cvc in customerVehicles)
                    {
                        if (cvc.TagId.Replace("FC", "00").ToLower() == tagId.Replace("FC", "00").ToLower())
                        {
                            result = cvc;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception in DoesVRNExist() function. " + ex.ToString());
            }

            return result;
        }

        private bool DoesExistInRecentCrossTalkPackets(Int32 plazaId, string tagId, DateTime tagReportingTime, string locationId, int ReaderPosition)
        {
            bool result = false;
            try
            {
                var obj = rfidRecentDataList.FirstOrDefault(e => (e.PlazaId == plazaId && e.ObjectId == tagId && (e.PacketTimeStamp > tagReportingTime.AddMinutes(-1 * Minutes) && e.PacketTimeStamp > tagReportingTime.AddMinutes(Minutes)) && e.ReaderPosition == ReaderPosition));
                if (obj != null)
                {
                    obj.PacketTimeStamp = tagReportingTime;
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception in finding recent crosstalk packets. " + ex.Message);
                result = false;
            }
            return result;
        }

        private bool DoesExistInRecentNodeFluxPackets(Int32 plazaId, string vrn, DateTime nodeFluxReportingTime, int cameraPosition)
        {
            bool result = false;
            try
            {
                LogMessage("Checking in recent nodeflux list...");

                var obj = anprRecentDataList.FirstOrDefault(e => (e.PlazaId == plazaId && e.VRN.Trim() == vrn.Trim() && (e.PacketTimeStamp > nodeFluxReportingTime.AddMinutes(-1 * Minutes) && e.PacketTimeStamp > nodeFluxReportingTime.AddMinutes(Minutes)) && e.cameraPosition == cameraPosition));
                if (obj != null)
                {
                    obj.PacketTimeStamp = nodeFluxReportingTime;
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception in finding recent nodeflux packets. " + ex.Message);
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

                lock (tollRates)
                {
                    currentTimeTollRates = VaaaN.MLFF.Libraries.CommonLibrary.Constants.GetTollRateCollection(transactionTime, tollRates);
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
                LogMessage("Failed to get toll rate. " + ex.Message);
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
                lock (hardwares)
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
                lock (customerAccounts)
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

        private VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE GetVehicleClassById(int vehicleClassId)
        {
            LogMessage("Getting vehicle class by name...");
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE result = null;

            try
            {
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicleClasses)
                {
                    if (vc.Id == vehicleClassId)
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
            string result = "NA";

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
                result = "NA";
            }

            return result;
        }

        private bool IsValidTag(string epcInput)
        {
            bool result = false;
            try
            {
                LogMessage("Checking the validity of the tag: " + epcInput + "...");
                string epc = epcInput.Trim();
                if (epc.Length > 2)
                {
                    string ftc = epc.Substring(0, 2); //ftc = First Two Characters
                    if ((ftc == "01") || (ftc == "02") || (ftc == "03") || (ftc == "04"))
                    {
                        int indexOfFC = epc.IndexOf("FC");
                        if (indexOfFC == -1)
                        {
                            //invalid tag
                            result = false;
                        }
                        else
                        {
                            result = true; //length of the vrn should be checked here
                        }
                    }
                    else
                    {
                        //invalid tag
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception in checking validity of the tag. " + ex.ToString());
                result = false;
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

        //ORIGINAL
        ////////public List<TranscationData> GetAssociatedTransactions(Int32 tmsId, Int32 plazaId, DateTime timestamp, string vrn, string pktType)
        ////////{
        ////////    if (pktType.ToLower() == "rfid")
        ////////    {
        ////////        return transcationDataList.Where(trans => (trans.TMSId == tmsId && trans.PlazaId == plazaId && trans.VRN.ToLower() == vrn.ToLower() && trans.TransactionDateTime <= timestamp.AddSeconds(30) && trans.TransactionDateTime >= timestamp.AddSeconds(-30)) && (trans.IKEFId > 0 || trans.IKERId > 0)).ToList();
        ////////    }
        ////////    else if (pktType.ToLower() == "anpr")
        ////////    {
        ////////        return transcationDataList.Where(trans => (trans.TMSId == tmsId && trans.PlazaId == plazaId && trans.VRN.ToLower() == vrn.ToLower() && trans.TransactionDateTime <= timestamp.AddSeconds(30) && trans.TransactionDateTime >= timestamp.AddSeconds(-30)) && (trans.AnprFId > 0 || trans.AnprRId > 0)).ToList();
        ////////    }
        ////////    else
        ////////    {
        ////////        return transcationDataList.Where(trans => (trans.TMSId == tmsId && trans.PlazaId == plazaId && trans.VRN.ToLower() == vrn.ToLower() && trans.TransactionDateTime <= timestamp.AddSeconds(30) && trans.TransactionDateTime >= timestamp.AddSeconds(-30))).ToList();
        ////////    }
        ////////}

        //the above function is commented and replaced with the following two - CJS 2019
        public List<TranscationData> GetAssociatedTransactions(Int32 tmsId, Int32 plazaId, DateTime timestamp, string vrn, int VehicleClassId, string pktType)
        {
            if (pktType.ToLower() == "rfid")
            {
                return transcationDataList.Where(trans => (trans.TMSId == tmsId && trans.PlazaId == plazaId && trans.VRN.ToLower().Trim() == vrn.ToLower().Trim() && trans.TransactionDateTime <= timestamp.AddMinutes(Minutes) && trans.TransactionDateTime >= timestamp.AddMinutes(-1 * Minutes)) && (trans.IKEFId > 0 || trans.IKERId > 0) && (trans.IKEFrontVehicleClassId == VehicleClassId || trans.IKERearVehicleClassId == VehicleClassId)).ToList();
            }
            else
            {
                return transcationDataList.Where(trans => (trans.TMSId == tmsId && trans.PlazaId == plazaId && trans.VRN.ToLower().Trim() == vrn.ToLower().Trim() && trans.TransactionDateTime <= timestamp.AddMinutes(Minutes) && trans.TransactionDateTime >= timestamp.AddMinutes(-1 * Minutes) && (trans.AnprFrontVehicleClassId == VehicleClassId || trans.AnprRearVehicleClassId == VehicleClassId) && (trans.AnprFId > 0 || trans.AnprRId > 0))).ToList();
            }
        }

        public List<TranscationData> GetAssociatedTransactionsANPR(Int32 tmsId, Int32 plazaId, DateTime timestamp, string vrn, Int32 vehicleClassId, int cameraPosition)
        {
            if (cameraPosition == 1) //front
            {
                //comparing front vehicle class id with rear class id
                return transcationDataList.Where(trans => (trans.AnprRearVehicleClassId == vehicleClassId && trans.TMSId == tmsId && trans.PlazaId == plazaId && trans.VRN.ToLower().Trim() == vrn.ToLower().Trim() && trans.TransactionDateTime <= timestamp.AddMinutes(Minutes) && trans.TransactionDateTime >= timestamp.AddMinutes(-1 * Minutes)) && (trans.AnprFId > 0 || trans.AnprRId > 0)).ToList();
            }
            else if (cameraPosition == 2) //rear
            {
                //comparing rear vehicle class id with front class id
                return transcationDataList.Where(trans => (trans.AnprFrontVehicleClassId == vehicleClassId && trans.TMSId == tmsId && trans.PlazaId == plazaId && trans.VRN.ToLower().Trim() == vrn.ToLower().Trim() && trans.TransactionDateTime <= timestamp.AddMinutes(Minutes) && trans.TransactionDateTime >= timestamp.AddMinutes(-1 * Minutes)) && (trans.AnprFId > 0 || trans.AnprRId > 0)).ToList();
            }
            else
            {
                return transcationDataList.Where(trans => (trans.TMSId == tmsId && trans.PlazaId == plazaId && trans.VRN.ToLower().Trim() == vrn.ToLower().Trim() && trans.TransactionDateTime <= timestamp.AddMinutes(Minutes) && trans.TransactionDateTime >= timestamp.AddMinutes(-1 * Minutes))).ToList();
            }
        }

        //public List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE> FilterToolRate(int plazaId, int laneType, DateTime transactionTime, int vehicleClass)
        //{
        //    return tollRates.Cast<VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE>().Where(trans => (trans.PlazaId == plazaId && trans.LaneTypeId == laneType && trans.VehicleClassId == vehicleClass)).ToList();
        //    //return tollRates.Cast<List>.Where(trans => (trans.TMSId == tmsId && trans.PlazaId == plazaId && trans.VRN.ToLower() == Vrn.ToLower() && trans.TransactionDateTime <= timestamp.AddSeconds(30) && trans.TransactionDateTime >= timestamp.AddSeconds(-30))).ToList();
        //}

        public Double CalculateSpeed(DateTime StartTime, DateTime EndTime)
        {
            Double Speed = 0;
            Double time = 0;
            LogMessage("Calculate Speed Start Date Time : " + StartTime.ToString() + " End Date Time : " + EndTime.ToString());
            try
            {
                Double Distance = TotalDistance;
                LogMessage("Check Distance : " + Distance.ToString());

                time = Math.Abs(((EndTime - StartTime).TotalSeconds));

                LogMessage("Check Time Taken : " + time);
                if (time > 0 && Distance > 0)
                    Speed = Distance / time;
                LogMessage("Speed in m/s : " + Speed);
            }
            catch (Exception ex)
            {
                LogMessage("Error in Calculate Speed. " + ex.ToString());
            }
            return Math.Round(3.6 * Speed);
        }

        private void CollectionUpdatingThreadFunction()
        {
            int counter = 0;
            //for the time being we can ignore deleted records
            while (!stopCollectionUpdatingThread)
            {
                try
                {
                    counter = counter + 1;

                    if (counter > 60) // will update in every 1 minute (60 time 1000ms = 1 minute)
                    {
                        LogMessage("Going to check for any update in collections...");

                        ActiveAnpr = VaaaN.MLFF.Libraries.CommonLibrary.BLL.HardwareBLL.GetActiveANPR();

                        #region Updating latest customer accounts
                        //access records whose creation time is greater than the lastCollectionUpdateTime and status is equal to processed
                        //VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection tempCA = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetLatestCustomerAccounts(lastCollectionUpdateTime);
                        //if(tempCA.Count > 0)
                        //{
                        //    LogMessage(tempCA.Count + " new customer account record(s) found.");
                        //    lock (customerAccounts)
                        //    {
                        //        for(int i = 0; i < tempCA.Count; i++)
                        //        {
                        //            customerAccounts.Add(tempCA[i]);
                        //        }
                        //    }
                        //    LogMessage("Customer account collection has been updated.");
                        //}
                        lock (customerAccounts)
                        {
                            customerAccounts = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsCollection();
                        }
                        #endregion

                        #region Updating latest customer vehicles
                        //access records whose creation time is greater than the lastCollectionUpdateTime and status is equal to processed
                        //VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection tempCV = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetLatestCustomerVehicles(lastCollectionUpdateTime);
                        //if(tempCV.Count > 0)
                        //{
                        //    LogMessage(tempCV.Count + " new customer vehicle record(s) found.");
                        //    lock (customerVehicles)
                        //    {
                        //        for(int i = 0; i < tempCV.Count; i++)
                        //        {
                        //            customerVehicles.Add(tempCV[i]);
                        //        }
                        //    }
                        //    LogMessage("Customer vehicle collection has been updated.");
                        //}
                        lock (customerVehicles)
                        {
                            customerVehicles = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetAllAsCollection();
                        }
                        #endregion

                        #region Updating latest hardwares
                        //access records whose creation time is greater than the lastCollectionUpdateTime
                        //VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection tempHardware = VaaaN.MLFF.Libraries.CommonLibrary.BLL.HardwareBLL.GetLatestHardwares(lastCollectionUpdateTime);
                        //if(tempHardware.Count > 0)
                        //{
                        //    LogMessage(tempHardware.Count + " new hardware record(s) found.");
                        //    lock (hardwares)
                        //    {
                        //        for(int i = 0; i < tempHardware.Count; i++)
                        //        {
                        //            hardwares.Add(tempHardware[i]);
                        //        }
                        //    }
                        //    LogMessage("Hardware collection has been updated.");
                        //}
                        lock (hardwares)
                        {
                            hardwares = VaaaN.MLFF.Libraries.CommonLibrary.BLL.HardwareBLL.GetAll();
                        }
                        #endregion

                        #region Updating latest tollRates
                        //access records whose creation time is greater than the lastCollectionUpdateTime
                        //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection tempTollRates = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TollRateBLL.GetLatestTollRates(lastCollectionUpdateTime);
                        //if (tempTollRates.Count > 0)
                        //{
                        //    LogMessage(tempTollRates.Count + " new toll rate record(s) found.");
                        //    lock (tollRates)
                        //    {
                        //        for (int i = 0; i < tollRates.Count; i++)
                        //        {
                        //            tollRates.Add(tempTollRates[i]);
                        //        }
                        //    }
                        //    LogMessage("Toll rates collection has been updated.");
                        //}
                        lock (tollRates)
                        {
                            tollRates = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TollRateBLL.GetAll();
                        }
                        #endregion

                        counter = 0;
                    }
                }

                catch (Exception ex)
                {
                    LogMessage("Exception in ListUpdatingThreadFunction(). " + ex.ToString());
                }
                finally
                {
                    lastCollectionUpdateTime = DateTime.Now; //<===================================== important

                    Thread.Sleep(1000); //should not use long time sleep like 1 minute, 1 hour etc
                }
            }

        }
        #endregion

        #region Service Logger
        private void LogMessage(String message)
        {
            logQueue.Enqueue(Environment.NewLine + "A.T. " + DateTime.Now.ToString("hh:mm:ss.FFFF tt") + ": " + message);
            //logQueue.Enqueue(Environment.NewLine + message);
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

    #region Class Porperty

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

    public class IkePktData
    {
        public string ObjectId { get; set; }
        public Int32 PlazaId { get; set; }
        public DateTime currentDateTime { get; set; }
        public DateTime PacketTimeStamp { get; set; }
        public Int32 VehicleClassId { get; set; }
        public Int32 PktId { get; set; }
        public String LocationId { get; set; }
        public Int32 ReaderPosition { get; set; }
    }

    public class ANPRPktData
    {
        public string VRN { get; set; }
        public Int32 PlazaId { get; set; }
        public Int32 cameraPosition { get; set; }
        public DateTime currentDateTime { get; set; }
        public DateTime PacketTimeStamp { get; set; }
        public Int32 PktId { get; set; }
        public Int32 VehicleClassId { get; set; }

    }

    public class TranscationData
    {
        public Int32 TMSId { get; set; }
        public Int32 PlazaId { get; set; }
        public Int32 LaneId { get; set; }
        public Int32 IsViolation { get; set; }
        public Int32 IsBalanceUpdated { get; set; }
        public string VRN { get; set; }
        public Int32 CameraPosition { get; set; }
        public Int64 TranscationId { get; set; }
        public Int64 IKEFId { get; set; }
        public Int64 IKERId { get; set; }
        public Int64 AnprFId { get; set; }
        public Int64 AnprRId { get; set; }

        //Added on Jan 2019--------------------------------
        public Int32 AnprFrontVehicleClassId { get; set; }
        public Int32 AnprRearVehicleClassId { get; set; }

        public Int32 IKEFrontVehicleClassId { get; set; }
        public Int32 IKERearVehicleClassId { get; set; }
        //-------------------------------------------------

        public DateTime CurrentDateTime { get; set; }
        public DateTime TransactionDateTime { get; set; }

        // public Int32 IKEVechileClassId { get; set; }
    }
    #endregion
}

