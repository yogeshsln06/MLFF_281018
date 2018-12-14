using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using System.Messaging;
using System.Collections;
using System.Threading;
using System.Globalization;

namespace VaaaN.MLFF.WindowsServices
{
    public partial class MainService : ServiceBase
    {
        #region Variables
        private Queue logQueue = new Queue();
        private Thread loggerThread;
        private volatile Boolean stopLoggerThread = false;

        DateTime lastCollectionUpdateTime = DateTime.MinValue;

        VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection lanes;
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection hardwares;
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCollection plazas;
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection customerAccounts;
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection customerVehicles;
        public static VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCollection tollRates;
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection vehicleClasses;

        private MessageQueue failedQueue;
        //private MessageQueue failedQueueIKE;
        //private MessageQueue failedQueueANPR;

        private MessageQueue inBoxQueue;
        //private MessageQueue inBoxQueueANPR;
        //private MessageQueue inBoxQueueIKE;

        private MessageQueue smsMessageQueue;
        private MessageQueue eventQueue;

        int ctpEntryId = 0;
        int nfpEntryId = 0;
        int currentTMSId = -1;
        Double TotalDistance = 30;

        DateTime countStartTime = DateTime.MinValue;
        int motorCycleCount = 0;
        int smallCount = 0;
        int mediumCount = 0;
        int bigCount = 0;
        string counterString = string.Empty;

        List<IkePktData> rfidRecentDataList = new List<IkePktData>();
        List<ANPRPktData> anprRecentDataList = new List<ANPRPktData>();
        List<TranscationData> transcationDataList = new List<TranscationData>();
        List<TagData> recentlyProcessedTagsList = new List<TagData>();

        List<TranscationData> TranscationDataFilterList = new List<TranscationData>();
        List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE> TollRateFilteredList = new List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.TollRateCBE>();

        VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.GeneralConfiguration generalFileConfig;
        VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.SMSFileConfiguration smsFileConfig;

        Thread collectionUpdaterThread;

        volatile Boolean stopCollectionUpdatingThread = false;
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
                //this.failedQueueIKE = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.failedQueueNameIKE);
                //this.failedQueueANPR = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.failedQueueNameANPR);
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

                #region Commented
                ////////#region IKE Queue
                ////////try
                ////////{
                ////////    LogMessage("Attaching listener to inbox queue (IKE)...");

                ////////    this.inBoxQueueIKE = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.inBoxQueueNameIKE);
                ////////    inBoxQueueIKE.PeekCompleted += new PeekCompletedEventHandler(InBoxQueueIKE_PeekCompleted);
                ////////    inBoxQueueIKE.BeginPeek();

                ////////    LogMessage("Inbox queue listener (IKE) has been attached.");
                ////////}
                ////////catch (Exception)
                ////////{
                ////////    throw;
                ////////}
                ////////#endregion

                ////////#region ANPR Queue
                ////////try
                ////////{
                ////////    LogMessage("Attaching listener to inbox queue (ANPR)...");

                ////////    this.inBoxQueueANPR = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.inBoxQueueNameANPR);
                ////////    inBoxQueueANPR.PeekCompleted += new PeekCompletedEventHandler(InBoxQueueANPR_PeekCompleted);
                ////////    inBoxQueueANPR.BeginPeek();

                ////////    LogMessage("Inbox queue listener (ANPR) has been attached.");
                ////////}
                ////////catch (Exception)
                ////////{
                ////////    throw;
                ////////}
                ////////#endregion
                #endregion

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
                //inBoxQueueIKE.PeekCompleted -= new PeekCompletedEventHandler(InBoxQueueIKE_PeekCompleted);
                //inBoxQueueANPR.PeekCompleted -= new PeekCompletedEventHandler(InBoxQueueANPR_PeekCompleted);
                inBoxQueue.PeekCompleted -= new PeekCompletedEventHandler(InBoxQueue_PeekCompleted);
            }
            catch(Exception ex)
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

        #region Commented
        ////////void InBoxQueueIKE_PeekCompleted(object sender, PeekCompletedEventArgs e)
        ////////{
        ////////    bool receiveRecord = false;
        ////////    MessageQueue mq = (MessageQueue)sender;

        ////////    try
        ////////    {
        ////////        Message m = (Message)mq.EndPeek(e.AsyncResult);
        ////////        m.Formatter = new BinaryMessageFormatter();

        ////////        LogMessage("Going to process InBoxQueue(IKE) message...");

        ////////        ProcessQueueMessageIKE(m);

        ////////        receiveRecord = true;
        ////////    }
        ////////    catch (Exception ex)
        ////////    {
        ////////        LogMessage("Error in peeking inbox(IKE) queue. " + ex.ToString());
        ////////        receiveRecord = false;
        ////////    }
        ////////    finally
        ////////    {
        ////////        //if (receiveRecord)
        ////////        //{
        ////////        mq.Receive();
        ////////        //}
        ////////        //else
        ////////        //{
        ////////        //receive and send to failed queue
        ////////        //}

        ////////        inBoxQueueIKE.BeginPeek();
        ////////    }
        ////////}

        ////////void InBoxQueueANPR_PeekCompleted(object sender, PeekCompletedEventArgs e)
        ////////{
        ////////    bool receiveRecord = false;
        ////////    MessageQueue mq = (MessageQueue)sender;

        ////////    try
        ////////    {
        ////////        Message m = (Message)mq.EndPeek(e.AsyncResult);
        ////////        m.Formatter = new BinaryMessageFormatter();

        ////////        LogMessage("Going to process InBoxQueue(ANPR) message...");

        ////////        ProcessQueueMessageANPR(m);

        ////////        receiveRecord = true;
        ////////    }
        ////////    catch (Exception ex)
        ////////    {
        ////////        LogMessage("Error in peeking inbox(ANPR) queue. " + ex.ToString());
        ////////        receiveRecord = false;
        ////////    }
        ////////    finally
        ////////    {
        ////////        //if (receiveRecord)
        ////////        //{
        ////////        mq.Receive();
        ////////        //}
        ////////        //else
        ////////        //{
        ////////        //receive and send to failed queue
        ////////        //}

        ////////        inBoxQueueANPR.BeginPeek();
        ////////    }
        ////////}
        #endregion

        void InBoxQueue_PeekCompleted(object sender, PeekCompletedEventArgs e)
        {
            bool receiveRecord = false;
            MessageQueue mq = (MessageQueue)sender;

            try
            {
                Message m = (Message)mq.EndPeek(e.AsyncResult);
                m.Formatter = new BinaryMessageFormatter();

                LogMessage("Going to process InBoxQueue message...");

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

        #region commented
        ////////private void ProcessQueueMessageIKE(Message m)
        ////////{
        ////////    if (m != null)
        ////////    {
        ////////        m.Formatter = new BinaryMessageFormatter();

        ////////        if (m.Body != null)
        ////////        {
        ////////            #region Processing packets
        ////////            if (m.Body is VaaaN.MLFF.Libraries.CommonLibrary.Common.CrossTalkPacket)
        ////////            {
        ////////                #region CrossTalk packet
        ////////                LogMessage("==IKE==");

        ////////                VaaaN.MLFF.Libraries.CommonLibrary.Common.CrossTalkPacket crossTalkPacket = (VaaaN.MLFF.Libraries.CommonLibrary.Common.CrossTalkPacket)m.Body;

        ////////                if (crossTalkPacket.Payload != null)
        ////////                {
        ////////                    if (crossTalkPacket.Payload is VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE)
        ////////                    {
        ////////                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE ctp = (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE)crossTalkPacket.Payload;

        ////////                        #region parsing tagid
        ////////                        ctp.ObjectId = ctp.ObjectId.Trim(); //otherwise trailing and leading spaces create problems
        ////////                        TagStructure ts = ParseEPC(ctp.ObjectId);

        ////////                        int eviClass = -1;
        ////////                        string eviVRN = "";
        ////////                        if (ts != null)
        ////////                        {
        ////////                            eviClass = ts.ClassId;
        ////////                            eviVRN = ts.VRN;

        ////////                            LogMessage("Checking the tag exists in the system or not. " + ctp.ObjectId);
        ////////                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE associatedCVCT = DoesTagExist(ctp.ObjectId);
        ////////                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE associatedCACT = null;
        ////////                            if (associatedCVCT != null)
        ////////                            {
        ////////                                LogMessage("Tag exists.");
        ////////                                associatedCACT = GetCustomerAccountById(associatedCVCT.AccountId);

        ////////                                LogMessage("The associated accoount is: " + associatedCACT.FirstName + " " + associatedCACT.LastName);

        ////////                                #region Update some fields in the CBE
        ////////                                try
        ////////                                {
        ////////                                    LogMessage("Trying to update TMSId, Creation Date etc. in the CBE...");
        ////////                                    ctp.TMSId = currentTMSId;
        ////////                                    //ctp.TimeStamp = ConversionDateTime(ctp.TimeStamp, "crosstalk");

        ////////                                    //in case of crosstalk not giving lane id and plaza id peek laneid and plazaid by hardwareid. 
        ////////                                    //hardwareid is assigned to specific lane of specific plaza.
        ////////                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane = GetLaneDetailByHardwareId(Convert.ToInt32(ctp.LocationId)); //locationid is hardwareid? (satya said)

        ////////                                    if (lane != null)
        ////////                                    {
        ////////                                        ctp.PlazaId = lane.PlazaId; //<== need to update as per device location
        ////////                                        ctp.LaneId = lane.LaneId; //<== need to update as per device location
        ////////                                    }
        ////////                                    else
        ////////                                    {
        ////////                                        LogMessage("No lane detail found against the hardware id: " + ctp.LocationId);
        ////////                                        //future processing should be closed here
        ////////                                    }

        ////////                                    ctp.CreationDate = System.DateTime.Now;
        ////////                                    ctp.ModifierId = 1;
        ////////                                    ctp.ModificationDate = System.DateTime.Now;
        ////////                                    LogMessage("Crosstalk CBE updated successfully.");


        ////////                                    #region Insert Into Even Queue for Live Event
        ////////                                    try
        ////////                                    {
        ////////                                        LogMessage("Trying to push crosstalk event to event queue...");
        ////////                                        //plaza id, plaza name, lane id, lane name, vrn, class, timestamp
        ////////                                        Message crosstalkEventMessage = new Message();
        ////////                                        crosstalkEventMessage.Formatter = new BinaryMessageFormatter();
        ////////                                        crosstalkEventMessage.TimeToBeReceived = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.eventQueueTimeOut;

        ////////                                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkEvent ctEvent = new Libraries.CommonLibrary.CBE.CrossTalkEvent();

        ////////                                        ctEvent.Timestamp = Convert.ToDateTime(ctp.TimeStamp);// Convert.ToDateTime(ConversionDateTime(ctp.TimeStamp, "crosstalk"));
        ////////                                        ctEvent.PlazaId = ctp.PlazaId;
        ////////                                        ctEvent.PlazaName = GetPlazaNameById(ctp.PlazaId);
        ////////                                        ctEvent.LaneName = GetLaneNameById(ctp.LaneId);
        ////////                                        ctEvent.VehicleClassName = associatedCVCT.VehicleClassName;
        ////////                                        ctEvent.VRN = associatedCVCT.VehRegNo;

        ////////                                        crosstalkEventMessage.Body = ctEvent;

        ////////                                        eventQueue.Send(crosstalkEventMessage);
        ////////                                        LogMessage("Crosstalk event pushed to event queue successfully.");
        ////////                                    }
        ////////                                    catch (Exception ex)
        ////////                                    {
        ////////                                        LogMessage("Exception in pushing crosstalk event to event queue. " + ex.ToString());
        ////////                                    }
        ////////                                    #endregion


        ////////                                }
        ////////                                catch (Exception ex)
        ////////                                {
        ////////                                    LogMessage("Exception in updating some fields. " + ex.ToString());
        ////////                                }
        ////////                                #endregion

        ////////                                //does recent transactions contains this tag? it may be reported repetatively. 
        ////////                                //check in tbl_crosstalk_packet's most recent transactions of the same plaza

        ////////                                #region Check in recent crosstalk packets
        ////////                                LogMessage("Checking tag has been already inserted..." + ctp.PlazaId + ", " + ctp.ObjectId + ", " + ctp.TimeStamp);
        ////////                                if (!DoesExistInRecentCrossTalkPackets(ctp.PlazaId, ctp.ObjectId, Convert.ToDateTime(ctp.TimeStamp)))
        ////////                                {
        ////////                                    #region Send to local database
        ////////                                    try
        ////////                                    {
        ////////                                        LogMessage("Sending crosstalk packet to local database. " + ctp.ObjectId);

        ////////                                        ctpEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CrossTalkBLL.Insert(ctp);
        ////////                                        ikeRecentDataList.Add(new IkePktData { PktId = ctpEntryId, PacketTimeStamp = Convert.ToDateTime(ctp.TimeStamp), VehicleClassId = associatedCVCT.VehicleClassId, ObjectId = ctp.ObjectId, PlazaId = ctp.PlazaId, currentDateTime = DateTime.Now });

        ////////                                        LogMessage("Crosstalk packet inserted successfully.");
        ////////                                    }
        ////////                                    catch (Exception ex)
        ////////                                    {
        ////////                                        #region Send data to Failed queue
        ////////                                        LogMessage("Failed to insert crosstalk packet." + ex.Message);
        ////////                                        try
        ////////                                        {
        ////////                                            LogMessage("Trying to send to failed queue...");

        ////////                                            m.Recoverable = true;
        ////////                                            failedQueueIKE.Send(m);

        ////////                                            LogMessage("Message sent to failed queue.");
        ////////                                        }
        ////////                                        catch (Exception exc)
        ////////                                        {
        ////////                                            LogMessage("***DATA LOST*** Failed to send to failed queue. Crosstalk packet Transaction is " + ctp.ToString() + exc.ToString());
        ////////                                        }
        ////////                                        #endregion
        ////////                                    }
        ////////                                    #endregion

        ////////                                    //is the associated VRN is already inserted in the transaction table by nodeflux front or nodeflux rear camera?
        ////////                                    //if inserted look by associated vrn and update, if not create a new transaction
        ////////                                    LogMessage("Searching associated transaction exists in the transaction table or not...");
        ////////                                    DateTime ctpDateTime = Convert.ToDateTime(ctp.TimeStamp);
        ////////                                    LogMessage("Search criteria: " + ctp.TMSId + ", " + ctp.PlazaId + ", " + ctpDateTime.ToString("dd/MM/yyyy hh:mm:ss.fff tt") + ", " + associatedCVCT.VehRegNo);
        ////////                                    //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans1 = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetCorrespondingTransactionInNodeFlux(ctp.TMSId, ctp.PlazaId, ctpDateTime, associatedCVCT.VehRegNo);
        ////////                                    TranscationDataFilterList = GetAssociatedData(ctp.TMSId, ctp.PlazaId, ctpDateTime, associatedCVCT.VehRegNo, "ANPR");
        ////////                                    if (TranscationDataFilterList.Count > 0)
        ////////                                    {
        ////////                                        if (TranscationDataFilterList.Count == 1)
        ////////                                        {
        ////////                                            #region Update in main transaction table
        ////////                                            try
        ////////                                            {
        ////////                                                #region Update CTP section in main transaction table
        ////////                                                LogMessage("Updating in main transaction table...");
        ////////                                                TranscationData Filtertransaction = TranscationDataFilterList[0];
        ////////                                                VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
        ////////                                                transaction.CrosstalkEntryId = Convert.ToInt32(Filtertransaction.IKEId);
        ////////                                                transaction.NodefluxEntryIdFront = Convert.ToInt32(Filtertransaction.AnprFId);
        ////////                                                transaction.NodefluxEntryIdRear = Convert.ToInt32(Filtertransaction.AnprRId);
        ////////                                                transaction.TMSId = Filtertransaction.TMSId;
        ////////                                                transaction.PlazaId = Filtertransaction.PlazaId;
        ////////                                                transaction.LaneId = Filtertransaction.LaneId;
        ////////                                                transaction.TransactionId = Filtertransaction.TranscationId;
        ////////                                                transaction.TransactionDateTime = Filtertransaction.TransactionDateTime;
        ////////                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateCrossTalkSection(transaction, ctpEntryId);//, eviVehicleClassId, eviVRN);
        ////////                                                var obj = TranscationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
        ////////                                                if (obj != null) obj.IKEId = ctpEntryId;
        ////////                                                LogMessage("IKE Transcation Updated successfully.");
        ////////                                                #endregion

        ////////                                                #region Get vehicle class matched to VRN front and rear
        ////////                                                //VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nfPacketFront;
        ////////                                                //VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nfPacketRear;
        ////////                                                Int32 vehicleClassIdFront = -1;
        ////////                                                Int32 vehicleClassIdRear = -1;
        ////////                                                if (transaction.NodefluxEntryIdFront > 0)
        ////////                                                {
        ////////                                                    //nfPacketFront = VaaaN.MLFF.Libraries.CommonLibrary.DAL.NodeFluxDAL.GetByEntryId(transaction.NodefluxEntryIdFront);
        ////////                                                    //if (nfPacketFront != null)
        ////////                                                    //{
        ////////                                                    //    vehicleClassIdFront = nfPacketFront.VehicleClassId;
        ////////                                                    //}
        ////////                                                    List<ANPRPktData> ANPRPktDataDetails = ANPRRecentDataList.Where(trans => (trans.PktId == transaction.NodefluxEntryIdFront)).OrderBy(o => o.PacketTimeStamp).ToList();
        ////////                                                    if (ANPRPktDataDetails.Count > 0)
        ////////                                                    {
        ////////                                                        vehicleClassIdFront = ANPRPktDataDetails[0].VehicleClassId;
        ////////                                                    }
        ////////                                                }
        ////////                                                if (transaction.NodefluxEntryIdRear > 0)
        ////////                                                {
        ////////                                                    //nfPacketRear = VaaaN.MLFF.Libraries.CommonLibrary.DAL.NodeFluxDAL.GetByEntryId(transaction.NodefluxEntryIdRear);
        ////////                                                    //if (nfPacketRear != null)
        ////////                                                    //{
        ////////                                                    //    vehicleClassIdRear = nfPacketRear.VehicleClassId;
        ////////                                                    //}
        ////////                                                    List<ANPRPktData> ANPRPktDataDetails = ANPRRecentDataList.Where(trans => (trans.PktId == transaction.NodefluxEntryIdRear)).OrderBy(o => o.PacketTimeStamp).ToList();
        ////////                                                    if (ANPRPktDataDetails.Count > 0)
        ////////                                                    {
        ////////                                                        vehicleClassIdRear = ANPRPktDataDetails[0].VehicleClassId;
        ////////                                                    }
        ////////                                                }
        ////////                                                #endregion

        ////////                                                //does the EVI class and AVC class matched? if not,  mark it as violation and leave it for manual review.
        ////////                                                //else deduct the balance
        ////////                                                #region Charging and SMSing
        ////////                                                //if anyone is matched, do the financial operation, no double charging
        ////////                                                if ((associatedCVCT.VehicleClassId == vehicleClassIdFront) || (associatedCVCT.VehicleClassId == vehicleClassIdRear))
        ////////                                                {
        ////////                                                    if (transaction.IsViolation == -1) //0 for normal, 1 for violtion, by default -1 (this means this is not not updated)
        ////////                                                    {
        ////////                                                        if (transaction.IsBalanceUpdated == -1) //0 for balance not updated, 1 means balance updated
        ////////                                                        {
        ////////                                                            if (associatedCACT != null)
        ////////                                                            {
        ////////                                                                //var obj1 = TranscationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
        ////////                                                                if (obj != null) obj.IsBalanceUpdated = 1;
        ////////                                                                //financial operation here
        ////////                                                                FinancialProcessing(associatedCVCT, associatedCACT, transaction);

        ////////                                                                //notification operation here
        ////////                                                                //NotificationProcessing(associatedCVCT, associatedCACT, transaction);
        ////////                                                            }
        ////////                                                            else
        ////////                                                            {
        ////////                                                                LogMessage("Associated customer account of this EVI id is found null.");
        ////////                                                            }
        ////////                                                        }
        ////////                                                    }
        ////////                                                }
        ////////                                                else
        ////////                                                {
        ////////                                                    //var obj1 = TranscationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
        ////////                                                    if (obj != null) obj.IsViolation = 1;
        ////////                                                    VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.MarkAsViolation(transaction);

        ////////                                                    //violation vms message
        ////////                                                }
        ////////                                                #endregion
        ////////                                            }
        ////////                                            catch (Exception ex)
        ////////                                            {
        ////////                                                LogMessage("Failed to insert crosstalk packet in main transaction table." + ex.Message);
        ////////                                            }
        ////////                                            #endregion
        ////////                                        }
        ////////                                        else
        ////////                                        {
        ////////                                            LogMessage("Abnormal case: Multiple entries found in the transaction table for this nodeflux packet in the specified time window (1 minute).");
        ////////                                        }
        ////////                                    }
        ////////                                    else
        ////////                                    {
        ////////                                        #region Insert into main transaction table
        ////////                                        try
        ////////                                        {
        ////////                                            LogMessage("No associated transaction found in transaction table. Inserting into main transaction table...");
        ////////                                            Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByCTP(ctp, ctpEntryId); //, eviVehicleClassId, eviVRN);
        ////////                                            TranscationDataList.Add(new TranscationData { TranscationId = TranscationId, LaneId = ctp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = associatedCVCT.VehRegNo, PlazaId = ctp.PlazaId, IKEId = ctpEntryId, CameraPosition = 0, TransactionDateTime = Convert.ToDateTime(ctp.TimeStamp), CurrentDateTime = DateTime.Now });
        ////////                                            LogMessage("Crosstalk packet inserted successfully.");
        ////////                                        }
        ////////                                        catch (Exception ex)
        ////////                                        {
        ////////                                            LogMessage("Failed to insert crosstalk packet in main transaction table." + ex.Message);
        ////////                                        }
        ////////                                        #endregion
        ////////                                    }
        ////////                                }
        ////////                                else
        ////////                                {
        ////////                                    LogMessage("Repeated crosstalk reporting. Discarded.");
        ////////                                }
        ////////                                #endregion
        ////////                            }
        ////////                            else
        ////////                            {
        ////////                                LogMessage("Tag " + ctp.ObjectId + " does not exist in the system. So discarded. Such type of vehicles will be detected by ANPR cameras.");
        ////////                            }
        ////////                        }
        ////////                        else
        ////////                        {
        ////////                            LogMessage("Invalid tag. " + ctp.ObjectId);
        ////////                        }
        ////////                        #endregion
        ////////                    }
        ////////                    else
        ////////                    {
        ////////                        LogMessage("Payload is not of crosstalk packet type.");
        ////////                    }
        ////////                }
        ////////                else
        ////////                {
        ////////                    LogMessage("Crosstalk packet's payload is null.");
        ////////                }
        ////////                #endregion
        ////////            }
        ////////            else
        ////////            {
        ////////                LogMessage("Current object is not valid crosstalk packet. " + m.Body.ToString());
        ////////            }
        ////////            #endregion
        ////////        }
        ////////        else
        ////////        {
        ////////            LogMessage("Message body is null.");
        ////////        }
        ////////    }
        ////////    else
        ////////    {
        ////////        LogMessage("Message is null.");
        ////////    }
        ////////}

        ////////private void ProcessQueueMessageANPR(Message m)
        ////////{
        ////////    if (m != null)
        ////////    {
        ////////        m.Formatter = new BinaryMessageFormatter();

        ////////        if (m.Body != null)
        ////////        {
        ////////            #region Processing packets
        ////////            if (m.Body is VaaaN.MLFF.Libraries.CommonLibrary.Common.NodeFluxPacket)
        ////////            {
        ////////                #region NodeFlux packet
        ////////                LogMessage("==ANPR==");

        ////////                VaaaN.MLFF.Libraries.CommonLibrary.Common.NodeFluxPacket nfpBody = (VaaaN.MLFF.Libraries.CommonLibrary.Common.NodeFluxPacket)m.Body;
        ////////                VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nfp = null;

        ////////                if (nfpBody.Payload != null)
        ////////                {
        ////////                    if (nfpBody.Payload is VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE)
        ////////                    {
        ////////                        nfp = (VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE)nfpBody.Payload;

        ////////                        if (nfp != null)
        ////////                        {
        ////////                            if (!string.IsNullOrEmpty(nfp.PlateNumber))
        ////////                            {
        ////////                                nfp.PlateNumber = nfp.PlateNumber.Trim(); //leading or trailing spaces create problem
        ////////                            }
        ////////                            else
        ////////                            {
        ////////                                nfp.PlateNumber = "";
        ////////                                LogMessage("Plate number is comming as blank!");
        ////////                            }

        ////////                            #region Update some fields in the CBE
        ////////                            try
        ////////                            {
        ////////                                LogMessage("Trying to update TMSId, CreationDate etc. in nodeflux packet..");
        ////////                                nfp.TMSId = currentTMSId;
        ////////                                //nfp.TimeStamp = ConversionDateTime(nfp.TimeStamp.ToString(), "nodeflux"); //already updated in WebAPI
        ////////                                LogMessage("Camera id is: " + nfp.CameraId);

        ////////                                //in case of nodeflux not giving lane id and plaza id peek laneid and plazaid by hardwareid. 
        ////////                                //hardwareid is assigned to specific lane of specific plaza.
        ////////                                VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane = GetLaneDetailByHardwareId(nfp.CameraId);

        ////////                                if (lane != null)
        ////////                                {
        ////////                                    nfp.GantryId = lane.PlazaId;
        ////////                                    nfp.LaneId = lane.LaneId;
        ////////                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE camera = GetHardwareById(nfp.CameraId);
        ////////                                    if (camera != null)
        ////////                                    {
        ////////                                        nfp.CameraPosition = camera.HardwarePosition.ToString(); //1 for front, 2 for rear
        ////////                                    }
        ////////                                    else
        ////////                                    {
        ////////                                        LogMessage("No camera found with id: " + nfp.CameraId);
        ////////                                    }
        ////////                                    //nodeflux is not giving vehicle class id, but name, so we have to get it by name
        ////////                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE nfVehicleClass = GetVehicleClassByName(nfp.VehicleClassName);
        ////////                                    if (nfVehicleClass != null)
        ////////                                    {
        ////////                                        nfp.VehicleClassId = nfVehicleClass.Id;
        ////////                                    }
        ////////                                    else
        ////////                                    {
        ////////                                        LogMessage("Vehicle class name might be wrong in nodeflux packet. (motorcycle, small, medium, big)" + nfp.VehicleClassName);
        ////////                                    }
        ////////                                }
        ////////                                else
        ////////                                {
        ////////                                    LogMessage("No lane detail found against the lane id: " + nfp.LaneId);
        ////////                                }

        ////////                                nfp.CreationDate = System.DateTime.Now;
        ////////                                nfp.ModifierId = -1;
        ////////                                nfp.ModificationDate = System.DateTime.Now;
        ////////                                LogMessage("Nodeflux CBE updated successfully");

        ////////                                #region event queue
        ////////                                try
        ////////                                {
        ////////                                    LogMessage("Trying to push nodeflux event to event queue...");
        ////////                                    //plaza id, plaza name, lane id, lane name, vrn, class, timestamp
        ////////                                    Message nodeFluxEventMessage = new Message();
        ////////                                    nodeFluxEventMessage.Formatter = new BinaryMessageFormatter();
        ////////                                    nodeFluxEventMessage.TimeToBeReceived = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.eventQueueTimeOut;

        ////////                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxEvent nfEvent = new Libraries.CommonLibrary.CBE.NodeFluxEvent();
        ////////                                    nfEvent.Timestamp = Convert.ToDateTime(nfp.TimeStamp);
        ////////                                    nfEvent.PlazaId = nfp.GantryId;
        ////////                                    nfEvent.PlazaName = GetPlazaNameById(nfp.GantryId);
        ////////                                    nfEvent.LaneName = lane.LaneName;
        ////////                                    nfEvent.VehicleClassName = nfp.VehicleClassName;
        ////////                                    nfEvent.VRN = nfp.PlateNumber;
        ////////                                    nfEvent.VideoURL = nfp.VideoURL;
        ////////                                    nfEvent.NumberPlatePath = nfp.PlateThumbnail;
        ////////                                    nfEvent.VehiclePath = nfp.VehicleThumbnail;
        ////////                                    if (nfp.CameraPosition == "1")
        ////////                                    {
        ////////                                        nfEvent.CameraLocation = "Front";
        ////////                                    }
        ////////                                    else if (nfp.CameraPosition == "2")
        ////////                                    {
        ////////                                        nfEvent.CameraLocation = "Rear";
        ////////                                    }
        ////////                                    else
        ////////                                    {
        ////////                                        nfEvent.CameraLocation = "Undefined";
        ////////                                    }

        ////////                                    nodeFluxEventMessage.Body = nfEvent;

        ////////                                    eventQueue.Send(nodeFluxEventMessage);
        ////////                                    LogMessage("NodeFlux event pushed to event queue successfully.");
        ////////                                }
        ////////                                catch (Exception ex)
        ////////                                {
        ////////                                    LogMessage("Exception in pushing nodeflux event to event queue. " + ex.ToString());
        ////////                                }
        ////////                                #endregion

        ////////                            }
        ////////                            catch (Exception ex)
        ////////                            {
        ////////                                LogMessage("Exception in updating nodeflux fields. " + ex.Message);
        ////////                            }
        ////////                            #endregion

        ////////                            //whether the vrn exists or does not exist, we have to push it to local table after checking the case of
        ////////                            //multiple reporting
        ////////                            LogMessage("Checking existing nodeflux records for: " + nfp.PlateNumber);
        ////////                            if (string.IsNullOrEmpty(nfp.PlateNumber) || nfp.PlateNumber == "Not Detected")
        ////////                            {
        ////////                                #region Send to local nodeflux database
        ////////                                try
        ////////                                {
        ////////                                    LogMessage("VRN is blank. Sending to local nodeflux table...");

        ////////                                    nfpEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.NodeFluxBLL.Insert(nfp);

        ////////                                    LogMessage("Nodeflux record with blank VRN inserted successfully.");
        ////////                                }
        ////////                                catch (Exception ex)
        ////////                                {
        ////////                                    #region Send data to Failed queue
        ////////                                    LogMessage("Failed to insert nodeflux packet. " + ex.Message);

        ////////                                    try
        ////////                                    {
        ////////                                        LogMessage("Trying to send nodeflux packet to failed queue...");

        ////////                                        m.Recoverable = true;
        ////////                                        failedQueueANPR.Send(m);

        ////////                                        LogMessage("Message sent nodflux packet to failed queue.");
        ////////                                    }
        ////////                                    catch (Exception exc)
        ////////                                    {
        ////////                                        LogMessage("***DATA LOST*** Failed to send to failed queue. nodeflux packet  is " + nfp.ToString() + exc.ToString());
        ////////                                    }
        ////////                                    #endregion
        ////////                                }
        ////////                                #endregion

        ////////                                #region Create a transaction Main transaction table 
        ////////                                try
        ////////                                {
        ////////                                    LogMessage("VRN is blank. Trying to insert into transaction table...");

        ////////                                    if (nfp.CameraPosition == "1") //1 means front, 2 means rear
        ////////                                    {
        ////////                                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPFront(nfp, nfpEntryId, 0);
        ////////                                        LogMessage("Transaction inserted by nf entry id front.");
        ////////                                    }
        ////////                                    else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
        ////////                                    {
        ////////                                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPRear(nfp, nfpEntryId, 0);
        ////////                                        LogMessage("Transaction inserted by nf entry id rear.");
        ////////                                    }
        ////////                                    else
        ////////                                    {
        ////////                                        LogMessage("Invalid camera position. " + nfp.CameraPosition);
        ////////                                    }
        ////////                                }
        ////////                                catch (Exception ex)
        ////////                                {
        ////////                                    LogMessage("Exception in inserting in main transaction table.");
        ////////                                }
        ////////                                #endregion
        ////////                            }
        ////////                            else if (!DoesExistInRecentNodeFluxPackets(nfp.GantryId, nfp.PlateNumber, Convert.ToDateTime(nfp.TimeStamp), Convert.ToInt32(nfp.CameraPosition)))
        ////////                            {
        ////////                                #region ANPR Data Found 
        ////////                                LogMessage("Does not exist in local nodeflux list.");

        ////////                                #region Send to local nodeflux database
        ////////                                try
        ////////                                {
        ////////                                    LogMessage("Sending to local nodeflux table. Plate number is: " + nfp.PlateNumber);

        ////////                                    nfpEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.NodeFluxBLL.Insert(nfp);
        ////////                                    ANPRRecentDataList.Add(new ANPRPktData { VehicleClassId = nfp.VehicleClassId, PktId = nfpEntryId, PacketTimeStamp = Convert.ToDateTime(nfp.TimeStamp), VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, cameraPosition = Convert.ToInt32(nfp.CameraPosition), currentDateTime = DateTime.Now });

        ////////                                    LogMessage("NodeFlux packet inserted successfully.");
        ////////                                }
        ////////                                catch (Exception ex)
        ////////                                {
        ////////                                    #region Send data to Failed queue
        ////////                                    LogMessage("Failed to insert nodeflux packet. " + ex.Message);

        ////////                                    try
        ////////                                    {
        ////////                                        LogMessage("Trying to send to failed queue...");

        ////////                                        m.Recoverable = true;
        ////////                                        failedQueueANPR.Send(m);

        ////////                                        LogMessage("Message sent to failed queue.");
        ////////                                    }
        ////////                                    catch (Exception exc)
        ////////                                    {
        ////////                                        LogMessage("***DATA LOST*** Failed to send to failed queue. nodeflux packet  is " + nfp.ToString() + exc.ToString());
        ////////                                    }
        ////////                                    #endregion
        ////////                                }
        ////////                                #endregion

        ////////                                //is the VRN actually exist in the system?
        ////////                                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE associatedCVNF = DoesVRNExist(nfp.PlateNumber);
        ////////                                DateTime nfpDateTime = Convert.ToDateTime(nfp.TimeStamp);
        ////////                                if (associatedCVNF != null)
        ////////                                {
        ////////                                    #region Checking VRN in recent transactions in tbl_transaction
        ////////                                    LogMessage("VRN exists in the system, checking whether it is in recent transactions or not...");

        ////////                                    LogMessage("Search criteria: " + nfp.TMSId + ", " + nfp.GantryId + ", " + nfpDateTime.ToString("dd/MM/yyyy hh:mm:ss.fff tt") + " " + nfp.PlateNumber);
        ////////                                    //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection associatedCrossTalkTrans = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetCorrespondingTransactionInCrossTalk(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber);
        ////////                                    TranscationDataFilterList = GetAssociatedData(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber, "IKE");
        ////////                                    #endregion

        ////////                                    if (TranscationDataFilterList.Count > 0)
        ////////                                    {
        ////////                                        #region Complete transaction, balance update and SMS
        ////////                                        if (TranscationDataFilterList.Count == 1)
        ////////                                        {
        ////////                                            LogMessage("Transaction found to update...");
        ////////                                            // VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = associatedCrossTalkTrans[0];
        ////////                                            TranscationData Filtertransaction = TranscationDataFilterList[0];
        ////////                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
        ////////                                            transaction.CrosstalkEntryId = Convert.ToInt32(Filtertransaction.IKEId);
        ////////                                            transaction.NodefluxEntryIdFront = Convert.ToInt32(Filtertransaction.AnprFId);
        ////////                                            transaction.NodefluxEntryIdRear = Convert.ToInt32(Filtertransaction.AnprRId);
        ////////                                            transaction.TMSId = Filtertransaction.TMSId;
        ////////                                            transaction.PlazaId = Filtertransaction.PlazaId;
        ////////                                            transaction.LaneId = Filtertransaction.LaneId;
        ////////                                            transaction.TransactionId = Filtertransaction.TranscationId;
        ////////                                            transaction.TransactionDateTime = Filtertransaction.TransactionDateTime;
        ////////                                            #region Get customer vehicle and customer account
        ////////                                            //Get vehicle details of the associated tagid (VRN, Customer Account id etc)
        ////////                                            LogMessage("Getting vehicle details...");
        ////////                                            var ObjectId = "";
        ////////                                            List<IkePktData> IkePktDataDetails = ikeRecentDataList.Where(trans => (trans.PktId == transaction.CrosstalkEntryId)).OrderBy(o => o.PacketTimeStamp).ToList();
        ////////                                            if (IkePktDataDetails.Count > 0)
        ////////                                            {
        ////////                                                ObjectId = IkePktDataDetails[0].ObjectId;
        ////////                                            }
        ////////                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicleInfo = DoesTagExist(ObjectId);

        ////////                                            //VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicleInfo = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetByTansactionCrosstalkEntryId(transaction.CrosstalkEntryId);

        ////////                                            //Get customer details of the associated tagid
        ////////                                            LogMessage("Getting customer details...");
        ////////                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccountInfo = GetCustomerAccountById(customerVehicleInfo.AccountId);
        ////////                                            #endregion

        ////////                                            #region Existing transaction update
        ////////                                            //in the main transaction table, if found update the nodeflux related fields. this is normal transaction
        ////////                                            if (nfp.CameraPosition == "1") //1 means front, 2 means rear
        ////////                                            {
        ////////                                                if (transaction.NodefluxEntryIdRear > 0)
        ////////                                                {
        ////////                                                    List<ANPRPktData> VehicleTimeStamp = ANPRRecentDataList.Where(trans => (trans.PlazaId == transaction.PlazaId && trans.PktId == transaction.NodefluxEntryIdRear)).OrderByDescending(o => o.PacketTimeStamp).ToList();
        ////////                                                    if (VehicleTimeStamp.Count > 0)
        ////////                                                    {
        ////////                                                        transaction.VehicleSpeed = CalculateSpeed(nfpDateTime, VehicleTimeStamp[0].PacketTimeStamp);
        ////////                                                    }
        ////////                                                }
        ////////                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPFront(transaction, nfpEntryId);
        ////////                                                //VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateNodefluxSectionFront(transaction, nfpEntryId);
        ////////                                                LogMessage("Transaction updated by nf entry id front.");
        ////////                                                var obj = TranscationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
        ////////                                                if (obj != null) obj.AnprFId = nfpEntryId;
        ////////                                            }
        ////////                                            else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
        ////////                                            {
        ////////                                                if (transaction.NodefluxEntryIdFront > 0)
        ////////                                                {
        ////////                                                    List<ANPRPktData> VehicleTimeStamp = ANPRRecentDataList.Where(trans => (trans.PlazaId == transaction.PlazaId && trans.PktId == transaction.NodefluxEntryIdFront)).OrderByDescending(o => o.PacketTimeStamp).ToList();
        ////////                                                    if (VehicleTimeStamp.Count > 0)
        ////////                                                    {
        ////////                                                        transaction.VehicleSpeed = CalculateSpeed(nfpDateTime, VehicleTimeStamp[0].PacketTimeStamp);
        ////////                                                    }
        ////////                                                }
        ////////                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPRear(transaction, nfpEntryId);
        ////////                                                //VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateNodefluxSectionRear(transaction, nfpEntryId);
        ////////                                                var obj = TranscationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
        ////////                                                if (obj != null) obj.AnprRId = nfpEntryId;
        ////////                                                LogMessage("Transaction updated by nf entry id rear.");
        ////////                                            }
        ////////                                            else
        ////////                                            {
        ////////                                                LogMessage("Invalid camera position. " + nfp.CameraPosition);
        ////////                                            }
        ////////                                            #endregion

        ////////                                            #region charging and notification
        ////////                                            //if the transactionno marked as violation before?
        ////////                                            if (Filtertransaction.IsViolation == -1) //0 for normal, 1 for violtion, by default -1 (not updated)
        ////////                                            {
        ////////                                                LogMessage("Transaction is not marked as violation previously. Going to check violation...");
        ////////                                                if (customerVehicleInfo.VehicleClassId == nfp.VehicleClassId)
        ////////                                                {
        ////////                                                    LogMessage("Tag class and NF class matched. Going to financial and notification processing...");
        ////////                                                    if (Filtertransaction.IsBalanceUpdated == -1) //0 for balance not updated, 1 means balance updated
        ////////                                                    {
        ////////                                                        var obj = TranscationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
        ////////                                                        if (obj != null) obj.IsBalanceUpdated = 1;
        ////////                                                        //financial operation here
        ////////                                                        FinancialProcessing(customerVehicleInfo, customerAccountInfo, transaction);
        ////////                                                        LogMessage("Financial processing has been done.");

        ////////                                                        //notification operation here
        ////////                                                        // NotificationProcessing(customerVehicleInfo, customerAccountInfo, transaction);
        ////////                                                        //LogMessage("Notification processing has been done.");
        ////////                                                    }
        ////////                                                    else
        ////////                                                    {
        ////////                                                        LogMessage("Balance is already updated.");
        ////////                                                    }
        ////////                                                }
        ////////                                                else
        ////////                                                {
        ////////                                                    LogMessage("Tag class and NF class does not match. Transaction will be marked as violation.");
        ////////                                                    //mark the transaction as violation and leave it for manual review
        ////////                                                    var obj = TranscationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
        ////////                                                    if (obj != null) obj.IsViolation = 1;
        ////////                                                    VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.MarkAsViolation(transaction);
        ////////                                                    LogMessage("Transaction is marked as violation.");
        ////////                                                }
        ////////                                            }
        ////////                                            #endregion


        ////////                                        }
        ////////                                        else
        ////////                                        {
        ////////                                            LogMessage("Abnormal case: Multiple entries found in the transaction table for this nodeflux packet in the specified time window (1 minute).");
        ////////                                        }
        ////////                                        #endregion
        ////////                                    }
        ////////                                    else
        ////////                                    {
        ////////                                        //no associated crosstalk transaction found
        ////////                                        #region Create/ Update a transaction Main transaction table 
        ////////                                        try
        ////////                                        {
        ////////                                            LogMessage("VRN exists but no associated crosstalk transaction found in transaction table. Checking associated nodeflux transaction...");

        ////////                                            //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection associatedNodeFluxTrans = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetCorrespondingTransactionInNodeFlux(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber);
        ////////                                            TranscationDataFilterList = GetAssociatedData(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber, "ANPR");
        ////////                                            if (TranscationDataFilterList.Count > 0)
        ////////                                            {
        ////////                                                #region Update Tran Table if ANPR found
        ////////                                                if (TranscationDataFilterList.Count == 1)
        ////////                                                {
        ////////                                                    LogMessage("Transaction found to update...");
        ////////                                                    //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = associatedNodeFluxTrans[0];
        ////////                                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
        ////////                                                    TranscationData Filtertransaction = TranscationDataFilterList[0];
        ////////                                                    transaction.NodefluxEntryIdFront = Convert.ToInt32(Filtertransaction.AnprFId);
        ////////                                                    transaction.NodefluxEntryIdRear = Convert.ToInt32(Filtertransaction.AnprRId);
        ////////                                                    transaction.TMSId = Filtertransaction.TMSId;
        ////////                                                    transaction.PlazaId = Filtertransaction.PlazaId;
        ////////                                                    transaction.LaneId = Filtertransaction.LaneId;
        ////////                                                    transaction.TransactionId = Filtertransaction.TranscationId;
        ////////                                                    transaction.TransactionDateTime = Filtertransaction.TransactionDateTime;
        ////////                                                    if (nfp.CameraPosition == "1") //1 means front, 2 means rear
        ////////                                                    {
        ////////                                                        // update the VEHICLE speed
        ////////                                                        if (transaction.NodefluxEntryIdRear > 0)
        ////////                                                        {
        ////////                                                            List<ANPRPktData> VehicleTimeStamp = ANPRRecentDataList.Where(trans => (trans.PlazaId == transaction.PlazaId && trans.PktId == transaction.NodefluxEntryIdRear)).OrderByDescending(o => o.PacketTimeStamp).ToList();
        ////////                                                            if (VehicleTimeStamp.Count > 0)
        ////////                                                            {
        ////////                                                                transaction.VehicleSpeed = CalculateSpeed(nfpDateTime, VehicleTimeStamp[0].PacketTimeStamp);
        ////////                                                            }
        ////////                                                        }
        ////////                                                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPFront(transaction, nfpEntryId);
        ////////                                                        LogMessage("nf entry id front has been updated in transaction.");
        ////////                                                        var obj = TranscationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
        ////////                                                        if (obj != null) obj.AnprFId = nfpEntryId;
        ////////                                                    }
        ////////                                                    else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
        ////////                                                    {
        ////////                                                        // update the VEHICLE speed
        ////////                                                        if (transaction.NodefluxEntryIdFront > 0)
        ////////                                                        {
        ////////                                                            List<ANPRPktData> VehicleTimeStamp = ANPRRecentDataList.Where(trans => (trans.PlazaId == transaction.PlazaId && trans.PktId == transaction.NodefluxEntryIdFront)).OrderByDescending(o => o.PacketTimeStamp).ToList();
        ////////                                                            if (VehicleTimeStamp.Count > 0)
        ////////                                                            {
        ////////                                                                transaction.VehicleSpeed = CalculateSpeed(nfpDateTime, VehicleTimeStamp[0].PacketTimeStamp);
        ////////                                                            }
        ////////                                                        }
        ////////                                                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPRear(transaction, nfpEntryId);
        ////////                                                        var obj = TranscationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
        ////////                                                        if (obj != null) obj.AnprRId = nfpEntryId;
        ////////                                                        LogMessage("nf entry id rear has been updated in transaction.");
        ////////                                                    }
        ////////                                                    else
        ////////                                                    {
        ////////                                                        LogMessage("Invalid camera position. " + nfp.CameraPosition);
        ////////                                                    }
        ////////                                                }
        ////////                                                else
        ////////                                                {
        ////////                                                    LogMessage("Abnormal case.");
        ////////                                                }
        ////////                                                #endregion
        ////////                                            }
        ////////                                            else
        ////////                                            {
        ////////                                                #region No Associatied Found
        ////////                                                if (nfp.CameraPosition == "1") //1 means front, 2 means rear
        ////////                                                {
        ////////                                                    Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPFront(nfp, nfpEntryId, 1);
        ////////                                                    TranscationDataList.Add(new TranscationData { TranscationId = TranscationId, LaneId = nfp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, AnprFId = nfpEntryId, CameraPosition = 1, TransactionDateTime = Convert.ToDateTime(nfp.TimeStamp), CurrentDateTime = DateTime.Now });
        ////////                                                    LogMessage("Transaction inserted by nf entry id front.");
        ////////                                                }
        ////////                                                else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
        ////////                                                {
        ////////                                                    Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPRear(nfp, nfpEntryId, 1);
        ////////                                                    TranscationDataList.Add(new TranscationData { TranscationId = TranscationId, LaneId = nfp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, AnprFId = nfpEntryId, CameraPosition = 2, TransactionDateTime = Convert.ToDateTime(nfp.TimeStamp), CurrentDateTime = DateTime.Now });
        ////////                                                    LogMessage("Transaction inserted by nf entry id rear.");
        ////////                                                }
        ////////                                                else
        ////////                                                {
        ////////                                                    LogMessage("Invalid camera position. " + nfp.CameraPosition);
        ////////                                                }
        ////////                                                #endregion
        ////////                                            }
        ////////                                        }
        ////////                                        catch (Exception ex)
        ////////                                        {
        ////////                                            LogMessage("Exception in inserting/updating in main transaction table. " + ex.ToString());
        ////////                                        }
        ////////                                        #endregion
        ////////                                    }
        ////////                                }
        ////////                                else
        ////////                                {
        ////////                                    //no associated vrn found in Database
        ////////                                    #region Create/ Update a transaction Main transaction table 
        ////////                                    try
        ////////                                    {
        ////////                                        LogMessage("VRN not exists. Checking associated nodeflux transaction...");

        ////////                                        //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection associatedNodeFluxTrans = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetCorrespondingTransactionInNodeFlux(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber);
        ////////                                        TranscationDataFilterList = GetAssociatedData(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber, "ANPR");
        ////////                                        if (TranscationDataFilterList.Count > 0)
        ////////                                        {
        ////////                                            #region Associate ANPR Found
        ////////                                            if (TranscationDataFilterList.Count == 1)
        ////////                                            {
        ////////                                                #region ANPR Associated Data found in ANPR
        ////////                                                TranscationData Filtertransaction = TranscationDataFilterList[0];
        ////////                                                LogMessage("Transaction found to update...");
        ////////                                                VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
        ////////                                                transaction.CrosstalkEntryId = Convert.ToInt32(Filtertransaction.IKEId);
        ////////                                                transaction.NodefluxEntryIdFront = Convert.ToInt32(Filtertransaction.AnprFId);
        ////////                                                transaction.NodefluxEntryIdRear = Convert.ToInt32(Filtertransaction.AnprRId);
        ////////                                                transaction.TMSId = Filtertransaction.TMSId;
        ////////                                                transaction.PlazaId = Filtertransaction.PlazaId;
        ////////                                                transaction.LaneId = Filtertransaction.LaneId;
        ////////                                                transaction.TransactionId = Filtertransaction.TranscationId;
        ////////                                                transaction.TransactionDateTime = Filtertransaction.TransactionDateTime;
        ////////                                                if (nfp.CameraPosition == "1") //1 means front, 2 means rear
        ////////                                                {
        ////////                                                    // update the VEHICLE speed
        ////////                                                    if (transaction.NodefluxEntryIdRear > 0)
        ////////                                                    {
        ////////                                                        List<ANPRPktData> VehicleTimeStamp = ANPRRecentDataList.Where(trans => (trans.PlazaId == transaction.PlazaId && trans.PktId == transaction.NodefluxEntryIdRear)).OrderByDescending(o => o.PacketTimeStamp).ToList();
        ////////                                                        if (VehicleTimeStamp.Count > 0)
        ////////                                                        {
        ////////                                                            transaction.VehicleSpeed = CalculateSpeed(nfpDateTime, VehicleTimeStamp[0].PacketTimeStamp);
        ////////                                                        }
        ////////                                                    }
        ////////                                                    VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPFront(transaction, nfpEntryId);
        ////////                                                    var obj = TranscationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
        ////////                                                    if (obj != null) obj.AnprFId = nfpEntryId;
        ////////                                                    LogMessage("nf entry id front has been updated in transaction.");
        ////////                                                }
        ////////                                                else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
        ////////                                                {
        ////////                                                    // update the VEHICLE speed
        ////////                                                    if (transaction.NodefluxEntryIdFront > 0)
        ////////                                                    {
        ////////                                                        List<ANPRPktData> VehicleTimeStamp = ANPRRecentDataList.Where(trans => (trans.PlazaId == transaction.PlazaId && trans.PktId == transaction.NodefluxEntryIdFront)).OrderByDescending(o => o.PacketTimeStamp).ToList();
        ////////                                                        if (VehicleTimeStamp.Count > 0)
        ////////                                                        {
        ////////                                                            transaction.VehicleSpeed = CalculateSpeed(VehicleTimeStamp[0].PacketTimeStamp, nfpDateTime);
        ////////                                                        }
        ////////                                                    }
        ////////                                                    VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPRear(transaction, nfpEntryId);
        ////////                                                    var obj = TranscationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
        ////////                                                    if (obj != null) obj.AnprRId = nfpEntryId;

        ////////                                                    LogMessage("nf entry id rear has been updated in transaction.");
        ////////                                                }
        ////////                                                else
        ////////                                                {
        ////////                                                    LogMessage("Invalid camera position. " + nfp.CameraPosition);
        ////////                                                }
        ////////                                                #endregion
        ////////                                            }
        ////////                                            else
        ////////                                            {
        ////////                                                LogMessage("Abnormal case.");
        ////////                                            }
        ////////                                            #endregion
        ////////                                        }
        ////////                                        else
        ////////                                        {
        ////////                                            #region No Transcation Found
        ////////                                            if (nfp.CameraPosition == "1") //1 means front, 2 means rear
        ////////                                            {

        ////////                                                Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPFront(nfp, nfpEntryId, 2);
        ////////                                                LogMessage("Transaction inserted by nf entry id front.");
        ////////                                                TranscationDataList.Add(new TranscationData { TranscationId = TranscationId, LaneId = nfp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, AnprFId = nfpEntryId, CameraPosition = 1, TransactionDateTime = Convert.ToDateTime(nfp.TimeStamp), CurrentDateTime = DateTime.Now });
        ////////                                            }
        ////////                                            else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
        ////////                                            {

        ////////                                                Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPRear(nfp, nfpEntryId, 2);
        ////////                                                TranscationDataList.Add(new TranscationData { TranscationId = TranscationId, LaneId = nfp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, AnprRId = nfpEntryId, CameraPosition = 2, TransactionDateTime = Convert.ToDateTime(nfp.TimeStamp), CurrentDateTime = DateTime.Now });
        ////////                                                LogMessage("Transaction inserted by nf entry id rear.");
        ////////                                            }
        ////////                                            else
        ////////                                            {
        ////////                                                LogMessage("Invalid camera position. " + nfp.CameraPosition);
        ////////                                            }
        ////////                                            #endregion
        ////////                                        }
        ////////                                    }
        ////////                                    catch (Exception ex)
        ////////                                    {
        ////////                                        LogMessage("Exception in inserting/updating in main transaction table. " + ex.ToString());
        ////////                                    }
        ////////                                    #endregion

        ////////                                    //***if exists in tbl_transaction updat it, otherwise insert***//


        ////////                                }
        ////////                                #endregion
        ////////                            }
        ////////                            else
        ////////                            {
        ////////                                LogMessage("Repeated nodeflux reporting. Discarded. Plate Number is: " + nfp.PlateNumber);
        ////////                            }
        ////////                        }
        ////////                        else
        ////////                        {
        ////////                            LogMessage("nfp is null.");
        ////////                        }
        ////////                    }
        ////////                    else
        ////////                    {
        ////////                        LogMessage("Payload is not of nodeflux packet type.");
        ////////                    }
        ////////                }
        ////////                else
        ////////                {
        ////////                    LogMessage("Nodeflux packet's payload is null.");
        ////////                }
        ////////                #endregion                       
        ////////            }
        ////////            else
        ////////            {
        ////////                LogMessage("Current object is not valid packet. " + m.Body.ToString());
        ////////            }
        ////////            #endregion
        ////////        }
        ////////        else
        ////////        {
        ////////            LogMessage("Message body is null.");
        ////////        }
        ////////    }
        ////////    else
        ////////    {
        ////////        LogMessage("Message is null.");
        ////////    }
        ////////}
        #endregion

        int packetCounter = 0;
        private void ProcessQueueMessage(Message m)
        {
            if (m != null)
            {
                m.Formatter = new BinaryMessageFormatter();

                //delete old records from lists------------------------
                packetCounter = packetCounter + 1;
                if (packetCounter > 10)
                {
                    //keep only last 3 minutes data
                    rfidRecentDataList.RemoveAll(e => e.currentDateTime < DateTime.Now.AddMinutes(-3));
                    anprRecentDataList.RemoveAll(e => e.currentDateTime < DateTime.Now.AddMinutes(-3));
                    transcationDataList.RemoveAll(e => e.CurrentDateTime < DateTime.Now.AddMinutes(-3));
                    recentlyProcessedTagsList.RemoveAll(e => e.CurrentDateTime < DateTime.Now.AddMinutes(-3)); //newly added

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

                                #region parsing tagid
                                ctp.ObjectId = ctp.ObjectId.Trim(); //otherwise trailing and leading spaces create problems
                                TagStructure ts = ParseEPC(ctp.ObjectId);

                                int eviClass = -1;
                                string eviVRN = "";
                                if (ts != null)
                                {
                                    //these two parsed out things are not used anywhere else - CJS
                                    eviClass = ts.ClassId;
                                    eviVRN = ts.VRN;

                                    LogMessage("Checking the tag exists in the system or not. " + ctp.ObjectId);
                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE associatedCVCT = DoesTagExist(ctp.ObjectId);
                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE associatedCACT = null;
                                    if (associatedCVCT != null)
                                    {
                                        LogMessage("Tag exists. Getting the corresponding customer account...");

                                        associatedCACT = GetCustomerAccountById(associatedCVCT.AccountId);

                                        LogMessage("The associated accoount is: " + associatedCACT.FirstName + " " + associatedCACT.LastName);

                                        #region Update some fields in the CBE
                                        try
                                        {
                                            LogMessage("Trying to update TMSId, Creation Date etc. in the CBE...");
                                            ctp.TMSId = currentTMSId;
                                            //ctp.TimeStamp = ConversionDateTime(ctp.TimeStamp, "crosstalk"); //this is handled in the API itself

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

                                            #region Insert Into Even Queue for Live Event
                                            try
                                            {
                                                LogMessage("Trying to push crosstalk event to event queue...");
                                                //plaza id, plaza name, lane id, lane name, vrn, class, timestamp
                                                Message crosstalkEventMessage = new Message();
                                                crosstalkEventMessage.Formatter = new BinaryMessageFormatter();
                                                crosstalkEventMessage.TimeToBeReceived = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.eventQueueTimeOut;

                                                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkEvent ctEvent = new Libraries.CommonLibrary.CBE.CrossTalkEvent();

                                                ctEvent.Timestamp = Convert.ToDateTime(ctp.TimeStamp);// Convert.ToDateTime(ConversionDateTime(ctp.TimeStamp, "crosstalk"));
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
                                            #endregion
                                        }
                                        catch (Exception ex)
                                        {
                                            LogMessage("Exception in updating some fields. " + ex.ToString());
                                        }
                                        #endregion

                                        //does recent transactions contains this tag? it may be reported repetatively. 
                                        //check in tbl_crosstalk_packet's most recent transactions of the same plaza

                                        #region Check in recent crosstalk packets
                                        LogMessage("Checking tag has been already inserted..." + ctp.PlazaId + ", " + ctp.ObjectId + ", " + ctp.TimeStamp + ", " + ctp.LocationId);
                                        if (!DoesExistInRecentCrossTalkPackets(ctp.PlazaId, ctp.ObjectId, Convert.ToDateTime(ctp.TimeStamp), ctp.LocationId))
                                        {
                                            #region Send to local database 
                                            //(same tag but separate location id hoga toh local database main bhejega)
                                            try
                                            {
                                                LogMessage("Sending crosstalk packet to local database. EPC: " + ctp.ObjectId + " Location: " + ctp.LocationId);

                                                ctpEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CrossTalkBLL.Insert(ctp);
                                                rfidRecentDataList.Add(new IkePktData { LocationId = ctp.LocationId, PktId = ctpEntryId, PacketTimeStamp = Convert.ToDateTime(ctp.TimeStamp), VehicleClassId = associatedCVCT.VehicleClassId, ObjectId = ctp.ObjectId, PlazaId = ctp.PlazaId, currentDateTime = DateTime.Now });

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
                                                    //failedQueueIKE.Send(m);
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

                                            LogMessage("Checking this tag is recently transacted... (may come from front and rear antenna both)");
                                            //if transaction is done for this tag recently no need to do following things. Check...
                                            var objRecentTag = recentlyProcessedTagsList.FirstOrDefault(x => x.TagId == ctp.ObjectId);

                                            if (objRecentTag == null)
                                            {
                                                //is the associated VRN is already inserted in the transaction table by nodeflux front or nodeflux rear camera?
                                                //if inserted look by associated vrn and update, if not create a new transaction
                                                LogMessage("Searching associated transaction exists in the transaction table or not...");
                                                DateTime ctpDateTime = Convert.ToDateTime(ctp.TimeStamp);
                                                LogMessage("Search criteria: " + ctp.TMSId + ", " + ctp.PlazaId + ", " + ctpDateTime.ToString("dd/MM/yyyy hh:mm:ss.fff tt") + ", " + associatedCVCT.VehRegNo);
                                                //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection trans1 = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetCorrespondingTransactionInNodeFlux(ctp.TMSId, ctp.PlazaId, ctpDateTime, associatedCVCT.VehRegNo);
                                                TranscationDataFilterList = GetAssociatedData(ctp.TMSId, ctp.PlazaId, ctpDateTime, associatedCVCT.VehRegNo, "ANPR");
                                                if (TranscationDataFilterList.Count > 0)
                                                {
                                                    if (TranscationDataFilterList.Count == 1)
                                                    {
                                                        #region Update in main transaction table
                                                        try
                                                        {
                                                            #region Update CTP section in main transaction table
                                                            LogMessage("Updating in main transaction table...");
                                                            TranscationData Filtertransaction = TranscationDataFilterList[0];
                                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
                                                            transaction.CrosstalkEntryId = Convert.ToInt32(Filtertransaction.IKEId);
                                                            transaction.NodefluxEntryIdFront = Convert.ToInt32(Filtertransaction.AnprFId);
                                                            transaction.NodefluxEntryIdRear = Convert.ToInt32(Filtertransaction.AnprRId);
                                                            transaction.TMSId = Filtertransaction.TMSId;
                                                            transaction.PlazaId = Filtertransaction.PlazaId;
                                                            transaction.LaneId = Filtertransaction.LaneId;
                                                            transaction.TransactionId = Filtertransaction.TranscationId;
                                                            transaction.TransactionDateTime = Filtertransaction.TransactionDateTime;
                                                            VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateCrossTalkSection(transaction, ctpEntryId);//, eviVehicleClassId, eviVRN);
                                                            var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                            if (obj != null) obj.IKEId = ctpEntryId;
                                                            LogMessage("RFID Transcation updated successfully.");
                                                            #endregion

                                                            #region Get vehicle class matched to VRN front and rear
                                                            //VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nfPacketFront;
                                                            //VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nfPacketRear;
                                                            Int32 vehicleClassIdFront = -1;
                                                            Int32 vehicleClassIdRear = -1;
                                                            if (transaction.NodefluxEntryIdFront > 0)
                                                            {
                                                                //nfPacketFront = VaaaN.MLFF.Libraries.CommonLibrary.DAL.NodeFluxDAL.GetByEntryId(transaction.NodefluxEntryIdFront);
                                                                //if (nfPacketFront != null)
                                                                //{
                                                                //    vehicleClassIdFront = nfPacketFront.VehicleClassId;
                                                                //}
                                                                List<ANPRPktData> ANPRPktDataDetails = anprRecentDataList.Where(trans => (trans.PktId == transaction.NodefluxEntryIdFront)).OrderBy(o => o.PacketTimeStamp).ToList();
                                                                if (ANPRPktDataDetails.Count > 0)
                                                                {
                                                                    vehicleClassIdFront = ANPRPktDataDetails[0].VehicleClassId;
                                                                }
                                                            }
                                                            if (transaction.NodefluxEntryIdRear > 0)
                                                            {
                                                                //nfPacketRear = VaaaN.MLFF.Libraries.CommonLibrary.DAL.NodeFluxDAL.GetByEntryId(transaction.NodefluxEntryIdRear);
                                                                //if (nfPacketRear != null)
                                                                //{
                                                                //    vehicleClassIdRear = nfPacketRear.VehicleClassId;
                                                                //}
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
                                                            if ((associatedCVCT.VehicleClassId == vehicleClassIdFront) || (associatedCVCT.VehicleClassId == vehicleClassIdRear))
                                                            {
                                                                if (transaction.IsViolation == -1) //0 for normal, 1 for violtion, by default -1 (this means this is not not updated)
                                                                {
                                                                    if (transaction.IsBalanceUpdated == -1) //0 for balance not updated, 1 means balance updated
                                                                    {
                                                                        if (associatedCACT != null)
                                                                        {
                                                                            //var obj1 = TranscationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                                            if (obj != null) obj.IsBalanceUpdated = 1;
                                                                            //financial operation here
                                                                            FinancialProcessing(associatedCVCT, associatedCACT, transaction);

                                                                            //notification operation here
                                                                            //NotificationProcessing(associatedCVCT, associatedCACT, transaction);
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
                                                                //var obj1 = TranscationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                                if (obj != null) obj.IsViolation = 1;
                                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.MarkAsViolation(transaction);

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
                                                        Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByCTP(ctp, ctpEntryId); //, eviVehicleClassId, eviVRN);
                                                        transcationDataList.Add(new TranscationData { TranscationId = TranscationId, LaneId = ctp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = associatedCVCT.VehRegNo, PlazaId = ctp.PlazaId, IKEId = ctpEntryId, CameraPosition = 0, TransactionDateTime = Convert.ToDateTime(ctp.TimeStamp), CurrentDateTime = DateTime.Now });
                                                        LogMessage("Crosstalk packet inserted successfully.");
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        LogMessage("Failed to insert crosstalk packet in main transaction table." + ex.Message);
                                                    }
                                                    #endregion
                                                }

                                                //adding the tagid to recently processed tag list
                                                recentlyProcessedTagsList.Add(new TagData { TagId = ctp.ObjectId, CurrentDateTime = DateTime.Now });
                                            }
                                            else
                                            {
                                                LogMessage("The transaction of this tag has been processed recently (from a different location id), so, discarded.");
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
                                        //nfp.TimeStamp = ConversionDateTime(nfp.TimeStamp.ToString(), "nodeflux"); //already updated in WebAPI
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
                                    if (string.IsNullOrEmpty(nfp.PlateNumber) || nfp.PlateNumber == "Not Detected")
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
                                                LogMessage("Trying to send nodeflux packet to failed queue...");

                                                m.Recoverable = true;
                                                //failedQueueANPR.Send(m);
                                                failedQueue.Send(m);

                                                LogMessage("Message sent nodflux packet to failed queue.");
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
                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPFront(nfp, nfpEntryId, 0);
                                                LogMessage("Transaction inserted by nf entry id front.");
                                            }
                                            else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
                                            {
                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPRear(nfp, nfpEntryId, 0);
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
                                        #region ANPR Data Found 
                                        LogMessage("Does not exist in local nodeflux list.");

                                        #region Send to local nodeflux database
                                        try
                                        {
                                            LogMessage("Sending to local nodeflux table. Plate number is: " + nfp.PlateNumber);

                                            nfpEntryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.NodeFluxBLL.Insert(nfp);
                                            anprRecentDataList.Add(new ANPRPktData { VehicleClassId = nfp.VehicleClassId, PktId = nfpEntryId, PacketTimeStamp = Convert.ToDateTime(nfp.TimeStamp), VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, cameraPosition = Convert.ToInt32(nfp.CameraPosition), currentDateTime = DateTime.Now });

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
                                                //failedQueueANPR.Send(m);
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
                                        DateTime nfpDateTime = Convert.ToDateTime(nfp.TimeStamp);
                                        if (associatedCVNF != null)
                                        {
                                            #region Checking VRN in recent transactions in tbl_transaction
                                            LogMessage("VRN exists in the system, checking whether it is in recent transactions or not...");

                                            LogMessage("Search criteria: " + nfp.TMSId + ", " + nfp.GantryId + ", " + nfpDateTime.ToString("dd/MM/yyyy hh:mm:ss.fff tt") + " " + nfp.PlateNumber);
                                            //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection associatedCrossTalkTrans = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetCorrespondingTransactionInCrossTalk(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber);
                                            TranscationDataFilterList = GetAssociatedData(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber, "RFID");
                                            #endregion

                                            if (TranscationDataFilterList.Count > 0)
                                            {
                                                #region Complete transaction, balance update and SMS
                                                if (TranscationDataFilterList.Count == 1)
                                                {
                                                    LogMessage("Transaction found to update...");
                                                    // VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = associatedCrossTalkTrans[0];
                                                    TranscationData Filtertransaction = TranscationDataFilterList[0];
                                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
                                                    transaction.CrosstalkEntryId = Convert.ToInt32(Filtertransaction.IKEId);
                                                    transaction.NodefluxEntryIdFront = Convert.ToInt32(Filtertransaction.AnprFId);
                                                    transaction.NodefluxEntryIdRear = Convert.ToInt32(Filtertransaction.AnprRId);
                                                    transaction.TMSId = Filtertransaction.TMSId;
                                                    transaction.PlazaId = Filtertransaction.PlazaId;
                                                    transaction.LaneId = Filtertransaction.LaneId;
                                                    transaction.TransactionId = Filtertransaction.TranscationId;
                                                    transaction.TransactionDateTime = Filtertransaction.TransactionDateTime;
                                                    #region Get customer vehicle and customer account
                                                    //Get vehicle details of the associated tagid (VRN, Customer Account id etc)
                                                    LogMessage("Getting vehicle details...");
                                                    var ObjectId = "";
                                                    List<IkePktData> IkePktDataDetails = rfidRecentDataList.Where(trans => (trans.PktId == transaction.CrosstalkEntryId)).OrderBy(o => o.PacketTimeStamp).ToList();
                                                    if (IkePktDataDetails.Count > 0)
                                                    {
                                                        ObjectId = IkePktDataDetails[0].ObjectId;
                                                    }
                                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicleInfo = DoesTagExist(ObjectId);

                                                    //VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicleInfo = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetByTansactionCrosstalkEntryId(transaction.CrosstalkEntryId);

                                                    //Get customer details of the associated tagid
                                                    LogMessage("Getting customer details...");
                                                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccountInfo = GetCustomerAccountById(customerVehicleInfo.AccountId);
                                                    #endregion

                                                    #region Existing transaction update
                                                    //in the main transaction table, if found update the nodeflux related fields. this is normal transaction
                                                    if (nfp.CameraPosition == "1") //1 means front, 2 means rear
                                                    {
                                                        if (transaction.NodefluxEntryIdRear > 0)
                                                        {
                                                            List<ANPRPktData> VehicleTimeStamp = anprRecentDataList.Where(trans => (trans.PlazaId == transaction.PlazaId && trans.PktId == transaction.NodefluxEntryIdRear)).OrderByDescending(o => o.PacketTimeStamp).ToList();
                                                            if (VehicleTimeStamp.Count > 0)
                                                            {
                                                                transaction.VehicleSpeed = CalculateSpeed(nfpDateTime, VehicleTimeStamp[0].PacketTimeStamp);
                                                            }
                                                        }
                                                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPFront(transaction, nfpEntryId);
                                                        //VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateNodefluxSectionFront(transaction, nfpEntryId);
                                                        LogMessage("Transaction updated by nf entry id front.");
                                                        var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                        if (obj != null) obj.AnprFId = nfpEntryId;
                                                    }
                                                    else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
                                                    {
                                                        if (transaction.NodefluxEntryIdFront > 0)
                                                        {
                                                            List<ANPRPktData> VehicleTimeStamp = anprRecentDataList.Where(trans => (trans.PlazaId == transaction.PlazaId && trans.PktId == transaction.NodefluxEntryIdFront)).OrderByDescending(o => o.PacketTimeStamp).ToList();
                                                            if (VehicleTimeStamp.Count > 0)
                                                            {
                                                                transaction.VehicleSpeed = CalculateSpeed(nfpDateTime, VehicleTimeStamp[0].PacketTimeStamp);
                                                            }
                                                        }
                                                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPRear(transaction, nfpEntryId);
                                                        //VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateNodefluxSectionRear(transaction, nfpEntryId);
                                                        var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                        if (obj != null) obj.AnprRId = nfpEntryId;
                                                        LogMessage("Transaction updated by nf entry id rear.");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Invalid camera position. " + nfp.CameraPosition);
                                                    }
                                                    #endregion

                                                    #region charging and notification
                                                    //if the transactionno marked as violation before?
                                                    if (Filtertransaction.IsViolation == -1) //0 for normal, 1 for violtion, by default -1 (not updated)
                                                    {
                                                        LogMessage("Transaction is not marked as violation previously. Going to check violation...");
                                                        if (customerVehicleInfo.VehicleClassId == nfp.VehicleClassId)
                                                        {
                                                            LogMessage("Tag class and NF class matched. Going to financial and notification processing...");
                                                            if (Filtertransaction.IsBalanceUpdated == -1) //0 for balance not updated, 1 means balance updated
                                                            {
                                                                var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                                if (obj != null) obj.IsBalanceUpdated = 1;
                                                                //financial operation here
                                                                FinancialProcessing(customerVehicleInfo, customerAccountInfo, transaction);
                                                                LogMessage("Financial processing has been done.");

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
                                                            LogMessage("Tag class and NF class does not match. Transaction will be marked as violation.");
                                                            //mark the transaction as violation and leave it for manual review
                                                            var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                            if (obj != null) obj.IsViolation = 1;
                                                            VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.MarkAsViolation(transaction);
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

                                                    //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection associatedNodeFluxTrans = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetCorrespondingTransactionInNodeFlux(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber);
                                                    TranscationDataFilterList = GetAssociatedData(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber, "ANPR");
                                                    if (TranscationDataFilterList.Count > 0)
                                                    {
                                                        #region Update Tran Table if ANPR found
                                                        if (TranscationDataFilterList.Count == 1)
                                                        {
                                                            LogMessage("Transaction found to update...");
                                                            //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = associatedNodeFluxTrans[0];
                                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
                                                            TranscationData Filtertransaction = TranscationDataFilterList[0];
                                                            transaction.NodefluxEntryIdFront = Convert.ToInt32(Filtertransaction.AnprFId);
                                                            transaction.NodefluxEntryIdRear = Convert.ToInt32(Filtertransaction.AnprRId);
                                                            transaction.TMSId = Filtertransaction.TMSId;
                                                            transaction.PlazaId = Filtertransaction.PlazaId;
                                                            transaction.LaneId = Filtertransaction.LaneId;
                                                            transaction.TransactionId = Filtertransaction.TranscationId;
                                                            transaction.TransactionDateTime = Filtertransaction.TransactionDateTime;
                                                            if (nfp.CameraPosition == "1") //1 means front, 2 means rear
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
                                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPFront(transaction, nfpEntryId);
                                                                LogMessage("nf entry id front has been updated in transaction.");
                                                                var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                                if (obj != null) obj.AnprFId = nfpEntryId;
                                                            }
                                                            else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
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
                                                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPRear(transaction, nfpEntryId);
                                                                var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                                if (obj != null) obj.AnprRId = nfpEntryId;
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
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        #region No Associatied Found
                                                        if (nfp.CameraPosition == "1") //1 means front, 2 means rear
                                                        {
                                                            Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPFront(nfp, nfpEntryId, 1);
                                                            transcationDataList.Add(new TranscationData { TranscationId = TranscationId, LaneId = nfp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, AnprFId = nfpEntryId, CameraPosition = 1, TransactionDateTime = Convert.ToDateTime(nfp.TimeStamp), CurrentDateTime = DateTime.Now });
                                                            LogMessage("Transaction inserted by nf entry id front.");
                                                        }
                                                        else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
                                                        {
                                                            Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPRear(nfp, nfpEntryId, 1);
                                                            transcationDataList.Add(new TranscationData { TranscationId = TranscationId, LaneId = nfp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, AnprFId = nfpEntryId, CameraPosition = 2, TransactionDateTime = Convert.ToDateTime(nfp.TimeStamp), CurrentDateTime = DateTime.Now });
                                                            LogMessage("Transaction inserted by nf entry id rear.");
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
                                                LogMessage("VRN not exists. Checking associated nodeflux transaction...");

                                                //VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCollection associatedNodeFluxTrans = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetCorrespondingTransactionInNodeFlux(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber);
                                                TranscationDataFilterList = GetAssociatedData(nfp.TMSId, nfp.GantryId, nfpDateTime, nfp.PlateNumber, "ANPR");
                                                if (TranscationDataFilterList.Count > 0)
                                                {
                                                    #region Associate ANPR Found
                                                    if (TranscationDataFilterList.Count == 1)
                                                    {
                                                        #region ANPR Associated Data found in ANPR
                                                        TranscationData Filtertransaction = TranscationDataFilterList[0];
                                                        LogMessage("Transaction found to update...");
                                                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction = new Libraries.CommonLibrary.CBE.TransactionCBE();
                                                        transaction.CrosstalkEntryId = Convert.ToInt32(Filtertransaction.IKEId);
                                                        transaction.NodefluxEntryIdFront = Convert.ToInt32(Filtertransaction.AnprFId);
                                                        transaction.NodefluxEntryIdRear = Convert.ToInt32(Filtertransaction.AnprRId);
                                                        transaction.TMSId = Filtertransaction.TMSId;
                                                        transaction.PlazaId = Filtertransaction.PlazaId;
                                                        transaction.LaneId = Filtertransaction.LaneId;
                                                        transaction.TransactionId = Filtertransaction.TranscationId;
                                                        transaction.TransactionDateTime = Filtertransaction.TransactionDateTime;
                                                        if (nfp.CameraPosition == "1") //1 means front, 2 means rear
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
                                                            VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPFront(transaction, nfpEntryId);
                                                            var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                            if (obj != null) obj.AnprFId = nfpEntryId;
                                                            LogMessage("nf entry id front has been updated in transaction.");
                                                        }
                                                        else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
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
                                                            VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.UpdateByNFPRear(transaction, nfpEntryId);
                                                            var obj = transcationDataList.FirstOrDefault(x => x.TranscationId == Filtertransaction.TranscationId);
                                                            if (obj != null) obj.AnprRId = nfpEntryId;

                                                            LogMessage("nf entry id rear has been updated in transaction.");
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

                                                        Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPFront(nfp, nfpEntryId, 2);
                                                        LogMessage("Transaction inserted by nf entry id front.");
                                                        transcationDataList.Add(new TranscationData { TranscationId = TranscationId, LaneId = nfp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, AnprFId = nfpEntryId, CameraPosition = 1, TransactionDateTime = Convert.ToDateTime(nfp.TimeStamp), CurrentDateTime = DateTime.Now });
                                                    }
                                                    else if (nfp.CameraPosition == "2") //1 means front, 2 means rear
                                                    {

                                                        Int64 TranscationId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.InsertByNFPRear(nfp, nfpEntryId, 2);
                                                        transcationDataList.Add(new TranscationData { TranscationId = TranscationId, LaneId = nfp.LaneId, IsBalanceUpdated = -1, IsViolation = -1, TMSId = 1, VRN = nfp.PlateNumber, PlazaId = nfp.GantryId, AnprRId = nfpEntryId, CameraPosition = 2, TransactionDateTime = Convert.ToDateTime(nfp.TimeStamp), CurrentDateTime = DateTime.Now });
                                                        LogMessage("Transaction inserted by nf entry id rear.");
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
            decimal tollToDeduct = -1;
            try
            {
                LogMessage("Finding out LaneType and toll rate to deduct...");
                int laneTypeId = GetLaneTypeByLaneId(transaction.LaneId);
                LogMessage("LaneType is: " + laneTypeId);
                tollToDeduct = GetTollRate(currentTMSId, laneTypeId, transaction.TransactionDateTime, customerVehicleInfo.VehicleClassId);
                LogMessage("Toll to deduct is (for motorcycle it may be 0.00): " + tollToDeduct);
                if (tollToDeduct == -1)
                {
                    tollToDeduct = 0;
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception in finding out lane type and toll to deduct. " + ex.ToString());
                tollToDeduct = -1;
            }
            #endregion

            if (tollToDeduct > -1)
            {
                Decimal CurrentAccountBalance = customerVehicleInfo.AccountBalance;
                Decimal AfterDeduction = CurrentAccountBalance - tollToDeduct;
                if (AfterDeduction > 0)
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
                        accountHistory.IsEmailSent = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.EmailSentStatus.Unsent;//will be updated later on//accountHistory.ModifierId = 1;//will be updated later on
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
                        NotificationProcessing(customerVehicleInfo, customerAccountInfo, transaction, tollToDeduct, AfterDeduction);
                        LogMessage("Transaction is marked as balance updated.");
                    }
                    catch (Exception ex)
                    {
                        LogMessage("Exception in marking the transaction as balance updated. " + ex.ToString());
                    }
                    #endregion

                }
                else {
                    LogMessage("Due to insufficient balance.");
                    NotificationProcessing(customerVehicleInfo, customerAccountInfo, transaction, tollToDeduct, AfterDeduction);
                }

            }
            else
            {
                LogMessage("Toll to deduct is -1.00. There is some error somewhere.");
            }
        }

        private void NotificationProcessing(VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicleInfo, VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccountInfo, VaaaN.MLFF.Libraries.CommonLibrary.CBE.TransactionCBE transaction, Decimal tollToDeduct, Decimal AfterDeduction)
        {
            try
            {
                smsFileConfig = VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.SMSFileConfiguration.Deserialize();
                LogMessage("Trying to push SMS to MSMQ...");

                Message smsMessage = new Message();
                smsMessage.Formatter = new BinaryMessageFormatter();
                VaaaN.MLFF.Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail smsDetail = new Libraries.CommonLibrary.Classes.SmsNotification.SMSDetail();
                //smsDetail.SMSMessage = "Akun anda telah dipotong untuk bertransaksi nomor kendaraan " + customerVehicleInfo.VehRegNo + " anda di tempat " + GetPlazaNameById(transaction.PlazaId) + " pada " + transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.dateTimeFormat24H) + ".";
                CultureInfo culture = new CultureInfo("id-ID");
                string RechareDate = transaction.TransactionDateTime.AddDays(4).ToString("dd-MMM-yyyy") + " 23:59:59";
                if (AfterDeduction > 0)
                {
                    string AFTERDEDUCTION = smsFileConfig.AFTERDEDUCTION;
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[tolltodeduct]", Decimal.Parse(tollToDeduct.ToString()).ToString("C", culture).Replace("Rp", ""));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[vehregno]", customerVehicleInfo.VehRegNo);
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[transactiondatetime]", transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.DATETIME_FORMAT_WITHOUT_SECONDSForSMS));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[plazaid]", GetPlazaNameById(transaction.PlazaId));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("[balance]", Decimal.Parse(AfterDeduction.ToString()).ToString("C", culture).Replace("Rp", ""));
                    AFTERDEDUCTION = AFTERDEDUCTION.Replace("tid", transaction.TransactionId.ToString());
                    if (AFTERDEDUCTION.Length > 160)
                    {
                        AFTERDEDUCTION = AFTERDEDUCTION.Substring(0, 149);
                    }
                    smsDetail.SMSMessage = AFTERDEDUCTION;// "Pelanggan Yth, telah dilakukan pemotongan senilai Rp " + Decimal.Parse(tollToDeduct.ToString()).ToString("C", culture).Replace("Rp", "") + " terhadap saldo SJBE anda atas transaksi kendaraan " + customerVehicleInfo.VehRegNo + " pada " + transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.DATETIME_FORMAT_WITHOUT_SECONDSForSMS) + " di tempat " + GetPlazaNameById(transaction.PlazaId) + ". Sisa saldo SJBE anda saat ini Rp " + Decimal.Parse(AfterDeduction.ToString()).ToString("C", culture).Replace("Rp", "") + " Ref: [" + transaction.TransactionId.ToString() + "]";
                }
                else {
                    string NOTIFICATION = smsFileConfig.NOTIFICATION;
                    NOTIFICATION = NOTIFICATION.Replace("[tolltodeduct]", Decimal.Parse(tollToDeduct.ToString()).ToString("C", culture).Replace("Rp", ""));
                    NOTIFICATION = NOTIFICATION.Replace("[vehregno]", customerVehicleInfo.VehRegNo);
                    NOTIFICATION = NOTIFICATION.Replace("[transactiondatetime]", transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.DATETIME_FORMAT_WITHOUT_SECONDSForSMS));
                    NOTIFICATION = NOTIFICATION.Replace("[plazaid]", GetPlazaNameById(transaction.PlazaId));
                    NOTIFICATION = NOTIFICATION.Replace("[recharedate]", RechareDate);
                    NOTIFICATION = NOTIFICATION.Replace("[balance]", Decimal.Parse((AfterDeduction + tollToDeduct).ToString()).ToString("C", culture).Replace("Rp", ""));
                    NOTIFICATION = NOTIFICATION.Replace("tid", transaction.TransactionId.ToString());
                    if (NOTIFICATION.Length > 160)
                    {
                        NOTIFICATION = NOTIFICATION.Substring(0, 149);
                    }
                    smsDetail.SMSMessage = NOTIFICATION;//"Pelanggan Yth, Saldo SJBE anda saat ini tidak mencukupi untuk dilakukan pemotongan senilai Rp " + Decimal.Parse(tollToDeduct.ToString()).ToString("C", culture).Replace("Rp", "") + " atas transaksi kendaraan " + customerVehicleInfo.VehRegNo + " pada " + transaction.TransactionDateTime.ToString(VaaaN.MLFF.Libraries.CommonLibrary.Constants.DATETIME_FORMAT_WITHOUT_SECONDSForSMS) + " di Gantry - Medan Merdeka Barat 1. Silahkan melakukan pengisian ulang saldo SJBE anda sebelum " + RechareDate + ". Keterlambatan pengisian ulang saldo akan dikenakan denda sebesar Rp 1.000.000,00. Sisa saldo SJBE anda saat ini Rp " + Decimal.Parse((AfterDeduction + tollToDeduct).ToString()).ToString("C", culture).Replace("Rp", "") + " Ref: [" + transaction.TransactionId.ToString() + "]";
                }

                LogMessage(smsDetail.SMSMessage);
                smsDetail.AccountId = customerAccountInfo.AccountId;
                smsDetail.CustomerName = customerAccountInfo.FirstName + " " + customerAccountInfo.LastName;
                smsDetail.SenderMobileNumber = customerAccountInfo.MobileNo;

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
                        if (cvc.TagId.ToLower() == tagId.ToLower())
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

        private bool DoesExistInRecentCrossTalkPackets(Int32 plazaId, string tagId, DateTime tagReportingTime, string locationId)
        {
            bool result = false;
            try
            {
                LogMessage("Checking in recent crosstalk list...");

                //VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCollection ctPackets = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CrossTalkBLL.GetRecent(plazaId, tagId, tagReportingTime);

                //foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CrossTalkPacketCBE ctp in ctPackets)
                //{
                //    if (ctp.ObjectId.ToLower() == tagId.ToLower())
                //    {
                //        result = true;
                //        break;
                //    }
                //}

                //the above code has been commented and the following code is used for efficiency
                if (rfidRecentDataList.Any(e => (e.PlazaId == plazaId && e.ObjectId == tagId && e.PacketTimeStamp > tagReportingTime.AddMinutes(-1) && e.LocationId == locationId)))//locatinid is newly added
                {
                    result = true;
                }
                //ikeRecentDataList.Any(x => (x.PlazaId == plazaId && x.ObjectId = tagId));
                //ikeRecentDataList.Select(x => x.ObjectId= tagId && x.PlazaId=plazaId);               
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
                //VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection nfPackets = VaaaN.MLFF.Libraries.CommonLibrary.BLL.NodeFluxBLL.GetRecent(plazaId, vrn, nodeFluxReportingTime, cameraPosition);

                //foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nfp in nfPackets)
                //{
                //    if (nfp.PlateNumber.ToLower() == vrn.ToLower())
                //    {
                //        result = true;
                //        break;
                //    }
                //}

                //the above code has been commented and the following code is used for efficiency
                if (anprRecentDataList.Any(e => (e.PlazaId == plazaId && e.VRN == vrn && e.cameraPosition == cameraPosition && e.PacketTimeStamp > nodeFluxReportingTime.AddMinutes(-1))))
                {
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

        public List<TranscationData> GetAssociatedData(Int32 tmsId, Int32 plazaId, DateTime timestamp, string vrn, string pktType)
        {
            if (pktType.ToLower() == "rfid")
            {
                return transcationDataList.Where(trans => (trans.TMSId == tmsId && trans.PlazaId == plazaId && trans.VRN.ToLower() == vrn.ToLower() && trans.TransactionDateTime <= timestamp.AddSeconds(30) && trans.TransactionDateTime >= timestamp.AddSeconds(-30)) && trans.IKEId > 0).ToList();
            }
            else if (pktType.ToLower() == "anpr")
            {
                return transcationDataList.Where(trans => (trans.TMSId == tmsId && trans.PlazaId == plazaId && trans.VRN.ToLower() == vrn.ToLower() && trans.TransactionDateTime <= timestamp.AddSeconds(30) && trans.TransactionDateTime >= timestamp.AddSeconds(-30)) && (trans.AnprFId > 0 || trans.AnprRId > 0)).ToList();
            }
            else
            {
                return transcationDataList.Where(trans => (trans.TMSId == tmsId && trans.PlazaId == plazaId && trans.VRN.ToLower() == vrn.ToLower() && trans.TransactionDateTime <= timestamp.AddSeconds(30) && trans.TransactionDateTime >= timestamp.AddSeconds(-30))).ToList();
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

                        #region Updating latest customer accounts
                        //access records whose creation time is greater than the lastCollectionUpdateTime
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
                        //access records whose creation time is greater than the lastCollectionUpdateTime
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
    }

    public class TagData
    {
        public string TagId { get; set; }
        public DateTime CurrentDateTime { get; set; }
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
        public Int64 IKEId { get; set; }
        public Int64 AnprFId { get; set; }
        public Int64 AnprRId { get; set; }

        public DateTime CurrentDateTime { get; set; }
        public DateTime TransactionDateTime { get; set; }
    }
}

