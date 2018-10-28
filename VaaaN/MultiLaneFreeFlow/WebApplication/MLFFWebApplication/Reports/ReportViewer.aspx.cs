using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Web.UI.WebControls;

namespace VaaaN.MLFF.WebApplication.Reports
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string startDate = Convert.ToString(Session["startDate"]);
                string endDate = Convert.ToString(Session["endDate"]);
                string rptname = Convert.ToString(Session["rptname"]);

                DataTable dt = new DataTable();
                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                Microsoft.Reporting.WebForms.LocalReport rvReportViewer1 = ReportViewer1.LocalReport;

                if (rptname != "")
                {
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.ServerReport.Refresh();
                    DataSet lds = new DataSet();

                    switch (rptname)
                    {
                        #region Sample Report
                        case "Sample Report":
                            {
                                dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.dtGetUserAll();
                                //dt = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.GetUserReport(startDate, endDate);
                                rvReportViewer1.ReportPath = Server.MapPath("~/Reports/sample.rdlc");
                                rvReportViewer1.DataSources.Add(new ReportDataSource("DataSet1", dt));
                                rvReportViewer1.DataSources.Add(new ReportDataSource("DataSetCommon", CreateCommonDataTable(rptname)));
                                break;
                            }

                            //case "Transaction Detail Report":
                            //    {
                            //        dt = VaaaN.PMS.CommonLibrary.BLL.ReportBLL.GetTransactionDetailReport(startDate, endDate);
                            //        rvReportViewer1.ReportPath = Server.MapPath("~/Reports/TransactionDetail.rdlc");
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("TransactionDetailDataSet", dt));
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("DataSetCommon", CreateCommonDataTable(rptname)));
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("ParameterDataSet", CreateParameterTable(startDate, endDate)));
                            //        break;
                            //    }

                            //case "Daily Traffic Count Report":
                            //    {
                            //        dt = VaaaN.PMS.CommonLibrary.BLL.ReportBLL.GetDailyTrafficCountReport(startDate, endDate);
                            //        rvReportViewer1.ReportPath = Server.MapPath("~/Reports/DailyTrafficCount.rdlc");
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("DailyTrafficCount", dt));
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("DataSetCommon", CreateCommonDataTable(rptname)));
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("ParameterDataSet", CreateParameterTable(startDate, endDate)));
                            //        break;
                            //    }

                            //case "Monthly Traffic Count Report":
                            //    {
                            //        dt = VaaaN.PMS.CommonLibrary.BLL.ReportBLL.GetMonthlyTrafficCountReport(startDate, endDate);
                            //        rvReportViewer1.ReportPath = Server.MapPath("~/Reports/DailyTrafficCount.rdlc");
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("DailyTrafficCount", dt));
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("DataSetCommon", CreateCommonDataTable(rptname)));
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("ParameterDataSet", CreateParameterTable(startDate, endDate)));
                            //        break;
                            //    }

                            //case "Parking Fare Report":
                            //    {
                            //        dt = VaaaN.PMS.CommonLibrary.BLL.ReportBLL.GetParkingFareReport(startDate, endDate);
                            //        rvReportViewer1.ReportPath = Server.MapPath("~/Reports/ParkingFareReport.rdlc");
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("ParkingFareData", dt));
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("DataSetCommon", CreateCommonDataTable(rptname)));
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("ParameterDataSet", CreateParameterTable(startDate, endDate)));
                            //        break;
                            //    }

                            //case "Parking Status Report":
                            //    {
                            //        dt = VaaaN.PMS.CommonLibrary.BLL.ReportBLL.GetParkingStatusReport(startDate, endDate);
                            //        rvReportViewer1.ReportPath = Server.MapPath("~/Reports/ParkingStatusReport.rdlc");
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("ParkingStatusData", dt));
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("DataSetCommon", CreateCommonDataTable(rptname)));
                            //        rvReportViewer1.DataSources.Add(new ReportDataSource("ParameterDataSet", CreateParameterTable(startDate, endDate)));
                            //        break;
                            //    }
                    }
                    #endregion
                    ReportViewer1.ShowRefreshButton = false;

                    // Section 2
                    ReportViewer1.DataBind();
                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.ServerReport.Refresh();

                }
            }
        }
        private DataTable CreateParameterTable(string startDate, string endDate)
        {
            VaaaN.MLFF.WebApplication.Reports.ParameterDataSet.ParameterDataDataTable dt = new VaaaN.MLFF.WebApplication.Reports.ParameterDataSet.ParameterDataDataTable();
            DataRow dr = dt.NewRow();

            dr["start_date"] = startDate;
            dr["end_date"] = endDate;
            dt.Rows.Add(dr);

            return dt;

        }
        private DataTable CreateCommonDataTable(string reportName)
        {
            VaaaN.MLFF.WebApplication.Reports.DataSetCommon.DataTableCommonDataTable dt = new VaaaN.MLFF.WebApplication.Reports.DataSetCommon.DataTableCommonDataTable();
            DataRow dr = dt.NewRow();
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE usr = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE();
            usr.UserId = Convert.ToInt32(Session["LoggedUserId"]);
            VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE user = VaaaN.MLFF.Libraries.CommonLibrary.BLL.UserBLL.GetUserById(usr);
            dr["footer_detail"] = reportName + " Generated at " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " By " + user.LoginName;
            dr["report_name"] = reportName;
            //VaaaN.PMS.CommonLibrary.CBE.OrganizationCBE organization = VaaaN.PMS.CommonLibrary.BLL.OrganizationBLL.GetOrganization();
            dr["company_name"] = "VaaaN Infra Pvt Ltd";
            dr["project_name"] = "PMS";

            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(@"C:\MLFF\Config\logo_1.jpg");
                using (MemoryStream mStream = new MemoryStream())
                {
                    img.Save(mStream, img.RawFormat);
                    dr["logo_1"] = mStream.ToArray();
                }

                img = System.Drawing.Image.FromFile(@"C:\PMS\Config\logo_2.jpg");
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