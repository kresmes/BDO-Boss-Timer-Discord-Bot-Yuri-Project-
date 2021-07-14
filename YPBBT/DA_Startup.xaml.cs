using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using System.Diagnostics;
using System.IO;
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

namespace YPBBT
{
    /// <summary>
    /// Interaction logic for DA_Startup.xaml
    /// </summary>
    public partial class DA_Startup : Window
    {
        DiscordSocketClient discord = new DiscordSocketClient();/// Discord_Client
        List<string> LanguageLinesCollection = new List<string>();/// Words List used for translation
        List<string> ServersCollection = new List<string>();/// List of Server ID's Collected using Discord_Client
        List<string> ChannelsCollection = new List<string>();/// List of Channel ID's Collected using Discord_Client
        List<string> AlertChannelsCollection = new List<string>();
        string AppVersion = "3.13b";/// Current App Version
        string Default_Language;/// String for Default Selected Langauge
        bool Profile_Setup_IsCompleted = false;///make sure if the setup if completed so when its closed it opens MainWindow
        public DA_Startup()
        {
            InitializeComponent();
            AppVersionLabel0.Content = "YPBBT " + AppVersion;
            AppVersionLabel1.Content = "YPBBT " + AppVersion;
            bool App_isalready_opend = false;
            this.Visibility = Visibility.Hidden;
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)///Check if the application is already opend
            {
                if(File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Profile"))
                {
                    string lines = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Profile");//read Profile File
                    string[] RefferalsFilter = lines.Split('|');
                    Load_Language_File(RefferalsFilter[0].ToString());
                }
                else
                {Load_Language_File();}
                App_isalready_opend = true;
                this.Visibility = Visibility.Hidden;
                ErrorMessageBox emb = new ErrorMessageBox(LanguageLinesCollection[23].ToString(), LanguageLinesCollection[24].ToString(), LanguageLinesCollection[25].ToString(), LanguageLinesCollection[14].ToString());
                emb.MB_typeYN_DA_Startup(LanguageLinesCollection[21].ToString(), LanguageLinesCollection[22].ToString(), 1, this);
                emb.Show();         
            }
            Run_Profile_Setup(App_isalready_opend);
        }
        public void Run_Profile_Setup(bool App_isalready_opend)///check if profile already been created if not start Profile Setup 
        {
            if (App_isalready_opend == false)
            {
                if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Resources/DA_Profile"))
                {
                    DA_MainWindow ma = new DA_MainWindow(0);
                    ma.Show();
                    this.Close();
                }
                else
                {
                    this.Visibility = Visibility.Visible;
                    CIDGRID.Visibility = Visibility.Hidden;
                    AlertCIDGRID.Visibility = Visibility.Hidden;
                    Step5NextButton.Visibility = Visibility.Hidden;
                    System.Threading.Thread.Sleep(2000);
                    tab0.Visibility = Visibility.Hidden;
                    tab1.Visibility = Visibility.Hidden;
                    tab2.Visibility = Visibility.Hidden;
                    tab3.Visibility = Visibility.Hidden;
                    tab4.Visibility = Visibility.Hidden;
                    tab5.Visibility = Visibility.Hidden;
                    tab6.Visibility = Visibility.Hidden;
                    tab7.Visibility = Visibility.Hidden;
                    mediaElement.LoadedBehavior = MediaState.Play;
                    mediaElement.Source = new Uri(System.IO.Directory.GetCurrentDirectory() + @"\Resources\img\SA_bckg.mp4", UriKind.Absolute);
                    Step1_2MediaElement.Stop();
                    Step2MediaElement.Stop();
                    Step3MediaElement.Stop();
                    Step4MediaElement.Stop();
                    Tabcontrol1.SelectedIndex = 7;
                }
            }
        }
        private void Load_Language_File(string SL = "en")///Load Language Files
        {
            Step1_2MediaElement.Stop();
            Step2MediaElement.Stop();
            Step3MediaElement.Stop();
            Step4MediaElement.Stop();
            Default_Language = SL;
            try { LanguageLinesCollection.Clear(); } catch (Exception) { }
            string lines = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Resources/Languages/" + SL);
            string[] RefferalsFilter = lines.Split('}');
            foreach (var rf in RefferalsFilter)
            {
                LanguageLinesCollection.Add(rf);
            }           
            S0Label0.Text = LanguageLinesCollection[0].ToString();
            Tc0SkipButton.Content = LanguageLinesCollection[1].ToString();// skip
            Tc0NextButton.Content = LanguageLinesCollection[2].ToString(); // Next
            S1L0.Content = LanguageLinesCollection[3].ToString() + " 1";//Step
            S1L1.Text = LanguageLinesCollection[5].ToString();
            Step1_OpenDiscordDevUrl_Button.ToolTip = LanguageLinesCollection[6].ToString();
            Step1_2Label.Text = LanguageLinesCollection[7].ToString();
            Step1NextButton.Content = LanguageLinesCollection[2].ToString();
            S2L0.Content = LanguageLinesCollection[3].ToString() + " 2";//Step
            //LanguageLinesCollection[4].ToString() is Empty
            S2L1.Text = LanguageLinesCollection[8].ToString();
            Step2NextButton.Content = LanguageLinesCollection[2].ToString(); // Next
            S3L0.Content = LanguageLinesCollection[3].ToString() + " 3";//Step
            S3L1.Text = LanguageLinesCollection[9].ToString();
            ClientIDLABEL.Content = LanguageLinesCollection[10].ToString();
            Step3CIDTextbox.Text = LanguageLinesCollection[10].ToString().Remove(LanguageLinesCollection[10].Length - 2, 1);
            Step3InviteBotButton.Content = LanguageLinesCollection[11].ToString();//Invite
            Step3NextButton.Content = LanguageLinesCollection[2].ToString(); // Next
            S4L0.Content = LanguageLinesCollection[3].ToString() + " 4";//Step
            S4L1.Text = LanguageLinesCollection[12].ToString();
            TokenLabel.Content = LanguageLinesCollection[13].ToString();//Token:
            Step4TokenTextbox.Text = LanguageLinesCollection[13].ToString().Remove(LanguageLinesCollection[13].Length - 2, 1);
            Step4TestTokenButton.Content = LanguageLinesCollection[14].ToString();//Test Token
            Step4NextButton.Content = LanguageLinesCollection[2].ToString(); // Next
            S5L0.Content = LanguageLinesCollection[3].ToString() + " 5";//Step
            S5L1.Text = LanguageLinesCollection[15].ToString();
            ServerIDLAbel.Content = LanguageLinesCollection[16].ToString();
            ChannelIDLabel.Content = LanguageLinesCollection[17].ToString();
            S5L2.Text = LanguageLinesCollection[18].ToString();
            Step5NextButton.Content = LanguageLinesCollection[2].ToString(); // Next
            S6L0.Content = LanguageLinesCollection[19].ToString();
            S6L1.Text = LanguageLinesCollection[20].ToString();
            //LanguageLinesCollection[21].ToString() is "WARNING"
            //LanguageLinesCollection[22].ToString() is "The Application is already running in the background do you want to close it before continuing?"
            //LanguageLinesCollection[23].ToString() is "Ok"
            //LanguageLinesCollection[24].ToString() is "No"
            //LanguageLinesCollection[25].ToString() is "Yes"
            AlertChannelIDLabel.Content = LanguageLinesCollection[26].ToString(); //Alert Channel ID:
            //LanguageLinesCollection[27].ToString() is > **Welcome To Yuri Project Bdo Boss Timer Self Rolling Settings**
            //LanguageLinesCollection[28].ToString() is > **U can customize all the settings on the self rolling menu**


            #region"animate Selected Language Tab"
            selctedlangImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resources/img/" + SL + ".png"));
            var left = Tabcontrol1.Margin.Left;
            TranslateTransform trans = new TranslateTransform();
            Tabcontrol1.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(left, 1000 + left, TimeSpan.FromSeconds(1));
            anim1.AutoReverse = true;
            trans.BeginAnimation(TranslateTransform.XProperty, anim1);

            DoubleAnimation da = new DoubleAnimation();
            da.From = 1;
            da.To = 0;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            da.Completed += (s, _) => {
                Tabcontrol1.SelectedIndex = 0;
            };
            Tabcontrol1.BeginAnimation(OpacityProperty, da);

            DoubleAnimation dar = new DoubleAnimation();
            dar.Duration = new Duration(TimeSpan.FromSeconds(2));
            dar.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, dar);
            #endregion
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)///Move Window by clicking and holding anywhere on it {DragMove()}
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
            catch (Exception) { }
        }
        private void Tc0SkipButton_Click(object sender, RoutedEventArgs e)///Skip Bot Creation Steps
        {
            Step3NextButton.Visibility = Visibility.Hidden;
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 3;
        }
        private void Tc0NextButton_Click(object sender, RoutedEventArgs e)///Tabcontrol0 NextButton Function
        {
            Step1_2Label.Visibility = Visibility.Hidden;
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
        private void Step1_OpenDiscordDevUrl_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discordapp.com/developers/applications/");
            Step1_2Label.Visibility = Visibility.Visible;
            Step1NextButton.Visibility = Visibility.Visible;
            Step1_2MediaElement.Visibility = Visibility.Visible;
            Step1_2MediaElement.Position = new TimeSpan(0, 0, 0);
            Step1_2MediaElement.Play();
        }
        private void Step1_2MediaElement_MediaEnded(object sender, RoutedEventArgs e)///Replay When Video Ended
        { Step1_2MediaElement.Position = new TimeSpan(0, 0, 0); Step1_2MediaElement.Play();}    
        private void Step1_2MediaElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)///Open Video On Click
        {System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/img/Step1.2.mp4");}            
        private void Step1NextButton_Click(object sender, RoutedEventArgs e)///Step 1 Tabcontrol1 Next_Button Function
        {
            Step2MediaElement.Stop();
            Step1_2Label.Visibility = Visibility.Hidden;
            Step1_2MediaElement.Visibility = Visibility.Hidden;
            Step2MediaElement.Position = new TimeSpan(0, 0, 0);
            Step2MediaElement.Play();
            Tabcontrol1.Opacity = 0;
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 2;
        }       

        private void Step2MediaElement_MediaEnded(object sender, RoutedEventArgs e)///Repeat the video if ended
        {Step2MediaElement.Position = new TimeSpan(0, 0, 0);Step2MediaElement.Play();}     
        private void Step2MediaElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)///Step 2 Creating a bot video Load
        {System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/img/Step2MakeBot.mp4");}                  
        private void Step2NextButton_Click(object sender, RoutedEventArgs e)///Step 2 Tabcontrol 2 Next_button Function
        {
            Step2MediaElement.Stop();
            Step3NextButton.Visibility = Visibility.Hidden;
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 3;
        }         
       
        private void Step3MediaElement_MediaEnded(object sender, RoutedEventArgs e)///Repeat When video is Ended
        { Step3MediaElement.Position = new TimeSpan(0, 0, 0); Step3MediaElement.Play(); }
        private void Step3CIDTextbox_GotFocus(object sender, RoutedEventArgs e)///Client ID TextBox Empty Textbox on focus Event
        { if (Step3CIDTextbox.Text == LanguageLinesCollection[10].ToString().Remove(LanguageLinesCollection[10].Length - 2, 1)) { Step3CIDTextbox.Text = ""; } }
        private void Step3InviteBotButton_Click(object sender, RoutedEventArgs e)///Check if the Client ID textBox is Valid ID and Invite The Bot To Discord Server
        {
            var bc = new BrushConverter();
            Step3CIDTextbox.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            Regex regex = new Regex("[^0-9-]+");///Regex Parameters for Numbers Only

            foreach (char c in Step3CIDTextbox.Text.ToString())//check for every character in Client ID TextBox.text making sure that all are numbers only if one of the characters are not then empty the textbox
            { if (regex.IsMatch(c.ToString()) == true) { Step3CIDTextbox.Text = ""; } }

            if (Step3CIDTextbox.Text == LanguageLinesCollection[10].ToString().Remove(LanguageLinesCollection[10].Length - 2, 1))
            { Step3CIDTextbox.Text = ""; }

            if (Step3CIDTextbox.Text != "")
            { System.Diagnostics.Process.Start("https://discordapp.com/oauth2/authorize?&client_id=" + Step3CIDTextbox.Text + "&scope=bot&permissions=8"); Step3NextButton.Visibility = Visibility.Visible; }
            else
            { Step3CIDTextbox.BorderBrush = Brushes.Red; }
        }
        private void Step3MediaElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)///Open Video On click
        { System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/img/Step2CID.mp4"); }
        private void Step3NextButton_Click(object sender, RoutedEventArgs e)///Step 3 Tabcontrol 3 Next_button Event
        {
            Step4NextButton.Visibility = Visibility.Hidden;
            Step3MediaElement.Position = new TimeSpan(0, 0, 0);
            Step3MediaElement.Stop();
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 4;
        }

        private void Step4TokenTextbox_GotFocus(object sender, RoutedEventArgs e)///Empty Token Textbox On focus if Text = Token
        { if (Step4TokenTextbox.Text == LanguageLinesCollection[13].ToString().Remove(LanguageLinesCollection[13].Length - 2, 1)){ Step4TokenTextbox.Text = ""; } }
        #region"Token Button Background Color Changes On mouse Enter/Leave"
        private void Step4TestTokenButton_MouseEnter(object sender, MouseEventArgs e)
        {var bc = new BrushConverter();Step4TestTokenButton.Background = (Brush)bc.ConvertFrom("#818182");}
        private void Step4TestTokenButton_MouseLeave(object sender, MouseEventArgs e)
        { var bc = new BrushConverter();Step4TestTokenButton.Background = (Brush)bc.ConvertFrom("#FFA9A9AA"); }
        #endregion
        private async void Step4TestTokenButton_Click(object sender, RoutedEventArgs e)///Test Token Using Catch Exception as triger for Failing to Connect
        {
            try
            {
                SidCombobox.Items.Clear();
                ServersCollection.Clear();
                
                await discord.LoginAsync(TokenType.Bot, Step4TokenTextbox.Text);
                await discord.StartAsync();
              
                Step4TokenTextbox.BorderBrush = Brushes.Green;
                Step4TestTokenButton.Background = Brushes.Green;
                Step4TestTokenButton.IsEnabled = false;
            }
            catch (Exception)
            {
                Step4TokenTextbox.BorderBrush = Brushes.Red;
                Step4TestTokenButton.Background = Brushes.Red;
            }

            if (Step4TokenTextbox.BorderBrush == Brushes.Green)
            { Step4NextButton.Visibility = Visibility.Visible; }
        } 
        private void Step4MediaElement_MediaEnded(object sender, RoutedEventArgs e)///Repeat Video if Ended
        {Step4MediaElement.Position = new TimeSpan(0, 0, 0); Step4MediaElement.Play();}
        private void Step4MediaElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)///Open Video On click
        {System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/Resources/img/Step4GetToken.mp4");}      
        private void Step4NextButton_Click(object sender, RoutedEventArgs e)///Step 4 Tabcontrol 4 Next_button Event
        {
            var servers = discord.Guilds;
            foreach (var server in servers)///Get Server Id's And Names and add them to servercollection list
            {SidCombobox.Items.Add(server.ToString());ServersCollection.Add(server.Id.ToString());}
            Step4MediaElement.Position = new TimeSpan(0, 0, 0);
            Step4MediaElement.Stop();
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 5;
        }
        private void SidCombobox_DropDownClosed(object sender, EventArgs e)///After Server is selected from Combobox Get Channel names and id's and add them to channels collection and Cidcombobox
        {          
            if (SidCombobox.SelectedIndex > -1)
            {
                CIDGRID.Visibility = Visibility.Visible;
                CidCombobox.Items.Clear();
                AlertCidCombobox.Items.Clear();
                ChannelsCollection.Clear();
                AlertChannelsCollection.Clear();

                ulong sid = ulong.Parse(ServersCollection[SidCombobox.SelectedIndex].ToString());
                var guild = discord.GetGuild(sid);
                var channels = guild.TextChannels;
                foreach (var channel in channels)
                { CidCombobox.Items.Add(channel.ToString()); ChannelsCollection.Add(channel.Id.ToString()); AlertCidCombobox.Items.Add(channel.ToString()); AlertChannelsCollection.Add(channel.Id.ToString());}
            }
        }
        private void CidCombobox_DropDownClosed(object sender, EventArgs e)
        {
            if(CidCombobox.SelectedIndex > -1)
            {
                AlertCidCombobox.Items.Clear();
                AlertChannelsCollection.Clear();
                int index = 0;
                foreach(var channel in ChannelsCollection)
                { if (index != CidCombobox.SelectedIndex) { AlertChannelsCollection.Add(channel); } index++; }
                index = 0;
                foreach (var channel in CidCombobox.Items)
                { if (index != CidCombobox.SelectedIndex) { AlertCidCombobox.Items.Add(channel.ToString()); } index++; }
                AlertCIDGRID.Visibility = Visibility.Visible;
            }
        }
        private void CidCombobox_DropDownOpened(object sender, EventArgs e)
        {
            CidCombobox.Items.Clear();
            AlertCidCombobox.Items.Clear();
            ChannelsCollection.Clear();
            AlertChannelsCollection.Clear();

            ulong sid = ulong.Parse(ServersCollection[SidCombobox.SelectedIndex].ToString());
            var guild = discord.GetGuild(sid);
            var channels = guild.TextChannels;
            foreach (var channel in channels)
            { CidCombobox.Items.Add(channel.ToString()); ChannelsCollection.Add(channel.Id.ToString()); AlertCidCombobox.Items.Add(channel.ToString()); AlertChannelsCollection.Add(channel.Id.ToString()); }
        }
        private void AlertCidCombobox_DropDownClosed(object sender, EventArgs e)
        {
            if (AlertCidCombobox.SelectedIndex > -1) 
            { Step5NextButton.Visibility = Visibility.Visible; }
        }
        private void AlertCidCombobox_DropDownOpened(object sender, EventArgs e)
        {
            AlertCidCombobox.Items.Clear();
            AlertChannelsCollection.Clear();

            ulong sid = ulong.Parse(ServersCollection[SidCombobox.SelectedIndex].ToString());
            var guild = discord.GetGuild(sid);
            var channels = guild.TextChannels;
            ulong MainChannel_ID = ulong.Parse(ChannelsCollection[CidCombobox.SelectedIndex]);
            foreach (var channel in channels)
            {
                if (channel.Id != MainChannel_ID)
                { AlertCidCombobox.Items.Add(channel.ToString()); AlertChannelsCollection.Add(channel.Id.ToString()); }
            }          
        }
        private void Step5NextButton_Click(object sender, RoutedEventArgs e)///Step5 Tabcontrol 5 NextButton function Save Data Profile
        {
            string data = Default_Language + "|" 
                + Step3CIDTextbox.Text + "|" 
                + Step4TokenTextbox.Text 
                + "|" + SidCombobox.Text + "{" + ServersCollection[SidCombobox.SelectedIndex].ToString() 
                + "|"+ CidCombobox.Text + "{" + ChannelsCollection[CidCombobox.SelectedIndex].ToString()
                + "|" + "@everyone{@everyone|" 
                + "@everyone{@everyone|" 
                + "@everyone{@everyone" 
                + "|2|30|" 
                + "@everyone{@everyone|" 
                + "@everyone{@everyone"
                + "|" + AlertCidCombobox.Text + "{" + AlertChannelsCollection[AlertCidCombobox.SelectedIndex].ToString();
            File.WriteAllText(Directory.GetCurrentDirectory() + "/Resources/DA_Profile", data);
            File.WriteAllText(Directory.GetCurrentDirectory() + "/Resources/DA_Bosses", File.ReadAllText(Directory.GetCurrentDirectory() + "/Resources/BossesOrigin"));
            File.WriteAllText(Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT", File.ReadAllText(Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT_Origin"));
            string SelfRolling = "[Channel]{[/Channel]"
                               + Environment.NewLine + "[StartMessage]"+ LanguageLinesCollection[27].ToString() + "[/StartMessage]"
                               + Environment.NewLine + "[MessageRoles][/MessageRoles]"
                               + Environment.NewLine + "[EndMessage]"+ LanguageLinesCollection[28].ToString() + "[/EndMessage]"
                               + Environment.NewLine + "[MainMessageID][/MainMessageID]";
            File.WriteAllText(Directory.GetCurrentDirectory() + "/Resources/SelfRolling", SelfRolling);

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 6;
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)///Join Discord Url
        {System.Diagnostics.Process.Start("https://discord.gg/8SCcCJq");}
        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)///Close Profile Startup and open main app 
        { Profile_Setup_IsCompleted = true; this.Close();}

        private void animate_Language_Label(string language_text)///animate language buttons hover language Transition
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.AutoReverse = false;
            SetLanguageLabel.BeginAnimation(OpacityProperty, da);
            SetLanguageLabel.Content = language_text;
        }
        #region"Language Buttons Mouse Enter Enevnts"
        private void frButton_MouseEnter(object sender, MouseEventArgs e)
        {animate_Language_Label("Choisir la langue");}
        private void enButton_MouseEnter(object sender, MouseEventArgs e)
        {animate_Language_Label("Select Language");}
        private void esButton_MouseEnter(object sender, MouseEventArgs e)
        {animate_Language_Label("Seleccione el idioma");}
        private void ruButton_MouseEnter(object sender, MouseEventArgs e)
        {animate_Language_Label("выбрать язык");}
        private void jpButton_MouseEnter(object sender, MouseEventArgs e)
        {animate_Language_Label("言語を選択する");}
        private void krButton_MouseEnter(object sender, MouseEventArgs e)
        {animate_Language_Label("언어를 선택하십시오");}
        #endregion
        #region"Load Clicked Language click_Event"
        private void frButton_Click(object sender, RoutedEventArgs e)
        {Load_Language_File("fr");}

        private void enButton_Click(object sender, RoutedEventArgs e)
        {Load_Language_File("en");}

        private void esButton_Click(object sender, RoutedEventArgs e)
        {Load_Language_File("es");}

        private void ruButton_Click(object sender, RoutedEventArgs e)
        {Load_Language_File("ru");}

        private void jpButton_Click(object sender, RoutedEventArgs e)
        {Load_Language_File("jp");}

        private void krButton_Click(object sender, RoutedEventArgs e)
        {Load_Language_File("kr");}
        #endregion
        private void SelectedlangButton_Click(object sender, RoutedEventArgs e)///Select Other Langauge Button
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            da.AutoReverse = false;
            Tabcontrol1.BeginAnimation(OpacityProperty, da);
            Tabcontrol1.SelectedIndex = 7;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)///Event on closing animate then close 
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(1));
            anim.Completed += (s, _) => Excute();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }
        private async void Excute()///excute Type of closing
        {
            if (Profile_Setup_IsCompleted == true)
            {
                await discord.StopAsync();
                DA_MainWindow ma = new DA_MainWindow(1);
                ma.Show();
            }
            this.Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)///Insures only inputed numbers on textbox
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)///repat video if ended
        {mediaElement.Position = new TimeSpan(0, 0, 0); mediaElement.Play();}

        private void Step4TokenTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            Step4TestTokenButton.IsEnabled = true;
        }

       
    }
}