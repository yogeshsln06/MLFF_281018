using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;
using VaaaN.MLFF.Libraries.CommonLibrary.MSMQ;
using VaaaN.MLFF.Libraries.CommonLibrary.Common;
using System.Data;
using System.Messaging;
using MLFFWebAPI.Models;
using VaaaN.MLFF.Libraries.CommonLibrary;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;
using System.Globalization;

namespace MLFFWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("VaaaN/IndonesiaMLFFApi")]
    public class VaaanAPIController : ApiController
    {
        #region Globel Variable 
        HttpResponseMessage response = null;
        static System.Messaging.MessageQueue inBoxQueueIKE;
        static System.Messaging.MessageQueue inBoxQueueANPR;
        static System.Messaging.MessageQueue inBoxQueue;
        string filepath = "";
        string rootpath = HttpContext.Current.Server.MapPath("~/events/" + DateTime.Now.ToString("dd-MMM-yyyy") + "/");
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        XmlReader xmlFile;
        string anprName = string.Empty;
        VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.SMSFileConfiguration smsFileConfig;

        #endregion

        #region API for RFID Data
        [Route("VaaaN/IndonesiaMLFFApi/SendCrossTalkEvent")]
        [HttpPost]
        public async Task<HttpResponseMessage> ReciveFilefromCrosstalk(HttpRequestMessage request)
        {
            var doc = new XmlDocument();
            #region Recive File from CrossTalk
            try
            {
                await Task.Delay(100);
                #region Read CrossTalk XML Data
                doc.Load(request.Content.ReadAsStreamAsync().Result);
                #endregion
                #region Create Physical Path to save CrossTalk XML Data as file
                if (!Directory.Exists(rootpath))
                {
                    Directory.CreateDirectory(rootpath);
                }
                filepath = rootpath + "CrossTalk/";
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                filepath = filepath + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".xml";
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                    doc.Save(filepath);
                }
                else
                {
                    var guid = Guid.NewGuid().ToString();
                    filepath = rootpath + "CrossTalk/" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + "-GUID-" + guid + ".xml";
                    File.Create(filepath).Dispose();
                    doc.Save(filepath);
                }
                response = Request.CreateResponse(HttpStatusCode.OK);
                //response = Request.CreateResponse(HttpStatusCode.OK, doc);
                #endregion
            }
            catch (IOException)
            {
                var guid = Guid.NewGuid().ToString();
                filepath = rootpath + "CrossTalk/" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + "-GUID-" + guid + ".xml";
                File.Create(filepath).Dispose();
                doc.Save(filepath);
            }
            catch (Exception ex)
            {

                ExceptionLogging.SendErrorToText(ex);
                Log("Error in API Recive File Crosstalk : " + ex);
            }
            #endregion



            #region Process File to MSMQ
            try
            {
                #region Read CrossTalk XML Data from File and load in Dataset
                xmlFile = new XmlNodeReader(doc);
                ds.ReadXml(xmlFile);
                #endregion
                #region Commented Code for Convert JSON from XML and read XML from Physical Location
                //string jsonString = JsonConvert.SerializeXmlNode(doc);
                //jsonString = jsonString.Replace(@"\", @"\\");
                //xmlFile = XmlReader.Create(filepath, new XmlReaderSettings());
                //ds.ReadXml(xmlFile);
                #endregion


                #region Loop for read data from DS
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    #region Required only for tag-observation events and need to ignore report
                    if (dr["type"].ToString().Contains("event:tag-observation"))
                    {
                        #region Pass data to CBE Liberrary
                        CrossTalkPacketCBE crosstalkPacketCBE = new CrossTalkPacketCBE();
                        crosstalkPacketCBE.EventType = dr["type"].ToString();
                        crosstalkPacketCBE.TimeStamp = Constants.UTCtoDateTime(dr["timestamp"].ToString());
                        crosstalkPacketCBE.UUID = dr["uuid"].ToString();
                        crosstalkPacketCBE.LocationId = dr["locationId"].ToString();
                        crosstalkPacketCBE.ParentUUID = dr["parentUUID"].ToString();
                        crosstalkPacketCBE.ObjectId = dr["objectId"].ToString();

                        #region Filter data according to Event Id and Event id is the common key of sub child
                        DataView dv = new DataView(ds.Tables[1]);
                        dv.RowFilter = String.Format("event_Id = '{0}'", dr["event_Id"].ToString());
                        dt = dv.ToTable();
                        foreach (DataRow Childdr in dt.Rows)
                        {
                            if (Childdr["id"].ToString() == "first-read")
                            {
                                crosstalkPacketCBE.FirstRead = Childdr["value"].ToString();
                            }
                            else if (Childdr["id"].ToString() == "last-read")
                            {
                                crosstalkPacketCBE.LastRead = Childdr["value"].ToString();
                            }
                            else if (Childdr["id"].ToString() == "observationUUID")
                            {
                                crosstalkPacketCBE.ObservationUUID = Childdr["value"].ToString();
                            }
                            else if (Childdr["id"].ToString() == "reads")
                            {
                                crosstalkPacketCBE.Reads = Childdr["value"].ToString();
                            }

                        }
                        #endregion

                        #endregion

                        #region Send data to MSMQ
                        try
                        {

                            CrossTalkPacket ctp = new CrossTalkPacket();
                            ctp.Source = "Source";
                            ctp.Destination = "Destination";
                            ctp.Payload = crosstalkPacketCBE;
                            ctp.TimeStamp = DateTime.Now;

                            Message m = new Message();
                            m.Formatter = new BinaryMessageFormatter();
                            m.Body = ctp;
                            m.Recoverable = true;
                            inBoxQueue = Queue.Create(Queue.inBoxQueueName);
                            inBoxQueue.Send(m);
                        }
                        catch (Exception ex)
                        {
                            ExceptionLogging.SendErrorToText(ex);
                            Log("Error in API ReciveFilefromCrosstalk Send data to MSMQ : " + ex);
                        }
                        #endregion
                    }
                    #endregion

                }
                #endregion
            }
            catch (Exception ex)
            {

                ExceptionLogging.SendErrorToText(ex);
                Log("Error in API ReciveFilefromCrosstalk : " + ex);
            }
            #endregion
            return response;
        }
        #endregion

        #region API for ANPR Data

        #region NodeFlux Data
        [Route("VaaaN/IndonesiaMLFFApi/SendNodefluxEvent")]
        [HttpPost]
        public async Task<HttpResponseMessage> ReciveDatafromNodeflux(NodeFluxPacketJSON objNodeFluxPacketJSON)
        {
            if (ANPRName().ToLower() == "nodeflux")
            {
                try
                {
                    #region Serialize the nodeflux JSON Data
                    string jsonString = JsonConvert.SerializeObject(objNodeFluxPacketJSON);
                    await Task.Delay(100);
                    #endregion

                    #region Create Physical Path to save nodeflux JSON Data as file
                    if (!Directory.Exists(rootpath))
                    {
                        Directory.CreateDirectory(rootpath);
                    }
                    filepath = rootpath + "NodeFlux/";
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    filepath = filepath + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".json";
                    if (!File.Exists(filepath))
                    {
                        File.Create(filepath).Dispose();
                        File.WriteAllText(filepath, jsonString);
                    }
                    else {
                        var guid = Guid.NewGuid().ToString();
                        filepath = rootpath + "NodeFlux/" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + "-GUID-" + guid + ".json";
                        File.Create(filepath).Dispose();
                        File.WriteAllText(filepath, jsonString);
                    }


                    response = Request.CreateResponse(HttpStatusCode.OK);
                    #endregion
                }
                catch (IOException)
                {
                    string jsonString = JsonConvert.SerializeObject(objNodeFluxPacketJSON);
                    var guid = Guid.NewGuid().ToString();
                    filepath = rootpath + "NodeFlux/" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + "-GUID-" + guid + ".json";
                    File.Create(filepath).Dispose();
                    File.WriteAllText(filepath, jsonString);
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendErrorToText(ex);
                    Log("Error in API to save Nodeflux File : " + ex);
                }

                try
                {
                    #region Pass data to CBE Liberrary
                    NodeFluxPacketCBE nodeFluxCBE = new NodeFluxPacketCBE();
                    nodeFluxCBE.EventType = objNodeFluxPacketJSON.Event_Type;
                    nodeFluxCBE.TimeStamp = Constants.ConversionDateTime(objNodeFluxPacketJSON.TimeStamp);
                    nodeFluxCBE.GantryId = 0;// objNodeFluxPacketJSON.Gantry_Id;
                    nodeFluxCBE.LaneId = objNodeFluxPacketJSON.Camera.Lane_Id;
                    nodeFluxCBE.Provider = 1;
                    if (string.IsNullOrEmpty(objNodeFluxPacketJSON.Camera.Camera_Position.ToString()))
                        nodeFluxCBE.CameraPosition = string.Empty;
                    else
                        nodeFluxCBE.CameraPosition = objNodeFluxPacketJSON.Camera.Camera_Position.ToString();

                    nodeFluxCBE.CameraId = objNodeFluxPacketJSON.Camera.Id;
                    nodeFluxCBE.CameraName = objNodeFluxPacketJSON.Camera.Name;
                    if (string.IsNullOrEmpty(objNodeFluxPacketJSON.Camera.Address))
                        nodeFluxCBE.CameraAddress = string.Empty;
                    else
                        nodeFluxCBE.CameraAddress = objNodeFluxPacketJSON.Camera.Address;
                    if (objNodeFluxPacketJSON.Camera.Coordinate.Length == 2)
                    {
                        nodeFluxCBE.CamaraCoordinate = objNodeFluxPacketJSON.Camera.Coordinate[0].ToString() + "," + objNodeFluxPacketJSON.Camera.Coordinate[1].ToString();
                    }
                    else if (objNodeFluxPacketJSON.Camera.Coordinate.Length == 1)
                    {
                        nodeFluxCBE.CamaraCoordinate = objNodeFluxPacketJSON.Camera.Coordinate[0].ToString();
                    }
                    else {
                        nodeFluxCBE.CamaraCoordinate = string.Empty;
                    }
                    nodeFluxCBE.PlateNumber = objNodeFluxPacketJSON.Data.Plate;
                    nodeFluxCBE.VehicleClassName = objNodeFluxPacketJSON.Data.Vehicle_Type;
                    nodeFluxCBE.VehicleSpeed = objNodeFluxPacketJSON.Data.Vehicle_Speed;

                    #region Convert 64 bit String into PNG Image
                    filepath = rootpath + @"Thumbnail\Plates\";
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    string imgfilepath = string.Empty;
                    string FileName = string.Empty;
                    FileName = "VRN_" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".png";
                    imgfilepath = filepath + FileName;
                    nodeFluxCBE.PlateThumbnail = SaveByteArrayAsImage(imgfilepath, objNodeFluxPacketJSON.Data.Thumbnail, FileName);
                    imgfilepath = string.Empty;
                    FileName = string.Empty;
                    filepath = rootpath + @"Thumbnail\Vehicle\";
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    FileName = "Vehicle_" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".png";
                    imgfilepath = filepath + FileName;
                    nodeFluxCBE.VehicleThumbnail = SaveByteArrayAsImage(imgfilepath, objNodeFluxPacketJSON.Data.Vehicle_Thumbnail, FileName);

                    #endregion

                    if (string.IsNullOrEmpty(objNodeFluxPacketJSON.Data.Video_URL))
                        nodeFluxCBE.VideoURL = string.Empty;
                    else if (objNodeFluxPacketJSON.Data.Video_URL.Contains("http"))
                        nodeFluxCBE.VideoURL = objNodeFluxPacketJSON.Data.Video_URL;
                    else
                        nodeFluxCBE.VideoURL = "http://" + objNodeFluxPacketJSON.Data.Video_URL;
                    #endregion

                    #region Send data to MSMQ
                    NodeFluxPacket nfp = new NodeFluxPacket();
                    nfp.Source = "Source";
                    nfp.Destination = "Destination";
                    nfp.Payload = nodeFluxCBE;
                    nfp.TimeStamp = DateTime.Now;

                    Message m = new Message();
                    m.Formatter = new BinaryMessageFormatter();
                    m.Body = nfp;
                    m.Recoverable = true;
                    inBoxQueue = Queue.Create(Queue.inBoxQueueName);
                    inBoxQueue.Send(m);
                    #endregion
                }
                catch (Exception ex)
                {

                    ExceptionLogging.SendErrorToText(ex);
                    Log("Error in API Nodeflux Send data to MSMQ : " + ex); ;
                }
            }
            return response;
        }
        #endregion

        #region Open ALPR
        [Route("VaaaN/IndonesiaMLFFApi/OpenAlpr")]
        [HttpPost]
        public async Task<HttpResponseMessage> ReciveDatafromOpenAlpr(OpenALPRPacketJSON objOpenALPRPacketJSON)
        {
            if (ANPRName().ToLower() == "openalpr")
            {
                try
                {

                    #region Serialize the OpenAlpr JSON Data
                    string jsonString = JsonConvert.SerializeObject(objOpenALPRPacketJSON);
                    await Task.Delay(100);
                    #endregion

                    #region Create Physical Path to save OpenAlpr JSON Data as file
                    if (!Directory.Exists(rootpath))
                    {
                        Directory.CreateDirectory(rootpath);
                    }
                    filepath = rootpath + "OpenAlpr/";
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    filepath = filepath + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".json";
                    if (!File.Exists(filepath))
                    {
                        File.Create(filepath).Dispose();
                        File.WriteAllText(filepath, jsonString);
                    }
                    else {
                        var guid = Guid.NewGuid().ToString();
                        filepath = rootpath + "OpenAlpr/" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + "-GUID-" + guid + ".json";
                        File.Create(filepath).Dispose();
                        File.WriteAllText(filepath, jsonString);
                    }


                    response = Request.CreateResponse(HttpStatusCode.OK);
                    #endregion
                }
                catch (IOException)
                {
                    string jsonString = JsonConvert.SerializeObject(objOpenALPRPacketJSON);
                    var guid = Guid.NewGuid().ToString();
                    filepath = rootpath + "OpenAlpr/" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + "-GUID-" + guid + ".json";
                    File.Create(filepath).Dispose();
                    File.WriteAllText(filepath, jsonString);
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendErrorToText(ex);
                    Log("Error in API to save OpenAlpr File : " + ex);
                }

                try
                {
                    #region Pass data to CBE Liberrary
                    NodeFluxPacketCBE nodeFluxCBE = new NodeFluxPacketCBE();
                    nodeFluxCBE.EventType = objOpenALPRPacketJSON.agent_type;
                    nodeFluxCBE.TimeStamp = Constants.UTCtoDateTime(objOpenALPRPacketJSON.epoch_end);
                    nodeFluxCBE.GantryId = 0;// objNodeFluxPacketJSON.Gantry_Id;
                    nodeFluxCBE.LaneId = 0;//objNodeFluxPacketJSON.Camera.Lane_Id;
                    nodeFluxCBE.Provider = 2;
                    nodeFluxCBE.CameraPosition = string.Empty;
                    nodeFluxCBE.CameraId = objOpenALPRPacketJSON.camera_id;
                    nodeFluxCBE.CameraName = objOpenALPRPacketJSON.web_server_config.camera_label;
                    nodeFluxCBE.CameraAddress = string.Empty;
                    nodeFluxCBE.CamaraCoordinate = string.Empty;
                    nodeFluxCBE.PlateNumber = objOpenALPRPacketJSON.best_plate_number;
                    nodeFluxCBE.VehicleClassName = VehicleClassForOpenALPR(objOpenALPRPacketJSON);
                    nodeFluxCBE.VehicleSpeed = 0;
                    nodeFluxCBE.PlateThumbnail = string.Empty;
                    nodeFluxCBE.VehicleThumbnail = string.Empty;
                    nodeFluxCBE.VideoURL = string.Empty;
                    //#region Convert 64 bit String into PNG Image
                    //filepath = rootpath + @"Thumbnail\Plates\";
                    //if (!Directory.Exists(filepath))
                    //{
                    //    Directory.CreateDirectory(filepath);
                    //}
                    ////dateTimeFormat24HForFileName

                    //string imgfilepath = string.Empty;
                    //string FileName = string.Empty;
                    //FileName = "VRN_" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".png";
                    //imgfilepath = filepath + FileName;
                    //nodeFluxCBE.PlateThumbnail = SaveByteArrayAsImage(imgfilepath, objNodeFluxPacketJSON.Data.Thumbnail, FileName);
                    //imgfilepath = string.Empty;
                    //FileName = string.Empty;
                    //filepath = rootpath + @"Thumbnail\Vehicle\";
                    //if (!Directory.Exists(filepath))
                    //{
                    //    Directory.CreateDirectory(filepath);
                    //}
                    //FileName = "Vehicle_" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".png";
                    //imgfilepath = filepath + FileName;
                    //nodeFluxCBE.VehicleThumbnail = SaveByteArrayAsImage(imgfilepath, objNodeFluxPacketJSON.Data.Vehicle_Thumbnail, FileName);

                    //#endregion

                    //if (string.IsNullOrEmpty(objNodeFluxPacketJSON.Data.Video_URL))
                    //    nodeFluxCBE.VideoURL = string.Empty;
                    //else
                    //    nodeFluxCBE.VideoURL = objNodeFluxPacketJSON.Data.Video_URL;
                    #endregion

                    #region Send data to MSMQ
                    NodeFluxPacket nfp = new NodeFluxPacket();
                    nfp.Source = "Source";
                    nfp.Destination = "Destination";
                    nfp.Payload = nodeFluxCBE;
                    nfp.TimeStamp = DateTime.Now;

                    Message m = new Message();
                    m.Formatter = new BinaryMessageFormatter();
                    m.Body = nfp;
                    m.Recoverable = true;
                    inBoxQueueANPR = Queue.Create(Queue.inBoxQueueName);
                    inBoxQueueANPR.Send(m);
                    #endregion
                }
                catch (Exception ex)
                {

                    ExceptionLogging.SendErrorToText(ex);
                    Log("Error in API OpenAlpr Send data to MSMQ : " + ex); ;
                }
            }
            return response;
        }
        #endregion

        //public HttpResponseMessage OpenAlpr()
        //{
        //    string text = Request.Content.ReadAsStringAsync().Result;
        //    #region Create Physical Path to save nodeflux JSON Data as file
        //    if (!Directory.Exists(rootpath))
        //    {
        //        Directory.CreateDirectory(rootpath);
        //    }
        //    filepath = rootpath + "OpenAlpr/";
        //    if (!Directory.Exists(filepath))
        //    {
        //        Directory.CreateDirectory(filepath);
        //    }
        //    filepath = filepath + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".json";
        //    if (!File.Exists(filepath))
        //    {
        //        File.Create(filepath).Dispose();
        //        File.WriteAllText(filepath, text);
        //    }
        //    else {
        //        var guid = Guid.NewGuid().ToString();
        //        filepath = rootpath + "OpenAlpr/" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + "-GUID-" + guid + ".json";
        //        File.Create(filepath).Dispose();
        //        File.WriteAllText(filepath, text);
        //    }


        //    response = Request.CreateResponse(HttpStatusCode.OK);
        //    #endregion

        //    return response;
        //}

        #endregion

        #region API for SEND SMS Data
        [Route("VaaaN/IndonesiaMLFFApi/SendSMS")]
        [HttpPost]
        public HttpResponseMessage SendSMS(HttpRequestMessage request)
        {
            try
            {
                CultureInfo culture = new CultureInfo("id-ID");
                LogInboundSMS("====================Inbound message (Start)=======");

                #region Variables

                string mobileNumber = "";
                string messageBody = "";
                string VRN = "";

                DateTime messageReceiveTime = DateTime.Now;
                DateTime transcationDateTime = DateTime.Now;
                Decimal rechargeAmount = 0;
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccount = null;
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicle = null;

                #endregion

                #region Read Inbound SMS XML Data
                var doc = new XmlDocument();
                try
                {
                    string data = WebUtility.UrlDecode(request.Content.ReadAsStringAsync().Result);
                    data = data.Replace("data=", "");

                    LogInboundSMS("XML response: " + data);
                    doc.LoadXml(data);
                }
                catch (Exception ex)
                {
                    LogInboundSMS("Failed to load xml document. : " + ex.Message);
                }
                #endregion

                #region Save SMS packet data in xml
                try
                {
                    LogInboundSMS("Saving xml file.");
                    if (!Directory.Exists(rootpath))
                    {
                        Directory.CreateDirectory(rootpath);
                    }
                    filepath = rootpath + "SMSInbound/";
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    filepath = filepath + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".xml";
                    if (!File.Exists(filepath))
                    {
                        File.Create(filepath).Dispose();
                    }

                    doc.Save(filepath);
                    response = Request.CreateResponse(HttpStatusCode.OK, doc);
                    LogInboundSMS("SMS xml file saved successfully.");
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendErrorToText(ex);
                    LogInboundSMS("Failed to save sms xml file. : " + ex);
                }

                #endregion

                #region Save Inbound message data in database
                try
                {
                    #region Parse XML
                    try
                    {
                        LogInboundSMS("Parsing XML data.");

                        xmlFile = new XmlNodeReader(doc);
                        ds.ReadXml(xmlFile);
                        dt = ds.Tables["mo_data"];
                        if (dt.Rows.Count > 0)
                        {
                            mobileNumber = dt.Rows[0]["msisdn"].ToString();
                            messageBody = dt.Rows[0]["sms"].ToString();
                        }

                        LogInboundSMS("XML parsed successfully.");
                    }
                    catch (Exception ex)
                    {
                        LogInboundSMS("Failed to parse xml. : " + ex.Message);
                    }
                    #endregion
                    string[] messageBodyFormat = messageBody.Split('_');
                    if (messageBodyFormat.Length == 2)
                    {
                        #region Save Incoming message in database
                        try
                        {
                            // Search account id by mobile number
                            LogInboundSMS("Searching account number by mobile number. Mobile nbr.:" + mobileNumber);
                            VRN = messageBodyFormat[1].ToString();
                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection customerAccounts = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetByMobileNumber(mobileNumber);
                            CustomerVehicleCBE objCustomerVehicle = new CustomerVehicleCBE();
                            objCustomerVehicle.VehRegNo = VRN;
                            customerVehicle = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetCustomerVehicleByVehRegNo(objCustomerVehicle);

                            #region SMS Communication History
                            if (customerAccounts.Count > 0 && customerVehicle.AccountId == customerAccounts[0].AccountId)
                            {
                                customerAccount = customerAccounts[0];
                                LogInboundSMS("Customer account found. Account id:" + customerAccount.AccountId);
                                if (customerAccounts.Count > 1)
                                {
                                    LogInboundSMS("Multiple customer account found for same mobile number. Considering only first one.");
                                }

                                VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE smsIncoming = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE();
                                smsIncoming.EntryId = 0;
                                smsIncoming.TmsId = 1;
                                smsIncoming.CustomerAccountId = customerAccount.AccountId;
                                smsIncoming.CustomerVehicleId = customerVehicle.EntryId;
                                smsIncoming.CustomerName = customerAccount.FirstName + " " + customerAccount.FirstName;
                                smsIncoming.MobileNumber = mobileNumber;
                                smsIncoming.MessageDirection = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDirection.Incoming;
                                smsIncoming.MessageBody = messageBody;
                                smsIncoming.SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent;
                                smsIncoming.ReceivedProcessStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSReceivedMessageProcessStatus.UnProcessed;
                                smsIncoming.MessageSendDateTime = DateTime.Now;
                                smsIncoming.MessageReceiveTime = messageReceiveTime;
                                smsIncoming.MessageDeliveryStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDeliveryStatus.UnDelivered; //DELIVERED=1,UNDELIVERED=2
                                smsIncoming.AttemptCount = 0;
                                smsIncoming.CreationDate = DateTime.Now;
                                smsIncoming.ModificationDate = DateTime.Now;
                                smsIncoming.ModifiedBy = 0;
                                LogInboundSMS("Inserting incoming message in database.");
                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.SMSCommunicationHistoryBLL.Insert(smsIncoming);
                                LogInboundSMS("Incoming message inserted successfully in database.");
                            }
                            else
                            {
                                LogInboundSMS("No customer account and vehicle found.");
                                #region Customer Account and Vehicle Found
                                VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE sms = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE();
                                sms.EntryId = 0;
                                sms.TmsId = 1;
                                sms.CustomerAccountId = 0;
                                sms.CustomerVehicleId = 0;
                                sms.CustomerName = "";
                                sms.MobileNumber = mobileNumber;
                                sms.MessageDirection = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDirection.Outgoing;
                                string NOCUSTOMERFOUND = smsFileConfig.NOCUSTOMERFOUND;

                                sms.MessageBody = NOCUSTOMERFOUND;
                                sms.SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent;
                                sms.ReceivedProcessStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSReceivedMessageProcessStatus.UnProcessed;
                                sms.MessageSendDateTime = DateTime.Now;
                                sms.MessageReceiveTime = DateTime.Now;
                                sms.MessageDeliveryStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDeliveryStatus.UnDelivered; //DELIVERED=1,UNDELIVERED=2
                                sms.AttemptCount = 0;
                                sms.CreationDate = DateTime.Now;
                                sms.ModificationDate = DateTime.Now;
                                sms.ModifiedBy = 0;
                                VaaaN.MLFF.Libraries.CommonLibrary.BLL.SMSCommunicationHistoryBLL.Insert(sms);
                                LogInboundSMS("Outbound message inserted successfully in database for no customer account and vehicle found.");
                                #endregion
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            LogInboundSMS("Failed to insert SMS communication history. : " + ex.Message);
                        }
                        #endregion

                        #region Create and save outgoing message
                        try
                        {
                            if (customerAccount != null)
                            {
                                smsFileConfig = VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.SMSFileConfiguration.Deserialize();
                                // Top up
                                if (messageBody.ToUpper().Contains("TOP-UP"))
                                {
                                    #region Process TOP UP message
                                    LogInboundSMS("===============TOP-UP=============");

                                    // Validate message- TODO
                                    bool isValidMessage = false;
                                    try
                                    {
                                        rechargeAmount = 100000;// For POC only
                                        isValidMessage = true;// TO DO

                                    }
                                    catch (Exception ex)
                                    {
                                        LogInboundSMS("Invalid message format. : " + ex.Message);
                                    }

                                    // Identify account id by mobile number
                                    if (isValidMessage)
                                    {
                                        #region Update account balance in database
                                        try
                                        {
                                            // Update account balance
                                            customerVehicle.AccountBalance += rechargeAmount;
                                            customerAccount.ModificationDate = DateTime.Now;

                                            LogInboundSMS("Updating customer account.");
                                            customerVehicle.AccountBalance = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.UpdateVehiclebalance(customerVehicle, rechargeAmount);
                                            LogInboundSMS("Customer account updated successfully.");
                                        }
                                        catch (Exception ex)
                                        {
                                            LogInboundSMS("Failed to update customer account. : " + ex.Message);
                                        }
                                        #endregion

                                        #region Update account history (POS transaction)
                                        Int32 entryId = 0;
                                        transcationDateTime = DateTime.Now;
                                        try
                                        {
                                            LogInboundSMS("Updating account history table...");
                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.AccountHistoryCBE accountHistory = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.AccountHistoryCBE();
                                            accountHistory.TMSId = customerAccount.TmsId;
                                            //accountHistory.EntryId = 0;//this  is the auto incremented and primary key of table
                                            accountHistory.AccountId = customerAccount.AccountId;
                                            accountHistory.CustomerVehicleEntryId = customerVehicle.EntryId;
                                            accountHistory.TransactionTypeId = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransactionType.Recharge;
                                            accountHistory.TransactionId = 0;// Transaction id will be used if amount is debited by lane transaction
                                            accountHistory.Amount = rechargeAmount;
                                            accountHistory.IsSMSSent = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent; //will be updated later on
                                            accountHistory.IsEmailSent = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.EmailSentStatus.Unsent; ;//will be updated later on
                                            accountHistory.CreationDate = transcationDateTime;
                                            accountHistory.ModificationDate = transcationDateTime;
                                            accountHistory.TransferStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.TransferStatus.NotTransferred;
                                            entryId = VaaaN.MLFF.Libraries.CommonLibrary.BLL.AccountHistoryBLL.Insert(accountHistory);
                                            LogInboundSMS("Account history table updated successfully.");
                                        }
                                        catch (Exception ex)
                                        {
                                            LogInboundSMS("Exception in recording in the Account History table. : " + ex.ToString());
                                        }

                                        #endregion

                                        #region Save outgoing message in database
                                        // This message will be sent by SMS service
                                        try
                                        {
                                            VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE smsOutgoing = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE();
                                            smsOutgoing.EntryId = 0;
                                            smsOutgoing.TmsId = 1;
                                            smsOutgoing.CustomerAccountId = customerAccount.AccountId;
                                            smsOutgoing.CustomerVehicleId = customerVehicle.EntryId;
                                            smsOutgoing.CustomerName = customerAccount.FirstName + " " + customerAccount.LastName;
                                            smsOutgoing.MobileNumber = customerAccount.MobileNo;
                                            smsOutgoing.MessageDirection = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDirection.Outgoing;
                                            //smsOutgoing.MessageBody = "Thanks for Top-Up with amount " + rechargeAmount + ". Your saldo is " + customerAccount.AccountBalance + ".";// Update message content TO DO
                                            string Topup = smsFileConfig.TOPUP;
                                            Topup = Topup.Replace("[rechargeamount]", Decimal.Parse(rechargeAmount.ToString()).ToString("C", culture).Replace("Rp", ""));
                                            Topup = Topup.Replace("[vehregno]", VRN);
                                            Topup = Topup.Replace("[balance]", Decimal.Parse(customerVehicle.AccountBalance.ToString()).ToString("C", culture).Replace("Rp", ""));
                                            Topup = Topup.Replace("[transactiondatetime]", transcationDateTime.ToString());
                                            Topup = Topup.Replace("tid", entryId.ToString());
                                            if (Topup.Length > 160)
                                            {
                                                Topup = Topup.Substring(0, 149);
                                            }
                                            smsOutgoing.MessageBody = Topup;
                                            //smsOutgoing.MessageBody = "Pelanggan Yth, terima kasih telah melakukan pengisian ulang saldo SJBE senilai Rp " + Decimal.Parse(rechargeAmount.ToString()).ToString("C", culture).Replace("Rp", "") + ". Saldo SJBE anda saat ini Rp " + Decimal.Parse(customerAccount.AccountBalance.ToString()).ToString("C", culture).Replace("Rp", "") + ". Ref: [" + entryId + "]";// Update message content TO DO
                                            smsOutgoing.SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent;
                                            smsOutgoing.ReceivedProcessStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSReceivedMessageProcessStatus.UnProcessed;
                                            smsOutgoing.MessageSendDateTime = DateTime.Now;
                                            smsOutgoing.MessageReceiveTime = DateTime.Now;
                                            smsOutgoing.MessageDeliveryStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDeliveryStatus.UnDelivered; //DELIVERED=1,UNDELIVERED=2
                                            smsOutgoing.AttemptCount = 0;
                                            smsOutgoing.CreationDate = DateTime.Now;
                                            smsOutgoing.ModificationDate = DateTime.Now;
                                            smsOutgoing.ModifiedBy = 0;

                                            LogInboundSMS("Inserting outbound message.");
                                            VaaaN.MLFF.Libraries.CommonLibrary.BLL.SMSCommunicationHistoryBLL.Insert(smsOutgoing);
                                            LogInboundSMS("outbound message inserted successfully.");
                                        }
                                        catch (Exception ex)
                                        {
                                            LogInboundSMS("Failed to insert outbound message. : " + ex.Message);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        // Create imvalid message
                                    }

                                    #endregion
                                }

                                // Balance check
                                if (messageBody.ToUpper().Contains("SALDO"))
                                {
                                    #region Process SALDO message

                                    // Search account by mobile number
                                    LogInboundSMS("===============SALDO=============");

                                    #region Save balance notification in database
                                    try
                                    {
                                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE sms = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE();
                                        sms.EntryId = 0;
                                        sms.TmsId = 1;
                                        sms.CustomerAccountId = customerAccount.AccountId;
                                        sms.CustomerVehicleId = customerVehicle.EntryId;
                                        sms.CustomerName = customerAccount.FirstName + " " + customerAccount.LastName;
                                        sms.MobileNumber = customerAccount.MobileNo;
                                        sms.MessageDirection = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDirection.Outgoing;
                                        string SALDO = smsFileConfig.SALDO;
                                        SALDO = SALDO.Replace("[vehregno]", VRN);
                                        SALDO = SALDO.Replace("[balance]", Decimal.Parse(customerVehicle.AccountBalance.ToString()).ToString("C", culture).Replace("Rp", ""));
                                        if (SALDO.Length > 160)
                                        {
                                            SALDO = SALDO.Substring(0, 149);
                                        }
                                        sms.MessageBody = SALDO;
                                        //sms.MessageBody = "Pelanggan Yth, Saldo SJBE anda saat ini Rp " + Decimal.Parse(customerAccount.AccountBalance.ToString()).ToString("C", culture).Replace("Rp", "") + ".";
                                        sms.SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent;
                                        sms.ReceivedProcessStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSReceivedMessageProcessStatus.UnProcessed;
                                        sms.MessageSendDateTime = DateTime.Now;
                                        sms.MessageReceiveTime = DateTime.Now;
                                        sms.MessageDeliveryStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDeliveryStatus.UnDelivered; //DELIVERED=1,UNDELIVERED=2
                                        sms.AttemptCount = 0;
                                        sms.CreationDate = DateTime.Now;
                                        sms.ModificationDate = DateTime.Now;
                                        sms.ModifiedBy = 0;
                                        LogInboundSMS("Inserting outbound message in database.");
                                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.SMSCommunicationHistoryBLL.Insert(sms);
                                        LogInboundSMS("Outbound message inserted successfully in database.");
                                    }
                                    catch (Exception ex)
                                    {
                                        LogInboundSMS("Failed to insert SMS communication history. : " + ex.Message);
                                    }
                                    #endregion

                                    #endregion
                                }
                            }
                            else
                            {
                                LogInboundSMS("Customer account found null.");
                            }
                        }
                        catch (Exception ex)
                        {
                            LogInboundSMS("Failed to create outbound message for incoming message. : " + ex.Message);
                        }

                        #endregion
                    }
                    else
                    {
                        LogInboundSMS("Invalid Format of SMS.");
                        #region Customer Account and Vehicle Found
                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE sms = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SMSCommunicationHistoryCBE();
                        sms.EntryId = 0;
                        sms.TmsId = 1;
                        sms.CustomerAccountId = 0;
                        sms.CustomerVehicleId = 0;
                        sms.CustomerName = "";
                        sms.MobileNumber = mobileNumber;
                        sms.MessageDirection = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDirection.Outgoing;
                        string INVALID = smsFileConfig.INVALID;
                        sms.MessageBody = INVALID;
                        sms.SentStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSSentStatus.Unsent;
                        sms.ReceivedProcessStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSReceivedMessageProcessStatus.UnProcessed;
                        sms.MessageSendDateTime = DateTime.Now;
                        sms.MessageReceiveTime = DateTime.Now;
                        sms.MessageDeliveryStatus = (int)VaaaN.MLFF.Libraries.CommonLibrary.Constants.SMSDeliveryStatus.UnDelivered; //DELIVERED=1,UNDELIVERED=2
                        sms.AttemptCount = 0;
                        sms.CreationDate = DateTime.Now;
                        sms.ModificationDate = DateTime.Now;
                        sms.ModifiedBy = 0;
                        VaaaN.MLFF.Libraries.CommonLibrary.BLL.SMSCommunicationHistoryBLL.Insert(sms);
                        LogInboundSMS("Outbound message inserted successfully in database for invalid format of sms.");
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    LogInboundSMS("Failed to insert SMS record. : " + ex.Message);
                }
                #endregion

                LogInboundSMS("====================Inbound message (Stop)=======");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                LogInboundSMS("Error in API SendSMS : " + ex);
            }

            return response;
        }
        #endregion

        #region API for Response SMS Data
        [Route("VaaaN/IndonesiaMLFFApi/ResponseSMS")]
        [HttpPost]
        public HttpResponseMessage ResponseSMS(HttpRequestMessage request)
        {
            try
            {
                LogInboundSMS("===========Responce SMS (Start)=======");

                #region Variables
                string mobileNumber = "";
                string referenceid = "";
                string statuscode = "";
                string transactionid = "";
                DateTime messageReceiveTime = DateTime.Now;

                #endregion

                #region Read Inbound SMS XML Data
                var doc = new XmlDocument();
                try
                {
                    LogInboundSMS("XML Raw data: " + request.Content.ReadAsStringAsync().Result);

                    string data = WebUtility.UrlDecode(request.Content.ReadAsStringAsync().Result);
                    data = data.Replace("data=", "");

                    LogInboundSMS("XML response: " + data);
                    doc.LoadXml(data);
                }
                catch (Exception ex)
                {
                    LogInboundSMS("Failed to load xml document. : " + ex.Message);
                }
                #endregion



                #region Parse XML
                //try
                //{
                //    LogInboundSMS("Parsing XML data.");

                //    xmlFile = new XmlNodeReader(doc);
                //    ds.ReadXml(xmlFile);
                //    dt = ds.Tables["mo_data"];
                //    if (dt.Rows.Count > 0)
                //    {
                //        mobileNumber = dt.Rows[0]["msisdn"].ToString();
                //        referenceid = dt.Rows[0]["reference_id"].ToString();
                //        statuscode = dt.Rows[0]["status_code"].ToString();
                //        transactionid = dt.Rows[0]["transaction_id"].ToString();
                //    }

                //    LogInboundSMS("XML parsed successfully.");
                //}
                //catch (Exception ex)
                //{
                //    LogInboundSMS("Failed to parse xml. : " + ex.Message);
                //}
                #endregion

                #region Update delivery ststaus

                // TODO
                #endregion

                LogInboundSMS("=========Responce message (Stop)=========");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                LogInboundSMS("Error in API ResponseSMS : " + ex);
            }

            return response;
        }
        #endregion

        #region Save Log 
        public void Log(String ExceptionMsg)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(ExceptionMsg, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.WebAPI);
        }
        public void LogInboundSMS(String msg)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(msg, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.InboundSMS);
        }

        #endregion

        #region Save Image from the string
        private string SaveByteArrayAsImage(string fullOutputPath, string bytesString, string FileName)
        {
            try
            {

                if (!string.IsNullOrEmpty(bytesString))
                {
                    string base64String = bytesString.Replace("\n", "");
                    byte[] imageBytes = Convert.FromBase64String(base64String);
                    Image x = (Bitmap)((new ImageConverter()).ConvertFrom(imageBytes));
                    x.Save(fullOutputPath);
                    if (fullOutputPath.Contains("events"))
                    {
                        string[] lines = Regex.Split(fullOutputPath, "events");

                        if (lines.Length == 2)
                        {
                            //   var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/events" + lines[1].ToString());
                            FileName = "events" + lines[1].ToString();
                        }
                    }
                }
                else
                {
                    FileName = string.Empty;
                }
            }
            catch (Exception ex)
            {
                FileName = string.Empty;
                ExceptionLogging.SendErrorToText(ex);
                Log("Error in API SaveByteArrayAsImage : " + ex);
            }
            return FileName;
        }
        #endregion

        [Route("VaaaN/IndonesiaMLFFApi/JSONPacket")]
        [HttpPost]
        public HttpResponseMessage JSONPacket()
        {
            string text = Request.Content.ReadAsStringAsync().Result;
            #region Create Physical Path to save nodeflux JSON Data as file
            if (!Directory.Exists(rootpath))
            {
                Directory.CreateDirectory(rootpath);
            }
            filepath = rootpath + "JSONPacket/";
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            filepath = filepath + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + ".json";
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Dispose();
                File.WriteAllText(filepath, text);
            }
            else {
                var guid = Guid.NewGuid().ToString();
                filepath = rootpath + "JSONPacket/" + DateTime.Now.ToString(Constants.dateTimeFormat24HForFileName) + "-GUID-" + guid + ".json";
                File.Create(filepath).Dispose();
                File.WriteAllText(filepath, text);
            }


            response = Request.CreateResponse(HttpStatusCode.OK);
            #endregion

            return response;
        }

        #region Helper Method
        public string VehicleClassForOpenALPR(OpenALPRPacketJSON objOpenALPRPacketJSON)
        {
            string vehicleClass = string.Empty;
            VehicleClassMappingCBE vehicleClassCBE = new VehicleClassMappingCBE();
            VehicleClassMappingCollection vehicleClassMappingCollection = new VehicleClassMappingCollection();
            List<bodytype> SortedList = objOpenALPRPacketJSON.vehicle.body_type.OrderByDescending(o => o.confidence).ToList();
            if (SortedList.Count > 0)
            {
                vehicleClassCBE.ANPRVehicleClassName = SortedList[0].name;
                vehicleClassCBE = VehicleClassMappingBLL.GetVehicleClassByANPRName(vehicleClassCBE);
                if (!string.IsNullOrEmpty(vehicleClassCBE.MLFFVehicleClassName))
                    vehicleClass = vehicleClassCBE.MLFFVehicleClassName;
            }

            return vehicleClass;
        }

        public string ANPRName()
        {
            try
            {
                anprName = System.Configuration.ConfigurationManager.AppSettings["anprName"].ToString();
            }
            catch (Exception)
            {

                anprName = "nodeflux";
            }

            return anprName;
        }
        #endregion
    }
}
