using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VaaaN.MLFF.Libraries.CommonLibrary;

namespace MLFFWebUI.Models
{
    public static class HelperClass
    {
        public static string Menulinkli = "<li class='nav-item mT-30 active'><a class='sidebar-link' href='[url]'>"
        + "<span class='icon-holder'><i class='c-blue-500 [icon]'></i></span><span class='title'>[menuname]</span></a></li>";
        public static string NewMenu(int userId, string MenuName, string ChildMenuName)
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
                        subModules = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubModuleBLL.GetByModuleId(module.ModuleId);
                    }
                    else
                    {
                        subModules = VaaaN.MLFF.Libraries.CommonLibrary.BLL.SubModuleBLL.GetByUserId(userId, module.ModuleId);
                    }

                    link = string.Empty;
                    ResourceManager rm = new ResourceManager(typeof(VaaaN.MLFF.Libraries.CommonLibrary.Resource.Resourceen));
                    string someString = rm.GetString(module.ModuleName);
                    if (!string.IsNullOrEmpty(module.ModuleUrl) && (subModules.Count == 0))
                    {
                        if (MenuName.ToLower() == module.ModuleName.ToLower())
                        {
                            sb.Append("<li class='nav-item mT-30 active'>");
                        }
                        else
                        {
                            sb.Append("<li class='nav-item'>");
                        }
                        sb.Append(" <a class='sidebar-link' href='" + module.ModuleUrl.ToLower() + "'>");
                        sb.Append("  <span class='icon-holder'>");
                        sb.Append("   <i class='c-blue-500 " + module.Icon + "'></i>");
                        sb.Append("  </span>");
                        sb.Append("   <span class='title'>" + someString + "</span>");
                        sb.Append(" </a>");
                        sb.Append("</li>");
                    }
                    else {
                        if (subModules.Count != 0)
                        {
                            if (MenuName.ToLower() == module.ModuleName.ToLower())
                            {
                                sb.Append("<li class='nav-item dropdown open'>");
                            }
                            else
                            {
                                sb.Append("<li class='nav-item dropdown'>");
                            }

                            sb.Append(" <a class='dropdown-toggle' href='javascript:void(0);'>");
                            sb.Append("  <span class='icon-holder'>");
                            sb.Append("   <i class='c-blue-500 " + module.Icon + "'></i>");
                            sb.Append("  </span>");
                            sb.Append("   <span class='title'>" + someString + "</span>");
                            sb.Append("   <span class='arrow'>");
                            sb.Append("     <i class='ti-angle-right'></i>");
                            sb.Append("   </span>");
                            sb.Append(" </a>");
                            if (MenuName.ToLower() == module.ModuleName.ToLower())
                            {
                                sb.Append(" <ul class='dropdown-menu' style='display: block;'>");
                            }
                            else
                            {
                                sb.Append(" <ul class='dropdown-menu'>");
                            }

                            foreach (VaaaN.MLFF.Libraries.CommonLibrary.CBE.SubmoduleCBE submodule in subModules)
                            {

                                someString = rm.GetString(submodule.SubModuleName);
                                if (ChildMenuName.ToLower() == submodule.SubModuleName.ToLower())
                                {
                                    sb.Append("<li class='active'>");
                                }
                                else
                                {
                                    sb.Append("<li>");
                                }

                                sb.Append(" <a class='dropdown-toggle' href='" + submodule.SubmoduleUrl.ToLower() + "'>");
                                sb.Append("  <span class='icon-holder'>");
                                sb.Append("   <i class='c-indigo-500 " + submodule.Icon + "'></i>");
                                sb.Append("  </span>");
                                sb.Append("   <span class='title'>" + someString + "</span>");
                                sb.Append(" </a>");
                                sb.Append("</li>");
                            }
                            sb.Append(" </ul>");
                            sb.Append("</li>");
                        }
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
            var enumValues = Enum.GetValues(typeof(Constants.ReviewTransactionCategory)) as Constants.ReviewTransactionCategory[];
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
            var enumValues = Enum.GetValues(typeof(Constants.ManualReviewTransactionCategory)) as Constants.ManualReviewTransactionCategory[];
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
        private static string GetManualTransactionCategoryName(Constants.ManualReviewTransactionCategory value)
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
        private static string GetTransactionCategoryName(Constants.ReviewTransactionCategory value)
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
            var enumValues = Enum.GetValues(typeof(Constants.ReportTransactionCategory)) as Constants.ReportTransactionCategory[];
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

        private static string GetTransactionCategoryName(Constants.ReportTransactionCategory value)
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
            var enumValues = Enum.GetValues(typeof(Constants.HardwareType)) as Constants.HardwareType[];
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

        private static string GetHardwareTypeName(Constants.HardwareType value)
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
            var enumValues = Enum.GetValues(typeof(Constants.HardwarePosition)) as Constants.HardwarePosition[];
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

        private static string GetHardwarePositionName(Constants.HardwarePosition value)
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
            var enumValues = Enum.GetValues(typeof(Constants.LaneType)) as Constants.LaneType[];
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

        private static string GeLaneTypeName(Constants.LaneType value)
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


        public class ResponseMessage
        {
            public List<ModelStateList> ModelState { get; set; }
        }

        public class ModelStateList
        {
            public string ErrorMessage { get; set; }
        }
    }
}