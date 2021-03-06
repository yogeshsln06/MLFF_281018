﻿using MLFFWebUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using VaaaN.MLFF.Libraries.CommonLibrary;
using VaaaN.MLFF.Libraries.CommonLibrary.BLL;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;

namespace MLFFWebUI.Controllers
{
    public class DashboardController : Controller
    {
        static MessageQueue eventQueue;
        static MessageQueue webDashboardMessageQueue;
        static MessageQueue webChartDashboardMessageQueue;
        public ActionResult Index()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "Dashboard", "");
            return View();
        }

        [HttpGet]
        public JsonResult GetMSMQDashBoardCounter()
        {

            JsonResult result = new JsonResult();
            DashboardEventCBE.DashBoardDataCount objDashBoardDataCount = new DashboardEventCBE.DashBoardDataCount();
            try
            {
                webDashboardMessageQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.webDashboardMessageQueue);
                Message[] msgs = webDashboardMessageQueue.GetAllMessages();
                if (msgs.Length > 0)
                {
                    foreach (Message msg in msgs)
                    {
                        Message m = msg;
                        m.Formatter = new BinaryMessageFormatter();
                        if (m != null)
                        {
                            m.Formatter = new BinaryMessageFormatter();
                            if (m.Body != null)
                            {
                                #region Processing packets
                                if (m.Body is DashboardEventCBE.DashBoardDataCount)
                                {
                                    #region CrossTalk packet
                                    objDashBoardDataCount = (DashboardEventCBE.DashBoardDataCount)m.Body;
                                    #endregion
                                }

                                #endregion
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to read MSMQ liveEvets data. " + ex.Message);
            }
            result.Data = objDashBoardDataCount;
            return Json(result, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string DashBoardTransactionData(ViewTransactionCBE transaction)
        {
            string result = "";

            string strstarttime = Convert.ToDateTime(transaction.StartDate).ToString("dd/MM/yyyy HH:mm:ss");
            string strendtime = Convert.ToDateTime(transaction.EndDate).ToString("dd/MM/yyyy HH:mm:ss");
            string Det = JsonConvert.SerializeObject(TransactionBLL.TransDeatilsReport(strstarttime, strendtime), Formatting.Indented);
            result = Det.Replace("\r", "").Replace("\n", "");
            return result;
        }

        //[HttpPost]
        //public string StackChartData(ViewTransactionCBE transaction)
        //{
        //    string result = "";
        //    string strstarttime = Convert.ToDateTime(transaction.StartDate).ToString("dd/MM/yyyy");
        //    string strendtime = Convert.ToDateTime(transaction.EndDate).ToString("dd/MM/yyyy HH:mm:ss");
        //    //Thread.Sleep(500);
        //     HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => new HelperAsync().StartProcessing(cancellationToken, strstarttime));
        //    // string Det = JsonConvert.SerializeObject(TransactionBLL.StackChartData(strstarttime, strendtime), Formatting.Indented);
        //    //result = Det.Replace("\r", "").Replace("\n", "");
        //    return result;
        //}

        [HttpGet]
        public string GetMSMQChartData()
        {
            string str = string.Empty;
            try
            {
                webChartDashboardMessageQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.webChartDashboardMessageQueue);
                Message[] msgs = webChartDashboardMessageQueue.GetAllMessages();
                if (msgs.Length > 0)
                {
                    foreach (Message msg in msgs)
                    {
                        Message m = msg;
                        m.Formatter = new BinaryMessageFormatter();
                        if (m != null)
                        {
                            m.Formatter = new BinaryMessageFormatter();
                            if (m.Body != null)
                            {
                                str = m.Body.ToString();
                                str = str.Replace("\r", "").Replace("\n", "");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to read MSMQ ChartEvets data. " + ex.Message);
            }
            return str;
            //return Json(str, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }

        #region Live Events
        public ActionResult LiveEvent()
        {
            List<LaneCBE> laneDataList = new List<LaneCBE>();
            ViewData["apiPath"] = HelperClass.GetAPIUrl();
            try
            {
                if (Session["LoggedUserId"] == null)
                {
                    return RedirectToAction("Logout", "Login");
                }
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "Dashboard", "liveevent");

                #region Gantry Class Dropdown
                List<SelectListItem> gantryList = new List<SelectListItem>();
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE> plaza = VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.GetAllAsList();

               // gantryList.Add(new SelectListItem() { Text = "", Value = "0" });
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE cr in plaza)
                {
                    gantryList.Add(new SelectListItem() { Text = cr.PlazaName, Value = System.Convert.ToString(cr.PlazaId) });
                }

                ViewBag.Gantry = gantryList;

                #endregion



            }
            catch (Exception)
            {

                HelperClass.LogMessage("Failed To Load Plaza List");
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetMSMQLiveEvent()
        {

            JsonResult result = new JsonResult();
            LiveEventCollection liveEvents = new LiveEventCollection();
            try
            {
                eventQueue = VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.Create(VaaaN.MLFF.Libraries.CommonLibrary.MSMQ.Queue.eventQueue);
                Message[] msgs = eventQueue.GetAllMessages();
                //eventQueue.Purge();
                if (msgs.Length > 0)
                {
                    foreach (Message msg in msgs)
                    {
                        Message m = msg;
                        m.Formatter = new BinaryMessageFormatter();
                        if (m != null)
                        {
                            m.Formatter = new BinaryMessageFormatter();
                            if (m.Body != null)
                            {
                                #region Processing packets
                                if (m.Body is CrossTalkEvent)
                                {
                                    #region CrossTalk packet
                                    CrossTalkEvent ctp = (CrossTalkEvent)m.Body;
                                    //CrossTalkEvent ctp = crossTalkPacket.Payload;
                                    LiveEventCBE liveEvent = new LiveEventCBE();
                                    liveEvent.PacketName = "CrossTalk";
                                    liveEvent.PlazaId = ctp.PlazaId;
                                    liveEvent.PlazaName = ctp.PlazaName;
                                    liveEvent.LaneId = ctp.LaneId;
                                    liveEvent.LaneName = ctp.LaneName;
                                    liveEvent.VehicleClassName = ctp.VehicleClassName;
                                    liveEvent.VRN = ctp.VRN;
                                    liveEvent.TagId = ctp.TagId;
                                    liveEvent.Datepacket = ctp.Timestamp.ToString("dd-MMM-yyyy hh:mm:ss tt");
                                    liveEvent.DeviceLocation = ctp.ReaderPosition.ToString();
                                    liveEvents.Add(liveEvent);
                                    #endregion
                                }
                                else if (m.Body is NodeFluxEvent)
                                {
                                    #region NodeFlux packet
                                    NodeFluxEvent ntp = (NodeFluxEvent)m.Body;
                                    LiveEventCBE liveEvent = new LiveEventCBE();
                                    liveEvent.PacketName = "NodeFlux - " + ntp.CameraLocation;
                                    liveEvent.PlazaName = ntp.PlazaName;
                                    liveEvent.PlazaId = ntp.PlazaId;
                                    liveEvent.LaneId = ntp.LaneId;
                                    liveEvent.LaneName = ntp.LaneName;
                                    liveEvent.VehicleClassName = ntp.VehicleClassName;
                                    liveEvent.VRN = ntp.VRN;
                                    liveEvent.DeviceLocation = ntp.CameraLocation;
                                    liveEvent.NumberPlatePath = ntp.NumberPlatePath;
                                    liveEvent.VehiclePath = ntp.VehiclePath.Replace('\\', '^');
                                    liveEvent.VideoURL = ntp.VideoURL.Replace('\\', '^'); ;
                                    liveEvent.Datepacket = ntp.Timestamp.ToString("dd-MMM-yyyy hh:mm:ss tt");
                                    liveEvents.Add(liveEvent);
                                    #endregion
                                }
                                #endregion
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                HelperClass.LogMessage("Failed to read MSMQ liveEvets data. " + ex.Message);
            }
            result.Data = liveEvents;
            return Json(result, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}