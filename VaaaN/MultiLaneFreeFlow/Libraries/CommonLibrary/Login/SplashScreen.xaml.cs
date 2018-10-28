
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

namespace VaaaN.MLFF.Libraries.CommonLibrary.Login
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();

            //labelProductInfo.Content = "TollMax Release 1.1.0.0";
           // labelStatus.Content = "Doing background tasks.";
        }

        DispatcherTimer timer = new DispatcherTimer();

        int counter = 0;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);

            //progressBar1.Minimum = 0;
            //progressBar1.Maximum = 100;
            //progressBar1.Value = 0;

            timer.Start();

            //Duration duration = new Duration(TimeSpan.FromSeconds(10));
            //System.Windows.Media.Animation.DoubleAnimation doubleanimation = new System.Windows.Media.Animation.DoubleAnimation(100.0, duration);
            //progressBar1.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            counter++;
            //if (progressBar1.Value == progressBar1.Maximum)
            //{
            //    labelStatus.Content = "Background tsk is completed successfully.";
            //}

            //if (progressBar1.Value >= progressBar1.Maximum)
            //{
            //    CloseSplashScreen();
            //}

            if (counter == 3)
            {
                CloseSplashScreen();
            }
            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //CloseSplashScreen();
        }

        private void CloseSplashScreen()
        {
            if (timer != null)
            {
                timer.Stop();
            }
            this.Close();
        }
    }
}
