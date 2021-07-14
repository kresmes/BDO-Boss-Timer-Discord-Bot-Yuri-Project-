using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace YPBBT
{
    public class AutoUpdate
    {
        DA_MainWindow Public_MainWindow;
        public void Check_For_Update(DA_MainWindow mainWindow)
        {
            Public_MainWindow = mainWindow;
            Application.Current.Dispatcher.Invoke((Action)(() => { Public_MainWindow.Processing_Status(false, Public_MainWindow.LanguageCollection[125].ToString()); }));
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://raw.githubusercontent.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/master/Resources/Version");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (String.IsNullOrWhiteSpace(response.CharacterSet))
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    string data = readStream.ReadToEnd();

                    Public_MainWindow.CurrentVersion = GetStrBetweenTags(data, "[AppVersion]", "[/AppVersion]");
                    if (GetStrBetweenTags(readStream.ReadToEnd(), "[TimeTable]", "[/TimeTable]") != "")
                    { File.WriteAllText(Directory.GetCurrentDirectory() + "/Resources/LYPBBTTT_Origin", GetStrBetweenTags(data, "[TimeTable]", "[/TimeTable]").Trim()); }
                    if(GetStrBetweenTags(readStream.ReadToEnd(), "[Bosses]", "[/Bosses]") != "")
                    { File.WriteAllText(Directory.GetCurrentDirectory() + "/Resources/BossesOrigin", GetStrBetweenTags(data, "[Bosses]", "[/Bosses]").Trim()); }
                    response.Close();
                    readStream.Close();
                    if(Public_MainWindow.CurrentVersion != Public_MainWindow.AppVersion)
                    {
                        string version = Public_MainWindow.CurrentVersion;
                        using (var client = new WebClient())
                        {
                            if(!Directory.Exists(System.IO.Directory.GetCurrentDirectory() + "/Update"))
                            { Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + "/Update");}
                            client.DownloadFile("https://github.com/kresmes/BDO-Boss-Timer-Discord-Bot-Yuri-Project-/releases/download/" + version + "/AutoUpdate.Update",
                                System.IO.Directory.GetCurrentDirectory() + "/Update/AutoUpdate.Update");
                            File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Update/installUpdate", "True");
                            Application.Current.Dispatcher.Invoke((Action)(() => { 
                                Public_MainWindow.Processing_Status(true, Public_MainWindow.LanguageCollection[126].ToString());
                                Public_MainWindow.AlertNewUpdate();
                                Public_MainWindow.appRestartButton.ToolTip = Public_MainWindow.LanguageCollection[146].ToString();
                            }));
                        }
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke((Action)(() => { Public_MainWindow.Processing_Status(true, Public_MainWindow.LanguageCollection[127].ToString()); }));
                    }
                }
            }
            catch (Exception) { 
                Public_MainWindow.CurrentVersion = Public_MainWindow.AppVersion; 
                Application.Current.Dispatcher.Invoke((Action)(() => { Public_MainWindow.Processing_Status(true, Public_MainWindow.LanguageCollection[128].ToString(), true); }));
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
    }
}
