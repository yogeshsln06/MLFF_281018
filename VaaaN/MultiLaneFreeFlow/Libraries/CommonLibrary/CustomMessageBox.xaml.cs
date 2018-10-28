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

using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace VaaaN.MLFF.Libraries.CommonLibrary
{
    /// <summary>
    /// Create the customized message box.
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        private static MessageBoxResult messageBoxResult = MessageBoxResult.None;
        private static CustomMessageBox customMessageBox;

        public CustomMessageBox()
        {
            InitializeComponent();
        }

        #region Show overloaded methods
        public static MessageBoxResult Show(string messageBoxText)
        {
            return Show(null, messageBoxText, "", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(string messageBoxText, string caption)
        {
            return Show(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText)
        {
            return Show(owner, messageBoxText, "", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            return Show(null, messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption)
        {
            return Show(owner, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return Show(null, messageBoxText, caption, button, icon, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button)
        {
            return Show(owner, messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
        {
            return Show(null, messageBoxText, caption, button, icon, defaultResult, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return Show(owner, messageBoxText, caption, button, icon, MessageBoxResult.None, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options)
        {
            return Show(null, messageBoxText, caption, button, icon, defaultResult, options);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
        {
            return Show(owner, messageBoxText, caption, button, icon, defaultResult, MessageBoxOptions.None);
        }

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options)
        {
            customMessageBox = new CustomMessageBox();
            customMessageBox.labelMessage.Text = messageBoxText;
            //customMessageBox.labelCaption.Content = caption;

            if (owner != null)
            {
                customMessageBox.Owner = owner;
            }
            
            System.Drawing.Bitmap bMap = null;
            switch (icon)
            {
                case MessageBoxImage.Asterisk:
                    bMap = System.Drawing.SystemIcons.Asterisk.ToBitmap();
                    if (caption.Length == 0)
                    {
                        caption = "Information";
                    }
                    break;

                case MessageBoxImage.Error:
                    bMap = System.Drawing.SystemIcons.Error.ToBitmap();
                    if (caption.Length == 0)
                    {
                        caption = "Error";
                    }
                    break;

                case MessageBoxImage.Exclamation:
                    bMap = System.Drawing.SystemIcons.Exclamation.ToBitmap();
                    if (caption.Length == 0)
                    {
                        caption = "Exclamation";
                    }
                    break;

                case MessageBoxImage.Question:
                    bMap = System.Drawing.SystemIcons.Question.ToBitmap();
                    if (caption.Length == 0)
                    {
                        caption = "Question";
                    }
                    break;

                default:
                    bMap = System.Drawing.SystemIcons.Information.ToBitmap();
                    if (caption.Length == 0)
                    {
                        caption = "Information";
                    }
                    break;
            }

            if (caption.Length == 0)
            {
                caption = "Message";
            }

            customMessageBox.labelCaption.Content = caption;
           
            MemoryStream ms = new MemoryStream();
            bMap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            System.Windows.Media.Imaging.BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();
            bImg.BeginInit();
            bImg.StreamSource = new MemoryStream(ms.ToArray());
            bImg.EndInit();
            customMessageBox.imageIcon.Source = bImg;
            
            //default result
            //options
            CreateButton(button);

            //Set button focus
            string buttonName = string.Empty;
            switch (button)
            {
                case MessageBoxButton.OK:
                    {
                        buttonName = "OK";
                        break;
                    }
                case MessageBoxButton.OKCancel:
                    {
                        buttonName = "OK";
                        break;
                    }
                case MessageBoxButton.YesNo:
                    {
                        buttonName = "NO";
                        break;
                    }
                case MessageBoxButton.YesNoCancel:
                    {
                        buttonName = "OK";
                        break;
                    }
            }
            for (int i = 0; i < customMessageBox.stackPanelButton.Children.Count; i++)
            {
                Button b = (Button)customMessageBox.stackPanelButton.Children[i];
                if (b.Name == buttonName)
                {
                    b.Focus();
                    break;
                }
            }

            customMessageBox.ShowDialog();
            return messageBoxResult;
        }
        #endregion

        #region Helper Methods
        private static void CreateButton(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OK:
                    {
                        Button buttonOK = new Button();
                        buttonOK.Name = "OK";
                        buttonOK.Tag = "ok";
                        buttonOK.Content = "OK";
                        buttonOK.Width = 80;
                        buttonOK.Height = 40;
                        buttonOK.FontSize = 12.0;
                        buttonOK.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");
                        buttonOK.Margin = new Thickness(0, 0, 5, 0);
                        buttonOK.Click += new RoutedEventHandler(customMessageBox.button_Click);
                        buttonOK.Focusable = true;
                        buttonOK.Focus();
                        customMessageBox.stackPanelButton.Children.Add(buttonOK);
                    }
                    break;

                case MessageBoxButton.OKCancel:
                    {
                        Button buttonCancel = new Button();
                        buttonCancel.Name = "Cancel";
                        buttonCancel.Tag = "cancel";
                        buttonCancel.Content = "Cancel";
                        buttonCancel.Width = 80;
                        buttonCancel.Height = 40;
                        buttonCancel.Margin = new Thickness(0, 0, 5, 0);
                        buttonCancel.Click += new RoutedEventHandler(customMessageBox.button_Click);
                        customMessageBox.stackPanelButton.Children.Add(buttonCancel);

                        Button buttonOK = new Button();
                        buttonOK.Name = "OK";
                        buttonOK.Tag = "ok";
                        buttonOK.Content = "OK";
                        buttonOK.Width = 80;
                        buttonOK.Height = 40;
                        buttonOK.Margin = new Thickness(0, 0, 5, 0);
                        buttonOK.Focusable = true;
                        buttonOK.Focus();
                        buttonOK.Click += new RoutedEventHandler(customMessageBox.button_Click);
                        customMessageBox.stackPanelButton.Children.Add(buttonOK);
                    }
                    break;

                case MessageBoxButton.YesNo:
                    {
                        Button buttonNo = new Button();
                        buttonNo.Name = "No";
                        buttonNo.Tag = "no";
                        buttonNo.Content = "No";
                        buttonNo.Width = 80;
                        buttonNo.Height = 40;
                        buttonNo.Margin = new Thickness(0, 0, 5, 0);
                        buttonNo.Click += new RoutedEventHandler(customMessageBox.button_Click);
                        buttonNo.Focusable = true;
                        buttonNo.Focus();
                        customMessageBox.stackPanelButton.Children.Add(buttonNo);

                        Button buttonYes = new Button();
                        buttonYes.Name = "Yes";
                        buttonYes.Tag = "yes";
                        buttonYes.Content = "Yes";
                        buttonYes.Width = 80;
                        buttonYes.Height = 40;
                        buttonYes.Margin = new Thickness(0, 0, 5, 0);
                        buttonYes.Click += new RoutedEventHandler(customMessageBox.button_Click);
                        customMessageBox.stackPanelButton.Children.Add(buttonYes);
                    }
                    break;

                case MessageBoxButton.YesNoCancel:
                    {
                        Button buttonCancel = new Button();
                        buttonCancel.Name = "Cancel";
                        buttonCancel.Tag = "cancel";
                        buttonCancel.Content = "Cancel";
                        buttonCancel.Width = 80;
                        buttonCancel.Height = 40;
                        buttonCancel.Margin = new Thickness(0, 0, 5, 0);
                        buttonCancel.Click += new RoutedEventHandler(customMessageBox.button_Click);
                        customMessageBox.stackPanelButton.Children.Add(buttonCancel);

                        Button buttonNo = new Button();
                        buttonNo.Name = "NO";
                        buttonNo.Tag = "no";
                        buttonNo.Content = "No";
                        buttonNo.Width = 80;
                        buttonNo.Height = 40;
                        buttonNo.Margin = new Thickness(0, 0, 5, 0);
                        buttonNo.Click += new RoutedEventHandler(customMessageBox.button_Click);
                        buttonNo.Focusable = true;
                        buttonNo.Focus();
                        customMessageBox.stackPanelButton.Children.Add(buttonNo);

                        Button buttonYes = new Button();
                        buttonYes.Name = "Yes";
                        buttonYes.Tag = "yes";
                        buttonYes.Content = "Yes";
                        buttonYes.Width = 80;
                        buttonYes.Height = 40;
                        buttonYes.Margin = new Thickness(0, 0, 5, 0);
                        buttonYes.Click += new RoutedEventHandler(customMessageBox.button_Click);
                        customMessageBox.stackPanelButton.Children.Add(buttonYes);
                    }
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Event
        void button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            string bTag = b.Tag.ToString().ToLower();
            switch (bTag)
            {
                case "cancel":
                    messageBoxResult = MessageBoxResult.Cancel;
                    break;
                case "no":
                    messageBoxResult = MessageBoxResult.No;
                    break;
                case "ok":
                    messageBoxResult = MessageBoxResult.OK;
                    break;
                case "yes":
                    messageBoxResult = MessageBoxResult.Yes;
                    break;
                default:
                    messageBoxResult = MessageBoxResult.None;
                    break;
            }

            customMessageBox.Close();
        }
        #endregion
    }
}
