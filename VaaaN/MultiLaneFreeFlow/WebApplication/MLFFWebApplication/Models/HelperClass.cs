using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
                            sb.Append("<a id=\"module_" + module.ModuleId + "\" class=\"parent list-group-item list-group-item-info\"   data-parent=\"#MainMenu\" href=\"" + module.ModuleUrl.ToLower() + "\">");//module.Url
                                                                                                                                                                                                                 //// sb.Append("<i  style=\"padding-right:10%; padding-left:1%;\" class=\"" + "" + "\"></i>"+ module.ModuleName + "</a>");
                            sb.Append("<i  style=\"padding-right:10%; padding-left:1%;\" class=\"" + "" + "\"></i>" + someString + "</a>");

                        }
                    }
                    else
                    {
                        sb.Append("<a id=\"module_" + module.ModuleId + "\" class=\"parent list-group-item list-group-item-info\"  data-toggle=\"collapse\"  data-Parent=\"#MainMenu\" href=\"#" + someString + "subMenu\">");

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
        public static string ImageSave(System.Web.HttpPostedFileBase file, string[] imageTypes, ref bool Value, string ImageName)
        {
            string ImagePath = string.Empty;
            if (file != null && file.ContentLength > 0)
            {
                if (!imageTypes.Contains(file.ContentType))
                {
                    Value = true;
                    return ImagePath;
                }

                if (file.ContentLength > 2097152) // about 2 MB
                {
                    Value = true;
                    return ImagePath;
                }
            }
            if (Value == false)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string vehicleImageName = ImageName + "_" + String.Format("{0:yyyyMMdd}", DateTime.Now);
                    string extension = System.IO.Path.GetExtension(file.FileName).ToLower();

                    String uploadFilePath = "\\Attachment\\";
                    // create a folder for distinct user -
                    string FolderName = "VehicleImage";
                    string pathWithFolderName = HttpContext.Current.Server.MapPath(uploadFilePath + FolderName);

                    bool folderExists = Directory.Exists(pathWithFolderName);
                    if (!folderExists)
                        Directory.CreateDirectory(pathWithFolderName);

                    if (extension.ToLower() == ".pdf")
                    {
                        //string renamedFile = System.Guid.NewGuid().ToString("N");
                        string filePath = String.Format(pathWithFolderName + "\\{0}{1}", vehicleImageName, extension);
                        file.SaveAs(filePath);
                    }
                    else
                    {
                        using (var img = System.Drawing.Image.FromStream(file.InputStream))
                        {
                            string filePath = String.Format(pathWithFolderName + "\\{0}{1}", vehicleImageName, extension);

                            // Save large size image, 600 x 600
                            VaaaN.MLFF.Libraries.CommonLibrary.Common.CommonClass.SaveToFolder(img, extension, new System.Drawing.Size(600, 600), filePath);
                        }
                    }
                    ImagePath = vehicleImageName + extension;
                }

            }

            return ImagePath;
        }
        public static string GetAPIUrl()
        {
            return System.Configuration.ConfigurationManager.AppSettings["apiPath"] + HttpContext.Current.Request.Url.Host + System.Configuration.ConfigurationManager.AppSettings["apiPort"];
        }
    }
}