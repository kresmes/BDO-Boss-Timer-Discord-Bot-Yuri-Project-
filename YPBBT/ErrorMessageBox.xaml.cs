using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Discord;
using Discord.WebSocket;

namespace YPBBT
{
    /// <summary>
    /// Interaction logic for ErrorMessageBox.xaml
    /// </summary>
    public partial class ErrorMessageBox : Window
    {
        int YN_Case;
        DiscordSocketClient discord = new DiscordSocketClient();
        DA_MainWindow damw;
        DA_Startup dasw;
        public ErrorMessageBox(string ok_tr,string No_tr,string Yes_tr, string test_token_tr)
        {
            InitializeComponent();
            OK.Content = ok_tr;
            NO.Content = No_tr;
            YES.Content = Yes_tr;           
            Test.Content = test_token_tr;
        }
        public void MB_typeOK(string et,string em, DA_MainWindow dm)
        {
            SystemSounds.Beep.Play();
            damw = dm;
            NO.Visibility = Visibility.Hidden;
            YES.Visibility = Visibility.Hidden;
            OK.Visibility = Visibility.Visible;
            Test.Visibility = Visibility.Hidden;
            Textbox1.Visibility = Visibility.Hidden;
            ErrorTitle.Content = et;
            ErrorMessage.Text = em;
        }
        public void MB_typeYN(string et, string em,int YN, DA_MainWindow dm)
        {
            SystemSounds.Exclamation.Play();
            NO.Visibility = Visibility.Visible;
            YES.Visibility = Visibility.Visible;
            OK.Visibility = Visibility.Hidden;
            Test.Visibility = Visibility.Hidden;
            Textbox1.Visibility = Visibility.Hidden;
            damw = dm;
            YN_Case = YN;
            ErrorTitle.Content = et;
            ErrorMessage.Text = em;
        }
        public void MB_typeYN_DA_Startup(string et, string em, int YN, DA_Startup ds)
        {
            SystemSounds.Exclamation.Play();
            NO.Visibility = Visibility.Visible;
            YES.Visibility = Visibility.Visible;
            OK.Visibility = Visibility.Hidden;
            Test.Visibility = Visibility.Hidden;
            Textbox1.Visibility = Visibility.Hidden;
            dasw = ds;
            YN_Case = YN;
            ErrorTitle.Content = et;
            ErrorMessage.Text = em;
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {          
            damw.enableMW();
            damw.Show();
            this.Close();
        }

        public void TestToken(string et, DiscordSocketClient ds, DA_MainWindow dm,string token)
        {
            ErrorTitle.Content = et;
            ErrorTitle.Visibility = Visibility.Visible;
            NO.Visibility = Visibility.Hidden;
            YES.Visibility = Visibility.Hidden;
            OK.Visibility = Visibility.Hidden;
            ErrorMessage.Visibility = Visibility.Hidden;
            Test.Visibility = Visibility.Visible;
            Textbox1.Visibility = Visibility.Visible;
            Textbox1.Text = token;
            discord = ds;
            damw = dm;
        }
        private async void Test_Click(object sender, RoutedEventArgs e)
        {           
            try
            {
                await discord.LoginAsync(TokenType.Bot, Textbox1.Text);
                await discord.StartAsync();
                System.Threading.Thread.Sleep(1000);
                Test.BorderBrush = Brushes.Green;
                Test.Background = Brushes.Green;
            }
            catch (Exception)
            {
                Test.BorderBrush = Brushes.Red;
                Test.Background = Brushes.Red;
            }

            if (Test.BorderBrush == Brushes.Green)
            {
                damw.ReturnFixedToken(discord, Textbox1.Text);
                damw.Show();
                this.Close();
            }
        }
        private void Step4TestTokenButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Test.Background = (Brush)bc.ConvertFrom("#818182");
        }

        private void Step4TestTokenButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Test.Background = (Brush)bc.ConvertFrom("#FFA9A9AA");
        }

        private void YES_Click(object sender, RoutedEventArgs e)
        {
            if(YN_Case == 0)
            {
                damw.confirmhardreset();
                damw.IsEnabled = true;
                this.Close();
            }
            if(YN_Case == 1)
            {
                System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + "/ExternalAppRestart.exe");              
            }
        }
        private void NO_Click(object sender, RoutedEventArgs e)
        {
            if (YN_Case == 0)
            {
                damw.IsEnabled = true;
                damw.Show();
                this.Close();
            }
            if (YN_Case == 1)
            {
                dasw.IsEnabled = true;
                dasw.Run_Profile_Setup(false);
                this.Close();
            }
        }
    }
}
