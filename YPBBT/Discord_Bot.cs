using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace YPBBT
{
    public class Discord_Bot
    {
        DA_MainWindow Public_MainWindow;
        public async void GetDiscordData(DA_MainWindow MainWindow)
        {
            Public_MainWindow = MainWindow;
            try
            {
                var bc = new BrushConverter();
                Application.Current.Dispatcher.Invoke((Action)(() => {
                    Public_MainWindow.DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFF1F1F1");
                    Public_MainWindow.DiscordBotConnectionStatusLabel.Content = Public_MainWindow.LanguageCollection[6].ToString();//"Connecting...";
                    if(Public_MainWindow.discord.ConnectionState == ConnectionState.Connected)
                    { Public_MainWindow.Processing_Status(false, "Reconnecting Bot..."); }
                    else
                    { Public_MainWindow.Processing_Status(false, "Connecting Bot..."); }                    
                }));
                await Public_MainWindow.discord.LoginAsync(TokenType.Bot, Public_MainWindow.Token);
                await Public_MainWindow.discord.StartAsync();
                System.Threading.Thread.Sleep(2000);

                Application.Current.Dispatcher.Invoke((Action)(() => {
                    if (Public_MainWindow.DiscordBotConnectionStatusLabel.Content.ToString() == Public_MainWindow.LanguageCollection[6].ToString())
                    {
                        Public_MainWindow.DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FF669174");
                        Public_MainWindow.DiscordBotConnectionStatusLabel.Content = Public_MainWindow.LanguageCollection[8].ToString();//"Connected";
                        Public_MainWindow.Processing_Status(true, "Bot is Connected.");
                    }                  
                }));
               
            }
            catch (Exception s)
            {
                if (s is Discord.Net.HttpException)
                {
                    await Public_MainWindow.discord.StopAsync();
                    Application.Current.Dispatcher.Invoke((Action)(() => {
                        ErrorMessageBox emb = new ErrorMessageBox(Public_MainWindow.LanguageCollection[119].ToString(), Public_MainWindow.LanguageCollection[120].ToString(), Public_MainWindow.LanguageCollection[121].ToString(), Public_MainWindow.LanguageCollection[122].ToString());
                        emb.MB_typeOK(Public_MainWindow.LanguageCollection[108].ToString(), s.Message + Environment.NewLine + Environment.NewLine + Public_MainWindow.LanguageCollection[109].ToString(), Public_MainWindow);
                        emb.Show();
                        Public_MainWindow.IsEnabled = false;
                    }));
                }
                var bc = new BrushConverter();
                Application.Current.Dispatcher.Invoke((Action)(() => {
                    Public_MainWindow.DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFBB3D3D");
                    Public_MainWindow.DiscordBotConnectionStatusLabel.Content = Public_MainWindow.LanguageCollection[7].ToString();//"Connection ERROR!";
                    Public_MainWindow.Processing_Status(true, "Failed to connect Bot.", true);
                }));
                /*StartPosting();*/
            }
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
        public void StartPosting_Cooldown(DA_MainWindow MainWindow)
        {
            System.Threading.Thread.Sleep(5000);
            Application.Current.Dispatcher.Invoke((Action)(() => {
                MainWindow.StartPostingButton.IsEnabled = true;             
            }));
        }
        public async void StartPosting(DA_MainWindow MainWindow)// purge discord channel and start posting
        {
            Public_MainWindow = MainWindow;
            if (Public_MainWindow.ServerID != 0)
            {
                try
                {
                    var bc = new BrushConverter();
                    try
                    {
                        Application.Current.Dispatcher.Invoke((Action)(() => {
                            Public_MainWindow.DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFF1F1F1");
                            Public_MainWindow.DiscordBotConnectionStatusLabel.Content = Public_MainWindow.LanguageCollection[6].ToString();//"Connecting...";
                        }));
                        await Public_MainWindow.discord.LoginAsync(TokenType.Bot, Public_MainWindow.Token);
                        await Public_MainWindow.discord.StartAsync();
                        System.Threading.Thread.Sleep(1000);
                    }
                    catch (Exception)
                    {
                        Application.Current.Dispatcher.Invoke((Action)(() => {
                            Public_MainWindow.DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFBB3D3D");
                            Public_MainWindow.DiscordBotConnectionStatusLabel.Content = Public_MainWindow.LanguageCollection[7].ToString();//"Connection ERROR!";
                        }));
                    }
                   
                    if (Public_MainWindow.discord.ConnectionState == Discord.ConnectionState.Connected)
                    {
                        var guild = Public_MainWindow.discord.GetGuild(Public_MainWindow.ServerID);
                        SocketTextChannel channel = guild.GetTextChannel(Public_MainWindow.Main_BotChannel_ID);
                       
                        var messages = await channel.GetMessagesAsync(100).FlattenAsync(); //defualt is 100
                        System.Threading.Thread.Sleep(2000);
                        await (channel as SocketTextChannel).DeleteMessagesAsync(messages);
                        var embed1 = new EmbedBuilder();
                        string ANmessage = "";
                        Application.Current.Dispatcher.Invoke((Action)(() => {
                            string[] pbu = Public_MainWindow.publicbossUrl.Split('|');
                            string[] bnu = Public_MainWindow.CbossNameLabel.Content.ToString().Split('&');
                           
                            if (Public_MainWindow.CbossNameLabel.Content.ToString().Contains("&"))
                            {
                                ANmessage = Public_MainWindow.LanguageCollection[123].ToString() + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + Public_MainWindow.LanguageCollection[85].ToString() + Environment.NewLine + "[" + bnu[1] + "](" + pbu[1] + ")" + " <---" + Public_MainWindow.LanguageCollection[85].ToString();
                            }
                            else
                            {
                                ANmessage = Public_MainWindow.LanguageCollection[123].ToString() + Environment.NewLine + "[" + bnu[0] + "](" + pbu[0] + ")" + " <---" + Public_MainWindow.LanguageCollection[85].ToString();
                            }                           
                            embed1 = new EmbedBuilder
                            {
                                Title = Public_MainWindow.CbossNameLabel.Content.ToString()/* + " <---" + LanguageCollection[87].ToString()*/,
                                ImageUrl = Public_MainWindow.publicNbossimage,
                                Color = Discord.Color.LightGrey,
                                //Url = publicbossUrl,
                                Description = ANmessage                               
                            };
                        }));
                        //Emoji emj = new Emoji("<:download1:832276391789199430>");
                        //var s = guild.Emotes;
                        //var exampleFooter = new EmbedFooterBuilder().WithText(
                        //      "Reaction 1" + " 0️⃣ " + Environment.NewLine
                        //    + "Reaction 2" + Environment.NewLine
                        //    + "Reaction 3" + Environment.NewLine);
                        //embed1.WithFooter(exampleFooter);
                        

                        var BossImage = await channel.SendMessageAsync("", false, embed1.Build());
                        var AlertChannel = guild.TextChannels.FirstOrDefault(ch => ch.Id == Public_MainWindow.Alert_BotChannel_ID);
                        var Alert_Embed = new EmbedBuilder
                        {
                            Title = "YPBBT is Connected" /*+ " <---" + LanguageCollection[87].ToString()*/,
                            ThumbnailUrl = "https://raw.githubusercontent.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/master/Resources/Images/Connected.png",                           
                            Color = Discord.Color.LightGrey,
                            //Url = publicbossUrl  
                            Description = "All Notifications Will be Send to this channel"
                        };
                        await AlertChannel.SendMessageAsync("", false, Alert_Embed.Build());
                        Public_MainWindow.bossImageID = BossImage.Id;

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
                        Application.Current.Dispatcher.Invoke((Action)(() => {
                            string nb = "> ```cs" + Environment.NewLine + "> " + Public_MainWindow.CurrentBossTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```";
                            string nti = "> ```cs" + Environment.NewLine + "> " + Public_MainWindow.NightInBdoTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```";
                            string iri = "> ```cs" + Environment.NewLine + "> " + Public_MainWindow.IRTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```";
                            string br = "> ```cs" + Environment.NewLine + "> " + Public_MainWindow.BRTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```";
                            string itr = "> ```cs" + Environment.NewLine + "> " + Public_MainWindow.ITRITimeLabel.Content.ToString() + Environment.NewLine + "> " + "```";
                            if (Public_MainWindow.CurrentBossTimeLabel.Content.ToString().StartsWith("00:"))
                            { nb = "> ```css" + Environment.NewLine + "> " + Public_MainWindow.CurrentBossTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```"; }
                            if (Public_MainWindow.NightInBdoTimeLabel.Content.ToString().StartsWith("00:"))
                            { nti = "> ```css" + Environment.NewLine + "> " + Public_MainWindow.NightInBdoTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```"; }
                            if (Public_MainWindow.IRTimeLabel.Content.ToString().StartsWith("00:"))
                            { iri = "> ```css" + Environment.NewLine + "> " + Public_MainWindow.IRTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```"; }
                            if (Public_MainWindow.BRTimeLabel.Content.ToString().StartsWith("00:"))
                            { br = "> ```css" + Environment.NewLine + "> " + Public_MainWindow.BRTimeLabel.Content.ToString() + Environment.NewLine + "> " + "```"; }
                            if (Public_MainWindow.ITRITimeLabel.Content.ToString().StartsWith("00:"))
                            { itr = "> ```css" + Environment.NewLine + "> " + Public_MainWindow.ITRITimeLabel.Content.ToString() + Environment.NewLine + "> " + "```"; }


                            message = "> " + Public_MainWindow.CbossLabel.Content.ToString() + Environment.NewLine
                                + nb + Environment.NewLine
                                + "> " + Public_MainWindow.NILabel.Content.ToString() + Environment.NewLine
                                + nti + Environment.NewLine
                                + "> " + Public_MainWindow.IRILabel.Content.ToString() + Environment.NewLine
                                + iri + Environment.NewLine
                                + "> " + Public_MainWindow.BRILabel.Content.ToString() + Environment.NewLine
                                + br + Environment.NewLine
                                + "> " + Public_MainWindow.ITRILabel.Content.ToString() + Environment.NewLine
                                + itr;
                        }));
                        Public_MainWindow.MainMessage = await channel.SendMessageAsync(message);
                        Public_MainWindow.MainMessageID = Public_MainWindow.MainMessage.Id;
                        Application.Current.Dispatcher.Invoke((Action)(() => {
                            if (Public_MainWindow.DiscordBotConnectionStatusLabel.Content.ToString() == Public_MainWindow.LanguageCollection[6].ToString())
                            {

                                Public_MainWindow.DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FF669174");
                                Public_MainWindow.DiscordBotConnectionStatusLabel.Content = Public_MainWindow.LanguageCollection[8].ToString();//"Connected";

                            }
                        }));
                        bool DTT_isChecked = false;
                        Application.Current.Dispatcher.Invoke((Action)(() => { DTT_isChecked = Public_MainWindow.DisplayTimeTableSetting.IsChecked.Value; }));

                        if (DTT_isChecked)
                        {
                            var timetablemessage = await channel.SendFileAsync(System.IO.Directory.GetCurrentDirectory() + "/Resources/TimeTable.png", Public_MainWindow.MOTR.ToString().ToUpper() + Public_MainWindow.LanguageCollection[43].ToString());
                            Public_MainWindow.TimtableID = timetablemessage.Id;
                        }

                        if (Public_MainWindow.discord.ConnectionState == Discord.ConnectionState.Connected)
                        { Application.Current.Dispatcher.Invoke((Action)(() => { MainWindow.SelfRollingStartUp(); })); }
                        Public_MainWindow.isposting = 1;
                    }
                }
                catch (Exception s)
                {
                    if (s is Discord.Net.HttpException)
                    {
                        await Public_MainWindow.discord.StopAsync();
                        Application.Current.Dispatcher.Invoke((Action)(() => {
                            ErrorMessageBox emb = new ErrorMessageBox(Public_MainWindow.LanguageCollection[119].ToString(), Public_MainWindow.LanguageCollection[120].ToString(), Public_MainWindow.LanguageCollection[121].ToString(), Public_MainWindow.LanguageCollection[122].ToString());
                            emb.MB_typeOK(Public_MainWindow.LanguageCollection[108].ToString(), s.Message + Environment.NewLine + Environment.NewLine + Public_MainWindow.LanguageCollection[115].ToString(), Public_MainWindow);
                            emb.Show();
                            Public_MainWindow.IsEnabled = false;
                        }));
                    }
                    if (s is System.ArgumentOutOfRangeException)
                    {
                        await Public_MainWindow.discord.StopAsync();
                        Application.Current.Dispatcher.Invoke((Action)(() => {
                            ErrorMessageBox emb = new ErrorMessageBox(Public_MainWindow.LanguageCollection[119].ToString(), Public_MainWindow.LanguageCollection[120].ToString(), Public_MainWindow.LanguageCollection[121].ToString(), Public_MainWindow.LanguageCollection[122].ToString());
                            emb.MB_typeOK(Public_MainWindow.LanguageCollection[108].ToString(), s.Message + Environment.NewLine + Environment.NewLine + Public_MainWindow.LanguageCollection[116].ToString(), Public_MainWindow);
                            emb.Show();
                            Public_MainWindow.IsEnabled = false;
                        }));
                    }
                    if (s is System.NullReferenceException)
                    {
                        Application.Current.Dispatcher.Invoke((Action)(() => {
                            ErrorMessageBox emb = new ErrorMessageBox(Public_MainWindow.LanguageCollection[119].ToString(), Public_MainWindow.LanguageCollection[120].ToString(), Public_MainWindow.LanguageCollection[121].ToString(), Public_MainWindow.LanguageCollection[122].ToString());
                            emb.MB_typeOK(Public_MainWindow.LanguageCollection[113].ToString(), Public_MainWindow.LanguageCollection[124].ToString(), Public_MainWindow);
                            emb.Show();
                            Public_MainWindow.IsEnabled = false;
                        }));
                    }
                    var bc = new BrushConverter();
                    Application.Current.Dispatcher.Invoke((Action)(() => {
                        Public_MainWindow.DiscordBotConnectionStatusLabel.Foreground = (Brush)bc.ConvertFrom("#FFBB3D3D");
                        Public_MainWindow.DiscordBotConnectionStatusLabel.Content = Public_MainWindow.LanguageCollection[7].ToString();
                    }));

                    /*StartPosting();*/
                }
            }
            else
            {
                Application.Current.Dispatcher.Invoke((Action)(() => {
                    ErrorMessageBox emb = new ErrorMessageBox(Public_MainWindow.LanguageCollection[119].ToString(), Public_MainWindow.LanguageCollection[120].ToString(), Public_MainWindow.LanguageCollection[121].ToString(), Public_MainWindow.LanguageCollection[122].ToString());
                    emb.MB_typeOK(Public_MainWindow.LanguageCollection[113].ToString(), Public_MainWindow.LanguageCollection[124].ToString(), Public_MainWindow);
                    emb.Show();
                    Public_MainWindow.IsEnabled = false;
                }));
            }

        }
        public void Missing_Icon_Fix(DA_MainWindow MainWindow)
        {
            System.Threading.Thread.Sleep(1000);
            Public_MainWindow = MainWindow;           
            Application.Current.Dispatcher.Invoke((Action)(() => {
                Public_MainWindow.ShowInTaskbar = false;
                Public_MainWindow.ShowInTaskbar = true;
                //Public_MainWindow.WindowState = WindowState.Normal;
            }));
        }
        public SocketGuild Get_Guild(DA_MainWindow MainWindow)
        {
            SocketGuild guild = MainWindow.discord.GetGuild(MainWindow.ServerID);
            return guild;
        }
        public IReadOnlyCollection<SocketGuild> Get_Guilds(DA_MainWindow MainWindow)
        {
            IReadOnlyCollection<SocketGuild> Servers = MainWindow.discord.Guilds;
            return Servers;
        }
        public IReadOnlyCollection<SocketTextChannel> Get_Channels(DA_MainWindow MainWindow, SocketGuild Guild)
        {
            IReadOnlyCollection<SocketTextChannel> Channels = Guild.TextChannels;
            return Channels;
        }
        public IReadOnlyCollection<SocketRole> Get_Roles(DA_MainWindow MainWindow, SocketGuild Guild)
        {
            IReadOnlyCollection<SocketRole> Roles = Guild.Roles;
            return Roles;
        }
        public async void AddReactionsAsync(List<string> Reactions, Discord.Rest.RestUserMessage Message)
        {
            foreach(var Reaction in Reactions)
            {
                if (Reaction != "" && Reaction != null)
                {
                    if (Reaction.Contains("<:") && Reaction.Contains(":") && Reaction.Contains(">"))
                    { await Message.AddReactionAsync(Emote.Parse(Reaction)); }
                    else
                    { await Message.AddReactionAsync(new Emoji(Reaction)); }
                }
            }            
        }
        public async void AddRoleToUser(string SelfRollingSettings, SocketReaction Reaction, DA_MainWindow MainWindow)
        {
            try
            {
                SocketRole Selected_Role = null;
                SocketGuildUser User = null;
                foreach (var data in SelfRollingSettings.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
                {
                    if (Reaction.Emote.ToString() == GetStrBetweenTags(data, "[Emote]", "[/Emote]"))
                    {
                        var Guild = MainWindow.discord.GetGuild(MainWindow.ServerID);
                        User = Guild.GetUser(Reaction.UserId);
                        Selected_Role = Guild.GetRole(ulong.Parse(GetStrBetweenTags(data, "[Role]", "[/Role]")));
                        break;
                    }
                }
                if (Selected_Role != null)
                { await (User as IGuildUser).AddRoleAsync(Selected_Role); }
            }
            catch (Exception) { }
        }
        public async void RemoveRoleToUser(string SelfRollingSettings, SocketReaction Reaction, DA_MainWindow MainWindow)
        {
            try
            {
                SocketRole Selected_Role = null;
                SocketGuildUser User = null;
                foreach (var data in SelfRollingSettings.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
                {
                    if (Reaction.Emote.ToString() == GetStrBetweenTags(data, "[Emote]", "[/Emote]"))
                    {
                        var Guild = MainWindow.discord.GetGuild(MainWindow.ServerID);
                        User = Guild.GetUser(Reaction.UserId);
                        Selected_Role = Guild.GetRole(ulong.Parse(GetStrBetweenTags(data, "[Role]", "[/Role]")));
                        break;
                    }
                }
                if (Selected_Role != null)
                { await (User as IGuildUser).RemoveRoleAsync(Selected_Role); }
            }
            catch (Exception) { }
        }
    }
}
