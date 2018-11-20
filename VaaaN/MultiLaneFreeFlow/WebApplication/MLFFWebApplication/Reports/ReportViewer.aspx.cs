using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Drawing;

namespace VaaaN.MLFF.WebApplication.Reports
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        #region Variable 
        DataTable dt = new DataTable();

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string startDate = Convert.ToString(Session["startDate"]);
                string endDate = Convert.ToString(Session["endDate"]);
                Int32 rptnameid = Convert.ToInt32(Session["rptname"]);
                Int32 GantryId = 0;
                Int32 VehicleClassId = 0;
                Int32 TransactionCategoryId = 0;

                if (Session["GantryId"] != null)
                    GantryId = Convert.ToInt32(Session["GantryId"]);

                if (Session["VehicleClassId"] != null)
                    VehicleClassId = Convert.ToInt32(Session["VehicleClassId"]);

                if (Session["TransactionCategoryId"] != null)
                    VehicleClassId = Convert.ToInt32(Session["TransactionCategoryId"]);

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                Microsoft.Reporting.WebForms.LocalReport rvReportViewer1 = ReportViewer1.LocalReport;

                string rptname = "";
                if (rptnameid > 0)
                {


                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection subModules = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection();
                    subModules = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubModuleBLL.GetByUserId(Convert.ToInt16(Session["LoggedUserId"]), 7);
                    foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCBE submodule in subModules)
                    {
                        if (submodule.SubModuleId == rptnameid)
                        {
                            rptname = submodule.SubModuleName;
                            break;
                        }
                    }

                    if (rptname != "")
                    {

                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.ServerReport.Refresh();
                        DataSet lds = new DataSet();

                        switch (rptname)
                        {
                            #region Transaction Details Report
                            case "Transaction Details":
                                {

                                    string strQuery = " WHERE 1=1 ";
                                    if (startDate != null && endDate != null)
                                    {
                                        strQuery += " AND  TRANSACTION_DATETIME BETWEEN TO_DATE('" + startDate + "','DD/MM/YYYY HH24:MI:SS') AND  TO_DATE('" + endDate + "','DD/MM/YYYY HH24:MI:SS')";
                                    }
                                    else if (startDate != null)
                                    {
                                        strQuery += " AND  TRANSACTION_DATETIME >= TO_DATE('" + startDate + "','DD/MM/YYYY HH24:MI:SS')";
                                    }
                                    else if (endDate != null)
                                    {
                                        strQuery += " AND  TRANSACTION_DATETIME <= TO_DATE('" + endDate + "','DD/MM/YYYY HH24:MI:SS')";
                                    }
                                    if (GantryId > 0)
                                    {
                                        strQuery += " AND T.PLAZA_ID = " + GantryId;
                                    }
                                    if (VehicleClassId > 0)
                                    {
                                        strQuery += " AND (CV_CTP.VEHICLE_CLASS_ID = " + VehicleClassId + " OR  NFPF.VEHICLE_CLASS_ID = " + VehicleClassId + " OR  NFPR.VEHICLE_CLASS_ID = " + VehicleClassId + ")";
                                    }
                                    if (TransactionCategoryId == 0)
                                    {
                                        strQuery += " AND NVL(IS_BALANCE_UPDATED,0) <> 1";
                                    }
                                    else if (TransactionCategoryId == 1)
                                    {
                                        strQuery += " AND T.CT_ENTRY_ID IS NOT NULL ";
                                    }
                                    else if (TransactionCategoryId == 2)
                                    {
                                        strQuery += " AND T.NF_ENTRY_ID_FRONT IS NOT NULL ";
                                    }
                                    else if (TransactionCategoryId == 3)
                                    {
                                        strQuery += " AND T.NF_ENTRY_ID_REAR IS NOT NULL ";
                                    }

                                    dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.TransactionBLL.GetDataTableFilteredRecords(strQuery);
                                    rvReportViewer1.ReportPath = Server.MapPath("~/Reports/TranscationReport.rdlc");
                                    rvReportViewer1.DataSources.Add(new ReportDataSource("ReportDataSet", dt));
                                    rvReportViewer1.DataSources.Add(new ReportDataSource("CommonDataSet", CreateCommonDataTable(rptname)));
                                    break;
                                }
                                #endregion
                        }


                        ReportViewer1.ShowRefreshButton = false;

                        // Section 2
                        ReportViewer1.DataBind();
                        ReportViewer1.LocalReport.Refresh();
                        ReportViewer1.ServerReport.Refresh();
                    }



                }


            }
        }

        private ParameterDataSet GetData(string query)
        {
            string conString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            SqlCommand cmd = new SqlCommand(query);
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    sda.SelectCommand = cmd;
                    using (ParameterDataSet dsCustomers = new ParameterDataSet())
                    {
                        sda.Fill(dsCustomers, "DataTable1");
                        return dsCustomers;
                    }
                }
            }
        }

        private DataTable CreateParameterTable(string startDate, string endDate)
        {
            CommonDataSet.ParameterDataTable dt = new CommonDataSet.ParameterDataTable();
            DataRow dr = dt.NewRow();
            dr["start_date"] = startDate;
            dr["end_date"] = endDate;
            dt.Rows.Add(dr);

            return dt;

        }

        private DataTable CreateCommonDataTable(string reportName)
        {
            CommonDataSet.CommonDataTable dt = new CommonDataSet.CommonDataTable();
            DataRow dr = dt.NewRow();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE usr = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE();
            usr.UserId = Convert.ToInt32(Session["LoggedUserId"]);
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE user = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.GetUserById(usr);
            dr["footer_detail"] = reportName + " Generated at " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " By " + user.LoginName;
            dr["report_name"] = reportName;
            dr["company_name"] = "Bali Tower";
            dr["project_name"] = "MLFF";

            try
            {
                Image img = Image.FromFile(@"C:\MLFF\Config\logo.jpg");
                using (MemoryStream mStream = new MemoryStream())
                {
                    img.Save(mStream, img.RawFormat);
                    dr["logo_1"] = mStream.ToArray();
                }

                img = Image.FromFile(@"C:\MLFF\Config\smarterplogo.jpg");
                using (MemoryStream mStream = new MemoryStream())
                {
                    img.Save(mStream, img.RawFormat);
                    dr["logo_2"] = mStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                LogMessage("Report logo not found." + ex.Message);
            }

            dt.Rows.Add(dr);

            return dt;
        }

        private void LogMessage(string msg)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(msg, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.MLFFWeb);
        }
    }
}