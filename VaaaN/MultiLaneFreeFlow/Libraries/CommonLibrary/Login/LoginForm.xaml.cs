
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Reflection;
using System.Windows.Controls.Primitives;
using System.Runtime.InteropServices;
using System.Collections;
using System.Data.Common;
using System.Data;
//using BioEnable.SDK.BioEnBSP;
//using NITGEN.SDK.NBioBSP;

namespace VaaaN.MLFF.Libraries.CommonLibrary.Login
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE currentUser = new VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE();
        VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.GeneralConfiguration generalConfig;
        int maxLoginAttempt;
        int loginAttempt;
        VaaaN.MLFF.Libraries.CommonLibrary.Language.ATMSResourceManager hrm;
        VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule logModuleName = VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.ErrorLogModule.LoginModule;
        int atmsId;

        #region Bioenable
        //BioAPI m_BioAPI;
        //NBioAPI.Type.WINDOW_OPTION m_WinOption;
        //NBioAPI.Type.DEVICE_INFO_EX[] m_DeviceInfoEx;
        //NBioAPI.Type.FIR_TEXTENCODE m_textFIR;
        //static short DeviceID = NBioAPI.Type.DEVICE_ID.NONE;
        //uint ret;
        #endregion

        public LoginForm(int ATMSId)
        {
            try
            {
                InitializeComponent();
                this.atmsId = ATMSId;

                hrm = new Language.ATMSResourceManager();
                generalConfig = VaaaN.MLFF.Libraries.CommonLibrary.XMLConfigurationClasses.GeneralConfiguration.Deserialize();

                textBlockVersionInfo.Text = "";
                textBlockVersionInfo.Inlines.Add(VaaaN.MLFF.Libraries.CommonLibrary.Constants.GetModuleNameForApplicationHeader(""));

                labelErrorMessage.Content = "";

                this.maxLoginAttempt = 5;
                this.loginAttempt = 0;

                #region Delete log file
                try
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.Constants.DeleteLogFile();
                }
                catch (Exception ex)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write("Failed to delete log file. " + ex.ToString(), logModuleName);
                }

                #endregion

                //if (tollCollectionConfig.IsBiometricLogin == "false")
                //{
                labelCheckBoxBiometric.Visibility = Visibility.Hidden;
                checkBoxIsBiometricLogin.Visibility = Visibility.Hidden;
                //}
                //else
                //{
                //textBoxPassword.IsEnabled = false;
                //checkBoxIsBiometricLogin.IsChecked = true;
                //checkBoxIsBiometricLogin.IsEnabled = false;
                //}
            }
            catch (Exception ex)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write("Failed to initialize login form" + ex.ToString(), logModuleName);
            }
        }

        public VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCBE CurrentUser
        {
            get { return this.currentUser; }
        }

        #region Events
        //Validate User
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (textBoxUserName.IsFocused)
            {
                // Password text box will be disabled for biometric login
                if (textBoxPassword.IsEnabled != false)
                {
                    textBoxPassword.Focus();
                    return;
                }
                else
                {
                    // It means biometric login 
                    buttonLogin.Focus();
                }
            }

            try
            {

                //#region Check plaza id mismatch in lane computer
                //try
                //{
                //    //Current computer plaza id which is mentioned in text file and plaza id in all tables must be same.
                //    if (!VaaaN.ATMS.Libraries.CommonLibrary.Constants.IsCurrentComputerLane() && !VaaaN.TollMax.Library.Constants.IsCurrentComputerHQMS())
                //    {
                //        bool plazaIdMismatch = false;

                //        //Get list of all tables
                //        ArrayList alTables = VaaaN.TollMax.Library.Constants.GetListOfAllDatabaseTables();

                //        //Check plaza id mismatch in all tables
                //        for (int i = 0; i < alTables.Count; i++)
                //        {
                //            string tableName = alTables[i].ToString();
                //            string query = "select count(*) from " + tableName + " where plaza_id <> " + plazaId;
                //            DbCommand cmd = VaaaN.TollMax.Library.DBF.DatabaseFunctions.GetSqlStringCommand(query);
                //            DataSet ds = VaaaN.TollMax.Library.DBF.DatabaseFunctions.LoadDataSet(cmd, tableName);

                //            if (tableName.ToLower() != "tbl_journey_plaza")
                //            {
                //                if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                //                {
                //                    VaaaN.ATMS.Libraries.CommonLibrary.Logger.Log.Write("Plaza id is mismatched in table " + tableName, logModuleName);
                //                    plazaIdMismatch = true;
                //                }
                //            }
                //        }

                //        if (plazaIdMismatch)
                //        {
                //            labelErrorMessage.Content = "Plaza ID is mismatched, please contact to system administrator.";
                //            return;
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    VaaaN.ATMS.Libraries.CommonLibrary.Logger.Log.Write("Failed to check plaza id mismatch." + ex.ToString(), logModuleName);
                //}
                //#endregion

                if (textBoxUserName.Text.Trim().Length == 0)
                {
                    labelErrorMessage.Content = hrm.GetString("Login_UserNameNotblank");
                    return;
                }

                if (textBoxPassword.Password.Trim().Length == 0 && checkBoxIsBiometricLogin.IsChecked != true)
                {
                    labelErrorMessage.Content = hrm.GetString("Login_PasswordNotblank");
                    return;
                }

                //Case insensensitive user name
                string loginName = this.textBoxUserName.Text.Trim().ToLower();
                //Case sensitive password
                string password = this.textBoxPassword.Password;

                Constants.LoginResult result = Constants.LoginResult.InvalidUser;

                try
                {
                    if (checkBoxIsBiometricLogin.IsChecked == true)
                    {
                        checkBoxIsBiometricLogin.IsChecked = false;
                        //If biometric login only
                        if (VerifyFingerPrint())
                        {
                            result = VaaaN.MLFF.Libraries.CommonLibrary.Constants.LoginResult.Successful;
                        }
                        else
                        {
                            result = VaaaN.MLFF.Libraries.CommonLibrary.Constants.LoginResult.FingerPrintNotMatched;
                            checkBoxIsBiometricLogin.IsChecked = true;
                            checkBoxIsBiometricLogin.IsEnabled = false;
                        }
                    }
                    else
                    {
                        //Check server connectivity

                        bool isPingSuccessful = true;

                        for (int i = 0; i < 5; i++)
                        {
                            //isPingSuccessful = VaaaN.MLFF.Libraries.CommonLibrary.Constants.IsPingSuccessful(generalConfig.AtmsServerIPAddress);

                            if (isPingSuccessful)
                                break;

                            System.Threading.Thread.Sleep(100);
                        }
                        isPingSuccessful = true;
                        if (isPingSuccessful)
                        {
                            result = VaaaN.MLFF.Libraries.CommonLibrary.BLL.LoginBLL.ValidateUser(loginName, password, ref currentUser);
                        }
                        else
                        {
                            result = VaaaN.MLFF.Libraries.CommonLibrary.Constants.LoginResult.DatabaseServerNotConnected;
                        }
                    }

                    switch (result)
                    {
                        case Constants.LoginResult.Successful:
                            this.DialogResult = true;
                            break;
                        case Constants.LoginResult.InvalidUser:
                            this.labelErrorMessage.Content = hrm.GetString("Login_InvalidUser");
                            this.UnsuccessfulLogin();
                            break;
                        case Constants.LoginResult.DatabaseError:
                            this.labelErrorMessage.Content = hrm.GetString("CMN_LBL_DBError");
                            this.UnsuccessfulLogin();
                            break;
                        case Constants.LoginResult.InvalidPassword:
                            this.labelErrorMessage.Content = hrm.GetString("Login_InvalidPassword");
                            this.UnsuccessfulLogin(false);
                            break;
                        case Constants.LoginResult.CannotLogin:
                            this.labelErrorMessage.Content = hrm.GetString("Login_CanNotLogin");
                            this.UnsuccessfulLogin();
                            break;
                        case Constants.LoginResult.AccountExpired:
                            this.labelErrorMessage.Content = hrm.GetString("Login_UserAccountExpired");
                            this.UnsuccessfulLogin();
                            break;
                        case Constants.LoginResult.FingerPrintNotMatched:
                            this.labelErrorMessage.Content = "Finger Print Mismatched";
                            this.UnsuccessfulLogin();
                            break;
                        case Constants.LoginResult.DatabaseServerNotConnected:
                            this.labelErrorMessage.Content = "Database server not connected.";
                            this.UnsuccessfulLogin();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write("Failed to validate user." + ex.ToString(), logModuleName);
                    //VaaaN.TollMax.Library.CustomMessageBox.Show(ex.Message, trm.GetString("CMN_LBL_Error"), MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    labelErrorMessage.Content = "Failed to validate user." + Environment.NewLine + ex.Message;
                    this.UnsuccessfulLogin();
                }

                if (loginAttempt == maxLoginAttempt)
                {
                    this.labelErrorMessage.Content = "Maximum login limit reached.";
                    this.loginAttempt = 0;
                }

                #region Delete 10 days older log files
                //try
                //{
                //    VaaaN.TollMax.Library.Constants.LogFileRemover();
                //}
                //catch (Exception ex)
                //{
                //    VaaaN.ATMS.Libraries.CommonLibrary.Logger.Log.Write("Failed to delete log file. " + ex.ToString(), VaaaN.TollMax.SharedLibrary.FileIO.Log.ErrorLogModule.Login);
                //}
                #endregion
            }
            catch (Exception ex)
            {
                VaaaN.MLFF.Libraries.CommonLibrary.Logger.Log.Write("Failed to initialize login form" + ex.ToString(), logModuleName);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Set the language from the resource file.
        /// </summary>
        private void SetLanguage()
        {

        }

        /// <summary>
        /// Things needs to be done when login attempt failed
        /// </summary>
        private void UnsuccessfulLogin()
        {
            UnsuccessfulLogin(true);
        }

        /// <summary>
        /// Things needs to be done when login attempt failed
        /// </summary>
        private void UnsuccessfulLogin(bool clearUserId)
        {
            //this.textBoxPassword.Text = "";
            if (clearUserId)
            {
                this.textBoxUserName.Text = "";
                this.textBoxPassword.Password = "";
                this.textBoxUserName.Focus();
            }
            else
            {
                this.textBoxPassword.Focus();
            }
            this.loginAttempt++;
        }
        #endregion

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBoxUserName.Focus();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Check Caps Lock key status
            if (Console.CapsLock == true)
            {
                labelErrorMessage.Content = "Caps Lock is on.";
            }
            else
            {
                labelErrorMessage.Content = "";
            }
        }

        private bool VerifyFingerPrint()
        {
            bool result = false;
            try
            {
                VaaaN.MLFF.Libraries.CommonLibrary.CBE.UserCollection users = VaaaN.MLFF.Libraries.CommonLibrary.DAL.LoginDAL.GetUserByLoginName(textBoxUserName.Text.Trim());

                if (users.Count == 0)
                {

                }
                else
                {
                    foreach (CBE.UserCBE user in users)
                    {
                        //uint ret;                      
                        //NBioAPI.Type.FIR_PAYLOAD myPayload = new NBioAPI.Type.FIR_PAYLOAD();
                        //NBioAPI.Type.FIR_TEXTENCODE t = new NBioAPI.Type.FIR_TEXTENCODE();
                        //if (user.FingerPrint1 == "")
                        //{
                        //    VaaaN.TollMax.Library.CustomMessageBox.Show("Finger Print is not enrolled for this user. ", hrm.GetString("CMN_LBL_Error"), MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                        //}
                        //else
                        //{
                        //    t.TextFIR = user.FingerPrint1;
                        //}

                        //t.IsWideChar = true;
                        //m_BioAPI = new BioAPI();
                        //uint nNumDevice;
                        //short[] nDeviceID;
                        //NBioAPI.Type.DEVICE_INFO_EX[] deviceInfoEx;
                        //ret = m_BioAPI.EnumerateDevice(out nNumDevice, out nDeviceID, out deviceInfoEx);
                        ////Get Device Id
                        //if (ret == BioAPI.Error.NONE)
                        //{
                        //    DeviceID = NBioAPI.Type.DEVICE_ID.AUTO;
                        //}
                        ////Initialize the device
                        //ret = m_BioAPI.OpenDevice(DeviceID);
                        //if (ret != BioAPI.Error.NONE)
                        //{
                        //    VaaaN.TollMax.Library.CustomMessageBox.Show("Biometric device is not connected.", hrm.GetString("CMN_LBL_Error"), MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                        //    return result;
                        //}

                        //ret = m_BioAPI.Verify(t, out result, myPayload);	// Verify with text encoded FIR

                        ////Close the device
                        //ret = m_BioAPI.CloseDevice(DeviceID);

                        //if (ret != NBioAPI.Error.NONE)
                        //{
                        //    return result = false;
                        //}
                        //else
                        //{
                        //    currentUser = user;
                        //    break;

                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return result;
        }
    }
}

