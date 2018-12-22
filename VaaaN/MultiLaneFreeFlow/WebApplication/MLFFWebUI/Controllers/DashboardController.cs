using MLFFWebUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Web;
using System.Web.Mvc;
using VaaaN.MLFF.Libraries.CommonLibrary;
using VaaaN.MLFF.Libraries.CommonLibrary.CBE;

namespace MLFFWebUI.Controllers
{
    public class DashboardController : Controller
    {
        static MessageQueue eventQueue;
        public ActionResult Index()
        {
            if (Session["LoggedUserId"] == null)
            {
                return RedirectToAction("Logout", "Login");
            }
            ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "Dashboard","");
            return View();
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
                ViewBag.MainMenu = HelperClass.NewMenu(Convert.ToInt16(Session["LoggedUserId"]), "Dashboard", "LiveMonitoring");

                #region Gantry Class Dropdown
                List<SelectListItem> gantryList = new List<SelectListItem>();
                List<VaaaN.MLFF.Libraries.CommonLibrary.CBE.PlazaCBE> plaza = VaaaN.MLFF.Libraries.CommonLibrary.BLL.PlazaBLL.GetAllAsList();

                gantryList.Add(new SelectListItem() { Text = "--Select Gantry--", Value = "0" });
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
                eventQueue.Purge();
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
                                    liveEvent.LaneName = ctp.LaneName;
                                    liveEvent.VehicleClassName = ctp.VehicleClassName;
                                    liveEvent.VRN = ctp.VRN;
                                    liveEvent.TagId = ctp.TagId;
                                    liveEvent.Datepacket = ctp.Timestamp.ToString(Constants.dateTimeFormat24HsqlServer);
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
                                    liveEvent.LaneName = ntp.LaneName;
                                    liveEvent.VehicleClassName = ntp.VehicleClassName;
                                    liveEvent.VRN = ntp.VRN;
                                    liveEvent.CameraLocation = ntp.CameraLocation;
                                    liveEvent.NumberPlatePath = ntp.NumberPlatePath.Replace('\\', '^'); ;
                                    liveEvent.VehiclePath = ntp.VehiclePath.Replace('\\', '^');
                                    liveEvent.VideoURL = ntp.VideoURL.Replace('\\', '^'); ;
                                    liveEvent.Datepacket = ntp.Timestamp.ToString(Constants.dateTimeFormat24HsqlServer);
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