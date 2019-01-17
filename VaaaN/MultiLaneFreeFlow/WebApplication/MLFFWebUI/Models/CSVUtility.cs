using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using VaaaN.MLFF.Libraries.CommonLibrary;

namespace MLFFWebUI.Models
{
    public class CSVUtility
    {
        public static MemoryStream GetCSV(DataTable dt)
        {
            MemoryStream stream = new MemoryStream();
            if (dt.Rows.Count > 0)
            {
                StreamWriter sw = new StreamWriter(stream);
                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dt.Columns[i].ToString());
                    if (i < iColCount)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i].ToString().Replace("\r\n", " "));
                        }
                        if (i < iColCount)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }
            dt.Rows.Clear();
            return stream;
        }

        public static void CreateCsvOld(string fpath, DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                StreamWriter sw = new StreamWriter(fpath, false);
                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dt.Columns[i].ToString());
                    if (i < iColCount)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i].ToString().Replace("\r\n", " "));
                        }
                        if (i < iColCount)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }
            dt.Rows.Clear();
        }

        public static Int16 CreateCsv(string fpath, DataTable dt)
        {
            Int16 RetVal = 0;
            if (dt.Rows.Count > 0)
            {
                StreamWriter sw = new StreamWriter(fpath, false);
                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dt.Columns[i].ToString());
                    if (i < iColCount)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i].ToString().Replace("\r\n", " ").Replace(",", " "));
                        }
                        if (i < iColCount)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
                RetVal = 1;
            }
            dt.Rows.Clear();
            return RetVal;
        }

        public static Int16 CreateCsvWithTitle(string fpath, DataTable dt, string Title)
        {
            Int16 RetVal = 0;
            if (dt.Rows.Count > 0)
            {
                StreamWriter sw = new StreamWriter(fpath, false);
                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    if (i == 0)
                    {
                        sw.Write(Title);
                    }
                    if (i < iColCount)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                sw.Write(sw.NewLine);
                iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dt.Columns[i].ToString());
                    if (i < iColCount)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i].ToString().Replace("\r\n", " ").Replace(",", " "));
                        }
                        if (i < iColCount)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
                RetVal = 1;
            }
            dt.Rows.Clear();
            return RetVal;
        }

        public static Int16 CreateCsvWithTitleFilter(string fpath, DataTable dt, string Title, object obj)
        {
            Int16 RetVal = 0;
            if (dt.Rows.Count > 0)
            {
                StreamWriter sw = new StreamWriter(fpath, false);
                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    if (i == 0)
                    {
                        sw.Write(Title);
                    }
                    if (i < iColCount)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                if (Title == "Customer")
                {
                    CustomerVehicleModel objCustomerVehicleModel = new CustomerVehicleModel();
                    objCustomerVehicleModel = (CustomerVehicleModel)obj;
                    #region Customer Id Filter
                    if (objCustomerVehicleModel.AccountId > 0)
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Customer ID");
                            }
                            else if (i == 1)
                            {
                                sw.Write(objCustomerVehicleModel.AccountId);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                    #region Resident Id Filter
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.ResidentId))
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Resident ID");
                            }
                            else if (i == 1)
                            {
                                sw.Write(objCustomerVehicleModel.ResidentId);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                    #region Name Filter
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.FirstName))
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Name");
                            }
                            else if (i == 1)
                            {
                                sw.Write(objCustomerVehicleModel.FirstName);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                    #region Mobile Phone Filter
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.MobileNo))
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Mobile Phone");
                            }
                            else if (i == 1)
                            {
                                sw.Write(objCustomerVehicleModel.MobileNo);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                    #region Email
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.EmailId))
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Email");
                            }
                            else if (i == 1)
                            {
                                sw.Write(objCustomerVehicleModel.EmailId);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                    #region Registration Num (VRN)
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.VehRegNo))
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Registration Num (VRN)");
                            }
                            else if (i == 1)
                            {
                                sw.Write(objCustomerVehicleModel.VehRegNo);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                }
                else if (Title == "Vehicle")
                {
                    CustomerVehicleModel objCustomerVehicleModel = new CustomerVehicleModel();
                    objCustomerVehicleModel = (CustomerVehicleModel)obj;
                    #region Customer Id Filter
                    if (objCustomerVehicleModel.AccountId > 0)
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Customer ID");
                            }
                            else if (i == 1)
                            {
                                sw.Write(objCustomerVehicleModel.AccountId);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                    #region Resident Id Filter
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.ResidentId))
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Resident ID");
                            }
                            else if (i == 1)
                            {
                                sw.Write(objCustomerVehicleModel.ResidentId);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                    #region Name Filter
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.FirstName))
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Name");
                            }
                            else if (i == 1)
                            {
                                sw.Write(objCustomerVehicleModel.FirstName);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                    #region Mobile Phone Filter
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.MobileNo))
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Mobile Phone");
                            }
                            else if (i == 1)
                            {
                                sw.Write(objCustomerVehicleModel.MobileNo);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                    #region Email
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.EmailId))
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Email");
                            }
                            else if (i == 1)
                            {
                                sw.Write(objCustomerVehicleModel.EmailId);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                    #region Certificate Num (VRCN)
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.VehicleRCNumber))
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Certificate Num (VRCN)");
                            }
                            else if (i == 1)
                            {
                                sw.Write(objCustomerVehicleModel.VehicleRCNumber);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                    #region Registration Num (VRN)
                    if (!string.IsNullOrEmpty(objCustomerVehicleModel.VehRegNo))
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Registration Num (VRN)");
                            }
                            else if (i == 1)
                            {
                                sw.Write(objCustomerVehicleModel.VehRegNo);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                    #region Class
                    if (objCustomerVehicleModel.VehicleClassId > 0)
                    {
                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection vehicleClasses = VaaaN.MLFF.Libraries.CommonLibrary.BLL.VehicleClassBLL.GetAllAsCollection();
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Class");
                            }
                            else if (i == 1)
                            {
                                sw.Write(GetVehicleClassByName(objCustomerVehicleModel.VehicleClassId, vehicleClasses));
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                    #region Status
                    if (objCustomerVehicleModel.QueueStatus > 0)
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Status");
                            }
                            else if (i == 1)
                            {
                                sw.Write(Constants.CustomerQueueStatusName[(objCustomerVehicleModel.QueueStatus) - 1]);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                    #region Exception
                    if (objCustomerVehicleModel.ExceptionFlag > 0)
                    {
                        iColCount = dt.Columns.Count;
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (i == 0)
                            {
                                sw.Write("Exception");
                            }
                            else if (i == 1)
                            {
                                sw.Write(Constants.ExceptionFlagName[(objCustomerVehicleModel.ExceptionFlag) - 1]);
                            }
                            if (i < iColCount)
                            {
                                sw.Write(",");
                            }
                        }
                        sw.Write(sw.NewLine);
                    }
                    #endregion
                }
                sw.Write(sw.NewLine);
                sw.Write(sw.NewLine);
                sw.Write(sw.NewLine);
                iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dt.Columns[i].ToString());
                    if (i < iColCount)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i].ToString().Replace("\r\n", " ").Replace(",", " "));
                        }
                        if (i < iColCount)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
                RetVal = 1;
            }
            dt.Rows.Clear();
            return RetVal;
        }

        private static string GetVehicleClassByName(Int32 vehicleClassId, VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCollection vehicleClasses)
        {
            string VehicleClassName = string.Empty;
            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.VehicleClassCBE vc in vehicleClasses)
            {
                if (vc.Id == vehicleClassId)
                {
                    VehicleClassName = vc.Name;
                    break;
                }
            }
            return VehicleClassName;
        }

    }
}