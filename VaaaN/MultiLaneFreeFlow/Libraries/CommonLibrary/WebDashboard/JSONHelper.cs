using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace VaaaN.MLFF.Libraries.CommonLibrary.WebDashboard
{
    public class JSONHelper
    {
        /// <summary>
        /// Converting object into JSON format
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            string json = new JavaScriptSerializer().Serialize(obj);
            return json;
        }

        /// <summary>
        /// Converting JSON format to object
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static VaaaN.MLFF.Libraries.CommonLibrary.WebDashboardStatusClass Deserialize(string jsonData)
        {
            WebDashboardStatusClass status = new JavaScriptSerializer().Deserialize<WebDashboardStatusClass>(jsonData);
            return status;
        }
    }
}
