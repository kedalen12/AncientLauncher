using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Threading;
using System.Text.RegularExpressions;

namespace AncientUpdater
{
    internal class Program
    {
        private const string Title =
            @"                           
 ____                     __              
/\  _`\                  /\ \             
\ \ \L\ \ __      ___    \_\ \     __     
 \ \ ,__/'__`\  /' _ `\  /'_` \  /'__`\   
  \ \ \/\ \L\.\_/\ \/\ \/\ \L\ \/\ \L\.\_ 
   \ \_\ \__/.\_\ \_\ \_\ \___,_\ \__/.\_\
    \/_/\/__/\/_/\/_/\/_/\/__,_ /\/__/\/_/
";
        private static string _installer = "";
        private static UpdateResources _update;

        private static void DeleteDirectory(string directory)
        {
            string[] files = Directory.GetFiles(directory);
            string[] dirs = Directory.GetDirectories(directory);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(directory, false);
        }

        
        static void Main(string[] args)
        {
            for (var i = 0; i < args.Length; i++)
            {
                if (args[i] == "-launchweb" && args.Length > i + 1)
                {
                    Process.Start(args[i + 1]);
                    return;
                }
            }
            File.Delete(System.IO.Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "GenerateShortCut.vbs"));
            Console.WriteLine(Title);
            _installer = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "AncientLauncherInstaller.exe");

            if (Directory.Exists(Path.Combine(Path.GetTempPath(), "ancient_temp")))
            {
                DeleteDirectory(Path.Combine(Path.GetTempPath(), "ancient_temp"));
            }

            //CHECK IF launcherConfig exists
                if (File.Exists(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "launcherConfig.txt")))
                {
                    var version =
                        File.ReadAllLines(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "launcherConfig.txt"));
                    _update = new UpdateResources(_installer, version[0]);

                }
                else
                {
                    if (File.Exists(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "AncientPanda_Launcher.exe")))
                    {
                        //Launcher is installed but this application was launched on its own
                        Console.WriteLine(
                            $"launcherConfig.txt was not find in current directory but Launcher executable was...\n This might be caused by executing the AncientUpdater file without having executed AncientLauncher once.");
                        Console.WriteLine($"Open AncientPanda_Launcher.exe...");
                        Console.WriteLine($"Exiting...");
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine($"AncientLauncher.exe was not found... Installing latest version");
                        Console.WriteLine($"Do you want to install the latest version? Y/N  ");
                        var x = Console.ReadLine() ?? "n";
                        if (x.Contains("Y") || x.Contains("y"))
                        {
                            _update = new UpdateResources(_installer);
                        }
                        else
                        {
                            Console.WriteLine("Exiting...");
                            Environment.Exit(0);
                        }

                    }
                }
                while (!_update.InstallationDone)
                {
                    Thread.Sleep(1000);
                }
        }
    }

    class UpdateResources
    {
        private volatile bool _completed;

        private static string _onlineZip =
            "https://dl.dropboxusercontent.com/s/cgxkkz6otn9i9ek/AncientLauncher.zip?dl=0";

        private string _installer;
        private string _currentVer;
        public bool InstallationDone = false;

        public UpdateResources(string installer, string currentVer)
        {
            this._currentVer = currentVer;
            this._installer = installer;
            CheckForUpdates(currentVer);
        }

        public UpdateResources(string installer)
        {
            this._installer = installer;
            DownloadFile();
        }

        private static string Get(string uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using(var response = (HttpWebResponse)request.GetResponse())
                using(var stream = response.GetResponseStream())
                using(var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while connecting to Ancient API {e}");
                return null;
            }
        }
        private static void DeleteDirectory(string directory)
        {
            string[] files = Directory.GetFiles(directory);
            string[] dirs = Directory.GetDirectories(directory);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(directory, false);
        }
        private void CheckForUpdates(string currentVersion)
        {
            try
            { 
                var onlineVer = Get("http://34.105.202.247:3000/launcher");
                Console.WriteLine(onlineVer);
                if (onlineVer == null)
                {
                    Console.WriteLine("Fatal exception... You may use current Launcher Version if it is installed...");
                    ExitSuccessful();
                }
                else
                {
                    var  onlineVerTxt = Regex.Replace(onlineVer, @"[\""]", "", RegexOptions.None).ToString();
                    if (currentVersion != onlineVerTxt)
                    {
                        Console.WriteLine("New versions Found -> current is {0} new  is -> {1}", currentVersion,
                            onlineVerTxt);
                        Console.Write("Do you want to install the latest update Y/N ");
                        var s = Console.ReadLine();
                        if (s.Contains("Y") || s.Contains("y"))
                        {
                            DownloadFile();
                        }
                        else
                        {
                            Console.WriteLine("Update canceled exiting...");
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Current version is up to date");
                        Environment.Exit(0);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ExitSuccessful();
                Console.Write($"Error while checking for updates {ex}");
            }
        }

        public void DownloadFile()
        {
            var procs = Process.GetProcessesByName("AncientPanda_Launcher");
            if (procs.Length != 0)
            {
                Console.WriteLine($"Found running instance/s of AncienLauncher... \n Closing");
            }

            foreach (var p in procs)
            {
                p.Kill();
            }

            var client = new WebClient();
            var uri = new Uri(_onlineZip);
            _completed = false;
            client.DownloadFileCompleted += (sender, args) => { RaiseSetup(); };
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
            client.DownloadFileAsync(uri, Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "AncientLauncher.zip"));
        }

        public bool DownloadCompleted => _completed;
        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.
            for (var i = 0; i <= 100; ++i)
            {
                Console.Write("\r{0}",
                    $"{e.BytesReceived / 1000} of {e.TotalBytesToReceive / 1000} kilobytes. {e.ProgressPercentage} % complete...");
            }
        }

        private void RaiseSetup()
        {
            try
            {
                Console.WriteLine($"Installing AncientLauncher ...");
                ZipFile.ExtractToDirectory(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "AncientLauncher.zip"),
                    Path.Combine(Path.GetTempPath(), "ancient_temp"));
                foreach (var newPath in Directory.GetFiles(Path.Combine(Path.GetTempPath(), "ancient_temp"), "*.*", 
                    SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(Path.Combine(Path.GetTempPath(), "ancient_temp"), Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)), true);
                Console.WriteLine($"Cleaning Installation ... \n");
                File.Delete(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "AncientLauncher.zip"));
                DeleteDirectory(Path.Combine(Path.GetTempPath(), "ancient_temp"));
                Console.WriteLine("Creating desktop shortcut...");
                AppShortcutToDesktop("AncientLauncher");
                Console.WriteLine($"Installation successful... \n");
                Console.WriteLine($"Exiting Application... \n");
                ExitSuccessful();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ExitSuccessful();
            }
        }
        public void AppShortcutToDesktop(string linkName)
        {
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "AncientLauncher.lnk"));
            using (StreamWriter sw = new StreamWriter(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "GenerateShortCut.vbs"), false))
            {
                sw.WriteLine("Dim WshShell\nSet WshShell = CreateObject(\"Wscript.shell\")\nDim strDesktop \nstrDesktop= WshShell.SpecialFolders(\"Desktop\")\nDim oMyShortcut\nSet oMyShortcut = WshShell.CreateShortcut(strDesktop + \"\\AncientLauncher.lnk\")\nOMyShortcut.TargetPath = \"\" + WshShell.CurrentDirectory + \"\\AncientPanda_Launcher.exe\"\noMyShortCut.Save\nSet WshShell= Nothing\nSet oMyShortcut= Nothing");
            }
            System.Diagnostics.Process.Start(System.IO.Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
            "GenerateShortCut.vbs"));
        }
        private void ExitSuccessful()
        {
            var exitTimer = new Timer(ExitNow, null, 3000, 5000);
            
        }

        private void ExitNow(object state)
        {
            InstallationDone = true;
            
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine(e.Cancelled == true ? "Download has been canceled." : "Download completed!");
            _completed = true;
        }
    }
}