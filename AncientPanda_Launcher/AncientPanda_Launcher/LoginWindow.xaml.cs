using Flurl.Http;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFCustomMessageBox;

namespace AncientPanda_Launcher
{

    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private string ipServerAddress = "http://34.76.253.250/";
        private Process updateProcess = null;
        private string sqlAddress = $"http://34.76.253.250/sqlconnect/login.php";
        public LoginWindow()
        {

            var duplicates = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location));
            if(duplicates.Length != 1)
            {
                MessageBoxResult result = CustomMessageBox.ShowOK(
                    $"{duplicates.Length} Instances of the launcher where found! \nAll instances (including this) will be terminated, you may relaunch!",
                    "Duplicate Alert",
                    "OK!"
                    );

                if(result != MessageBoxResult.None)
                {
                    foreach (var xs in duplicates)
                    {
                        xs.Kill();
                    }
                }
            }
                InitializeComponent();
                var path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "launcherConfig.txt");
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(Constants.LauncherVersion);
                    fs.Write(info, 0, info.Length);
                }

                if (File.Exists(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "AncientUpdater.exe")))
                {
                    Process process = new Process();
                    process.StartInfo =
                        new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "AncientUpdater.exe"));
                    process.EnableRaisingEvents = true;
                    process.Start();
                    updateProcess = process;
                    BlockWhileUpdate();
                    if (!string.IsNullOrEmpty(Properties.Settings.Default.username) &&
                        !string.IsNullOrEmpty(Properties.Settings.Default.password))
                    {
                        Login(Properties.Settings.Default.username, Properties.Settings.Default.password, true);
                    }

                    GetData();
                }
                else
                {
                    var result = CustomMessageBox.ShowOKCancel(
                        "AncientUpdater.exe was not found in your system... This will prevent the execution of the launcher\n You may download it by pressing 'Download'",
                        "Exception Raise #77 AncientUpdater.exe not found!",
                        "Download",
                        "Exit");
                    if (result == MessageBoxResult.Cancel)
                    {
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        //Download Ancient Updater.
                        var webClient = new WebClient();
                        var completed = false;
                        webClient.DownloadFileCompleted += (s, e) =>
                        {
                            CustomMessageBox.ShowOK(
                                "Download Completed! The launcher will now RESTART.",
                                "Download Completed",
                                "Ok");
                            System.Diagnostics.Process.Start(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "AncientPanda_Launcher.exe"));
                            Application.Current.Shutdown();
                        };
                        webClient.DownloadFileAsync(new Uri(Constants.AncientUpdater),"AncientUpdater.exe");
                    }

                }
        }

        private void GetData()
        {
            var data = Get("http://34.76.253.250:3000/version");
            data = data.Replace("{", "").ToString().Replace("\"", "").ToString().Replace("}", "");
            var splitVersionString = data.Split(',');
            foreach (var currentString in splitVersionString)
            {
                if (!currentString.Contains("animal")) continue;
                var str = currentString.Split(':')[1];
                if (str.Contains("|"))
                {
                    Constants.onlineZip = str.Replace('|', ':');
                    continue;
                }
                Constants.newVer = str;
            }

        }

        private void BlockWhileUpdate()
        {
            while (!updateProcess.HasExited)
            {
                updateProcess.WaitForExit();
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        public static string Get(string uri)
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
            request.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;
            using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        private void VerifyInput()
        {
            if (string.IsNullOrEmpty(UserTxt.Text) && string.IsNullOrEmpty(PassTxt.Password))
            {
                ErrorTxt.Visibility = Visibility.Visible;
                ErrorTxt.Content = "Username and Password cannot be empty.";

            }
            else if (string.IsNullOrEmpty(UserTxt.Text))
            {
                ErrorTxt.Visibility = Visibility.Visible;

                ErrorTxt.Content = "Username cannot be empty.";

            }
            else if (string.IsNullOrEmpty(PassTxt.Password))
            {
                ErrorTxt.Visibility = Visibility.Visible;
                ErrorTxt.Content = "Password cannot be empty.";
            }
            else
            {
                Login(UserTxt.Text, PassTxt.Password, false);
            }
        }
        private void Rectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            VerifyInput();
        }
        private void StateTxt_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            var bc = new BrushConverter();
            PlayBtn.Fill = (Brush)bc.ConvertFrom("#FF06D6A0");
        }

        private void StateTxt_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.Cursor != Cursors.Wait)
                Mouse.OverrideCursor = Cursors.Hand;
            var bc = new BrushConverter();
            PlayBtn.Fill = (Brush)bc.ConvertFrom("#FF06D6D6");

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private async void Login(string username, string password, bool start)
        {
            var responseString = await sqlAddress
                .PostUrlEncodedAsync(new { user = username, pass = password })
                .ReceiveString();
            if (responseString[0] == '0')
            {
                //CorrectLogin
                if (RememberCheck.IsChecked == true && !start)
                {
                    //REMEMBER USER
                    Properties.Settings.Default.username = username;
                    Properties.Settings.Default.password = password;
                    Properties.Settings.Default.Save();
                }
                Constants.Username = username;
                Constants.Password = password;
                var mainForm = new MainWindow();
                mainForm.Show();
                this.Close();

            }
            else
            {
                if (responseString.StartsWith("Error code #5"))
                {
                    ErrorTxt.Visibility = Visibility.Visible;
                    ErrorTxt.Content = $"{username} does not exist";
                } else if(responseString.StartsWith("Error code #7"))
                {
                    ErrorTxt.Visibility = Visibility.Visible;
                    ErrorTxt.Content = $"Incorrect password for {username}";
                } else
                {
                    ErrorTxt.Visibility = Visibility.Visible;
                    ErrorTxt.Content = $"Unknown connection error.";
                }
            }

        }

        private void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.Cursor != Cursors.Wait)
                Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Ellipse_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;

        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var ps = Process.GetProcessesByName("AnimalWar.exe");
            if (ps.Length != 0)
            {
                foreach (var game in ps)
                {
                    game.Kill();
                }
            }
            System.Windows.Application.Current.Shutdown();
        }

        private void MinimizeOverride_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void DragBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        private void UserTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            VerifyInput();
        }
    }
}
