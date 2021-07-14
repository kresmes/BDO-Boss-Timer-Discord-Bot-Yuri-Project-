using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YPBBT.Properties;

namespace YPBBT
{
    /// <summary>
    /// Interaction logic for DA_OverlayModWindow.xaml
    /// </summary>
    public partial class DA_OverlayModWindow : Window
    {       
        DA_MainWindow damw;
        int expanded = 0;
        double io;
        public DA_OverlayModWindow(DA_MainWindow mw)
        {
            damw = mw;
            InitializeComponent();
            WindowOv.Top = (System.Windows.SystemParameters.PrimaryScreenHeight / 2) - 60;
            if (Settings.Default["PlaySoundSetting"].ToString() == "")
            { SoundOptionCheckBox.IsChecked = false; }
            if (Settings.Default["PlaySoundSetting"].ToString() == "1")
            { SoundOptionCheckBox.IsChecked = true; }
            if (Settings.Default["PlaySoundSetting"].ToString() == "0")
            { SoundOptionCheckBox.IsChecked = false; }

            if (Settings.Default["NTPlaySoundSetting"].ToString() == "")
            { NTSoundOptionCheckBox.IsChecked = false; }
            if (Settings.Default["NTPlaySoundSetting"].ToString() == "1")
            { NTSoundOptionCheckBox.IsChecked = true; }
            if (Settings.Default["NTPlaySoundSetting"].ToString() == "0")
            { NTSoundOptionCheckBox.IsChecked = false; }

            if (Settings.Default["IRPlaySoundSetting"].ToString() == "")
            { IRSoundOptionCheckBox.IsChecked = false; }
            if (Settings.Default["IRPlaySoundSetting"].ToString() == "1")
            { IRSoundOptionCheckBox.IsChecked = true; }
            if (Settings.Default["IRPlaySoundSetting"].ToString() == "0")
            { IRSoundOptionCheckBox.IsChecked = false; }

            if (Settings.Default["BRPlaySoundSetting"].ToString() == "")
            { BRSoundOptionCheckBox.IsChecked = false; }
            if (Settings.Default["BRPlaySoundSetting"].ToString() == "1")
            { BRSoundOptionCheckBox.IsChecked = true; }
            if (Settings.Default["BRPlaySoundSetting"].ToString() == "0")
            { BRSoundOptionCheckBox.IsChecked = false; }

            if (Settings.Default["ITRPlaySoundSetting"].ToString() == "")
            { ITRSoundOptionCheckBox.IsChecked = false; }
            if (Settings.Default["ITRPlaySoundSetting"].ToString() == "1")
            { ITRSoundOptionCheckBox.IsChecked = true; }
            if (Settings.Default["ITRPlaySoundSetting"].ToString() == "0")
            { ITRSoundOptionCheckBox.IsChecked = false; }



            if (Settings.Default["OverlayTransparency"].ToString() == "")
            {io = 1;}
            else
            {io = double.Parse(Settings.Default["OverlayTransparency"].ToString());}
            if (io == 0)
            { io = 0.1; }
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = io;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            this.BeginAnimation(OpacityProperty, da);
            Settings.Default.Save();

            WindowOv.Left = 0;
            WindowOv.Top = 0;
            this.ShowInTaskbar = false;
            WindowOv.Topmost = true;
        }
        public void load(BitmapImage img)
        {
            try 
            {
               if(Settings.Default["Overlay_Position"].ToString() != "")
               {
                  string[] ps = Settings.Default["Overlay_Position"].ToString().Split('|');
                    WindowOv.Top = double.Parse(ps[0]);
                    WindowOv.Left = double.Parse(ps[1]);
               }
               else
               { WindowOv.Top = (System.Windows.SystemParameters.PrimaryScreenHeight / 2) - 60; }
            }
            catch (Exception) { WindowOv.Top = (System.Windows.SystemParameters.PrimaryScreenHeight / 2) - 60; }
           
            BossImage.Source = img;
            if (Settings.Default["PlaySoundSetting"].ToString() == "")
            { SoundOptionCheckBox.IsChecked = false; }
            if (Settings.Default["PlaySoundSetting"].ToString() == "1")
            { SoundOptionCheckBox.IsChecked = true; } 
            if (Settings.Default["PlaySoundSetting"].ToString() == "0")
            { SoundOptionCheckBox.IsChecked = false; }

            if (Settings.Default["NTPlaySoundSetting"].ToString() == "")
            { NTSoundOptionCheckBox.IsChecked = false; }
            if (Settings.Default["NTPlaySoundSetting"].ToString() == "1")
            { NTSoundOptionCheckBox.IsChecked = true; }
            if (Settings.Default["NTPlaySoundSetting"].ToString() == "0")
            { NTSoundOptionCheckBox.IsChecked = false; }

            if (Settings.Default["IRPlaySoundSetting"].ToString() == "")
            { IRSoundOptionCheckBox.IsChecked = false; }
            if (Settings.Default["IRPlaySoundSetting"].ToString() == "1")
            { IRSoundOptionCheckBox.IsChecked = true; }
            if (Settings.Default["IRPlaySoundSetting"].ToString() == "0")
            { IRSoundOptionCheckBox.IsChecked = false; }

            if (Settings.Default["BRPlaySoundSetting"].ToString() == "")
            { BRSoundOptionCheckBox.IsChecked = false; }
            if (Settings.Default["BRPlaySoundSetting"].ToString() == "1")
            { BRSoundOptionCheckBox.IsChecked = true; }
            if (Settings.Default["BRPlaySoundSetting"].ToString() == "0")
            { BRSoundOptionCheckBox.IsChecked = false; }

            if (Settings.Default["ITRPlaySoundSetting"].ToString() == "")
            { ITRSoundOptionCheckBox.IsChecked = false; }
            if (Settings.Default["ITRPlaySoundSetting"].ToString() == "1")
            { ITRSoundOptionCheckBox.IsChecked = true; }
            if (Settings.Default["ITRPlaySoundSetting"].ToString() == "0")
            { ITRSoundOptionCheckBox.IsChecked = false; }


            if (Settings.Default["OverlayTransparency"].ToString() == "")
            {
                io = 1;
            }
            else
            { io = double.Parse(Settings.Default["OverlayTransparency"].ToString()); }
            if (io == 0)
            { io = 0.1; }
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = io;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            this.BeginAnimation(OpacityProperty, da);

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                { this.DragMove(); }
                Settings.Default["Overlay_Position"] = WindowOv.Top.ToString() + "|" + WindowOv.Left.ToString();
                Settings.Default.Save();
            }
            catch (Exception) { }
        }
        public void UpdateData(string brtext, BitmapImage img, string label1, string bossname, string bosstime, string label2, string nighttime, string label3, string IrTime, string BrTime, string bossSpawnCB, string NightTimecb, string ImperialResetCB,string BRCB,string ITRCB,string PSOL, string ITR,string ITRT)
        {
            BRLabel.Content = brtext;
            BossImage.Source = img;
            Label1.Content = label1;
            BossNameLabel.Content = bossname;
            BossTimeLabel.Content = bosstime;
            Label2.Content = label2;
            NightInBdoTimeLabel.Content = nighttime;
            Label3.Content = label3;
            IRTimeLabel.Content = IrTime;
            BrTimeLabel.Content = BrTime;
            SoundOptionCheckBox.Content = bossSpawnCB;
            NTSoundOptionCheckBox.Content = NightTimecb;
            IRSoundOptionCheckBox.Content = ImperialResetCB;
            BRSoundOptionCheckBox.Content = BRCB;
            ITRSoundOptionCheckBox.Content = ITRCB;
            PlaySoundOnLabel.Content = PSOL;
            ITRL.Content = ITR;
            ITRTimeLabel.Content = ITRT;
            if (expanded == 0)
            { BossImage.ToolTip = Label1.Content.ToString() + Environment.NewLine + BossNameLabel.Content.ToString() + Environment.NewLine + BossTimeLabel.Content.ToString(); }
            else
            { BossImage.ToolTip = null; }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            damw.GetOverlayState();
            damw.Show();
            this.Close();
        }

        private void SoundOptionCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (SoundOptionCheckBox.IsChecked == true)
            {
                Settings.Default["PlaySoundSetting"] = "1";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["PlaySoundSetting"] = "0";
                Settings.Default.Save();
            }
        }

        private void NTSoundOptionCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (NTSoundOptionCheckBox.IsChecked == true)
            {
                Settings.Default["NTPlaySoundSetting"] = "1";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["NTPlaySoundSetting"] = "0";
                Settings.Default.Save();
            }
        }

        private void IRSoundOptionCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (IRSoundOptionCheckBox.IsChecked == true)
            {
                Settings.Default["IRPlaySoundSetting"] = "1";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["IRPlaySoundSetting"] = "0";
                Settings.Default.Save();
            }
        }     
        private void BossImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ExpandMoreButton.Content.ToString() == "-")
            {
                ExpandMoreButton.Content = "+";
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = WindowOv.ActualHeight;
                daH.To = WindowOv.ActualHeight - 185;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.3));
                WindowOv.BeginAnimation(Ellipse.HeightProperty, daH);
            }
            else
            {
                if (expanded == 0)
                {
                    expanded = 1;
                    DoubleAnimation daW = new DoubleAnimation();
                    daW.From = WindowOv.ActualWidth;
                    daW.To = WindowOv.ActualWidth + 227.333;
                    daW.Duration = new Duration(TimeSpan.FromSeconds(0.3));
                    WindowOv.BeginAnimation(Ellipse.WidthProperty, daW);
                }
                else
                {
                    expanded = 0;
                    DoubleAnimation daW = new DoubleAnimation();
                    daW.From = WindowOv.ActualWidth;
                    daW.To = WindowOv.ActualWidth - 227.333;
                    daW.Duration = new Duration(TimeSpan.FromSeconds(0.3));
                    WindowOv.BeginAnimation(Ellipse.WidthProperty, daW);
                }
            }
        }

        private void ExpandMoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExpandMoreButton.Content.ToString() == "+")
            {
                ExpandMoreButton.Content = "-";
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = WindowOv.ActualHeight;
                daH.To = WindowOv.ActualHeight + 185;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.3));
                WindowOv.BeginAnimation(Ellipse.HeightProperty, daH);
            }
            else
            {
                ExpandMoreButton.Content = "+";
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = WindowOv.ActualHeight;
                daH.To = WindowOv.ActualHeight - 185;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.3));
                WindowOv.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void BRSoundOptionCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (BRSoundOptionCheckBox.IsChecked == true)
            {
                Settings.Default["BRPlaySoundSetting"] = "1";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["BRPlaySoundSetting"] = "0";
                Settings.Default.Save();
            }
        }

        private void ITRSoundOptionCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (ITRSoundOptionCheckBox.IsChecked == true)
            {
                Settings.Default["ITRPlaySoundSetting"] = "1";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["ITRPlaySoundSetting"] = "0";
                Settings.Default.Save();
            }
        }
    }
}
