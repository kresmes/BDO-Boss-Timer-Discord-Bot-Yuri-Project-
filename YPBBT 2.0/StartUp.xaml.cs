using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace YPBBT_2._0
{
    /// <summary>
    /// Interaction logic for StartUp.xaml
    /// </summary>
    public partial class StartUp : Window
    {
        DiscordSocketClient discord = new DiscordSocketClient();
        List<string> LanguageLinesCollection = new List<string>();
        string defLang;
        int gate = 0;
        public StartUp()
        {
            var exists = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1;


            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                MessageBoxResult result = MessageBox.Show("The Application is already running in the background do you want to close it before continuing?", "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/ExtrenalAppRestart.exe");
                        const string processName = "YPBBT"; // without '.exe'
                        Process.GetProcesses()
                                    .Where(pr => pr.ProcessName == processName)
                                    .ToAsyncEnumerable()
                                    .ForEachAsync(p => p.Kill());
                        break;
                    case MessageBoxResult.No:

                        break;

                }
               
            }
            if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Resources/Profile"))
            {
                MainWindow ma = new MainWindow();
                ma.Show();
                this.Close();
            }
            InitializeComponent();
            
            mediaElement.Source = new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/bckg.mp4");
            mediaElement.Play();
            System.Threading.Thread.Sleep(2000);
            tab0.Visibility = Visibility.Hidden;
            tab1.Visibility = Visibility.Hidden;
            tab2.Visibility = Visibility.Hidden;
            tab3.Visibility = Visibility.Hidden;
            tab4.Visibility = Visibility.Hidden;
            tab5.Visibility = Visibility.Hidden;
            tab6.Visibility = Visibility.Hidden;
            tab7.Visibility = Visibility.Hidden;
            Step1_2MediaElement.Stop();
            Step2MediaElement.Stop();
            Step3MediaElement.Stop();
            Step4MediaElement.Stop();
            Tabcontrol1.SelectedIndex = 7;
        }

        private void ReadLanguage(string SL)
        {
            defLang = SL;
            try { LanguageLinesCollection.Clear(); } catch (Exception) { }
            string lines = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/Languages/" + SL);
            string[] RefferalsFilter = lines.Split('|');
            foreach (var rf in RefferalsFilter)
            {
                LanguageLinesCollection.Add(rf);
                //MessageBox.Show(rf);
            }
            S0Label0.Text = LanguageLinesCollection[0].ToString();
            Tc0SkipButton.Content = LanguageLinesCollection[1].ToString();
            Tc0NextButton.Content = LanguageLinesCollection[2].ToString();
            S1L0.Content = LanguageLinesCollection[3].ToString();
            S1L1.Text = LanguageLinesCollection[4].ToString();
            Step1_1Button.Content = LanguageLinesCollection[5].ToString();
            Step1_2Label.Text = LanguageLinesCollection[6].ToString();
            Step1_2Button.Content = LanguageLinesCollection[7].ToString();
            Step1NextButton.Content = LanguageLinesCollection[8].ToString();
            S2L0.Content = LanguageLinesCollection[9].ToString();
            S2L1.Text = LanguageLinesCollection[10].ToString();
            Step2HelpButton.Content = LanguageLinesCollection[11].ToString();
            Step2NextButton.Content = LanguageLinesCollection[12].ToString();
            S3L0.Content = LanguageLinesCollection[13].ToString();
            S3L1.Text = LanguageLinesCollection[14].ToString();
            Step3InviteBotButton.Content = LanguageLinesCollection[15].ToString();
            S3L2.Text = LanguageLinesCollection[16].ToString();
            Step3HelpButton.Content = LanguageLinesCollection[17].ToString();
            Step3NextButton.Content = LanguageLinesCollection[18].ToString();
            S4L0.Content = LanguageLinesCollection[19].ToString();
            S4L1.Text = LanguageLinesCollection[20].ToString();
            Step4TestTokenButton.Content = LanguageLinesCollection[21].ToString();
            S4L2.Text = LanguageLinesCollection[22].ToString();
            Step4HelpButton.Content = LanguageLinesCollection[23].ToString();
            Step4NextButton.Content = LanguageLinesCollection[24].ToString();
            S5L0.Content = LanguageLinesCollection[25].ToString();
            S5L1.Text = LanguageLinesCollection[26].ToString();
            S5L2.Text = LanguageLinesCollection[27].ToString();
            Step5NextButton.Content = LanguageLinesCollection[28].ToString();
            S6L0.Content = LanguageLinesCollection[29].ToString();
            S6L1.Text = LanguageLinesCollection[30].ToString();
            selctedlangImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/"+SL+".png"));
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 0;
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

      
        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = new TimeSpan(0, 0, 8);
            mediaElement.Play();
        }

        private void Tabcontrol1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Tabcontrol1.Opacity = 0;
            //DoubleAnimation da = new DoubleAnimation();
            //da.From = 1;
            //da.To = 0;
            //da.Duration = new Duration(TimeSpan.FromSeconds(1));
            //da.AutoReverse = true;
            //Tabcontrol1.BeginAnimation(OpacityProperty, da);
        }

        private void Tc0NextButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Tc0NextButton.Background = (Brush)bc.ConvertFrom("#818182");
        }

        private void Tc0NextButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Tc0NextButton.Background = (Brush)bc.ConvertFrom("#FFA9A9AA");
        }

        private void Tc0SkipButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Tc0SkipButton.Background = (Brush)bc.ConvertFrom("#8c2d2d");
        }

        private void Tc0SkipButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Tc0SkipButton.Background = (Brush)bc.ConvertFrom("#FFBB3D3D");
        }

        private void Tc0NextButton_Click(object sender, RoutedEventArgs e)
        {
            Step1_2Label.Visibility = Visibility.Hidden;
            Step1_2Button.Visibility = Visibility.Hidden;
            Step1_2MediaElement.Visibility = Visibility.Hidden;
            Step1NextButton.Visibility = Visibility.Hidden;
            Tabcontrol1.Opacity = 0;
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 1;
        }

        private void Step1_1Button_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step1_1Button.Background = (Brush)bc.ConvertFrom("#818182");
        }

        private void Step1_1Button_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step1_1Button.Background = (Brush)bc.ConvertFrom("#FFA9A9AA");
        }

        private void Step1_1Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discordapp.com/developers/applications/");
            Step1_2Label.Visibility = Visibility.Visible;
            Step1_2Button.Visibility = Visibility.Visible;
            Step1NextButton.Visibility = Visibility.Visible;
        }

        private void Step1_2MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Step1_2MediaElement.Position = new TimeSpan(0, 0, 0);
            Step1_2MediaElement.Play();
        }

        private void Step1_2Button_Click(object sender, RoutedEventArgs e)
        {
            Step1_2MediaElement.Visibility = Visibility.Visible;
            Step1_2MediaElement.Position = new TimeSpan(0, 0, 0);
            Step1_2MediaElement.Play();
        }

        private void Step1_2Button_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step1_2Button.Background = (Brush)bc.ConvertFrom("#818182");
        }

        private void Step1_2Button_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step1_2Button.Background = (Brush)bc.ConvertFrom("#FFA9A9AA");
        }

        private void Step1_2MediaElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/Step1.2.mp4");
        }

        private void Step1_2MediaElement_LostFocus(object sender, RoutedEventArgs e)
        {
            Step1_2MediaElement.Position = new TimeSpan(0, 0, 0);
            Step1_2MediaElement.Stop();
            Step1_2MediaElement.Visibility = Visibility.Hidden;          
        }

        private void Step1_2MediaElement_MouseLeave(object sender, MouseEventArgs e)
        {
            Step1_2MediaElement.Position = new TimeSpan(0, 0, 0);
            Step1_2MediaElement.Stop();
            Step1_2MediaElement.Visibility = Visibility.Hidden;
        }

        private void Step1NextButton_Click(object sender, RoutedEventArgs e)
        {
            Step1_2Label.Visibility = Visibility.Hidden;
            Step1_2Button.Visibility = Visibility.Hidden;
            Step1_2MediaElement.Visibility = Visibility.Hidden;
            Step2MediaElement.Visibility = Visibility.Hidden;
            Tabcontrol1.Opacity = 0;
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 2;
        }

        private void Step1NextButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step1NextButton.Background = (Brush)bc.ConvertFrom("#818182");
        }

        private void Step1NextButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step1NextButton.Background = (Brush)bc.ConvertFrom("#FFA9A9AA");
        }

        private void Step2CIDTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(Step3CIDTextbox.Text == "Client ID")
            { Step3CIDTextbox.Text = ""; }          
        }

        private void Step2MediaElement_LostFocus(object sender, RoutedEventArgs e)
        {
            Step2MediaElement.Position = new TimeSpan(0, 0, 0);
            Step2MediaElement.Stop();
            Step2MediaElement.Visibility = Visibility.Hidden;
        }

        private void Step2InviteBotButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step3InviteBotButton.Background = (Brush)bc.ConvertFrom("#818182");
        }

        private void Step2InviteBotButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step3InviteBotButton.Background = (Brush)bc.ConvertFrom("#FFA9A9AA");
        }

        private void Step2InviteBotButton_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            Step3CIDTextbox.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            Regex regex = new Regex("[^0-9-]+");
            string s = Step3CIDTextbox.Text.ToString();
            foreach (char c in s)
            {
                if (regex.IsMatch(c.ToString()) == true)
                { Step3CIDTextbox.Text = ""; }
            }          
            if (Step3CIDTextbox.Text == "Client ID")
            { Step3CIDTextbox.Text = ""; }
            if (Step3CIDTextbox.Text != "")
            {
                System.Diagnostics.Process.Start("https://discordapp.com/oauth2/authorize?&client_id="+Step3CIDTextbox.Text+"&scope=bot&permissions=0");
                Step3NextButton.Visibility = Visibility.Visible;
            }
            else
            {
                Step3CIDTextbox.BorderBrush = Brushes.Red;
            }
        }

        private void Step2MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Step2MediaElement.Position = new TimeSpan(0, 0, 0);
            Step2MediaElement.Play();
        }

        private void Step2MediaElement_MouseLeave(object sender, MouseEventArgs e)
        {
            Step2MediaElement.Position = new TimeSpan(0, 0, 0);
            Step2MediaElement.Stop();
            Step2MediaElement.Visibility = Visibility.Hidden;
        }

        private void Step2MediaElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/Step2MakeBot.mp4");
        }

        private void Step2HelpButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step2HelpButton.Background = (Brush)bc.ConvertFrom("#818182");
        }

        private void Step2HelpButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step2HelpButton.Background = (Brush)bc.ConvertFrom("#FFA9A9AA");
        }

        private void Step2HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Step2MediaElement.Visibility = Visibility.Visible;
        }

        private void Step2NextButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step2NextButton.Background = (Brush)bc.ConvertFrom("#818182");
        }

        private void Step2NextButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step2NextButton.Background = (Brush)bc.ConvertFrom("#FFA9A9AA");
        }

        private void Step2NextButton_Click(object sender, RoutedEventArgs e)
        {
            Step3MediaElement.Visibility = Visibility.Hidden;
            Step3NextButton.Visibility = Visibility.Hidden;
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 3;
        }

        private void Step3HelpButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step3HelpButton.Background = (Brush)bc.ConvertFrom("#818182");
        }

        private void Step3HelpButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step3HelpButton.Background = (Brush)bc.ConvertFrom("#FFA9A9AA");
        }

        private void Step3HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Step3MediaElement.Visibility = Visibility.Visible;
        }

        private void Step3MediaElement_LostFocus(object sender, RoutedEventArgs e)
        {
            Step3MediaElement.Position = new TimeSpan(0, 0, 0);
            Step3MediaElement.Stop();
            Step3MediaElement.Visibility = Visibility.Hidden;
        }

        private void Step3MediaElement_MouseLeave(object sender, MouseEventArgs e)
        {
            Step3MediaElement.Position = new TimeSpan(0, 0, 0);
            Step3MediaElement.Stop();
            Step3MediaElement.Visibility = Visibility.Hidden;
        }

        private void Step3MediaElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/Step2CID.mp4");
        }

        private void Step3MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Step3MediaElement.Position = new TimeSpan(0, 0, 0);
            Step3MediaElement.Play();
        }

        private void Step3NextButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step3NextButton.Background = (Brush)bc.ConvertFrom("#818182");
        }

        private void Step3NextButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step3NextButton.Background = (Brush)bc.ConvertFrom("#FFA9A9AA");
        }

        private void Step3NextButton_Click(object sender, RoutedEventArgs e)
        {
            Step4MediaElement.Visibility = Visibility.Hidden;
            Step4NextButton.Visibility = Visibility.Hidden;
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 4;
        }

        private void Step4TokenTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(Step4TokenTextbox.Text == "Token")
            { Step4TokenTextbox.Text = ""; }
        }

        private void Step4TestTokenButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step4TestTokenButton.Background = (Brush)bc.ConvertFrom("#818182");
        }

        private void Step4TestTokenButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step4TestTokenButton.Background = (Brush)bc.ConvertFrom("#FFA9A9AA");
        }

        private async void Step4TestTokenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              await discord.LoginAsync(TokenType.Bot, Step4TokenTextbox.Text);
              await discord.StartAsync();
                Step4TokenTextbox.BorderBrush = Brushes.Green;
                Step4TestTokenButton.Background = Brushes.Green;
            }
            catch (Exception) { 
                Step4TokenTextbox.BorderBrush = Brushes.Red;
                Step4TestTokenButton.Background = Brushes.Red;
            }

            if(Step4TokenTextbox.BorderBrush == Brushes.Green)
            { Step4NextButton.Visibility = Visibility.Visible; }
        }

        private void Step4HelpButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step4HelpButton.Background = (Brush)bc.ConvertFrom("#818182");
        }

        private void Step4HelpButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step4HelpButton.Background = (Brush)bc.ConvertFrom("#FFA9A9AA");
        }

        private void Step4HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Step4MediaElement.Visibility = Visibility.Visible;
        }

        private void Step4MediaElement_LostFocus(object sender, RoutedEventArgs e)
        {
            Step4MediaElement.Position = new TimeSpan(0, 0, 0);
            Step4MediaElement.Stop();
            Step4MediaElement.Visibility = Visibility.Hidden;
        }

        private void Step4MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Step4MediaElement.Position = new TimeSpan(0, 0, 0);
            Step4MediaElement.Play();
        }

        private void Step4MediaElement_MouseLeave(object sender, MouseEventArgs e)
        {
            Step4MediaElement.Position = new TimeSpan(0, 0, 0);
            Step4MediaElement.Stop();
            Step4MediaElement.Visibility = Visibility.Hidden;
        }

        private void Step4MediaElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/Step4GetToken.mp4");
        }

        private void Step4NextButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step4NextButton.Background = (Brush)bc.ConvertFrom("#818182");
        }

        private void Step4NextButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step4NextButton.Background = (Brush)bc.ConvertFrom("#FFA9A9AA");
        }

        private void Step4NextButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 5;
        }

        private void Tc0SkipButton_Click(object sender, RoutedEventArgs e)
        {
            Step3MediaElement.Visibility = Visibility.Hidden;
            Step3NextButton.Visibility = Visibility.Hidden;
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 3;
        }

        private void HelpSID_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/ServerID.mp4");
        }

        private void HelpCID_Click(object sender, RoutedEventArgs e)
        {
          System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/ChannelID.mp4");
        }

        private void Step5SIDTExtbox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(Step5SIDTExtbox.Text == "Server ID")
            { Step5SIDTExtbox.Text = ""; }
        }

        private void Step5CIDTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(Step5CIDTextbox.Text == "Channel ID")
            { Step5CIDTextbox.Text = ""; }
        }

        private void Step5NextButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step5NextButton.Background = (Brush)bc.ConvertFrom("#818182");
        }

        private void Step5NextButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Step5NextButton.Background = (Brush)bc.ConvertFrom("#FFA9A9AA");
        }

        private void Step5NextButton_Click(object sender, RoutedEventArgs e)
        {
            int g = 0;
            if (Step5SIDTExtbox.Text != "Server ID" && Step5CIDTextbox.Text != "Channel ID")
            { g = 1; }
            Regex regex = new Regex("[^0-9-]+");
            string s = Step5SIDTExtbox.Text.ToString();
            foreach (char c in s)
            {
                if (regex.IsMatch(c.ToString()) == true)
                { Step5SIDTExtbox.Text = ""; }
            }
            string s1 = Step5CIDTextbox.Text.ToString();
            foreach (char c in s1)
            {
                if (regex.IsMatch(c.ToString()) == true)
                { Step5CIDTextbox.Text = ""; }
            }
            if (Step5SIDTExtbox.Text != "" && Step5CIDTextbox.Text != "" && g == 1)
          {
                if(Step5RoleTextbox.Text == "")
                { Step5RoleTextbox.Text = "<@RoleID>"; }
                string data = defLang+"|"+ Step3CIDTextbox.Text+"|"+ Step4TokenTextbox.Text+"|"+ Step5SIDTExtbox.Text +"|"+ Step5CIDTextbox.Text+"|"+ Step5RoleTextbox.Text+"|<@RoleID>" + "|<@RoleID>" +"|5|30";                
                System.IO.File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/Profile", data);

                DoubleAnimation da = new DoubleAnimation();
                da.From = 0;
                da.To = 1;
                da.Duration = new Duration(TimeSpan.FromSeconds(2));
                da.AutoReverse = false;
                Tabcontrol1.BeginAnimation(OpacityProperty, da);
                Tabcontrol1.SelectedIndex = 6;
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/8SCcCJq");
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            gate = 1;
            this.Close();
        }

        private void frButton_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            SetLanguageLabel.BeginAnimation(OpacityProperty, da);
            SetLanguageLabel.Content = "Choisir la langue";
        }

        private void enButton_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            SetLanguageLabel.BeginAnimation(OpacityProperty, da);
            SetLanguageLabel.Content = "Select Language";
        }

        private void esButton_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            SetLanguageLabel.BeginAnimation(OpacityProperty, da);
            SetLanguageLabel.Content = "Seleccione el idioma";
        }

        private void ruButton_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            SetLanguageLabel.BeginAnimation(OpacityProperty, da);
            SetLanguageLabel.Content = "выбрать язык";
        }

        private void jpButton_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            SetLanguageLabel.BeginAnimation(OpacityProperty, da);
            SetLanguageLabel.Content = "言語を選択する";
        }

        private void krButton_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            SetLanguageLabel.BeginAnimation(OpacityProperty, da);
            SetLanguageLabel.Content = "언어를 선택하십시오";
        }

        private void frButton_Click(object sender, RoutedEventArgs e)
        {
            ReadLanguage("fr");
            
        }

        private void enButton_Click(object sender, RoutedEventArgs e)
        {
            ReadLanguage("en");
        }

        private void esButton_Click(object sender, RoutedEventArgs e)
        {
            ReadLanguage("es");
        }

        private void ruButton_Click(object sender, RoutedEventArgs e)
        {
            ReadLanguage("ru");
        }

        private void jpButton_Click(object sender, RoutedEventArgs e)
        {
            ReadLanguage("jp");
        }

        private void krButton_Click(object sender, RoutedEventArgs e)
        {
            ReadLanguage("kr");
        }

        private void SelectedlangButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 7;
        }

        private void HelpRID_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/ConvertRole.mp4");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(1));
            anim.Completed += (s, _) => Excute();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }
        private async void Excute()
        {
            if (gate == 1)
            {
                await discord.StopAsync();
                MainWindow ma = new MainWindow();
                ma.Show();
            }
            this.Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
