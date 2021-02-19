using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AncientPanda_Launcher
{
    public enum ErrorCodes
    {
        DownloadEnd = 1,
        FatalException,
        Other
    }
    enum LauncherState
    {
        ready,
        failed,
        downloadingGame,
        downloadingUpdate,
        running,
        serverOffline
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string rootPath;
        private Process currentGame = null;
        private string gameZip;
        private string gameExe;
        private LauncherState currentLauncherState;
        internal LauncherState CurrentState
        {
            get => currentLauncherState;
            set
            {
                currentLauncherState = value;
                switch (currentLauncherState)
                {
                    case LauncherState.ready:
                        StateTxt.Content = "Play";
                        break;
                    case LauncherState.failed:
                        StateTxt.Content = "Failed";

                        break;
                    case LauncherState.downloadingGame:
                        StateTxt.Content = "Downloading";

                        break;
                    case LauncherState.downloadingUpdate:
                        StateTxt.Content = "Downloading";
                        break;
                    case LauncherState.running:
                        StateTxt.Content = "Running";

                        break;
                    case LauncherState.serverOffline:
                        StateTxt.Content = "Offline";
                        break;
                    default:
                        break;
                }
            }
        }

        private void CheckForUpdates()
        {


            if (File.Exists(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "AnimalWar_Game", "AnimalWar.exe")))
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.version))
                {
                    Version localVersion = new Version(Properties.Settings.Default.version);

                    try
                    {
                        WebClient webClient = new WebClient();
                        Version onlineVer = new Version(Constants.newVer);
                        if (onlineVer.IsVersionDifferent(localVersion))
                        {
                            InstallGameFiles(true, onlineVer);
                        }
                        else
                        {
                            CurrentState = LauncherState.ready;
                        }
                    }
                    catch (Exception ex)
                    {
                        CurrentState = LauncherState.failed;
                        ShowCustomError($"Error while fetching updates\n{ex}", "", "Ok :(", ErrorCodes.Other);
                    }
                }
                else
                {
                    InstallGameFiles(false, Version.zero);

                }
            }
            else
            {
                InstallGameFiles(false, Version.zero);

            }

        }

        public WebClient downloadClient;
        private void InstallGameFiles(bool isUpdate, Version _newVersion)
        {
            try
            {
             
                downloadClient = new WebClient();
                CurrentState = isUpdate ? LauncherState.downloadingUpdate : LauncherState.downloadingGame;
                if (!isUpdate)
                {
                    _newVersion = new Version(Constants.newVer);
                }
                downloadClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadComplete);
                downloadClient.DownloadProgressChanged += (s, e) =>
                {
                    DownloadBar.Visibility = Visibility.Visible;
                    DownloadTxt.Visibility = Visibility.Visible;
                    DownloadAmount.Visibility = Visibility.Visible;
                    DownloadAmount.Content = $"{e.BytesReceived / 1000000} of {e.TotalBytesToReceive / 1000000} Mb";
                    DownloadBar.Value = e.ProgressPercentage;
                    DownloadTxt.Content = $"{ e.ProgressPercentage}%";
                };
                downloadClient.DownloadFileAsync(new Uri(Constants.onlineZip), gameZip, _newVersion);
                isDownloading = true;
            }
            catch (Exception ex)
            {
                CurrentState = LauncherState.failed;
                ShowCustomError($"Error while downloading\n{ex}","", "Ok :(", ErrorCodes.Other);
            }

        }
        public ZipArchive zip;

        public Progress<ZipProgress> _progress;
        private void ExtractZip()
        {
            WebClient wc = new WebClient();
            Stream zipReadingStream = wc.OpenRead(gameZip);
            zip = new ZipArchive(zipReadingStream);
            zip.ExtractToDirectory(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "AnimalWar_Game"), _progress);
            zip.Dispose();
        }

        private bool Disposed;
        private async void DownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    downloadClient.Dispose();
                    downloadClient.Disposed += (sender, eventArgs) =>
                    {
                        File.Delete(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Build.zip"));
                        if (Logout)
                        {
                            Properties.Settings.Default.username = null;
                            Properties.Settings.Default.password = null;
                            Properties.Settings.Default.Save();
                            var loginWindow = new LoginWindow();
                            loginWindow.Show();
                            this.Close();
                        }
                    };
                    downloadClient = null;
                }
                else
                {
                    DownloadTxt.Visibility = Visibility.Hidden;
                    string onlineVersionDownloaded = ((Version)e.UserState).ToString();
                    await Task.Run(() => ExtractZip());
                    Properties.Settings.Default.version = onlineVersionDownloaded;
                    Properties.Settings.Default.Save();
                    File.Delete(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Build.zip"));
                    CurrentState = LauncherState.ready;
                    DownloadAmount.Visibility = Visibility.Hidden;
                    DownloadBar.Visibility = Visibility.Hidden;
                    isDownloading = false;
                }
            }
            catch (Exception ex)
            {
                CurrentState = LauncherState.failed;
                ShowCustomError($"Fatal exception during installation\n{ex}","","Ok :(", ErrorCodes.Other);
              
            }
        }

        private void Report(object sender, ZipProgress e)
        {
            DownloadAmount.Content = $"Extracting installation files... {e.CurrentItem}";
            DownloadBar.Value = e.Processed;
        }

        public MainWindow()
        {

            _progress = new Progress<ZipProgress>();
            _progress.ProgressChanged += Report;
            InitializeComponent();
    
            if (File.Exists(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Build.zip")))
            {
                File.Delete(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Build.zip"));
            }
            rootPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            gameZip = Path.Combine(rootPath, "Build.zip");
            gameExe = Path.Combine(rootPath, "AnimalWar_Game", "AnimalWar.exe");
            Constants.ServerIpAddress = Properties.Settings.Default.ip;
            ipConfig.Text = Constants.ServerIpAddress;
        }
        private bool isConnecting = false;
        public async Task SocketConnect()
        {
            isConnecting = true;
            StateTxt.Content = "Connecting...";
            var tcpClient = new TcpClient();
                try
                {
                    await tcpClient.ConnectAsync(Properties.Settings.Default.ip, 27017);
                if (tcpClient.Connected)
                {
                    CheckForUpdates();
                } else
                {
                    throw new Exception("Offline");
                }
                }
                catch (Exception ex)
                {
                    CurrentState = LauncherState.serverOffline;
                }
            tcpClient.Close();
            tcpClient.Dispose();
            tcpClient = null;
            isConnecting = false;
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            UserLabel.Content = Constants.Username.ToUpper();
            if (IsIPAddress(Properties.Settings.Default.ip))
            {
                //Verify that we have a valid ip saved
                SocketConnect();
            }
        }
        private void StartGame()
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(gameExe);
            process.StartInfo.WorkingDirectory = Path.Combine(rootPath, "AnimalWar_Game");
            process.StartInfo.Arguments = $"-ip {Constants.ServerIpAddress} -user {Constants.Username} -pass {Constants.Password}";
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(CallOnExit);
            process.Start();
            currentGame = process;

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {

        }
        void CallOnExit(object sender, System.EventArgs e)
        {
            currentGame = null;
        }


        private void ipConfig_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (IsIPAddress(ipConfig.Text))
                {
                    Properties.Settings.Default.ip = ipConfig.Text;
                    Properties.Settings.Default.Save();
                } else
                {
                    var defaultIp = "";
                    if (string.IsNullOrEmpty(Properties.Settings.Default.ip))
                    {
                        defaultIp = "127.0.0.1";
                    } else if(Properties.Settings.Default.ip.ToLower() != ipConfig.Text.ToLower())
                    {
                        defaultIp = Properties.Settings.Default.ip;
                    } else
                    {
                        defaultIp = "127.0.0.1";
                    }
                    ShowCustomError($"The provided ip {ipConfig.Text} is not a valid IP\nThe ip field will fallback to {defaultIp}", "", "Ok", ErrorCodes.Other);
                    Properties.Settings.Default.ip = defaultIp;
                    ipConfig.Text = defaultIp;
                    Properties.Settings.Default.Save();
                    Keyboard.ClearFocus();
                    SettingsMenu.Visibility = Visibility.Hidden;
                }

            }
        }

        private bool IsIPAddress(string ipAddress)
        {
            bool retVal = false;

            try
            {
                IPAddress address;
                retVal = IPAddress.TryParse(ipAddress, out address);
            }
            catch (Exception ex)
            {
                ShowCustomError($"Error while trying to parse the Ip {ipAddress}", "", "Ok", ErrorCodes.Other);

            }
            return retVal;
        }
        private bool isDownloading = false;

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (isDownloading)
            {
                ShowCustomError("You cannot logout while a download is in progress...", "Cancel Download", "Continue Download", ErrorCodes.DownloadEnd);
            }
            else
            {
                Properties.Settings.Default.username = null;
                Properties.Settings.Default.password = null;
                Properties.Settings.Default.Save();
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }


        private void Rectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CurrentState == LauncherState.serverOffline) {
                if (!isConnecting)
                {
                    SocketConnect();
                }
            } else if (File.Exists(gameExe) && CurrentState == LauncherState.ready && currentGame == null)
            {
                SocketConnect(StartGame);   
            }
            else if (CurrentState == LauncherState.failed)
            {
                CheckForUpdates();

            }
            else
            {
                CheckForUpdates();
            } 
        }

        private async Task SocketConnect(Action startGame)
        {
            isConnecting = true;
            var tcpClient = new TcpClient();
            try
            {
                await tcpClient.ConnectAsync(Properties.Settings.Default.ip, 27017);
                if (tcpClient.Connected)
                {
                    startGame.Invoke();
                }
                else
                {
                    throw new Exception("Offline");
                }
            }
            catch (Exception ex)
            {
                ShowCustomError("The server you are trying to connect to is currently Offline", "", "Ok :(", ErrorCodes.Other);
                CurrentState = LauncherState.serverOffline;
            }
            tcpClient.Close();
            tcpClient.Dispose();
            tcpClient = null;
            isConnecting = false;
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



        struct Version
        {
            internal static Version zero = new Version(0, 0, 0);
            private short major;
            private short minor;
            private short subminor;

            internal Version(short _major, short _minor, short _subminor)
            {
                major = _major;
                minor = _minor;
                subminor = _subminor;
            }
            internal Version(string _version)
            {
                string[] _versionStrings = _version.Split('.');
                if (_versionStrings.Length != 3)
                {
                    major = 0;
                    minor = 0;
                    subminor = 0;
                    return;
                }
                major = short.Parse(_versionStrings[0]);
                minor = short.Parse(_versionStrings[1]);
                subminor = short.Parse(_versionStrings[2]);
            }
            internal bool IsVersionDifferent(Version version)
            {
                if (major != version.major)
                {
                    //NEW MAJOR RELEASE
                    return true;
                }
                else
                {
                    if (minor != version.minor)
                    {
                        //NEW MINOR RELEASE STILL REQUIRED
                        return true;
                    }
                    else
                    {
                        if (subminor != version.subminor)
                        {
                            //NEW SUBMINOR RELEASE RECOMEND INSTALLATION
                            return true;
                        }
                    }
                }
                return false;
            }

            public override string ToString()
            {
                return $"{major}.{minor}.{subminor}";
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

        private Action customAction;
        private Action okAction;

        private void PerformWarningCustomAction_MouseDown (object sender, MouseButtonEventArgs e)
        {
            customAction?.Invoke();
        }
        private void PerformWarningOkAction_MouseDown (object sender, MouseButtonEventArgs e)
        {
            okAction?.Invoke();
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
        public void ShowCustomError(string Error, string CustomBtnText, string OkBtnTxt, ErrorCodes ErrorCode)
        {
            customAction = null;
            okAction = null;
            OkActionWarningBtn.Content = OkBtnTxt;
            ErrorContentTxt.Text = Error;
            ErrorGrid.Visibility = Visibility.Visible;
            switch (ErrorCode)
            {
                case ErrorCodes.DownloadEnd:
                    okAction = ContinueDownloading;
                    break;
                case ErrorCodes.FatalException:
                    break;
                case ErrorCodes.Other:
                    okAction = HideMessage;
                    break;
                default:
                    break;
            }

        }
        private void HideMessage()
        {
            ErrorGrid.Visibility = Visibility.Hidden;
        }

        #region ErrorVoids
        private bool Logout = false;
        private void ContinueDownloading()
        {
            ErrorGrid.Visibility = Visibility.Hidden;
        }

        #endregion



        private void MinimizeOverride_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ConfigSett_Btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SettingsMenu.Visibility = SettingsMenu.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private void DragBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        private void LaunchGitHub_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var openUpdaters = Process.GetProcessesByName("AncientUpdater.exe");
                if(openUpdaters.Length != 0)
                {
                    foreach (var updater in openUpdaters)
                    {
                        updater.Kill();
                    }
                }
                var prs = new Process();
                prs.StartInfo.FileName = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "AncientUpdater.exe");
                prs.StartInfo.Arguments = "-launchweb https://github.com/Ancient-Panda-Studio/AnimalWARS";
                prs.Start();
            }
            catch (Exception ex)
            {
                ShowCustomError($"Could not launch https://github.com/Ancient-Panda-Studio/AnimalWARS\n{ex}","", "Ok :(", ErrorCodes.Other);
            }

        }

        private void LaunchGitHub_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.Cursor != Cursors.Wait)
                Mouse.OverrideCursor = Cursors.Hand;

        }

        private void LaunchGitHub_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
