using CefSharp;
using CefSharp.Wpf;
using Discord;
using Discord.WebSocket;
using Dsafa.WpfColorPicker;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using YPBBT.Properties;
using Color = Discord.Color;
//using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using YPBBT_DLL;


namespace YPBBT
{
    /// <summary>
    /// Interaction logic for DA_MainWindow.xaml
    /// </summary>
    ///  private System.Windows.Forms.NotifyIcon MyNotifyIcon;    
    public partial class DA_MainWindow : Window
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private const int GWL_EX_STYLE = -20;
        private const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;

        public DA_MainWindow() => InitializeComponent();

        private System.Windows.Forms.NotifyIcon MyNotifyIcon;
        public DiscordSocketClient discord = new DiscordSocketClient();
        public List<string> ProfileCollection = new List<string>();
        public List<string> LanguageCollection = new List<string>();
        List<string> BossesCollection = new List<string>();
        List<string> Origin_BossesCollection = new List<string>();
        List<string> ServersCollection = new List<string>();
        List<string> ChannelsCollection = new List<string>();
        List<string> AlertChannelsCollection = new List<string>();
        List<string> RolesCollection = new List<string>();           
        public DataTable TimeTable = new DataTable();
        int MainB;
        int PmaxC;
        DateTime NBT;
        DateTime CBT;
        DateTime PBT;
        string DefaultLanguage;
        ulong ClientID;
        public string Token;
        public ulong ServerID;
        public ulong Main_BotChannel_ID;
        string BossSpawnRole;
        string NightTimeRole;
        string ImperialResetRole;
        string BarteringResetRole;
        string ImperialTradingResetRole;
        int UpdateMesssageInterval;
        int SharedDay;
        int SharedTime;
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timer1 = new DispatcherTimer();
        public string MOTR;
        int startupRS;
        int overlayState = 0;
        DA_OverlayModWindow omw;
        public ulong MainMessageID;
        int SaveLatestTimeTable = 0;
        public ulong TimtableID = 0;
        int intervalMessageUpdate = 0;
        public string publicNbossimage;
        public string publicbossUrl;
        public ulong bossImageID;
        public string AppVersion = "3.13b";
        public string CurrentVersion = "";
        string currentbossrole1 = "";
        string currentbossrole2 = "";
        double lastSliderValue;
        int lastSelectedSource;
        public Discord.Rest.RestUserMessage MainMessage;
        int gridview1Row = 0;
        int gridview1Column = 0;
        public int isposting = 0;
        int botuptime = 0;
        string BossesListData = "";
        string subcolor = "";
        string maincolor = "";
        string last_HM_Set = "";
        int language_changed = -1;
        string ScarletMode_Message = "";
        string Updated_ScarletMode_Message = "";
        int Selectected_boss = -1;
        public ulong Alert_BotChannel_ID = 0;
        List<int> ScarletMode_CurrentBossROWxColumn = new List<int> { 0, 0 };
        List<string> SelfRollingRolesCollections = new List<string>();
        string SelfRollingSettings = null;
        string SelfRolling_LoadedReaction = null;
        Discord.Rest.RestUserMessage SelfRolling_MainMessage;
        bool SelfRolling_WaitingReaction = false;
        Discord.Rest.RestUserMessage SelfRolling_GeTReactionMessage = null;
        List<string> SelfRollingMessageRolesCollections = new List<string>();
        int SelfRolling_Messages_ListBox_SelectedIndex = -1;
        public DA_MainWindow(int OL)
        {
            omw = new DA_OverlayModWindow(this);
            if (OL == 0)
            {
                try
                {
                    if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/debug.log"))
                    { File.Delete(System.IO.Directory.GetCurrentDirectory() + "/debug.log"); }
                }
                catch (Exception) { }
            }                     
            InitializeComponent();
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

            if (Settings.Default["MainColor"].ToString() == "")
            {
                Settings.Default["MainColor"] = "#A028282B";
                Settings.Default.Save();
                maincolor = "#A028282B";
            }
            else
            { maincolor = Settings.Default["MainColor"].ToString();}
            if (Settings.Default["SubColor"].ToString() == "")
            {
                Settings.Default["SubColor"] = "#FF8B81FC";
                Settings.Default.Save();
                subcolor = "#FF8B81FC";
            }
            else
            { subcolor = Settings.Default["SubColor"].ToString();}
           
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString(maincolor);//Default #FF28282B
            Application.Current.Resources["MainColor"] = brush;
            brush = (Brush)converter.ConvertFromString(subcolor);//Default #FF8B81FC
            Application.Current.Resources["SubColor"] = brush;
            if (Settings.Default["HeaderColor"].ToString() == "")
            {
                Settings.Default["HeaderColor"] = "#962C2D32";
                Settings.Default.Save();
                converter = new System.Windows.Media.BrushConverter();
                brush = (Brush)converter.ConvertFromString("#962C2D32");//Default #FF28282B
                Application.Current.Resources["HeaderColor"] = brush;
            }
            else
            {
                converter = new System.Windows.Media.BrushConverter();
                brush = (Brush)converter.ConvertFromString(Settings.Default["HeaderColor"].ToString());//Default #FF28282B
                Application.Current.Resources["HeaderColor"] = brush;
            }
            AddSaveBossNameTextBox.IsEnabled = false;
            string fversion = AppVersion;
            try { fversion = fversion.Substring(0, fversion.IndexOf(".") + 2); } catch (Exception) { }
            mainWindow.Title = "YPBBT " + fversion;
            VersionLogoLabel.Content = fversion;

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                AutoUpdate AU = new AutoUpdate();
                AU.Check_For_Update(this);
            }).Start();

            SelfRollingSettings = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/SelfRolling");
            foreach(var RM in GetStrBetweenTags(SelfRollingSettings, "[MessageRoles]", "[/MessageRoles]").Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
            {
                if (RM != "")
                { SelfRollingMessageRolesCollections.Add(RM); }
            }
            string Origin_BossesList = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/BossesOrigin");
            string[] Origin_BossListSP = Origin_BossesList.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (string boss in Origin_BossListSP)
            { Origin_BossesCollection.Add(boss); }

            string finalBosslist = "";
           
            if (!File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Bosses"))
            {
              string OriginBossesList = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/BossesOrigin");
                string[] BossListSplited = OriginBossesList.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                int line = 0;
                foreach (string bossdata in BossListSplited)
                {
                    if (line == 0)
                    { finalBosslist += bossdata; }
                    else
                    { finalBosslist += Environment.NewLine + bossdata; }
                    line++;
                }
                File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Bosses", finalBosslist);
                BossesListData = finalBosslist;
            }
            if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Bosses") && Settings.Default["OriginBossList"].ToString() != File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/BossesOrigin"))
            {
                string OriginBossesList = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/BossesOrigin");
                Settings.Default["OriginBossList"] = OriginBossesList;
                Settings.Default.Save();
                string[] BossListSplited = OriginBossesList.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                string BossesList = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Bosses");
                string[] BossListSP = BossesList.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                finalBosslist = BossesList;
                foreach (string BossData in BossListSP)
                {
                    if (BossData != "")
                    {
                        foreach (string BD in BossListSplited)
                        {
                            if (BD != "")
                            {
                                string[] BDS = BD.Split(',');
                                string[] bossdataS = BossData.Split(',');
                                if (bossdataS[0] == BDS[0])
                                {
                                    if (bossdataS.Length == 5)
                                    { finalBosslist = finalBosslist.Replace(bossdataS[0] + "," + bossdataS[1] + "," + bossdataS[2] + "," + bossdataS[3] + "," + bossdataS[4], BDS[0] + "," + BDS[1] + "," + BDS[2] + "," + bossdataS[3] + "," + bossdataS[4] + "," + BDS[5]); }
                                    if (bossdataS.Length == 6)
                                    { finalBosslist = finalBosslist.Replace(bossdataS[0] + "," + bossdataS[1] + "," + bossdataS[2] + "," + bossdataS[3] + "," + bossdataS[4] + "," + bossdataS[5], BDS[0] + "," + BDS[1] + "," + BDS[2] + "," + bossdataS[3] + "," + bossdataS[4] + "," + BDS[5]); }
                                }
                            }
                        }
                    }
                }
                foreach (string BD in BossListSplited)
                {
                    string[] BDS = BD.Split(',');
                    if (!BossesList.Contains(BDS[0] + ","))
                    { finalBosslist += Environment.NewLine + BD; }
                }
                File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Bosses", finalBosslist);
                BossesListData = finalBosslist;
            }
           if(!File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT"))
           {
                string Origin_LYPBBTTT_Data = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT_Origin");
                File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT", Origin_LYPBBTTT_Data);
           }
            Tab0.Visibility = Visibility.Hidden;
            tab1.Visibility = Visibility.Hidden;
            tab2.Visibility = Visibility.Hidden;
            tab3.Visibility = Visibility.Hidden;
            tab4.Visibility = Visibility.Hidden;
            tab5.Visibility = Visibility.Hidden;
            atc0.Visibility = Visibility.Hidden;
            atc1.Visibility = Visibility.Hidden;
            atc2.Visibility = Visibility.Hidden;
            atc3.Visibility = Visibility.Hidden;
            atc4.Visibility = Visibility.Hidden;
            estc0.Visibility = Visibility.Hidden;
            estc1.Visibility = Visibility.Hidden;
            estc2.Visibility = Visibility.Hidden;
            TimeTableGrid.Visibility = Visibility.Hidden;
            //RemoveBossButton.Visibility = Visibility.Hidden;
            Settings.Default["OverlayState"] = "0";
            Settings.Default.Save();

            if(Settings.Default["AnimatedBackgroundSource"].ToString() == "")
            {
                Settings.Default["AnimatedBackgroundSource"] = System.IO.Directory.GetCurrentDirectory() + @"\Resources\img\bckg.mp4";
                Settings.Default.Save();
            }

            if (Settings.Default["AnimatedBackgroundCheckbox"].ToString() == "")
            { AnimatedBackgroundCheckBox.IsChecked = true; }
            if (Settings.Default["AnimatedBackgroundCheckbox"].ToString() == "1")
            { AnimatedBackgroundCheckBox.IsChecked = true; }
            if (Settings.Default["AnimatedBackgroundCheckbox"].ToString() == "0")
            { AnimatedBackgroundCheckBox.IsChecked = false; }

            if(AnimatedBackgroundCheckBox.IsChecked == true)
            {
                mediaElement.IsEnabled = true;
                var bc = new BrushConverter();
                PickAnimatedBackground.Foreground = (Brush)bc.ConvertFrom("#FFC6C6C9");
                PickAnimatedBackground.IsEnabled = true;
                mediaElement.Visibility = Visibility.Visible;
                mediaElement.LoadedBehavior = MediaState.Play;
                mediaElement.Source = new Uri(Settings.Default["AnimatedBackgroundSource"].ToString(), UriKind.Absolute);
            }
            else
            {
                mediaElement.IsEnabled = false;
                var bc = new BrushConverter();
                PickAnimatedBackground.Foreground = (Brush)bc.ConvertFrom("#FF72727A");
                PickAnimatedBackground.IsEnabled = false;
                mediaElement.Visibility = Visibility.Hidden;               
            }

            if (Settings.Default["BackgroundImageSource"].ToString() == "")
            {
                Settings.Default["BackgroundImageSource"] = System.IO.Directory.GetCurrentDirectory() + "/Resources/img/bckg.png";
                Settings.Default.Save();
            }

            if (Settings.Default["BackgroundImageCheckbox"].ToString() == "")
            { BackgroundImageCheckbox.IsChecked = true; }
            if (Settings.Default["BackgroundImageCheckbox"].ToString() == "1")
            { BackgroundImageCheckbox.IsChecked = true; }
            if (Settings.Default["BackgroundImageCheckbox"].ToString() == "0")
            { BackgroundImageCheckbox.IsChecked = false; }

            if (BackgroundImageCheckbox.IsChecked == true)
            {
                backgImageBox.IsEnabled = true;
                var bc = new BrushConverter();
                PickBackgroundImage.Foreground = (Brush)bc.ConvertFrom("#FFC6C6C9");
                PickBackgroundImage.IsEnabled = true;
                backgImageBox.Visibility = Visibility.Visible;
                try { backgImageBox.Source = new BitmapImage(new Uri(Settings.Default["BackgroundImageSource"].ToString())); }
                catch (Exception) {
                    Settings.Default["BackgroundImageSource"] = System.IO.Directory.GetCurrentDirectory() + "/Resources/img/bckg.png";
                    Settings.Default.Save();
                    backgImageBox.Source = new BitmapImage(new Uri(Settings.Default["BackgroundImageSource"].ToString()));
                }
            }
            else
            {
                backgImageBox.IsEnabled = false;
                var bc = new BrushConverter();
                PickBackgroundImage.Foreground = (Brush)bc.ConvertFrom("#FF72727A");
                PickBackgroundImage.IsEnabled = true;
                backgImageBox.Visibility = Visibility.Hidden;
            }

            

            MyNotifyIcon = new System.Windows.Forms.NotifyIcon();
            MyNotifyIcon.Icon = new System.Drawing.Icon(
                            System.IO.Directory.GetCurrentDirectory() + "/Resources/img/icon.ico");
            MyNotifyIcon.MouseDown +=
                new System.Windows.Forms.MouseEventHandler
                    (MyNotifyIcon_MouseDoubleClick);
            
            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Tick += timer1_Tick;           

            int lastSS = 0;
            try { lastSS = int.Parse(Settings.Default["SelectedSource"].ToString()); } catch (Exception) { }
            lastSelectedSource = lastSS;
            SourceComboBox.Items.Add("LYPBBT");
            SourceComboBox.Items.Add("MMOTIMER");
            //SourceComboBox.Items.Add("BdoBossTimer");
            SourceComboBox.SelectedIndex = lastSS;

            BotHostComboBox.Items.Add("Local");
            BotHostComboBox.Items.Add("polisystems");
            BotHostComboBox.SelectedIndex = 0;
            BotHostComboBox.IsEnabled = false;

            LanguageDropBox.Items.Add("Français");
            LanguageDropBox.Items.Add("English");
            LanguageDropBox.Items.Add("Español");
            LanguageDropBox.Items.Add("русский");
            LanguageDropBox.Items.Add("日本人");
            LanguageDropBox.Items.Add("한국어");

            try { ProfileCollection.Clear(); } catch (Exception) { }
            foreach (var pc in File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Profile").Split('|'))
            { ProfileCollection.Add(pc); }
            DefaultLanguage = ProfileCollection[0].ToString();
            ClientID = ulong.Parse(ProfileCollection[1].ToString());
            Token = ProfileCollection[2].ToString();

            ServerID = ulong.Parse(ProfileCollection[3].ToString().Split('{')[1].ToString());

            Main_BotChannel_ID = ulong.Parse(ProfileCollection[4].ToString().Split('{')[1]);
            Alert_BotChannel_ID = ulong.Parse(ProfileCollection[12].ToString().Split('{')[1]);

            BossSpawnRole = ProfileCollection[5].ToString().Split('{')[1].ToString();
            if (BossSpawnRole != "")
            {
                if (BossSpawnRole != "@everyone" && BossSpawnRole != "@here")
                { BossSpawnRole = "<@&" + BossSpawnRole + ">"; }
            }
            string[] ntr = ProfileCollection[6].ToString().Split('{');
            NightTimeRole = ntr[1].ToString();
            if (NightTimeRole != "")
            {
                if (NightTimeRole != "@everyone" && NightTimeRole != "@here")
                { NightTimeRole = "<@&" + NightTimeRole + ">"; }
            }
            string[] irr = ProfileCollection[7].ToString().Split('{');
            ImperialResetRole = irr[1].ToString();
            if (ImperialResetRole != "")
            {
                if (ImperialResetRole != "@everyone" && ImperialResetRole != "@here")
                { ImperialResetRole = "<@&" + ImperialResetRole + ">"; }
            }
            string[] brr = ProfileCollection[10].ToString().Split('{');
            BarteringResetRole = brr[1].ToString();
            if (BarteringResetRole != "")
            {
                if (BarteringResetRole != "@everyone" && BarteringResetRole != "@here")
                { BarteringResetRole = "<@&" + BarteringResetRole + ">"; }
            }
            string[] itrr = ProfileCollection[11].ToString().Split('{');
            ImperialTradingResetRole = itrr[1].ToString();
            if (ImperialTradingResetRole != "")
            {
                if (ImperialTradingResetRole != "@everyone" && ImperialTradingResetRole != "@here")
                { ImperialTradingResetRole = "<@&" + ImperialTradingResetRole + ">"; }
            }

            UpdateMesssageInterval = int.Parse(ProfileCollection[8].ToString());
            //AnouncmentMessageInterval = int.Parse(ProfileCollection[9].ToString());         

            LoadAlarmsSettings();
            LoadDefaultLanguage();


            if (Settings.Default["EditSpawnHoursSlider"].ToString() == "")
            {
                EditSpawnHoursSlider.Value = 0;
                Settings.Default["EditSpawnHoursSlider"] = EditSpawnHoursSlider.Value.ToString();
                Settings.Default.Save();
            }
            EditSpawnHoursSlider.Value = Int32.Parse(Settings.Default["EditSpawnHoursSlider"].ToString());

            if (EditSpawnHoursSlider.Value.ToString().Contains("-"))
            { EditSpawnHoursLabael1.Content = LanguageCollection[22].ToString()+ " " + EditSpawnHoursSlider.Value; }
            else
            { EditSpawnHoursLabael1.Content = LanguageCollection[22].ToString() + " +" + EditSpawnHoursSlider.Value; }
            if (EditSpawnHoursSlider.Value == 0)
            { EditSpawnHoursLabael1.Content = LanguageCollection[22].ToString() + " " + EditSpawnHoursSlider.Value; }
            startupRS = 0;          
        }  
        public void AlertNewUpdate()
        {
            var bc = new BrushConverter();
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0.2;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromMilliseconds(1500));
            da.AutoReverse = true;
            da.RepeatBehavior = RepeatBehavior.Forever;
            appRestartButton.BeginAnimation(OpacityProperty, da);
            appRestartButton.Background = (Brush)bc.ConvertFrom(subcolor);
        }
        private void mainWindow_Activated(object sender, EventArgs e)
        {
            if (startupRS == 0 && MOTR == null)
            {
                var bc = new BrushConverter();
                alertbosstabrectangle.Fill = (Brush)bc.ConvertFrom(subcolor);
                Homehighlight.Fill = (Brush)bc.ConvertFrom(subcolor);
                Timetablehighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                bosslisthighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                SelfRollingHighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                settingshighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                abouthighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                alertnighttabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                alertimperialtabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                alertbarteringtabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                alertimperialresettabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                exssoundbuttonrectangle.Fill = (Brush)bc.ConvertFrom(subcolor);
                exstimetablebuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                exsalertbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                AddNewBossButton.Content = LanguageCollection[42].ToString();
                Processing_Status_Grid.Width = 0;
                StartPostingButton.IsEnabled = false;    
                
                if(AUTCheckbox.IsChecked == true)
                { File.WriteAllText(Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT", File.ReadAllText(Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT_Origin")); }
                              
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    Discord_Bot Bot = new Discord_Bot();
                    Bot.GetDiscordData(this);
                    Bot.StartPosting_Cooldown(this);
                }).Start();
                startupRS = 1;
                string region = Settings.Default["DefaultRegion"].ToString();//get Last Saved Region  
                if (region == "")
                {
                    region = "NA";
                    Settings.Default["DefaultRegion"] = "NA";
                    Settings.Default.Save();
                }
                MOTR = region;
                if (SourceComboBox.SelectedIndex == 0)
                { GetLYPBBTTimeTable(); }
                if (SourceComboBox.SelectedIndex == 1)
                { GetTimeTable(region); }//Get info from Html Code
                if (SourceComboBox.SelectedIndex == 2)
                { GetUrlSource("https://bdobosstimer.com/?&server=" + region); }
                new Thread(() =>//icon bug fix
                {
                    Thread.CurrentThread.IsBackground = true;
                    Discord_Bot Bot = new Discord_Bot();
                    Bot.Missing_Icon_Fix(this);
                }).Start();               
            }
        }          
        private void LoadDefaultLanguage()
        {
            LanguageCollection.Clear();
            string lines = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/Languages/" + DefaultLanguage + "-m");
            string[] Collection = lines.Split('}');
            foreach (string rf in Collection)
            { LanguageCollection.Add(rf); }
            //MessageBox.Show(LanguageCollection.Count.ToString());
            SourceLabel.Content = LanguageCollection[0].ToString();
            RegionLabel.Content = LanguageCollection[1].ToString();
            BoshostLabel.Content = LanguageCollection[2].ToString();
            BoshostLabel.ToolTip = LanguageCollection[3].ToString();
            BotStatusLabel.Content = LanguageCollection[4].ToString();
            
            DiscordBotConnectionStatusLabel.Content = LanguageCollection[5].ToString();
            //LanguageCollection[6].ToString() is "Connecting..."
            //LanguageCollection[7].ToString() is "Connection Error!"
            //LanguageCollection[8].ToString() is " Connected"
            PbossLabel.Content = LanguageCollection[9].ToString();
            CbossLabel.Content = LanguageCollection[10].ToString();
            LbossLabel.Content = LanguageCollection[11].ToString();
            StartPostingButton.ToolTip = LanguageCollection[12].ToString();
            appRestartButton.ToolTip = LanguageCollection[13].ToString();          
            ConnectDiscordBotButton.ToolTip = LanguageCollection[14].ToString();
            DisconnectDiscordBot.ToolTip = LanguageCollection[15].ToString();
            AlarmsSettingsLabel.Content = LanguageCollection[16].ToString();
            BsAlarmLabel.Content = LanguageCollection[17].ToString();
            NTAlarmLabel.Content = LanguageCollection[18].ToString();
            IRAlarmLabel.Content = LanguageCollection[19].ToString();
            BRAlarmLabel.Content = LanguageCollection[20].ToString();
            ITRAlarmLabel.Content = LanguageCollection[21].ToString();
            EditSpawnHoursLabael1.Content = LanguageCollection[22].ToString();
            try { EditSpawnHoursSlider.Value = Int32.Parse(Settings.Default["EditSpawnHoursSlider"].ToString()); } catch (Exception) { EditSpawnHoursSlider.Value = 0; }

            if (EditSpawnHoursSlider.Value.ToString().Contains("-"))
            { EditSpawnHoursLabael1.Content = LanguageCollection[22].ToString() + " " + EditSpawnHoursSlider.Value; }
            else
            { EditSpawnHoursLabael1.Content = LanguageCollection[22].ToString() + " +" + EditSpawnHoursSlider.Value; }
            if (EditSpawnHoursSlider.Value == 0)
            { EditSpawnHoursLabael1.Content = LanguageCollection[22].ToString() + " " + EditSpawnHoursSlider.Value; }
            EditSpawnHoursSlider.ToolTip = LanguageCollection[23].ToString();

            BossSpawnAlarmCheckBox1.Content = LanguageCollection[24].ToString() + "1";
            BossSpawnAlarmCheckBox2.Content = LanguageCollection[24].ToString() + "2";
            BossSpawnAlarmCheckBox3.Content = LanguageCollection[24].ToString() + "3";

            NightTimeAlarmCheckBox1.Content = LanguageCollection[24].ToString() + "1";
            NightTimeAlarmCheckBox2.Content = LanguageCollection[24].ToString() + "2";
            NightTimeAlarmCheckBox3.Content = LanguageCollection[24].ToString() + "3";

            ImperialResetCheckBox1.Content = LanguageCollection[24].ToString() + "1";
            ImperialResetCheckBox2.Content = LanguageCollection[24].ToString() + "2";
            ImperialResetCheckBox3.Content = LanguageCollection[24].ToString() + "3";

            BRAlarmCheckBox1.Content = LanguageCollection[24].ToString() + "1";
            BRAlarmCheckBox2.Content = LanguageCollection[24].ToString() + "2";
            BRAlarmCheckBox3.Content = LanguageCollection[24].ToString() + "3";

            ITRAlarmCheckBox1.Content = LanguageCollection[24].ToString() + "1";
            ITRAlarmCheckBox2.Content = LanguageCollection[24].ToString() + "2";
            ITRAlarmCheckBox3.Content = LanguageCollection[24].ToString() + "3";

            //NILabel.Content = LanguageCollection[25].ToString();// Night in:
            IRILabel.Content = LanguageCollection[26].ToString();
            Notficationsettingslabel.Content = LanguageCollection[27].ToString();
            BRILabel.Content = LanguageCollection[28].ToString();
            ITRILabel.Content = LanguageCollection[29].ToString();

            PlaySoundOnLabel.Content = LanguageCollection[30].ToString();
            SoundOptionCheckBox.Content = LanguageCollection[31].ToString();
            NTSoundOptionCheckBox.Content = LanguageCollection[32].ToString();
            IRSoundOptionCheckBox.Content = LanguageCollection[33].ToString();
            BRSoundOptionCheckBox.Content = LanguageCollection[34].ToString();
            ITRSoundOptionCheckBox.Content = LanguageCollection[35].ToString();

            PostSettingsLabel.Content = LanguageCollection[36].ToString();
            DisplayTimeTableSetting.Content = LanguageCollection[37].ToString();

            AlertsSettingsLabel.Content = LanguageCollection[38].ToString();
            AlertonhousandminutesCheckBox.Content = LanguageCollection[39].ToString();

            TimeTableTimeColumnsLabel.Content = LanguageCollection[40].ToString();
            TimeTableBossesLabel.Content = LanguageCollection[41].ToString();
            TimeTableAddColumnButton.Content = LanguageCollection[42].ToString();//Add New
            //LanguageCollection[43].ToString() is "Time Table"

            BossListLabel.Content = LanguageCollection[44].ToString();
            BossListBossNameLabel.Content = LanguageCollection[45].ToString();
            BossListBossCustomNameLabel.Content = LanguageCollection[46].ToString();
            DILBLLabel.Content = LanguageCollection[47].ToString();
            AddnewBossTestImgLinkButton.Content = LanguageCollection[48].ToString();
            BossListSpecificRoleForthisBossLabel.Content = LanguageCollection[49].ToString();
            DILBLLabelLocal.Content = LanguageCollection[50].ToString();
            AddnewBossTestImgLinkButtonLocal.Content = LanguageCollection[48].ToString();
            BSLLLabel.Content = LanguageCollection[51].ToString();
            //AddSaveBossButton.Content = LanguageCollection[52].ToString();//Save
            RemoveBossButton.Content = LanguageCollection[53].ToString();// Remove

            BossListRestoreDefaultButton.Content = LanguageCollection[54].ToString();// Restore Default
            RestoreTimeTableDefaultButton.Content = LanguageCollection[54].ToString();

            SettingsLabel.Content = LanguageCollection[55].ToString();
            LanguageSLabel.Content = LanguageCollection[56].ToString();
            ClientIDLABEL.Content = LanguageCollection[57].ToString();
            TokenLabel.Content = LanguageCollection[58].ToString();
            ServerIDLAbel.Content = LanguageCollection[59].ToString();
            ChannelIDLabel.Content = LanguageCollection[60].ToString();
            UpdateIntervalLabel.Content = LanguageCollection[61].ToString();
            UTCombobox.ToolTip = LanguageCollection[62].ToString();
            //RMMLabel.Content = LanguageCollection[63].ToString();
            //RMMCombobox.ToolTip = LanguageCollection[64].ToString();
            //SettingsKeepMessagesCheckBox.Content = LanguageCollection[65].ToString();
            OverlayTransparencyLabel.Content = LanguageCollection[66].ToString();
            OverlayTransparencyLabel.ToolTip = LanguageCollection[67].ToString();
            PDRLabel.Content = LanguageCollection[68].ToString();
            BsPRLabel.Content = LanguageCollection[69].ToString();
            NTPRLabel.Content = LanguageCollection[70].ToString();
            IRPRLabel.Content = LanguageCollection[71].ToString();
            BRPRLabel.Content = LanguageCollection[72].ToString();
            ITRPRLabel.Content = LanguageCollection[73].ToString();
            ACMLabel.Content = LanguageCollection[74].ToString();
            BSACM1.Content = LanguageCollection[75].ToString() + "1";
            BSACM2.Content = LanguageCollection[75].ToString() + "2";
            BSACM3.Content = LanguageCollection[75].ToString() + "3";
            //LanguageCollection[76].ToString() is "<bossname> is Spawning in <00:00:00>"
            NTCM1.Content = LanguageCollection[77].ToString() + "1";
            NTCM2.Content = LanguageCollection[77].ToString() + "2";
            NTCM3.Content = LanguageCollection[77].ToString() + "3";
            //LanguageCollection[78].ToString() is "Night time in <00:00:00>"
            IRCM1.Content = LanguageCollection[79].ToString() + "1";
            IRCM2.Content = LanguageCollection[79].ToString() + "2";
            IRCM3.Content = LanguageCollection[79].ToString() + "3";
            //LanguageCollection[80].ToString() is "Imperial will Reset in <00:00:00>"
            BRCM1.Content = LanguageCollection[81].ToString() + "1";
            BRCM2.Content = LanguageCollection[81].ToString() + "2";
            BRCM3.Content = LanguageCollection[81].ToString() + "3";
            //LanguageCollection[82].ToString() is "Bartering will Reset in <00:00:00>"
            ITRCM1.Content = LanguageCollection[83].ToString() + "1";
            ITRCM2.Content = LanguageCollection[83].ToString() + "2";
            ITRCM3.Content = LanguageCollection[83].ToString() + "3";
            //LanguageCollection[84].ToString() is "Imperial Trading will Reset in <00:00:00>"
            //LanguageCollection[85].ToString() is "Click Here For Location"
            //NSListBox.ToolTip = LanguageCollection[86].ToString(); "Scroll Down For More"

            DisplayImageLinkextBoxLocal.ToolTip = LanguageCollection[87].ToString();

            BSA1CMTextBox.ToolTip = LanguageCollection[88].ToString();
            BSA2CMTextBox.ToolTip = BSA1CMTextBox.ToolTip;
            BSA3CMTextBox.ToolTip = BSA1CMTextBox.ToolTip;

            NTA1CMTextbBox.ToolTip = LanguageCollection[89].ToString();
            NTA2CMTextbBox.ToolTip = NTA1CMTextbBox.ToolTip;
            NTA3CMTextbBox.ToolTip = NTA1CMTextbBox.ToolTip;

            IRA1CMTextBox.ToolTip = LanguageCollection[90].ToString();
            IRA2CMTextBox.ToolTip = IRA1CMTextBox.ToolTip;
            IRA3CMTextBox.ToolTip = IRA1CMTextBox.ToolTip;

            BRA1CMTextBox.ToolTip = LanguageCollection[91].ToString();
            BRA2CMTextBox.ToolTip = BRA1CMTextBox.ToolTip;
            BRA3CMTextBox.ToolTip = BRA1CMTextBox.ToolTip;

            ITRA1CMTextBox.ToolTip = LanguageCollection[92].ToString();
            ITRA2CMTextBox.ToolTip = ITRA1CMTextBox.ToolTip;
            ITRA3CMTextBox.ToolTip = ITRA1CMTextBox.ToolTip;

            AboutLabel.Content = LanguageCollection[93].ToString();
            VersionLabel.Content = LanguageCollection[94].ToString();
            AppVersionLabel.Content = AppVersion;
            MainColorLabel.Content = LanguageCollection[95].ToString();
            MainColorPick.Content = LanguageCollection[96].ToString();
            SubColorLabel.Content = LanguageCollection[97].ToString();
            SubColorPick.Content = MainColorPick.Content;
            ABLabel.Content = LanguageCollection[98].ToString();
            PickAnimatedBackground.Content = LanguageCollection[99].ToString();
            BILabel.Content = LanguageCollection[100].ToString();
            PickBackgroundImage.Content = PickAnimatedBackground.Content;
            ResetMainColorButton.Content = LanguageCollection[101].ToString();
            ResetSubColorButton.Content = ResetMainColorButton.Content;
            ResetAnimatedBackgroundButton.Content = ResetMainColorButton.Content;
            ResetBackgroundImageButton.Content = ResetMainColorButton.Content;
            AnimatedBackgroundCheckBox.Content = LanguageCollection[102].ToString();
            BackgroundImageCheckbox.Content = LanguageCollection[103].ToString();
            HrdResetAppButton.Content = LanguageCollection[104].ToString();
            githubbutton.ToolTip = LanguageCollection[105].ToString();
            JoinDiscordButton.ToolTip = LanguageCollection[106].ToString();
            //NILabel.Content = LanguageCollection[107].ToString();// Day in
            //LanguageCollection[108].ToString() is "Discord Error"
            //LanguageCollection[109].ToString() is "Solution: The bot token is incorrect please make sure u have it the right one"
            //LanguageCollection[110].ToString() is "Source Error"
            //LanguageCollection[111].ToString() is "Failed to collect Data from source " + Environment.NewLine + Environment.NewLine + "Solution: User Diffrent source"
            //LanguageCollection[112].ToString() is "Please enter the correct Token"
            //LanguageCollection[113].ToString() is "WARNING"
            //LanguageCollection[114].ToString() is "Hard Resting the app well delete all saved Settings and information. are u sure u want to reset the app?"
            //LanguageCollection[115].ToString() is "Solution: The bot must have admin permission in order to clean the channel"
            //LanguageCollection[116].ToString() is "Solution: Delete all old messages in the channel or create a new empty channel just for the bot"
            //LanguageCollection[117].ToString() is "Unknow Error"
            //LanguageCollection[118].ToString() is "Solution: There have been problem loading local Resource please contact us on discord and will try our best to help whit the problem"
            RemoveTimeTableColumn.Content = LanguageCollection[53].ToString();
            //LanguageCollection[119].ToString() is "Ok"
            //LanguageCollection[120].ToString() is "No"
            //LanguageCollection[121].ToString() is "Yes"
            //LanguageCollection[122].ToString() is "Test Token"
            //LanguageCollection[123].ToString() is "Bosses Locations"
            //LanguageCollection[124].ToString() is "Server or the channel cannot be None u must select Server and channel in the settings Before Posting"
            //LanguageCollection[125].ToString() is "Checking For Updates..."
            //LanguageCollection[126].ToString() is "Update is Ready To install."
            //LanguageCollection[127].ToString() is "Application is Up-To-Date."
            //LanguageCollection[128].ToString() is "Failed to get version"
            ScarletModeTimeTableSetting_CheckBox.Content = LanguageCollection[129].ToString();//is "Scarlet Mode"
            //LanguageCollection[130].ToString() is "Self Rolling"
            SRLabel2RC.Content = LanguageCollection[131].ToString();// is "Rolling Channel:"
            SRLabel3.Content = LanguageCollection[132].ToString();// is "Enable Self Rolling:"
            SRStartMessageLabel.Content = LanguageCollection[133].ToString();// is "Enable Self Rolling:"
            UpdateSelfRollingButton.Content = LanguageCollection[134].ToString();// is "Update"
            SRRR5.Content = LanguageCollection[135].ToString();// is "Roles/Reaction with Message:"
            SelfRollingAddNewSelfRole_button.Content = LanguageCollection[136].ToString();// is "Add New"
            SelfRollingRemoveSelfRole_button.Content = LanguageCollection[137].ToString();// is "Remove"
            SRRolesLabel.Content = LanguageCollection[138].ToString();// is "Role:"
            SelfRollingInsertRole.Content = LanguageCollection[139].ToString();// is "Insert Role"
            SelfRollingInsertEmoji.Content = LanguageCollection[140].ToString();// is "Insert Emoji"
            SRReactionLabel.Content = LanguageCollection[141].ToString();// is "Reaction:" 
            SelfRolling_GetReactionButton.Content = LanguageCollection[142].ToString();// is "Get Emoji" 
            SREMLabel.Content = LanguageCollection[143].ToString();// is "End Message:" 
            AlertChannelID.Content = LanguageCollection[144].ToString();// is "Alert Channel ID:" 
            OALabel.Content = LanguageCollection[145].ToString();// is "Optimize App :" 
            //LanguageCollection[146].ToString(); is Restart: New Update is ready to install
            AUTTLabel.Content = LanguageCollection[147].ToString(); //Auto Update Time Tables:
            AUTTLabel.ToolTip = LanguageCollection[148].ToString(); //Auto Update Time Tablesfrom online source


            if (Settings.Default["BSA1CM"].ToString() == "")
            {
                BSA1CMTextBox.Text = LanguageCollection[76].ToString();
            }
            else
            {
                BSA1CMTextBox.Text = Settings.Default["BSA1CM"].ToString();
            }
            if (Settings.Default["BSA2CM"].ToString() == "")
            {
                BSA2CMTextBox.Text = LanguageCollection[76].ToString();
            }
            else
            {
                BSA2CMTextBox.Text = Settings.Default["BSA2CM"].ToString();
            }
            if (Settings.Default["BSA3CM"].ToString() == "")
            {
                BSA3CMTextBox.Text = LanguageCollection[76].ToString();
            }
            else
            {
                BSA3CMTextBox.Text = Settings.Default["BSA3CM"].ToString();
            }

            if (Settings.Default["NTA1CM"].ToString() == "")
            {
                NTA1CMTextbBox.Text = LanguageCollection[78].ToString();
            }
            else
            {
                NTA1CMTextbBox.Text = Settings.Default["NTA1CM"].ToString();
            }
            if (Settings.Default["NTA2CM"].ToString() == "")
            {
                NTA2CMTextbBox.Text = LanguageCollection[78].ToString();
            }
            else
            {
                NTA2CMTextbBox.Text = Settings.Default["NTA2CM"].ToString();
            }
            if (Settings.Default["NTA3CM"].ToString() == "")
            {
                NTA3CMTextbBox.Text = LanguageCollection[78].ToString();
            }
            else
            {
                NTA3CMTextbBox.Text = Settings.Default["NTA3CM"].ToString();
            }

            if (Settings.Default["IRA1CM"].ToString() == "")
            {
                IRA1CMTextBox.Text = LanguageCollection[80].ToString();
            }
            else
            {
                IRA1CMTextBox.Text = Settings.Default["IRA1CM"].ToString();
            }
            if (Settings.Default["IRA2CM"].ToString() == "")
            {
                IRA2CMTextBox.Text = LanguageCollection[80].ToString();
            }
            else
            {
                IRA2CMTextBox.Text = Settings.Default["IRA2CM"].ToString();
            }
            if (Settings.Default["IRA3CM"].ToString() == "")
            {
                IRA3CMTextBox.Text = LanguageCollection[80].ToString();
            }
            else
            {
                IRA3CMTextBox.Text = Settings.Default["IRA3CM"].ToString();
            }

            if (Settings.Default["BRA1CM"].ToString() == "")
            {
                BRA1CMTextBox.Text = LanguageCollection[82].ToString();
            }
            else
            {
                BRA1CMTextBox.Text = Settings.Default["BRA1CM"].ToString();
            }
            if (Settings.Default["BRA2CM"].ToString() == "")
            {
                BRA2CMTextBox.Text = LanguageCollection[82].ToString();
            }
            else
            {
                BRA2CMTextBox.Text = Settings.Default["BRA2CM"].ToString();
            }
            if (Settings.Default["BRA3CM"].ToString() == "")
            {
                BRA3CMTextBox.Text = LanguageCollection[82].ToString();
            }
            else
            {
                BRA3CMTextBox.Text = Settings.Default["BRA3CM"].ToString();
            }

            if (Settings.Default["ITRA1CM"].ToString() == "")
            {
                ITRA1CMTextBox.Text = LanguageCollection[84].ToString();
            }
            else
            {
                ITRA1CMTextBox.Text = Settings.Default["ITRA1CM"].ToString();
            }
            if (Settings.Default["ITRA2CM"].ToString() == "")
            {
                ITRA2CMTextBox.Text = LanguageCollection[84].ToString();
            }
            else
            {
                ITRA2CMTextBox.Text = Settings.Default["ITRA2CM"].ToString();
            }
            if (Settings.Default["ITRA3CM"].ToString() == "")
            {
                ITRA3CMTextBox.Text = LanguageCollection[84].ToString();
            }
            else
            {
                ITRA3CMTextBox.Text = Settings.Default["ITRA3CM"].ToString();
            }
           

        }
        private void LoadAlarmsSettings() //load Last Saved User Settings
        {
            if (Settings.Default["BossSpawnAlarmCheckBox"].ToString() == "")
            {
                BossSpawnAlarm1Textbox.Text = "30";
                BossSpawnAlarmCheckBox1.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["BossSpawnAlarmCheckBox"].ToString().Split(',');
                BossSpawnAlarm1Textbox.Text = cbD[0].ToString();
                if (cbD[1].ToString() == "0")
                { BossSpawnAlarmCheckBox1.IsChecked = false; }
                if (cbD[1].ToString() == "1")
                { BossSpawnAlarmCheckBox1.IsChecked = true; }
            }

            if (Settings.Default["BossSpawnAlarmCheckBox"].ToString() == "")
            {
                BossSpawnAlarm2Textbox.Text = "15";
                BossSpawnAlarmCheckBox2.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["BossSpawnAlarmCheckBox"].ToString().Split(',');
                BossSpawnAlarm2Textbox.Text = cbD[2].ToString();
                if (cbD[3].ToString() == "0")
                { BossSpawnAlarmCheckBox2.IsChecked = false; }
                if (cbD[3].ToString() == "1")
                { BossSpawnAlarmCheckBox2.IsChecked = true; }
            }

            if (Settings.Default["BossSpawnAlarmCheckBox"].ToString() == "")
            {
                BossSpawnAlarm3Textbox.Text = "05";
                BossSpawnAlarmCheckBox3.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["BossSpawnAlarmCheckBox"].ToString().Split(',');
                BossSpawnAlarm3Textbox.Text = cbD[4].ToString();
                if (cbD[5].ToString() == "0")
                { BossSpawnAlarmCheckBox3.IsChecked = false; }
                if (cbD[5].ToString() == "1")
                { BossSpawnAlarmCheckBox3.IsChecked = true; }
            }

            if (Settings.Default["NightTimeAlarmCheckBox"].ToString() == "")
            {
                NightTimeAlarm1Textbox.Text = "30";
                NightTimeAlarmCheckBox1.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["NightTimeAlarmCheckBox"].ToString().Split(',');
                NightTimeAlarm1Textbox.Text = cbD[0].ToString();
                if (cbD[1].ToString() == "0")
                { NightTimeAlarmCheckBox1.IsChecked = false; }
                if (cbD[1].ToString() == "1")
                { NightTimeAlarmCheckBox1.IsChecked = true; }
            }

            if (Settings.Default["NightTimeAlarmCheckBox"].ToString() == "")
            {
                NightTimeAlarm2Textbox.Text = "15";
                NightTimeAlarmCheckBox2.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["NightTimeAlarmCheckBox"].ToString().Split(',');
                NightTimeAlarm2Textbox.Text = cbD[2].ToString();
                if (cbD[3].ToString() == "0")
                { NightTimeAlarmCheckBox2.IsChecked = false; }
                if (cbD[3].ToString() == "1")
                { NightTimeAlarmCheckBox2.IsChecked = true; }
            }

            if (Settings.Default["NightTimeAlarmCheckBox"].ToString() == "")
            {
                NightTimeAlarm3Textbox.Text = "05";
                NightTimeAlarmCheckBox3.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["NightTimeAlarmCheckBox"].ToString().Split(',');
                NightTimeAlarm3Textbox.Text = cbD[4].ToString();
                if (cbD[5].ToString() == "0")
                { NightTimeAlarmCheckBox3.IsChecked = false; }
                if (cbD[5].ToString() == "1")
                { NightTimeAlarmCheckBox3.IsChecked = true; }
            }

            if (Settings.Default["ImperialResetCheckBox"].ToString() == "")
            {
                ImperialResetAlarm1Textbox.Text = "30";
                ImperialResetCheckBox1.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["ImperialResetCheckBox"].ToString().Split(',');
                ImperialResetAlarm1Textbox.Text = cbD[0].ToString();
                if (cbD[1].ToString() == "0")
                { ImperialResetCheckBox1.IsChecked = false; }
                if (cbD[1].ToString() == "1")
                { ImperialResetCheckBox1.IsChecked = true; }
            }

            if (Settings.Default["ImperialResetCheckBox"].ToString() == "")
            {
                ImperialResetAlarm2Textbox.Text = "15";
                ImperialResetCheckBox2.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["ImperialResetCheckBox"].ToString().Split(',');
                ImperialResetAlarm2Textbox.Text = cbD[2].ToString();
                if (cbD[3].ToString() == "0")
                { ImperialResetCheckBox2.IsChecked = false; }
                if (cbD[3].ToString() == "1")
                { ImperialResetCheckBox2.IsChecked = true; }
            }

            if (Settings.Default["ImperialResetCheckBox"].ToString() == "")
            {
                ImperialResetAlarm3Textbox.Text = "05";
                ImperialResetCheckBox3.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["ImperialResetCheckBox"].ToString().Split(',');
                ImperialResetAlarm3Textbox.Text = cbD[4].ToString();
                if (cbD[5].ToString() == "0")
                { ImperialResetCheckBox3.IsChecked = false; }
                if (cbD[5].ToString() == "1")
                { ImperialResetCheckBox3.IsChecked = true; }
            }

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

            if (Settings.Default["DisplayTimeTableSetting"].ToString() == "")
            { DisplayTimeTableSetting.IsChecked = true; }
            if (Settings.Default["DisplayTimeTableSetting"].ToString() == "1")
            { DisplayTimeTableSetting.IsChecked = true; }
            if (Settings.Default["DisplayTimeTableSetting"].ToString() == "0")
            { DisplayTimeTableSetting.IsChecked = false; }

            if (Settings.Default["OverlayTransparency"].ToString() == "")
            { TransparacySlider.Value = 100; }
            else { TransparacySlider.Value = double.Parse(Settings.Default["OverlayTransparency"].ToString()); }

            //if (Settings.Default["SettingKeepMessages"].ToString() == "")
            //{
            //    SettingsKeepMessagesCheckBox.IsChecked = false;
            //    RMMCombobox.Opacity = 1;
            //    RMMCombobox.IsEnabled = true;
            //}
            //if (Settings.Default["SettingKeepMessages"].ToString() == "1")
            //{
            //    SettingsKeepMessagesCheckBox.IsChecked = true;
            //    RMMCombobox.Opacity = 0.3;
            //    RMMCombobox.IsEnabled = false;
            //}
            //if (Settings.Default["SettingKeepMessages"].ToString() == "0")
            //{
            //    SettingsKeepMessagesCheckBox.IsChecked = false;
            //    RMMCombobox.Opacity = 1;
            //    RMMCombobox.IsEnabled = true;
            //}
            if (Settings.Default["AlarmonhoursCheckbox"].ToString() == "")
            {
                AlertonhousandminutesCheckBox.IsChecked = false;
                Settings.Default["AlarmonhoursCheckbox"] = "0";
                Settings.Default.Save();
            }
            if (Settings.Default["AlarmonhoursCheckbox"].ToString() == "0")
            {
                AlertonhousandminutesCheckBox.IsChecked = false;
            }
            if (Settings.Default["AlarmonhoursCheckbox"].ToString() == "1")
            {
                AlertonhousandminutesCheckBox.IsChecked = true;
            }

            if (Settings.Default["BRAlarmCheckBox"].ToString() == "")
            {
                BRAlarm1Textbox.Text = "30";
                BRAlarmCheckBox1.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["BRAlarmCheckBox"].ToString().Split(',');
                BRAlarm1Textbox.Text = cbD[0].ToString();
                if (cbD[1].ToString() == "0")
                { BRAlarmCheckBox1.IsChecked = false; }
                if (cbD[1].ToString() == "1")
                { BRAlarmCheckBox1.IsChecked = true; }
            }

            if (Settings.Default["BRAlarmCheckBox"].ToString() == "")
            {
                BRAlarm2Textbox.Text = "15";
                BRAlarmCheckBox2.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["BRAlarmCheckBox"].ToString().Split(',');
                BRAlarm2Textbox.Text = cbD[2].ToString();
                if (cbD[3].ToString() == "0")
                { BRAlarmCheckBox2.IsChecked = false; }
                if (cbD[3].ToString() == "1")
                { BRAlarmCheckBox2.IsChecked = true; }
            }

            if (Settings.Default["BRAlarmCheckBox"].ToString() == "")
            {
                BRAlarm3Textbox.Text = "05";
                BRAlarmCheckBox3.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["BRAlarmCheckBox"].ToString().Split(',');
                BRAlarm3Textbox.Text = cbD[4].ToString();
                if (cbD[5].ToString() == "0")
                { BRAlarmCheckBox3.IsChecked = false; }
                if (cbD[5].ToString() == "1")
                { BRAlarmCheckBox3.IsChecked = true; }
            }



            if (Settings.Default["ITRAlarmCheckBox"].ToString() == "")
            {
                ITRAlarm1Textbox.Text = "30";
                ITRAlarmCheckBox1.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["ITRAlarmCheckBox"].ToString().Split(',');
                ITRAlarm1Textbox.Text = cbD[0].ToString();
                if (cbD[1].ToString() == "0")
                { ITRAlarmCheckBox1.IsChecked = false; }
                if (cbD[1].ToString() == "1")
                { ITRAlarmCheckBox1.IsChecked = true; }
            }

            if (Settings.Default["ITRAlarmCheckBox"].ToString() == "")
            {
                ITRAlarm2Textbox.Text = "15";
                ITRAlarmCheckBox2.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["ITRAlarmCheckBox"].ToString().Split(',');
                ITRAlarm2Textbox.Text = cbD[2].ToString();
                if (cbD[3].ToString() == "0")
                { ITRAlarmCheckBox2.IsChecked = false; }
                if (cbD[3].ToString() == "1")
                { ITRAlarmCheckBox2.IsChecked = true; }
            }

            if (Settings.Default["ITRAlarmCheckBox"].ToString() == "")
            {
                ITRAlarm3Textbox.Text = "05";
                ITRAlarmCheckBox3.IsChecked = true;
            }
            else
            {
                string[] cbD = Settings.Default["ITRAlarmCheckBox"].ToString().Split(',');
                ITRAlarm3Textbox.Text = cbD[4].ToString();
                if (cbD[5].ToString() == "0")
                { ITRAlarmCheckBox3.IsChecked = false; }
                if (cbD[5].ToString() == "1")
                { ITRAlarmCheckBox3.IsChecked = true; }
            }
            //High_RenderQuality
            if (Settings.Default["High_RenderQuality"].ToString() == "")
            {
                BitmapScaling(false);
                OptimizeAppCheckbox.IsChecked = false;
            }
            else
            {
                bool is_Optimized = bool.Parse(Settings.Default["High_RenderQuality"].ToString());
                if(is_Optimized)
                {
                    BitmapScaling(true);
                    OptimizeAppCheckbox.IsChecked = true;
                }
                else
                {
                    BitmapScaling(false);
                    OptimizeAppCheckbox.IsChecked = false;
                }
            }
            if(Settings.Default["ScarletMode"].ToString() == "")
            {
                ScarletModeTimeTableSetting_CheckBox.IsChecked = true;
                Settings.Default["ScarletMode"] = "true";
                Settings.Default.Save();
            }
            else
            { ScarletModeTimeTableSetting_CheckBox.IsChecked = bool.Parse(Settings.Default["ScarletMode"].ToString()); }
            
            if (Settings.Default["SelfRolling"].ToString() == "")
            {
                EnableSelfRollingCheckbox.IsChecked = false;
                Settings.Default["SelfRolling"] = "false";
                Settings.Default.Save();
            }
            else
            { EnableSelfRollingCheckbox.IsChecked = bool.Parse(Settings.Default["SelfRolling"].ToString()); }
            if(Settings.Default["AutoUpdateTable"].ToString() == "")
            {
                AUTCheckbox.IsChecked = true;
                Settings.Default["AutoUpdateTable"] = true.ToString();
                Settings.Default.Save();
            }
            else
            {
                AUTCheckbox.IsChecked = bool.Parse(Settings.Default["AutoUpdateTable"].ToString());
            }
        }     
        private async void GetTimeTable(string r)// creat TimeTable and Get Time Logs
        {
            ScarletModeTimeTableSetting_CheckBox.IsChecked = false;
            ScarletModeTimeTableSetting_CheckBox.IsEnabled = false;
            Updated_ScarletMode_Message = "";
            ScarletMode_Message = "";
            PreviousBossTimeLabel.ToolTip = "";
            CurrentBossTimeLabel.ToolTip = "";
            NextBossTimeLabel.ToolTip = "";
            currentbossrole1 = "";
            currentbossrole2 = "";
            publicbossUrl = "";
            timer1.Stop();
            MOTR = r;
            startupRS = 0;
            var html = @"https://mmotimer.com/bdo/?server=" + r;
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = new HtmlDocument();
            string[] BossListSplited = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Bosses").Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            try { htmlDoc = web.Load(html); }
            catch {
                ErrorMessageBox emb = new ErrorMessageBox(LanguageCollection[119].ToString(), LanguageCollection[120].ToString(), LanguageCollection[121].ToString(), LanguageCollection[122].ToString());
                emb.MB_typeOK(LanguageCollection[110].ToString(), LanguageCollection[111].ToString(), this);
                emb.Show();
                this.IsEnabled = false;
            }
            var htmlNodes1 = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='ttab']").InnerText;
            var htmlregionDB = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[1]/div[4]/div/div[1]/div").InnerText;
            var aftertheNext = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='mainTbl']/tbody/tr[1]/td[2]/span").InnerText;
            var pSecondboss = "";
            try { pSecondboss = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[1]/div[1]/div/div[2]/div[1]").InnerText; } catch (Exception) { }
            PbossNamLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[1]/div[1]/div/div/div[1]").InnerText;
            if (pSecondboss != "")
            { PbossNamLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[1]/div[1]/div/div/div[1]").InnerText + " & " + pSecondboss; }
            PreviousBossTimeLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[1]/div[1]/div/div/div[2]").InnerText;
            var cSecondboss = "";
            try { cSecondboss = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[1]/div[2]/div/div[2]/div[1]").InnerText; } catch (Exception) { }
            CbossNameLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[1]/div[2]/div/div[1]/div[1]").InnerText;
            if (cSecondboss != "")
            { CbossNameLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[1]/div[2]/div/div[1]/div[1]").InnerText + " & " + cSecondboss; }
            CurrentBossTimeLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[1]/div[2]/div/div[1]/div[3]").InnerText;
            var nSecondboss = "";
            try { nSecondboss = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[1]/div[3]/div/div[2]/div[1]").InnerText; } catch (Exception) { }
            NBossNameLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[1]/div[3]/div/div/div[1]").InnerText;
            if (nSecondboss != "")
            { NBossNameLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[1]/div[3]/div/div/div[1]").InnerText + " & " + nSecondboss; }
            NextBossTimeLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[1]/div[3]/div/div/div[3]").InnerText;
            try { NightInBdoTimeLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[2]/div[1]/div[1]/span[2]").InnerText; } catch (Exception) { }
            try { IRTimeLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[2]/div[2]/div[1]/span").InnerText; } catch (Exception) { }
            try { BRTimeLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[2]/div[1]/div[3]/span").InnerText; } catch (Exception) { }
            try { ITRITimeLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[2]/div[2]/div[3]/span").InnerText; } catch (Exception) { }
            try { NILabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[1]/div[2]/div[1]/div[1]/span[1]").InnerText; } catch (Exception) { }

            if(NILabel.Content.ToString() == "Day in:")
            { NILabel.Content = LanguageCollection[107].ToString(); }
            else
            { NILabel.Content = LanguageCollection[25].ToString(); }

            RegionComboBox.Items.Clear();
            RegionComboBox.Items.Add("ps4-asia".ToUpper());
            RegionComboBox.Items.Add("eu".ToUpper());
            RegionComboBox.Items.Add("ps4-xbox-eu".ToUpper());
            RegionComboBox.Items.Add("jp".ToUpper());
            RegionComboBox.Items.Add("kr".ToUpper());
            RegionComboBox.Items.Add("mena".ToUpper());
            RegionComboBox.Items.Add("na".ToUpper());
            RegionComboBox.Items.Add("ps4-xbox-na".ToUpper());
            RegionComboBox.Items.Add("ru".ToUpper());
            RegionComboBox.Items.Add("sa".ToUpper());
            RegionComboBox.Items.Add("sea".ToUpper());
            RegionComboBox.Items.Add("th".ToUpper());
            RegionComboBox.Items.Add("tw".ToUpper());

            RegionComboBox.Text = Settings.Default["DefaultRegion"].ToString().ToUpper();

            if (RegionComboBox.SelectedIndex == -1)
            { RegionComboBox.Text = "NA"; }
            #region Load Timetable from online source and arrange it to Local Table
            string input = htmlNodes1.Replace(" ", "");
            string[] data = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            try
            {
                TimeTable.Clear();
                for (int i = TimeTable.Rows.Count - 1; i >= 0; i--)
                {
                    if (TimeTable.Rows[i][0].ToString() == "")
                        TimeTable.Rows[i].Delete();
                }
                foreach (var column in TimeTable.Columns.Cast<DataColumn>().ToArray())
                {
                    if (TimeTable.AsEnumerable().All(dr => dr.IsNull(column)))
                        TimeTable.Columns.Remove(column);
                }
                gridview1.ItemsSource = null;
                TimeTable.AcceptChanges();
            }
            catch (Exception) { }
            int Crow = 0;
            int Ccolumn = 0;
            int MaxColumn = TimeTable.Columns.Count;
            foreach (var bit in data)
            {
                string outpted_bit = bit;
                if (bit == "" && TimeTable.Rows.Count == 1 && TimeTable.Columns.Count > 0 && Crow == 0)
                {
                    TimeTable.Rows.Add(new object[] { "" });
                    Ccolumn = 0;
                    Crow++;
                }
                if (bit != "" && Crow == 0)
                {
                    TimeTable.Columns.Add(Ccolumn.ToString());
                    if (Ccolumn == 0)
                    { TimeTable.Rows.Add(new object[] { "" }); }
                    DataRow dr = TimeTable.Rows[Crow];
                    string bitP = bit;
                    if (bit.Contains(Environment.NewLine))
                    { bitP = bit.Replace(Environment.NewLine, "," + Environment.NewLine); }
                    dr[Ccolumn] = bitP;
                    MaxColumn++;
                    Ccolumn++;
                }
                outpted_bit = bit;
                if (bit == "" && Ccolumn == MaxColumn && TimeTable.Columns.Count > 0)
                {
                    TimeTable.Rows.Add(new object[] { "" });
                    Ccolumn = 0;
                    Crow++;
                }
                if (bit == "" && TimeTable.Rows.Count > 1 && Ccolumn > 0 && Ccolumn < MaxColumn)
                {
                    DataRow dr = TimeTable.Rows[Crow];
                    string bitP = bit;
                    dr[Ccolumn] = bitP;
                    Ccolumn++;
                }
                outpted_bit = bit;
                if (bit != "" && TimeTable.Rows.Count > 1 && Ccolumn < MaxColumn)
                {
                    DataRow dr = TimeTable.Rows[Crow];
                    string bitP = bit;
                    string boss_name1 = "";
                    string boss_name2 = "";
                    foreach (string BossData in BossListSplited)
                    {
                        if (BossData != "" && outpted_bit.Contains(BossData.Split(',')[0]))
                        {
                            boss_name1 = BossData.Split(',')[0];
                            outpted_bit = bit.Replace(BossData.Split(',')[0], "");
                            break;
                        }
                    }
                    foreach (string BossData in BossListSplited)
                    {
                        if (BossData != "" && outpted_bit.Contains(BossData.Split(',')[0]))
                        {
                            boss_name2 = BossData.Split(',')[0];
                            outpted_bit = bit.Replace(BossData.Split(',')[0], "");
                            break;
                        }
                    }
                    bitP = boss_name1 + "," + Environment.NewLine + boss_name2;
                    if (bitP == "," + Environment.NewLine)
                    { bitP = "-"; }
                    if(Ccolumn == 0)
                    { bitP = bit; }
                    dr[Ccolumn] = bitP;
                    Ccolumn++;
                }

            }
            for (int i = TimeTable.Rows.Count - 1; i >= 0; i--)
            {
                if (TimeTable.Rows[i][0].ToString() == "")
                    TimeTable.Rows[i].Delete();
            }

            TimeTable.AcceptChanges();
            int drow = 0;
            int dayC = 0;
            foreach (DataRow row in TimeTable.Rows)
            {
                if (aftertheNext.Contains(row["0"].ToString()))
                { dayC = drow; }
                drow++;
            }
            SharedDay = dayC;
            gridview1.ItemsSource = TimeTable.DefaultView;
            #endregion
            startupRS = 1;
            try
            {
                BossesCollection.Clear();
                BossCollectionListBox.Items.Clear();
            }
            catch (Exception) { }
            
            foreach (string BossData in BossListSplited)
            {
                if (BossData != "")
                {
                    BossesCollection.Add(BossData.Trim());
                    BossCollectionListBox.Items.Add(BossData.Split(',')[0]);
                }
            }

            PBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/Boss.png"));
            NBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/Boss.png"));
            LBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/Boss.png"));
            Getimg gm = new Getimg();

            string First_boss = "";
            string Second_boss = "";
           
            foreach (string Boss in BossesCollection)
            {
                string bossName = Boss.Substring(0, Boss.IndexOf(",") + 1);
                bossName = bossName.Replace(",", "");
                bossName = bossName.Replace(Environment.NewLine, "");

                if (PbossNamLabel.Content.ToString().Contains(bossName))
                {
                    string[] bossdata = Boss.Split(',');
                    string imgurl = bossdata[1].ToString();
                    if (bossdata[5].ToString() != "")
                    {
                        imgurl = bossdata[5].ToString();
                        if (imgurl.Contains("<Local>"))
                        { imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
                    }
                    try { PBImageBox.Source = gm.GETIMAGE(imgurl); } catch (Exception) { }
                    if (PbossNamLabel.Content.ToString().Contains("&"))
                    { try { PBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/" + PbossNamLabel.Content.ToString() + ".png"); } catch (Exception) { } }
                }
                if (CbossNameLabel.Content.ToString().Contains(bossName))
                {
                    string[] bossdata = Boss.Split(',');
                    try
                    {
                        string imgurl = bossdata[1].ToString();
                        if (bossdata[5].ToString() == "")
                        {
                            imgurl = bossdata[1].ToString();
                        }
                        else
                        {
                            imgurl = bossdata[5].ToString();
                            if (imgurl.Contains("<Local>"))
                            { imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
                        }
                        publicNbossimage = bossdata[1].ToString();
                        if (!CbossNameLabel.Content.ToString().Contains("&"))
                        { publicbossUrl = bossdata[2].ToString(); }
                        else
                        {
                            if (publicbossUrl == "")
                            { publicbossUrl = bossdata[2].ToString(); }
                            else
                            { publicbossUrl += " | " + bossdata[2].ToString(); }
                        }
                        NBImageBox.Source = gm.GETIMAGE(imgurl);
                        if (CbossNameLabel.Content.ToString().Contains("&"))
                        { try { NBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/" + CbossNameLabel.Content.ToString() + ".png"); } catch (Exception) { } }
                    }
                    catch (Exception) { }
                }
                if (cSecondboss.Contains(bossName) && CbossNameLabel.Content.ToString().Contains("&") && cSecondboss != "")
                {
                    string[] bossdata = Boss.Split(',');
                    if (publicbossUrl == "")
                    { publicbossUrl = bossdata[2].ToString(); }
                    else
                    { publicbossUrl += "|" + bossdata[2].ToString(); }
                }
                if (NBossNameLabel.Content.ToString().Contains(bossName))
                {
                    string[] bossdata = Boss.Split(',');
                    string imgurl = bossdata[1].ToString();
                    if (bossdata[5].ToString() != "")
                    {
                        imgurl = bossdata[5].ToString();
                        if (imgurl.Contains("<Local>"))
                        { imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
                    }
                    try { LBImageBox.Source = gm.GETIMAGE(imgurl); } catch (Exception) { }
                    if (NBossNameLabel.Content.ToString().Contains("&"))
                    { try { LBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/" + NBossNameLabel.Content.ToString() + ".png"); } catch (Exception) { } }
                }
            }

            if (PbossNamLabel.Content.ToString().Contains(" & "))
            {
                string fullbossname = "";
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    string name = PbossNamLabel.Content.ToString();
                    name = name.Substring(0, name.IndexOf("&"));
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        if (bossdata[3].ToString() != "")
                        {
                            fullbossname += bossdata[3].ToString() + " & ";
                        }
                    }
                }
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    string name = PbossNamLabel.Content.ToString();
                    name = name.Substring(name.IndexOf("&") + 1);
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        if (bossdata[3].ToString() != "")
                        {
                            fullbossname += bossdata[3].ToString();
                        }
                    }
                }
                if (fullbossname != "")
                { PbossNamLabel.Content = fullbossname; }

            }
            else
            {
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    if (PbossNamLabel.Content.ToString().Contains(bossdata[0].ToString()))
                    {
                        if (bossdata[3].ToString() != "")
                        { PbossNamLabel.Content = bossdata[3].ToString(); }
                    }

                }
            }

            if (CbossNameLabel.Content.ToString().Contains(" & "))
            {
                string fullbossname = "";
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    string name = CbossNameLabel.Content.ToString();
                    name = name.Substring(0, name.IndexOf("&"));
                    First_boss = name;
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        string[] br = bossdata[4].ToString().Split('{');
                        currentbossrole1 = br[1].ToString();
                        if (bossdata[3].ToString() != "")
                        {
                            fullbossname += bossdata[3].ToString() + " & ";
                        }
                    }
                }
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    string name = CbossNameLabel.Content.ToString();
                    name = name.Substring(name.IndexOf("&") + 1);
                    Second_boss = name;
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        string[] br = bossdata[4].ToString().Split('{');
                        currentbossrole2 = br[1].ToString();
                        if (bossdata[3].ToString() != "")
                        {
                            fullbossname += bossdata[3].ToString();
                        }
                    }
                }
                if (fullbossname != "")
                { CbossNameLabel.Content = fullbossname; }
            }
            else
            {
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    if (CbossNameLabel.Content.ToString().TrimStart().TrimEnd() == bossdata[0].ToString())
                    {
                        if (bossdata[3].ToString() != "")
                        {
                            string[] br = bossdata[4].ToString().Split('{');
                            currentbossrole1 = br[1].ToString();
                            CbossNameLabel.Content = bossdata[3].ToString();
                        }
                        else
                        {
                            string[] br = bossdata[4].ToString().Split('{');
                            currentbossrole1 = br[1].ToString();
                        }
                    }

                }
            }

            if (NBossNameLabel.Content.ToString().Contains(" & "))
            {
                string fullbossname = "";
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    string name = NBossNameLabel.Content.ToString();
                    name = name.Substring(0, name.IndexOf("&"));
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        if (bossdata[3].ToString() != "")
                        {
                            fullbossname += bossdata[3].ToString() + " & ";
                        }
                    }
                }
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    string name = NBossNameLabel.Content.ToString();
                    name = name.Substring(name.IndexOf("&") + 1);
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        if (bossdata[3].ToString() != "")
                        {
                            fullbossname += bossdata[3].ToString();
                        }
                    }
                }
                if (fullbossname != "")
                { NBossNameLabel.Content = fullbossname; }
            }
            else
            {
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    if (NBossNameLabel.Content.ToString().Contains(bossdata[0].ToString()))
                    {
                        if (bossdata[3].ToString() != "")
                        { NBossNameLabel.Content = bossdata[3].ToString(); }
                    }

                }
            }

            SaveLatestTimeTable = 0;
            intervalMessageUpdate = UpdateMesssageInterval;

            if (CbossNameLabel.Content.ToString().Contains(" & "))
            {
                publicNbossimage = "https://raw.githubusercontent.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/master/Resources/Images/" + First_boss.Replace(" ", "") + "%20%26%20" + Second_boss.Replace(" ", "") + ".png";
            }
            if (discord.ConnectionState == Discord.ConnectionState.Connected && isposting == 1)
            {
                try
                {
                    var guild = discord.GetGuild(ServerID);
                    var channel = guild.GetTextChannel(Main_BotChannel_ID);
                    var Message = await channel.GetMessageAsync(bossImageID) as IUserMessage;
                    string[] pbu = publicbossUrl.Split('|');
                    string[] bnu = CbossNameLabel.Content.ToString().Split('&');
                    string ANmessage = "";
                    if (CbossNameLabel.Content.ToString().Contains("&"))
                    {
                        ANmessage = LanguageCollection[123].ToString() + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[85].ToString() + Environment.NewLine + "[" + bnu[1] + "](" + pbu[1] + ")" + " <---" + LanguageCollection[85].ToString();
                    }
                    else
                    {
                        ANmessage = LanguageCollection[123].ToString() + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[85].ToString();
                    }
                    var embed1 = new EmbedBuilder
                    {
                        Title = CbossNameLabel.Content.ToString() /*+ " <---" + LanguageCollection[87].ToString()*/,
                        ImageUrl = publicNbossimage,
                        Color = Color.LightGrey,
                        //Url = publicbossUrl  
                        Description = ANmessage
                    };
                    await Message.ModifyAsync(msg => msg.Embed = embed1.Build());
                }
                catch (Exception) { }
            }
            TimeSpan CC = TimeSpan.FromHours(1);
            if (EditSpawnHoursSlider.Value.ToString().Contains("-"))
            {
                TimeSpan PDT = TimeSpan.Parse(PreviousBossTimeLabel.Content.ToString());
                PreviousBossTimeLabel.Content = PDT.Add(CC).ToString(@"hh\:mm\:ss");

                TimeSpan CDT = TimeSpan.Parse(CurrentBossTimeLabel.Content.ToString());
                CurrentBossTimeLabel.Content = CDT.Subtract(CC).ToString(@"hh\:mm\:ss");

                TimeSpan NDT = TimeSpan.Parse(NextBossTimeLabel.Content.ToString());
                NextBossTimeLabel.Content = NDT.Subtract(CC).ToString(@"hh\:mm\:ss");
            }
            if (EditSpawnHoursSlider.Value > 0)
            {
                TimeSpan PDT = TimeSpan.Parse(PreviousBossTimeLabel.Content.ToString());
                PreviousBossTimeLabel.Content = PDT.Subtract(CC).ToString(@"hh\:mm\:ss");

                TimeSpan CDT = TimeSpan.Parse(CurrentBossTimeLabel.Content.ToString());
                CurrentBossTimeLabel.Content = CDT.Add(CC).ToString(@"hh\:mm\:ss");

                TimeSpan NDT = TimeSpan.Parse(NextBossTimeLabel.Content.ToString());
                NextBossTimeLabel.Content = NDT.Add(CC).ToString(@"hh\:mm\:ss");
            }
            if (currentbossrole1 != "")
            {
                if (currentbossrole1 != "@everyone" && currentbossrole1 != "@here")
                { currentbossrole1 = "<@&" + currentbossrole1 + ">"; }
            }
            if (currentbossrole2 != "")
            {
                if (currentbossrole2 != "@everyone" && currentbossrole2 != "@here")
                { currentbossrole2 = "<@&" + currentbossrole2 + ">"; }
            }

            if (discord.ConnectionState == Discord.ConnectionState.Connected && isposting == 1)
            {
                if (DisplayTimeTableSetting.IsChecked == true)
                {
                    if (TimtableID != 0)
                    {
                        var guild = discord.GetGuild(ServerID);
                        var channel = guild.GetTextChannel(Main_BotChannel_ID);
                        await channel.DeleteMessageAsync(TimtableID);
                        TimtableID = 0;
                    }

                    if (TimtableID == 0)
                    {
                        try { gridview1.SelectedIndex = SharedDay; } catch (Exception) { }
                        if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"))
                        { File.Delete(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"); }
                        //int width = int.Parse(gridview1.ActualWidth.ToString());//error
                        //int height = int.Parse(gridview1.ActualHeight.ToString());
                        RenderTargetBitmap renderTargetBitmap =
                       new RenderTargetBitmap(1920, 1080, 180, 250, PixelFormats.Pbgra32);
                        renderTargetBitmap.Render(gridview1);
                        PngBitmapEncoder pngImage = new PngBitmapEncoder();
                        pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                        using (Stream fileStream = File.Create(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"))
                        {
                            pngImage.Save(fileStream);
                        }
                        var guild = discord.GetGuild(ServerID);
                        var channel = guild.GetTextChannel(Main_BotChannel_ID);
                        var timetablemessage = await channel.SendFileAsync(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png", MOTR.ToString().ToUpper() + LanguageCollection[43].ToString());
                        TimtableID = timetablemessage.Id;
                    }
                }
            }
           
            timer1.Start();
        }     
        void timer1_Tick(object sender, EventArgs e) //Timer Loop message Updates&others
        {          
            if(discord.ConnectionState == Discord.ConnectionState.Connected && botuptime >= 30)
            { 
                discord.SetStatusAsync(Discord.UserStatus.Online); botuptime = 0;
                if(NBossNameLabel.Content.ToString() == "...")
                { GetLYPBBTTimeTable(); }
            }
            botuptime++;
            TimeSpan CC = TimeSpan.FromSeconds(1); 
            PbossNamLabel.ToolTip = PbossNamLabel.Content.ToString();
            CbossNameLabel.ToolTip = CbossNameLabel.Content.ToString();
            NBossNameLabel.ToolTip = NBossNameLabel.Content.ToString();
           
            TimeSpan PDT = TimeSpan.Parse(PreviousBossTimeLabel.Content.ToString());
            PreviousBossTimeLabel.Content = PDT.Add(CC).ToString(@"hh\:mm\:ss");          
             
            TimeSpan CDT = TimeSpan.Parse(CurrentBossTimeLabel.Content.ToString());
            CurrentBossTimeLabel.Content = CDT.Subtract(CC).ToString(@"hh\:mm\:ss");

            TimeSpan NDT = TimeSpan.Parse(NextBossTimeLabel.Content.ToString());
            NextBossTimeLabel.Content = NDT.Subtract(CC).ToString(@"hh\:mm\:ss");

            TimeSpan NiBDO = TimeSpan.Parse(NightInBdoTimeLabel.Content.ToString());
            NightInBdoTimeLabel.Content = NiBDO.Subtract(CC).ToString(@"hh\:mm\:ss");

            TimeSpan IR = TimeSpan.Parse(IRTimeLabel.Content.ToString());
            IRTimeLabel.Content = IR.Subtract(CC).ToString(@"hh\:mm\:ss");

            TimeSpan BRI = TimeSpan.Parse(BRTimeLabel.Content.ToString());
            BRTimeLabel.Content = BRI.Subtract(CC).ToString(@"hh\:mm\:ss");

            TimeSpan ITRI = TimeSpan.Parse(ITRITimeLabel.Content.ToString());
            ITRITimeLabel.Content = ITRI.Subtract(CC).ToString(@"hh\:mm\:ss");


            if (overlayState == 1)
            {
                try
                {
                    BitmapImage img = new BitmapImage();
                    img = NBImageBox.Source as BitmapImage;
                    omw.UpdateData(BRILabel.Content.ToString() , img, CbossLabel.Content.ToString(), CbossNameLabel.Content.ToString(), CurrentBossTimeLabel.Content.ToString(), NILabel.Content.ToString(), NightInBdoTimeLabel.Content.ToString(), IRILabel.Content.ToString(), IRTimeLabel.Content.ToString(), BRTimeLabel.Content.ToString(), SoundOptionCheckBox.Content.ToString(), NTSoundOptionCheckBox.Content.ToString(), IRSoundOptionCheckBox.Content.ToString(), BRSoundOptionCheckBox.Content.ToString(), ITRSoundOptionCheckBox.Content.ToString(), PlaySoundOnLabel.Content.ToString(), ITRILabel.Content.ToString(), ITRITimeLabel.Content.ToString());
                }
                catch (Exception) { }

            }        
                     
                #region "Boss Alert's"
                if (AlertonhousandminutesCheckBox.IsChecked == true)
                {
                    if (BossSpawnAlarmCheckBox1.IsChecked == true && CurrentBossTimeLabel.Content.ToString() == BossSpawnAlarm1Textbox.Text + ":00" + ":00")
                    {
                        string message = BSA1CMTextBox.Text;
                        DiscordNotifyBossSpwn(message);
                        if (SoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playBossAlertaudio();
                        }
                    }

                    if (BossSpawnAlarmCheckBox2.IsChecked == true && CurrentBossTimeLabel.Content.ToString() == BossSpawnAlarm1Textbox.Text + ":00" + ":00")
                    {
                        string message = BSA2CMTextBox.Text;
                        DiscordNotifyBossSpwn(message);
                        if (SoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playBossAlertaudio();
                        }
                    }

                    if (BossSpawnAlarmCheckBox3.IsChecked == true && CurrentBossTimeLabel.Content.ToString() == BossSpawnAlarm1Textbox.Text + ":00" + ":00")
                    {
                        string message = BSA3CMTextBox.Text;
                        DiscordNotifyBossSpwn(message);
                        if (SoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playBossAlertaudio();
                        }
                    }
                }
                if (BossSpawnAlarmCheckBox1.IsChecked == true && CurrentBossTimeLabel.Content.ToString() == "00:" + BossSpawnAlarm1Textbox.Text + ":00")
                {
                    string message = BSA1CMTextBox.Text;
                    DiscordNotifyBossSpwn(message);
                    if (SoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playBossAlertaudio();
                    }
                }

                if (BossSpawnAlarmCheckBox2.IsChecked == true && CurrentBossTimeLabel.Content.ToString() == "00:" + BossSpawnAlarm2Textbox.Text + ":00")
                {
                    string message = BSA2CMTextBox.Text;
                    DiscordNotifyBossSpwn(message);
                    if (SoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playBossAlertaudio();
                    }
                }

                if (BossSpawnAlarmCheckBox3.IsChecked == true && CurrentBossTimeLabel.Content.ToString() == "00:" + BossSpawnAlarm3Textbox.Text + ":00")
                {
                    string message = BSA3CMTextBox.Text;
                    DiscordNotifyBossSpwn(message);
                    if (SoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playBossAlertaudio();
                    }
                }
                #endregion
                #region "NightTime Alert's"
                if (AlertonhousandminutesCheckBox.IsChecked == true)
                {
                    if (NightTimeAlarmCheckBox1.IsChecked == true && NightInBdoTimeLabel.Content.ToString() == NightTimeAlarm1Textbox.Text + ":00" + ":00" && NILabel.Content.ToString() == LanguageCollection[25].ToString())
                    {
                        string message = NTA1CMTextbBox.Text;
                        DiscordNotifyNightTime(message);
                        if (NTSoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playNightAlertaudio();
                        }
                    }

                    if (NightTimeAlarmCheckBox2.IsChecked == true && NightInBdoTimeLabel.Content.ToString() == NightTimeAlarm2Textbox.Text + ":00" + ":00" && NILabel.Content.ToString() == LanguageCollection[25].ToString())
                    {
                        string message = NTA2CMTextbBox.Text;
                        DiscordNotifyNightTime(message);
                        if (NTSoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playNightAlertaudio();
                        }
                    }

                    if (NightTimeAlarmCheckBox3.IsChecked == true && NightInBdoTimeLabel.Content.ToString() == NightTimeAlarm3Textbox.Text + ":00" + ":00" && NILabel.Content.ToString() == LanguageCollection[25].ToString())
                    {
                        string message = NTA3CMTextbBox.Text;
                        DiscordNotifyNightTime(message);
                        if (NTSoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playNightAlertaudio();
                        }
                    }
                }
                if (NightTimeAlarmCheckBox1.IsChecked == true && NightInBdoTimeLabel.Content.ToString() == "00:" + NightTimeAlarm1Textbox.Text + ":00" && NILabel.Content.ToString() == LanguageCollection[25].ToString())
                {
                    string message = NTA1CMTextbBox.Text;
                    DiscordNotifyNightTime(message);
                    if (NTSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playNightAlertaudio();
                    }
                }

                if (NightTimeAlarmCheckBox2.IsChecked == true && NightInBdoTimeLabel.Content.ToString() == "00:" + NightTimeAlarm2Textbox.Text + ":00" && NILabel.Content.ToString() == LanguageCollection[25].ToString())
                {
                    string message = NTA2CMTextbBox.Text;
                    DiscordNotifyNightTime(message);
                    if (NTSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playNightAlertaudio();
                    }
                }

                if (NightTimeAlarmCheckBox3.IsChecked == true && NightInBdoTimeLabel.Content.ToString() == "00:" + NightTimeAlarm3Textbox.Text + ":00" && NILabel.Content.ToString() == LanguageCollection[25].ToString())
                {
                    string message = NTA3CMTextbBox.Text;
                    DiscordNotifyNightTime(message);
                    if (NTSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playNightAlertaudio();
                    }
                }
                #endregion
                #region "Imperial Reset Alert's"
                if (AlertonhousandminutesCheckBox.IsChecked == true)
                {
                    if (ImperialResetCheckBox1.IsChecked == true && IRTimeLabel.Content.ToString() == ImperialResetAlarm1Textbox.Text + ":00" + ":00")
                    {
                        string message = IRA1CMTextBox.Text;
                        DiscordNotifyImperialReset(message);
                        if (IRSoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playImperialResetAlertaudio();
                        }
                    }

                    if (ImperialResetCheckBox2.IsChecked == true && IRTimeLabel.Content.ToString() == ImperialResetAlarm2Textbox.Text + ":00" + ":00")
                    {
                        string message = IRA2CMTextBox.Text;
                        DiscordNotifyImperialReset(message);
                        if (IRSoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playImperialResetAlertaudio();
                        }
                    }

                    if (ImperialResetCheckBox3.IsChecked == true && IRTimeLabel.Content.ToString() == ImperialResetAlarm3Textbox.Text + ":00" + ":00")
                    {
                        string message = IRA3CMTextBox.Text;
                        DiscordNotifyImperialReset(message);
                        if (IRSoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playImperialResetAlertaudio();
                        }
                    }
                }
                if (ImperialResetCheckBox1.IsChecked == true && IRTimeLabel.Content.ToString() == "00:" + ImperialResetAlarm1Textbox.Text + ":00")
                {
                    string message = IRA1CMTextBox.Text;
                    DiscordNotifyImperialReset(message);
                    if (IRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playImperialResetAlertaudio();
                    }
                }

                if (ImperialResetCheckBox2.IsChecked == true && IRTimeLabel.Content.ToString() == "00:" + ImperialResetAlarm2Textbox.Text + ":00")
                {
                    string message = IRA2CMTextBox.Text;
                    DiscordNotifyImperialReset(message);
                    if (IRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playImperialResetAlertaudio();
                    }
                }

                if (ImperialResetCheckBox3.IsChecked == true && IRTimeLabel.Content.ToString() == "00:" + ImperialResetAlarm3Textbox.Text + ":00")
                {
                    string message = IRA3CMTextBox.Text;
                    DiscordNotifyImperialReset(message);
                    if (IRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playImperialResetAlertaudio();
                    }
                }
                #endregion
                #region "Bartering Reset Alert's"
                if (AlertonhousandminutesCheckBox.IsChecked == true)
                {
                    if (BRAlarmCheckBox1.IsChecked == true && BRTimeLabel.Content.ToString() == BRAlarm1Textbox.Text + ":00" + ":00")
                    {
                        string message = BRA1CMTextBox.Text;
                        DiscordNotifyBR(message);
                        if (BRSoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playBarteringAlertaudio();
                        }
                    }

                    if (BRAlarmCheckBox2.IsChecked == true && BRTimeLabel.Content.ToString() == BRAlarm2Textbox.Text + ":00" + ":00")
                    {
                        string message = BRA2CMTextBox.Text;
                        DiscordNotifyBR(message);
                        if (BRSoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playBarteringAlertaudio();
                        }
                    }

                    if (BRAlarmCheckBox3.IsChecked == true && BRTimeLabel.Content.ToString() == BRAlarm3Textbox.Text + ":00" + ":00")
                    {
                        string message = BRA3CMTextBox.Text;
                        DiscordNotifyBR(message);
                        if (BRSoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playBarteringAlertaudio();
                        }
                    }
                }
                if (BRAlarmCheckBox1.IsChecked == true && BRTimeLabel.Content.ToString() == "00:" + BRAlarm1Textbox.Text + ":00")
                {
                    string message = BRA1CMTextBox.Text;
                    DiscordNotifyBR(message);
                    if (BRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playBarteringAlertaudio();
                    }
                }

                if (BRAlarmCheckBox2.IsChecked == true && BRTimeLabel.Content.ToString() == "00:" + BRAlarm2Textbox.Text + ":00")
                {
                    string message = BRA2CMTextBox.Text;
                    DiscordNotifyBR(message);
                    if (BRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playBarteringAlertaudio();
                    }
                }

                if (BRAlarmCheckBox3.IsChecked == true && BRTimeLabel.Content.ToString() == "00:" + BRAlarm3Textbox.Text + ":00")
                {
                    string message = BRA3CMTextBox.Text;
                    DiscordNotifyBR(message);
                    if (BRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playBarteringAlertaudio();
                    }
                }
                #endregion
                #region "Imperial Trading Reset Alert's"
                if (AlertonhousandminutesCheckBox.IsChecked == true)
                {
                    if (ITRAlarmCheckBox1.IsChecked == true && ITRITimeLabel.Content.ToString() == ITRAlarm1Textbox.Text + ":00" + ":00")
                    {
                        string message = ITRA1CMTextBox.Text;
                        DiscordNotifyITR(message);
                        if (ITRSoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playImperialTradingResetAlertaudio();
                        }
                    }

                    if (ITRAlarmCheckBox2.IsChecked == true && ITRITimeLabel.Content.ToString() == ITRAlarm2Textbox.Text + ":00" + ":00")
                    {
                        string message = ITRA2CMTextBox.Text;
                        DiscordNotifyITR(message);
                        if (ITRSoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playImperialTradingResetAlertaudio();
                        }
                    }

                    if (ITRAlarmCheckBox3.IsChecked == true && ITRITimeLabel.Content.ToString() == ITRAlarm3Textbox.Text + ":00" + ":00")
                    {
                        string message = ITRA3CMTextBox.Text;
                        DiscordNotifyITR(message);
                        if (ITRSoundOptionCheckBox.IsChecked == true)
                        {
                            playAudio pa = new playAudio();
                            pa.playImperialTradingResetAlertaudio();
                        }
                    }
                }
                if (ITRAlarmCheckBox1.IsChecked == true && ITRITimeLabel.Content.ToString() == "00:" + ITRAlarm1Textbox.Text + ":00")
                {
                    string message = ITRA1CMTextBox.Text;
                    DiscordNotifyITR(message);
                    if (ITRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playImperialTradingResetAlertaudio();
                    }
                }

                if (ITRAlarmCheckBox2.IsChecked == true && ITRITimeLabel.Content.ToString() == "00:" + ITRAlarm2Textbox.Text + ":00")
                {
                    string message = ITRA2CMTextBox.Text;
                    DiscordNotifyITR(message);
                    if (ITRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playImperialTradingResetAlertaudio();
                    }
                }

                if (ITRAlarmCheckBox3.IsChecked == true && ITRITimeLabel.Content.ToString() == "00:" + ITRAlarm3Textbox.Text + ":00")
                {
                    string message = ITRA3CMTextBox.Text;
                    DiscordNotifyITR(message);
                    if (ITRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playImperialTradingResetAlertaudio();
                    }
                }
            #endregion

            if (CurrentBossTimeLabel.Content.ToString() != "00:00:00")
            {
                if (NightInBdoTimeLabel.Content.ToString() == "00:00:00")
                {
                   // if (NILabel.Content.ToString() == LanguageCollection[25].ToString())
                   // {
                        if (SourceComboBox.SelectedIndex == 0)
                        { GetLYPBBTTimeTable(); }
                        if (SourceComboBox.SelectedIndex == 1)
                        { GetTimeTable(MOTR); }//Get info from Html Code
                        if (SourceComboBox.SelectedIndex == 2)
                        { GetUrlSource("https://bdobosstimer.com/?&server=" + MOTR); }
                  //  }
                }
                if (IRTimeLabel.Content.ToString() == "00:00:00")
                {
                    if (SourceComboBox.SelectedIndex == 0)
                    { GetLYPBBTTimeTable(); }
                    if (SourceComboBox.SelectedIndex == 1)
                    { GetTimeTable(MOTR); }//Get info from Html Code
                    if (SourceComboBox.SelectedIndex == 2)
                    { GetUrlSource("https://bdobosstimer.com/?&server=" + MOTR); }
                }
                if (BRTimeLabel.Content.ToString() == "00:00:00")
                {
                    if (SourceComboBox.SelectedIndex == 0)
                    { GetLYPBBTTimeTable(); }
                    if (SourceComboBox.SelectedIndex == 1)
                    { GetTimeTable(MOTR); }//Get info from Html Code
                    if (SourceComboBox.SelectedIndex == 2)
                    { GetUrlSource("https://bdobosstimer.com/?&server=" + MOTR); }
                }
                if (ITRITimeLabel.Content.ToString() == "00:00:00")
                {
                    if (SourceComboBox.SelectedIndex == 0)
                    { GetLYPBBTTimeTable(); }
                    if (SourceComboBox.SelectedIndex == 1)
                    { GetTimeTable(MOTR); }//Get info from Html Code
                    if (SourceComboBox.SelectedIndex == 2)
                    { GetUrlSource("https://bdobosstimer.com/?&server=" + MOTR); }
                }
            }

            if (CurrentBossTimeLabel.Content.ToString() == "00:00:00")
            {
                if (SourceComboBox.SelectedIndex == 0)
                { GetLYPBBTTimeTable(); }
                if (SourceComboBox.SelectedIndex == 1)
                { GetTimeTable(MOTR); }//Get info from Html Code
                if (SourceComboBox.SelectedIndex == 2)
                { GetUrlSource("https://bdobosstimer.com/?&server=" + MOTR); }
            }

            if (SaveLatestTimeTable == 0)
                {
                    try
                    {
                        try { gridview1.SelectedIndex = SharedDay; } catch (Exception) { }
                        if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"))
                        { File.Delete(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"); }
                        int width = int.Parse(gridview1.ActualWidth.ToString());
                        int height = int.Parse(gridview1.ActualHeight.ToString());
                        RenderTargetBitmap renderTargetBitmap =
                       new RenderTargetBitmap(1920, 1080, 180, 250, PixelFormats.Pbgra32);
                        renderTargetBitmap.Render(gridview1);
                        PngBitmapEncoder pngImage = new PngBitmapEncoder();
                        pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                        using (Stream fileStream = File.Create(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"))
                        {
                            pngImage.Save(fileStream);
                        }
                        SaveLatestTimeTable = 1;
                    }
                    catch (Exception) { }
            }


            if (discord.ConnectionState == Discord.ConnectionState.Connected && isposting == 1)
            {
                if (discord.ConnectionState == Discord.ConnectionState.Connected && intervalMessageUpdate == UpdateMesssageInterval)
                {
                    try { EditMessage(); } catch (Exception) { }
                }
                if (intervalMessageUpdate >= UpdateMesssageInterval)
                { intervalMessageUpdate = 0; }
                intervalMessageUpdate++;                            
            }
        }    
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)//Move Window Using any Ui objects
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
            catch (Exception) { }
        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)// loop BackVid
        {
            mediaElement.Position = new TimeSpan(0, 0, 0);
            mediaElement.Play();
        }

        public DataGridCell GetCell(int rowIndex, int columnIndex, DataGrid dg)// future Code Usage For CustomTimeTables
        {
            DataGridRow row = dg.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            DataGridCellsPresenter p = GetVisualChild<DataGridCellsPresenter>(row);
            DataGridCell cell = p.ItemContainerGenerator.ContainerFromIndex(columnIndex) as DataGridCell;
            return cell;
        }

        static T GetVisualChild<T>(Visual parent) where T : Visual // future Code Usage For CustomTimeTables
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        public void Count()// future Code Usage For CustomTimeTables
        {
            timer.Stop();
            System.Threading.Thread.Sleep(2000);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;

            List<string> TimeBored = new List<string>();
            DataRow dr1 = TimeTable.Rows[0];
            int v = 0;
            foreach (var column in TimeTable.Columns.Cast<DataColumn>().ToArray())
            {
                if (v > 0)
                { TimeBored.Add(dr1[v].ToString()); }
                v++;
            }
            int MaxC = 0;
            int NextB = 0;
            int FinalB = -1;
            DateTime PT = DateTime.Now;
            DateTime PTC = TimeZone.CurrentTimeZone.ToUniversalTime(PT);
            foreach (var i in TimeBored)
            {
                DateTime CC = DateTime.Parse(TimeBored[NextB].ToString());
                DateTime CCC = TimeZone.CurrentTimeZone.ToUniversalTime(CC);
                if (CCC > PTC)
                { FinalB = NextB; }
                if (FinalB == -1)
                { NextB++; }
                MaxC++;
            }
            if (FinalB > 0)
            {
                DateTime PB = DateTime.Parse(TimeBored[FinalB - 1].ToString());
                PBT = TimeZone.CurrentTimeZone.ToUniversalTime(PB);
            }
            DateTime CB = DateTime.Parse(TimeBored[FinalB].ToString());
            CBT = TimeZone.CurrentTimeZone.ToUniversalTime(CB);
            if (FinalB < MaxC)
            {
                if (FinalB + 1 < MaxC)
                {
                    DateTime NB = DateTime.Parse(TimeBored[FinalB + 1].ToString());
                    NBT = TimeZone.CurrentTimeZone.ToUniversalTime(NB);
                }
                else
                {
                    DateTime NB = DateTime.Parse(TimeBored[0].ToString());
                    NBT = TimeZone.CurrentTimeZone.ToUniversalTime(NB);
                }
            }
            MainB = FinalB;
            PmaxC = MaxC;

            int CBI = FinalB + 1;
            int day = 0;
            for (int i = 0; i < 8; i++)
            {

                DataRow dr = TimeTable.Rows[i];
                if (dr[0].ToString() == DateTime.Today.DayOfWeek.ToString())
                { day = i; }
            }
            DataRow drV = TimeTable.Rows[day];
            if (drV[CBI].ToString() == "")
            {
                while (true)
                {
                    CBI++;
                    if (drV[CBI].ToString() != "")
                    { break; }
                }
            }

            if (CBI > 0)
            {
                DataRow dr2 = TimeTable.Rows[day];
                PbossNamLabel.Content = dr2[CBI - 1].ToString();
                if (PbossNamLabel.Content.ToString().Contains(","))
                { PbossNamLabel.Content = PbossNamLabel.Content.ToString().Replace("," + Environment.NewLine, " & "); }
            }
            else
            {
                PbossNamLabel.Content = "Prevoius Boss";
            }
            DataRow dr3 = TimeTable.Rows[day];
            CbossNameLabel.Content = dr3[CBI].ToString();
            if (CbossNameLabel.Content.ToString().Contains(","))
            { CbossNameLabel.Content = CbossNameLabel.Content.ToString().Replace("," + Environment.NewLine, " & "); }
            if (CBI < TimeTable.Columns.Count - 1)
            {
                DataRow dr4 = TimeTable.Rows[day];
                NBossNameLabel.Content = dr4[CBI + 1].ToString();
                if (NBossNameLabel.Content.ToString().Contains(","))
                { NBossNameLabel.Content = NBossNameLabel.Content.ToString().Replace("," + Environment.NewLine, " & "); }
            }
            else
            {
                NBossNameLabel.Content = "Followed Boss";
            }

            SharedDay = day;
            SharedTime = CBI;
            DateTime TN = DateTime.Now;


            timer.Start();

        }

        void timer_Tick(object sender, EventArgs e)// future Code Usage For CustomTimeTables
        {
            DateTime PT = DateTime.Now;
            DateTime PTC = TimeZone.CurrentTimeZone.ToUniversalTime(PT);

            if (MainB > 0)
            {

                TimeSpan PFTC = PBT.Subtract(PTC);
                PreviousBossTimeLabel.Content = "-" + PFTC.ToString(@"hh\:mm\:ss");
            }
            else
            {
                PreviousBossTimeLabel.Content = "00:00:00";
            }
            TimeSpan FTC = CBT.Subtract(PTC);
            CurrentBossTimeLabel.Content = FTC.ToString(@"hh\:mm\:ss");
            if (MainB < PmaxC)
            {
                TimeSpan NFTC = NBT.Subtract(PTC);
                NextBossTimeLabel.Content = NFTC.ToString(@"hh\:mm\:ss");
            }
            else
            {
                NextBossTimeLabel.Content = "00:00:00";
            }
            if (FTC.ToString(@"hh\:mm\:ss") == "00:00:00")
            {
                try
                {
                    DataGridCell cell = GetCell(SharedDay, SharedTime, gridview1);
                    var bc = new BrushConverter();
                    cell.Background = (Brush)bc.ConvertFrom("#E51E2129");
                }
                catch (Exception) { }
                Count();
            }


            try
            {
                DataGridCell cell = GetCell(SharedDay, SharedTime, gridview1);
                var bc = new BrushConverter();
                cell.Background = (Brush)bc.ConvertFrom("#FF767676");
            }
            catch (Exception) { }

        }

        private void RegionComboBox_DropDownClosed(object sender, EventArgs e)//update dropbox content
        {
            if (RegionComboBox.Text != MOTR.ToUpper())
            {
                Settings.Default["DefaultRegion"] = RegionComboBox.Text.ToLower();
                Settings.Default.Save();
                MOTR = RegionComboBox.Text.ToLower();
                if (SourceComboBox.SelectedIndex == 0)
                { GetLYPBBTTimeTable(); }
                if (SourceComboBox.SelectedIndex == 1)
                { GetTimeTable(RegionComboBox.Text.ToLower()); }
                if (SourceComboBox.SelectedIndex == 2)
                { GetUrlSource("https://bdobosstimer.com/?&server=" + RegionComboBox.Text.ToLower()); }
            }
        }

        private void gridview1_GotFocus(object sender, RoutedEventArgs e)// select TimezoneDay on focus
        {
            try
            {
                //gridview1.SelectedIndex = SharedDay;
            }
            catch (Exception) { }
        }
        private void AutoSaveBossData()
        {
            int i = 0;
            int r = 0;
            if(SRFTBCombobox.SelectedIndex == -1)
            { SRFTBCombobox.SelectedIndex = 0; }
            foreach (string item in BossesCollection)
            {
                string bossName = item.Substring(0, item.IndexOf(",") + 1);
                bossName = bossName.Replace(",", "");
                bossName = bossName.Replace(Environment.NewLine, "");
                if (bossName.ToLower() == AddSaveBossNameTextBox.Text.ToLower())
                {
                    r = i;
                }
                i++;
            }
            BossesCollection[r] = AddSaveBossNameTextBox.Text + "," + DisplayImageLinkextBox.Text + "," + BossSpawnLocationLinkTextBox.Text + "," + CBNTextbox.Text + "," + SRFTBCombobox.Text + "{" + RolesCollection[SRFTBCombobox.SelectedIndex].ToString() + "," + DisplayImageLinkextBoxLocal.Text;
        }
        private void AddnewBossTestImgLinkButton_Click(object sender, RoutedEventArgs e)// test image url loadasync() code base
        {
            Getimg gm = new Getimg();
            DisplayImageLinkextBox.BorderBrush = Brushes.Silver;
            AddSaveBossPictureBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/Boss.png"));
            string imgurl = DisplayImageLinkextBox.Text;
            //if (imgurl.Contains("<Local>"))
            //{ imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
            try { AddSaveBossPictureBox.Source = gm.GETIMAGE(imgurl); } catch (Exception) { DisplayImageLinkextBox.BorderBrush = Brushes.Red; }
        }      
        private void RemoveBossButton_Click(object sender, RoutedEventArgs e)//remove selected boss from listbox
        {
            if (BossCollectionListBox.SelectedIndex > -1)
            {
                BossesCollection.RemoveAt(BossCollectionListBox.SelectedIndex);
                BossCollectionListBox.Items.RemoveAt(BossCollectionListBox.SelectedIndex);
                try { BossCollectionListBox.SelectedIndex = 0; } catch (Exception) { }
                Trigger_bossSelection();
            }
        }

        private async void BossesListButton_Click(object sender, RoutedEventArgs e)//change tabcontrol
        {
            if (discord.ConnectionState == Discord.ConnectionState.Connected)
            {
                if (tabcontrol1.SelectedIndex == 3)
                { SaveGlobalSettings(); }
                if (SourceComboBox.SelectedIndex == 0 && tabcontrol1.SelectedIndex == 1)
                { Save_LYPBBTTT(); GetLYPBBTTimeTable(); }
                GetIds();
                TimeTableGrid.Visibility = Visibility.Hidden;

                try { BossCollectionListBox.SelectedIndex = 0; Selectected_boss = BossCollectionListBox.SelectedIndex; } catch (Exception) { }
                try
                {
                    gridview1.SelectedIndex = SharedDay;
                    DoubleAnimation da = new DoubleAnimation();
                    da.From = 0;
                    da.To = 1;
                    da.Duration = new Duration(TimeSpan.FromSeconds(1));
                    da.AutoReverse = false;
                    tabcontrol1.BeginAnimation(OpacityProperty, da);
                }
                catch (Exception) { }
                tabcontrol1.SelectedIndex = 2;
                var bc = new BrushConverter();
                Homehighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                Timetablehighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                bosslisthighlight.Fill = (Brush)bc.ConvertFrom(subcolor);
                SelfRollingHighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                settingshighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                abouthighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                if (BossCollectionListBox.Items.Count > 0)
                {
                    try
                    {
                        Getimg gm = new Getimg();
                        DisplayImageLinkextBox.BorderBrush = (Brush)bc.ConvertFrom("#FF434349");                       
                        RemoveBossButton.Visibility = Visibility.Visible;

                        foreach (string item in BossesCollection)
                        {
                            string bossName = item.Substring(0, item.IndexOf(",") + 1);
                            bossName = bossName.Replace(",", "");
                            bossName = bossName.Replace(Environment.NewLine, "");
                            if (bossName.ToString() == BossCollectionListBox.SelectedItem.ToString())
                            {
                                string[] bossdata = item.Split(',');
                                AddSaveBossNameTextBox.Text = bossdata[0].ToString();
                                DisplayImageLinkextBox.Text = bossdata[1].ToString();
                                DisplayImageLinkextBoxLocal.Text = bossdata[5].ToString();
                                AddSaveBossPictureBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/Boss.png"));
                                if (bossdata[5].ToString() == "")
                                {
                                    string imgurl = bossdata[1].ToString();
                                    //if (imgurl.Contains("<Local>"))
                                    //{ imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
                                    try { AddSaveBossPictureBox.Source = gm.GETIMAGE(imgurl); } catch (Exception) { DisplayImageLinkextBox.BorderBrush = Brushes.Red; }
                                }
                                else
                                {
                                    string imgurl = bossdata[5].ToString();
                                    if (imgurl.Contains("<Local>"))
                                    { imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
                                    try { AddSaveBossPictureBox.Source = gm.GETIMAGE(imgurl); } catch (Exception) { DisplayImageLinkextBoxLocal.BorderBrush = Brushes.Red; }
                                }
                                BossSpawnLocationLinkTextBox.Text = bossdata[2].ToString();
                                CBNTextbox.Text = bossdata[3].ToString();
                                string[] br = bossdata[4].ToString().Split('{');
                                SRFTBCombobox.Text = br[0].ToString();
                                if (SRFTBCombobox.SelectedIndex == -1)
                                {
                                    if (br[1].ToString() != "")
                                    {
                                        try
                                        {
                                            var guild = discord.GetGuild(ServerID);
                                            ulong Role_id = ulong.Parse(br[1].ToString());
                                            var Role = guild.GetRole(Role_id);
                                            SRFTBCombobox.Text = "@" + Role.Name.ToString();
                                        }
                                        catch (Exception) { SRFTBCombobox.Text = "None"; }
                                    }
                                    else
                                    { SRFTBCombobox.Text = "None"; }
                                }
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
            else
            {
                try
                {
                    var bc = new BrushConverter();

                    DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFF1F1F1");
                    DiscordBotConnectionStatusLabel.Content = LanguageCollection[6].ToString();//"Connecting...";
                    await discord.LoginAsync(TokenType.Bot, Token);
                    await discord.StartAsync();
                    System.Threading.Thread.Sleep(1000);

                    if (DiscordBotConnectionStatusLabel.Content.ToString() == LanguageCollection[6].ToString())
                    {
                        DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FF669174");
                        DiscordBotConnectionStatusLabel.Content = LanguageCollection[8].ToString();//"Connected";
                    }
                }
                catch (Exception)
                {
                    var bc = new BrushConverter();
                    DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFBB3D3D");
                    DiscordBotConnectionStatusLabel.Content = LanguageCollection[7].ToString();//"Connection ERROR!";
                    ErrorMessageBox emb = new ErrorMessageBox(LanguageCollection[119].ToString(), LanguageCollection[120].ToString(), LanguageCollection[121].ToString(), LanguageCollection[122].ToString());
                    emb.TestToken(LanguageCollection[112].ToString(), discord, this, Token);
                    emb.Show();
                    this.IsEnabled = false;

                    /*StartPosting();*/
                }
            }
        }

        private void TimeTableButton_Click(object sender, RoutedEventArgs e)//change tabcontrol
        {
            RemoveTimeTableColumn.Visibility = Visibility.Hidden;
            if (tabcontrol1.SelectedIndex == 3)
            { SaveGlobalSettings(); }
            if (tabcontrol1.SelectedIndex == 2)
            { savebosslist(); }
            EditTimeTableGrid.Visibility = Visibility.Hidden;
            if (SourceComboBox.SelectedIndex == 0)
            { EditTimeTableGrid.Visibility = Visibility.Visible; gridviewbox_Copy1.Visibility = Visibility.Hidden; }
            if(SourceComboBox.SelectedIndex > 0)
            { gridviewbox_Copy1.Visibility = Visibility.Visible; }
            TimeTableLabel.Content = MOTR.ToUpper() + LanguageCollection[43].ToString();                      
            try
            {
                gridview1Column = 1;
                gridview1.SelectedIndex = SharedDay;
                DoubleAnimation da = new DoubleAnimation();
                da.From = 0;
                da.To = 1;
                da.Duration = new Duration(TimeSpan.FromSeconds(1));
                da.AutoReverse = false;
                tabcontrol1.BeginAnimation(OpacityProperty, da);
                TimeTableGrid.BeginAnimation(OpacityProperty, da);
            }
            catch (Exception) { }
            TimeTableGrid.Visibility = Visibility.Visible;
            tabcontrol1.SelectedIndex = 1;
            var bc = new BrushConverter();
            Homehighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            Timetablehighlight.Fill = (Brush)bc.ConvertFrom(subcolor);
            SelfRollingHighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            bosslisthighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            settingshighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            abouthighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
        }
      
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)// make sure only numbers filled in linked textbox's
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #region "Alarms Settings"
        private void BossSpawnAlarmCheckBox1_Click(object sender, RoutedEventArgs e)
        {
            SaveBossSapwnAlrarmSettings();
        }
        private void SaveBossSapwnAlrarmSettings()
        {
            if (BossSpawnAlarm1Textbox.Text == "")
            { BossSpawnAlarm1Textbox.Text = "30"; }
            if (BossSpawnAlarm2Textbox.Text == "")
            { BossSpawnAlarm2Textbox.Text = "15"; }
            if (BossSpawnAlarm3Textbox.Text == "")
            { BossSpawnAlarm3Textbox.Text = "05"; }

            if (BossSpawnAlarm1Textbox.Text == "0")
            { BossSpawnAlarm1Textbox.Text = "30"; }
            if (BossSpawnAlarm2Textbox.Text == "0")
            { BossSpawnAlarm2Textbox.Text = "15"; }
            if (BossSpawnAlarm3Textbox.Text == "0")
            { BossSpawnAlarm3Textbox.Text = "05"; }

            if (int.Parse(BossSpawnAlarm1Textbox.Text) < 10)
            { BossSpawnAlarm1Textbox.Text = "0" + int.Parse(BossSpawnAlarm1Textbox.Text); }
            if (int.Parse(BossSpawnAlarm2Textbox.Text) < 10)
            { BossSpawnAlarm2Textbox.Text = "0" + int.Parse(BossSpawnAlarm2Textbox.Text); }
            if (int.Parse(BossSpawnAlarm3Textbox.Text) < 10)
            { BossSpawnAlarm3Textbox.Text = "0" + int.Parse(BossSpawnAlarm3Textbox.Text); }

            string bitResult = "";
            if (BossSpawnAlarmCheckBox1.IsChecked == true)
            { bitResult += BossSpawnAlarm1Textbox.Text + "," + "1,"; }
            else
            { bitResult += BossSpawnAlarm1Textbox.Text + "," + "0,"; }

            if (BossSpawnAlarmCheckBox2.IsChecked == true)
            { bitResult += BossSpawnAlarm2Textbox.Text + "," + "1,"; }
            else
            { bitResult += BossSpawnAlarm2Textbox.Text + "," + "0,"; }

            if (BossSpawnAlarmCheckBox3.IsChecked == true)
            { bitResult += BossSpawnAlarm3Textbox.Text + "," + "1"; }
            else
            { bitResult += BossSpawnAlarm3Textbox.Text + "," + "0"; }


            Settings.Default["BossSpawnAlarmCheckBox"] = bitResult;
            Settings.Default.Save();
        }

        private void BossSpawnAlarm1Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveBossSapwnAlrarmSettings();
        }

        private void BossSpawnAlarm2Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveBossSapwnAlrarmSettings();
        }

        private void BossSpawnAlarmCheckBox2_Click(object sender, RoutedEventArgs e)
        {
            SaveBossSapwnAlrarmSettings();
        }

        private void BossSpawnAlarm3Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveBossSapwnAlrarmSettings();
        }

        private void BossSpawnAlarmCheckBox3_Click(object sender, RoutedEventArgs e)
        {
            SaveBossSapwnAlrarmSettings();
        }
        private void SaveNightTimeAlrarmSettings()
        {
            if (NightTimeAlarm1Textbox.Text == "")
            { NightTimeAlarm1Textbox.Text = "30"; }
            if (NightTimeAlarm2Textbox.Text == "")
            { NightTimeAlarm2Textbox.Text = "15"; }
            if (NightTimeAlarm3Textbox.Text == "")
            { NightTimeAlarm3Textbox.Text = "05"; }

            if (NightTimeAlarm1Textbox.Text == "0")
            { NightTimeAlarm1Textbox.Text = "30"; }
            if (NightTimeAlarm2Textbox.Text == "0")
            { NightTimeAlarm2Textbox.Text = "15"; }
            if (NightTimeAlarm3Textbox.Text == "0")
            { NightTimeAlarm3Textbox.Text = "05"; }

            if (int.Parse(NightTimeAlarm1Textbox.Text) < 10)
            { NightTimeAlarm1Textbox.Text = "0" + int.Parse(NightTimeAlarm1Textbox.Text); }
            if (int.Parse(NightTimeAlarm2Textbox.Text) < 10)
            { NightTimeAlarm2Textbox.Text = "0" + int.Parse(NightTimeAlarm2Textbox.Text); }
            if (int.Parse(NightTimeAlarm3Textbox.Text) < 10)
            { NightTimeAlarm3Textbox.Text = "0" + int.Parse(NightTimeAlarm3Textbox.Text); }

            string bitResult = "";
            if (NightTimeAlarmCheckBox1.IsChecked == true)
            { bitResult += NightTimeAlarm1Textbox.Text + "," + "1,"; }
            else
            { bitResult += NightTimeAlarm1Textbox.Text + "," + "0,"; }

            if (NightTimeAlarmCheckBox2.IsChecked == true)
            { bitResult += NightTimeAlarm2Textbox.Text + "," + "1,"; }
            else
            { bitResult += NightTimeAlarm2Textbox.Text + "," + "0,"; }

            if (NightTimeAlarmCheckBox3.IsChecked == true)
            { bitResult += NightTimeAlarm3Textbox.Text + "," + "1"; }
            else
            { bitResult += NightTimeAlarm3Textbox.Text + "," + "0"; }


            Settings.Default["NightTimeAlarmCheckBox"] = bitResult;
            Settings.Default.Save();
        }

        private void NightTimeAlarm1Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveNightTimeAlrarmSettings();
        }

        private void NightTimeAlarmCheckBox1_Click(object sender, RoutedEventArgs e)
        {
            SaveNightTimeAlrarmSettings();
        }

        private void NightTimeAlarm2Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveNightTimeAlrarmSettings();
        }

        private void NightTimeAlarmCheckBox2_Click(object sender, RoutedEventArgs e)
        {
            SaveNightTimeAlrarmSettings();
        }

        private void NightTimeAlarm3Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveNightTimeAlrarmSettings();
        }

        private void NightTimeAlarmCheckBox3_Click(object sender, RoutedEventArgs e)
        {
            SaveNightTimeAlrarmSettings();
        }

        private void SaveImperialResetAlrarmSettings()
        {
            if (ImperialResetAlarm1Textbox.Text == "")
            { ImperialResetAlarm1Textbox.Text = "30"; }
            if (ImperialResetAlarm2Textbox.Text == "")
            { ImperialResetAlarm2Textbox.Text = "15"; }
            if (ImperialResetAlarm3Textbox.Text == "")
            { ImperialResetAlarm3Textbox.Text = "05"; }

            if (ImperialResetAlarm1Textbox.Text == "0")
            { ImperialResetAlarm1Textbox.Text = "30"; }
            if (ImperialResetAlarm2Textbox.Text == "0")
            { ImperialResetAlarm2Textbox.Text = "15"; }
            if (ImperialResetAlarm3Textbox.Text == "0")
            { ImperialResetAlarm3Textbox.Text = "05"; }

            if (int.Parse(ImperialResetAlarm1Textbox.Text) < 10)
            { ImperialResetAlarm1Textbox.Text = "0" + int.Parse(ImperialResetAlarm1Textbox.Text); }
            if (int.Parse(ImperialResetAlarm2Textbox.Text) < 10)
            { ImperialResetAlarm2Textbox.Text = "0" + int.Parse(ImperialResetAlarm2Textbox.Text); }
            if (int.Parse(ImperialResetAlarm3Textbox.Text) < 10)
            { ImperialResetAlarm3Textbox.Text = "0" + int.Parse(ImperialResetAlarm3Textbox.Text); }

            string bitResult = "";
            if (ImperialResetCheckBox1.IsChecked == true)
            { bitResult += ImperialResetAlarm1Textbox.Text + "," + "1,"; }
            else
            { bitResult += ImperialResetAlarm1Textbox.Text + "," + "0,"; }

            if (ImperialResetCheckBox2.IsChecked == true)
            { bitResult += ImperialResetAlarm2Textbox.Text + "," + "1,"; }
            else
            { bitResult += ImperialResetAlarm2Textbox.Text + "," + "0,"; }

            if (ImperialResetCheckBox3.IsChecked == true)
            { bitResult += ImperialResetAlarm3Textbox.Text + "," + "1"; }
            else
            { bitResult += ImperialResetAlarm3Textbox.Text + "," + "0"; }


            Settings.Default["ImperialResetCheckBox"] = bitResult;
            Settings.Default.Save();
        }

        private void ImperialResetAlarm1Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveImperialResetAlrarmSettings();
        }

        private void ImperialResetCheckBox1_Click(object sender, RoutedEventArgs e)
        {
            SaveImperialResetAlrarmSettings();
        }

        private void ImperialResetAlarm2Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveImperialResetAlrarmSettings();
        }

        private void ImperialResetCheckBox2_Click(object sender, RoutedEventArgs e)
        {
            SaveImperialResetAlrarmSettings();
        }

        private void ImperialResetAlarm3Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveImperialResetAlrarmSettings();
        }

        private void ImperialResetCheckBox3_Click(object sender, RoutedEventArgs e)
        {
            SaveImperialResetAlrarmSettings();
        }
        #endregion
        private void CloseappButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      
        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)// on closing action disconnect discord and animate fade out
        {
            try
            {
                var guild = discord.GetGuild(ServerID);
                var channel = guild.GetTextChannel(Main_BotChannel_ID);
                var messages = await channel.GetMessagesAsync(100).FlattenAsync(); //defualt is 100
                await (channel as SocketTextChannel).DeleteMessagesAsync(messages);
            }
            catch (Exception) { }
            await discord.StopAsync();
            omw.Close();
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(1));
            anim.Completed += (s, _) => System.Environment.Exit(1);
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private void SendTotrayButton_Click(object sender, RoutedEventArgs e)//send app to tray
        {
            this.WindowState = WindowState.Minimized;
            this.ShowInTaskbar = false;
            MyNotifyIcon.Visible = true;
        }
        void MyNotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)// restore from tray by double clicking icon in tray
        {
            MyNotifyIcon.Visible = false;
            this.ShowInTaskbar = true;
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            this.BeginAnimation(OpacityProperty, da);
            this.WindowState = WindowState.Normal;
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
        }

        private void appRestartButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/ExternalAppRestart.exe");
        }

        private void ConnectDiscordBotButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discordapp.com/oauth2/authorize?&client_id=" + ClientID + "&scope=bot&permissions=8");
        }
        private async void DisconnectDiscordBot_Click(object sender, RoutedEventArgs e)
        {
            if (discord.ConnectionState == Discord.ConnectionState.Connected)
            {
                isposting = 0;
                //var bc = new BrushConverter();
                //await discord.StopAsync();
                //DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFBB3D3D");
                //DiscordBotConnectionStatusLabel.Content = LanguageCollection[5].ToString();           
                try
                {
                    var guild = discord.GetGuild(ServerID);
                    var channel = guild.GetTextChannel(Main_BotChannel_ID);
                    var messages = await channel.GetMessagesAsync(100).FlattenAsync(); //defualt is 100
                    await (channel as SocketTextChannel).DeleteMessagesAsync(messages);
                }
                catch (Exception) { }
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    Discord_Bot Bot = new Discord_Bot();
                    Bot.GetDiscordData(this);
                }).Start();
            }
        }
        private async void GetIds()
        {           
            UTCombobox.SelectedIndex = -1;
            UTCombobox.Items.Clear();

            BSPRComboBox.SelectedIndex = -1;
            BSPRComboBox.Items.Clear();
            NTPRComboBox.SelectedIndex = -1;
            NTPRComboBox.Items.Clear();
            IRPRComboBox.SelectedIndex = -1;
            IRPRComboBox.Items.Clear();
            BRPRComboBox.SelectedIndex = -1;
            BRPRComboBox.Items.Clear();
            ITRPRComboBox.SelectedIndex = -1;
            ITRPRComboBox.Items.Clear();
            SelRollingRoleComboBox.Items.Clear();
            SelRollingRoleComboBox.SelectedIndex = -1;
            SidCombobox.SelectedIndex = -1;
            SidCombobox.Items.Clear();
            ChidCombobox.SelectedIndex = -1;
            ChidCombobox.Items.Clear();
            AlertChidCombobox.Items.Clear();
            RollingChannelComboBox.Items.Clear();
            AlertChidCombobox.SelectedIndex = -1;
            SRFTBCombobox.Items.Clear();
            SRFTBCombobox.SelectedIndex = -1;

            ServersCollection.Clear();
            ChannelsCollection.Clear();
            AlertChannelsCollection.Clear();
            RolesCollection.Clear();
            SelfRollingRolesCollections.Clear();

            SidCombobox.Items.Add("None");
            ServersCollection.Add("0");
            ChidCombobox.Items.Add("None");
            AlertChidCombobox.Items.Add("None");
            RollingChannelComboBox.Items.Add("None");
            ChannelsCollection.Add("0");
            AlertChannelsCollection.Add("0");
            RolesCollection.Add("");
            RolesCollection.Add("@everyone");
            RolesCollection.Add("@here");
            BSPRComboBox.Items.Add("None");
            BSPRComboBox.Items.Add("@everyone");
            BSPRComboBox.Items.Add("@here");
            NTPRComboBox.Items.Add("None");
            NTPRComboBox.Items.Add("@everyone");
            NTPRComboBox.Items.Add("@here");
            IRPRComboBox.Items.Add("None");
            IRPRComboBox.Items.Add("@everyone");
            IRPRComboBox.Items.Add("@here");
            BRPRComboBox.Items.Add("None");
            BRPRComboBox.Items.Add("@everyone");
            BRPRComboBox.Items.Add("@here");
            ITRPRComboBox.Items.Add("None");
            ITRPRComboBox.Items.Add("@everyone");
            ITRPRComboBox.Items.Add("@here");
            SRFTBCombobox.Items.Add("None");
            SRFTBCombobox.Items.Add("@everyone");
            SRFTBCombobox.Items.Add("@here");
            int SV = 2;
            while (true)
            {
                if (SV < 60)
                { UTCombobox.Items.Add(SV.ToString()); }

                SV++;
                if (SV > 50)
                { break; }
            }
            //IReadOnlyCollection<SocketTextChannel> Channels = null;
            SocketGuild guild = null;
            if (discord.ConnectionState == Discord.ConnectionState.Connected)
            {
                Discord_Bot bot = new Discord_Bot();
               
                IReadOnlyCollection<SocketGuild> Servers = null;               
                Processing_Status(false, "Fetching Data...");               
                await Task.Run(() => { Servers = bot.Get_Guilds(this); });
                foreach(var server in Servers) { if(server.Id == ServerID) { guild = server; break; } }
                //Channels = guild.TextChannels;
                //await Task.Run(() => { Channels = bot.Get_Channels(this, guild); });
                //await Task.Run(() => { Roles = bot.Get_Roles(this, guild); });
                

                foreach (var Server in Servers)
                {
                    ServersCollection.Add(Server.Id.ToString());
                    SidCombobox.Items.Add(Server.ToString());
                }
                foreach (var Channel in guild.TextChannels)
                {
                    ChannelsCollection.Add(Channel.Id.ToString());
                    ChidCombobox.Items.Add(Channel.ToString());
                }
                foreach (var Role in guild.Roles)
                {
                    if (Role.ToString() != "@everyone")
                    {
                        RolesCollection.Add(Role.Id.ToString());
                        SelfRollingRolesCollections.Add(Role.Id.ToString());
                        SRFTBCombobox.Items.Add("@" + Role.ToString());
                        BSPRComboBox.Items.Add("@" + Role.ToString());
                        NTPRComboBox.Items.Add("@" + Role.ToString());
                        IRPRComboBox.Items.Add("@" + Role.ToString());
                        BRPRComboBox.Items.Add("@" + Role.ToString());
                        ITRPRComboBox.Items.Add("@" + Role.ToString());
                        SelRollingRoleComboBox.Items.Add("@" + Role.ToString());
                    }
                }
            }
            TimeTableGrid.Visibility = Visibility.Hidden;
            CidTextbox.Text = ClientID.ToString();
            TokenTextbox.Text = Token;

            string[] sid = ProfileCollection[3].ToString().Split('{');
            SidCombobox.Text = sid[0].ToString();
            if (SidCombobox.SelectedIndex == -1)
            {
                if (sid[1].ToString() != "")
                {
                    try
                    {
                        //ulong Server_id = ulong.Parse(sid[1].ToString());
                        //var Server = discord.GetGuild(Server_id);
                        SidCombobox.Text = guild.Name.ToString();
                    }
                    catch (Exception) { SidCombobox.Text = "None"; }
                }
                else
                { SidCombobox.Text = "None"; }
            }

            string[] cid = ProfileCollection[4].ToString().Split('{');
            ChidCombobox.Text = cid[0];
            if (ChidCombobox.SelectedIndex == -1)
            {
                if (cid[1].ToString() != "")
                {
                    try
                    {
                        //var guild = discord.GetGuild(ServerID);
                        SocketGuildChannel channel = guild.GetTextChannel(ulong.Parse(cid[1].ToString()));
                        foreach(var chn in guild.TextChannels) { if(chn.Id == ulong.Parse(cid[1].ToString())) { channel = chn; break; } }
                        ChidCombobox.Text = channel.Name.ToString();
                    }
                    catch (Exception) { ChidCombobox.Text = "None"; }
                }
                else
                { ChidCombobox.Text = "None"; }
            }
            foreach (var Channel in guild.TextChannels)
            {
                if (Channel.Id != ulong.Parse(ProfileCollection[4].ToString().Split('{')[1]))
                {
                    AlertChidCombobox.Items.Add(Channel.ToString());
                    AlertChannelsCollection.Add(Channel.Id.ToString());
                    RollingChannelComboBox.Items.Add(Channel.ToString());
                }
            }
            string[] Alertcid = ProfileCollection[12].ToString().Split('{');
            AlertChidCombobox.Text = Alertcid[0];
            if (AlertChidCombobox.SelectedIndex == -1)
            {
                if (Alertcid[1].ToString() != "")
                {
                    try
                    {
                        var channel = guild.GetTextChannel(ulong.Parse(Alertcid[1].ToString()));
                        AlertChidCombobox.Text = channel.Name.ToString();
                    }
                    catch (Exception) { AlertChidCombobox.Text = "None"; }
                }
                else
                { AlertChidCombobox.Text = "None"; }
            }
            string[] rollingId = GetStrBetweenTags(File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/SelfRolling"), "[Channel]", "[/Channel]").Split('{');
            RollingChannelComboBox.Text = rollingId[0];
            if (RollingChannelComboBox.SelectedIndex == -1)
            {
                if (rollingId[1].ToString() != "")
                {
                    try
                    {
                        var channel = guild.GetTextChannel(ulong.Parse(rollingId[1].ToString()));
                        RollingChannelComboBox.Text = channel.Name.ToString();
                    }
                    catch (Exception) { RollingChannelComboBox.Text = "None"; }
                }
                else
                { RollingChannelComboBox.Text = "None"; }
            }
            string[] bsr = ProfileCollection[5].ToString().Split('{');
            BSPRComboBox.Text = bsr[0].ToString();
            if (BSPRComboBox.SelectedIndex == -1)
            {
                if (bsr[1].ToString() != "")
                {
                    try
                    {
                        ulong Role_id = ulong.Parse(bsr[1].ToString());
                        var Role = guild.GetRole(Role_id);
                        BSPRComboBox.Text = "@" + Role.Name.ToString();
                    }
                    catch (Exception) { BSPRComboBox.Text = "None"; }
                }
                else
                { BSPRComboBox.Text = "None"; }
            }

            string[] ntr = ProfileCollection[6].ToString().Split('{');
            NTPRComboBox.Text = ntr[0].ToString();
            if (NTPRComboBox.SelectedIndex == -1)
            {
                if (ntr[1].ToString() != "")
                {
                    try
                    {
                        ulong Role_id = ulong.Parse(ntr[1].ToString());
                        var Role = guild.GetRole(Role_id);
                        NTPRComboBox.Text = "@" + Role.Name.ToString();
                    }
                    catch (Exception) { NTPRComboBox.Text = "None"; }
                }
                else
                { NTPRComboBox.Text = "None"; }
            }

            string[] irr = ProfileCollection[7].ToString().Split('{');
            IRPRComboBox.Text = irr[0].ToString();
            if (IRPRComboBox.SelectedIndex == -1)
            {
                if (irr[1].ToString() != "")
                {
                    try
                    {
                        ulong Role_id = ulong.Parse(irr[1].ToString());
                        var Role = guild.GetRole(Role_id);
                        IRPRComboBox.Text = "@" + Role.Name.ToString();
                    }
                    catch (Exception) { IRPRComboBox.Text = "None"; }
                }
                else
                { IRPRComboBox.Text = "None"; }
            }

            string[] brr = ProfileCollection[10].ToString().Split('{');
            BRPRComboBox.Text = brr[0].ToString();
            if (BRPRComboBox.SelectedIndex == -1)
            {
                if (brr[1].ToString() != "")
                {
                    try
                    {
                        ulong Role_id = ulong.Parse(brr[1].ToString());
                        var Role = guild.GetRole(Role_id);
                        BRPRComboBox.Text = "@" + Role.Name.ToString();
                    }
                    catch (Exception) { BRPRComboBox.Text = "None"; }
                }
                else
                { BRPRComboBox.Text = "None"; }
            }

            string[] itrr = ProfileCollection[11].ToString().Split('{');
            ITRPRComboBox.Text = itrr[0].ToString();
            if (ITRPRComboBox.SelectedIndex == -1)
            {
                if (itrr[1].ToString() != "")
                {
                    try
                    {
                        ulong Role_id = ulong.Parse(itrr[1].ToString());
                        var Role = guild.GetRole(Role_id);
                        ITRPRComboBox.Text = "@" + Role.Name.ToString();
                    }
                    catch (Exception) { ITRPRComboBox.Text = "None"; }
                }
                else
                { ITRPRComboBox.Text = "None"; }
            }

            UTCombobox.Text = UpdateMesssageInterval.ToString();
            if(UTCombobox.SelectedIndex == -1)
            { UTCombobox.Text = "2"; UpdateMesssageInterval = 2; }
            if (DefaultLanguage == "fr")
            { LanguageDropBox.Text = "Français"; }
            if (DefaultLanguage == "en")
            { LanguageDropBox.Text = "English"; }
            if (DefaultLanguage == "es")
            { LanguageDropBox.Text = "Español"; }
            if (DefaultLanguage == "ru")
            { LanguageDropBox.Text = "русский"; }
            if (DefaultLanguage == "jp")
            { LanguageDropBox.Text = "日本人"; }
            if (DefaultLanguage == "kr")
            { LanguageDropBox.Text = "한국어"; }

            language_changed = LanguageDropBox.SelectedIndex;
            Processing_Status(true, "Loading Complete.");
        }
        private async void SettingsButton_Click(object sender, RoutedEventArgs e)// load Profile settings on click
        {
            if (discord.ConnectionState == Discord.ConnectionState.Connected)
            {
                if (tabcontrol1.SelectedIndex == 2)
                { savebosslist(); }
                if (SourceComboBox.SelectedIndex == 0 && tabcontrol1.SelectedIndex == 1)
                { Save_LYPBBTTT(); GetLYPBBTTimeTable(); }
                GetIds();

                try
                {
                    DoubleAnimation da = new DoubleAnimation();
                    da.From = 0;
                    da.To = 1;
                    da.Duration = new Duration(TimeSpan.FromSeconds(1));
                    da.AutoReverse = false;
                    tabcontrol1.BeginAnimation(OpacityProperty, da);
                }
                catch (Exception) { }
                tabcontrol1.SelectedIndex = 3;
                var bc = new BrushConverter();
                Homehighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                Timetablehighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                bosslisthighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                SelfRollingHighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                settingshighlight.Fill = (Brush)bc.ConvertFrom(subcolor);
            }
            else
            {
                try
                {
                    var bc = new BrushConverter();

                    DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFF1F1F1");
                    DiscordBotConnectionStatusLabel.Content = LanguageCollection[6].ToString();//"Connecting...";
                    await discord.LoginAsync(TokenType.Bot, Token);
                    await discord.StartAsync();
                    System.Threading.Thread.Sleep(1000);

                    if (DiscordBotConnectionStatusLabel.Content.ToString() == LanguageCollection[6].ToString())
                    {
                        DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FF669174");
                        DiscordBotConnectionStatusLabel.Content = LanguageCollection[8].ToString();//"Connected";
                    }
                }
                catch (Exception)
                {
                    var bc = new BrushConverter();
                    DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFBB3D3D");
                    DiscordBotConnectionStatusLabel.Content = LanguageCollection[7].ToString();//"Connection ERROR!";
                    ErrorMessageBox emb = new ErrorMessageBox(LanguageCollection[119].ToString(), LanguageCollection[120].ToString(), LanguageCollection[121].ToString(), LanguageCollection[122].ToString());
                    emb.TestToken(LanguageCollection[112], discord, this, Token);
                    emb.Show();
                    this.IsEnabled = false;

                    /*StartPosting();*/
                }
            }
        }
        private void ChidCombobox_DropDownClosed(object sender, EventArgs e)
        {
            AlertChidCombobox.Items.Clear();
            AlertChannelsCollection.Clear();
            AlertChidCombobox.Items.Add("None");
            RollingChannelComboBox.Items.Add("None");
            AlertChannelsCollection.Add("0");
            RollingChannelComboBox.Items.Clear();
            foreach (var channel in discord.GetGuild(ServerID).TextChannels)
            {
                if (channel.Id != ulong.Parse(ChannelsCollection[ChidCombobox.SelectedIndex]))
                {
                    AlertChidCombobox.Items.Add(channel.ToString());
                    AlertChannelsCollection.Add(channel.Id.ToString());
                    RollingChannelComboBox.Items.Add(channel.ToString());
                }
            }
            if (ulong.Parse(ChannelsCollection[ChidCombobox.SelectedIndex]) != ulong.Parse(ProfileCollection[12].Split('{')[1]) && ChidCombobox.SelectedIndex != 0)
            {
                AlertChidCombobox.Text = ProfileCollection[12].Split('{')[0];
            }
            if (AlertChidCombobox.SelectedIndex == -1)
            { AlertChidCombobox.SelectedIndex = 0; }
        }
        private void SaveGlobalSettings()
        {
            ClientID = ulong.Parse(CidTextbox.Text);
            Token = TokenTextbox.Text;
            ServerID = ulong.Parse(ServersCollection[SidCombobox.SelectedIndex].ToString());
            Main_BotChannel_ID = ulong.Parse(ChannelsCollection[ChidCombobox.SelectedIndex].ToString());
            BossSpawnRole = RolesCollection[BSPRComboBox.SelectedIndex].ToString();
            Alert_BotChannel_ID = ulong.Parse(AlertChannelsCollection[AlertChidCombobox.SelectedIndex]);

            if (BossSpawnRole != "")
            {
                if (BossSpawnRole != "@everyone" && BossSpawnRole != "@here")
                { BossSpawnRole = "<@&" + BossSpawnRole + ">"; }
            }
            NightTimeRole = RolesCollection[NTPRComboBox.SelectedIndex].ToString();
            if (NightTimeRole != "")
            {
                if (NightTimeRole != "@everyone" && NightTimeRole != "@here")
                { NightTimeRole = "<@&" + NightTimeRole + ">"; }
            }
            ImperialResetRole = RolesCollection[IRPRComboBox.SelectedIndex].ToString();
            if (ImperialResetRole != "")
            {
                if (ImperialResetRole != "@everyone" && ImperialResetRole != "@here")
                { ImperialResetRole = "<@&" + ImperialResetRole + ">"; }
            }
            BarteringResetRole = RolesCollection[BRPRComboBox.SelectedIndex].ToString();
            if (BarteringResetRole != "")
            {
                if (BarteringResetRole != "@everyone" && BarteringResetRole != "@here")
                { BarteringResetRole = "<@&" + BarteringResetRole + ">"; }
            }
            ImperialTradingResetRole = RolesCollection[ITRPRComboBox.SelectedIndex].ToString();
            if (ImperialTradingResetRole != "")
            {
                if (ImperialTradingResetRole != "@everyone" && ImperialTradingResetRole != "@here")
                { ImperialTradingResetRole = "<@&" + ImperialTradingResetRole + ">"; }
            }
            UpdateMesssageInterval = int.Parse(UTCombobox.Text);
            //AnouncmentMessageInterval = int.Parse(RMMCombobox.Text);
            File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Profile", DefaultLanguage + "|" 
                + CidTextbox.Text + "|" 
                + TokenTextbox.Text + "|" 
                + SidCombobox.Text + "{" + ServersCollection[SidCombobox.SelectedIndex].ToString() + "|" 
                + ChidCombobox.Text+ "{" + ChannelsCollection[ChidCombobox.SelectedIndex].ToString() + "|" 
                + BSPRComboBox.Text + "{" + RolesCollection[BSPRComboBox.SelectedIndex].ToString() + "|" 
                + NTPRComboBox.Text + "{" + RolesCollection[NTPRComboBox.SelectedIndex].ToString() + "|" 
                + IRPRComboBox.Text + "{" + RolesCollection[IRPRComboBox.SelectedIndex].ToString() + "|" 
                + UTCombobox.Text + "|" 
                + "0" + "|" 
                + BRPRComboBox.Text + "{" + RolesCollection[BRPRComboBox.SelectedIndex].ToString() + "|"
                + ITRPRComboBox.Text + "{" + RolesCollection[ITRPRComboBox.SelectedIndex].ToString() + "|"
                + AlertChidCombobox.Text + "{" + AlertChannelsCollection[AlertChidCombobox.SelectedIndex]);

            ProfileCollection.Clear();
            ProfileCollection.Add(DefaultLanguage);
            ProfileCollection.Add(CidTextbox.Text);
            ProfileCollection.Add(TokenTextbox.Text);
            ProfileCollection.Add(SidCombobox.Text + "{" + ServersCollection[SidCombobox.SelectedIndex].ToString());
            ProfileCollection.Add(ChidCombobox.Text + "{" + ChannelsCollection[ChidCombobox.SelectedIndex].ToString());
            ProfileCollection.Add(BSPRComboBox.Text + "{" + RolesCollection[BSPRComboBox.SelectedIndex].ToString());
            ProfileCollection.Add(NTPRComboBox.Text + "{" + RolesCollection[NTPRComboBox.SelectedIndex].ToString());
            ProfileCollection.Add(IRPRComboBox.Text + "{" + RolesCollection[IRPRComboBox.SelectedIndex].ToString());
            ProfileCollection.Add(UTCombobox.Text);
            ProfileCollection.Add("0");
            ProfileCollection.Add(BRPRComboBox.Text + "{" + RolesCollection[BRPRComboBox.SelectedIndex].ToString());
            ProfileCollection.Add(ITRPRComboBox.Text + "{" + RolesCollection[ITRPRComboBox.SelectedIndex].ToString());
            ProfileCollection.Add(AlertChidCombobox.Text + "{" + AlertChannelsCollection[AlertChidCombobox.SelectedIndex]);

            //StartPosting();
            //FIX
            if (BSA1CMTextBox.Text == "")
            {
                BSA1CMTextBox.Text = LanguageCollection[76].ToString();
            }
            if (BSA1CMTextBox.Text == LanguageCollection[76].ToString())
            {
                Settings.Default["BSA1CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["BSA1CM"] = BSA1CMTextBox.Text;
                Settings.Default.Save();
            }

            if (BSA2CMTextBox.Text == "")
            {
                BSA2CMTextBox.Text = LanguageCollection[76].ToString();
            }
            if (BSA2CMTextBox.Text == LanguageCollection[76].ToString())
            {
                Settings.Default["BSA2CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["BSA2CM"] = BSA2CMTextBox.Text;
                Settings.Default.Save();
            }

            if (BSA3CMTextBox.Text == "")
            {
                BSA3CMTextBox.Text = LanguageCollection[76].ToString();
            }
            if (BSA3CMTextBox.Text == LanguageCollection[76].ToString())
            {
                Settings.Default["BSA3CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["BSA3CM"] = BSA3CMTextBox.Text;
                Settings.Default.Save();
            }


            if (NTA1CMTextbBox.Text == "")
            {
                NTA1CMTextbBox.Text = LanguageCollection[78].ToString();
            }
            if (NTA1CMTextbBox.Text == LanguageCollection[78].ToString())
            {
                Settings.Default["NTA1CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["NTA1CM"] = NTA1CMTextbBox.Text;
                Settings.Default.Save();
            }

            if (NTA2CMTextbBox.Text == "")
            {
                NTA2CMTextbBox.Text = LanguageCollection[78].ToString();
            }
            if (NTA2CMTextbBox.Text == LanguageCollection[78].ToString())
            {
                Settings.Default["NTA2CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["NTA2CM"] = NTA2CMTextbBox.Text;
                Settings.Default.Save();
            }

            if (NTA3CMTextbBox.Text == "")
            {
                NTA3CMTextbBox.Text = LanguageCollection[78].ToString();
            }
            if (NTA3CMTextbBox.Text == LanguageCollection[78].ToString())
            {
                Settings.Default["NTA3CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["NTA3CM"] = NTA3CMTextbBox.Text;
                Settings.Default.Save();
            }


            if (IRA1CMTextBox.Text == "")
            {
                IRA1CMTextBox.Text = LanguageCollection[80].ToString();
            }
            if (IRA1CMTextBox.Text == LanguageCollection[80].ToString())
            {
                Settings.Default["IRA1CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["IRA1CM"] = IRA1CMTextBox.Text;
                Settings.Default.Save();
            }

            if (IRA2CMTextBox.Text == "")
            {
                IRA2CMTextBox.Text = LanguageCollection[80].ToString();
            }
            if (IRA2CMTextBox.Text == LanguageCollection[80].ToString())
            {
                Settings.Default["IRA2CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["IRA2CM"] = IRA2CMTextBox.Text;
                Settings.Default.Save();
            }

            if (IRA3CMTextBox.Text == "")
            {
                IRA3CMTextBox.Text = LanguageCollection[80].ToString();
            }
            if (IRA3CMTextBox.Text == LanguageCollection[80].ToString())
            {
                Settings.Default["IRA3CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["IRA3CM"] = IRA3CMTextBox.Text;
                Settings.Default.Save();
            }


            if (BRA1CMTextBox.Text == "")
            {
                BRA1CMTextBox.Text = LanguageCollection[82].ToString();
            }
            if (BRA1CMTextBox.Text == LanguageCollection[82].ToString())
            {
                Settings.Default["BRA1CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["BRA1CM"] = BRA1CMTextBox.Text;
                Settings.Default.Save();
            }

            if (BRA2CMTextBox.Text == "")
            {
                BRA2CMTextBox.Text = LanguageCollection[82].ToString();
            }
            if (BRA2CMTextBox.Text == LanguageCollection[82].ToString())
            {
                Settings.Default["BRA2CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["BRA2CM"] = BRA2CMTextBox.Text;
                Settings.Default.Save();
            }

            if (BRA3CMTextBox.Text == "")
            {
                BRA3CMTextBox.Text = LanguageCollection[82].ToString();
            }
            if (BRA3CMTextBox.Text == LanguageCollection[82].ToString())
            {
                Settings.Default["BRA3CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["BRA3CM"] = BRA3CMTextBox.Text;
                Settings.Default.Save();
            }
            //FIX
            if (ITRA1CMTextBox.Text == "")
            {
                ITRA1CMTextBox.Text = LanguageCollection[84].ToString();
            }
            if (ITRA1CMTextBox.Text == LanguageCollection[84].ToString())
            {
                Settings.Default["ITRA1CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["ITRA1CM"] = ITRA1CMTextBox.Text;
                Settings.Default.Save();
            }

            if (ITRA2CMTextBox.Text == "")
            {
                ITRA2CMTextBox.Text = LanguageCollection[84].ToString();
            }
            if (ITRA2CMTextBox.Text == LanguageCollection[84].ToString())
            {
                Settings.Default["ITRA2CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["ITRA2CM"] = ITRA2CMTextBox.Text;
                Settings.Default.Save();
            }

            if (ITRA3CMTextBox.Text == "")
            {
                ITRA3CMTextBox.Text = LanguageCollection[84].ToString();
            }
            if (ITRA3CMTextBox.Text == LanguageCollection[84].ToString())
            {
                Settings.Default["ITRA3CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["ITRA3CM"] = ITRA3CMTextBox.Text;
                Settings.Default.Save();
            }
            tabcontrol1.SelectedIndex = 0;
            try
            {
                DoubleAnimation da = new DoubleAnimation();
                da.From = 0;
                da.To = 1;
                da.Duration = new Duration(TimeSpan.FromSeconds(1));
                da.AutoReverse = false;
                tabcontrol1.BeginAnimation(OpacityProperty, da);
            }
            catch (Exception) { }
           
        }

        private void HrdResetAppButton_Click(object sender, RoutedEventArgs e)// format app saved data
        {
            ErrorMessageBox emb = new ErrorMessageBox(LanguageCollection[119].ToString(), LanguageCollection[120].ToString(), LanguageCollection[121].ToString(), LanguageCollection[122].ToString());
            emb.MB_typeYN(LanguageCollection[113].ToString(), LanguageCollection[114].ToString(), 0, this);
            emb.Show();
            IsEnabled = false;
        }
        public async void confirmhardreset()
        {
            Settings.Default["DefaultRegion"] = "";
            Settings.Default["BossSpawnAlarmCheckBox"] = "";
            Settings.Default["NightTimeAlarmCheckBox"] = "";
            Settings.Default["ImperialResetCheckBox"] = "";
            Settings.Default["PlaySoundSetting"] = "";
            Settings.Default["OverlayTransparency"] = "";
            Settings.Default["OverlayState"] = "";
            Settings.Default["NTPlaySoundSetting"] = "";
            Settings.Default["IRPlaySoundSetting"] = "";
            Settings.Default["DisplayTimeTableSetting"] = "";
            Settings.Default["BSA1CM"] = "";
            Settings.Default["BSA2CM"] = "";
            Settings.Default["BSA3CM"] = "";
            Settings.Default["NTA1CM"] = "";
            Settings.Default["NTA2CM"] = "";
            Settings.Default["NTA3CM"] = "";
            Settings.Default["IRA1CM"] = "";
            Settings.Default["IRA2CM"] = "";
            Settings.Default["IRA3CM"] = "";
            Settings.Default["EditSpawnHoursSlider"] = "";
            Settings.Default["SettingKeepMessages"] = "";
            Settings.Default["AlarmonhoursCheckbox"] = "";
            Settings.Default["BRAlarmCheckBox"] = "";
            Settings.Default["ITRAlarmCheckBox"] = "";
            Settings.Default["ITRA1CM"] = "";
            Settings.Default["ITRA2CM"] = "";
            Settings.Default["ITRA3CM"] = "";
            Settings.Default["BRA1CM"] = "";
            Settings.Default["BRA2CM"] = "";
            Settings.Default["BRA3CM"] = "";
            Settings.Default["SelectedSource"] = "";
            Settings.Default["DefaultRegion1"] = "";
            Settings.Default["MainColor"] = "";
            Settings.Default["SubColor"] = "";
            Settings.Default["HeaderColor"] = "";
            Settings.Default["OriginBossList"] = "";
            Settings.Default["AnimatedBackgroundSource"] = "";
            Settings.Default["AnimatedBackgroundCheckbox"] = "";
            Settings.Default["BackgroundImageSource"] = "";
            Settings.Default["BackgroundImageCheckbox"] = "";
            Settings.Default["BRPlaySoundSetting"] = "";
            Settings.Default["ITRPlaySoundSetting"] = "";
            Settings.Default["Overlay_Position"] = "";
            Settings.Default["High_RenderQuality"] = "";
            Settings.Default["ScarletMode"] = "";
            Settings.Default["SelfRolling"] = "";
            Settings.Default["AutoUpdateTable"] = "";
            Settings.Default.Save();
            await discord.StopAsync();
            File.Delete(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Profile");
            File.Delete(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Bosses");
            File.Delete(System.IO.Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT");
            File.Delete(System.IO.Directory.GetCurrentDirectory() + "/Resources/SelfRolling");
            System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Launcher.exe");
            Close();
        }
       
        #region "Help Buttons"
        private void Button_Click(object sender, RoutedEventArgs e)
        { System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/img/Step2CID.mp4"); }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        { System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/img/Step4GetToken.mp4"); }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        { System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/img/ConvertRole.mp4"); }
        #endregion
        private void SoundOptionCheckBox_Click(object sender, RoutedEventArgs e)//save sound settings
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

        private void IoverlayModeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            this.ShowInTaskbar = false;
            //MyNotifyIcon.Visible = true;
            try
            {
                BitmapImage img = new BitmapImage();
                img = NBImageBox.Source as BitmapImage;
                omw = new DA_OverlayModWindow(this);
                omw.load(img);
                omw.Show();
                //omw.Visibility = Visibility.Visible;
            }
            catch (Exception) { omw.Show(); }
            overlayState = 1;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            OverlayTransperancyValueLabel.Content = TransparacySlider.Value.ToString();
            OverlayTransperancyValueLabel.Opacity = (TransparacySlider.Value) + 0.1;
            Settings.Default["OverlayTransparency"] = TransparacySlider.Value.ToString();
            Settings.Default.Save();
        }
        public void GetOverlayState()
        {
            overlayState = 0;
            MyNotifyIcon.Visible = false;
            this.ShowInTaskbar = true;

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

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            this.BeginAnimation(OpacityProperty, da);
            this.WindowState = WindowState.Normal;
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
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

        #region "max minutes allowed for alerts"
        private void BossSpawnAlarm1Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.Parse(BossSpawnAlarm1Textbox.Text) > 59)
                { BossSpawnAlarm1Textbox.Text = "59"; }
            }
            catch (Exception) { }
        }

        private void BossSpawnAlarm2Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.Parse(BossSpawnAlarm2Textbox.Text) > 59)
                { BossSpawnAlarm2Textbox.Text = "59"; }
            }
            catch (Exception) { }
        }

        private void BossSpawnAlarm3Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.Parse(BossSpawnAlarm3Textbox.Text) > 59)
                { BossSpawnAlarm3Textbox.Text = "59"; }
            }
            catch (Exception) { }
        }

        private void NightTimeAlarm1Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.Parse(NightTimeAlarm1Textbox.Text) > 59)
                { NightTimeAlarm1Textbox.Text = "59"; }
            }
            catch (Exception) { }
        }

        private void NightTimeAlarm2Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.Parse(NightTimeAlarm2Textbox.Text) > 59)
            { NightTimeAlarm2Textbox.Text = "59"; }
        }

        private void NightTimeAlarm3Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.Parse(NightTimeAlarm3Textbox.Text) > 59)
                { NightTimeAlarm3Textbox.Text = "59"; }
            }
            catch (Exception) { }
        }

        private void ImperialResetAlarm1Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.Parse(ImperialResetAlarm1Textbox.Text) > 59)
                { ImperialResetAlarm1Textbox.Text = "59"; }
            }
            catch (Exception) { }
        }

        private void ImperialResetAlarm2Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.Parse(ImperialResetAlarm2Textbox.Text) > 59)
                { ImperialResetAlarm2Textbox.Text = "59"; }
            }
            catch (Exception) { }
        }

        private void ImperialResetAlarm3Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.Parse(ImperialResetAlarm3Textbox.Text) > 59)
                { ImperialResetAlarm3Textbox.Text = "59"; }
            }
            catch (Exception) { }
        }
        #endregion

        private void EditMessage()//update Timers on discord
        {
            try
            {
                var guild = discord.GetGuild(ServerID);
                var channel = guild.GetTextChannel(Main_BotChannel_ID);
                var Message = MainMessage; /*await channel.GetMessageAsync(MainMessageID) as IUserMessage;*/
                string message = "";
                string nb = "> ```cs" + Environment.NewLine + "> " + CurrentBossTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```";
                string nti = "> ```cs" + Environment.NewLine + "> " + NightInBdoTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```";
                string iri = "> ```cs" + Environment.NewLine + "> " + IRTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```";
                string br = "> ```cs" + Environment.NewLine + "> " + BRTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```";
                string itr = "> ```cs" + Environment.NewLine + "> " + ITRITimeLabel.Content.ToString() + Environment.NewLine + "> " + "```";
                if (CurrentBossTimeLabel.Content.ToString().StartsWith("00:"))
                { nb = "> ```css" + Environment.NewLine + "> " + CurrentBossTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```"; }
                if (NightInBdoTimeLabel.Content.ToString().StartsWith("00:"))
                { nti = "> ```css" + Environment.NewLine + "> " + NightInBdoTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```"; }
                if (IRTimeLabel.Content.ToString().StartsWith("00:"))
                { iri = "> ```css" + Environment.NewLine + "> " + IRTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```"; }
                if (BRTimeLabel.Content.ToString().StartsWith("00:"))
                { br = "> ```css" + Environment.NewLine + "> " + BRTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```"; }
                if (ITRITimeLabel.Content.ToString().StartsWith("00:"))
                { itr = "> ```css" + Environment.NewLine + "> " + ITRITimeLabel.Content.ToString() + Environment.NewLine + "> " + "```"; }

                if (ScarletModeTimeTableSetting_CheckBox.IsChecked == true)
                { Update_ScarletMode(); }

                message = "> " + CbossLabel.Content.ToString() + Environment.NewLine
                    + nb + Environment.NewLine
                    + "> " + NILabel.Content.ToString() + Environment.NewLine
                    + nti + Environment.NewLine
                    + "> " + IRILabel.Content.ToString() + Environment.NewLine
                    + iri + Environment.NewLine
                    + "> " + BRILabel.Content.ToString() + Environment.NewLine
                    + br + Environment.NewLine
                    + "> " + ITRILabel.Content.ToString() + Environment.NewLine
                    + itr
                    + Environment.NewLine 
                    + Environment.NewLine
                    + Updated_ScarletMode_Message;
                //var MainMessage = await channel.SendMessageAsync(message);
                Message.ModifyAsync(msg => msg.Content = message);//must be non await to prevent over stack call
                MainMessage = Message;
                
            }
            catch (Exception) { }
        }      
        private async void StartPostingButton_Click(object sender, RoutedEventArgs e)
        {
            Discord_Bot Bot = new Discord_Bot();
            StartPostingButton.IsEnabled = false;
            await Task.Run(() => { Bot.StartPosting(this); Bot.StartPosting_Cooldown(this); });
            //MessageBox.Show(((GC.GetTotalMemory(true) / 1024f) / 1024f).ToString());
            //MessageBox.Show(BossesCollection.FirstOrDefault(s => s.Split(',')[0].ToLower() == "Nouver".ToLower()));
            //var guild = discord.GetGuild(ServerID);
            //var emojis = guild.Emotes;
            //string url = "";
            //foreach (var m in emojis)
            //{
            //    url = m.Url;
            //}
            //Getimg gm = new Getimg();
            //try { SSRR_Img.Source = gm.GETIMAGE(url); } catch (Exception) { }
        }

        private async void DisplayTimeTableSetting_Click(object sender, RoutedEventArgs e)// time table discplay option
        {
            if (discord.ConnectionState == Discord.ConnectionState.Connected && isposting == 1)
            {
                if (DisplayTimeTableSetting.IsChecked == true)
                {
                    if (TimtableID == 0)
                    {
                        try { gridview1.SelectedIndex = SharedDay; } catch (Exception) { }
                        if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"))
                        { File.Delete(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"); }
                        //int width = int.Parse(gridview1.ActualWidth.ToString());//error
                        //int height = int.Parse(gridview1.ActualHeight.ToString());
                        RenderTargetBitmap renderTargetBitmap =
                       new RenderTargetBitmap(1920, 1080, 180, 250, PixelFormats.Pbgra32);
                        renderTargetBitmap.Render(gridview1);
                        PngBitmapEncoder pngImage = new PngBitmapEncoder();
                        pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                        using (Stream fileStream = File.Create(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"))
                        {
                            pngImage.Save(fileStream);
                        }
                        var guild = discord.GetGuild(ServerID);
                        var channel = guild.GetTextChannel(Main_BotChannel_ID);
                        var timetablemessage = await channel.SendFileAsync(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png", MOTR.ToString().ToUpper() + LanguageCollection[43].ToString());
                        TimtableID = timetablemessage.Id;
                    }

                    Settings.Default["DisplayTimeTableSetting"] = "1";
                    Settings.Default.Save();
                }
                else
                {
                    if (TimtableID != 0)
                    {
                        var guild = discord.GetGuild(ServerID);
                        var channel = guild.GetTextChannel(Main_BotChannel_ID);
                        await channel.DeleteMessageAsync(TimtableID);
                        TimtableID = 0;
                    }

                    Settings.Default["DisplayTimeTableSetting"] = "0";
                    Settings.Default.Save();
                }
            }
            else
            {
                if (DisplayTimeTableSetting.IsChecked == true)
                {
                    Settings.Default["DisplayTimeTableSetting"] = "1";
                    Settings.Default.Save();
                }
                else
                {
                    Settings.Default["DisplayTimeTableSetting"] = "0";
                    Settings.Default.Save();
                }
            }
        }
        #region "Discord Notify By ping"
        private async void DiscordNotifyBossSpwn(string bossmessage)
        {
            if (discord.ConnectionState == Discord.ConnectionState.Connected && isposting == 1)
            {
                try
                {
                        if (bossmessage.Contains("<00:00:00>"))
                        { bossmessage = bossmessage.Replace("<00:00:00>", CurrentBossTimeLabel.Content.ToString()); }
                        var guild = discord.GetGuild(ServerID);
                        var channel = guild.GetTextChannel(Alert_BotChannel_ID);
                        string ANmessage = "";
                        string[] pbu = publicbossUrl.Split('|');
                        string[] bnu = CbossNameLabel.Content.ToString().Split('&');
                        if (CbossNameLabel.Content.ToString().Contains("&"))
                        { ANmessage = LanguageCollection[123].ToString() + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[85].ToString() + Environment.NewLine + "[" + bnu[1] + "](" + pbu[1] + ")" + " <---" + LanguageCollection[85].ToString(); }
                        else { ANmessage = LanguageCollection[123].ToString() + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[85].ToString(); }
                        var Alert_Embed = new EmbedBuilder
                        {
                            Title = CbossNameLabel.Content.ToString() /*+ " <---" + LanguageCollection[87].ToString()*/,
                            ThumbnailUrl = publicNbossimage,
                            Color = Discord.Color.LightGrey,
                            //Url = publicbossUrl  
                            Description = bossmessage.Replace("<bossname>", "") + Environment.NewLine +ANmessage
                        };
                        await channel.SendMessageAsync(">" + " " + BossSpawnRole + " " + currentbossrole1 + " " + currentbossrole2 ,false , Alert_Embed.Build());                  
                }
                catch (Exception) { }
            }
        }
        private async void DiscordNotifyNightTime(string custommessage)
        {
            if (discord.ConnectionState == Discord.ConnectionState.Connected && isposting == 1)
            {
                try
                {               
                        if (custommessage.Contains("<00:00:00>"))
                        { custommessage = custommessage.Replace("<00:00:00>", NightInBdoTimeLabel.Content.ToString()); }
                        var guild = discord.GetGuild(ServerID);
                        var channel = guild.GetTextChannel(Alert_BotChannel_ID);                        
                        var Alert_Embed = new EmbedBuilder
                        {
                            Title = "Night Time" /*+ " <---" + LanguageCollection[87].ToString()*/,
                            ThumbnailUrl = "https://raw.githubusercontent.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/master/Resources/Images/day-and-night.png",
                            Color = Discord.Color.LightGrey,
                            //Url = publicbossUrl  
                            Description = custommessage
                        };
                        await channel.SendMessageAsync("> " + NightTimeRole, false , Alert_Embed.Build());               
                }
                catch (Exception) { }
            }
        }
        private async void DiscordNotifyImperialReset(string custommessage)
        {
            if (discord.ConnectionState == Discord.ConnectionState.Connected && isposting == 1)
            {
                try
                {
                    if (custommessage.Contains("<00:00:00>"))
                    { custommessage = custommessage.Replace("<00:00:00>", IRTimeLabel.Content.ToString()); }
                    var guild = discord.GetGuild(ServerID);
                    var channel = guild.GetTextChannel(Alert_BotChannel_ID);
                    var Alert_Embed = new EmbedBuilder
                    {
                        Title = "Imperial Reset" /*+ " <---" + LanguageCollection[87].ToString()*/,
                        ThumbnailUrl = "https://raw.githubusercontent.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/master/Resources/Images/BS_icon.png",
                        Color = Discord.Color.LightGrey,
                        //Url = publicbossUrl  
                        Description = custommessage
                    };
                    await channel.SendMessageAsync("> " + ImperialResetRole, false, Alert_Embed.Build());
                }
                catch (Exception) { }
            }
        }
        private async void DiscordNotifyBR(string custommessage)
        {
            if (discord.ConnectionState == Discord.ConnectionState.Connected && isposting == 1)
            {
                try
                {
                    if (custommessage.Contains("<00:00:00>"))
                    { custommessage = custommessage.Replace("<00:00:00>", BRTimeLabel.Content.ToString()); }
                    var guild = discord.GetGuild(ServerID);
                    var channel = guild.GetTextChannel(Alert_BotChannel_ID);
                    var Alert_Embed = new EmbedBuilder
                    {
                        Title = "Bartering Reset" /*+ " <---" + LanguageCollection[87].ToString()*/,
                        ThumbnailUrl = "https://raw.githubusercontent.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/master/Resources/Images/BS_icon.png",
                        Color = Discord.Color.LightGrey,
                        //Url = publicbossUrl  
                        Description = custommessage
                    };
                   await channel.SendMessageAsync("> " + BarteringResetRole, false, Alert_Embed.Build());
                }
                catch (Exception) { }
            }
        }
        private async void DiscordNotifyITR(string custommessage)
        {
            if (discord.ConnectionState == Discord.ConnectionState.Connected && isposting == 1)
            {
                try
                {
                    if (custommessage.Contains("<00:00:00>"))
                    { custommessage = custommessage.Replace("<00:00:00>", BRTimeLabel.Content.ToString()); }
                    var guild = discord.GetGuild(ServerID);
                    var channel = guild.GetTextChannel(Alert_BotChannel_ID);
                    var Alert_Embed = new EmbedBuilder
                    {
                        Title = "Imperial Trading Reset" /*+ " <---" + LanguageCollection[87].ToString()*/,
                        ThumbnailUrl = "https://raw.githubusercontent.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/master/Resources/Images/BS_icon.png",
                        Color = Discord.Color.LightGrey,
                        //Url = publicbossUrl  
                        Description = custommessage
                    };
                   await channel.SendMessageAsync("> " + ImperialTradingResetRole, false, Alert_Embed.Build());
                }
                catch (Exception) { }
            }
        }
        #endregion
      
        private async void GetUrlSource(string url)
        {
            //int i = 0;
            //CefSharpSettings.LegacyJavascriptBindingEnabled = false;
            //CefSharpSettings.WcfEnabled = false;

            //webbrowser1.Address = url;
            //webbrowser1.LoadingStateChanged += (sender1, args) =>
            //{
            //    if (args.IsLoading == false)
            //    {
            //        i = 1;
            //    }
            //};
            //while (true)
            //{
            //    Thread.Sleep(500);
            //    if (i == 1)
            //    {
            //        var htmlSource = await webbrowser1.GetSourceAsync();
            //        if (SourceComboBox.SelectedIndex == 2)
            //        { GetBdoBossTimer(htmlSource); }
            //        break;
            //    }
            //}
        }

        private async void GetBdoBossTimer(string source)
        {
            //webbrowser1.Address = "about:blank";
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(source);
            currentbossrole1 = "";
            currentbossrole2 = "";
            publicbossUrl = "";
            timer1.Stop();
            startupRS = 0;

            PbossNamLabel.Content = "X";

            var cSecondboss = "";
            try { cSecondboss = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[2]/div[2]/h2").InnerText; } catch (Exception) { }
            CbossNameLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[2]/div[1]/h2").InnerText;
            if (cSecondboss != "")
            { CbossNameLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[2]/div[1]/h2").InnerText + " & " + cSecondboss; }
            CurrentBossTimeLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[1]/div[6]/div[1]").InnerText;

            var nSecondboss = "";
            try { nSecondboss = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[3]/div[3]/h5").InnerText; } catch (Exception) { }
            NBossNameLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[3]/div[2]/h5").InnerText;
            if (nSecondboss != "")
            { NBossNameLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[3]/div[2]/h5").InnerText + " & " + nSecondboss; }
            NextBossTimeLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[3]/div[1]/h3[2]/span").InnerText;

            try { NightInBdoTimeLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[4]/div[1]/p[1]/span[2]").InnerText; } catch (Exception) { }
            try { IRTimeLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[4]/div[2]/p[1]/span[2]").InnerText; } catch (Exception) { }
            try { BRTimeLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[4]/div[1]/p[2]/span[2]").InnerText; } catch (Exception) { }
            try { ITRITimeLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[4]/div[2]/p[2]/span[2]").InnerText; } catch (Exception) { }
            try { NILabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[4]/div[1]/p[1]/span[1]").InnerText; } catch (Exception) { }

            if (NILabel.Content.ToString() == "Night in")
            { NILabel.Content = LanguageCollection[25].ToString(); }
            else
            { NILabel.Content = LanguageCollection[107].ToString(); }

            RegionComboBox.Items.Clear();
            RegionComboBox.Items.Add("eu".ToUpper());
            RegionComboBox.Items.Add("jp".ToUpper());
            RegionComboBox.Items.Add("kr".ToUpper());
            RegionComboBox.Items.Add("mena".ToUpper());
            RegionComboBox.Items.Add("na".ToUpper());
            RegionComboBox.Items.Add("ru".ToUpper());
            RegionComboBox.Items.Add("sa".ToUpper());
            RegionComboBox.Items.Add("sea".ToUpper());
            RegionComboBox.Items.Add("th".ToUpper());
            RegionComboBox.Items.Add("tw".ToUpper());
            RegionComboBox.Items.Add("xbox-na".ToUpper());
            RegionComboBox.Items.Add("xbox-eu".ToUpper());
            RegionComboBox.Items.Add("ps4-na".ToUpper());
            RegionComboBox.Items.Add("ps4-eu".ToUpper());
            RegionComboBox.Items.Add("ps4-asia".ToUpper());

            RegionComboBox.Text = Settings.Default["DefaultRegion"].ToString().ToUpper();

            if (RegionComboBox.SelectedIndex == -1)
            { RegionComboBox.Text = "NA"; }
            source = source.Replace(@"<td class=""head"" scope=""col"">", @"<td class=""head"" scope=""col"">" + Environment.NewLine);
            string Timezone = GetStrBetweenTags(source, @"<option value=""original"">", @"</option>");
            htmlDoc.LoadHtml(source);
            var TimeRow = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[3]/div/div[2]/div/table/thead/tr").InnerText;
            Clipboard.SetText(TimeRow);
            string input = TimeRow;
            string[] data = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            try
            {
                TimeTable.Clear();
                for (int i = TimeTable.Rows.Count - 1; i >= 0; i--)
                {
                    if (TimeTable.Rows[i][0].ToString() == "")
                        TimeTable.Rows[i].Delete();
                }
                foreach (var column in TimeTable.Columns.Cast<DataColumn>().ToArray())
                {
                    if (TimeTable.AsEnumerable().All(dr => dr.IsNull(column)))
                        TimeTable.Columns.Remove(column);
                }
                gridview1.ItemsSource = null;
                TimeTable.AcceptChanges();
            }
            catch (Exception) { }
            int Crow = 0;
            int Ccolumn = 0;
            int MaxColumn = TimeTable.Columns.Count;
            foreach (var bit in data)
            {
                if (Crow == 0 && Ccolumn == 0)
                {
                    TimeTable.Columns.Add(Ccolumn.ToString());
                    TimeTable.Rows.Add(new object[] { "" });
                    DataRow dr = TimeTable.Rows[Crow];
                    dr[Ccolumn] = Timezone;
                    Ccolumn++;
                }
                if (Crow == 0 && Ccolumn > 0 && bit.Length != 0 && bit.Length <= 6)
                {
                    TimeTable.Columns.Add(Ccolumn.ToString());
                    DataRow dr = TimeTable.Rows[Crow];
                    dr[Ccolumn] = bit;
                    Ccolumn++;
                }
            }
            int linP = 1;
            int day = 1;
            int ccl = 0;
            int maxcl = TimeTable.Columns.Count;
            TimeTable.Rows.Add(new object[] { "" });
            while (true)
            {
                foreach (var cl in TimeTable.Columns)
                {
                    if (ccl == maxcl)
                    {
                        TimeTable.Rows.Add(new object[] { "" });
                        day++;
                        ccl = 0;
                        linP = 1;
                    }
                    var obj = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[3]/div/div[2]/div/table/tbody/tr[" + day.ToString() + "]/td[" + linP.ToString() + "]").InnerText;
                    DataRow dr = TimeTable.Rows[day];
                    string objF = obj.Replace("&amp;", "," + Environment.NewLine);
                    dr[ccl] = objF;
                    ccl++;
                    linP++;
                }
                if (day == 7)
                { break; }
            }

            for (int i = TimeTable.Rows.Count - 1; i >= 0; i--)
            {
                if (TimeTable.Rows[i][0].ToString() == "")
                    TimeTable.Rows[i].Delete();
            }

            TimeTable.AcceptChanges();
            //int drow = 0;
            int dayC = 0;

            SharedDay = dayC;
            gridview1.ItemsSource = TimeTable.DefaultView;

            try
            {
                BossesCollection.Clear();
                BossCollectionListBox.Items.Clear();
            }
            catch (Exception) { }
            string BossesList = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Bosses");
            string[] BossListSplited = BossesList.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (string BossData in BossListSplited)
            {
                if (BossData != "")
                {
                    BossesCollection.Add(BossData.Replace(Environment.NewLine, ""));
                    string bossName = BossData.Substring(0, BossData.IndexOf(",") + 1);
                    bossName = bossName.Replace(",", "");
                    bossName = bossName.Replace(Environment.NewLine, "");
                    BossCollectionListBox.Items.Add(bossName);
                }
            }

            PBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/Boss.png"));
            NBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/Boss.png"));
            LBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/Boss.png"));
            Getimg gm = new Getimg();
            string First_boss = "";
            string Second_boss = "";
            foreach (string item in BossesCollection)
            {
                string bossName = item.Substring(0, item.IndexOf(",") + 1);
                bossName = bossName.Replace(",", "");
                bossName = bossName.Replace(Environment.NewLine, "");

                if (PbossNamLabel.Content.ToString().Contains(bossName))
                {
                    string[] bossdata = item.Split(',');
                    string imgurl = bossdata[1].ToString();
                    if (bossdata[5].ToString() != "")
                    {
                        imgurl = bossdata[5].ToString();
                        if (imgurl.Contains("<Local>"))
                        { imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
                    }
                    try { PBImageBox.Source = gm.GETIMAGE(imgurl); } catch (Exception) { }
                    if (PbossNamLabel.Content.ToString().Contains("&"))
                    { try { PBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/" + PbossNamLabel.Content.ToString() + ".png"); } catch (Exception) { } }
                }
                if (CbossNameLabel.Content.ToString().Contains(bossName))
                {
                    string[] bossdata = item.Split(',');
                    try
                    {
                        string imgurl = bossdata[1].ToString();
                        if (bossdata[5].ToString() == "")
                        {
                            imgurl = bossdata[1].ToString();
                        }
                        else
                        {
                            imgurl = bossdata[5].ToString();
                            if (imgurl.Contains("<Local>"))
                            { imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
                        }
                        publicNbossimage = bossdata[1].ToString();
                        if (!CbossNameLabel.Content.ToString().Contains("&"))
                        { publicbossUrl = bossdata[2].ToString(); }
                        else
                        {
                            if (publicbossUrl == "")
                            { publicbossUrl = bossdata[2].ToString(); }
                            else
                            { publicbossUrl += " | " + bossdata[2].ToString(); }
                        }
                        NBImageBox.Source = gm.GETIMAGE(imgurl);
                        if (CbossNameLabel.Content.ToString().Contains("&"))
                        { try { NBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/" + CbossNameLabel.Content.ToString() + ".png"); } catch (Exception) { } }

                    }
                    catch (Exception) { }
                }

                if (cSecondboss.Contains(bossName) && CbossNameLabel.Content.ToString().Contains("&") && cSecondboss != "")
                {
                    string[] bossdata = item.Split(',');
                    if (publicbossUrl == "")
                    { publicbossUrl = bossdata[2].ToString(); }
                    else
                    { publicbossUrl += "|" + bossdata[2].ToString(); }
                }
                if (NBossNameLabel.Content.ToString().Contains(bossName))
                {
                    string[] bossdata = item.Split(',');
                    string imgurl = bossdata[1].ToString();
                    if (bossdata[5].ToString() != "")
                    {
                        imgurl = bossdata[5].ToString();
                        if (imgurl.Contains("<Local>"))
                        { imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
                    }
                    try { LBImageBox.Source = gm.GETIMAGE(imgurl); } catch (Exception) { }
                    if (NBossNameLabel.Content.ToString().Contains("&"))
                    { try { LBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/" + NBossNameLabel.Content.ToString() + ".png"); } catch (Exception) { } }
                    //if (bossdata[3].ToString() != "")
                    //{ NBossNameLabel.Content = bossdata[3].ToString(); }
                }

            }

            if (PbossNamLabel.Content.ToString().Contains(" & "))
            {
                string fullbossname = "";
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    string name = PbossNamLabel.Content.ToString();
                    name = name.Substring(0, name.IndexOf("&"));
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        if (bossdata[3].ToString() != "")
                        {
                            fullbossname += bossdata[3].ToString() + " & ";
                        }
                    }
                }
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    string name = PbossNamLabel.Content.ToString();
                    name = name.Substring(name.IndexOf("&") + 1);
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        if (bossdata[3].ToString() != "")
                        {
                            fullbossname += bossdata[3].ToString();
                        }
                    }
                }
                if (fullbossname != "")
                { PbossNamLabel.Content = fullbossname; }

            }
            else
            {
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    if (PbossNamLabel.Content.ToString().Contains(bossdata[0].ToString()))
                    {
                        if (bossdata[3].ToString() != "")
                        { PbossNamLabel.Content = bossdata[3].ToString(); }
                    }

                }
            }

            if (CbossNameLabel.Content.ToString().Contains(" & "))
            {
                string fullbossname = "";
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    string name = CbossNameLabel.Content.ToString();
                    name = name.Substring(0, name.IndexOf("&"));
                    First_boss = name;
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        string[] br = bossdata[4].ToString().Split('{');
                        currentbossrole1 = br[1].ToString();
                        if (bossdata[3].ToString() != "")
                        {
                            fullbossname += bossdata[3].ToString() + " & ";
                        }
                    }
                }
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    string name = CbossNameLabel.Content.ToString();
                    name = name.Substring(name.IndexOf("&") + 1);
                    Second_boss = name;
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        string[] br = bossdata[4].ToString().Split('{');
                        currentbossrole2 = br[1].ToString();
                        if (bossdata[3].ToString() != "")
                        {
                            fullbossname += bossdata[3].ToString();
                        }
                    }
                }
                if (fullbossname != "")
                { CbossNameLabel.Content = fullbossname; }
            }
            else
            {
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    if (CbossNameLabel.Content.ToString().Contains(bossdata[0].ToString()))
                    {
                        if (bossdata[3].ToString() != "")
                        {
                            string[] br = bossdata[4].ToString().Split('{');
                            currentbossrole1 = br[1].ToString();
                            CbossNameLabel.Content = bossdata[3].ToString();
                        }
                        else
                        {
                            string[] br = bossdata[4].ToString().Split('{');
                            currentbossrole1 = br[1].ToString();
                        }
                    }

                }
            }
            if (NBossNameLabel.Content.ToString().Contains(" & "))
            {
                string fullbossname = "";
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    string name = NBossNameLabel.Content.ToString();
                    name = name.Substring(0, name.IndexOf("&"));
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        if (bossdata[3].ToString() != "")
                        {
                            fullbossname += bossdata[3].ToString() + " & ";
                        }
                    }
                }
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    string name = NBossNameLabel.Content.ToString();
                    name = name.Substring(name.IndexOf("&") + 1);
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        if (bossdata[3].ToString() != "")
                        {
                            fullbossname += bossdata[3].ToString();
                        }
                    }
                }
                if (fullbossname != "")
                { NBossNameLabel.Content = fullbossname; }
            }
            else
            {
                foreach (string item in BossesCollection)
                {
                    string[] bossdata = item.Split(',');
                    if (NBossNameLabel.Content.ToString().Contains(bossdata[0].ToString()))
                    {
                        if (bossdata[3].ToString() != "")
                        { NBossNameLabel.Content = bossdata[3].ToString(); }
                    }

                }
            }

            SaveLatestTimeTable = 0;
            intervalMessageUpdate = UpdateMesssageInterval;
            if (CbossNameLabel.Content.ToString().Contains(" & "))
            {
                publicNbossimage = "https://raw.githubusercontent.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/master/Resources/Images/" + First_boss.Replace(" ", "") + "%20%26%20" + Second_boss.Replace(" ", "") + ".png";
            }
            if (discord.ConnectionState == Discord.ConnectionState.Connected && isposting == 1)
            {
                try
                {
                    var guild = discord.GetGuild(ServerID);
                    var channel = guild.GetTextChannel(Main_BotChannel_ID);
                    var Message = await channel.GetMessageAsync(bossImageID) as IUserMessage;
                    string[] pbu = publicbossUrl.Split('|');
                    string[] bnu = CbossNameLabel.Content.ToString().Split('&');
                    string ANmessage = "";
                    if (CbossNameLabel.Content.ToString().Contains("&"))
                    {
                        ANmessage = LanguageCollection[123].ToString() + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[85].ToString() + Environment.NewLine + "[" + bnu[1] + "](" + pbu[1] + ")" + " <---" + LanguageCollection[85].ToString();
                    }
                    else
                    {
                        ANmessage = LanguageCollection[123].ToString() + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[85].ToString();
                    }
                    var embed1 = new EmbedBuilder
                    {
                        Title = CbossNameLabel.Content.ToString() /*+ " <---" + LanguageCollection[87].ToString()*/,
                        ImageUrl = publicNbossimage,
                        Color = Color.LightGrey,
                        //Url = publicbossUrl  
                        Description = ANmessage
                    };
                    await Message.ModifyAsync(msg => msg.Embed = embed1.Build());
                }
                catch (Exception) { }
            }
            if (EditSpawnHoursSlider.Value.ToString().Contains("-"))
            {
                DateTime PDT = DateTime.Parse(PreviousBossTimeLabel.Content.ToString());
                string pdtl = PDT.AddHours(double.Parse(EditSpawnHoursSlider.Value.ToString().Replace("-", "+"))).ToString(@"hh\:mm\:ss");
                PreviousBossTimeLabel.Content = pdtl;

                DateTime CDT = DateTime.Parse(CurrentBossTimeLabel.Content.ToString());
                string cbtl = CDT.AddHours(EditSpawnHoursSlider.Value).ToString(@"hh\:mm\:ss");
                CurrentBossTimeLabel.Content = cbtl;

                DateTime NDT = DateTime.Parse(NextBossTimeLabel.Content.ToString());
                string nbtl = NDT.AddHours(EditSpawnHoursSlider.Value).ToString(@"hh\:mm\:ss");
                NextBossTimeLabel.Content = nbtl;
            }
            if (EditSpawnHoursSlider.Value > 0)
            {
                DateTime PDT = DateTime.Parse(PreviousBossTimeLabel.Content.ToString());
                string pdtl = PDT.AddHours(double.Parse("-" + EditSpawnHoursSlider.Value.ToString())).ToString(@"hh\:mm\:ss");
                PreviousBossTimeLabel.Content = pdtl;

                DateTime CDT = DateTime.Parse(CurrentBossTimeLabel.Content.ToString());
                string cbtl = CDT.AddHours(EditSpawnHoursSlider.Value).ToString(@"hh\:mm\:ss");
                CurrentBossTimeLabel.Content = cbtl;

                DateTime NDT = DateTime.Parse(NextBossTimeLabel.Content.ToString());
                string nbtl = NDT.AddHours(EditSpawnHoursSlider.Value).ToString(@"hh\:mm\:ss");
                NextBossTimeLabel.Content = nbtl;
            }
            if (currentbossrole1 != "")
            {
                if(currentbossrole1 != "@everyone" && currentbossrole1 != "@here")
                { currentbossrole1 = "<@&" + currentbossrole1 + ">"; }
            }
            if (currentbossrole2 != "")
            {
                if (currentbossrole2 != "@everyone" && currentbossrole2 != "@here")
                { currentbossrole2 = "<@&" + currentbossrole2 + ">"; }
            }

            if (discord.ConnectionState == Discord.ConnectionState.Connected && isposting == 1)
            {
                if (DisplayTimeTableSetting.IsChecked == true)
                {
                    if (TimtableID != 0)
                    {
                        var guild = discord.GetGuild(ServerID);
                        var channel = guild.GetTextChannel(Main_BotChannel_ID);
                        await channel.DeleteMessageAsync(TimtableID);
                        TimtableID = 0;
                    }

                    if (TimtableID == 0)
                    {
                        try { gridview1.SelectedIndex = SharedDay; } catch (Exception) { }
                        if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"))
                        { File.Delete(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"); }
                        //int width = int.Parse(gridview1.ActualWidth.ToString());//error
                        //int height = int.Parse(gridview1.ActualHeight.ToString());
                        RenderTargetBitmap renderTargetBitmap =
                       new RenderTargetBitmap(1920, 1080, 180, 250, PixelFormats.Pbgra32);
                        renderTargetBitmap.Render(gridview1);
                        PngBitmapEncoder pngImage = new PngBitmapEncoder();
                        pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                        using (Stream fileStream = File.Create(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"))
                        {
                            pngImage.Save(fileStream);
                        }
                        var guild = discord.GetGuild(ServerID);
                        var channel = guild.GetTextChannel(Main_BotChannel_ID);
                        var timetablemessage = await channel.SendFileAsync(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png", MOTR.ToString().ToUpper() + LanguageCollection[43].ToString());
                        TimtableID = timetablemessage.Id;
                    }
                }
            }
            
            timer1.Start();

        }
        public static string GetStrBetweenTags(string value, string startTag, string endTag)
        {
            if (value.Contains(startTag) && value.Contains(endTag))
            {
                int index = value.IndexOf(startTag) + startTag.Length;
                return value.Substring(index, value.IndexOf(endTag) - index);
            }
            else
                return null;
        }
      
        private void SaveSettings2Button_Click(object sender, RoutedEventArgs e)
        {
            

            tabcontrol1.SelectedIndex = 3;
            try
            {
                DoubleAnimation da = new DoubleAnimation();
                da.From = 0;
                da.To = 1;
                da.Duration = new Duration(TimeSpan.FromSeconds(1));
                da.AutoReverse = false;
                tabcontrol1.BeginAnimation(OpacityProperty, da);
            }
            catch (Exception) { }
        }

        private void EditSpawnHoursSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //timer1.Stop();
            TimeSpan CC = TimeSpan.FromHours(1);
            if (EditSpawnHoursSlider.Value > lastSliderValue)
            {
                TimeSpan PDT = TimeSpan.Parse(PreviousBossTimeLabel.Content.ToString());
                PreviousBossTimeLabel.Content = PDT.Subtract(CC).ToString(@"hh\:mm\:ss");

                TimeSpan CDT = TimeSpan.Parse(CurrentBossTimeLabel.Content.ToString());
                CurrentBossTimeLabel.Content = CDT.Add(CC).ToString(@"hh\:mm\:ss");

                TimeSpan NDT = TimeSpan.Parse(NextBossTimeLabel.Content.ToString());
                NextBossTimeLabel.Content = NDT.Add(CC).ToString(@"hh\:mm\:ss");
            }
            else
            {
                TimeSpan PDT = TimeSpan.Parse(PreviousBossTimeLabel.Content.ToString());
                PreviousBossTimeLabel.Content = PDT.Add(CC).ToString(@"hh\:mm\:ss");

                TimeSpan CDT = TimeSpan.Parse(CurrentBossTimeLabel.Content.ToString());
                CurrentBossTimeLabel.Content = CDT.Subtract(CC).ToString(@"hh\:mm\:ss");

                TimeSpan NDT = TimeSpan.Parse(NextBossTimeLabel.Content.ToString());
                NextBossTimeLabel.Content = NDT.Subtract(CC).ToString(@"hh\:mm\:ss");
            }
            lastSliderValue = EditSpawnHoursSlider.Value;
            if (EditSpawnHoursSlider.Value.ToString().Contains("-"))
            { EditSpawnHoursLabael1.Content = LanguageCollection[22].ToString() + " " + EditSpawnHoursSlider.Value; }
            else
            { EditSpawnHoursLabael1.Content = LanguageCollection[22].ToString() + " +" + EditSpawnHoursSlider.Value; }
            if (EditSpawnHoursSlider.Value == 0)
            { EditSpawnHoursLabael1.Content = LanguageCollection[22].ToString() + " " + EditSpawnHoursSlider.Value; }
            Settings.Default["EditSpawnHoursSlider"] = EditSpawnHoursSlider.Value.ToString();
            Settings.Default.Save();
        }        

        private void AlertonhousandminutesCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (AlertonhousandminutesCheckBox.IsChecked == true)
            {
                Settings.Default["AlarmonhoursCheckbox"] = "1";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["AlarmonhoursCheckbox"] = "0";
                Settings.Default.Save();
            }
        }       

        private void BRAlarm1Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveBarteringResetAlrarmSettings();
        }
        private void SaveBarteringResetAlrarmSettings()
        {
            if (BRAlarm1Textbox.Text == "")
            { BRAlarm1Textbox.Text = "30"; }
            if (BRAlarm2Textbox.Text == "")
            { BRAlarm2Textbox.Text = "15"; }
            if (BRAlarm3Textbox.Text == "")
            { BRAlarm3Textbox.Text = "05"; }

            if (BRAlarm1Textbox.Text == "0")
            { BRAlarm1Textbox.Text = "30"; }
            if (BRAlarm2Textbox.Text == "0")
            { BRAlarm2Textbox.Text = "15"; }
            if (BRAlarm3Textbox.Text == "0")
            { BRAlarm3Textbox.Text = "05"; }

            if (int.Parse(BRAlarm1Textbox.Text) < 10)
            { BRAlarm1Textbox.Text = "0" + int.Parse(BRAlarm1Textbox.Text); }
            if (int.Parse(BRAlarm2Textbox.Text) < 10)
            { BRAlarm2Textbox.Text = "0" + int.Parse(BRAlarm2Textbox.Text); }
            if (int.Parse(BRAlarm3Textbox.Text) < 10)
            { BRAlarm3Textbox.Text = "0" + int.Parse(BRAlarm3Textbox.Text); }

            string bitResult = "";
            if (BRAlarmCheckBox1.IsChecked == true)
            { bitResult += BRAlarm1Textbox.Text + "," + "1,"; }
            else
            { bitResult += BRAlarm1Textbox.Text + "," + "0,"; }

            if (BRAlarmCheckBox2.IsChecked == true)
            { bitResult += BRAlarm2Textbox.Text + "," + "1,"; }
            else
            { bitResult += BRAlarm2Textbox.Text + "," + "0,"; }

            if (BRAlarmCheckBox3.IsChecked == true)
            { bitResult += BRAlarm3Textbox.Text + "," + "1"; }
            else
            { bitResult += BRAlarm3Textbox.Text + "," + "0"; }

            Settings.Default["BRAlarmCheckBox"] = bitResult;
            Settings.Default.Save();
        }

        private void BRAlarm1Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.Parse(BRAlarm1Textbox.Text) > 59)
                { BRAlarm1Textbox.Text = "59"; }
            }
            catch (Exception) { }
        }

        private void BRAlarm2Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveBarteringResetAlrarmSettings();
        }

        private void BRAlarm2Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.Parse(BRAlarm2Textbox.Text) > 59)
                { BRAlarm2Textbox.Text = "59"; }
            }
            catch (Exception) { }
        }

        private void BRAlarm3Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveBarteringResetAlrarmSettings();
        }

        private void BRAlarm3Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.Parse(BRAlarm3Textbox.Text) > 59)
                { BRAlarm3Textbox.Text = "59"; }
            }
            catch (Exception) { }
        }

        private void BRAlarmCheckBox1_Click(object sender, RoutedEventArgs e)
        {
            SaveBarteringResetAlrarmSettings();
        }

        private void BRAlarmCheckBox2_Click(object sender, RoutedEventArgs e)
        {
            SaveBarteringResetAlrarmSettings();
        }

        private void BRAlarmCheckBox3_Click(object sender, RoutedEventArgs e)
        {
            SaveBarteringResetAlrarmSettings();
        }
        private void SaveImperialTradingResetAlrarmSettings()
        {
            if (ITRAlarm1Textbox.Text == "")
            { ITRAlarm1Textbox.Text = "30"; }
            if (ITRAlarm2Textbox.Text == "")
            { ITRAlarm2Textbox.Text = "15"; }
            if (ITRAlarm3Textbox.Text == "")
            { ITRAlarm3Textbox.Text = "05"; }

            if (ITRAlarm1Textbox.Text == "0")
            { ITRAlarm1Textbox.Text = "30"; }
            if (ITRAlarm2Textbox.Text == "0")
            { ITRAlarm2Textbox.Text = "15"; }
            if (ITRAlarm3Textbox.Text == "0")
            { ITRAlarm3Textbox.Text = "05"; }

            if (int.Parse(ITRAlarm1Textbox.Text) < 10)
            { ITRAlarm1Textbox.Text = "0" + int.Parse(ITRAlarm1Textbox.Text); }
            if (int.Parse(ITRAlarm2Textbox.Text) < 10)
            { ITRAlarm2Textbox.Text = "0" + int.Parse(ITRAlarm2Textbox.Text); }
            if (int.Parse(ITRAlarm3Textbox.Text) < 10)
            { ITRAlarm3Textbox.Text = "0" + int.Parse(ITRAlarm3Textbox.Text); }

            string bitResult = "";
            if (ITRAlarmCheckBox1.IsChecked == true)
            { bitResult += ITRAlarm1Textbox.Text + "," + "1,"; }
            else
            { bitResult += ITRAlarm1Textbox.Text + "," + "0,"; }

            if (ITRAlarmCheckBox2.IsChecked == true)
            { bitResult += ITRAlarm2Textbox.Text + "," + "1,"; }
            else
            { bitResult += ITRAlarm2Textbox.Text + "," + "0,"; }

            if (ITRAlarmCheckBox3.IsChecked == true)
            { bitResult += ITRAlarm3Textbox.Text + "," + "1"; }
            else
            { bitResult += ITRAlarm3Textbox.Text + "," + "0"; }


            Settings.Default["ITRAlarmCheckBox"] = bitResult;
            Settings.Default.Save();
        }

        private void ITRAlarm1Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveImperialTradingResetAlrarmSettings();
        }

        private void ITRAlarmCheckBox1_Click(object sender, RoutedEventArgs e)
        {
            SaveImperialTradingResetAlrarmSettings();
        }

        private void ITRAlarm1Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.Parse(ITRAlarm1Textbox.Text) > 59)
                { ITRAlarm1Textbox.Text = "59"; }
            }
            catch (Exception) { }
        }

        private void ITRAlarm2Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveImperialTradingResetAlrarmSettings();
        }

        private void ITRAlarm2Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.Parse(ITRAlarm2Textbox.Text) > 59)
                { ITRAlarm2Textbox.Text = "59"; }
            }
            catch (Exception) { }
        }

        private void ITRAlarmCheckBox2_Click(object sender, RoutedEventArgs e)
        {
            SaveImperialTradingResetAlrarmSettings();
        }

        private void ITRAlarm3Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveImperialTradingResetAlrarmSettings();
        }

        private void ITRAlarm3Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.Parse(ITRAlarm3Textbox.Text) > 59)
                { ITRAlarm3Textbox.Text = "59"; }
            }
            catch (Exception) { }
        }

        private void ITRAlarmCheckBox3_Click(object sender, RoutedEventArgs e)
        {
            SaveImperialTradingResetAlrarmSettings();
        }
   
       
        private void BotHostComboBox_DropDownClosed(object sender, EventArgs e)
        {
            
        }

        private void SourceComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (lastSelectedSource != SourceComboBox.SelectedIndex)
            {
                Settings.Default["SelectedSource"] = SourceComboBox.SelectedIndex.ToString();
                Settings.Default.Save();
                lastSelectedSource = SourceComboBox.SelectedIndex;
                string region = Settings.Default["DefaultRegion"].ToString();//get Last Saved Region    
                if (SourceComboBox.SelectedIndex == 0)
                { GetLYPBBTTimeTable(); }
                if (SourceComboBox.SelectedIndex == 1)
                { GetTimeTable(region); }
                if (SourceComboBox.SelectedIndex == 2)
                { GetUrlSource("https://bdobosstimer.com/?&server=" + region); }
            }
        }

        private void AddnewBossTestImgLinkButtonLocal_Click(object sender, RoutedEventArgs e)
        {
            Getimg gm = new Getimg();
            DisplayImageLinkextBoxLocal.BorderBrush = Brushes.Silver;
            AddSaveBossPictureBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/Boss.png"));
            string imgurl = DisplayImageLinkextBoxLocal.Text;
            if (imgurl.Contains("<Local>"))
            { imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
            try { AddSaveBossPictureBox.Source = gm.GETIMAGE(imgurl); } catch (Exception) { DisplayImageLinkextBoxLocal.BorderBrush = Brushes.Red; }
        }

        public async void GetLYPBBTTimeTable()
        {
            try
            {
                ScarletMode_Message = "";
                ScarletModeTimeTableSetting_CheckBox.IsEnabled = true;
                ScarletModeTimeTableSetting_CheckBox.IsChecked = bool.Parse(Settings.Default["ScarletMode"].ToString());
                botuptime = 0;
                timer1.Stop();
                //Thread.Sleep(1000);
                currentbossrole1 = "";
                currentbossrole2 = "";
                publicbossUrl = "";

                RegionComboBox.Items.Clear();
                RegionComboBox.Items.Add("PS4-ASIA");
                RegionComboBox.Items.Add("EU");
                RegionComboBox.Items.Add("PS4-XBOX-EU");
                RegionComboBox.Items.Add("JP");
                RegionComboBox.Items.Add("KR");
                RegionComboBox.Items.Add("MENA");
                RegionComboBox.Items.Add("NA");
                RegionComboBox.Items.Add("PS4-XBOX-NA");
                RegionComboBox.Items.Add("RU");
                RegionComboBox.Items.Add("SA");
                RegionComboBox.Items.Add("SEA");
                RegionComboBox.Items.Add("TH");
                RegionComboBox.Items.Add("TW");

                TimeTableBoss1DropBox.Items.Clear();
                TimeTableBoss1DropBox.Items.Add("None");
                TimeTableBoss1DropBox.Items.Add("Kzarka");
                TimeTableBoss1DropBox.Items.Add("NM Kzarka");
                TimeTableBoss1DropBox.Items.Add("Nouver");
                TimeTableBoss1DropBox.Items.Add("BS Nouver");
                TimeTableBoss1DropBox.Items.Add("Karanda");
                TimeTableBoss1DropBox.Items.Add("SB Karanda");
                TimeTableBoss1DropBox.Items.Add("Kutum");
                TimeTableBoss1DropBox.Items.Add("Offin");
                TimeTableBoss1DropBox.Items.Add("Garmoth");
                TimeTableBoss1DropBox.Items.Add("Quint");
                TimeTableBoss1DropBox.Items.Add("Muraka");
                TimeTableBoss1DropBox.Items.Add("Vell");

                TimeTableBoss2DropBox.Items.Clear();
                TimeTableBoss2DropBox.Items.Add("None");
                TimeTableBoss2DropBox.Items.Add("Kzarka");
                TimeTableBoss2DropBox.Items.Add("NM Kzarka");
                TimeTableBoss2DropBox.Items.Add("Nouver");
                TimeTableBoss2DropBox.Items.Add("BS Nouver");
                TimeTableBoss2DropBox.Items.Add("Karanda");
                TimeTableBoss2DropBox.Items.Add("SB Karanda");
                TimeTableBoss2DropBox.Items.Add("Kutum");
                TimeTableBoss2DropBox.Items.Add("Offin");
                TimeTableBoss2DropBox.Items.Add("Garmoth");
                TimeTableBoss2DropBox.Items.Add("Quint");
                TimeTableBoss2DropBox.Items.Add("Muraka");
                TimeTableBoss2DropBox.Items.Add("Vell");


                TimeColumnHoursDropBox.Items.Clear();
                TimeColumnMinutesDropBox.Items.Clear();
                int HV = 0;
                while (true)
                {
                    if (HV < 24)
                    {
                        if (HV.ToString().Length == 1)
                        { TimeColumnHoursDropBox.Items.Add("0" + HV.ToString()); }
                        else
                        { TimeColumnHoursDropBox.Items.Add(HV.ToString()); }
                    }
                    if (HV.ToString().Length == 1)
                    { TimeColumnMinutesDropBox.Items.Add("0" + HV.ToString()); }
                    else
                    { TimeColumnMinutesDropBox.Items.Add(HV.ToString()); }

                    HV++;
                    if (HV > 59)
                    { break; }
                }

                string timetable = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT");
                RegionComboBox.Text = Settings.Default["DefaultRegion"].ToString().ToUpper();
                if (RegionComboBox.Text == "")
                { RegionComboBox.Text = "EU"; }
                int rsi = RegionComboBox.SelectedIndex;
                int UtcValue = 0;
                switch (rsi)
                {
                    case 0:
                        UtcValue = +9;
                        break;
                    case 1:
                        UtcValue = +1;
                        break;
                    case 2:
                        UtcValue = +1;
                        break;
                    case 3:
                        UtcValue = +9;
                        break;
                    case 4:
                        UtcValue = +9;
                        break;
                    case 5:
                        UtcValue = +3;
                        break;
                    case 6:
                        UtcValue = -8;
                        break;
                    case 7:
                        UtcValue = -8;
                        break;
                    case 8:
                        UtcValue = +3;
                        break;
                    case 9:
                        UtcValue = -3;
                        break;
                    case 10:
                        UtcValue = +8;
                        break;
                    case 11:
                        UtcValue = +7;
                        break;
                    case 12:
                        UtcValue = +8;
                        break;
                }
                DateTime thisDate = DateTime.UtcNow.AddHours(UtcValue);
                try
                {
                    TimeTable.Clear();
                    for (int i = TimeTable.Rows.Count - 1; i >= 0; i--)
                    {
                        if (TimeTable.Rows[i][0].ToString() == "")
                            TimeTable.Rows[i].Delete();
                    }
                    foreach (var column in TimeTable.Columns.Cast<DataColumn>().ToArray())
                    {
                        if (TimeTable.AsEnumerable().All(dr => dr.IsNull(column)))
                            TimeTable.Columns.Remove(column);
                    }
                    gridview1.ItemsSource = null;
                    TimeTable.AcceptChanges();
                }
                catch (Exception) { }
                TimeTable.Columns.Add("");
                string timezone = "Utc " + UtcValue;
                if (!UtcValue.ToString().Contains("-"))
                { timezone = "Utc +" + UtcValue; }
                TimeTable.Rows.Add(new object[] { timezone });
                TimeTable.Rows.Add(new object[] { DayOfWeek.Monday });
                TimeTable.Rows.Add(new object[] { DayOfWeek.Tuesday });
                TimeTable.Rows.Add(new object[] { DayOfWeek.Wednesday });
                TimeTable.Rows.Add(new object[] { DayOfWeek.Thursday });
                TimeTable.Rows.Add(new object[] { DayOfWeek.Friday });
                TimeTable.Rows.Add(new object[] { DayOfWeek.Saturday });
                TimeTable.Rows.Add(new object[] { DayOfWeek.Sunday });

                if (RegionComboBox.SelectedIndex == -1)
                { RegionComboBox.Text = "NA"; }
                string RegionTimeTable = GetStrBetweenTags(timetable, "[" + RegionComboBox.Text + "]", "[/" + RegionComboBox.Text + "]");
                string[] TimeTableRows = RegionTimeTable.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                int rx = 0;
                foreach (string row in TimeTableRows)
                {
                    if (row != "")
                    {
                        string[] TimeTableColumns = row.Split('|');
                        int cx = 1;
                        foreach (string cell in TimeTableColumns)
                        {
                            if (rx == 0)
                            { TimeTable.Columns.Add(""); }
                            DataRow itdr = TimeTable.Rows[rx];
                            string celloutput = cell;
                            if (celloutput.Contains(","))
                            { celloutput = celloutput.Replace(",", "," + Environment.NewLine); }
                            itdr[cx] = celloutput;
                            cx++;
                        }
                        rx++;
                    }
                }
                DateTime DT = DateTime.UtcNow.AddHours(UtcValue);
                Console.WriteLine(DT.DayOfWeek + " Date: " + DT.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + " Time: " + DT.ToString(@"HH: mm"), CultureInfo.InvariantCulture);
                int cr = 0;
                int DayIndex = -1;
                foreach (var row in TimeTable.Rows)
                {
                    if (cr > 0)
                    {
                        DataRow dr = TimeTable.Rows[cr];
                        if (dr[0].ToString() == DT.DayOfWeek.ToString())
                        { DayIndex = cr; }
                    }
                    cr++;
                }

                int cl = 0;
                int g = 0;
                int HourIndex = -1;
                foreach (var column in TimeTable.Columns)
                {
                    if (cl > 0)
                    {
                        DataRow dr = TimeTable.Rows[0];
                        DateTime CLDT = DateTime.Parse(DT.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + " " + dr[cl].ToString() + ":00", CultureInfo.InvariantCulture);
                        if (CLDT > DT && g == 0)
                        {
                            HourIndex = cl;
                            g = 1;
                        }
                    }
                    cl++;
                }
                int day_correction = 0;
                if (HourIndex == -1)
                {
                    HourIndex = 1;
                    day_correction++;
                    if (DayIndex < TimeTable.Rows.Count - 1)
                    { DayIndex++; }
                    else
                    { DayIndex = 1; }
                }
                string cSecondboss = "";
                #region "Current Boss"
                int CDT_addedDays = 0;
                while (true)
                {
                    DataRow dr = TimeTable.Rows[DayIndex];
                    if (dr[HourIndex].ToString() == "")
                    {
                        if (HourIndex < TimeTable.Columns.Count - 1)
                        {
                            HourIndex++;
                        }
                        else
                        {
                            HourIndex = 1;
                            if (DayIndex < TimeTable.Rows.Count - 1)
                            {
                                DayIndex++;
                                CDT_addedDays++;
                            }
                            else
                            {
                                DayIndex = 1;
                                CDT_addedDays++;
                            }
                        }
                    }
                    else
                    { break; }
                }

                DataRow dr1 = TimeTable.Rows[0];
                DateTime CDT = DateTime.Parse(DT.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + " " + dr1[HourIndex].ToString() + ":00", CultureInfo.InvariantCulture);
                CDT = CDT.AddDays(CDT_addedDays);
                CDT = CDT.AddDays(day_correction);
                TimeSpan duration = CDT.Subtract(DT);
                CurrentBossTimeLabel.Content = duration.ToString(@"hh\:mm\:ss");
                Console.WriteLine("Row: " + DayIndex + " " + "Column: " + HourIndex);
                try
                {
                    DataRow DrT = TimeTable.Rows[0];
                    string Time = DrT[HourIndex].ToString();
                    DrT = TimeTable.Rows[DayIndex];
                    string Day = DrT[0].ToString();
                    CurrentBossTimeLabel.ToolTip = Day + " " + Time;
                }
                catch (Exception) { }
                dr1 = TimeTable.Rows[DayIndex];
                CbossNameLabel.Content = dr1[HourIndex].ToString().Replace("," + Environment.NewLine, " & ");
                if (CbossNameLabel.Content.ToString().Contains(" & "))
                {cSecondboss = CbossNameLabel.Content.ToString().Split('&')[1].Trim();}

                #endregion
                #region "Next Boss"

                int N_HourIndex = HourIndex;
                int N_DayIndex = DayIndex;
                N_HourIndex++;
                CDT_addedDays = 0;
                if (N_HourIndex > TimeTable.Columns.Count - 1)
                {
                    N_HourIndex = 1;
                    N_DayIndex++;
                    CDT_addedDays++;
                    if (N_DayIndex > TimeTable.Rows.Count - 1)
                    {
                        N_DayIndex = 1;
                        CDT_addedDays = N_DayIndex;
                    }
                }

                while (true)
                {
                    DataRow dr = TimeTable.Rows[N_DayIndex];
                    if (dr[N_HourIndex].ToString() == "")
                    {
                        if (N_HourIndex < TimeTable.Columns.Count - 1)
                        {
                            N_HourIndex++;
                        }
                        else
                        {
                            N_HourIndex = 1;
                            if (N_DayIndex < TimeTable.Rows.Count - 1)
                            {
                                N_DayIndex++;
                                CDT_addedDays++;
                            }
                            else
                            {
                                N_DayIndex = 1;
                                CDT_addedDays++;
                            }
                        }
                    }
                    else
                    { break; }
                }
                dr1 = TimeTable.Rows[0];
                CDT = DateTime.Parse(DT.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + " " + dr1[N_HourIndex].ToString() + ":00", CultureInfo.InvariantCulture);
                if (CDT < DT)
                { CDT = CDT.AddDays(1); }
                CDT = CDT.AddDays(CDT_addedDays);
                CDT = CDT.AddDays(day_correction);
                duration = CDT.Subtract(DT);
                NextBossTimeLabel.Content = duration.ToString(@"hh\:mm\:ss");
                Console.WriteLine("Row: " + N_DayIndex + " " + "Column: " + N_HourIndex);
                try
                {
                    DataRow DrT = TimeTable.Rows[0];
                    string Time = DrT[N_HourIndex].ToString();
                    DrT = TimeTable.Rows[N_DayIndex];
                    string Day = DrT[0].ToString();
                    NextBossTimeLabel.ToolTip = Day + " " + Time;
                }
                catch (Exception) { }
                dr1 = TimeTable.Rows[N_DayIndex];
                NBossNameLabel.Content = dr1[N_HourIndex].ToString().Replace("," + Environment.NewLine, " & ");
                #endregion
                #region "Previous Boss"

                int P_HourIndex = HourIndex;
                int P_DayIndex = DayIndex;
                P_HourIndex--;
                CDT_addedDays = 0;
                if (P_HourIndex <= 0)
                {
                    P_HourIndex = TimeTable.Columns.Count - 1;
                    P_DayIndex--;
                    CDT_addedDays--;
                    if (P_DayIndex == 0)
                    {
                        P_DayIndex = TimeTable.Rows.Count - 1;
                        CDT_addedDays = P_DayIndex * -1;
                    }
                }
                while (true)
                {
                    DataRow dr = TimeTable.Rows[P_DayIndex];
                    if (dr[P_HourIndex].ToString() == "")
                    {
                        if (P_HourIndex > 1)
                        {
                            P_HourIndex--;
                        }
                        else
                        {
                            P_HourIndex = TimeTable.Columns.Count - 1;
                            if (P_DayIndex > 1)
                            {
                                P_DayIndex--;
                                CDT_addedDays--;
                            }
                            else
                            {
                                P_DayIndex = TimeTable.Rows.Count - 1;
                                CDT_addedDays--;
                            }
                        }
                    }
                    else
                    { break; }
                }
                dr1 = TimeTable.Rows[0];
                CDT = DateTime.Parse(DT.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + " " + dr1[P_HourIndex].ToString() + ":00", CultureInfo.InvariantCulture);
                CDT = CDT.AddDays(CDT_addedDays);
                duration = DT.Subtract(CDT);
                PreviousBossTimeLabel.Content = duration.ToString(@"hh\:mm\:ss");
                Console.WriteLine("Row: " + P_DayIndex + " " + "Column: " + P_HourIndex);
                try
                {
                    DataRow DrT = TimeTable.Rows[0];
                    string Time = DrT[P_HourIndex].ToString();
                    DrT = TimeTable.Rows[P_DayIndex];
                    string Day = DrT[0].ToString();
                    PreviousBossTimeLabel.ToolTip = Day + " " + Time;
                }
                catch (Exception) { }
                dr1 = TimeTable.Rows[P_DayIndex];
                PbossNamLabel.Content = dr1[P_HourIndex].ToString().Replace("," + Environment.NewLine, " & ");
                #endregion

                ////filter                              
                try
                {
                    BossesCollection.Clear();
                    BossCollectionListBox.Items.Clear();
                }
                catch (Exception) { }
                string BossesList = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Bosses");
                string[] BossListSplited = BossesList.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                foreach (string BossData in BossListSplited)
                {
                    if (BossData != "")
                    {
                        BossesCollection.Add(BossData.Replace(Environment.NewLine, ""));
                        string bossName = BossData.Substring(0, BossData.IndexOf(",") + 1);
                        bossName = bossName.Replace(",", "");
                        bossName = bossName.Replace(Environment.NewLine, "");
                        BossCollectionListBox.Items.Add(bossName);
                    }
                }

                PBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/Boss.png"));
                NBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/Boss.png"));
                LBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/Boss.png"));
                Getimg gm = new Getimg();

                string First_boss = "";
                string Second_boss = "";
                foreach (string item in BossesCollection)
                {
                    if (PbossNamLabel.Content.ToString().Contains(item.Split(',')[0]))
                    {
                        string[] bossdata = item.Split(',');
                        string imgurl = bossdata[1].ToString();
                        if (bossdata[5].ToString() != "")
                        {
                            imgurl = bossdata[5].ToString();
                            if (imgurl.Contains("<Local>"))
                            { imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
                        }
                        try { PBImageBox.Source = gm.GETIMAGE(imgurl); } catch (Exception) { }
                        if (PbossNamLabel.Content.ToString().Contains("&"))
                        { try { PBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/" + PbossNamLabel.Content.ToString() + ".png"); } catch (Exception) { } }
                    }
                    if (CbossNameLabel.Content.ToString().Contains(item.Split(',')[0]))
                    {
                        string[] bossdata = item.Split(',');
                        try
                        {
                            string imgurl = bossdata[1].ToString();
                            if (bossdata[5].ToString() == "")
                            {
                                imgurl = bossdata[1].ToString();
                            }
                            else
                            {
                                imgurl = bossdata[5].ToString();
                                if (imgurl.Contains("<Local>"))
                                { imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
                            }
                            publicNbossimage = bossdata[1].ToString();
                            if (!CbossNameLabel.Content.ToString().Contains("&"))
                            { publicbossUrl = bossdata[2].ToString(); }
                            else
                            {
                                if (publicbossUrl == "")
                                { publicbossUrl = bossdata[2].ToString(); }
                                else
                                { publicbossUrl += " | " + bossdata[2].ToString(); }
                            }
                            NBImageBox.Source = gm.GETIMAGE(imgurl);
                            if (CbossNameLabel.Content.ToString().Contains("&"))
                            { try { NBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/" + CbossNameLabel.Content.ToString() + ".png"); } catch (Exception) { } }

                        }
                        catch (Exception) { }
                    }

                    if (cSecondboss == item.Split(',')[0] && CbossNameLabel.Content.ToString().Contains("&") && cSecondboss != "")
                    {
                        string[] bossdata = item.Split(',');
                        if (publicbossUrl == "")
                        { publicbossUrl = bossdata[2].ToString(); }
                        else
                        { publicbossUrl += "|" + bossdata[2].ToString(); }
                    }
                    if (NBossNameLabel.Content.ToString().Contains(item.Split(',')[0]))
                    {
                        string[] bossdata = item.Split(',');
                        string imgurl = bossdata[1].ToString();
                        if (bossdata[5].ToString() != "")
                        {
                            imgurl = bossdata[5].ToString();
                            if (imgurl.Contains("<Local>"))
                            { imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
                        }
                        try { LBImageBox.Source = gm.GETIMAGE(imgurl); } catch (Exception) { }
                        if (NBossNameLabel.Content.ToString().Contains("&"))
                        { try { LBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/" + NBossNameLabel.Content.ToString() + ".png"); } catch (Exception) { } }
                        //if (bossdata[3].ToString() != "")
                        //{ NBossNameLabel.Content = bossdata[3].ToString(); }
                    }

                }

                if (PbossNamLabel.Content.ToString().Contains(" & "))//here
                {
                    string fullbossname = "";
                    foreach (string item in BossesCollection)
                    {
                        string[] bossdata = item.Split(',');
                        string name = PbossNamLabel.Content.ToString();
                        name = name.Substring(0, name.IndexOf("&"));
                        if (name.Contains(bossdata[0].ToString()))
                        {
                            if (bossdata[3].ToString() != "")
                            {
                                fullbossname += bossdata[3].ToString() + " & ";
                            }
                        }
                    }
                    foreach (string item in BossesCollection)
                    {
                        string[] bossdata = item.Split(',');
                        string name = PbossNamLabel.Content.ToString();
                        name = name.Substring(name.IndexOf("&") + 1);
                        if (name.Contains(bossdata[0].ToString()))
                        {
                            if (bossdata[3].ToString() != "")
                            {
                                fullbossname += bossdata[3].ToString();
                            }
                        }
                    }
                    if (fullbossname != "")
                    { PbossNamLabel.Content = fullbossname; }

                }
                else
                {
                    foreach (string item in BossesCollection)
                    {
                        string[] bossdata = item.Split(',');
                        if (PbossNamLabel.Content.ToString().Contains(bossdata[0].ToString()))
                        {
                            if (bossdata[3].ToString() != "")
                            { PbossNamLabel.Content = bossdata[3].ToString(); }
                        }

                    }
                }

                if (CbossNameLabel.Content.ToString().Contains(" & "))
                {
                    string fullbossname = "";
                    foreach (string item in BossesCollection)
                    {
                        string[] bossdata = item.Split(',');
                        string name = CbossNameLabel.Content.ToString();
                        name = name.Substring(0, name.IndexOf("&"));
                        First_boss = name;
                        if (name.Contains(bossdata[0].ToString()))
                        {
                            string[] br = bossdata[4].ToString().Split('{');
                            currentbossrole1 = br[1].ToString();
                            if (bossdata[3].ToString() != "")
                            {
                                fullbossname += bossdata[3].ToString() + " & ";
                            }
                        }
                    }
                    foreach (string item in BossesCollection)
                    {
                        string[] bossdata = item.Split(',');
                        string name = CbossNameLabel.Content.ToString();
                        name = name.Substring(name.IndexOf("&") + 1);
                        Second_boss = name;
                        if (name.Contains(bossdata[0].ToString()))
                        {
                            string[] br = bossdata[4].ToString().Split('{');
                            currentbossrole2 = br[1].ToString();
                            if (bossdata[3].ToString() != "")
                            {
                                fullbossname += bossdata[3].ToString();
                            }
                        }
                    }
                    if (fullbossname != "")
                    { CbossNameLabel.Content = fullbossname; }
                }
                else
                {
                    foreach (string item in BossesCollection)
                    {
                        string[] bossdata = item.Split(',');
                        if (CbossNameLabel.Content.ToString().TrimStart().TrimEnd() == bossdata[0].ToString())
                        {
                            if (bossdata[3].ToString() != "")
                            {
                                string[] br = bossdata[4].ToString().Split('{');
                                currentbossrole1 = br[1].ToString();
                                CbossNameLabel.Content = bossdata[3].ToString();
                            }
                            else
                            {
                                string[] br = bossdata[4].ToString().Split('{');
                                currentbossrole1 = br[1].ToString();
                            }
                        }

                    }
                }
                if (NBossNameLabel.Content.ToString().Contains(" & "))
                {
                    string fullbossname = "";
                    foreach (string item in BossesCollection)
                    {
                        string[] bossdata = item.Split(',');
                        string name = NBossNameLabel.Content.ToString();
                        name = name.Substring(0, name.IndexOf("&"));
                        if (name.Contains(bossdata[0].ToString()))
                        {
                            if (bossdata[3].ToString() != "")
                            {
                                fullbossname += bossdata[3].ToString() + " & ";
                            }
                        }
                    }
                    foreach (string item in BossesCollection)
                    {
                        string[] bossdata = item.Split(',');
                        string name = NBossNameLabel.Content.ToString();
                        name = name.Substring(name.IndexOf("&") + 1);
                        if (name.Contains(bossdata[0].ToString()))
                        {
                            if (bossdata[3].ToString() != "")
                            {
                                fullbossname += bossdata[3].ToString();
                            }
                        }
                    }
                    if (fullbossname != "")
                    { NBossNameLabel.Content = fullbossname; }
                }
                else
                {
                    foreach (string item in BossesCollection)
                    {
                        string[] bossdata = item.Split(',');
                        if (NBossNameLabel.Content.ToString().Contains(bossdata[0].ToString()))
                        {
                            if (bossdata[3].ToString() != "")
                            { NBossNameLabel.Content = bossdata[3].ToString(); }
                        }

                    }
                }

                Calculate_NightTime();
                Calculate_ImperialReset();
                Calculate_Bartering();
                Calculate_ImperialTradingReset();
                TimeTable.AcceptChanges();
                gridview1.ItemsSource = TimeTable.DefaultView;
                if (currentbossrole1 != "")
                {
                    if (currentbossrole1 != "@everyone" && currentbossrole1 != "@here")
                    { currentbossrole1 = "<@&" + currentbossrole1 + ">"; }
                }
                if (currentbossrole2 != "")
                {
                    if (currentbossrole2 != "@everyone" && currentbossrole2 != "@here")
                    { currentbossrole2 = "<@&" + currentbossrole2 + ">"; }
                }
                SharedDay = DayIndex;
                gridview1.ItemsSource = TimeTable.DefaultView;
                try { gridview1.SelectedIndex = SharedDay; } catch (Exception) { }
                if (CbossNameLabel.Content.ToString().Contains(" & "))
                {
                    publicNbossimage = "https://raw.githubusercontent.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/master/Resources/Images/" + First_boss.Replace(" ", "") + "%20%26%20" + Second_boss.Replace(" ", "") + ".png";
                }
                if (discord.ConnectionState == Discord.ConnectionState.Connected && isposting == 1)
                {
                    try
                    {
                        var guild = discord.GetGuild(ServerID);
                        var channel = guild.GetTextChannel(Main_BotChannel_ID);
                        var Message = await channel.GetMessageAsync(bossImageID) as IUserMessage;
                        string[] pbu = publicbossUrl.Split('|');
                        string[] bnu = CbossNameLabel.Content.ToString().Split('&');
                        string ANmessage = "";
                        if (CbossNameLabel.Content.ToString().Contains("&"))
                        {
                            ANmessage = LanguageCollection[123].ToString() + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[85].ToString() + Environment.NewLine + "[" + bnu[1] + "](" + pbu[1] + ")" + " <---" + LanguageCollection[85].ToString();
                        }
                        else
                        {
                            ANmessage = LanguageCollection[123].ToString() + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[85].ToString();
                        }
                        var embed1 = new EmbedBuilder
                        {
                            Title = CbossNameLabel.Content.ToString() /*+ " <---" + LanguageCollection[87].ToString()*/,
                            ImageUrl = publicNbossimage,
                            Color = Color.LightGrey,
                            //Url = publicbossUrl  
                            Description = ANmessage
                        };
                        await Message.ModifyAsync(msg => msg.Embed = embed1.Build());
                    }
                    catch (Exception) { }
                }

                TimeSpan CC = TimeSpan.FromHours(1);
                if (EditSpawnHoursSlider.Value.ToString().Contains("-"))
                {
                    TimeSpan PDT = TimeSpan.Parse(PreviousBossTimeLabel.Content.ToString());
                    PreviousBossTimeLabel.Content = PDT.Add(CC).ToString(@"hh\:mm\:ss");

                    TimeSpan CDTI = TimeSpan.Parse(CurrentBossTimeLabel.Content.ToString());
                    CurrentBossTimeLabel.Content = CDTI.Subtract(CC).ToString(@"hh\:mm\:ss");

                    TimeSpan NDT = TimeSpan.Parse(NextBossTimeLabel.Content.ToString());
                    NextBossTimeLabel.Content = NDT.Subtract(CC).ToString(@"hh\:mm\:ss");
                }
                if (EditSpawnHoursSlider.Value > 0)
                {
                    TimeSpan PDT = TimeSpan.Parse(PreviousBossTimeLabel.Content.ToString());
                    PreviousBossTimeLabel.Content = PDT.Subtract(CC).ToString(@"hh\:mm\:ss");

                    TimeSpan CDTI = TimeSpan.Parse(CurrentBossTimeLabel.Content.ToString());
                    CurrentBossTimeLabel.Content = CDTI.Add(CC).ToString(@"hh\:mm\:ss");

                    TimeSpan NDT = TimeSpan.Parse(NextBossTimeLabel.Content.ToString());
                    NextBossTimeLabel.Content = NDT.Add(CC).ToString(@"hh\:mm\:ss");
                }
                if (discord.ConnectionState == Discord.ConnectionState.Connected && isposting == 1)
                {
                    if (DisplayTimeTableSetting.IsChecked == true)
                    {
                        if (TimtableID != 0)
                        {
                            var guild = discord.GetGuild(ServerID);
                            var channel = guild.GetTextChannel(Main_BotChannel_ID);
                            await channel.DeleteMessageAsync(TimtableID);
                            TimtableID = 0;
                        }

                        if (TimtableID == 0)
                        {
                            try { gridview1.SelectedIndex = SharedDay; } catch (Exception) { }
                            if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"))
                            { File.Delete(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"); }
                            //int width = int.Parse(gridview1.ActualWidth.ToString());//error
                            //int height = int.Parse(gridview1.ActualHeight.ToString());
                            RenderTargetBitmap renderTargetBitmap =
                           new RenderTargetBitmap(1920, 1080, 180, 250, PixelFormats.Pbgra32);
                            renderTargetBitmap.Render(gridview1);
                            PngBitmapEncoder pngImage = new PngBitmapEncoder();
                            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                            using (Stream fileStream = File.Create(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png"))
                            {
                                pngImage.Save(fileStream);
                            }
                            var guild = discord.GetGuild(ServerID);
                            var channel = guild.GetTextChannel(Main_BotChannel_ID);
                            var timetablemessage = await channel.SendFileAsync(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png", MOTR.ToString().ToUpper() + LanguageCollection[43].ToString());
                            TimtableID = timetablemessage.Id;
                        }
                    }
                }
                //SaveLatestTimeTable = 0;               
                timer1.Start();
            }
            catch (Exception s)
            {
                ErrorMessageBox emb = new ErrorMessageBox(LanguageCollection[119].ToString(), LanguageCollection[120].ToString(), LanguageCollection[121].ToString(), LanguageCollection[122].ToString());
                emb.MB_typeOK(LanguageCollection[117].ToString(), s.Message + Environment.NewLine + Environment.NewLine + LanguageCollection[118].ToString(), this);
                emb.Show();
                this.IsEnabled = false;
            }
        }      
        private void Calculate_NightTime()
        {           
            DateTime DT = DateTime.Parse(DateTime.UtcNow.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + " 00:00:00", CultureInfo.InvariantCulture);
            DateTime DTN = DateTime.UtcNow;
            while (true)
            {
                DT = DT.AddHours(4);
                if (DTN < DT)
                { break; }
            }          
            TimeSpan DRE = TimeSpan.FromMinutes(40); 
            TimeSpan DRI = TimeSpan.FromMinutes(20);
            TimeSpan Duration = DT.Subtract(DTN);
            TimeSpan DRc = TimeSpan.Parse("03:40:00");

            if (Duration > DRE && Duration < DRc)
            {
                NILabel.Content = LanguageCollection[25].ToString();
                Duration = Duration.Subtract(DRI);
                NightInBdoTimeLabel.Content = Duration.ToString(@"hh\:mm\:ss");
            }
            else
            {
                NILabel.Content = LanguageCollection[107].ToString();
                Duration = Duration.Add(DRI);
                TimeSpan Di = TimeSpan.FromHours(3);
                if(Duration > Di)
                {
                    Di = TimeSpan.FromHours(4);
                    Duration = Duration.Subtract(Di);
                }
                NightInBdoTimeLabel.Content = Duration.ToString(@"hh\:mm\:ss");
            }          
        }
        private void Calculate_ImperialReset()
        {
            DateTime DT = DateTime.Parse(DateTime.UtcNow.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + " 00:00:00", CultureInfo.InvariantCulture);
            DateTime DTN = DateTime.UtcNow;
            
            while (true)
            {
                DT = DT.AddHours(3);
                if (DTN < DT)
                { break; }
            }
            TimeSpan Duration = DTN.Subtract(DT);
            IRTimeLabel.Content = Duration.ToString(@"hh\:mm\:ss");
           
        }
        private void Calculate_Bartering()
        {
            DateTime DT = DateTime.Parse(DateTime.UtcNow.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + " 00:00:00", CultureInfo.InvariantCulture);
            DateTime DTN = DateTime.UtcNow;
            while(true)
            {
                DT = DT.AddHours(4);
                if (DTN < DT)
                { break; }
            }
            TimeSpan Duration = DTN.Subtract(DT);
            BRTimeLabel.Content = Duration.ToString(@"hh\:mm\:ss");
          
        }
        private void Calculate_ImperialTradingReset()
        {
            DateTime DT = DateTime.Parse(DateTime.UtcNow.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + " 00:00:00", CultureInfo.InvariantCulture);
            DateTime DTN = DateTime.UtcNow;
            while (true)
            {
                DT = DT.AddHours(4);
                if (DTN < DT)
                { break; }
            }
            TimeSpan Duration = DTN.Subtract(DT);
            ITRITimeLabel.Content = Duration.ToString(@"hh\:mm\:ss");
          
        }

        private void gridview1_CurrentCellChanged(object sender, EventArgs e)
        {
            //gridview1.SelectedIndex = -1;
        }
        private void gridview1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //DataRowView dataRow = (DataRowView)gridview1.SelectedItem;
                //int index = gridview1.CurrentCell.Column.DisplayIndex;
                //string cellValue = dataRow.Row.ItemArray[index].ToString();
                DataRow dr = TimeTable.Rows[0];
                DataGridCellInfo cellInfo = gridview1.CurrentCell;
                DataGridColumn column = cellInfo.Column;
                gridview1Column = column.DisplayIndex;
                gridview1Row = gridview1.SelectedIndex;
                if (gridview1Column > 0)
                { RemoveTimeTableColumn.Visibility = Visibility.Visible; }
                else
                { RemoveTimeTableColumn.Visibility = Visibility.Hidden; }
                TimeTableBoss1DropBox.IsEnabled = true; TimeTableBoss2DropBox.IsEnabled = true;
                if (gridview1Row == 0 || gridview1Column == 0 )
                { TimeTableBoss1DropBox.IsEnabled = false; TimeTableBoss2DropBox.IsEnabled = false; }
                TimeColumnHoursDropBox.IsEnabled = true; 
                TimeColumnMinutesDropBox.IsEnabled = true;
                if (gridview1Column == 0)
                {
                    TimeColumnHoursDropBox.IsEnabled = false; TimeColumnHoursDropBox.SelectedIndex = -1;
                    TimeColumnMinutesDropBox.IsEnabled = false; TimeColumnMinutesDropBox.SelectedIndex = -1;
                    TimeTableBoss1DropBox.Text = "None";
                    TimeTableBoss2DropBox.Text = "None";
                }
                string input = dr[gridview1Column].ToString();
                string Hours = input.Substring(0, input.IndexOf(":") + 0);
                string Minutes = input.Substring(input.IndexOf(":") + 1);
                TimeColumnHoursDropBox.Text = Hours;
                TimeColumnMinutesDropBox.Text = Minutes;
                DataRow dr1 = TimeTable.Rows[gridview1Row];
                TimeTableBoss1DropBox.Text = "None";
                TimeTableBoss2DropBox.Text = "None";
                last_HM_Set = TimeColumnHoursDropBox.Text + ":" + TimeColumnMinutesDropBox.Text;
                if (gridview1Row > 0)
                {
                    if (dr1[gridview1Column].ToString().Contains(","))
                    {
                        input = dr1[gridview1Column].ToString();
                        string Boss1 = input.Substring(0, input.IndexOf(",") + 0);
                        string Boss2 = input.Substring(input.IndexOf(",") + 1);
                        Boss2 = Boss2.Replace(Environment.NewLine, "");
                        TimeTableBoss1DropBox.Text = Boss1;
                        TimeTableBoss2DropBox.Text = Boss2;
                    }
                    else
                    {
                        if (dr1[gridview1Column].ToString() == "")
                        { TimeTableBoss1DropBox.Text = "None"; }
                        else
                        { TimeTableBoss1DropBox.Text = dr1[gridview1Column].ToString(); }
                    }
                }
                             
            }
            catch (Exception) { }
        }
        private void TimeColumnHoursDropBox_DropDownClosed(object sender, EventArgs e)
        {
            if (gridview1Column > 0 && last_HM_Set != TimeColumnHoursDropBox.Text + ":" + TimeColumnMinutesDropBox.Text && TimeColumnHoursDropBox.SelectedIndex > -1 && TimeColumnMinutesDropBox.SelectedIndex > -1)
            {
                DataRow dr = TimeTable.Rows[0];
                dr[gridview1Column] = TimeColumnHoursDropBox.Text + ":" + TimeColumnMinutesDropBox.Text;
                Correct_GridViewColumns_Order();
            }
        }
        private void TimeColumnMinutesDropBox_DropDownClosed(object sender, EventArgs e)
        {
            if (gridview1Column > 0 && last_HM_Set != TimeColumnHoursDropBox.Text + ":" + TimeColumnMinutesDropBox.Text && TimeColumnHoursDropBox.SelectedIndex > -1 && TimeColumnMinutesDropBox.SelectedIndex > -1)
            {
                DataRow dr = TimeTable.Rows[0];
                dr[gridview1Column] = TimeColumnHoursDropBox.Text + ":" + TimeColumnMinutesDropBox.Text;
                Correct_GridViewColumns_Order();
            }
        }
        private void Correct_GridViewColumns_Order()
        {
            try
            {
                RemoveTimeTableColumn.Visibility = Visibility.Hidden;
                var Times = new List<TimeSpan>();
                var Timelist = new List<string>();
                var ColumnList = new List<string>();
                int clv = 0;
                foreach (var cli in TimeTable.Columns)
                {
                    if (clv > 0)
                    {
                        DataRow dr = TimeTable.Rows[0];
                        TimeSpan DT = TimeSpan.Parse(dr[clv].ToString());
                        Times.Add(DT);
                        ColumnList.Add(cli.ToString());
                        Timelist.Add(dr[clv].ToString());
                    }
                    clv++;
                }
                Times.Sort((x, y) => y.CompareTo(x));
                Times.Reverse();
                int tc = 1;

                foreach (var dt in Times)
                {

                    int fcli = 1;
                    int cli = 0;
                    foreach (var cl in ColumnList)
                    {
                        if (dt.ToString(@"hh\:mm") == Timelist[cli].ToString())
                        {
                            fcli = cli;
                        }
                        cli++;
                    }
                    TimeTable.Columns[ColumnList[fcli].ToString()].SetOrdinal(tc);
                    tc++;
                }
                gridview1.ItemsSource = null;
                TimeTable.AcceptChanges();
                gridview1.ItemsSource = TimeTable.DefaultView;
                TimeColumnHoursDropBox.SelectedIndex = -1;
                TimeColumnMinutesDropBox.SelectedIndex = -1;
                TimeTableBoss1DropBox.Text = "None";
                TimeTableBoss2DropBox.Text = "None";

            }
            catch (Exception) { }
        }
       

        private void TimeTableAddColumnButton_Click(object sender, RoutedEventArgs e)
        {          
            DataRow dr = TimeTable.Rows[0];
            TimeSpan DTA = TimeSpan.FromMinutes(1);
            TimeSpan DT = TimeSpan.Parse(dr[TimeTable.Columns.Count-1].ToString());
            DT = DT.Add(DTA);
            TimeTable.Columns.Add("");
            gridview1Column = TimeTable.Columns.Count - 1;
            dr[gridview1Column] = DT.ToString(@"hh\:mm");
            gridview1.ItemsSource = null;
            TimeTable.AcceptChanges();
            gridview1.ItemsSource = TimeTable.DefaultView;
            Correct_GridViewColumns_Order();
            string input = DT.ToString(@"hh\:mm");
            string Hours = input.Substring(0, input.IndexOf(":") + 0);
            string Minutes = input.Substring(input.IndexOf(":") + 1);
            TimeColumnHoursDropBox.Text = Hours;
            TimeColumnMinutesDropBox.Text = Minutes;
            gridview1.SelectedIndex = 0;           
        }
        private void RemoveTimeTableColumn_Click(object sender, RoutedEventArgs e)
        {
            if(TimeColumnHoursDropBox.SelectedIndex > -1 && TimeColumnMinutesDropBox.SelectedIndex > -1 && gridview1Column > 0)
            {
                System.Data.DataColumn clo = new System.Data.DataColumn();
                DataRow dr = TimeTable.Rows[0];
                int ct = 0;
                foreach(System.Data.DataColumn cl in TimeTable.Columns)
                {
                    if (dr[ct].ToString() == TimeColumnHoursDropBox.Text.ToString() + ":" + TimeColumnMinutesDropBox.Text.ToString())
                    {
                        clo = cl;
                    }
                    ct++;
                }
                TimeTable.Columns.Remove(clo);
                gridview1.ItemsSource = null;
                TimeTable.AcceptChanges();
                gridview1.ItemsSource = TimeTable.DefaultView;
            }          
        }
        private void RestoreTimeTableDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            string Origin_LYPBBTTT_Data = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT_Origin");
            File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT", Origin_LYPBBTTT_Data);
            GetLYPBBTTimeTable();
        }
        private void TimeTableBoss1DropBox_DropDownClosed(object sender, EventArgs e)
        {
            if (gridview1Column > 0 && gridview1Row > 0)
            {
                if (TimeTableBoss1DropBox.Text == "None" && TimeTableBoss2DropBox.Text != "None")
                {
                    TimeTableBoss1DropBox.Text = TimeTableBoss2DropBox.Text;
                    TimeTableBoss2DropBox.Text = "None";
                }
                DataRow dr = TimeTable.Rows[gridview1Row];
                if (TimeTableBoss1DropBox.Text != "None")
                {
                    if (TimeTableBoss2DropBox.Text == "None")
                    { dr[gridview1Column] = TimeTableBoss1DropBox.Text; }
                    else
                    { dr[gridview1Column] = TimeTableBoss1DropBox.Text + "," + Environment.NewLine + TimeTableBoss2DropBox.Text; }
                }
                else
                { dr[gridview1Column] = ""; }
            }
        }

        private void TimeTableBoss2DropBox_DropDownClosed(object sender, EventArgs e)
        {
            if (gridview1Column > 0 && gridview1Row > 0)
            {
                if (TimeTableBoss1DropBox.Text == "None" && TimeTableBoss2DropBox.Text != "None")
                {
                    TimeTableBoss1DropBox.Text = TimeTableBoss2DropBox.Text;
                    TimeTableBoss2DropBox.Text = "None";
                }
                DataRow dr = TimeTable.Rows[gridview1Row];
                if (TimeTableBoss1DropBox.Text != "None")
                {
                    if (TimeTableBoss2DropBox.Text == "None")
                    { dr[gridview1Column] = TimeTableBoss1DropBox.Text; }
                    else
                    { dr[gridview1Column] = TimeTableBoss1DropBox.Text + "," + Environment.NewLine + TimeTableBoss2DropBox.Text; }
                }
                else
                { dr[gridview1Column] = ""; }
            }
        }
        private void Save_LYPBBTTT()
        {
            string LYPBBTTT_Data = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT");
            string RegionTimeTable = GetStrBetweenTags(LYPBBTTT_Data, "[" + RegionComboBox.Text + "]", "[/" + RegionComboBox.Text + "]");
            string orignal_tablet = "[" + RegionComboBox.Text + "]" + RegionTimeTable + "[/" + RegionComboBox.Text + "]";
            string output = "";
            int y = 0;           
            foreach(var cr in TimeTable.Rows)
            {
                if(output != "")
                { output += Environment.NewLine; }
                DataRow dr = TimeTable.Rows[y];
                int x = 0;
                foreach (var cl in TimeTable.Columns)
                {
                    if (x > 0)
                    {
                        if (x < TimeTable.Columns.Count - 1)
                        { output += dr[x].ToString().Replace(Environment.NewLine, "") + "|"; }
                        else
                        { output += dr[x].ToString().Replace(Environment.NewLine, ""); }
                    }
                    x++;
                }
                y++;
            }
            string New_table = "[" + RegionComboBox.Text + "]" + Environment.NewLine + output + Environment.NewLine + "[/" + RegionComboBox.Text + "]";
            LYPBBTTT_Data = LYPBBTTT_Data.Replace(orignal_tablet, New_table);
            File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT",LYPBBTTT_Data);
        }
        private void alertbosstab_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            alertstabcontrol.BeginAnimation(OpacityProperty, da);
            alertstabcontrol.SelectedIndex = 0;

            var bc = new BrushConverter();
            alertbosstabrectangle.Fill = (Brush)bc.ConvertFrom(subcolor);
            alertnighttabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertimperialtabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertbarteringtabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertimperialresettabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
        }

        private void alertnighttabbutton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            alertstabcontrol.BeginAnimation(OpacityProperty, da);
            alertstabcontrol.SelectedIndex = 1;

            var bc = new BrushConverter();
            alertbosstabrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertnighttabbuttonrectangle.Fill = (Brush)bc.ConvertFrom(subcolor);
            alertimperialtabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertbarteringtabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertimperialresettabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
        }

        private void alertimperialtabbutton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            alertstabcontrol.BeginAnimation(OpacityProperty, da);
            alertstabcontrol.SelectedIndex = 2;

            var bc = new BrushConverter();
            alertbosstabrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertnighttabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertimperialtabbuttonrectangle.Fill = (Brush)bc.ConvertFrom(subcolor);
            alertbarteringtabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertimperialresettabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
        }

        private void alertbarteringtabbutton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            alertstabcontrol.BeginAnimation(OpacityProperty, da);
            alertstabcontrol.SelectedIndex = 3;

            var bc = new BrushConverter();
            alertbosstabrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertnighttabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertimperialtabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertbarteringtabbuttonrectangle.Fill = (Brush)bc.ConvertFrom(subcolor);
            alertimperialresettabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
        }

        private void alertimperialresettabbutton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            alertstabcontrol.BeginAnimation(OpacityProperty, da);
            alertstabcontrol.SelectedIndex = 4;

            var bc = new BrushConverter();
            alertbosstabrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertnighttabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertimperialtabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertbarteringtabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertimperialresettabbuttonrectangle.Fill = (Brush)bc.ConvertFrom(subcolor);
        }

        private void exssoundbutton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            extrasettingstabcontrol.BeginAnimation(OpacityProperty, da);
            extrasettingstabcontrol.SelectedIndex = 0;

            var bc = new BrushConverter();
            exssoundbuttonrectangle.Fill = (Brush)bc.ConvertFrom(subcolor);
            exstimetablebuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            exsalertbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
        }

        private void exstimetablebutton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            extrasettingstabcontrol.BeginAnimation(OpacityProperty, da);
            extrasettingstabcontrol.SelectedIndex = 1;

            var bc = new BrushConverter();
            exssoundbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            exstimetablebuttonrectangle.Fill = (Brush)bc.ConvertFrom(subcolor);
            exsalertbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
        }

        private void exsalertbutton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            extrasettingstabcontrol.BeginAnimation(OpacityProperty, da);
            extrasettingstabcontrol.SelectedIndex = 2;

            var bc = new BrushConverter();
            exssoundbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            exstimetablebuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            exsalertbuttonrectangle.Fill = (Brush)bc.ConvertFrom(subcolor);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if(tabcontrol1.SelectedIndex == 3)
            { SaveGlobalSettings(); }
            if (tabcontrol1.SelectedIndex == 2)
            { savebosslist(); }
            if(SourceComboBox.SelectedIndex == 0 && tabcontrol1.SelectedIndex == 1)
            { Save_LYPBBTTT();GetLYPBBTTimeTable(); }
            TimeTableGrid.Visibility = Visibility.Hidden;
            alertstabcontrol.SelectedIndex = 0;
            extrasettingstabcontrol.SelectedIndex = 0;
            var bc = new BrushConverter();
            alertbosstabrectangle.Fill = (Brush)bc.ConvertFrom(subcolor);
            Homehighlight.Fill = (Brush)bc.ConvertFrom(subcolor);
            Timetablehighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            bosslisthighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            SelfRollingHighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            settingshighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            abouthighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertnighttabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertimperialtabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertbarteringtabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            alertimperialresettabbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            exssoundbuttonrectangle.Fill = (Brush)bc.ConvertFrom(subcolor);
            exstimetablebuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            exsalertbuttonrectangle.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            tabcontrol1.BeginAnimation(OpacityProperty, da);
            tabcontrol1.SelectedIndex = 0;
            Homehighlight.Fill = (Brush)bc.ConvertFrom(subcolor);
            Timetablehighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            bosslisthighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            SelfRollingHighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            settingshighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            abouthighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
        }      
        private void BSA1CMTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BSA1RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = BSA1RGrid.ActualHeight;
                daH.To = BSA1RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                BSA1RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }
        private void BSA1CMTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (BSA1RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = BSA1RGrid.ActualHeight;
                daH.To = BSA1RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                BSA1RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void BSA2CMTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BSA2RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = BSA2RGrid.ActualHeight;
                daH.To = BSA2RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                BSA2RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void BSA2CMTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (BSA2RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = BSA2RGrid.ActualHeight;
                daH.To = BSA2RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                BSA2RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void BSA3CMTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BSA3RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = BSA3RGrid.ActualHeight;
                daH.To = BSA3RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                BSA3RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void BSA3CMTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (BSA3RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = BSA3RGrid.ActualHeight;
                daH.To = BSA3RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                BSA3RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void NTA1CMTextbBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NTA1RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = NTA1RGrid.ActualHeight;
                daH.To = NTA1RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                NTA1RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void NTA1CMTextbBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NTA1RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = NTA1RGrid.ActualHeight;
                daH.To = NTA1RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                NTA1RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void NTA2CMTextbBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NTA2RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = NTA2RGrid.ActualHeight;
                daH.To = NTA2RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                NTA2RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void NTA2CMTextbBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NTA2RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = NTA2RGrid.ActualHeight;
                daH.To = NTA2RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                NTA2RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void NTA3CMTextbBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NTA3RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = NTA3RGrid.ActualHeight;
                daH.To = NTA3RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                NTA3RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void NTA3CMTextbBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NTA3RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = NTA3RGrid.ActualHeight;
                daH.To = NTA3RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                NTA3RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void IRA1CMTextBox_GotFocus(object sender, RoutedEventArgs e)
        {            
            if (IRA1RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = IRA1RGrid.ActualHeight;
                daH.To = IRA1RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                IRA1RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void IRA1CMTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IRA1RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = IRA1RGrid.ActualHeight;
                daH.To = IRA1RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                IRA1RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void IRA2CMTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (IRA2RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = IRA2RGrid.ActualHeight;
                daH.To = IRA2RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                IRA2RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void IRA2CMTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IRA2RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = IRA2RGrid.ActualHeight;
                daH.To = IRA2RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                IRA2RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void IRA3CMTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (IRA3RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = IRA3RGrid.ActualHeight;
                daH.To = IRA3RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                IRA3RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void IRA3CMTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IRA3RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = IRA3RGrid.ActualHeight;
                daH.To = IRA3RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                IRA3RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void BRA1CMTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BRA1RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = BRA1RGrid.ActualHeight;
                daH.To = BRA1RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                BRA1RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }           
        }

        private void BRA1CMTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (BRA1RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = BRA1RGrid.ActualHeight;
                daH.To = BRA1RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                BRA1RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void BRA2CMTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BRA2RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = BRA2RGrid.ActualHeight;
                daH.To = BRA2RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                BRA2RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void BRA2CMTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (BRA2RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = BRA2RGrid.ActualHeight;
                daH.To = BRA2RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                BRA2RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void BRA3CMTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BRA3RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = BRA3RGrid.ActualHeight;
                daH.To = BRA3RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                BRA3RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void BRA3CMTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (BRA3RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = BRA3RGrid.ActualHeight;
                daH.To = BRA3RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                BRA3RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void ITRA1CMTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ITRA1RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = ITRA1RGrid.ActualHeight;
                daH.To = ITRA1RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                ITRA1RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }            
        }

        private void ITRA1CMTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ITRA1RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = ITRA1RGrid.ActualHeight;
                daH.To = ITRA1RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                ITRA1RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void ITRA2CMTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ITRA2RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = ITRA2RGrid.ActualHeight;
                daH.To = ITRA2RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                ITRA2RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void ITRA2CMTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ITRA2RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = ITRA2RGrid.ActualHeight;
                daH.To = ITRA2RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                ITRA2RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void ITRA3CMTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ITRA3RGrid.ActualHeight == 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = ITRA3RGrid.ActualHeight;
                daH.To = ITRA3RGrid.ActualHeight + 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                ITRA3RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }

        private void ITRA3CMTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ITRA3RGrid.ActualHeight > 28)
            {
                DoubleAnimation daH = new DoubleAnimation();
                daH.From = ITRA3RGrid.ActualHeight;
                daH.To = ITRA3RGrid.ActualHeight - 100;
                daH.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                ITRA3RGrid.BeginAnimation(Ellipse.HeightProperty, daH);
            }
        }
        public async void enableMW()
        {
            var bc = new BrushConverter();
            await discord.StopAsync();
            DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFBB3D3D");
            DiscordBotConnectionStatusLabel.Content = LanguageCollection[5].ToString();
            this.IsEnabled = true;
        }

        private async void LanguageDropBox_DropDownClosed(object sender, EventArgs e)
        {
            if (LanguageDropBox.SelectedIndex != language_changed)
            {
                language_changed = LanguageDropBox.SelectedIndex;

                if (LanguageDropBox.SelectedIndex == 0)
                {
                    DefaultLanguage = "fr";
                    LoadDefaultLanguage();
                }
                if (LanguageDropBox.SelectedIndex == 1)
                {
                    DefaultLanguage = "en";
                    LoadDefaultLanguage();
                }
                if (LanguageDropBox.SelectedIndex == 2)
                {
                    DefaultLanguage = "es";
                    LoadDefaultLanguage();
                }
                if (LanguageDropBox.SelectedIndex == 3)
                {
                    DefaultLanguage = "ru";
                    LoadDefaultLanguage();
                }
                if (LanguageDropBox.SelectedIndex == 4)
                {
                    DefaultLanguage = "jp";
                    LoadDefaultLanguage();
                }
                if (LanguageDropBox.SelectedIndex == 5)
                {
                    DefaultLanguage = "kr";
                    LoadDefaultLanguage();
                }

                if (SourceComboBox.SelectedIndex == 0)
                { Calculate_NightTime(); }
                if (discord.ConnectionState == Discord.ConnectionState.Connected && isposting == 1)
                {
                    try
                    {
                        var guild = discord.GetGuild(ServerID);
                        var channel = guild.GetTextChannel(Main_BotChannel_ID);
                        var Message = await channel.GetMessageAsync(bossImageID) as IUserMessage;
                        string[] pbu = publicbossUrl.Split('|');
                        string[] bnu = CbossNameLabel.Content.ToString().Split('&');
                        string ANmessage = "";
                        if (CbossNameLabel.Content.ToString().Contains("&"))
                        {
                            ANmessage = LanguageCollection[123].ToString() + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[85].ToString() + Environment.NewLine + "[" + bnu[1] + "](" + pbu[1] + ")" + " <---" + LanguageCollection[85].ToString();
                        }
                        else
                        {
                            ANmessage = LanguageCollection[123].ToString() + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[85].ToString();
                        }
                        var embed1 = new EmbedBuilder
                        {
                            Title = CbossNameLabel.Content.ToString() /*+ " <---" + LanguageCollection[87].ToString()*/,
                            ImageUrl = publicNbossimage,
                            Color = Color.LightGrey,
                            //Url = publicbossUrl  
                            Description = ANmessage
                        };
                        await Message.ModifyAsync(msg => msg.Embed = embed1.Build());
                    }
                    catch (Exception) { }
                }

            }
        }
        public void ReturnFixedToken(DiscordSocketClient ds,string tk)
        {
            this.IsEnabled = true;
            discord = ds;
            var bc = new BrushConverter();
            DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FF669174");
            DiscordBotConnectionStatusLabel.Content = LanguageCollection[8].ToString();//"Connected";
            Token = tk;            
            tabcontrol1.SelectedIndex = 0;
        }

        private void CBNTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            AutoSaveBossData();
        }

        private void SRFTBCombobox_DropDownClosed(object sender, EventArgs e)
        {          
            AutoSaveBossData();
        }

        private void BossSpawnLocationLinkTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AutoSaveBossData();
        }

        private void DisplayImageLinkextBoxLocal_LostFocus(object sender, RoutedEventArgs e)
        {
            AutoSaveBossData();
        }

        private void DisplayImageLinkextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AutoSaveBossData();
        }
        private void savebosslist()
        {
            string BossList = "";
            foreach (string item in BossesCollection)
            {
                if (item != "")
                { BossList += item + Environment.NewLine; }
            }
            try
            {
                File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Bosses", BossList.TrimEnd());
                Processing_Status(true, "Changes Been Saved");
            }
            catch (Exception) { Processing_Status(true, "Failed to Save", true); }
            
            if (BossesListData != BossList)
            {
                BossesListData = BossList;
                if (SourceComboBox.SelectedIndex == 0)
                { GetLYPBBTTimeTable(); }
                if (SourceComboBox.SelectedIndex == 1)
                { GetTimeTable(MOTR); }//Get info from Html Code
                if (SourceComboBox.SelectedIndex == 2)
                { GetUrlSource("https://bdobosstimer.com/?&server=" + MOTR); }
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            if (tabcontrol1.SelectedIndex == 3)
            { SaveGlobalSettings(); }
            if (tabcontrol1.SelectedIndex == 2)
            { savebosslist(); }
            if (SourceComboBox.SelectedIndex == 0 && tabcontrol1.SelectedIndex == 1)
            { Save_LYPBBTTT(); GetLYPBBTTimeTable(); }
            TimeTableGrid.Visibility = Visibility.Hidden;
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            tabcontrol1.BeginAnimation(OpacityProperty, da);
            tabcontrol1.SelectedIndex = 4;
            var bc = new BrushConverter();
            Homehighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            Timetablehighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            bosslisthighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            SelfRollingHighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            settingshighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            abouthighlight.Fill = (Brush)bc.ConvertFrom(subcolor);
        }

        private void MainColorPick_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(maincolor);
            var initialColor = color;
            var dialog = new ColorPickerDialog(initialColor);
            var bc = new BrushConverter();
            dialog.WindowStyle = WindowStyle.None;
            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;           
            dialog.BorderThickness = new Thickness(1.0);
            dialog.BorderBrush = (Brush)bc.ConvertFrom("#FF434349");            
            dialog.Background = (Brush)bc.ConvertFrom(maincolor);            
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var newColor = dialog.Color;
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString(newColor.ToString());//Default #FF28282B
                maincolor = newColor.ToString();
                Settings.Default["MainColor"] = newColor.ToString();
                Settings.Default.Save();
                Application.Current.Resources["MainColor"] = brush;
                int R = int.Parse(newColor.R.ToString());
                R = R + 4;
                int G = int.Parse(newColor.G.ToString());
                G = G + 5;
                int B = int.Parse(newColor.B.ToString());
                B = B + 7;
                System.Drawing.Color myColor = new System.Drawing.Color();
                try { myColor = System.Drawing.Color.FromArgb(R, G, B); } catch (Exception) { myColor = System.Drawing.Color.FromArgb(newColor.R, newColor.G, newColor.B); }               
                string hex = "#" + myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");
                brush = (Brush)converter.ConvertFromString(hex);
                Application.Current.Resources["HeaderColor"] = brush;

            }
        }
        private void ResetMainColorButton_Click(object sender, RoutedEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#A028282B");
            Settings.Default["MainColor"] = "#A028282B";
            Settings.Default.Save();
            maincolor = "#A028282B";
            Application.Current.Resources["MainColor"] = brush;
            Settings.Default["HeaderColor"] = "#962C2D32";
            Settings.Default.Save();
            brush = (Brush)converter.ConvertFromString("#962C2D32");
            Application.Current.Resources["HeaderColor"] = brush;

        }
        private void SubColorPick_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(subcolor);
            var initialColor = color;
            var dialog = new ColorPickerDialog(initialColor);
            var bc = new BrushConverter();
            dialog.WindowStyle = WindowStyle.None;
            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dialog.BorderThickness = new Thickness(1.0);
            dialog.BorderBrush = (Brush)bc.ConvertFrom("#FF434349");
            dialog.Background = (Brush)bc.ConvertFrom(maincolor);
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var newColor = dialog.Color;
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString(newColor.ToString());//Default #FF28282B
                subcolor = newColor.ToString();
                Settings.Default["SubColor"] = newColor.ToString();
                Settings.Default.Save();
                Application.Current.Resources["SubColor"] = brush;
                abouthighlight.Fill = (Brush)bc.ConvertFrom(subcolor);               
            }
        }

        private void ResetSubColorButton_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#FF8B81FC");
            Settings.Default["SubColor"] = "#FF8B81FC";
            Settings.Default.Save();
            subcolor = "#FF8B81FC";
            Application.Current.Resources["SubColor"] = brush;
            abouthighlight.Fill = (Brush)bc.ConvertFrom(subcolor);
        }

        private async void BossListRestoreDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            string finalBosslist = "";
            if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Bosses"))
            {
                string OriginBossesList = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/BossesOrigin");
                string[] BossListSplited = OriginBossesList.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                Settings.Default["OriginBossList"] = OriginBossesList;
                Settings.Default.Save();
                string BossesList = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Bosses");
                string[] BossListSP = BossesList.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                finalBosslist = BossesList;
                foreach (string BossData in BossListSP)
                {
                    if (BossData != "")
                    {
                        foreach (string BD in BossListSplited)
                        {
                            if (BD != "")
                            {
                                string[] BDS = BD.Split(',');
                                string[] bossdataS = BossData.Split(',');
                                if (bossdataS[0] == BDS[0])
                                {
                                    if (bossdataS.Length == 5)
                                    { finalBosslist = finalBosslist.Replace(bossdataS[0] + "," + bossdataS[1] + "," + bossdataS[2] + "," + bossdataS[3] + "," + bossdataS[4]
                                        , BDS[0] + "," + BDS[1] + "," + BDS[2] + "," + bossdataS[3] + "," + bossdataS[4] + "," + BDS[5]); }

                                    if (bossdataS.Length == 6)
                                    { finalBosslist = finalBosslist.Replace(bossdataS[0] + "," + bossdataS[1] + "," + bossdataS[2] + "," + bossdataS[3] + "," + bossdataS[4] + "," + bossdataS[5]
                                        , BDS[0] + "," + BDS[1] + "," + BDS[2] + "," + bossdataS[3] + "," + bossdataS[4] + "," + BDS[5]); }
                                }
                            }
                        }
                    }
                }
                foreach (string BD in BossListSplited)
                {
                    bool is_missing = true;
                    foreach(string boss in BossListSP)
                    {
                        if(boss.Split(',')[0].ToLower() == BD.Split(',')[0].ToLower())
                        { is_missing = false;}
                    }
                    if(is_missing)
                    { finalBosslist += Environment.NewLine + BD; }
                }
            }
            File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Bosses", finalBosslist);
            BossesListData = finalBosslist;
             try
             {
                BossesCollection.Clear();
                BossCollectionListBox.Items.Clear();
             }
             catch (Exception) { }
            string[] BossListSplited1 = BossesListData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (string BossData in BossListSplited1)
            {
                if (BossData != "")
                {
                    BossesCollection.Add(BossData.Replace(Environment.NewLine, ""));
                    string bossName = BossData.Substring(0, BossData.IndexOf(",") + 1);
                    bossName = bossName.Replace(",", "");
                    bossName = bossName.Replace(Environment.NewLine, "");
                    BossCollectionListBox.Items.Add(bossName);
                }
            }

            if (discord.ConnectionState == Discord.ConnectionState.Connected)
            {
                if (tabcontrol1.SelectedIndex == 3)
                { SaveGlobalSettings(); }
                GetIds();
                TimeTableGrid.Visibility = Visibility.Hidden;

                try { BossCollectionListBox.SelectedIndex = 0; } catch (Exception) { }
                try
                {
                    gridview1.SelectedIndex = SharedDay;
                    DoubleAnimation da = new DoubleAnimation();
                    da.From = 0;
                    da.To = 1;
                    da.Duration = new Duration(TimeSpan.FromSeconds(1));
                    da.AutoReverse = false;
                    tabcontrol1.BeginAnimation(OpacityProperty, da);
                }
                catch (Exception) { }
                tabcontrol1.SelectedIndex = 2;
                var bc = new BrushConverter();
                Homehighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                Timetablehighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                bosslisthighlight.Fill = (Brush)bc.ConvertFrom(subcolor);
                SelfRollingHighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                settingshighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                abouthighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
            }
            else
            {
                try
                {
                    var bc = new BrushConverter();

                    DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFF1F1F1");
                    DiscordBotConnectionStatusLabel.Content = LanguageCollection[6].ToString();//"Connecting...";
                    await discord.LoginAsync(TokenType.Bot, Token);
                    await discord.StartAsync();
                    System.Threading.Thread.Sleep(1000);

                    if (DiscordBotConnectionStatusLabel.Content.ToString() == LanguageCollection[6].ToString())
                    {
                        DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FF669174");
                        DiscordBotConnectionStatusLabel.Content = LanguageCollection[8].ToString();//"Connected";
                    }
                }
                catch (Exception)
                {
                    var bc = new BrushConverter();
                    DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFBB3D3D");
                    DiscordBotConnectionStatusLabel.Content = LanguageCollection[7].ToString();//"Connection ERROR!";
                    ErrorMessageBox emb = new ErrorMessageBox(LanguageCollection[119].ToString(), LanguageCollection[120].ToString(), LanguageCollection[121].ToString(), LanguageCollection[122].ToString());
                    emb.TestToken(LanguageCollection[112].ToString(), discord, this, Token);
                    emb.Show();
                    this.IsEnabled = false;

                    /*StartPosting();*/
                }
            }
        }

        private void ResetAnimatedBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default["AnimatedBackgroundSource"] = System.IO.Directory.GetCurrentDirectory() + @"\Resources\img\bckg.mp4";
            Settings.Default.Save();
            Settings.Default["AnimatedBackgroundCheckbox"] = "1";
            AnimatedBackgroundCheckBox.IsChecked = true;
            Settings.Default.Save();
            mediaElement.IsEnabled = true;
            var bc = new BrushConverter();
            PickAnimatedBackground.Foreground = (Brush)bc.ConvertFrom("#FFC6C6C9");
            PickAnimatedBackground.IsEnabled = true;
            mediaElement.Visibility = Visibility.Visible;
            mediaElement.LoadedBehavior = MediaState.Play;
            mediaElement.Source = new Uri(Settings.Default["AnimatedBackgroundSource"].ToString(), UriKind.Absolute);
            mediaElement.Play();
        }

        private void AnimatedBackgroundCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (AnimatedBackgroundCheckBox.IsChecked == true)
            {
                Settings.Default["AnimatedBackgroundCheckbox"] = "1";
                Settings.Default.Save();
                mediaElement.IsEnabled = true;
                var bc = new BrushConverter();
                PickAnimatedBackground.Foreground = (Brush)bc.ConvertFrom("#FFC6C6C9");
                PickAnimatedBackground.IsEnabled = true;
                mediaElement.Visibility = Visibility.Visible;
                mediaElement.LoadedBehavior = MediaState.Play;
                mediaElement.Source = new Uri(Settings.Default["AnimatedBackgroundSource"].ToString(), UriKind.Absolute);
                mediaElement.Play();
            }
            else
            {             
                Settings.Default["AnimatedBackgroundCheckbox"] = "0";
                Settings.Default.Save();
                mediaElement.Stop();
                mediaElement.IsEnabled = false;
                var bc = new BrushConverter();
                PickAnimatedBackground.Foreground = (Brush)bc.ConvertFrom("#FF72727A");
                PickAnimatedBackground.IsEnabled = false;
                mediaElement.Visibility = Visibility.Hidden;
            }
        }

        private void PickAnimatedBackground_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".mp4";
            dlg.Filter = "Video (*.mp4,*.avi,*.flv,*.mkv,*.mov,*.webm,*.wmv)|*.mp4; *.avi; *.flv; *.mkv; *.mov; *.webm; *.wmv";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                File.Copy(filename, System.IO.Directory.GetCurrentDirectory() + @"\Resources\img\bckg.mp4");
                Settings.Default["AnimatedBackgroundSource"] = filename;
                Settings.Default.Save();               
                mediaElement.IsEnabled = true;
                var bc = new BrushConverter();
                PickAnimatedBackground.Foreground = (Brush)bc.ConvertFrom("#FFC6C6C9");
                PickAnimatedBackground.IsEnabled = true;
                mediaElement.Visibility = Visibility.Visible;
                mediaElement.LoadedBehavior = MediaState.Play;
                mediaElement.Source = new Uri(Settings.Default["AnimatedBackgroundSource"].ToString(), UriKind.Absolute);
                mediaElement.Play();
            }
        }

        private void PickBackgroundImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"; ;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                Settings.Default["BackgroundImageSource"] = filename;
                Settings.Default.Save();
                backgImageBox.IsEnabled = true;
                var bc = new BrushConverter();
                PickBackgroundImage.Foreground = (Brush)bc.ConvertFrom("#FFC6C6C9");
                PickBackgroundImage.IsEnabled = true;
                backgImageBox.Visibility = Visibility.Visible;
                backgImageBox.Source = new BitmapImage(new Uri(Settings.Default["BackgroundImageSource"].ToString()));
            }
        }

        private void ResetBackgroundImageButton_Click(object sender, RoutedEventArgs e)
        {           
            Settings.Default["BackgroundImageSource"] = System.IO.Directory.GetCurrentDirectory() + "/Resources/img/bckg.png";
            Settings.Default.Save();
            Settings.Default["BackgroundImageCheckbox"] = "1";
            BackgroundImageCheckbox.IsChecked = true;
            Settings.Default.Save();
            backgImageBox.IsEnabled = true;
            var bc = new BrushConverter();
            PickBackgroundImage.Foreground = (Brush)bc.ConvertFrom("#FFC6C6C9");
            PickBackgroundImage.IsEnabled = true;
            backgImageBox.Visibility = Visibility.Visible;
            backgImageBox.Source = new BitmapImage(new Uri(Settings.Default["BackgroundImageSource"].ToString()));
        }

        private void BackgroundImageCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if (BackgroundImageCheckbox.IsChecked == true)
            {
                Settings.Default["BackgroundImageCheckbox"] = "1";
                Settings.Default.Save();
                backgImageBox.IsEnabled = true;
                var bc = new BrushConverter();
                PickBackgroundImage.Foreground = (Brush)bc.ConvertFrom("#FFC6C6C9");
                PickBackgroundImage.IsEnabled = true;
                backgImageBox.Visibility = Visibility.Visible;
                backgImageBox.Source = new BitmapImage(new Uri(Settings.Default["BackgroundImageSource"].ToString()));
            }
            else
            {
                Settings.Default["BackgroundImageCheckbox"] = "0";
                Settings.Default.Save();
                backgImageBox.IsEnabled = false;
                var bc = new BrushConverter();
                PickBackgroundImage.Foreground = (Brush)bc.ConvertFrom("#FFC6C6C9");
                PickBackgroundImage.IsEnabled = false;
                backgImageBox.Visibility = Visibility.Hidden;
            }
        }

        private void githubbutton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-");
        }

        private void JoinDiscordButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/8SCcCJq");
        }
        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;          
            File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/UnhandledException_Debug.txt",
                e.StackTrace.ToString()
                + Environment.NewLine
                + "exception : " + e.Message
                );
        }
        private void Build_ScarletData()
        {
            ScarletMode_Message = "";
            int cr = 0;
            int cc = 0;
            DataRow Dr = TimeTable.Rows[cr];
            while (true)
            {
                if (Dr[cc].ToString() == NextBossTimeLabel.ToolTip.ToString().Split(' ')[1])
                { break; }
                cc++;
            }
            while (true)
            {
                Dr = TimeTable.Rows[cr];
                if (Dr[0].ToString() == NextBossTimeLabel.ToolTip.ToString().Split(' ')[0])
                { break; }
                cr++;
            }
            DataRow Dr1 = TimeTable.Rows[0];
            while (true)
            {
                if (Dr[cc].ToString() != "")
                {
                    ScarletMode_Message += Environment.NewLine
                        + string.Format("{0, -30} | {1, 20} | {2, 20}", Dr[cc].ToString().Replace("," + Environment.NewLine, " & ") + " ", "Spawns Time: " + Dr[0].ToString() + " " + Dr1[cc].ToString() + " ", "Spawns in: " + "<00:00:00>");
                    cc++;
                }
                else
                { cc++; }
                if (TimeTable.Columns.Count <= cc)
                { break; }

            }

        }
        private void Update_ScarletMode()
        {                    
            if (ScarletMode_Message == "" || ScarletMode_Message == null)
            { Build_ScarletData(); }
            else
            {
                string[] Inp = ScarletMode_Message.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                if(Inp.Length > 1)
                {
                    if (Inp[1].Contains(CbossNameLabel.Content.ToString()) && Inp[1].Contains(CurrentBossTimeLabel.ToolTip.ToString()))
                    { Build_ScarletData(); }
                }                
            }
            Updated_ScarletMode_Message = "";
            string[] Inputes = ScarletMode_Message.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var Outputs = new List<string> { };
            int rsi = RegionComboBox.SelectedIndex;
            int UtcValue = 0;
            switch (rsi)
            {
                case 0:
                    UtcValue = +9;
                    break;
                case 1:
                    UtcValue = +1;
                    break;
                case 2:
                    UtcValue = +1;
                    break;
                case 3:
                    UtcValue = +9;
                    break;
                case 4:
                    UtcValue = +9;
                    break;
                case 5:
                    UtcValue = +3;
                    break;
                case 6:
                    UtcValue = -8;
                    break;
                case 7:
                    UtcValue = -8;
                    break;
                case 8:
                    UtcValue = +3;
                    break;
                case 9:
                    UtcValue = -3;
                    break;
                case 10:
                    UtcValue = +8;
                    break;
                case 11:
                    UtcValue = +7;
                    break;
                case 12:
                    UtcValue = +8;
                    break;
            }
            DateTime DT = DateTime.UtcNow.AddHours(UtcValue);
            DateTime CDT = DateTime.Now;
            TimeSpan duration = TimeSpan.Zero;
            foreach (string input in Inputes)
            {
                if(input != "")
                {
                    string[] Date_Time = GetStrBetweenTags(input, "Spawns Time: ", " | Spawns in: ").Split(' ');
                    CDT = DateTime.Parse(DT.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + " " + Date_Time[1].Trim() + ":00", CultureInfo.InvariantCulture);
                    duration = CDT.Subtract(DT);
                    Outputs.Add(input.Replace("<00:00:00>", duration.ToString(@"hh\:mm\:ss")));
                }
            }
            string Final_filter = "";
            foreach(string output in Outputs)
            {
                Final_filter += output + Environment.NewLine;
            }
            Updated_ScarletMode_Message = "**Bosses For Today**" + Environment.NewLine + "```css" + Environment.NewLine
                + Final_filter.TrimEnd()
                + Environment.NewLine + "```";
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var helper = new WindowInteropHelper(this).Handle;
            SetWindowLong(helper, GWL_EX_STYLE, (GetWindowLong(helper, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW); // hiding From alt+tab
        }    
        public async void Processing_Status(bool Is_Process_Completed, string Process_Message, bool Process_Failed = false)
        {
            if (Is_Process_Completed)
            {
                if (Process_Failed)
                {
                    Processing_status_Image.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/img/Cancel.png"));
                    Processing_status_Image.Visibility = Visibility.Visible;
                    Processing_Status_TextBlock.Text = Process_Message;
                    DoubleAnimation da = new DoubleAnimation();
                    da.From = 0;
                    da.To = 1;
                    da.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
                    da.AutoReverse = false;
                    Processing_Status_TextBlock.BeginAnimation(OpacityProperty, da);
                    Processing_status_Image.BeginAnimation(OpacityProperty, da);

                    da.From = 1;
                    da.To = 0;
                    da.Duration = new Duration(TimeSpan.FromMilliseconds(300));
                    Process_Loading_icon.BeginAnimation(OpacityProperty, da);

                    await Task.Delay(1500);
                    da.From = 200;
                    da.To = 0;
                    da.Duration = new Duration(TimeSpan.FromMilliseconds(300));
                    da.AutoReverse = false;
                    Processing_Status_Grid.BeginAnimation(WidthProperty, da);
                }
                else
                {
                    Processing_status_Image.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/img/Completed.png"));
                    Processing_status_Image.Visibility = Visibility.Visible;
                    Processing_Status_TextBlock.Text = Process_Message;

                    DoubleAnimation da = new DoubleAnimation();
                    da.From = 0;
                    da.To = 1;
                    da.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
                    da.AutoReverse = false;
                    Processing_Status_TextBlock.BeginAnimation(OpacityProperty, da);
                    Processing_status_Image.BeginAnimation(OpacityProperty, da);

                    da.From = 1;
                    da.To = 0;
                    da.Duration = new Duration(TimeSpan.FromMilliseconds(300));
                    Process_Loading_icon.BeginAnimation(OpacityProperty, da);
                    da.From = 0;
                    da.To = 200;
                    da.Duration = new Duration(TimeSpan.FromMilliseconds(300));
                    da.AutoReverse = false;
                    Processing_Status_Grid.BeginAnimation(WidthProperty, da);

                    await Task.Delay(1500);
                    da.From = 200;
                    da.To = 0;
                    da.Duration = new Duration(TimeSpan.FromMilliseconds(300));
                    da.AutoReverse = false;
                    Processing_Status_Grid.BeginAnimation(WidthProperty, da);
                }
            }
            else
            {
                Processing_status_Image.Visibility = Visibility.Hidden;
                Processing_Status_TextBlock.Text = Process_Message;
                Processing_Status_TextBlock.Opacity = 1;
                Process_Loading_icon.Opacity = 1;
                DoubleAnimation da = new DoubleAnimation();
                da.From = 0;
                da.To = 1;
                da.Duration = new Duration(TimeSpan.FromMilliseconds(300));
                Process_Loading_icon.BeginAnimation(OpacityProperty, da);

                da.From = 0;
                da.To = 200;
                da.Duration = new Duration(TimeSpan.FromMilliseconds(300));
                da.AutoReverse = false;
                Processing_Status_Grid.BeginAnimation(WidthProperty, da);
            }
        }      
        private void AddNewBossButton_Click(object sender, RoutedEventArgs e)
        {
            AddSaveBossPictureBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/Boss.png"));
            AddSaveBossNameTextBox.Text = "";
            CBNTextbox.Text = "";
            DisplayImageLinkextBox.Text = "";           
            DisplayImageLinkextBoxLocal.Text = "";
            SRFTBCombobox.SelectedIndex = -1;
            BossSpawnLocationLinkTextBox.Text = "";
            AddSaveBossNameTextBox.IsEnabled = true;
            string new_fileName = "New Boss_0";
            foreach (string item in BossCollectionListBox.Items)
            {
                if(item.Contains(new_fileName.Split('_')[0]))
                { 
                    if(int.Parse(item.Split('_')[1]) > int.Parse(new_fileName.Split('_')[1]))
                    { new_fileName = item;}
                }
            }
            BossCollectionListBox.Items.Add(new_fileName.Split('_')[0] + "_" + (int.Parse(new_fileName.Split('_')[1]) + 1).ToString());
            AddSaveBossNameTextBox.Text = new_fileName.Split('_')[0] + "_" + (int.Parse(new_fileName.Split('_')[1]) + 1).ToString();
            DisplayImageLinkextBox.Text = "https://raw.githubusercontent.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/master/Resources/Images/Boss.png";
            DisplayImageLinkextBoxLocal.Text = "<Local>/Resources/Bosses_img/Boss.png";
            SRFTBCombobox.SelectedIndex = 0;
            BossCollectionListBox.SelectedIndex = BossCollectionListBox.Items.Count - 1;
            BossesCollection.Add(AddSaveBossNameTextBox.Text + ","
                + DisplayImageLinkextBox.Text + ","
                + BossSpawnLocationLinkTextBox.Text + ","
                + CBNTextbox.Text + ","
                + SRFTBCombobox.Text + "{" + RolesCollection[SRFTBCombobox.SelectedIndex].ToString() + ","
                + DisplayImageLinkextBoxLocal.Text);
            string bosses = "";
            foreach(string boss in BossesCollection)
            { bosses += boss + Environment.NewLine;}
            File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Bosses", bosses.TrimEnd());
            Selectected_boss = BossCollectionListBox.SelectedIndex;
        }

        private void BossCollectionListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        { Trigger_bossSelection(); }
        private void Trigger_bossSelection()
        {
            if (BossCollectionListBox.SelectedIndex > -1)
            {
                try
                {
                    Selectected_boss = BossCollectionListBox.SelectedIndex;
                    Getimg gm = new Getimg();
                    var bc = new BrushConverter();
                    DisplayImageLinkextBox.BorderBrush = (Brush)bc.ConvertFrom("#FF434349");
                    RemoveBossButton.Visibility = Visibility.Visible;
                    AddSaveBossNameTextBox.IsEnabled = true;
                    foreach (string origin_item in Origin_BossesCollection)
                    {
                        if (BossCollectionListBox.SelectedItem.ToString() == origin_item.Split(',')[0])
                        { AddSaveBossNameTextBox.IsEnabled = false; break; }
                    }
                    foreach (string item in BossesCollection)
                    {
                        string bossName = item.Substring(0, item.IndexOf(",") + 1);
                        bossName = bossName.Replace(",", "");
                        bossName = bossName.Replace(Environment.NewLine, "");
                        if (bossName.ToString() == BossCollectionListBox.SelectedItem.ToString())
                        {
                            string[] bossdata = item.Split(',');
                            AddSaveBossNameTextBox.Text = bossdata[0].ToString();
                            DisplayImageLinkextBox.Text = bossdata[1].ToString();
                            DisplayImageLinkextBoxLocal.Text = bossdata[5].ToString();
                            AddSaveBossPictureBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses_img/Boss.png"));
                            if (bossdata[5].ToString() == "")
                            {
                                string imgurl = bossdata[1].ToString();
                                //if (imgurl.Contains("<Local>"))
                                //{ imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
                                try { AddSaveBossPictureBox.Source = gm.GETIMAGE(imgurl); } catch (Exception) { DisplayImageLinkextBox.BorderBrush = Brushes.Red; }
                            }
                            else
                            {
                                string imgurl = bossdata[5].ToString();
                                if (imgurl.Contains("<Local>"))
                                { imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
                                try { AddSaveBossPictureBox.Source = gm.GETIMAGE(imgurl); } catch (Exception) { DisplayImageLinkextBoxLocal.BorderBrush = Brushes.Red; }
                            }
                            BossSpawnLocationLinkTextBox.Text = bossdata[2].ToString();
                            CBNTextbox.Text = bossdata[3].ToString();
                            string[] br = bossdata[4].ToString().Split('{');
                            SRFTBCombobox.Text = br[0].ToString();
                            if (SRFTBCombobox.SelectedIndex == -1)
                            {
                                if (br[1].ToString() != "")
                                {
                                    try
                                    {
                                        var guild = discord.GetGuild(ServerID);
                                        ulong Role_id = ulong.Parse(br[1].ToString());
                                        var Role = guild.GetRole(Role_id);
                                        SRFTBCombobox.Text = "@" + Role.Name.ToString();
                                    }
                                    catch (Exception) { SRFTBCombobox.Text = "None"; }
                                }
                                else
                                { SRFTBCombobox.Text = "None"; }
                            }
                        }
                    }                   
                }
                catch (Exception) { }
            }
        }       
        private void AddSaveBossNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(AddSaveBossNameTextBox.Text.Length > 0)
            { BossCollectionListBox.IsEnabled = true; }
            else
            { BossCollectionListBox.IsEnabled = false; }
            BossCollectionListBox.Items[Selectected_boss] = AddSaveBossNameTextBox.Text;
            BossCollectionListBox.SelectedIndex = Selectected_boss;
        }
        private void OptimizeAppCheckbox_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default["High_RenderQuality"] = OptimizeAppCheckbox.IsChecked.ToString();
            Settings.Default.Save();
            BitmapScaling(bool.Parse(OptimizeAppCheckbox.IsChecked.ToString()));
        }

        private void Discord_Settings_Click(object sender, RoutedEventArgs e)
        {
            if (tabcontrol1.SelectedIndex == 2)
            { savebosslist(); }
            if (SourceComboBox.SelectedIndex == 0 && tabcontrol1.SelectedIndex == 1)
            { Save_LYPBBTTT(); GetLYPBBTTimeTable(); }

        }

        private void ScarletModeTimeTableSetting_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Updated_ScarletMode_Message = "";
            Settings.Default["ScarletMode"] = ScarletModeTimeTableSetting_CheckBox.IsChecked.ToString();
            Settings.Default.Save();
        }      
        private void BitmapScaling(bool Is_Low)
        {
            if (Is_Low)
            {
                RenderOptions.SetBitmapScalingMode(APP_Logo, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(Processing_status_Image, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(Process_Loading_icon, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(StartPostingButton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(appRestartButton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(ConnectDiscordBotButton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(DisconnectDiscordBot_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(PBImageBox, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(NBImageBox, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(LBImageBox, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(alertbosstab_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(alertnighttabbutton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(alertimperialtabbutton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(alertimperialresettabbutton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(exssoundbutton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(exstimetablebutton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(exsalertbutton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(mediaElement, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(backgImageBox, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(HomeButton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(TimeTableButton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(BossesListButton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(SettingsButton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(AboutButton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(AddSaveBossPictureBox, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(IoverlayModeButton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(SendTotrayButton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(CloseappButton_img, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(githubbutton, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(JoinDiscordButton, BitmapScalingMode.LowQuality);
            }
            else
            {
                RenderOptions.SetBitmapScalingMode(APP_Logo, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(Processing_status_Image, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(Process_Loading_icon, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(StartPostingButton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(appRestartButton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(ConnectDiscordBotButton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(DisconnectDiscordBot_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(PBImageBox, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(NBImageBox, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(LBImageBox, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(alertbosstab_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(alertnighttabbutton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(alertimperialtabbutton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(alertimperialresettabbutton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(exssoundbutton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(exstimetablebutton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(exsalertbutton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(mediaElement, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(backgImageBox, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(HomeButton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(TimeTableButton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(BossesListButton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(SettingsButton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(AboutButton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(AddSaveBossPictureBox, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(IoverlayModeButton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(SendTotrayButton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(CloseappButton_img, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(githubbutton, BitmapScalingMode.HighQuality);
                RenderOptions.SetBitmapScalingMode(JoinDiscordButton, BitmapScalingMode.HighQuality);
            }
        }
        private void SelfRollingButton_Click(object sender, RoutedEventArgs e)
        {
            if (discord.ConnectionState == Discord.ConnectionState.Connected)
            {              
                GetIds();
                new Thread(() =>
                {
                    Thread.Sleep(1000);
                    Application.Current.Dispatcher.Invoke((Action)(() => {                       
                        tabcontrol1.SelectedIndex = 5;
                        SelfRollingMessageRolesCollections.Clear();
                        SelfRolling_Messages_ListBox.Items.Clear();
                        if (RollingChannelComboBox.SelectedIndex != -1 && RollingChannelComboBox.Text != "None")
                        {
                            EnableSelfRollingCheckbox.IsChecked = bool.Parse(Settings.Default["SelfRolling"].ToString());
                            EnableSelfRollingCheckbox.IsEnabled = true;
                        }
                        else
                        { EnableSelfRollingCheckbox.IsChecked = false; EnableSelfRollingCheckbox.IsEnabled = false; }

                        if (EnableSelfRollingCheckbox.IsChecked == true)
                        { UpdateSelfRollingButton.Visibility = Visibility.Visible; RollingGrid.Visibility = Visibility.Visible; }
                        else
                        { UpdateSelfRollingButton.Visibility = Visibility.Hidden; RollingGrid.Visibility = Visibility.Hidden; }
                        SelfRollingStartMesage.Text = GetStrBetweenTags(SelfRollingSettings, "[StartMessage]", "[/StartMessage]");
                        foreach (var RM in GetStrBetweenTags(SelfRollingSettings, "[MessageRoles]", "[/MessageRoles]").Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
                        {
                            if (RM != "")
                            { SelfRollingMessageRolesCollections.Add(RM); SelfRolling_Messages_ListBox.Items.Add(GetStrBetweenTags(RM, "[Message]", "[/Message]")); }
                        }
                        SelfRollingEndMessage.Text = GetStrBetweenTags(SelfRollingSettings, "[EndMessage]", "[/EndMessage]");
                        SelfRolling_LoadedReaction = "";
                        SelfRollingInsertRole.Visibility = Visibility.Hidden;
                        SelfRollingInsertEmoji.Visibility = Visibility.Hidden;
                        var bc = new BrushConverter();
                        Homehighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                        Timetablehighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                        bosslisthighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                        SelfRollingHighlight.Fill = (Brush)bc.ConvertFrom(subcolor);
                        settingshighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                        abouthighlight.Fill = (Brush)bc.ConvertFrom("#008B81FC");
                    }));
                }).Start();                       
            }
        }
        private void RollingChannelComboBox_DropDownClosed(object sender, EventArgs e)
        { if (RollingChannelComboBox.SelectedIndex > -1) { Save_SelfRolling_Settings(); }}
        private void Save_SelfRolling_Settings()
        {
            SelfRollingSettings = "[Channel]" + RollingChannelComboBox.Text + "{" + AlertChannelsCollection[RollingChannelComboBox.SelectedIndex] + "[/Channel]"
                + Environment.NewLine
                + "[StartMessage]" + SelfRollingStartMesage.Text + "[/StartMessage]" 
                + Environment.NewLine;
            string roles = "";
            ulong id = 0;
            if(SelfRolling_MainMessage != null)
            { id = SelfRolling_MainMessage.Id; }
            foreach (var role in SelfRollingMessageRolesCollections) { roles += role + Environment.NewLine;}
            SelfRollingSettings += "[MessageRoles]" + roles + "[/MessageRoles]"
                + Environment.NewLine + "[EndMessage]" + SelfRollingEndMessage.Text + "[/EndMessage]"
                + Environment.NewLine + "[MainMessageID]" + id + "[/MainMessageID]";

            File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/SelfRolling", SelfRollingSettings);

            if (RollingChannelComboBox.SelectedIndex != -1 && RollingChannelComboBox.Text != "None")
            {
                EnableSelfRollingCheckbox.IsChecked = bool.Parse(Settings.Default["SelfRolling"].ToString());
                EnableSelfRollingCheckbox.IsEnabled = true;
            }
            else
            { EnableSelfRollingCheckbox.IsChecked = false; EnableSelfRollingCheckbox.IsEnabled = false; }

            if (EnableSelfRollingCheckbox.IsChecked == true)
            { UpdateSelfRollingButton.Visibility = Visibility.Visible; RollingGrid.Visibility = Visibility.Visible; }
            else
            { UpdateSelfRollingButton.Visibility = Visibility.Hidden; RollingGrid.Visibility = Visibility.Hidden; }
        }
        public async void SelfRollingStartUp()
        {
            if (EnableSelfRollingCheckbox.IsChecked == true && discord.ConnectionState == Discord.ConnectionState.Connected)
            {               
                try
                {
                    ulong SelfRollingChannel_ID = ulong.Parse(GetStrBetweenTags(SelfRollingSettings, "[Channel]", "[/Channel]").Split('{')[1]);
                    Discord_Bot bot = new Discord_Bot();
                    IReadOnlyCollection<SocketGuild> Servers = null;
                    List<string> Reactions = new List<string>();
                    await Task.Run(() => { Servers = bot.Get_Guilds(this); });
                    SocketGuild guild = null;
                    foreach (var server in Servers) { if (server.Id == ServerID) { guild = server; break; } }
                    SocketTextChannel channel = null;
                    foreach (var chn in guild.TextChannels) { if (chn.Id == SelfRollingChannel_ID) { channel = chn; break; } }
                    if (channel != null)
                    {
                        string SelfRollingMessage_Build = GetStrBetweenTags(SelfRollingSettings, "[StartMessage]", "[/StartMessage]") + Environment.NewLine;
                        int roleamounts = 0;
                        var RoleMessagesRoles = GetStrBetweenTags(SelfRollingSettings, "[MessageRoles]", "[/MessageRoles]").Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        foreach (var RoleMessage in RoleMessagesRoles)
                        {
                            roleamounts++;
                            if (roleamounts < RoleMessagesRoles.Length)
                            { SelfRollingMessage_Build += GetStrBetweenTags(RoleMessage, "[Message]", "[/Message]") + Environment.NewLine; }
                            else
                            { SelfRollingMessage_Build += GetStrBetweenTags(RoleMessage, "[Message]", "[/Message]"); }
                            Reactions.Add(GetStrBetweenTags(RoleMessage, "[Emote]", "[/Emote]"));
                        }
                        SelfRollingMessage_Build += GetStrBetweenTags(SelfRollingSettings, "[EndMessage]", "[/EndMessage]");
                        try
                        {
                            SelfRolling_MainMessage = (Discord.Rest.RestUserMessage)await channel.GetMessageAsync(ulong.Parse(GetStrBetweenTags(SelfRollingSettings, "[MainMessageID]", "[/MainMessageID]")));
                            await SelfRolling_MainMessage.ModifyAsync(msg => msg.Content = SelfRollingMessage_Build);
                            await Task.Run(() => { bot.AddReactionsAsync(Reactions, SelfRolling_MainMessage); });
                            SelfRollingSettings = SelfRollingSettings.Replace("[MainMessageID]"
                                + GetStrBetweenTags(SelfRollingSettings, "[MainMessageID]", "[/MainMessageID]") + "[/MainMessageID]"
                                , "[MainMessageID]" + SelfRolling_MainMessage.Id + "[/MainMessageID]");
                            File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/SelfRolling", SelfRollingSettings);
                        }
                        catch (Exception)
                        {
                            SelfRolling_MainMessage = await channel.SendMessageAsync(SelfRollingMessage_Build);
                            await Task.Run(() => { bot.AddReactionsAsync(Reactions, SelfRolling_MainMessage); });
                            SelfRollingSettings = SelfRollingSettings.Replace("[MainMessageID]"
                               + GetStrBetweenTags(SelfRollingSettings, "[MainMessageID]", "[/MainMessageID]") + "[/MainMessageID]"
                               , "[MainMessageID]" + SelfRolling_MainMessage.Id + "[/MainMessageID]");
                            File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/SelfRolling", SelfRollingSettings);
                        }
                        discord.ReactionAdded -= ReactionAdded_Event;
                        discord.ReactionRemoved -= ReactionRemoved_Event;
                        discord.ReactionAdded += ReactionAdded_Event;
                        discord.ReactionRemoved += ReactionRemoved_Event;
                    }
                }
                catch (Exception) { }
            }
        }
        private void EnableSelfRollingCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if (EnableSelfRollingCheckbox.IsChecked == true)
            { 
                UpdateSelfRollingButton.Visibility = Visibility.Visible;
                RollingGrid.Visibility = Visibility.Visible;
                if (discord.ConnectionState == Discord.ConnectionState.Connected)
                {
                    discord.ReactionAdded += ReactionAdded_Event;
                    discord.ReactionRemoved += ReactionRemoved_Event;
                    SelfRollingStartUp();
                }
            }
            else
            { 
                UpdateSelfRollingButton.Visibility = Visibility.Hidden;
                RollingGrid.Visibility = Visibility.Hidden;
                if (discord.ConnectionState == Discord.ConnectionState.Connected)
                {
                    discord.ReactionAdded -= ReactionAdded_Event;
                    discord.ReactionRemoved -= ReactionRemoved_Event;
                }
            }
            Settings.Default["SelfRolling"] = EnableSelfRollingCheckbox.IsChecked.ToString();
            Settings.Default.Save();           
        }
        public async Task ReactionAdded_Event(Cacheable<IUserMessage, UInt64> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (SelfRolling_WaitingReaction && SelfRolling_GeTReactionMessage.Id == message.Id)
            {
                SelfRolling_LoadedReaction = reaction.Emote.ToString();
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    if (SelfRolling_LoadedReaction.Contains("<") && SelfRolling_LoadedReaction.Contains(":") && SelfRolling_LoadedReaction.Contains(">"))
                    {
                       var gm = new Getimg();
                       SelfRollingReactionPicture.Source = gm.GETIMAGE("https://cdn.discordapp.com/emojis/" + SelfRolling_LoadedReaction.Split(':')[2].Replace(">", "") + ".png");
                    }
                    else
                    { SelfRollingReactionPicture.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/img/SelfRoleLogo.png")); }
                }));
                await channel.DeleteMessageAsync(SelfRolling_GeTReactionMessage.Id);
                Application.Current.Dispatcher.Invoke((Action)(() => { 
                    SelfRollingInsertEmoji.Visibility = Visibility.Visible;
                    try
                    {
                        SelfRolling_RoleMessage.Text =
                        SelfRolling_RoleMessage.Text.Replace(GetStrBetweenTags(SelfRollingMessageRolesCollections[SelfRolling_Messages_ListBox_SelectedIndex], "[Emote]", "[/Emote]"), SelfRolling_LoadedReaction);
                    }catch (Exception) { }
                    Save_CurrentSelfRolling_RoleSettings(); 
                }));                
                //var emote = Emote.Parse("SelfRolling_LoadedReaction");
            }
            if (message.Id == SelfRolling_MainMessage.Id && !reaction.User.Value.IsBot)
            {
                Discord_Bot bot = new Discord_Bot();
                await Task.Run(() => { bot.AddRoleToUser(SelfRollingSettings, reaction, this); });
            }
        }
        public async Task ReactionRemoved_Event(Cacheable<IUserMessage, UInt64> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (message.Id == SelfRolling_MainMessage.Id && !reaction.User.Value.IsBot)
            {
                Discord_Bot bot = new Discord_Bot();
                await Task.Run(() => { bot.RemoveRoleToUser(SelfRollingSettings, reaction, this); });
            }
        }
        private void SelRollingRoleComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if(SelRollingRoleComboBox.SelectedIndex != -1)
            {
                SelfRollingInsertRole.Visibility = Visibility.Visible;
                try
                {
                    var role = GetStrBetweenTags(SelfRollingMessageRolesCollections[SelfRolling_Messages_ListBox_SelectedIndex], "[Role]", "[/Role]");
                    SelfRolling_RoleMessage.Text = SelfRolling_RoleMessage.Text.Replace("<@&" + role + ">", "<@&" + SelfRollingRolesCollections[SelRollingRoleComboBox.SelectedIndex] + ">");
                }
                catch (Exception) { }
                Save_CurrentSelfRolling_RoleSettings();
            }
        }

        private async void SelfRolling_GetReactionButton_Click(object sender, RoutedEventArgs e)
        {
            if (discord.ConnectionState == Discord.ConnectionState.Connected)
            {
                SelfRolling_LoadedReaction = "";
                ulong SelfRollingChannel_ID = ulong.Parse(GetStrBetweenTags(SelfRollingSettings, "[Channel]", "[/Channel]").Split('{')[1]);
                IReadOnlyCollection<SocketGuild> Servers = null;
                Discord_Bot bot = new Discord_Bot();
                await Task.Run(() => { Servers = bot.Get_Guilds(this); });
                SocketGuild guild = null;
                foreach (var server in Servers) { if (server.Id == ServerID) { guild = server; break; } }
                SocketTextChannel channel = null;
                foreach (var chn in guild.TextChannels) { if (chn.Id == SelfRollingChannel_ID) { channel = chn; break; } }
                if (channel != null)
                {
                    var embed1 = new EmbedBuilder();
                    embed1 = new EmbedBuilder
                    {
                        Title = "LYPBBT Self Rolling",
                        Color = Discord.Color.LightGrey,
                        //Url = publicbossUrl,
                        Description = "React to this message to capture the Role"
                    };
                    SelfRolling_GeTReactionMessage = await channel.SendMessageAsync("", false, embed1.Build());
                }
                SelfRolling_WaitingReaction = true;
                discord.ReactionAdded -= ReactionAdded_Event;
                discord.ReactionAdded += ReactionAdded_Event;
            }
        }

        private void SelfRollingInsertRole_Click(object sender, RoutedEventArgs e)
        {SelfRolling_RoleMessage.Text += " <@&" + SelfRollingRolesCollections[SelRollingRoleComboBox.SelectedIndex] + "> "; Save_CurrentSelfRolling_RoleSettings(); }

        private void SelfRollingInsertEmoji_Click(object sender, RoutedEventArgs e)
        { SelfRolling_RoleMessage.Text += " " + SelfRolling_LoadedReaction + " "; Save_CurrentSelfRolling_RoleSettings(); }

        private void SelfRollingAddNewSelfRole_button_Click(object sender, RoutedEventArgs e)
        {
            SelfRolling_RoleMessage.Text = "New Role";
            SelRollingRoleComboBox.SelectedIndex = -1;
            SelfRolling_LoadedReaction = "";
            SelfRollingReactionPicture.Source = null;

            SelfRolling_Messages_ListBox.Items.Add(SelfRolling_RoleMessage.Text);
            SelfRollingMessageRolesCollections.Add("[Message]" + SelfRolling_RoleMessage.Text + "[/Message]" + "[Role][/Role]" + "[Emote][/Emote]");
            SelfRolling_Messages_ListBox.SelectedIndex = SelfRolling_Messages_ListBox.Items.Count - 1;
            SelfRolling_Messages_ListBox_SelectedIndex = SelfRolling_Messages_ListBox.Items.Count - 1;
            Save_SelfRolling_Settings();
        }

        private void SelfRollingStartMesage_LostFocus(object sender, RoutedEventArgs e)
        { Save_SelfRolling_Settings(); }    
        private void Save_CurrentSelfRolling_RoleSettings()
        {
            try
            {
                string Role = "";
                try { Role = SelfRollingRolesCollections[SelRollingRoleComboBox.SelectedIndex]; }
                catch (Exception) { Role = ""; }
                SelfRolling_Messages_ListBox.Items[SelfRolling_Messages_ListBox_SelectedIndex] = SelfRolling_RoleMessage.Text;
                SelfRollingMessageRolesCollections[SelfRolling_Messages_ListBox_SelectedIndex] = "[Message]" + SelfRolling_RoleMessage.Text + "[/Message]" + "[Role]" + Role + "[/Role]" + "[Emote]" + SelfRolling_LoadedReaction + "[/Emote]";
                Save_SelfRolling_Settings();
            }
            catch (Exception) { }
        }
        private void UpdateSelfRollingButton_Click(object sender, RoutedEventArgs e)
        { SelfRollingStartUp(); }

        private void SelfRolling_Messages_ListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(SelfRolling_Messages_ListBox.SelectedIndex > -1)
            {
                SelfRollingInsertRole.Visibility = Visibility.Hidden;
                SelfRollingInsertEmoji.Visibility = Visibility.Hidden;
                SelfRolling_Messages_ListBox_SelectedIndex = SelfRolling_Messages_ListBox.SelectedIndex;
                SelfRolling_RoleMessage.Text = GetStrBetweenTags(SelfRollingMessageRolesCollections[SelfRolling_Messages_ListBox.SelectedIndex], "[Message]", "[/Message]");
                string Role = GetStrBetweenTags(SelfRollingMessageRolesCollections[SelfRolling_Messages_ListBox.SelectedIndex], "[Role]", "[/Role]");
                int role_Name = -1;
                SelRollingRoleComboBox.SelectedIndex = -1;
                foreach(var roleID in SelfRollingRolesCollections)
                {
                    role_Name++;
                    if (roleID == Role)
                    { SelRollingRoleComboBox.SelectedIndex = role_Name;break; }                   
                }
                SelfRolling_LoadedReaction = GetStrBetweenTags(SelfRollingMessageRolesCollections[SelfRolling_Messages_ListBox.SelectedIndex], "[Emote]", "[/Emote]");
                if (SelfRolling_LoadedReaction.Contains("<") && SelfRolling_LoadedReaction.Contains(":") && SelfRolling_LoadedReaction.Contains(">"))
                {
                    var gm = new Getimg();
                    SelfRollingReactionPicture.Source = gm.GETIMAGE("https://cdn.discordapp.com/emojis/" + SelfRolling_LoadedReaction.Split(':')[2].Replace(">", "") + ".png");
                }
                else
                { SelfRollingReactionPicture.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/img/SelfRoleLogo.png")); }
                if (SelRollingRoleComboBox.SelectedIndex != -1)
                {SelfRollingInsertRole.Visibility = Visibility.Visible;}
                if(SelfRolling_LoadedReaction != "")
                { SelfRollingInsertEmoji.Visibility = Visibility.Visible; }
            }
        }

        private void SelfRollingRemoveSelfRole_button_Click(object sender, RoutedEventArgs e)
        {
            if(SelfRolling_Messages_ListBox.SelectedIndex > -1)
            {
                SelfRollingMessageRolesCollections.RemoveAt(SelfRolling_Messages_ListBox_SelectedIndex);
                SelfRolling_Messages_ListBox.Items.RemoveAt(SelfRolling_Messages_ListBox_SelectedIndex);
                Save_SelfRolling_Settings();
            }
        }

        private void AUTCheckbox_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default["AutoUpdateTable"] = AUTCheckbox.IsChecked.ToString();
            Settings.Default.Save();
        }

        private void SelfRolling_RoleMessage_KeyUp(object sender, KeyEventArgs e)
        { if (SelfRolling_Messages_ListBox_SelectedIndex > -1) { Save_CurrentSelfRolling_RoleSettings(); } }

    }
}
