#region Copyright message
/*
© copyright 2011 VaaaN Infra (P) Ltd. All rights reserved.

This file contains Proprietary information of VaaaN Infra (P) Ltd.
and should be treated as proprietary and confidential.

The use, copying, disclosure or modification of the programs and/or 
source code is prohibited unless otherwise provided for in the license 
or confidential agreements.

========================================================
Author           :  VaaaN Infra                  
Company          :  VaaaN Infra     
Date of Creation :                              
========================================================
*/
#endregion

using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace VaaaN.TollMax.Library.TollMaxLibrary.Language
{
    /// <summary>
    /// This class is used to get the resorce from the file. Key is specified 
    /// to get the value.
    /// </summary>
	[Serializable]
	public class TollMaxResourceManager
	{
        private System.Resources.ResourceManager resourceManager;
		private string resourceBaseName = "VaaaN.TollMax.Library.TollMaxLibrary.Language.Resource";
    
		/// <summary>
		/// Constructor <br>
		/// </summary>
		public TollMaxResourceManager()
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
        public TollMaxResourceManager(CultureInfo cInfo)
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
			catch(ArgumentException e)
			{
				VaaaN.TollMax.Library.TollMaxLibrary.CustomMessageBox.Show(e.Message);
				throw(e);
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
