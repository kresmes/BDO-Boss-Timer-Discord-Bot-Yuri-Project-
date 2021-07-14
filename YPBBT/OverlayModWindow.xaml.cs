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
    /// Interaction logic for OverlayModWindow.xaml
    /// </summary>
    public partial class OverlayModWindow : Window
    {
        double io;
        public OverlayModWindow()
        {
            InitializeComponent();
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
           
            if (Settings.Default["OverlayTransparency"].ToString() == "")
            {
                io= 1;
            }
            else
            { io = double.Parse(Settings.Default["OverlayTransparency"].ToString());  }
            //MessageBox.Show(this.Opacity.ToString());
            //MessageBox.Show(io.ToString());
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
        public void load()
        {
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
            if (Settings.Default["OverlayTransparency"].ToString() == "")
            {
                io = 1;
            }
            else
            { io = double.Parse(Settings.Default["OverlayTransparency"].ToString()); }
            if(io == 0)
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
                    this.DragMove();              
            }
            catch (Exception) { }
        }
        public void UpdateData(string buttontext, BitmapImage img, string label1,string bossname,string bosstime,string label2,string nighttime,string label3,string IrTime,string label4,string bossSpawnCB,string NightTimecb,string ImperialResetCB)
        {
            exitButton.Content = buttontext;
            BossImage.Source = img;
            Label1.Content = label1;
            BossNameLabel.Content = bossname;
            BossTimeLabel.Content = bosstime;
            Label2.Content = label2;
            NightInBdoTimeLabel.Content = nighttime;
            Label3.Content = label3;
            IRTimeLabel.Content = IrTime;
            Label4.Content = label4;
            SoundOptionCheckBox.Content = bossSpawnCB;
            NTSoundOptionCheckBox.Content = NightTimecb;
            IRSoundOptionCheckBox.Content = ImperialResetCB;
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow nw = new MainWindow(1);
            nw.GetOverlayState(0);
            this.Visibility = Visibility.Hidden;
            
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
    }
}
