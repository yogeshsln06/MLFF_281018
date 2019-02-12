using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace VaaaN.MLFF.VehicleFlowSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string frontPacket = string.Empty;
        string crossTalkPacketFront = string.Empty;
        string rearPacket = string.Empty;
        string crossTalkPacketRear = string.Empty;

        List<TextBox> orderTextBoxes = new List<TextBox>();
        List<String> orderedList = new List<string>();

        VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection vehicles;
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection accounts;
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection vehicleClasses;

        //string avcWebAPIAddress = "http://192.168.1.111:5555/VaaaN/MLFFWebAPI/SendNodeFluxPaket";
        //string etcWebAPIAddress = "http://192.168.1.111:5555/VaaaN/MLFFWebAPI/SendCrossTalkPaket";

        Dictionary<int, string> customers = new Dictionary<int, string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                vehicles = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetAllAsCollection();
                vehicleClasses = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAllAsCollection();

                //Load customer vehicles
                comboBoxVehicle.Items.Add("--Select--");
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE cv in vehicles)
                {
                    comboBoxVehicle.Items.Add(cv.EntryId + ", " + cv.AccountId + ", " + cv.VehicleClassId + ", " + cv.VehRegNo + ", " + cv.TagId + ", " + cv.AccountBalance);
                }
                comboBoxVehicle.SelectedIndex = 0;

                //load customer accounts
                accounts = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsCollection();

                orderTextBoxes.Add(this.textBoxFrontOrder);
                orderTextBoxes.Add(this.textBoxCrossTalkOrderFront);
                orderTextBoxes.Add(this.textBoxRearOrder);
                orderTextBoxes.Add(this.textBoxCrossTalkOrderRear);

                //load transaction time
                //textBoxDateTime.Text = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff");

                //ordered list initialization (4 items)
                orderedList.Add("");
                orderedList.Add("");
                orderedList.Add("");
                orderedList.Add("");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load window." + ex.Message);
            }
        }

        int currentEntryId = -1;
        int currentAccountId = -1;
        int currentVehicleClassId = -1;
        string currentVehicleRegNum = String.Empty;
        string nffVehicleRegNum = string.Empty;
        string nfrVehicleRegNum = string.Empty;
        string currentTagId = String.Empty;
        string nffVehicleClass = String.Empty;
        string nfrVehicleClass = String.Empty;
        private void comboBoxVehicle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            if (cb.SelectedIndex != 0)
            {
                //MessageBox.Show("You have selected: " + cb.SelectedItem.ToString());

                string[] vals = cb.SelectedItem.ToString().Split(new char[] { ',' });
                currentEntryId = Convert.ToInt32(vals[0]);
                currentAccountId = Convert.ToInt32(vals[1]);
                currentVehicleClassId = Convert.ToInt32(vals[2]);
                currentVehicleRegNum = vals[3];
                currentTagId = vals[4];

                //display account details of the corresponding vehicle
                textBoxCustomer.Content = GetAccountById(currentAccountId);

                VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE temp = new Libraries.CommonLibrary.CBE.VehicleClassCBE();
                temp.Id = Convert.ToInt32(currentVehicleClassId);
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vehicle = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetVehicleClassId(temp);

                textBoxVehicle.Content = vehicle.Id + ", " + vehicle.Name;

                textBoxNFFText.Text = currentVehicleRegNum;
                textBoxNFRText.Text = currentVehicleRegNum;

                comboBoxFrontClass.Text = GetVehicleClassNameByVRN(currentVehicleRegNum).ToLower();
                comboBoxRearClass.Text = GetVehicleClassNameByVRN(currentVehicleRegNum).ToLower();
            }
        }

        private string GetAccountById(int accId)
        {
            string result = String.Empty;
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE ca in accounts)
            {
                if (ca.AccountId == accId)
                {
                    result = ca.AccountId + ", " + ca.FirstName + " " + ca.LastName + ", " + ca.AccountBalance;
                }
            }
            return result;
        }

        string myTimeSTamp = string.Empty;
        private void buttonSimulate_Click(object sender, RoutedEventArgs e)
        {
            //DateTime transactionTime = DateTime.ParseExact(textBoxDateTime.Text, "dd/MM/yyyy HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
            // DateTime transactionTime = DateTime.ParseExact(System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"), "dd/MM/yyyy HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
            DateTime transactionTime = DateTime.Now;
            //MessageBox.Show(transactionTime.ToString("dd/MM/yyyy HH:mm:ss.fff"));
            //myTimeSTamp = DateTimeToUnixTimestamp(transactionTime).ToString();

            frontPacket = string.Empty;
            crossTalkPacketFront = string.Empty;
            rearPacket = string.Empty;
            crossTalkPacketRear = string.Empty;

            nffVehicleRegNum = textBoxNFFText.Text;
            nfrVehicleRegNum = textBoxNFRText.Text;

            nffVehicleClass = comboBoxFrontClass.Text;
            nfrVehicleClass = comboBoxRearClass.Text;

            if (checkBoxFrontAttachement.IsChecked == true)
            {
                //frontPacket = CreateFrontPacket(transactionTime, currentVehicleRegNum);
                frontPacket = CreateFrontPacket(transactionTime, nffVehicleRegNum, nffVehicleClass);
            }
            if (checkBoxCrossTalkAttachmentFront.IsChecked == true)
            {
                crossTalkPacketFront = CreateCrossTalkPacketFront(currentTagId, transactionTime);
            }
            if (checkBoxRearAttachementFront.IsChecked == true)
            {
                //rearPacket = CreateRearPacket(transactionTime, currentVehicleRegNum);
                rearPacket = CreateRearPacket(transactionTime, nfrVehicleRegNum, nfrVehicleClass);
            }
            if (checkBoxCrossTalkAttachmentRear.IsChecked == true)
            {
                crossTalkPacketRear = CreateCrossTalkPacketRear(currentTagId, transactionTime);
            }

            SendPacketsInOrder(frontPacket, crossTalkPacketFront, rearPacket, crossTalkPacketRear);

            MessageBox.Show("Finished.");
        }

        private string CreateFrontPacket(DateTime transactionTime, string vrn, string vehicleClass)
        {
            string result = string.Empty;
            try
            {
                //camera id will determine whether it is front or rear. to create class violation, the vehicle_type field needs to be changed.
                result = "{ " +
                          "\"Event_Type\": \"vehicle-classification-plate-event\", " +
                          "\"TimeStamp\": \"" + transactionTime.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "\", " +
                          "\"Gantry_Id\": 0, " +
                          "\"Camera\": { " +
                                "\"Camera_Position\": 0, " +
                                "\"Id\": 21, " +
                                "\"Name\": \"Lane2_FF_Panasonic\", " +
                                "\"Address\": \"Simulator\", " +
                                "\"Coordinate\": [ \"-6.2328758\", \"106.6506586\" ], " +
                                "\"Lane_Id\": 0 " +
                                "}, " +
                          "\"Data\": { " +
                            "\"Plate\": \"" + vrn + "\", " +
                            "\"Vehicle_Type\": \"" + vehicleClass + "\", " +
                            "\"Vehicle_Speed\": 30, " +
                            "\"Thumbnail\": \"http://poc-erp-hikvision.balitower.co.id:6040/pic?=d4=i052z10d3s996-d91115m4ep=t7i9i*d1=*ipd9=*2s6i1d8=*9b43f2a86-744b186-4b921e-62i5c5*e428bd1\", " +
                            // "\"Thumbnail\": \"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\", " +
                            "\"Vehicle_Thumbnail\": \"http://poc-erp-hikvision.balitower.co.id:6040/pic?=d4=i052z10d3s996-d91115m4ep=t7i9i*d1=*ipd9=*2s6i1d8=*9b43f2a86-744b186-4b921e-62i5c5*e428bd1\", " +
                            "\"Video_URL\": null " +
                          "} " +
                        "}";
            }
            catch (Exception ex)
            {
                result = string.Empty;
            }
            return result;
        }

        private string GetVehicleClassNameByVRN(string vrn)
        {
            string result = string.Empty;

            try
            {
                int classId = -1;
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE cv in vehicles)
                {
                    if (cv.VehRegNo.Trim().ToLower() == vrn.Trim().ToLower())
                    {
                        classId = cv.VehicleClassId;
                        break;
                    }
                }
                if (classId != -1)
                {
                    foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicleClasses)
                    {
                        if (vc.Id == classId)
                        {
                            result = vc.Name;
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

        private string CreateCrossTalkPacketFront(string tagId, DateTime transactionTime)
        {
            string result = string.Empty;
            try
            {
                //timestamp part is added with three extra zeros for millisecond part locationid should be proper
                result =
                    "<events> " +
                      "<event objectId=\"" + tagId + "\" parentUUID=\"2BB5CAD0-1F66-710D-8682-547811838B92\" locationId=\"" + 7 + "\" uuid=\"5DB5ED55-4EB6-C47A-2E32-AD8E262A840D\" timestamp=\"" + DateTimeToUnixTimestamp(transactionTime) + "000" + "\" type=\"com:nofilis:crosstalk:event:tag-observation\"> " +
                        "<property value=\"1325565648758\" id=\"first-read\"/> " +
                        "<property value=\"1325565648758\" id=\"last-read\"/> " +
                        "<property value=\"BD63426A-D8DE-F008-DD76-87E94B5D8B36\" id=\"observationUUID\"/>" +
                        "<property value=\"1\" id=\"reads\"/> " +
                      "</event> " +
                    "</events>";
            }
            catch (Exception ex)
            {
                result = string.Empty;
            }
            return result;
        }

        private string CreateRearPacket(DateTime transactionTime, string vrn, string vehicleClass)
        {
            string result = string.Empty;
            try
            {
                //camera id will determine whether it is front or rear. to create class violation, the vehicle_type field needs to be changed.
                result = "{ " +
                          "\"Event_Type\": \"vehicle-classification-plate-event\", " +
                          "\"TimeStamp\": \"" + transactionTime.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "\", " +
                          "\"Gantry_Id\": 0, " +
                          "\"Camera\": { " +
                                "\"Camera_Position\": 0, " +
                                "\"Id\": 24, " +
                                "\"Name\": \"Lane2_FF_Panasonic\", " +
                                "\"Address\": \"Simulator\", " +
                                "\"Coordinate\": [ \"-6.2328758\", \"106.6506586\" ], " +
                                "\"Lane_Id\": 0 " +
                                "}, " +
                                "\"Data\": { " +
                                            "\"Plate\": \"" + vrn + "\", " +
                                            "\"Vehicle_Type\": \"" + vehicleClass + "\", " +
                                            "\"Vehicle_Speed\": 40, " +
                                            "\"Thumbnail\": \"http://poc-erp-hikvision.balitower.co.id:6040/pic?=d4=i052z10d3s996-d91115m4ep=t7i9i*d1=*ipd9=*2s6i1d8=*9b43f2a86-744b186-4b921e-62i5c5*e428bd1\", " +
                                            // "\"Thumbnail\": \"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\", " +
                                            "\"Vehicle_Thumbnail\": \"http://poc-erp-hikvision.balitower.co.id:6040/pic?=d4=i052z10d3s996-d91115m4ep=t7i9i*d1=*ipd9=*2s6i1d8=*9b43f2a86-744b186-4b921e-62i5c5*e428bd1\", " +
                                            "\"Video_URL\": null " +
                                          "} " +
                        "}";
            }
            catch (Exception ex)
            {
                result = string.Empty;
            }
            return result;
        }

        private string CreateCrossTalkPacketRear(string tagId, DateTime transactionTime)
        {
            string result = string.Empty;
            try
            {
                //timestamp part is added with three extra zeros for millisecond part   locationid should be proper
                result =
                    "<events> " +
                      "<event objectId=\"" + tagId + "\" parentUUID=\"2BB5CAD0-1F66-710D-8682-547811838B92\" locationId=\"" + 8 + "\" uuid=\"5DB5ED55-4EB6-C47A-2E32-AD8E262A840D\" timestamp=\"" + DateTimeToUnixTimestamp(transactionTime) + "000" + "\" type=\"com:nofilis:crosstalk:event:tag-observation\"> " +
                        "<property value=\"1325565648758\" id=\"first-read\"/> " +
                        "<property value=\"1325565648758\" id=\"last-read\"/> " +
                        "<property value=\"BD63426A-D8DE-F008-DD76-87E94B5D8B36\" id=\"observationUUID\"/>" +
                        "<property value=\"1\" id=\"reads\"/> " +
                      "</event> " +
                    "</events>";
            }
            catch (Exception ex)
            {
                result = string.Empty;
            }
            return result;
        }

        private Int32 DateTimeToUnixTimestampOriginal(DateTime dt)
        {
            Int32 result = 0;

            result = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            return result;
        }

        private int DateTimeToUnixTimestamp(DateTime dt)
        {
            int result = 0;

            DateTime baseTime = new DateTime(1970, 1, 1);

            TimeSpan ts = (TimeSpan)(dt.ToUniversalTime() - baseTime);

            result = (int)ts.TotalSeconds;

            return result;
        }

        private void SendPacketsInOrder(string frontPacket, string crossTalkPacketFront, string rearPacket, string crossTalkPacketRear)
        {
            foreach (TextBox tb in orderTextBoxes)
            {
                if (tb.Text == "1")
                {
                    if (tb.Name.ToLower() == "textboxfrontorder") orderedList[0] = frontPacket + "~front";
                    else if (tb.Name.ToLower() == "textboxcrosstalkorderfront") orderedList[0] = crossTalkPacketFront + "~crosstalkfront";
                    else if (tb.Name.ToLower() == "textboxrearorder") orderedList[0] = rearPacket + "~rear";
                    else if (tb.Name.ToLower() == "textboxcrosstalkorderrear") orderedList[0] = crossTalkPacketRear + "~crosstalkrear";
                    else MessageBox.Show("Invalid order textbox name.");
                }
                else if (tb.Text == "2")
                {
                    if (tb.Name.ToLower() == "textboxfrontorder") orderedList[1] = frontPacket + "~front";
                    else if (tb.Name.ToLower() == "textboxcrosstalkorderfront") orderedList[1] = crossTalkPacketFront + "~crosstalkfront";
                    else if (tb.Name.ToLower() == "textboxrearorder") orderedList[1] = rearPacket + "~rear";
                    else if (tb.Name.ToLower() == "textboxcrosstalkorderrear") orderedList[1] = crossTalkPacketRear + "~crosstalkrear";
                    else MessageBox.Show("Invalid order textbox name.");
                }
                else if (tb.Text == "3")
                {
                    if (tb.Name.ToLower() == "textboxfrontorder") orderedList[2] = frontPacket + "~front";
                    else if (tb.Name.ToLower() == "textboxcrosstalkorderfront") orderedList[2] = crossTalkPacketFront + "~crosstalkfront";
                    else if (tb.Name.ToLower() == "textboxrearorder") orderedList[2] = rearPacket + "~rear";
                    else if (tb.Name.ToLower() == "textboxcrosstalkorderrear") orderedList[2] = crossTalkPacketRear + "~crosstalkrear";
                    else MessageBox.Show("Invalid order textbox name.");
                }
                else if (tb.Text == "4")
                {
                    if (tb.Name.ToLower() == "textboxfrontorder") orderedList[3] = frontPacket + "~front";
                    else if (tb.Name.ToLower() == "textboxcrosstalkorderfront") orderedList[3] = crossTalkPacketFront + "~crosstalkfront";
                    else if (tb.Name.ToLower() == "textboxrearorder") orderedList[3] = rearPacket + "~rear";
                    else if (tb.Name.ToLower() == "textboxcrosstalkorderrear") orderedList[3] = crossTalkPacketRear + "~crosstalkrear";
                    else MessageBox.Show("Invalid order textbox name.");
                }
                else
                {
                    MessageBox.Show("Invalid order test.");
                }
            }

            foreach (string s in orderedList)
            {
                string[] vals = s.Split(new char[] { '~' });
                string packet = vals[0];
                if (!string.IsNullOrEmpty(packet))
                {
                    Send(s);
                    RandomWaitWithinOneMinute(); //in seconds
                                                 // MessageBox.Show("Success");
                }
            }

        }

        private void Send(string packetCommaType)
        {
            string[] vals = packetCommaType.Split(new char[] { '~' });
            string packet = vals[0];
            string type = vals[1];

            if (!string.IsNullOrEmpty(packet))
            {
                if ((type.ToLower() == "crosstalkfront") || (type.ToLower() == "crosstalkrear")) //front front and rear crosstalk integrated
                {
                    if (!string.IsNullOrEmpty(packet))
                    {
                        HttpClient client = new HttpClient();
                        string url = textBoxAPIAddress.Text.Trim() + "/VaaaN/IndonesiaMLFFApi/SendCrossTalkEvent"; // "http://192.168.1.63:5555/VaaaN/IndonesiaMLFFApi/SendCrossTalkEvent";
                        client.BaseAddress = new Uri(url);

                        client.PostAsync(
                            "",
                            new StringContent(
                                packet,
                                Encoding.UTF8,
                                "application/text"));

                        Console.WriteLine("Crosstalk packet has been sent successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Crosstalk packet is not attached.");
                    }
                }
                else if ((type.ToLower() == "front") || (type.ToLower() == "rear"))
                {
                    if (!string.IsNullOrEmpty(packet))
                    {
                        HttpClient client = new HttpClient();
                        // "http://192.168.1.63:5555/VaaaN/IndonesiaMLFFApi/SendHikvisionEvent"; <======== sample url
                        string url = textBoxAPIAddress.Text.Trim() + "/VaaaN/IndonesiaMLFFApi/SendHikvisionEvent";
                        client.BaseAddress = new Uri(url);

                        client.PostAsync(
                            "",
                            new StringContent(
                                packet,
                                Encoding.UTF8,
                                "application/json"));

                        Console.WriteLine("Nodeflux packet(" + type.ToLower() + ") has been sent successfully.");
                    }
                    else
                    {
                        if (type.ToLower() == "front")
                        {
                            Console.WriteLine("Crosstalk front packet is not attached.");
                        }
                        else if (type.ToLower() == "rear")
                        {
                            Console.WriteLine("Crosstalk rear packet is not attached.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid packet.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid type");
                }
            }
        }

        private void RandomWaitWithinOneMinute()
        {
            Random rand = new Random(System.DateTime.Now.Millisecond);
            int waitingTime = rand.Next(500, 1500);
            Thread.Sleep(waitingTime);
            Console.WriteLine("Waiting for " + waitingTime + " milliseconds.");

        }

        private void buttonGenerateNFHTML_Click(object sender, RoutedEventArgs e)
        {
            ////////VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCollection nfPackets = VaaaN.MLFF.Libraries.CommonLibrary.BLL.NodeFluxBLL.GetAll();

            ////////StringBuilder sb = new StringBuilder();
            ////////int index = 0;
            ////////foreach(VaaaN.MLFF.Libraries.CommonLibrary.CBE.NodeFluxPacketCBE nfp in nfPackets)
            ////////{
            ////////    sb.Append("<tr>");

            ////////    sb.Append("<td>");
            ////////    sb.Append(nfp.EntryId.ToString());
            ////////    sb.Append("</td>" + Environment.NewLine);

            ////////    sb.Append("<td>");
            ////////    sb.Append(nfp.PlateNumber);
            ////////    sb.Append("</td>" + Environment.NewLine);

            ////////    sb.Append("<td>");
            ////////    sb.Append("<img src=\"" + nfp.PlateThumbnail + "\"/>");
            ////////    sb.Append("</td>" + Environment.NewLine);

            ////////    sb.Append("</tr>" + Environment.NewLine);

            ////////    index = index + 1;

            ////////    if (index > 10) break;
            ////////}

            ////////Console.WriteLine(sb.ToString());

            //-------------------------------------------------------------------------------------------

            //RandomVehicleNumberGenerator(); //<=========================== important for creating 20,000 vehicles inside the database

            //-------------------------------------------------------------------------------------------

            ////check the performance of Dictionary
            //for(int i =0; i < 10000000; i++)
            //{
            //    customers.Add(i, "customer " + i);
            //}

            //string myValue = string.Empty;
            //if (customers.TryGetValue(9999999, out myValue))
            //{
            //    Console.Write(myValue);
            //}

        }

        private void RandomVehicleNumberGenerator()
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string numbers = "0123456789";
            string classes = "1234";

            int charIndex = 0;
            int numIndex = 0;
            int classIndex = 0;

            Dictionary<int, string> vrns = new Dictionary<int, string>();

            int x = 1;

            string strvrn = string.Empty;
            //string strVRNPrevious = string.Empty;

            Random randomAccount = new Random();

            for (int i = 0; i < 20000; i++)
            {
                StringBuilder vrn = new StringBuilder();


                Random rand1 = new Random(System.DateTime.Now.Millisecond * x);
                Random rand2 = new Random(System.DateTime.Now.Minute * x);
                Random rand3 = new Random(System.DateTime.Now.Hour * x);

                classIndex = rand3.Next(0, 4);
                string classString = classes[classIndex].ToString();
                //vrn.Append(classString);

                //vrn.Append("=");

                charIndex = rand1.Next(0, 25);
                vrn.Append(characters[charIndex].ToString());
                charIndex = rand1.Next(0, 25);
                vrn.Append(characters[charIndex].ToString());

                //vrn.Append("-");

                numIndex = rand2.Next(0, 9);
                vrn.Append(numbers[numIndex].ToString());
                numIndex = rand2.Next(0, 9);
                vrn.Append(numbers[numIndex].ToString());

                charIndex = rand1.Next(0, 25);
                vrn.Append(characters[charIndex].ToString());

                //vrn.Append("-");

                numIndex = rand2.Next(0, 9);
                vrn.Append(numbers[numIndex].ToString());
                numIndex = rand2.Next(0, 9);
                vrn.Append(numbers[numIndex].ToString());
                numIndex = rand2.Next(0, 9);
                vrn.Append(numbers[numIndex].ToString());
                numIndex = rand2.Next(0, 9);
                vrn.Append(numbers[numIndex].ToString());

                x = x + 1;//for random seed

                //strVRNPrevious = strvrn;
                strvrn = vrn.ToString();

                //if (i % 10 == 0)
                //{
                //    strvrn = strVRNPrevious;
                //}

                if (!vrns.ContainsValue(strvrn)) //checking accidental generation of duplicate vrn
                {
                    vrns.Add(i, strvrn);
                    //Console.Write(vrn.ToString() + " ");
                    //Console.Write(VRNToByte(classString, vrn.ToString()));
                    //Console.WriteLine();
                    Console.WriteLine(i.ToString().PadLeft(4, ' ') + " - " + strvrn);

                    //inserting in database---------------------------------------
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE vehicle = new Libraries.CommonLibrary.CBE.CustomerVehicleCBE();
                    vehicle.TMSId = 1;
                    //vehicle.EntryId = i;
                    vehicle.AccountId = randomAccount.Next(1, 10); //<========== check number of accounts in the cutomer table
                    vehicle.VehRegNo = strvrn;
                    vehicle.TagId = VRNToByte(classString, strvrn);
                    vehicle.VehicleClassId = Convert.ToInt32(classString);
                    vehicle.CreationDate = System.DateTime.Now;
                    //vehicle.ModificationDate = System.DateTime.Now;
                    //vehicle.ModifiedBy = 1;
                    vehicle.TransferStatus = 1;
                    vehicle.AccountBalance = 1000000;

                    VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.Insert(vehicle);
                    //------------------------------------------------------------
                }
                else
                {
                    Console.WriteLine("Duplicate generation of " + strvrn);
                }

            }
        }

        private string VRNToByte(string vehicleClass, string vehicleRegistrationNumber)
        {
            string[] result = new string[12];

            int index = 0;
            result[index] = "0" + vehicleClass;
            index = index + 1;

            string vrn = vehicleRegistrationNumber.ToUpper();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(vrn);

            string[] values = BitConverter.ToString(bytes).Split(new char[] { '-' });

            for (int i = 0; i < values.Length; i++)
            {
                result[index] = values[i];
                index = index + 1;
            }

            result[index] = "FC";
            index = index + 1;

            for (int j = index; j < 12; j++)
            {
                result[j] = "00";
            }

            string finalResult = string.Empty;

            for (int k = 0; k < 12; k++)
            {
                finalResult = finalResult + result[k];
            }

            return finalResult;
        }

        Boolean isStarted = false;
        MLFFSimulator mySimulator = null;
        private void buttonStartMLFF_Click(object sender, RoutedEventArgs e)
        {
            if (!isStarted)
            {
                mySimulator = new MLFFSimulator();
                mySimulator.GantryId = 1;
                mySimulator.APIIPAddress = textBoxAPIAddress.Text;

                mySimulator.Start();
                MessageBox.Show("MLFF simulator has been started.");
                isStarted = true;
            }
            else
            {
                MessageBox.Show("Already started.");
            }
        }

        private void buttonStopMLFF_Click(object sender, RoutedEventArgs e)
        {
            if (isStarted == true)
            {
                if (mySimulator != null)
                {
                    mySimulator.Stop();
                    MessageBox.Show("MLFF simulator has been stopped.");
                }
                isStarted = false;
            }
            else
            {
                MessageBox.Show("Already in stopped state.");
            }
        }
    }


    public class MLFFSimulator
    {
        bool isToSendPacket = true; //switch to link/ delink the API
        int gantryId = 0;
        string apiIPAddress = string.Empty;

        public Int32 GantryId
        {
            get
            {
                return this.gantryId;
            }
            set
            {
                this.gantryId = value;
            }
        }

        public String APIIPAddress
        {
            get
            {
                return this.apiIPAddress;
            }
            set
            {
                this.apiIPAddress = value;
            }
        }

        string normalOrder = "1234"; //1 = ANPR Front, 2 = RFID Front, 3 = ANPR Rear, 4 = RFID Rear

        Thread simulatorThread = null;
        volatile bool stopSimulatorThread = false;

        VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCollection lanes = null;
        Dictionary<int, VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE> laneDictionary = new Dictionary<int, Libraries.CommonLibrary.CBE.LaneCBE>();
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCollection hardwares = null;
        Dictionary<int, VaaaN.MLFF.Libraries.CommonLibrary.CBE.HardwareCBE> hardwareDictionary = new Dictionary<int, Libraries.CommonLibrary.CBE.HardwareCBE>();
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCollection customerAccounts = null;
        Dictionary<int, VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE> customerAccountsDictionary = new Dictionary<int, Libraries.CommonLibrary.CBE.CustomerAccountCBE>();

        VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCollection registeredVehicles = null;
        Dictionary<int, VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE> registeredVehiclesDictionary = new Dictionary<int, Libraries.CommonLibrary.CBE.CustomerVehicleCBE>();
        Dictionary<int, string> registeredVehiclesDictionaryString = new Dictionary<int, string>();

        Dictionary<int, UnregisteredVehicle> unRegisteredVehiclesDictionaryString = new Dictionary<int, UnregisteredVehicle>();

        VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection vehicleClasses = null;

        private void Initialize()
        {
            gantryId = 1;

            //initialize lanes for the given gantry id
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE tempLane = new Libraries.CommonLibrary.CBE.LaneCBE();
            tempLane.PlazaId = gantryId;
            lanes = VaaaN.MLFF.Libraries.CommonLibrary.BLL.LaneBLL.GetLaneByPlazaId(tempLane);
            //convert lanes to dictionary
            for (int i = 0; i < lanes.Count; i++)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane = lanes[i];
                laneDictionary.Add(i, lane);
            }

            //initialize vehicle class
            vehicleClasses = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAllAsCollection();

            //initialize customerAccounts
            customerAccounts = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerAccountBLL.GetAllAsCollection();
            //convert customerAccounts to dictionary
            for (int i = 0; i < customerAccounts.Count; i++)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerAccountCBE customerAccount = customerAccounts[i];
                customerAccountsDictionary.Add(i, customerAccount);
            }

            //initialize registered customerVehicles (75%)
            registeredVehicles = VaaaN.MLFF.Libraries.CommonLibrary.BLL.CustomerVehicleBLL.GetAllAsCollection();
            //convert customerAccounts to  dictionary
            for (int i = 0; i < registeredVehicles.Count; i++)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE customerVehicle = registeredVehicles[i];
                registeredVehiclesDictionary.Add(i, customerVehicle);
                registeredVehiclesDictionaryString.Add(i, customerVehicle.TagId);
            }

            //initialize unregistered vehicles (25%)
            unRegisteredVehiclesDictionaryString = CreateUnRegisteredVehicle(10000, registeredVehiclesDictionaryString);
        }

        private void SetTrafficDensity()
        {

        }

        public void Start()
        {
            Initialize();

            SetTrafficDensity();

            simulatorThread = new Thread(new ThreadStart(this.SimulatorThreadFunction));
            simulatorThread.IsBackground = true;
            simulatorThread.Start();
        }

        public void Stop()
        {
            stopSimulatorThread = true;

            Thread.Sleep(200);

            if (simulatorThread != null)
            {
                if (simulatorThread.IsAlive == true)
                {
                    simulatorThread.Abort();
                }
            }
        }

        string currentVehicleClassName = String.Empty;

        int randomSeed = 0;
        private void SimulatorThreadFunction()
        {
            while (!stopSimulatorThread)
            {
                randomSeed = randomSeed + 1;
                if (randomSeed > 999999)
                    randomSeed = 0;

                Random randomWait = new Random();

                Boolean doRFIDMiss = false;
                Random rfidMissProbabibility = new Random(randomSeed + 1);
                int rfidMissProbabibilityIndex;

                Boolean doCameraMiss = false;
                Random cameraMissProbability = new Random(randomSeed + 2);
                int cameraMissProbabilityIndex;

                Boolean doANPRMistake = false;
                Random anprMistakeProbability = new Random(randomSeed + 3);
                int anprMistakeProbabilityIndex;

                Boolean doClassificationMistake = false;
                Random classificationMistakeProbability = new Random(randomSeed + 4);
                int classificationMistakeProbabilityIndex;

                Boolean doLaneSwitch = false;
                Random laneswitchingProbability = new Random(randomSeed + 5);
                int laneswitchingProbabilityIndex;

                try
                {
                    DateTime dt = System.DateTime.Now;
                    //pick a vehicle randomly-------------------------------------
                    bool isRegistered = true; //by default

                    Random registrationProbability = new Random();
                    int registrationProbabilityIndex = registrationProbability.Next(0, 5); //20% probability

                    if (registrationProbabilityIndex == 0)
                    {
                        isRegistered = false;
                    }

                    //===================================================
                    anprMistakeProbabilityIndex = anprMistakeProbability.Next(0, 10); //10% probability

                    if (anprMistakeProbabilityIndex == 0)
                    {
                        doANPRMistake = true;
                    }
                    else
                    {
                        doANPRMistake = false;
                    }
                    //===================================================

                    //===================================================
                    classificationMistakeProbabilityIndex = classificationMistakeProbability.Next(0, 10); //10% probability

                    if (classificationMistakeProbabilityIndex == 0)
                    {
                        doClassificationMistake = true;
                    }
                    else
                    {
                        doClassificationMistake = false;
                    }
                    //===================================================

                    string currentTagId = String.Empty;
                    string currentVRN = string.Empty;
                    int currentVehicleClassid = 0;

                    if (isRegistered == true)
                    {
                        Random randomVehicleRegistered = new Random();
                        int randomVehicleRegisteredIndex = randomVehicleRegistered.Next(0, registeredVehicles.Count);

                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE vehicle;
                        registeredVehiclesDictionary.TryGetValue(randomVehicleRegisteredIndex, out vehicle);

                        currentTagId = vehicle.TagId;
                        currentVRN = vehicle.VehRegNo;
                        currentVehicleClassid = vehicle.VehicleClassId;

                        //Console.WriteLine(dt.ToString("dd/MM/yyyy HH:mm:ss.fff") + " - Passing vehicle..." + currentVRN + ", " + GetVehicleClassNameById(currentVehicleClassid));
                    }

                    if (isRegistered == false)
                    {
                        Random randomVehicleUnregistered = new Random();
                        int randomVehicleUnregisteredIndex = randomVehicleUnregistered.Next(0, unRegisteredVehiclesDictionaryString.Count);

                        UnregisteredVehicle unRegisteredVehicle;
                        unRegisteredVehiclesDictionaryString.TryGetValue(randomVehicleUnregisteredIndex, out unRegisteredVehicle);

                        //currentTagId = vehicle.TagId;
                        currentVRN = unRegisteredVehicle.VRN; // vehicle.VehRegNo;
                        currentVehicleClassid = unRegisteredVehicle.VehicleClass;

                        //Console.WriteLine(dt.ToString("dd/MM/yyyy HH:mm:ss.fff") + " - Passing vehicle.............." + currentVRN + ", " + currentVehicleClassid);
                    }

                    Console.WriteLine(dt.ToString("dd/MM/yyyy HH:mm:ss.fff") + " - Passing vehicle..." + currentVRN + ", " + GetVehicleClassNameById(currentVehicleClassid));


                    if (doANPRMistake)
                    {
                        Console.Write("Do ANPR mistake.<=====================================");
                        currentVRN = currentVRN + "X";
                        Console.WriteLine(currentVRN);
                    }

                    if (doClassificationMistake) //classification mistake is visible in registered vehicle only
                    {
                        Console.Write("Do classification mistake.<=====================================");
                        int tempClassid = 0;
                        if (currentVehicleClassid == 1)
                        {
                            tempClassid = 4;
                        }
                        else if (currentVehicleClassid == 2)
                        {
                            tempClassid = 3;
                        }
                        else if (currentVehicleClassid == 3)
                        {
                            tempClassid = 2;
                        }
                        else if (currentVehicleClassid == 4)
                        {
                            tempClassid = 1;
                        }
                        else
                        {
                            tempClassid = 1; //<========================
                        }
                        Console.WriteLine(GetVehicleClassNameById(currentVehicleClassid) + "=>" + GetVehicleClassNameById(tempClassid));
                        currentVehicleClassid = tempClassid;
                    }

                    currentVehicleClassName = GetVehicleClassNameById(currentVehicleClassid);
                    //-------------------------------------------------------------

                    //pick a lane randomly-----------------------------------------
                    //set the hardware ids as per picked up lane
                    int currentFrontCameraId = 0;
                    int currentRearCameraId = 0;
                    int currentFrontRFIDId = 0;
                    int currentRearRFIDId = 0;

                    Random randomLane = new Random();
                    int randomLaneIndex = randomLane.Next(0, lanes.Count);

                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.LaneCBE lane;
                    laneDictionary.TryGetValue(randomLaneIndex, out lane);

                    currentFrontCameraId = lane.CameraIdFront;
                    currentRearCameraId = lane.CameraIdRear;
                    currentFrontRFIDId = lane.AntennaIdFront;
                    currentRearRFIDId = lane.AntennaIdRear;
                    //-------------------------------------------------------------

                    Random randomWaitBetweenPackets = new Random();

                    //---------------------------------------------------------------------------

                    //Do you want to simulate variable packet sequence?

                    //Do you want to simulate a vehicle class violaion?

                    //Do you want to simulate lane switching?

                    //Do you want to simulate wrong number plate?

                    //Do you want to simulate a front reader fail?

                    //Do you want to simulate a rear reader fail?

                    //Do you want to simulate a front camera fail?

                    //Do you want to simulate a rear camera fail?

                    //Do you want to simulate repeated reporting by camera?

                    //Do you want to simulate repeated reporting by RFID?

                    //Do you want to simulate unregistered vehicles?

                    //----------------------------------------------------------------------------

                    string newOrder = Reshuffle(normalOrder.ToCharArray());

                    for (int i = 0; i < newOrder.Length; i++)
                    {
                        int currentIndex = Convert.ToInt32(newOrder[i].ToString());

                        if (currentIndex == 1) //ANPR front
                        {
                            Thread.Sleep(randomWaitBetweenPackets.Next(10, 50));
                            ANPRPacket frontANPRPacket = new ANPRPacket();
                            frontANPRPacket.Timestamp = System.DateTime.Now;
                            frontANPRPacket.HardwareId = currentFrontCameraId; //<=========================== any front camera id of the gantry
                            frontANPRPacket.VRN = currentVRN; //<=============================VRN of the selected vehicle


                            frontANPRPacket.VehicleClassName = currentVehicleClassName;


                            if (isToSendPacket)
                            {
                                SendANPRPacket(frontANPRPacket);
                            }
                        }
                        else if (currentIndex == 2) //RFID front
                        {
                            if (isRegistered)
                            {
                                Thread.Sleep(randomWaitBetweenPackets.Next(10, 50));
                                RFIDPacket frontRFIDPacket = new RFIDPacket();
                                frontRFIDPacket.Timestamp = System.DateTime.Now;
                                frontRFIDPacket.HardwareId = currentFrontRFIDId; //<=========================== any front RFID id of the gantry
                                frontRFIDPacket.TagId = currentTagId; //<========================== tag id of the selected vehicle

                                if (isToSendPacket)
                                {
                                    SendRFIDPacket(frontRFIDPacket);
                                }
                            }
                        }
                        else if (currentIndex == 3)//ANPR rear
                        {
                            Thread.Sleep(randomWaitBetweenPackets.Next(10, 50));
                            ANPRPacket rearANPRPacket = new ANPRPacket();
                            rearANPRPacket.Timestamp = System.DateTime.Now;
                            rearANPRPacket.HardwareId = currentRearCameraId; //<=========================== any rear camera id of the gantry
                            rearANPRPacket.VRN = currentVRN; //<=============================VRN of the selected vehicle

                            rearANPRPacket.VehicleClassName = currentVehicleClassName;

                            if (isToSendPacket)
                            {
                                SendANPRPacket(rearANPRPacket);
                            }
                        }
                        else if (currentIndex == 4) //RFID rear
                        {
                            if (isRegistered)
                            {
                                Thread.Sleep(randomWaitBetweenPackets.Next(10, 50));
                                RFIDPacket rearRFIDPacket = new RFIDPacket();
                                rearRFIDPacket.Timestamp = System.DateTime.Now;
                                rearRFIDPacket.HardwareId = currentRearRFIDId; //<=========================== any rear RFID id of the gantry
                                rearRFIDPacket.TagId = currentTagId; //<========================== tag id of the selected vehicle

                                if (isToSendPacket)
                                {
                                    SendRFIDPacket(rearRFIDPacket);
                                }
                            }
                        }
                        else
                        {
                            //it will never happen
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {

                    int msRandom = randomWait.Next(100, 500); //10 to 100 <===== this will determine traffic density
                    Thread.Sleep(msRandom);
                }
            }
        }

        private Dictionary<int, UnregisteredVehicle> CreateUnRegisteredVehicle(int howMany, Dictionary<int, string> registeredVehiclesDictionaryString)
        {
            Dictionary<int, string> vrns = new Dictionary<int, string>();
            Dictionary<int, UnregisteredVehicle> result = new Dictionary<int, UnregisteredVehicle>();

            try
            {
                string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string numbers = "0123456789";
                string classes = "1234";

                int charIndex = 0;
                int numIndex = 0;
                int classIndex = 0;

                int x = 1;

                string strvrn = string.Empty;
                //string strVRNPrevious = string.Empty;

                Random randomAccount = new Random();

                #region loop
                for (int i = 0; i < howMany; i++)
                {
                    StringBuilder vrn = new StringBuilder();

                    Random rand1 = new Random(System.DateTime.Now.Millisecond * x);
                    Random rand2 = new Random(System.DateTime.Now.Minute * x);
                    Random rand3 = new Random(System.DateTime.Now.Hour * x);

                    classIndex = rand3.Next(0, 4);
                    string classString = classes[classIndex].ToString();
                    //vrn.Append(classString);

                    //vrn.Append("=");

                    charIndex = rand1.Next(0, 25);
                    vrn.Append(characters[charIndex].ToString());
                    charIndex = rand1.Next(0, 25);
                    vrn.Append(characters[charIndex].ToString());

                    //vrn.Append("-");

                    numIndex = rand2.Next(0, 9);
                    vrn.Append(numbers[numIndex].ToString());
                    numIndex = rand2.Next(0, 9);
                    vrn.Append(numbers[numIndex].ToString());

                    charIndex = rand1.Next(0, 25);
                    vrn.Append(characters[charIndex].ToString());

                    //vrn.Append("-");

                    numIndex = rand2.Next(0, 9);
                    vrn.Append(numbers[numIndex].ToString());
                    numIndex = rand2.Next(0, 9);
                    vrn.Append(numbers[numIndex].ToString());
                    numIndex = rand2.Next(0, 9);
                    vrn.Append(numbers[numIndex].ToString());
                    numIndex = rand2.Next(0, 9);
                    vrn.Append(numbers[numIndex].ToString());

                    vrn.Append("-UV"); //<======================================= a symbol for unregistered vehicle

                    x = x + 1;//for random seed

                    strvrn = vrn.ToString();

                    if (!vrns.ContainsValue(strvrn) && !registeredVehiclesDictionaryString.ContainsValue(strvrn)) //checking accidental generation of duplicate vrn and not contained in registered vehicle
                    {
                        vrns.Add(i, strvrn);

                        //generating random vehicle class between 1 and 4
                        Random randomVehicleClass = new Random(x);
                        int randomVehicleClassIndex = randomVehicleClass.Next(1, 5);

                        result.Add(i, new UnregisteredVehicle(strvrn, randomVehicleClassIndex));
                    }
                    else
                    {
                        //Console.WriteLine("Duplicate generation of " + strvrn);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

            return result;
        }

        private string VRNToByte(string vehicleClass, string vehicleRegistrationNumber)
        {
            string[] result = new string[12];

            int index = 0;
            result[index] = "0" + vehicleClass;
            index = index + 1;

            string vrn = vehicleRegistrationNumber.ToUpper();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(vrn);

            string[] values = BitConverter.ToString(bytes).Split(new char[] { '-' });

            for (int i = 0; i < values.Length; i++)
            {
                result[index] = values[i];
                index = index + 1;
            }

            result[index] = "FC";
            index = index + 1;

            for (int j = index; j < 12; j++)
            {
                result[j] = "00";
            }

            string finalResult = string.Empty;

            for (int k = 0; k < 12; k++)
            {
                finalResult = finalResult + result[k];
            }

            return finalResult;
        }

        private string GetVehicleClassNameByVRN(string vrn)
        {
            string result = string.Empty;

            try
            {
                int classId = -1;
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.CustomerVehicleCBE cv in registeredVehicles)
                {
                    if (cv.VehRegNo.Trim().ToLower() == vrn.Trim().ToLower())
                    {
                        classId = cv.VehicleClassId;
                        break;
                    }
                }
                if (classId != -1)
                {
                    foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicleClasses)
                    {
                        if (vc.Id == classId)
                        {
                            result = vc.Name;
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

        private string GetVehicleClassNameById(int vehicleClassId)
        {
            string result = string.Empty;

            try
            {
                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicleClasses)
                {
                    if (vc.Id == vehicleClassId)
                    {
                        result = vc.Name;
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

        private void SendANPRPacket(ANPRPacket rdp)
        {
            string packet = rdp.ToString();

            HttpClient client = new HttpClient();
            string url = this.apiIPAddress + "/VaaaN/IndonesiaMLFFApi/SendHikvisionEvent";
            client.BaseAddress = new Uri(url);

            client.PostAsync("", new StringContent(packet, Encoding.UTF8, "application/json"));
        }

        private void SendRFIDPacket(RFIDPacket adp)
        {
            string packet = adp.ToString();

            HttpClient client = new HttpClient();
            string url = this.apiIPAddress + "/VaaaN/IndonesiaMLFFApi/SendCrossTalkEvent";
            client.BaseAddress = new Uri(url);

            client.PostAsync("", new StringContent(packet, Encoding.UTF8, "application/text"));
        }

        private string Reshuffle(char[] texts)
        {
            Random rand = new Random();
            // Knuth shuffle algorithm :: courtesy of Wikipedia :)
            for (int t = 0; t < texts.Length; t++)
            {
                char tmp = texts[t];
                int r = rand.Next(t, texts.Length);
                texts[t] = texts[r];
                texts[r] = tmp;
            }

            return new string(texts);
        }

        public class ANPRPacket
        {
            int hardwareId = 0;
            DateTime timestamp = DateTime.MinValue;
            string vrn = string.Empty;
            string vehicleClassName = string.Empty;
            int speed = 0;

            public int HardwareId
            {
                get
                {
                    return this.hardwareId;
                }
                set
                {
                    this.hardwareId = value;
                }
            }

            public DateTime Timestamp
            {
                get
                {
                    return this.timestamp;
                }
                set
                {
                    this.timestamp = value;
                }
            }

            public string VRN
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

            public string VehicleClassName
            {
                get
                {
                    return this.vehicleClassName;
                }
                set
                {
                    this.vehicleClassName = value;
                }
            }

            public int Speed
            {
                get
                {
                    return this.speed;
                }
                set
                {
                    this.speed = value;
                }
            }

            public override string ToString()
            {
                string result = string.Empty;
                try
                {
                    //GetVehicleClassNameByVRN(vrn)
                    //camera id will determine whether it is front or rear. to create class violation, the vehicle_type field needs to be changed.
                    result = "{ " +
                              "\"Event_Type\": \"vehicle-classification-plate-event\", " +
                              "\"TimeStamp\": \"" + this.timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "\", " +
                              "\"Gantry_Id\": -1, " +
                              "\"Camera\": { " +
                                    "\"Camera_Position\": -1, " +
                                    "\"Id\": " + this.hardwareId + ", " +
                                    "\"Name\": \"gantryId-laneId-brand\", " +
                                    "\"Address\": \"gantry address\", " +
                                    "\"Coordinate\": [ \"-6.2328758\", \"106.6506586\" ], " +
                                    "\"Lane_Id\": -1 " +
                                    "}, " +
                              "\"Data\": { " +
                                "\"Plate\": \"" + this.vrn + "\", " +
                                "\"Vehicle_Type\": \"" + this.vehicleClassName + "\", " +
                                "\"Vehicle_Speed\": " + this.speed + ", " +
                                "\"Thumbnail\": \"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\", " +
                                "\"Vehicle_Thumbnail\": \"/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAIBAQEBAQIBAQECAgICAgQDAgICAgUEBAMEBgUGBgYF\nBgYGBwkIBgcJBwYGCAsICQoKCgoKBggLDAsKDAkKCgr/2wBDAQICAgICAgUDAwUKBwYHCgoKCgoK\nCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgr/wAARCAJkAiEDASIA\nAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQA\nAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3\nODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWm\np6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEA\nAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSEx\nBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElK\nU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3\nuLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD8s5ZS\nx6fSojJzn9aeRvGcfrSCPuQKAE+Z8YNXbOMKOO68k1HHEgUEjPFTRsBynTHSgC2oDKM09UeMb19e\ntU1uwh2tgEe9WIrtSo+bg9qALrzyBcgEHvkdKqTliMkdasJPGxyI+PWopmypOOtAFPazNjFSIveh\nuG464qaOByMEdaACORgRgZx61qW2xoxvPb1rNZNmFxwO9LPqEdrBhZMADk0ASa5e22m2zXEkgUe9\neK/E3x2dSuHt7eXCjIyD6VrfE3x9dyB7W3c7Tx1ry6Ytct5ssm7ceeaAK0k5lbL1d02dNvlAjnI6\n1QulVJCsRBx3FMsrh45txPGfWgA1JCl2+T1NV6nvi0sxk9emagHPSgBU6/hT6jDbSPenlsqSKAGU\n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\n\", " +
                                "\"Video_URL\": null " +
                              "} " +
                            "}";
                }
                catch (Exception ex)
                {
                    result = string.Empty;
                }
                return result;
            }
        }

        public class RFIDPacket
        {
            int hardwareId = 0;
            DateTime timestamp = DateTime.MinValue;
            string tagId = string.Empty;

            public int HardwareId
            {
                get
                {
                    return this.hardwareId;
                }
                set
                {
                    this.hardwareId = value;
                }
            }

            public DateTime Timestamp
            {
                get
                {
                    return this.timestamp;
                }
                set
                {
                    this.timestamp = value;
                }
            }

            public string TagId
            {
                get
                {
                    return this.tagId;
                }
                set
                {
                    this.tagId = value;
                }
            }

            private int DateTimeToUnixTimestamp(DateTime dt)
            {
                int result = 0;

                DateTime baseTime = new DateTime(1970, 1, 1);

                TimeSpan ts = (TimeSpan)(dt.ToUniversalTime() - baseTime);

                result = (int)ts.TotalSeconds;

                return result;
            }

            public override string ToString()
            {
                string result = string.Empty;
                try
                {
                    //timestamp part is added with three extra zeros for millisecond part
                    result =
                        "<events> " +
                          "<event objectId=\"" + this.tagId + "\" parentUUID=\"2BB5CAD0-1F66-710D-8682-547811838B92\" locationId=\"" + this.hardwareId + "\" uuid=\"5DB5ED55-4EB6-C47A-2E32-AD8E262A840D\" timestamp=\"" + DateTimeToUnixTimestamp(this.timestamp) + "000" + "\" type=\"com:nofilis:crosstalk:event:tag-observation\"> " +
                            "<property value=\"1325565648758\" id=\"first-read\"/> " +
                            "<property value=\"1325565648758\" id=\"last-read\"/> " +
                            "<property value=\"BD63426A-D8DE-F008-DD76-87E94B5D8B36\" id=\"observationUUID\"/>" +
                            "<property value=\"1\" id=\"reads\"/> " +
                          "</event> " +
                        "</events>";
                }
                catch (Exception ex)
                {
                    result = string.Empty;
                }
                return result;
            }
        }

        public class UnregisteredVehicle
        {
            private string vrn = string.Empty;
            private int vehicleClass = 0;

            public UnregisteredVehicle(string vrn, int vehicleClass)
            {
                this.vrn = vrn;
                this.vehicleClass = vehicleClass;
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

            public Int32 VehicleClass
            {
                get
                {
                    return this.vehicleClass;
                }
                set
                {
                    this.vehicleClass = value;
                }
            }
        }
    }
}
