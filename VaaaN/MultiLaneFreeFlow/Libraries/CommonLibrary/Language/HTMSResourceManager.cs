using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Language
{
    public class ATMSResourceManager
    {
        private System.Resources.ResourceManager resourceManager;
        private string resourceBaseName = "VaaaN.ATMS.Libraries.CommonLibrary.Language.Resource";

        /// <summary>
        /// Constructor <br>
        /// </summary>
        public ATMSResourceManager()
        {
            SetCultureInfo(new CultureInfo("en"));
        }

        /// <summary>
        /// Set the current culture info
        /// </summary>
        /// <param name="cInfo"></param>
        private void SetCultureInfo(CultureInfo cInfo)
        {
            Thread.CurrentThread.CurrentCulture = cInfo;
            Thread.CurrentThread.CurrentUICulture = cInfo;
            Init();
        }
        /// <summary>
        /// This constructor only for web applications. Because the cultureInfo in the
        /// default constructor for the web applications is always En-US
        /// </summary>
        /// <param name="cf">Current culture information</param>
        public ATMSResourceManager(CultureInfo cInfo)
        {
            SetCultureInfo(cInfo);
        }

        /// <summary>
        /// Initialize resource mannager. <br>
        /// The method is called in the constructor of this class. The client cannot invoke the
        /// method directly. 
        /// </summary>
        protected void Init()
        {
            try
            {
                resourceManager = new ResourceManager(resourceBaseName, Assembly.GetExecutingAssembly());
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message);
                throw (e);
            }
        }

        /// <summary>
        /// Retrieve string resource according to the string name.
        /// </summary>
        /// <param name="resourceKey">string name</param>
        /// <returns>the value of <c>name</c> if successful,
        ///  <c>name</c>, otherwise. 
        /// </returns>
        public string GetString(string resourceKey)
        {
            string resourceValue = "";
            try
            {
                resourceValue = resourceManager.GetString(resourceKey);
                if (resourceValue == null)
                {
                    // if the resource not exist, retrieve the default value
                    ResourceSet resourceSet =
                        resourceManager.GetResourceSet(new CultureInfo("en"), true, false);
                    resourceValue = resourceSet.GetString(resourceKey);
                }
            }
            catch (System.Exception)
            {
                resourceValue = resourceKey;
            }
            finally
            {
                if (resourceValue == null || resourceValue == "")
                {
                    resourceValue = resourceKey;
                }
            }
            return resourceValue;
        }

        /// <summary>
        /// Get Object from the resource file. It can be image ,video, audio and 
        /// others.
        /// </summary>
        /// <param name="resourceKey">Name of the resource</param>
        /// <returns>Return Object</returns>
        public object GetObject(string resourceKey)
        {
            object resourceValue = null;
            resourceValue = resourceManager.GetObject(resourceKey);

            return resourceValue;
        }
    }
}

