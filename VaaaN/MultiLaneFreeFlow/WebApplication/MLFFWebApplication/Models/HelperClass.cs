using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace VaaaN.MLFF.WebApplication.Models
{
    public static class HelperClass
    {
        public static string NewMenu(int userId)
        {
            StringBuilder sb = new StringBuilder();
            //string path = Server.MapPath("");
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCollection modules = null;
                // Get all modules which are authorized to this user.
                if (userId == 1)// super admin
                {
                    modules = VaaaN.MLFF.Libraries.CommonLibrary.DAL.ModuleDAL.GetAll();
                }
                else
                {
                    modules = VaaaN.MLFF.Libraries.CommonLibrary.DAL.ModuleDAL.GetByUserId(userId);
                }

                foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.ModuleCBE module in modules)
                {
                    string link = "";

                    // Get all submodules which are authorized to this user
                    VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection subModules = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection();

                    if (userId == 1)// super admin
                    {
                        VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCollection subModulesTemp = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubModuleBLL.GetAll();

                        foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCBE subModule in subModulesTemp)
                        {
                            if (subModule.ModuleId == module.ModuleId)
                            {
                                subModules.Add(subModule);
                            }
                        }
                    }
                    else
                    {
                        subModules = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubModuleBLL.GetByUserId(userId, module.ModuleId);
                    }

                    link = string.Empty;
                    ResourceManager rm = new ResourceManager(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen));
                    string someString = rm.GetString(module.ModuleName);
                

                    if ((subModules.Count == 0) || module.ModuleId == 7) //7 for reports
                    { 

                        if (module.IsGuiVisible != 0)
                        {
                            sb.Append("<a id=\"module_" + module.ModuleId + "\" class=\"list-group-item list-group-item-info\" style=\"background-color:rgb(46, 109, 164);  color:rgb(217, 237, 247);\"  data-parent=\"#MainMenu\" href=\"" + module.ModuleUrl.ToLower() + "\">");//module.Url
                           //// sb.Append("<i  style=\"padding-right:10%; padding-left:1%;\" class=\"" + "" + "\"></i>"+ module.ModuleName + "</a>");
                            sb.Append("<i  style=\"padding-right:10%; padding-left:1%;\" class=\"" + "" + "\"></i>"+ someString + "</a>");

                        }
                    }
                    else
                    {
                        sb.Append("<a id=\"module_" + module.ModuleId + "\" class=\"list-group-item list-group-item-info\" style=\"background-color:rgb(46, 109, 164);  color:rgb(217, 237, 247);\" data-toggle=\"collapse\"  data-Parent=\"#MainMenu\" href=\"#" + someString + "subMenu\">");

                        sb.Append("<i  style=\"padding-right:10%; padding-left:1%;\" class=\"" + "" + "\"></i>" + someString + "");
                        sb.Append("<i  style=\"float:right\" class=\"fa fa-caret-down\"></i></a>");
                        sb.Append("<div class=\"collapse list-group-submenu\"  id=\"" + someString + "subMenu\">");
                        foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCBE subModule in subModules)
                        {
                            if (subModule.IsGuiVisible != 0)
                            {
                                string substring = rm.GetString(subModule.SubModuleName);
                                sb.Append("<a id=\"submodule_" + subModule.SubModuleId + "\" class=\"list-group-item list-group-item-info\" data-parent=\"#" + module.ModuleName + "subMenu\" href=\"" + subModule.SubmoduleUrl.ToLower() + "\">");//Submodule.Url
                                sb.Append("<i  style=\"padding-right:10%; padding-left:1%;\" class=\"fa fa-caret-right\"></i>" + substring + "</a>");

                            }
                        }
                        sb.Append("</div>");
                    }
                }
            }
            catch (Exception)
            {
                //VaaaN.ATMS.Libraries.CCHLibrary.ErrorLogger.LogMessage("Failed to create menu." + ex.Message);
            }

            return sb.ToString();
        }

        public static void LogMessage(string msg)
        {
            VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write(msg, VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.MLFFWeb);
        }

        public static IEnumerable<SelectListItem> GetReviewTransactionCategory()
        {
            var selectList = new List<SelectListItem>();

            // Get all values of the Industry enum
            var enumValues = Enum.GetValues(typeof(Libraries.CommonLibrary.Constants.ReviewTransactionCategory)) as Libraries.CommonLibrary.Constants.ReviewTransactionCategory[];
            if (enumValues == null)
                return null;

            foreach (var enumValue in enumValues)
            {
                // Create a new SelectListItem element and set its 
                // Value and Text to the enum value and description.
                selectList.Add(new SelectListItem
                {
                    Value = Convert.ToInt32(enumValue).ToString(),
                    // GetIndustryName just returns the Display.Name value
                    // of the enum - check out the next chapter for the code of this function.
                    Text = GetTransactionCategoryName(enumValue)
                });
            }

            return selectList;
        }
        public static IEnumerable<SelectListItem> GetManualReviewTransactionCategory()
        {
            var selectList = new List<SelectListItem>();

            // Get all values of the Industry enum
            var enumValues = Enum.GetValues(typeof(Libraries.CommonLibrary.Constants.ManualReviewTransactionCategory)) as Libraries.CommonLibrary.Constants.ManualReviewTransactionCategory[];
            if (enumValues == null)
                return null;

            foreach (var enumValue in enumValues)
            {
                // Create a new SelectListItem element and set its 
                // Value and Text to the enum value and description.
                selectList.Add(new SelectListItem
                {
                    Value = Convert.ToInt32(enumValue).ToString(),
                    // GetIndustryName just returns the Display.Name value
                    // of the enum - check out the next chapter for the code of this function.
                    Text = GetManualTransactionCategoryName(enumValue)
                });
            }

            return selectList;
        }
        private static string GetManualTransactionCategoryName(Libraries.CommonLibrary.Constants.ManualReviewTransactionCategory value)
        {
            // Get the MemberInfo object for supplied enum value
            var memberInfo = value.GetType().GetMember(value.ToString());
            if (memberInfo.Length != 1)
                return null;

            // Get DisplayAttibute on the supplied enum value
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false)
                                   as DisplayAttribute[];
            if (displayAttribute == null || displayAttribute.Length != 1)
                return null;

            return displayAttribute[0].Name;
        }
        private static string GetTransactionCategoryName(Libraries.CommonLibrary.Constants.ReviewTransactionCategory value)
        {
            // Get the MemberInfo object for supplied enum value
            var memberInfo = value.GetType().GetMember(value.ToString());
            if (memberInfo.Length != 1)
                return null;

            // Get DisplayAttibute on the supplied enum value
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false)
                                   as DisplayAttribute[];
            if (displayAttribute == null || displayAttribute.Length != 1)
                return null;

            return displayAttribute[0].Name;
        }

        public static IEnumerable<SelectListItem> GetReportTransactionCategory()
        {
            var selectList = new List<SelectListItem>();

            // Get all values of the Industry enum
            var enumValues = Enum.GetValues(typeof(Libraries.CommonLibrary.Constants.ReportTransactionCategory)) as Libraries.CommonLibrary.Constants.ReportTransactionCategory[];
            if (enumValues == null)
                return null;

            foreach (var enumValue in enumValues)
            {
                // Create a new SelectListItem element and set its 
                // Value and Text to the enum value and description.
                selectList.Add(new SelectListItem
                {
                    Value = Convert.ToInt32(enumValue).ToString(),
                    // GetIndustryName just returns the Display.Name value
                    // of the enum - check out the next chapter for the code of this function.
                    Text = GetTransactionCategoryName(enumValue)
                });
            }

            return selectList;
        }

        private static string GetTransactionCategoryName(Libraries.CommonLibrary.Constants.ReportTransactionCategory value)
        {
            // Get the MemberInfo object for supplied enum value
            var memberInfo = value.GetType().GetMember(value.ToString());
            if (memberInfo.Length != 1)
                return null;

            // Get DisplayAttibute on the supplied enum value
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false)
                                   as DisplayAttribute[];
            if (displayAttribute == null || displayAttribute.Length != 1)
                return null;

            return displayAttribute[0].Name;
        }


        public static IEnumerable<SelectListItem> GetHardwareType()
        {
            var selectList = new List<SelectListItem>();

            // Get all values of the Industry enum
            var enumValues = Enum.GetValues(typeof(Libraries.CommonLibrary.Constants.HardwareType)) as Libraries.CommonLibrary.Constants.HardwareType[];
            if (enumValues == null)
                return null;

            foreach (var enumValue in enumValues)
            {
                // Create a new SelectListItem element and set its 
                // Value and Text to the enum value and description.
                selectList.Add(new SelectListItem
                {
                    Value = Convert.ToInt32(enumValue).ToString(),
                    // GetIndustryName just returns the Display.Name value
                    // of the enum - check out the next chapter for the code of this function.
                    Text = GetHardwareTypeName(enumValue)
                });
            }

            return selectList;
        }

        private static string GetHardwareTypeName(Libraries.CommonLibrary.Constants.HardwareType value)
        {
            // Get the MemberInfo object for supplied enum value
            var memberInfo = value.GetType().GetMember(value.ToString());
            if (memberInfo.Length != 1)
                return null;

            // Get DisplayAttibute on the supplied enum value
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false)
                                   as DisplayAttribute[];
            if (displayAttribute == null || displayAttribute.Length != 1)
                return null;

            return displayAttribute[0].Name;
        }

        public static IEnumerable<SelectListItem> GetHardwarePosition()
        {
            var selectList = new List<SelectListItem>();

            // Get all values of the Industry enum
            var enumValues = Enum.GetValues(typeof(Libraries.CommonLibrary.Constants.HardwarePosition)) as Libraries.CommonLibrary.Constants.HardwarePosition[];
            if (enumValues == null)
                return null;

            foreach (var enumValue in enumValues)
            {
                // Create a new SelectListItem element and set its 
                // Value and Text to the enum value and description.
                selectList.Add(new SelectListItem
                {
                    Value = Convert.ToInt32(enumValue).ToString(),
                    // GetIndustryName just returns the Display.Name value
                    // of the enum - check out the next chapter for the code of this function.
                    Text = GetHardwarePositionName(enumValue)
                });
            }

            return selectList;
        }

        private static string GetHardwarePositionName(Libraries.CommonLibrary.Constants.HardwarePosition value)
        {
            // Get the MemberInfo object for supplied enum value
            var memberInfo = value.GetType().GetMember(value.ToString());
            if (memberInfo.Length != 1)
                return null;

            // Get DisplayAttibute on the supplied enum value
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false)
                                   as DisplayAttribute[];
            if (displayAttribute == null || displayAttribute.Length != 1)
                return null;

            return displayAttribute[0].Name;
        }


        public static IEnumerable<SelectListItem> GetLaneType()
        {
            var selectList = new List<SelectListItem>();

            // Get all values of the Industry enum
            var enumValues = Enum.GetValues(typeof(Libraries.CommonLibrary.Constants.LaneType)) as Libraries.CommonLibrary.Constants.LaneType[];
            if (enumValues == null)
                return null;

            foreach (var enumValue in enumValues)
            {
                // Create a new SelectListItem element and set its 
                // Value and Text to the enum value and description.
                selectList.Add(new SelectListItem
                {
                    Value = Convert.ToInt32(enumValue).ToString(),
                    // GetIndustryName just returns the Display.Name value
                    // of the enum - check out the next chapter for the code of this function.
                    Text = GeLaneTypeName(enumValue)
                });
            }

            return selectList;
        }

        private static string GeLaneTypeName(Libraries.CommonLibrary.Constants.LaneType value)
        {
            // Get the MemberInfo object for supplied enum value
            var memberInfo = value.GetType().GetMember(value.ToString());
            if (memberInfo.Length != 1)
                return null;

            // Get DisplayAttibute on the supplied enum value
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false)
                                   as DisplayAttribute[];
            if (displayAttribute == null || displayAttribute.Length != 1)
                return null;

            return displayAttribute[0].Name;
        }
    }
}