using CefSharp;
using CefSharp.Wpf;
using Discord;
using Discord.WebSocket;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
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

namespace YPBBT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// /// lang last 87
    ///    
    public partial class MainWindow : Window 
    {
        private System.Windows.Forms.NotifyIcon MyNotifyIcon;
        DiscordSocketClient discord = new DiscordSocketClient();
        List<string> ProfileCollection = new List<string>();
        List<string> LanguageCollection = new List<string>();
        List<string> BossesCollection = new List<string>();
        public DataTable TimeTable = new DataTable();
        int MainB;
        int PmaxC;
        DateTime NBT;
        DateTime CBT;
        DateTime PBT;
        string DefaultLanguage;
        ulong ClientID;
        string Token;
        ulong ServerID;
        ulong ChannelID;
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
        string MOTR;
        int startupRS;
        int overlayState = 0;
        OverlayModWindow omw = new OverlayModWindow();
        ulong MainMessageID;
        int SaveLatestTimeTable = 0;
        ulong TimtableID = 0;
        int intervalMessageUpdate = 0;
        string publicNbossimage;
        string publicbossUrl;
        ulong bossImageID;
        int AnouncmentMessageInterval;
        int AnouncmentIntervalToDeleteMessages = 0;
        ulong DiscordNotifyBossSpwnID = 0;
        ulong DiscordNotifyNightTimeID = 0;
        ulong DiscordNotifyImperialResetID = 0;
        ulong DiscordNotifyBarteringID = 0;
        ulong DiscordNotifyImperialTradingID = 0;
        string AppVersion = "2.1a";
        string CurrentVersion = "";
        string currentbossrole1 = "";
        string currentbossrole2 = "";
        double lastSliderValue;
        int lastSelectedSource;
        Discord.Rest.RestUserMessage MainMessage;
        int gridview1Row = 0;
        int gridview1Column = 0;

        public MainWindow(int OL)
        {
            if (OL == 0)
            {
                if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/debug.log"))
                { File.Delete(System.IO.Directory.GetCurrentDirectory() + "/debug.log"); }
            }
            InitializeComponent();
            AddSaveBossNameTextBox.IsEnabled = false;
            string fversion = AppVersion;
            try { fversion = fversion.Substring(0, fversion.IndexOf(".") + 2); } catch (Exception) { }
            mainWindow.Title = "YPBBT " + fversion;
            GitHub.Content = "   YPBBT " + fversion;
            string urlAddress = "https://raw.githubusercontent.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/master/YPBBT%202.0/CurrentVersion";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (String.IsNullOrWhiteSpace(response.CharacterSet))
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                    CurrentVersion = readStream.ReadToEnd();
                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception) { CurrentVersion = AppVersion; }
            string finalBosslist = "";
            if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses"))
            {
                string OriginBossesList = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/BossesOrigin");
                string[] BossListSplited = OriginBossesList.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                string BossesList = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses");
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
                                    if(bossdataS.Length == 5)
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
                    if (!BossesList.Contains(BDS[0]+","))
                    { finalBosslist += Environment.NewLine + BD; }
                }
            }
            else
            {
                string OriginBossesList = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/BossesOrigin");
                string[] BossListSplited = OriginBossesList.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                int line = 0;
                foreach(string bossdata in BossListSplited)
                {
                    if(line == 0)
                    { finalBosslist += bossdata; }
                    else
                    { finalBosslist += Environment.NewLine + bossdata; }
                    line++;
                }
            }
            File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses", finalBosslist);
            Tab0.Visibility = Visibility.Hidden;
            tab1.Visibility = Visibility.Hidden;
            tab2.Visibility = Visibility.Hidden;
            tab3.Visibility = Visibility.Hidden;
            tab4.Visibility = Visibility.Hidden;
            tab5.Visibility = Visibility.Hidden;
            AlarmsTab0.Visibility = Visibility.Hidden;
            AlarmsTab1.Visibility = Visibility.Hidden;
            TimeTableGrid.Visibility = Visibility.Hidden;
            AlarmsPreviewsTab.Visibility = Visibility.Hidden;

            Settings.Default["OverlayState"] = "0";
            Settings.Default.Save();
            MyNotifyIcon = new System.Windows.Forms.NotifyIcon();
            MyNotifyIcon.Icon = new System.Drawing.Icon(
                            System.IO.Directory.GetCurrentDirectory() + "/Resources/icon.ico");
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
            SourceComboBox.Items.Add("BdoBossTimer");
            SourceComboBox.SelectedIndex = lastSS;

            BotHostComboBox.Items.Add("Local");
            BotHostComboBox.Items.Add("polisystems");
            try { ProfileCollection.Clear(); } catch (Exception) { }
            string lines = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/Profile");//read Profile File
            string[] RefferalsFilter = lines.Split('|');
            foreach (var rf in RefferalsFilter)
            { ProfileCollection.Add(rf); }
            DefaultLanguage = ProfileCollection[0].ToString();
            ClientID = ulong.Parse(ProfileCollection[1].ToString());
            Token = ProfileCollection[2].ToString();
            ServerID = ulong.Parse(ProfileCollection[3].ToString());
            ChannelID = ulong.Parse(ProfileCollection[4].ToString());
            BossSpawnRole = ProfileCollection[5].ToString();
            NightTimeRole = ProfileCollection[6].ToString();
            ImperialResetRole = ProfileCollection[7].ToString();
            BarteringResetRole = ProfileCollection[10].ToString();
            ImperialTradingResetRole = ProfileCollection[11].ToString();
            UpdateMesssageInterval = int.Parse(ProfileCollection[8].ToString());
            AnouncmentMessageInterval = int.Parse(ProfileCollection[9].ToString());          

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
            { EditSpawnHoursLabael1.Content = "Edit Spawn Hours " + EditSpawnHoursSlider.Value; }
            else
            { EditSpawnHoursLabael1.Content = "Edit Spawn Hours +" + EditSpawnHoursSlider.Value; }
            if (EditSpawnHoursSlider.Value == 0)
            { EditSpawnHoursLabael1.Content = "Edit Spawn Hours " + EditSpawnHoursSlider.Value; }
            startupRS = 0;
           
          
        }
        private void mainWindow_Activated(object sender, EventArgs e)
        {

            if (startupRS == 0 && MOTR == null)
            {
                startupRS = 1;
                string region = Settings.Default["DefaultRegion"].ToString();//get Last Saved Region  
                Console.WriteLine("Activated");
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
            }
        }
        private void LoadDefaultLanguage()
        {
            LanguageCollection.Clear();
            string lines = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/Languages/" + DefaultLanguage + "-m");
            string[] Collection = lines.Split('|');
            foreach (string rf in Collection)
            { LanguageCollection.Add(rf); }
            TimeTableButton.Content = LanguageCollection[0].ToString();
            BossesListButton.Content = LanguageCollection[1].ToString();
            SettingsButton.Content = LanguageCollection[2].ToString();
            SourceLabel.Content = LanguageCollection[3].ToString();
            SendTotrayButton.Content = LanguageCollection[4].ToString();
            appRestartButton.Content = LanguageCollection[5].ToString();
            StartPostingButton.Content = LanguageCollection[6].ToString();
            ConnectDiscordBotButton.Content = LanguageCollection[7].ToString();
            DisconnectDiscordBot.Content = LanguageCollection[8].ToString();
            BotStatusLabel.Content = LanguageCollection[9].ToString();
            RegionLabel.Content = LanguageCollection[10].ToString();
            PbossLabel.Content = LanguageCollection[11].ToString();
            CbossLabel.Content = LanguageCollection[12].ToString();
            LbossLabel.Content = LanguageCollection[13].ToString();
            DiscordBotConnectionStatusLabel.Content = LanguageCollection[14].ToString();
            soundLabel.Content = LanguageCollection[15].ToString();
            BsAlarmLabel.Content = LanguageCollection[16].ToString();
            BossSpawnAlarmCheckBox1.Content = LanguageCollection[17].ToString();
            BossSpawnAlarmCheckBox2.Content = LanguageCollection[18].ToString();
            PlaySoundOnLabel.Content = LanguageCollection[19].ToString();
            BossSpawnAlarmCheckBox3.Content = LanguageCollection[20].ToString();
            SoundOptionCheckBox.Content = LanguageCollection[21].ToString();
            NTAlarmLabel.Content = LanguageCollection[22].ToString();
            NTSoundOptionCheckBox.Content = LanguageCollection[23].ToString();
            IRSoundOptionCheckBox.Content = LanguageCollection[24].ToString();

            NightTimeAlarmCheckBox1.Content = LanguageCollection[25].ToString();
            PostSettingsLabel.Content = LanguageCollection[26].ToString();
            NightTimeAlarmCheckBox2.Content = LanguageCollection[27].ToString();
            DisplayTimeTableSetting.Content = LanguageCollection[28].ToString();
            NightTimeAlarmCheckBox3.Content = LanguageCollection[29].ToString();
            IRAlarmLabel.Content = LanguageCollection[30].ToString();
            NILabel.Content = LanguageCollection[31].ToString();
            IRILabel.Content = LanguageCollection[32].ToString();
            ImperialResetCheckBox1.Content = LanguageCollection[33].ToString();
            ImperialResetCheckBox2.Content = LanguageCollection[34].ToString();
            ImperialResetCheckBox3.Content = LanguageCollection[35].ToString();
            IoverlayModeButton.Content = LanguageCollection[36].ToString();
            BossListLabel.Content = LanguageCollection[46].ToString();
            BossListBossNameLabel.Content = LanguageCollection[47].ToString();
            DILBLLabel.Content = LanguageCollection[48].ToString();
            AddnewBossTestImgLinkButton.Content = LanguageCollection[49].ToString();
            BSLLLabel.Content = LanguageCollection[50].ToString();
            AddSaveBossButton.Content = LanguageCollection[51].ToString();
            RemoveBossButton.Content = LanguageCollection[52].ToString();
            SettingsLabel.Content = LanguageCollection[55].ToString();
            LanguageSLabel.Content = LanguageCollection[56].ToString();
            ClientIDLABEL.Content = LanguageCollection[57].ToString();
            TokenLabel.Content = LanguageCollection[58].ToString();
            ServerIDLAbel.Content = LanguageCollection[59].ToString();
            ChannelIDLabel.Content = LanguageCollection[60].ToString();
            HrdResetAppButton.Content = LanguageCollection[61].ToString();
            HRWLabel.Content = LanguageCollection[62].ToString();
            OTLabel.Content = LanguageCollection[63].ToString();
            BsPRLabel.Content = LanguageCollection[64].ToString();
            NTPRLabel.Content = LanguageCollection[65].ToString();
            IRPRLabel.Content = LanguageCollection[66].ToString();
            BUDLAbel.Content = LanguageCollection[67].ToString();
            RecLabel.Content = LanguageCollection[68].ToString();
            RMMLabel.Content = LanguageCollection[69].ToString();
            BossListBossCustomNameLabel.Content = LanguageCollection[72].ToString();
            BossListSpecificRoleForthisBossLabel.Content = LanguageCollection[73].ToString();
            BSA1CMLabel.Content = LanguageCollection[74].ToString();
            BSA1CMLabel1.Content = LanguageCollection[75].ToString();
            BSA2CMLabel.Content = LanguageCollection[76].ToString();
            BSA2CMLabel1.Content = LanguageCollection[77].ToString();
            BSA3CMLabel.Content = LanguageCollection[78].ToString();
            BSA3CMLabel1.Content = LanguageCollection[79].ToString();
            NTA1CMLabel.Content = LanguageCollection[80].ToString();
            NTA2CMLabel.Content = LanguageCollection[81].ToString();
            NTA3CMLabel.Content = LanguageCollection[82].ToString();
            IRA1CMLabel.Content = LanguageCollection[83].ToString();
            IRA2CMLabel.Content = LanguageCollection[84].ToString();
            IRA3CMLabel.Content = LanguageCollection[85].ToString();


            if (Settings.Default["BSA1CM"].ToString() == "")
            {
                BSA1CMTextBox.Text = LanguageCollection[43].ToString();
            }
            else
            {
                BSA1CMTextBox.Text = Settings.Default["BSA1CM"].ToString();
            }
            if (Settings.Default["BSA2CM"].ToString() == "")
            {
                BSA2CMTextBox.Text = LanguageCollection[43].ToString();
            }
            else
            {
                BSA2CMTextBox.Text = Settings.Default["BSA2CM"].ToString();
            }
            if (Settings.Default["BSA3CM"].ToString() == "")
            {
                BSA3CMTextBox.Text = LanguageCollection[43].ToString();
            }
            else
            {
                BSA3CMTextBox.Text = Settings.Default["BSA3CM"].ToString();
            }

            if (Settings.Default["NTA1CM"].ToString() == "")
            {
                NTA1CMTextbBox.Text = LanguageCollection[44].ToString();
            }
            else
            {
                NTA1CMTextbBox.Text = Settings.Default["NTA1CM"].ToString();
            }
            if (Settings.Default["NTA2CM"].ToString() == "")
            {
                NTA2CMTextbBox.Text = LanguageCollection[44].ToString();
            }
            else
            {
                NTA2CMTextbBox.Text = Settings.Default["NTA2CM"].ToString();
            }
            if (Settings.Default["NTA3CM"].ToString() == "")
            {
                NTA3CMTextbBox.Text = LanguageCollection[44].ToString();
            }
            else
            {
                NTA3CMTextbBox.Text = Settings.Default["NTA3CM"].ToString();
            }

            if (Settings.Default["IRA1CM"].ToString() == "")
            {
                IRA1CMTextBox.Text = LanguageCollection[45].ToString();
            }
            else
            {
                IRA1CMTextBox.Text = Settings.Default["IRA1CM"].ToString();
            }
            if (Settings.Default["IRA2CM"].ToString() == "")
            {
                IRA2CMTextBox.Text = LanguageCollection[45].ToString();
            }
            else
            {
                IRA2CMTextBox.Text = Settings.Default["IRA2CM"].ToString();
            }
            if (Settings.Default["IRA3CM"].ToString() == "")
            {
                IRA3CMTextBox.Text = LanguageCollection[45].ToString();
            }
            else
            {
                IRA3CMTextBox.Text = Settings.Default["IRA3CM"].ToString();
            }
            if (Settings.Default["ITRA1CM"].ToString() == "")
            {
                ITRA1CMTextBox.Text = LanguageCollection[88].ToString();
            }
            else
            {
                ITRA1CMTextBox.Text = Settings.Default["ITRA1CM"].ToString();
            }
            if (Settings.Default["ITRA2CM"].ToString() == "")
            {
                ITRA2CMTextBox.Text = LanguageCollection[88].ToString();
            }
            else
            {
                ITRA2CMTextBox.Text = Settings.Default["ITRA2CM"].ToString();
            }
            if (Settings.Default["ITRA3CM"].ToString() == "")
            {
                ITRA3CMTextBox.Text = LanguageCollection[88].ToString();
            }
            else
            {
                ITRA3CMTextBox.Text = Settings.Default["ITRA3CM"].ToString();
            }
            if (Settings.Default["BRA1CM"].ToString() == "")
            {
                BRA1CMTextBox.Text = LanguageCollection[89].ToString();
            }
            else
            {
                BRA1CMTextBox.Text = Settings.Default["BRA1CM"].ToString();
            }
            if (Settings.Default["BRA2CM"].ToString() == "")
            {
                BRA2CMTextBox.Text = LanguageCollection[89].ToString();
            }
            else
            {
                BRA2CMTextBox.Text = Settings.Default["BRA2CM"].ToString();
            }
            if (Settings.Default["BRA3CM"].ToString() == "")
            {
                BRA3CMTextBox.Text = LanguageCollection[89].ToString();
            }
            else
            {
                BRA3CMTextBox.Text = Settings.Default["BRA3CM"].ToString();
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

            if (Settings.Default["DisplayTimeTableSetting"].ToString() == "")
            { DisplayTimeTableSetting.IsChecked = true; }
            if (Settings.Default["DisplayTimeTableSetting"].ToString() == "1")
            { DisplayTimeTableSetting.IsChecked = true; }
            if (Settings.Default["DisplayTimeTableSetting"].ToString() == "0")
            { DisplayTimeTableSetting.IsChecked = false; }

            if (Settings.Default["OverlayTransparency"].ToString() == "")
            { TransparacySlider.Value = 100; }
            else { TransparacySlider.Value = double.Parse(Settings.Default["OverlayTransparency"].ToString()); }

            if (Settings.Default["SettingKeepMessages"].ToString() == "")
            {
                SettingsKeepMessagesCheckBox.IsChecked = false;
                RMmtextbox.IsEnabled = true;
            }
            if (Settings.Default["SettingKeepMessages"].ToString() == "1")
            {
                SettingsKeepMessagesCheckBox.IsChecked = true;
                RMmtextbox.IsEnabled = false;
            }
            if (Settings.Default["SettingKeepMessages"].ToString() == "0")
            {
                SettingsKeepMessagesCheckBox.IsChecked = false;
                RMmtextbox.IsEnabled = true;
            }
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

        }
        private async void GetTimeTable(string r)// creat TimeTable and Get Time Logs
        {
            currentbossrole1 = "";
            currentbossrole2 = "";
            publicbossUrl = "";
            timer1.Stop();
            MOTR = r;
            startupRS = 0;
            var html = @"https://mmotimer.com/bdo/?server=" + r;
            HtmlWeb web = new HtmlWeb();           
            var htmlDoc = new HtmlDocument();
            try { htmlDoc = web.Load(html); } catch { MessageBox.Show("Error:404"); }
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
                if (bit == "" && TimeTable.Rows.Count == 1 && TimeTable.Columns.Count > 0 && Crow == 0)
                {
                    TimeTable.Rows.Add(new object[] { "" });
                    Ccolumn = 0;
                    //tableString += Environment.NewLine;
                    Crow++;
                }
                if (bit != "" && Crow == 0)
                {
                    TimeTable.Columns.Add(Ccolumn.ToString());
                    if (Ccolumn == 0)
                    { TimeTable.Rows.Add(new object[] { "" }); }
                    DataRow dr = TimeTable.Rows[Crow];
                    string bitP = bit;
                    if (bit.Contains(","))
                    { bitP = bit.Replace(",", "," + Environment.NewLine); }
                    dr[Ccolumn] = bitP;
                    //tableString += "     " + bitP;
                    MaxColumn++;
                    Ccolumn++;
                    //MessageBox.Show(bit);
                }

                if (bit == "" && Ccolumn == MaxColumn && TimeTable.Columns.Count > 0)
                {
                    TimeTable.Rows.Add(new object[] { "" });
                    Ccolumn = 0;
                    //tableString += Environment.NewLine;
                    Crow++;
                }
                if (bit == "" && TimeTable.Rows.Count > 1 && Ccolumn > 0 && Ccolumn < MaxColumn)
                {
                    DataRow dr = TimeTable.Rows[Crow];
                    string bitP = bit;
                    if (bit.Contains(","))
                    { bitP = bit.Replace(",", "," + Environment.NewLine); }
                    dr[Ccolumn] = bitP;
                    //tableString += "     " + bitP;
                    Ccolumn++;
                }
                if (bit != "" && TimeTable.Rows.Count > 1 && Ccolumn < MaxColumn)
                {
                    DataRow dr = TimeTable.Rows[Crow];
                    string bitP = bit;
                    if (bit.Contains(","))
                    { bitP = bit.Replace(",", "," + Environment.NewLine); }
                    dr[Ccolumn] = bitP;
                    //tableString += "     " + bitP;
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
            //int timeC = 0;
            int dayC = 0;
            foreach (DataRow row in TimeTable.Rows)
            {
                if (aftertheNext.Contains(row["0"].ToString()))
                { dayC = drow; }
                drow++;
            }
            SharedDay = dayC;
            gridview1.ItemsSource = TimeTable.DefaultView;
            startupRS = 1;
            try
            {
                BossesCollection.Clear();
                BossCollectionListBox.Items.Clear();
            }
            catch (Exception) { }
            string BossesList = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses");
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

            PBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Boss.png"));
            NBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Boss.png"));
            LBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Boss.png"));
            Getimg gm = new Getimg();

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
                    if(PbossNamLabel.Content.ToString().Contains("&"))
                    { try{ PBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/" + PbossNamLabel.Content.ToString() + ".png"); } catch (Exception){ } }
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
                        { try { NBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/" + CbossNameLabel.Content.ToString() + ".png"); } catch (Exception) { } }
                        //if (bossdata[3].ToString() != "")
                        //{ CbossNameLabel.Content = bossdata[3].ToString(); }
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
                    { try { LBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/" + NBossNameLabel.Content.ToString() + ".png"); } catch (Exception){ } }
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
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        currentbossrole1 = bossdata[4].ToString();
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
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        currentbossrole2 = bossdata[4].ToString();
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
                            currentbossrole1 = bossdata[4].ToString();
                            CbossNameLabel.Content = bossdata[3].ToString();
                        }
                        else
                        {
                            currentbossrole1 = bossdata[4].ToString();
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
            try
            {
                var guild = discord.GetGuild(ServerID);
                var channel = guild.GetTextChannel(ChannelID);
                var Message = await channel.GetMessageAsync(bossImageID) as IUserMessage;
                string[] pbu = publicbossUrl.Split('|');
                string[] bnu = CbossNameLabel.Content.ToString().Split('&');
                string ANmessage = "";
                if (CbossNameLabel.Content.ToString().Contains("&"))
                {
                    ANmessage = "Bosses Locations" + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[87].ToString() + Environment.NewLine + "[" + bnu[1] + "](" + pbu[1] + ")" + " <---" + LanguageCollection[87].ToString();
                }
                else
                {
                    ANmessage = "Bosses Locations" + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[87].ToString();
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
            timer1.Start();
        }       
        void timer1_Tick(object sender, EventArgs e) //Timer Loop message Updates&others
        {
            //if (checkversion == 0)
            //{
            //    timer1.Stop();
            //    if (AppVersion != CurrentVersion)
            //    {
            //        var bc = new BrushConverter();
            //        GitHub.Background = (Brush)bc.ConvertFrom("#99773139");

            //        DoubleAnimation da = new DoubleAnimation();
            //        da.From = 0.3;
            //        da.To = 1;
            //        da.Duration = new Duration(TimeSpan.FromSeconds(1));
            //        da.AutoReverse = true;
            //        da.RepeatBehavior = RepeatBehavior.Forever;
            //        GitHub.BeginAnimation(OpacityProperty, da);

            //        string urlAddress1 = "https://raw.githubusercontent.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/master/YPBBT%202.0/NewVersionLog_" + DefaultLanguage;

            //        HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(urlAddress1);
            //        HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();

            //        if (response1.StatusCode == HttpStatusCode.OK)
            //        {
            //            Stream receiveStream1 = response1.GetResponseStream();
            //            StreamReader readStream1 = null;

            //            if (String.IsNullOrWhiteSpace(response1.CharacterSet))
            //                readStream1 = new StreamReader(receiveStream1);
            //            else
            //                readStream1 = new StreamReader(receiveStream1, Encoding.GetEncoding(response1.CharacterSet));

            //            string NewVersionLog = readStream1.ReadToEnd();
            //            response1.Close();
            //            readStream1.Close();

            //            MessageBoxResult result = MessageBox.Show(NewVersionLog, "Update!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            //            switch (result)
            //            {
            //                case MessageBoxResult.Yes:
            //                    System.Diagnostics.Process.Start("https://github.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/releases");
            //                    GetTimeTable(MOTR);
            //                    break;
            //                case MessageBoxResult.No:
            //                    GetTimeTable(MOTR);
            //                    break;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        timer1.Start();
            //    }
            //    checkversion = 1;
            //}
       

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

            DateTime PDT = DateTime.Parse(PreviousBossTimeLabel.Content.ToString());
            string pdtl = PDT.AddSeconds(1).ToString(@"hh\:mm\:ss");
            if (pdtl.StartsWith("12:"))
            { pdtl = "00" + PDT.AddSeconds(1).ToString(@":mm\:ss"); }
            PreviousBossTimeLabel.Content = pdtl;

            DateTime CDT = DateTime.Parse(CurrentBossTimeLabel.Content.ToString());
            string cbtl = CDT.AddSeconds(-1).ToString(@"hh\:mm\:ss");
            if (cbtl.StartsWith("12:"))
            { cbtl = "00" + CDT.AddSeconds(-1).ToString(@":mm\:ss"); }
            CurrentBossTimeLabel.Content = cbtl;

            DateTime NDT = DateTime.Parse(NextBossTimeLabel.Content.ToString());
            string nbtl = NDT.AddSeconds(-1).ToString(@"hh\:mm\:ss");
            if (nbtl.StartsWith("12:"))
            { nbtl = "00" + NDT.AddSeconds(-1).ToString(@":mm\:ss"); }
            NextBossTimeLabel.Content = nbtl;

            DateTime NiBDO = DateTime.Parse(NightInBdoTimeLabel.Content.ToString());
            string nibdol = NiBDO.AddSeconds(-1).ToString(@"hh\:mm\:ss");
            if (nibdol.StartsWith("12:"))
            { nibdol = "00" + NiBDO.AddSeconds(-1).ToString(@":mm\:ss"); }
            NightInBdoTimeLabel.Content = nibdol;

            DateTime IR = DateTime.Parse(IRTimeLabel.Content.ToString());
            string irl = IR.AddSeconds(-1).ToString(@"hh\:mm\:ss");
            if (irl.StartsWith("12:"))
            { irl = "00" + IR.AddSeconds(-1).ToString(@":mm\:ss"); }
            IRTimeLabel.Content = irl;

            DateTime BRI = DateTime.Parse(BRTimeLabel.Content.ToString());
            string bril = BRI.AddSeconds(-1).ToString(@"hh\:mm\:ss");
            if (bril.StartsWith("12:"))
            { bril = "00" + BRI.AddSeconds(-1).ToString(@":mm\:ss"); }
            BRTimeLabel.Content = bril;

            DateTime ITRI = DateTime.Parse(ITRITimeLabel.Content.ToString());
            string itril = ITRI.AddSeconds(-1).ToString(@"hh\:mm\:ss");
            if (itril.StartsWith("12:"))
            { itril = "00" + ITRI.AddSeconds(-1).ToString(@":mm\:ss"); }
            ITRITimeLabel.Content = itril;


            if (overlayState == 1)
            {
                try
                {
                    BitmapImage img = new BitmapImage();
                    img = NBImageBox.Source as BitmapImage;
                    omw.UpdateData(LanguageCollection[42].ToString(), img, CbossLabel.Content.ToString(), CbossNameLabel.Content.ToString(), CurrentBossTimeLabel.Content.ToString(), NILabel.Content.ToString(), NightInBdoTimeLabel.Content.ToString(), IRILabel.Content.ToString(), IRTimeLabel.Content.ToString(), soundLabel.Content.ToString(), SoundOptionCheckBox.Content.ToString(), NTSoundOptionCheckBox.Content.ToString(), IRSoundOptionCheckBox.Content.ToString());
                }
                catch (Exception) { }

            }

            if (CurrentBossTimeLabel.Content.ToString() != "00:00:00")
            {
                if (NightInBdoTimeLabel.Content.ToString() == "00:00:00")
                {
                    if (SourceComboBox.SelectedIndex == 1)
                    { GetTimeTable(MOTR); }//Get info from Html Code
                    if (SourceComboBox.SelectedIndex == 2)
                    { GetUrlSource("https://bdobosstimer.com/?&server=" + MOTR); }
                }
                if (IRTimeLabel.Content.ToString() == "00:00:00")
                {
                    if (SourceComboBox.SelectedIndex == 1)
                    { GetTimeTable(MOTR); }//Get info from Html Code
                    if (SourceComboBox.SelectedIndex == 2)
                    { GetUrlSource("https://bdobosstimer.com/?&server=" + MOTR); }
                }
                if (BRTimeLabel.Content.ToString() == "00:00:00")
                {
                    if (SourceComboBox.SelectedIndex == 1)
                    { GetTimeTable(MOTR); }//Get info from Html Code
                    if (SourceComboBox.SelectedIndex == 2)
                    { GetUrlSource("https://bdobosstimer.com/?&server=" + MOTR); }
                }
                if (ITRITimeLabel.Content.ToString() == "00:00:00")
                {
                    if (SourceComboBox.SelectedIndex == 1)
                    { GetTimeTable(MOTR); }//Get info from Html Code
                    if (SourceComboBox.SelectedIndex == 2)
                    { GetUrlSource("https://bdobosstimer.com/?&server=" + MOTR); }
                }
            }

            if (CurrentBossTimeLabel.Content.ToString() == "00:00:00")
            {
                if (SourceComboBox.SelectedIndex == 1)
                { GetTimeTable(MOTR); }//Get info from Html Code
                if (SourceComboBox.SelectedIndex == 2)
                { GetUrlSource("https://bdobosstimer.com/?&server=" + MOTR); }
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
                if (NightTimeAlarmCheckBox1.IsChecked == true && NightInBdoTimeLabel.Content.ToString() == NightTimeAlarm1Textbox.Text + ":00" + ":00")
                {
                    string message = NTA1CMTextbBox.Text;
                    DiscordNotifyNightTime(message);
                    if (NTSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playNightAlertaudio();
                    }
                }

                if (NightTimeAlarmCheckBox2.IsChecked == true && NightInBdoTimeLabel.Content.ToString() == NightTimeAlarm2Textbox.Text + ":00" + ":00")
                {
                    string message = NTA2CMTextbBox.Text;
                    DiscordNotifyNightTime(message);
                    if (NTSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playNightAlertaudio();
                    }
                }

                if (NightTimeAlarmCheckBox3.IsChecked == true && NightInBdoTimeLabel.Content.ToString() == NightTimeAlarm3Textbox.Text  +":00" + ":00")
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
            if (NightTimeAlarmCheckBox1.IsChecked == true && NightInBdoTimeLabel.Content.ToString() == "00:" + NightTimeAlarm1Textbox.Text + ":00")
            {
                string message = NTA1CMTextbBox.Text;
                DiscordNotifyNightTime(message);
                if (NTSoundOptionCheckBox.IsChecked == true)
                {
                    playAudio pa = new playAudio();
                    pa.playNightAlertaudio();
                }
            }

            if (NightTimeAlarmCheckBox2.IsChecked == true && NightInBdoTimeLabel.Content.ToString() == "00:" + NightTimeAlarm2Textbox.Text + ":00")
            {
                string message = NTA2CMTextbBox.Text;
                DiscordNotifyNightTime(message);
                if (NTSoundOptionCheckBox.IsChecked == true)
                {
                    playAudio pa = new playAudio();
                    pa.playNightAlertaudio();
                }
            }

            if (NightTimeAlarmCheckBox3.IsChecked == true && NightInBdoTimeLabel.Content.ToString() == "00:" + NightTimeAlarm3Textbox.Text + ":00")
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
                if (ImperialResetCheckBox1.IsChecked == true && IRTimeLabel.Content.ToString() ==  ImperialResetAlarm1Textbox.Text + ":00" + ":00")
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
                    if (IRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playImperialResetAlertaudio();
                    }
                }

                if (BRAlarmCheckBox2.IsChecked == true && BRTimeLabel.Content.ToString() == BRAlarm2Textbox.Text + ":00" + ":00")
                {
                    string message = BRA2CMTextBox.Text;
                    DiscordNotifyBR(message);
                    if (IRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playImperialResetAlertaudio();
                    }
                }

                if (BRAlarmCheckBox3.IsChecked == true && BRTimeLabel.Content.ToString() == BRAlarm3Textbox.Text + ":00" + ":00")
                {
                    string message = BRA3CMTextBox.Text;
                    DiscordNotifyBR(message);
                    if (IRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playImperialResetAlertaudio();
                    }
                }
            }
            if (BRAlarmCheckBox1.IsChecked == true && BRTimeLabel.Content.ToString() == "00:" + BRAlarm1Textbox.Text + ":00")
            {
                string message = BRA1CMTextBox.Text;
                DiscordNotifyBR(message);
                if (IRSoundOptionCheckBox.IsChecked == true)
                {
                    playAudio pa = new playAudio();
                    pa.playImperialResetAlertaudio();
                }
            }

            if (BRAlarmCheckBox2.IsChecked == true && BRTimeLabel.Content.ToString() == "00:" + BRAlarm2Textbox.Text + ":00")
            {
                string message = BRA2CMTextBox.Text;
                DiscordNotifyBR(message);
                if (IRSoundOptionCheckBox.IsChecked == true)
                {
                    playAudio pa = new playAudio();
                    pa.playImperialResetAlertaudio();
                }
            }

            if (BRAlarmCheckBox3.IsChecked == true && BRTimeLabel.Content.ToString() == "00:" + BRAlarm3Textbox.Text + ":00")
            {
                string message = BRA3CMTextBox.Text;
                DiscordNotifyBR(message);
                if (IRSoundOptionCheckBox.IsChecked == true)
                {
                    playAudio pa = new playAudio();
                    pa.playImperialResetAlertaudio();
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
                    if (IRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playImperialResetAlertaudio();
                    }
                }

                if (ITRAlarmCheckBox2.IsChecked == true && ITRITimeLabel.Content.ToString() == ITRAlarm2Textbox.Text + ":00" + ":00")
                {
                    string message = ITRA2CMTextBox.Text;
                    DiscordNotifyITR(message);
                    if (IRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playImperialResetAlertaudio();
                    }
                }

                if (ITRAlarmCheckBox3.IsChecked == true && ITRITimeLabel.Content.ToString() == ITRAlarm3Textbox.Text + ":00" + ":00")
                {
                    string message = ITRA3CMTextBox.Text;
                    DiscordNotifyITR(message);
                    if (IRSoundOptionCheckBox.IsChecked == true)
                    {
                        playAudio pa = new playAudio();
                        pa.playImperialResetAlertaudio();
                    }
                }
            }
            if (ITRAlarmCheckBox1.IsChecked == true && ITRITimeLabel.Content.ToString() == "00:" + ITRAlarm1Textbox.Text + ":00")
            {
                string message = ITRA1CMTextBox.Text;
                DiscordNotifyITR(message);
                if (IRSoundOptionCheckBox.IsChecked == true)
                {
                    playAudio pa = new playAudio();
                    pa.playImperialResetAlertaudio();
                }
            }

            if (ITRAlarmCheckBox2.IsChecked == true && ITRITimeLabel.Content.ToString() == "00:" + ITRAlarm2Textbox.Text + ":00")
            {
                string message = ITRA2CMTextBox.Text;
                DiscordNotifyITR(message);
                if (IRSoundOptionCheckBox.IsChecked == true)
                {
                    playAudio pa = new playAudio();
                    pa.playImperialResetAlertaudio();
                }
            }

            if (ITRAlarmCheckBox3.IsChecked == true && ITRITimeLabel.Content.ToString() == "00:" + ITRAlarm3Textbox.Text + ":00")
            {
                string message = ITRA3CMTextBox.Text;
                DiscordNotifyITR(message);
                if (IRSoundOptionCheckBox.IsChecked == true)
                {
                    playAudio pa = new playAudio();
                    pa.playImperialResetAlertaudio();
                }
            }
            #endregion


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

            if (DiscordBotConnectionStatusLabel.Content.ToString() == LanguageCollection[39].ToString() && intervalMessageUpdate == UpdateMesssageInterval)
            {
                try { EditMessage(); } catch (Exception) { }
            }
            if (intervalMessageUpdate >= UpdateMesssageInterval)
            { intervalMessageUpdate = 0; }
            intervalMessageUpdate++;

            if (DiscordBotConnectionStatusLabel.Content.ToString() == LanguageCollection[39].ToString() && AnouncmentIntervalToDeleteMessages == AnouncmentMessageInterval && SettingsKeepMessagesCheckBox.IsChecked == false)
            {
                try { DeleteAnouncmentMessages(); } catch (Exception) { }
            }
            if (DiscordBotConnectionStatusLabel.Content.ToString() == LanguageCollection[39].ToString() && AnouncmentIntervalToDeleteMessages == AnouncmentMessageInterval && SettingsKeepMessagesCheckBox.IsChecked == true)
            {
                if (DiscordNotifyBossSpwnID != 0)
                { DiscordNotifyBossSpwnID = 0; }
                if (DiscordNotifyNightTimeID != 0)
                { DiscordNotifyNightTimeID = 0; }
                if (DiscordNotifyImperialResetID != 0)
                { DiscordNotifyImperialResetID = 0; }
                if (DiscordNotifyBarteringID != 0)
                { DiscordNotifyBarteringID = 0; }
                if (DiscordNotifyImperialTradingID != 0)
                { DiscordNotifyImperialTradingID = 0; }
            }
            if (AnouncmentIntervalToDeleteMessages >= AnouncmentMessageInterval)
            { AnouncmentIntervalToDeleteMessages = 0; }
            AnouncmentIntervalToDeleteMessages++;
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
            mediaElement.Position = new TimeSpan(0, 0, 4);
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
                //MessageBox.Show(i.ToString());
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
                {GetTimeTable(RegionComboBox.Text.ToLower());}
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

        private void BossCollectionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)//readBosses Data on select
        {
            try
            {
                Getimg gm = new Getimg();
                DisplayImageLinkextBox.BorderBrush = Brushes.Silver;
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
                        AddSaveBossPictureBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Boss.png"));
                        if(bossdata[5].ToString() == "")
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
                        SRFTBTextbox.Text = bossdata[4].ToString();
                    }
                }
            }
            catch (Exception) { }
        }

        private void AddSaveBossNameTextBox_TextChanged_1(object sender, TextChangedEventArgs e)//check if boss exist
        {

            int i = 0;
            foreach (string item in BossesCollection)
            {
                string bossName = item.Substring(0, item.IndexOf(",") + 1);
                bossName = bossName.Replace(",", "");
                bossName = bossName.Replace(Environment.NewLine, "");

                //MessageBox.Show(bossName);
                if (i == 0)
                {
                    if (bossName.ToLower() == AddSaveBossNameTextBox.Text.ToLower())
                    {
                        i++;

                    }

                }

            }
            if (i == 0)
            {
                AddSaveBossButton.Content = LanguageCollection[53].ToString();
            }
            else
            {
                AddSaveBossButton.Content = LanguageCollection[54].ToString();
            }
        }

        private void AddnewBossTestImgLinkButton_Click(object sender, RoutedEventArgs e)// test image url loadasync() code base
        {
            Getimg gm = new Getimg();
            DisplayImageLinkextBox.BorderBrush = Brushes.Silver;
            AddSaveBossPictureBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Boss.png"));
            string imgurl = DisplayImageLinkextBox.Text;
            //if (imgurl.Contains("<Local>"))
            //{ imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
            try { AddSaveBossPictureBox.Source = gm.GETIMAGE(imgurl); } catch (Exception) { DisplayImageLinkextBox.BorderBrush = Brushes.Red; }
        }

        private void BossListButton_Click(object sender, RoutedEventArgs e)// change Tabcontrol
        {
            string BossList = "";
            foreach (string item in BossesCollection)
            {
                if (item != "")
                { BossList += item + Environment.NewLine; }
            }
            File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses", BossList);
            if (SourceComboBox.SelectedIndex == 1)
            { GetTimeTable(MOTR); }//Get info from Html Code
            if (SourceComboBox.SelectedIndex == 2)
            { GetUrlSource("https://bdobosstimer.com/?&server=" + MOTR); }
            tabcontrol1.SelectedIndex = 0;
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
        }

        private void AddSaveBossButton_Click(object sender, RoutedEventArgs e)//save bosses changes if true
        {
            if (AddSaveBossNameTextBox.Text != "")
            {
                if (AddSaveBossButton.Content.ToString() == LanguageCollection[54].ToString())
                {
                    int i = 0;
                    int r = 0;
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
                    BossesCollection[r] = AddSaveBossNameTextBox.Text + "," + DisplayImageLinkextBox.Text + "," + BossSpawnLocationLinkTextBox.Text + "," + CBNTextbox.Text + "," + SRFTBTextbox.Text + "," + DisplayImageLinkextBoxLocal.Text;
                    MessageBox.Show(LanguageCollection[86].ToString());
                }
                else
                {
                    BossesCollection.Add(AddSaveBossNameTextBox.Text + "," + DisplayImageLinkextBox.Text + "," + BossSpawnLocationLinkTextBox.Text + "," + CBNTextbox.Text + "," + SRFTBTextbox.Text + "," + DisplayImageLinkextBoxLocal.Text);
                    BossCollectionListBox.Items.Add(AddSaveBossNameTextBox.Text);
                }
            }
        }

        private void RemoveBossButton_Click(object sender, RoutedEventArgs e)//remove selected boss from listbox
        {
            if (BossCollectionListBox.SelectedIndex != -1)
            {
                BossesCollection.RemoveAt(BossCollectionListBox.SelectedIndex);
                BossCollectionListBox.Items.RemoveAt(BossCollectionListBox.SelectedIndex);
            }
        }

        private void BossesListButton_Click(object sender, RoutedEventArgs e)//change tabcontrol
        {
            tabcontrol1.SelectedIndex = 2;
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
        }

        private void TimeTableButton_Click(object sender, RoutedEventArgs e)//change tabcontrol
        {
            TimeTableLabel.Content = MOTR.ToUpper() + LanguageCollection[40].ToString();
            tabcontrol1.SelectedIndex = 1;
            TimeTableGrid.Visibility = Visibility.Visible;
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
        }

        private void TimeTableBackButton_Click(object sender, RoutedEventArgs e)// go back to tab0
        {
            TimeTableGrid.Visibility = Visibility.Hidden;
            tabcontrol1.SelectedIndex = 0;
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
            //try
            //{
            //    startupRS = 0;
            //    string region = Settings.Default["DefaultRegion"].ToString();
            //    if (region == "ps4-asia")
            //    { RegionComboBox.SelectedIndex = 0; }
            //    if (region == "eu")
            //    { RegionComboBox.SelectedIndex = 1; }
            //    if (region == "ps4-xbox-eu")
            //    { RegionComboBox.SelectedIndex = 2; }
            //    if (region == "jp")
            //    { RegionComboBox.SelectedIndex = 3; }
            //    if (region == "kr")
            //    { RegionComboBox.SelectedIndex = 4; }
            //    if (region == "mena")
            //    { RegionComboBox.SelectedIndex = 5; }
            //    if (region == "na")
            //    { RegionComboBox.SelectedIndex = 6; }
            //    if (region == "ps4-xbox-na")
            //    { RegionComboBox.SelectedIndex = 7; }
            //    if (region == "ru")
            //    { RegionComboBox.SelectedIndex = 8; }
            //    if (region == "sa")
            //    { RegionComboBox.SelectedIndex = 9; }
            //    if (region == "sea")
            //    { RegionComboBox.SelectedIndex = 10; }
            //    if (region == "th")
            //    { RegionComboBox.SelectedIndex = 11; }
            //    if (region == "tw")
            //    { RegionComboBox.SelectedIndex = 12; }
            //    startupRS = 1;
            //}
            //catch (Exception) { }
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

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)// on closing action disconnect discord and animate fade out
        {
            try
            {
                var guild = discord.GetGuild(ServerID);
                var channel = guild.GetTextChannel(ChannelID);
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
            System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/YPBBT.exe");
            this.Close();
        }

        private void ConnectDiscordBotButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discordapp.com/oauth2/authorize?&client_id=" + ClientID + "&scope=bot&permissions=0");
        }
        private async void DisconnectDiscordBot_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            await discord.StopAsync();
            DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFBB3D3D");
            DiscordBotConnectionStatusLabel.Content = LanguageCollection[14].ToString();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)// save language changes
        {
            LanguageExpander1.IsExpanded = false;
            if (LanguageListBox.SelectedIndex == 0)
            {
                DefaultLanguage = "fr";
                LoadDefaultLanguage();
            }
            if (LanguageListBox.SelectedIndex == 1)
            {
                DefaultLanguage = "en";
                LoadDefaultLanguage();
            }
            if (LanguageListBox.SelectedIndex == 2)
            {
                DefaultLanguage = "es";
                LoadDefaultLanguage();
            }
            if (LanguageListBox.SelectedIndex == 3)
            {
                DefaultLanguage = "ru";
                LoadDefaultLanguage();
            }
            if (LanguageListBox.SelectedIndex == 4)
            {
                DefaultLanguage = "jp";
                LoadDefaultLanguage();
            }
            if (LanguageListBox.SelectedIndex == 5)
            {
                DefaultLanguage = "kr";
                LoadDefaultLanguage();
            }
            LanguageExpander1.Header = LanguageListBox.Items[LanguageListBox.SelectedIndex].ToString().Replace("System.Windows.Controls.ListBoxItem: ", "");

        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)// load Profile settings on click
        {
            CidTextbox.Text = ClientID.ToString();
            TokenTextbox.Text = Token;
            SidTextbox.Text = ServerID.ToString();
            ChidTextbox.Text = ChannelID.ToString();
            BSTextbox.Text = BossSpawnRole;
            NTtextbox.Text = NightTimeRole;
            IRTextbox.Text = ImperialResetRole;
            BRPRtextbox.Text = BarteringResetRole;
            ITRPRTextbox.Text = ImperialTradingResetRole;
            MIUTextbox.Text = UpdateMesssageInterval.ToString();
            RMmtextbox.Text = AnouncmentMessageInterval.ToString();
            if (DefaultLanguage == "fr")
            { LanguageExpander1.Header = "Français"; }
            if (DefaultLanguage == "en")
            { LanguageExpander1.Header = "English"; }
            if (DefaultLanguage == "es")
            { LanguageExpander1.Header = "Español"; }
            if (DefaultLanguage == "ru")
            { LanguageExpander1.Header = "русский"; }
            if (DefaultLanguage == "jp")
            { LanguageExpander1.Header = "日本人"; }
            if (DefaultLanguage == "kr")
            { LanguageExpander1.Header = "한국어"; }
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

        private void GobackSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default["OverlayTransparency"] = TransparacySlider.Value.ToString();
            Settings.Default.Save();
            ClientID = ulong.Parse(CidTextbox.Text);
            Token = TokenTextbox.Text;
            ServerID = ulong.Parse(SidTextbox.Text);
            ChannelID = ulong.Parse(ChidTextbox.Text);
            BossSpawnRole = BSTextbox.Text;
            NightTimeRole = NTtextbox.Text;
            ImperialResetRole = IRTextbox.Text;
            BarteringResetRole = BRPRtextbox.Text;
            ImperialTradingResetRole = ITRPRTextbox.Text; 
            UpdateMesssageInterval = int.Parse(MIUTextbox.Text);
            AnouncmentMessageInterval = int.Parse(RMmtextbox.Text);
            File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/Profile", DefaultLanguage + "|" + CidTextbox.Text + "|" + TokenTextbox.Text + "|" + SidTextbox.Text + "|" + ChidTextbox.Text + "|" + BSTextbox.Text + "|" + NTtextbox.Text + "|" + IRTextbox.Text + "|" + MIUTextbox.Text + "|" + RMmtextbox.Text +"|"+ BRPRtextbox.Text+"|"+ ITRPRTextbox.Text);
            //StartPosting();
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
            //try
            //{
            //    startupRS = 0;
            //    string region = Settings.Default["DefaultRegion"].ToString();
            //    if (region == "ps4-asia")
            //    { RegionComboBox.SelectedIndex = 0; }
            //    if (region == "eu")
            //    { RegionComboBox.SelectedIndex = 1; }
            //    if (region == "ps4-xbox-eu")
            //    { RegionComboBox.SelectedIndex = 2; }
            //    if (region == "jp")
            //    { RegionComboBox.SelectedIndex = 3; }
            //    if (region == "kr")
            //    { RegionComboBox.SelectedIndex = 4; }
            //    if (region == "mena")
            //    { RegionComboBox.SelectedIndex = 5; }
            //    if (region == "na")
            //    { RegionComboBox.SelectedIndex = 6; }
            //    if (region == "ps4-xbox-na")
            //    { RegionComboBox.SelectedIndex = 7; }
            //    if (region == "ru")
            //    { RegionComboBox.SelectedIndex = 8; }
            //    if (region == "sa")
            //    { RegionComboBox.SelectedIndex = 9; }
            //    if (region == "sea")
            //    { RegionComboBox.SelectedIndex = 10; }
            //    if (region == "th")
            //    { RegionComboBox.SelectedIndex = 11; }
            //    if (region == "tw")
            //    { RegionComboBox.SelectedIndex = 12; }
            //    startupRS = 1;
            //}
            //catch (Exception) { }

        }

        private async void HrdResetAppButton_Click(object sender, RoutedEventArgs e)// format app saved data
        {
            MessageBoxResult result = MessageBox.Show(LanguageCollection[70].ToString(), "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    File.Delete(System.IO.Directory.GetCurrentDirectory() + "/Resources/Profile");
                    Settings.Default["DefaultRegion"] = "";
                    Settings.Default.Save();
                    Settings.Default["BossSpawnAlarmCheckBox"] = "";
                    Settings.Default.Save();
                    Settings.Default["NightTimeAlarmCheckBox"] = "";
                    Settings.Default.Save();
                    Settings.Default["ImperialResetCheckBox"] = "";
                    Settings.Default.Save();
                    Settings.Default["PlaySoundSetting"] = "";
                    Settings.Default.Save();
                    Settings.Default["OverlayTransparency"] = "";
                    Settings.Default.Save();
                    Settings.Default["OverlayState"] = "";
                    Settings.Default.Save();
                    Settings.Default["NTPlaySoundSetting"] = "";
                    Settings.Default.Save();
                    Settings.Default["IRPlaySoundSetting"] = "";
                    Settings.Default.Save();
                    Settings.Default["DisplayTimeTableSetting"] = "";
                    Settings.Default.Save();
                    Settings.Default["BSA1CM"] = "";
                    Settings.Default.Save();
                    Settings.Default["BSA2CM"] = "";
                    Settings.Default.Save();
                    Settings.Default["BSA3CM"] = "";
                    Settings.Default.Save();
                    Settings.Default["NTA1CM"] = "";
                    Settings.Default.Save();
                    Settings.Default["NTA2CM"] = "";
                    Settings.Default.Save();
                    Settings.Default["NTA3CM"] = "";
                    Settings.Default.Save();
                    Settings.Default["IRA1CM"] = "";
                    Settings.Default.Save();
                    Settings.Default["IRA2CM"] = "";
                    Settings.Default.Save();
                    Settings.Default["IRA3CM"] = "";
                    Settings.Default.Save();
                    await discord.StopAsync();
                    System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/YPBBT 2.0.exe");
                    this.Close();
                    break;
                case MessageBoxResult.No:

                    break;
            }
        }

        private void MIUTextbox_TextChanged(object sender, TextChangedEventArgs e)//
        {
            try
            {
                if (int.Parse(MIUTextbox.Text) > 60)
                { MIUTextbox.Text = "60"; }
            }
            catch (Exception) { }
        }
        #region "Help Buttons"
        private void Button_Click(object sender, RoutedEventArgs e)
        { System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/Step2CID.mp4"); }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        { System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/Step4GetToken.mp4"); }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        { System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/ServerID.mp4"); }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        { System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/ChannelID.mp4"); }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        { System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/ConvertRole.mp4"); }
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
            MyNotifyIcon.Visible = true;
            try
            {
                omw.load();
                omw.Visibility = Visibility.Visible;
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
        public void GetOverlayState(int value)
        {
            overlayState = 0;
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

        private async void EditMessage()//update Timers on discord
        {
            try
            {
                var guild = discord.GetGuild(ServerID);
                var channel = guild.GetTextChannel(ChannelID);
                var Message = MainMessage; /*await channel.GetMessageAsync(MainMessageID) as IUserMessage;*/
                string message = "";
                string nb = "> ```cs" + Environment.NewLine + "> " + CurrentBossTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```";
                string nti = "> ```cs" + Environment.NewLine +"> " +NightInBdoTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```";
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


                message = "> " + CbossLabel.Content.ToString() + Environment.NewLine
                    + nb + Environment.NewLine
                    + "> " + NILabel.Content.ToString() + Environment.NewLine
                    + nti + Environment.NewLine
                    + "> " + IRILabel.Content.ToString() + Environment.NewLine
                    + iri + Environment.NewLine
                    + "> " + BRILabel.Content.ToString() + Environment.NewLine
                    + br + Environment.NewLine
                    + "> " + ITRILabel.Content.ToString() + Environment.NewLine
                    + itr;
                //var MainMessage = await channel.SendMessageAsync(message);
              await Message.ModifyAsync(msg => msg.Content = message);//must be non await to prevent over stack call
               MainMessage = Message;
            }
            catch (Exception) { }
        }
        private async void StartPosting()// purge discord channel and start posting
        {
            try
            {
                var bc = new BrushConverter();
                try
                {
                    DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFF1F1F1");
                    DiscordBotConnectionStatusLabel.Content = LanguageCollection[37].ToString();//"Connecting...";
                    await discord.LoginAsync(TokenType.Bot, Token);
                    await discord.StartAsync();
                    System.Threading.Thread.Sleep(1000);
                }
                catch (Exception)
                {
                    DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFBB3D3D");
                    DiscordBotConnectionStatusLabel.Content = LanguageCollection[38].ToString();//"Connection ERROR!";
                }

                if (DiscordBotConnectionStatusLabel.Content.ToString() == LanguageCollection[37].ToString())
                {
                    DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FF70BB88");
                    DiscordBotConnectionStatusLabel.Content = LanguageCollection[39].ToString();//"Connected";
                }

                if (DiscordBotConnectionStatusLabel.Content.ToString() == LanguageCollection[39].ToString())
                {
                    System.Threading.Thread.Sleep(2000);
                    var guild = discord.GetGuild(ServerID);
                    var channel = guild.GetTextChannel(ChannelID);
                    var messages = await channel.GetMessagesAsync(100).FlattenAsync(); //defualt is 100
                    await (channel as SocketTextChannel).DeleteMessagesAsync(messages);
                    string[] pbu = publicbossUrl.Split('|');
                    string[] bnu = CbossNameLabel.Content.ToString().Split('&');
                    string ANmessage = "";
                    if(CbossNameLabel.Content.ToString().Contains("&"))
                    {
                        ANmessage = "Bosses Locations" + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[87].ToString() + Environment.NewLine + "[" + bnu[1] + "](" + pbu[1] + ")" + " <---" + LanguageCollection[87].ToString();
                    }
                    else
                    {
                        ANmessage = "Bosses Locations" + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[87].ToString();
                    }
                    var embed1 = new EmbedBuilder
                    {
                        Title = CbossNameLabel.Content.ToString()/* + " <---" + LanguageCollection[87].ToString()*/,
                        ImageUrl = publicNbossimage,
                        Color = Color.LightGrey,
                        //Url = publicbossUrl,
                        Description = ANmessage
                    };                   
                    var BossImage = await channel.SendMessageAsync("", false, embed1.Build());
                    bossImageID = BossImage.Id;

                    //var embed = new EmbedBuilder
                    //{
                    //    Title = "Boss Name"
                    //};
                    //embed.AddField("Next Boss in:", "00:00:00", true);
                    //embed.AddField("Night In:", "00:00:00", true);
                    //embed.AddField("Imperial Reset:", "00:00:00", true)
                    //     .WithColor(Color.Blue)
                    //     .WithUrl("https://example.com")
                    //    .Build();
                    //var MainMessage = await channel.SendMessageAsync("", false, embed.Build());
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


                    message = "> " + CbossLabel.Content.ToString() + Environment.NewLine
                        + nb + Environment.NewLine
                        + "> " + NILabel.Content.ToString() + Environment.NewLine
                        + nti + Environment.NewLine
                        + "> " + IRILabel.Content.ToString() + Environment.NewLine
                        + iri + Environment.NewLine
                        + "> " + BRILabel.Content.ToString() + Environment.NewLine
                        + br + Environment.NewLine
                        + "> " + ITRILabel.Content.ToString() + Environment.NewLine
                        + itr;
                    MainMessage = await channel.SendMessageAsync(message);                    
                    MainMessageID = MainMessage.Id;
                    if (DisplayTimeTableSetting.IsChecked == true)
                    {
                        var timetablemessage = await channel.SendFileAsync(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png", MOTR.ToString().ToUpper() + LanguageCollection[40].ToString());
                        TimtableID = timetablemessage.Id;
                    }


                }
            }
            catch (Exception s)
            {
                var bc = new BrushConverter();
                DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFBB3D3D");
                DiscordBotConnectionStatusLabel.Content = LanguageCollection[38].ToString();

                Clipboard.SetText(s.Message.ToString());

                string warning = "";
                if (s.Message.ToString().Contains("403"))
                { warning = LanguageCollection[71].ToString(); }
                if (warning == "")
                { MessageBox.Show(s.Message); }
                else
                { MessageBox.Show(warning); }

                /*StartPosting();*/
            }
        }

        private void StartPostingButton_Click(object sender, RoutedEventArgs e)
        {
            if (DiscordBotConnectionStatusLabel.Content.ToString() != LanguageCollection[39].ToString())
            { StartPosting(); }
        }

        private async void DisplayTimeTableSetting_Click(object sender, RoutedEventArgs e)// time table discplay option
        {
            if (DiscordBotConnectionStatusLabel.Content.ToString() == LanguageCollection[39].ToString())
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
                        var channel = guild.GetTextChannel(ChannelID);
                        var timetablemessage = await channel.SendFileAsync(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png", MOTR.ToString().ToUpper() + LanguageCollection[40].ToString());
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
                        var channel = guild.GetTextChannel(ChannelID);
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
            try
            {
                if (DiscordNotifyBossSpwnID == 0)
                {
                    if (bossmessage.Contains("<00:00:00>"))
                    { bossmessage = bossmessage.Replace("<00:00:00>", CurrentBossTimeLabel.Content.ToString()); }
                    var guild = discord.GetGuild(ServerID);
                    var channel = guild.GetTextChannel(ChannelID);
                    var message = await channel.SendMessageAsync(">" + " " + BossSpawnRole + " " + currentbossrole1 + " " + currentbossrole2 + " " + CbossNameLabel.Content.ToString() + " " + bossmessage);
                    DiscordNotifyBossSpwnID = message.Id;
                    AnouncmentIntervalToDeleteMessages = 0;
                }
            }
            catch (Exception) { }
        }
        private async void DiscordNotifyNightTime(string custommessage)
        {
            try
            {
                if (DiscordNotifyNightTimeID == 0)
                {
                    if (custommessage.Contains("<00:00:00>"))
                    { custommessage = custommessage.Replace("<00:00:00>", NightInBdoTimeLabel.Content.ToString()); }
                    var guild = discord.GetGuild(ServerID);
                    var channel = guild.GetTextChannel(ChannelID);
                    var message = await channel.SendMessageAsync(">" + " " + NightTimeRole + " " + custommessage);
                    DiscordNotifyNightTimeID = message.Id;
                    AnouncmentIntervalToDeleteMessages = 0;
                }
            }
            catch (Exception) { }
        }
        private async void DiscordNotifyImperialReset(string custommessage)
        {
            try
            {
                if (DiscordNotifyImperialResetID == 0)
                {
                    if (custommessage.Contains("<00:00:00>"))
                    { custommessage = custommessage.Replace("<00:00:00>", IRTimeLabel.Content.ToString()); }
                    var guild = discord.GetGuild(ServerID);
                    var channel = guild.GetTextChannel(ChannelID);
                    var message = await channel.SendMessageAsync(">" + " " + ImperialResetRole + " " + custommessage);
                    DiscordNotifyImperialResetID = message.Id;
                    AnouncmentIntervalToDeleteMessages = 0;
                }
            }
            catch (Exception) { }
        }
        private async void DiscordNotifyBR(string custommessage)
        {
            try
            {
                if (DiscordNotifyBarteringID == 0)
                {
                    if (custommessage.Contains("<00:00:00>"))
                    { custommessage = custommessage.Replace("<00:00:00>", BRTimeLabel.Content.ToString()); }
                    var guild = discord.GetGuild(ServerID);
                    var channel = guild.GetTextChannel(ChannelID);
                    var message = await channel.SendMessageAsync(">" + " " + BarteringResetRole + " " + custommessage);
                    DiscordNotifyBarteringID = message.Id;
                    AnouncmentIntervalToDeleteMessages = 0;
                }
            }
            catch (Exception) { }
        }
        private async void DiscordNotifyITR(string custommessage)
        {
            try
            {
                if (DiscordNotifyImperialTradingID == 0)
                {
                    if (custommessage.Contains("<00:00:00>"))
                    { custommessage = custommessage.Replace("<00:00:00>", BRTimeLabel.Content.ToString()); }
                    var guild = discord.GetGuild(ServerID);
                    var channel = guild.GetTextChannel(ChannelID);
                    var message = await channel.SendMessageAsync(">" + " " + ImperialTradingResetRole + " " + custommessage);
                    DiscordNotifyImperialTradingID = message.Id;
                    AnouncmentIntervalToDeleteMessages = 0;
                }
            }
            catch (Exception) { }
        }
        #endregion
        private async void DeleteAnouncmentMessages()// delete pinged messaged after interval pass
        {
            try
            {
                var guild = discord.GetGuild(ServerID);
                var channel = guild.GetTextChannel(ChannelID);
                if (DiscordNotifyBossSpwnID != 0)
                {
                    await channel.DeleteMessageAsync(DiscordNotifyBossSpwnID);
                    DiscordNotifyBossSpwnID = 0;
                }
                if (DiscordNotifyNightTimeID != 0)
                {
                    await channel.DeleteMessageAsync(DiscordNotifyNightTimeID);
                    DiscordNotifyNightTimeID = 0;
                }
                if (DiscordNotifyImperialResetID != 0)
                {
                    await channel.DeleteMessageAsync(DiscordNotifyImperialResetID);
                    DiscordNotifyImperialResetID = 0;
                }
                if (DiscordNotifyBarteringID != 0)
                {
                    await channel.DeleteMessageAsync(DiscordNotifyBarteringID);
                    DiscordNotifyBarteringID = 0;
                }
                if (DiscordNotifyImperialTradingID != 0)
                {
                    await channel.DeleteMessageAsync(DiscordNotifyImperialTradingID);
                    DiscordNotifyImperialTradingID = 0;
                }
            }
            catch (Exception) { }
        }
        private void RMmtextbox_TextChanged(object sender, TextChangedEventArgs e)// set max message deletion Interval
        {
            try
            {
                if (int.Parse(RMmtextbox.Text) <= 0)
                { RMmtextbox.Text = "1"; }
                if (int.Parse(RMmtextbox.Text) >= 50)
                { RMmtextbox.Text = "50"; }
            }
            catch (Exception) { }
        }

        private void GitHub_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/releases");
        }
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
            webbrowser1.Address = "about:blank";
           
            //webbrowser1.IsEnabled = false;

            //MessageBox.Show(@"<td class=""head"" scope=""col"">");
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(source);
            currentbossrole1 = "";
            currentbossrole2 = "";
            timer1.Stop();
            startupRS = 0;
          
            //Clipboard.SetText(htmlNodes1);
            //MessageBox.Show(htmlNodes1);
            //var htmlregionDB = htmlDoc.DocumentNode.SelectSingleNode("/html/body/header/div/div/nav/ul/li[2]/ul").InnerText;
            //var aftertheNext = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='mainTbl']/tbody/tr[1]/td[2]/span").InnerText;

            //var pSecondboss = "";
            //try { pSecondboss = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[2]/div[2]/h2").InnerText; } catch (Exception) { }
            PbossNamLabel.Content = "X";
            //if (pSecondboss != "")
            //{ PbossNamLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[2]/div[1]/h2").InnerText + " & " + pSecondboss; }
            //PreviousBossTimeLabel.Content = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[1]/div[2]/div/div/div[1]/div[1]/div[6]/div[1]").InnerText;

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

            source = source.Replace(@"<td class=""head"" scope=""col"">", @"<td class=""head"" scope=""col"">" + Environment.NewLine);
            string Timezone = GetStrBetweenTags(source, @"<option value=""original"">", @"</option>");
            htmlDoc.LoadHtml(source);
            var TimeRow = htmlDoc.DocumentNode.SelectSingleNode("/html/body/section[3]/div/div[2]/div/table/thead/tr").InnerText;
            //MessageBox.Show(TimeRow);
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
                if(day == 7)
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
            //string aftertheNext = Environment.NewLine + DateTime.Now.DayOfWeek.ToString(); // using local day
            //MessageBox.Show(aftertheNext);
            //foreach (DataRow row in TimeTable.Rows)
            //{
            //    DataRow dr = TimeTable.Rows[drow];
            //    if (aftertheNext.Contains(dr[0].ToString()))
            //    { dayC = drow; }
            //    drow++;
            //}
            SharedDay = dayC;
            gridview1.ItemsSource = TimeTable.DefaultView;

            try
            {
                BossesCollection.Clear();
                BossCollectionListBox.Items.Clear();
            }
            catch (Exception) { }
            string BossesList = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses");
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

            PBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Boss.png"));
            NBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Boss.png"));
            LBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Boss.png"));
            Getimg gm = new Getimg();

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
                    { try { PBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/" + PbossNamLabel.Content.ToString() + ".png"); } catch (Exception) { } }
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
                        { try { NBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/" + CbossNameLabel.Content.ToString() + ".png"); } catch (Exception) { } }
                        //if (bossdata[3].ToString() != "")
                        //{ CbossNameLabel.Content = bossdata[3].ToString(); }
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
                    { try { LBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/" + NBossNameLabel.Content.ToString() + ".png"); } catch (Exception) { } }
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
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        currentbossrole1 = bossdata[4].ToString();
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
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        currentbossrole2 = bossdata[4].ToString();
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
                            currentbossrole1 = bossdata[4].ToString();
                            CbossNameLabel.Content = bossdata[3].ToString();
                        }
                        else
                        {
                            currentbossrole1 = bossdata[4].ToString();
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
            try
            {
                var guild = discord.GetGuild(ServerID);
                var channel = guild.GetTextChannel(ChannelID);
                var Message = await channel.GetMessageAsync(bossImageID) as IUserMessage;
                string[] pbu = publicbossUrl.Split('|');
                string[] bnu = CbossNameLabel.Content.ToString().Split('&');
                string ANmessage = "";
                if (CbossNameLabel.Content.ToString().Contains("&"))
                {
                    ANmessage = "Bosses Locations" + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[87].ToString() + Environment.NewLine + "[" + bnu[1] + "](" + pbu[1] + ")" + " <---" + LanguageCollection[87].ToString();
                }
                else
                {
                    ANmessage = "Bosses Locations" + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + LanguageCollection[87].ToString();
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
        private void GlobalSettingNextButton_Click(object sender, RoutedEventArgs e)
        {

            tabcontrol1.SelectedIndex = 4;
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

        private void SaveSettings2Button_Click(object sender, RoutedEventArgs e)
        {
            if (BSA1CMTextBox.Text == "")
            {
                BSA1CMTextBox.Text = LanguageCollection[43].ToString();
            }
            if (BSA1CMTextBox.Text == LanguageCollection[43].ToString())
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
                BSA2CMTextBox.Text = LanguageCollection[43].ToString();
            }
            if (BSA2CMTextBox.Text == LanguageCollection[43].ToString())
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
                BSA3CMTextBox.Text = LanguageCollection[43].ToString();
            }
            if (BSA3CMTextBox.Text == LanguageCollection[43].ToString())
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
                NTA1CMTextbBox.Text = LanguageCollection[44].ToString();
            }
            if (NTA1CMTextbBox.Text == LanguageCollection[44].ToString())
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
                NTA2CMTextbBox.Text = LanguageCollection[44].ToString();
            }
            if (NTA2CMTextbBox.Text == LanguageCollection[44].ToString())
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
                NTA3CMTextbBox.Text = LanguageCollection[44].ToString();
            }
            if (NTA3CMTextbBox.Text == LanguageCollection[44].ToString())
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
                IRA1CMTextBox.Text = LanguageCollection[45].ToString();
            }
            if (IRA1CMTextBox.Text == LanguageCollection[45].ToString())
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
                IRA2CMTextBox.Text = LanguageCollection[45].ToString();
            }
            if (IRA2CMTextBox.Text == LanguageCollection[45].ToString())
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
                IRA3CMTextBox.Text = LanguageCollection[45].ToString();
            }
            if (IRA3CMTextBox.Text == LanguageCollection[45].ToString())
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
                BRA1CMTextBox.Text = LanguageCollection[89].ToString();
            }
            if (BRA1CMTextBox.Text == LanguageCollection[89].ToString())
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
                BRA2CMTextBox.Text = LanguageCollection[89].ToString();
            }
            if (BRA2CMTextBox.Text == LanguageCollection[89].ToString())
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
                BRA3CMTextBox.Text = LanguageCollection[89].ToString();
            }
            if (BRA3CMTextBox.Text == LanguageCollection[89].ToString())
            {
                Settings.Default["BRA3CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["BRA3CM"] = BRA3CMTextBox.Text;
                Settings.Default.Save();
            }

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
            if (EditSpawnHoursSlider.Value > lastSliderValue)
            {
                DateTime PDT = DateTime.Parse(PreviousBossTimeLabel.Content.ToString());
                string pdtl = PDT.AddHours(-1).ToString(@"hh\:mm\:ss");
                PreviousBossTimeLabel.Content = pdtl;

                DateTime CDT = DateTime.Parse(CurrentBossTimeLabel.Content.ToString());
                string cbtl = CDT.AddHours(1).ToString(@"hh\:mm\:ss");
                CurrentBossTimeLabel.Content = cbtl;

                DateTime NDT = DateTime.Parse(NextBossTimeLabel.Content.ToString());
                string nbtl = NDT.AddHours(1).ToString(@"hh\:mm\:ss");
                NextBossTimeLabel.Content = nbtl;
            }
            else
            {
                DateTime PDT = DateTime.Parse(PreviousBossTimeLabel.Content.ToString());
                string pdtl = PDT.AddHours(1).ToString(@"hh\:mm\:ss");
                PreviousBossTimeLabel.Content = pdtl;

                DateTime CDT = DateTime.Parse(CurrentBossTimeLabel.Content.ToString());
                string cbtl = CDT.AddHours(-1).ToString(@"hh\:mm\:ss");
                CurrentBossTimeLabel.Content = cbtl;

                DateTime NDT = DateTime.Parse(NextBossTimeLabel.Content.ToString());
                string nbtl = NDT.AddHours(-1).ToString(@"hh\:mm\:ss");
                NextBossTimeLabel.Content = nbtl;
            }
            lastSliderValue = EditSpawnHoursSlider.Value;
            if (EditSpawnHoursSlider.Value.ToString().Contains("-"))
            { EditSpawnHoursLabael1.Content = "Edit Spawn Hours " + EditSpawnHoursSlider.Value; }
            else
            { EditSpawnHoursLabael1.Content = "Edit Spawn Hours +" + EditSpawnHoursSlider.Value; }
            if (EditSpawnHoursSlider.Value == 0)
            { EditSpawnHoursLabael1.Content = "Edit Spawn Hours " + EditSpawnHoursSlider.Value; }
            Settings.Default["EditSpawnHoursSlider"] = EditSpawnHoursSlider.Value.ToString();
            Settings.Default.Save();
        }

        private void SettingsKeepMessagesCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsKeepMessagesCheckBox.IsChecked == true)
            {
                Settings.Default["SettingKeepMessages"] = "1";
                Settings.Default.Save();
                RMmtextbox.IsEnabled = false;
            }
            else
            {
                Settings.Default["SettingKeepMessages"] = "0";
                Settings.Default.Save();
                RMmtextbox.IsEnabled = true;
            }
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

        private void AlarmsNextTab_Click(object sender, RoutedEventArgs e)
        {
            AlarmsTabControl.SelectedIndex = 1;
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            AlarmsTabControl.BeginAnimation(OpacityProperty, da);
            AlarmsPreviewsTab.Visibility = Visibility.Visible;
            AlarmsNextTab.Visibility = Visibility.Hidden;
        }

        private void AlarmsPreviewsTab_Click(object sender, RoutedEventArgs e)
        {
            AlarmsTabControl.SelectedIndex = 0;
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            AlarmsTabControl.BeginAnimation(OpacityProperty, da);
            AlarmsPreviewsTab.Visibility = Visibility.Hidden;
            AlarmsNextTab.Visibility = Visibility.Visible;
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

        private void CMNextPage_Click(object sender, RoutedEventArgs e)
        {
            tabcontrol1.SelectedIndex = 5;
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

        private void CMBback_Click(object sender, RoutedEventArgs e)
        {
            if (ITRA1CMTextBox.Text == "")
            {
                ITRA1CMTextBox.Text = LanguageCollection[88].ToString();
            }
            if (ITRA1CMTextBox.Text == LanguageCollection[88].ToString())
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
                ITRA2CMTextBox.Text = LanguageCollection[88].ToString();
            }
            if (ITRA2CMTextBox.Text == LanguageCollection[88].ToString())
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
                ITRA3CMTextBox.Text = LanguageCollection[88].ToString();
            }
            if (ITRA3CMTextBox.Text == LanguageCollection[88].ToString())
            {
                Settings.Default["ITRA3CM"] = "";
                Settings.Default.Save();
            }
            else
            {
                Settings.Default["ITRA3CM"] = ITRA3CMTextBox.Text;
                Settings.Default.Save();
            }

            tabcontrol1.SelectedIndex = 4;
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

        private void SourceHelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (SourceComboBox.SelectedIndex == 1)
            { Process.Start("https://mmotimer.com/bdo/?server=" + MOTR); }
            if (SourceComboBox.SelectedIndex == 2)
            { Process.Start("https://bdobosstimer.com/?&server=" + MOTR); }
        }

        private void BotHostHelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (BotHostComboBox.SelectedIndex == 1)
            { Process.Start("https://polisystems.ch/EN/index"); }
        }

        private void BotHostComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (BotHostComboBox.SelectedIndex == 0)
            { BotHostHelpButton.Visibility = Visibility.Hidden; }
            if (BotHostComboBox.SelectedIndex == 1)
            { BotHostHelpButton.Visibility = Visibility.Visible; }
        }

        private void SourceComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (lastSelectedSource != SourceComboBox.SelectedIndex)
            {
                Settings.Default["SelectedSource"] = SourceComboBox.SelectedIndex.ToString();
                Settings.Default.Save();
                lastSelectedSource = SourceComboBox.SelectedIndex;
                string region = Settings.Default["DefaultRegion"].ToString();//get Last Saved Region xbox-na         
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
            AddSaveBossPictureBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Boss.png"));
            string imgurl = DisplayImageLinkextBoxLocal.Text;
            if (imgurl.Contains("<Local>"))
            { imgurl = imgurl.Replace("<Local>", System.IO.Directory.GetCurrentDirectory()); }
            try { AddSaveBossPictureBox.Source = gm.GETIMAGE(imgurl); } catch (Exception) { DisplayImageLinkextBoxLocal.BorderBrush = Brushes.Red; }
        }

        private void GetLYPBBTTimeTable()
        {
            string timetable = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT");

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
            TimeTableBoss1DropBox.Items.Add("Nouver");
            TimeTableBoss1DropBox.Items.Add("Karanda");
            TimeTableBoss1DropBox.Items.Add("Kutum");
            TimeTableBoss1DropBox.Items.Add("Offin");
            TimeTableBoss1DropBox.Items.Add("Garmoth");
            TimeTableBoss1DropBox.Items.Add("Quint");
            TimeTableBoss1DropBox.Items.Add("Muraka");
            TimeTableBoss1DropBox.Items.Add("Vell");

            TimeTableBoss2DropBox.Items.Clear();
            TimeTableBoss2DropBox.Items.Add("None");
            TimeTableBoss2DropBox.Items.Add("Kzarka");
            TimeTableBoss2DropBox.Items.Add("Nouver");
            TimeTableBoss2DropBox.Items.Add("Karanda");
            TimeTableBoss2DropBox.Items.Add("Kutum");
            TimeTableBoss2DropBox.Items.Add("Offin");
            TimeTableBoss2DropBox.Items.Add("Garmoth");
            TimeTableBoss2DropBox.Items.Add("Quint");
            TimeTableBoss2DropBox.Items.Add("Muraka");
            TimeTableBoss2DropBox.Items.Add("Vell");

            TimeColumnHoursDropBox.Items.Clear();
            TimeColumnMinutesDropBox.Items.Clear();
            int HV = 0;
           while(true)
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

            RegionComboBox.Text = Settings.Default["DefaultRegion"].ToString().ToUpper();
            if(RegionComboBox.Text == "")
            { RegionComboBox.Text = "EU"; }
            int rsi = RegionComboBox.SelectedIndex;
            int UtcValue = 0;
            switch (rsi)
            {
                case 0:
                    UtcValue = +9; 
                    break;
                case 1:
                    UtcValue = +2;
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
                    UtcValue = -6;
                    break;
            }
            DateTime thisDate = DateTime.UtcNow.AddHours(UtcValue);
            Console.WriteLine(thisDate.DayOfWeek.ToString() + "//"+thisDate.ToString(@"hh\:mm\:ss"));
            TimeTable.Clear();
            TimeTable.Columns.Add("");
            string timezone = "Utc " + UtcValue;
            if(!UtcValue.ToString().Contains("-"))
            { timezone = "Utc +" + UtcValue; }
            TimeTable.Rows.Add(new object[] { timezone});
            TimeTable.Rows.Add(new object[] { DayOfWeek.Monday}); 
            TimeTable.Rows.Add(new object[] { DayOfWeek.Tuesday}); 
            TimeTable.Rows.Add(new object[] { DayOfWeek.Wednesday}); 
            TimeTable.Rows.Add(new object[] { DayOfWeek.Thursday}); 
            TimeTable.Rows.Add(new object[] { DayOfWeek.Friday}); 
            TimeTable.Rows.Add(new object[] { DayOfWeek.Saturday }); 
            TimeTable.Rows.Add(new object[] { DayOfWeek.Sunday});

            string RegionTimeTable = GetStrBetweenTags(timetable, "[" + RegionComboBox.Text + "]", "[/" + RegionComboBox.Text + "]");
            string[] TimeTableRows = RegionTimeTable.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int rx = 0;
            foreach(string row in TimeTableRows)
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
                        if(celloutput.Contains(","))
                        { celloutput = celloutput.Replace(",", "," + Environment.NewLine); }
                        itdr[cx] = celloutput;
                        cx++;
                    }
                    rx++;
                }
            }
            DataRow dr = TimeTable.Rows[0];
            string input = dr[1].ToString();
            if (dr[1].ToString() == "")
            {
                TimeColumnHoursDropBox.Text = "00";
                TimeColumnMinutesDropBox.Text = "00";
            }
            else
            {
                string Hours = input.Substring(0, input.IndexOf(":") + 0);
                string Minutes = input.Substring(input.IndexOf(":") + 1);
                TimeColumnHoursDropBox.Text = Hours;
                TimeColumnMinutesDropBox.Text = Minutes;
            }
            //MessageBox.Show(thisDate.ToString(@"HH\:mm"));
            DateTime dt = thisDate;
            List<DateTime> times = times = new List<DateTime>();
            int cx1 = 0;
            foreach(var cl in TimeTable.Columns)
            {
                if (cx1 > 0)
                {
                    DataRow itdr = TimeTable.Rows[0];
                    string cell = itdr[cx1].ToString();
                    DateTime dt1 = DateTime.Parse(cell);
                    times.Add(dt1);
                }
                cx1++;
            }
            var closestTime = (from t in times orderby (t - dt).Duration() select t).First();
            cx1 = 0;
            int CbC = 1;
            foreach (var cl in TimeTable.Columns)
            {
                if (cx1 > 0)
                {
                    DataRow itdr = TimeTable.Rows[0];
                    string cell = itdr[cx1].ToString();
                    if(closestTime.ToString(@"HH\:mm") == cell)
                    { CbC = cx1; }
                }
                cx1++;
            }
            DateTime ct = DateTime.Parse(closestTime.ToString(@"HH\:mm"));
            if(ct < dt)
            { CbC++; }           
            int cr = 0;
            int CbR = 1;         
            foreach(var ra in TimeTable.Rows)
            {
                if(cr > 0)
                {
                    DataRow itdr = TimeTable.Rows[cr];
                    if(dt.DayOfWeek.ToString() == itdr[0].ToString())
                    { CbR = cr; }                    
                }
                cr++;
            }
            if (CbC == TimeTable.Columns.Count - 1)
            { CbC = 1; CbR++; }
            //filter
            string cSecondboss = "";
            DataRow BN = TimeTable.Rows[CbR];
            string CBN = BN[CbC].ToString();
            if(CBN == "")
            {
                while(true)
                {
                    BN = TimeTable.Rows[CbR];
                    CBN = BN[CbC].ToString();

                    if (CBN != "")
                    { break; }
                    if(CbC == TimeTable.Columns.Count-1)
                    {
                        CbC = 0;
                        CbR++;
                        if(CbR >= 8)
                        { CbR = 1; }
                    }
                    CbC++;
                }
            }
            if(CBN.Contains(","))
            {
                CBN = CBN.Replace("," + Environment.NewLine, " & ");
                cSecondboss = CBN.Substring(CBN.IndexOf(" & ") + 3);
            }
            CbossNameLabel.Content = CBN;
            //cb timespan
            DataRow rfcb = TimeTable.Rows[0];
            DateTime cbts = DateTime.Parse(rfcb[CbC].ToString()+":00");
            TimeSpan Cduration = DateTime.Parse(cbts.ToString(@"hh\:mm\:ss")).Subtract(DateTime.Parse(dt.ToString(@"hh\:mm\:ss")));
            CurrentBossTimeLabel.Content = Cduration.ToString(@"hh\:mm\:ss");
            //MessageBox.Show(cbtf.ToString(@"hh\:mm\:ss"));
            int PCbc = CbC - 1;
            int PCbR = CbR;
            if(PCbc == 0)
            {
                PCbR = PCbR - 1;
                BN = TimeTable.Rows[PCbR];
                PCbc = TimeTable.Columns.Count-1;
            }
            string PBN = BN[PCbc].ToString();
            if(PBN == "")
            {             
                while(true)
                {
                    BN = TimeTable.Rows[PCbR];
                    PBN = BN[PCbc].ToString();

                    if (PBN != "")
                    { break; }
                    if (PCbc == 1)
                    {
                        PCbc = TimeTable.Columns.Count;
                        PCbR--;
                        if (PCbR <= 0)
                        { PCbR = 7; }
                    }
                    PCbc--;
                }
            }    
            if (PBN.Contains(","))
            { PBN = PBN.Replace("," + Environment.NewLine, " & "); }
            PbossNamLabel.Content = PBN;

            DateTime Pcbts = DateTime.Parse(rfcb[PCbc].ToString() + ":00");
            //TimeSpan duration = DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime));
            TimeSpan duration = DateTime.Parse(Pcbts.ToString(@"hh\:mm\:ss")).Subtract(DateTime.Parse(dt.ToString(@"hh\:mm\:ss")));
            PreviousBossTimeLabel.Content = duration.ToString(@"hh\:mm\:ss");

            int NCbc = CbC + 1;
            int NCbR = CbR;
            if (NCbc == TimeTable.Columns.Count)
            {
                NCbR = NCbR + 1;
                BN = TimeTable.Rows[NCbR];
                NCbc = 1;
            }
            BN = TimeTable.Rows[NCbR];
            string NBN = BN[NCbc].ToString();
            if(NBN == "")
            {
                while(true)
                {
                    BN = TimeTable.Rows[NCbR];
                    NBN = BN[NCbc].ToString();
                    if (NBN != "")
                    { break; }
                    if(NCbc == TimeTable.Columns.Count -1)
                    {
                        NCbc = 0;
                        NCbR++;
                        if (NCbR >= 8)
                        { NCbR = 1; }
                    }
                    NCbc++;
                }
            }
            if (NBN.Contains(","))
            { NBN = NBN.Replace("," + Environment.NewLine, " & "); }
            NBossNameLabel.Content = NBN;
            DateTime Ncbts = DateTime.Parse(rfcb[NCbc].ToString() + ":00");
            //TimeSpan duration = DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime));
            TimeSpan Nduration = DateTime.Parse(Ncbts.ToString(@"hh\:mm\:ss")).Subtract(DateTime.Parse(dt.ToString(@"hh\:mm\:ss")));
            NextBossTimeLabel.Content = Nduration.ToString(@"hh\:mm\:ss");


            //filter
            try
            {
                BossesCollection.Clear();
                BossCollectionListBox.Items.Clear();
            }
            catch (Exception) { }
            string BossesList = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/Bosses");
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

            PBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Boss.png"));
            NBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Boss.png"));
            LBImageBox.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/Boss.png"));
            Getimg gm = new Getimg();

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
                    { try { PBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/" + PbossNamLabel.Content.ToString() + ".png"); } catch (Exception) { } }
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
                        { try { NBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/" + CbossNameLabel.Content.ToString() + ".png"); } catch (Exception) { } }
                        //if (bossdata[3].ToString() != "")
                        //{ CbossNameLabel.Content = bossdata[3].ToString(); }
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
                    { try { LBImageBox.Source = gm.GETIMAGE(System.IO.Directory.GetCurrentDirectory() + "/Resources/" + NBossNameLabel.Content.ToString() + ".png"); } catch (Exception) { } }
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
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        currentbossrole1 = bossdata[4].ToString();
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
                    if (name.Contains(bossdata[0].ToString()))
                    {
                        currentbossrole2 = bossdata[4].ToString();
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
                            currentbossrole1 = bossdata[4].ToString();
                            CbossNameLabel.Content = bossdata[3].ToString();
                        }
                        else
                        {
                            currentbossrole1 = bossdata[4].ToString();
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



            TimeTable.AcceptChanges();
            gridview1.ItemsSource = TimeTable.DefaultView;
            timer1.Start();
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
                Console.WriteLine(gridview1Row);
                string input = dr[gridview1Column].ToString();
                string Hours = input.Substring(0, input.IndexOf(":") + 0);
                string Minutes = input.Substring(input.IndexOf(":") + 1);
                TimeColumnHoursDropBox.Text = Hours;
                TimeColumnMinutesDropBox.Text = Minutes;
                //TimtableCellTextbox.Text = cellValue;
                //TimtableCellTextbox.Text = "Column: " + gridview1.CurrentCell.Column.DisplayIndex.ToString() + "Row: " + gridview1.SelectedIndex.ToString();
                DataRow dr1 = TimeTable.Rows[gridview1Row];
                TimeTableBoss1DropBox.Text = "None";
                TimeTableBoss2DropBox.Text = "None";
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
            DataRow dr = TimeTable.Rows[0];
            dr[gridview1Column] = TimeColumnHoursDropBox.Text + ":" + TimeColumnMinutesDropBox.Text;
        }
        private void TimeColumnMinutesDropBox_DropDownClosed(object sender, EventArgs e)
        {
            DataRow dr = TimeTable.Rows[0];
            dr[gridview1Column] = TimeColumnHoursDropBox.Text + ":" + TimeColumnMinutesDropBox.Text;
        }        
        private void TimeTableAddColumnButton_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = TimeTable.Rows[0];
            TimeTable.Columns.Add("");
            gridview1Column = TimeTable.Columns.Count - 1;
            dr[gridview1Column] = "00:00";
            gridview1.ItemsSource = null;
            TimeTable.AcceptChanges();
            gridview1.ItemsSource = TimeTable.DefaultView;
            TimeColumnHoursDropBox.Text = "00";
            TimeColumnMinutesDropBox.Text = "00";
            gridview1.SelectedIndex = 0;
        }

        private void TimeTableBoss1DropBox_DropDownClosed(object sender, EventArgs e)
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

        private void TimeTableBoss2DropBox_DropDownClosed(object sender, EventArgs e)
        {
            if(TimeTableBoss1DropBox.Text == "None" && TimeTableBoss2DropBox.Text != "None")
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
}

