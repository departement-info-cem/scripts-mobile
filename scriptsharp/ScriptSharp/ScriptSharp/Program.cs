using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

// dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true
// permet de generer un seul gros .exe avec tout ce qu'il faut dedans

namespace ScriptSharp
{
    class Program
    {
        static string localCache = "\\\\ed5depinfo\\Logiciels\\Android\\scripts\\cache";


        // Android Studio
        static string STUDIO_URL =
            "https://redirector.gvt1.com/edgedl/android/studio/ide-zips/2024.1.1.12/android-studio-2024.1.1.12-windows.zip";

        static string FLUTTER_PLUGIN_URL_STUDIO =
            "https://plugins.jetbrains.com/plugin/download?rel=true&updateId=582965";

        static string DART_PLUGIN_URL_STUDIO = "https://plugins.jetbrains.com/plugin/download?rel=true&updateId=573248";

        static string FLUTTER_INTL_PLUGIN_URL_STUDIO =
            "https://plugins.jetbrains.com/plugin/download?rel=true&updateId=541419";

        // Android SDK and sdkmanager
        static string CORRETTO_URL = "https://corretto.aws/downloads/latest/amazon-corretto-17-x64-windows-jdk.zip";

        // IntelliJ
        static string IDEA_URL = "https://download.jetbrains.com/idea/ideaIC-2024.2.1.win.zip";

        // Flutter
        static string FLUTTER_SDK =
            "https://storage.googleapis.com/flutter_infra_release/releases/stable/windows/flutter_windows_3.24.0-stable.zip";

        static string URL_3N5 = "https://github.com/departement-info-cem/3N5-Prog3/archive/refs/heads/main.zip";
        static string URL_4N6 = "https://github.com/departement-info-cem/4N6-Mobile/archive/refs/heads/master.zip";
        static string URL_5N6 = "https://github.com/departement-info-cem/5N6-mobile-2/archive/refs/heads/main.zip";
        static string URL_KMB = "https://github.com/departement-info-cem/KickMyB-Server/archive/refs/heads/main.zip";

        static Boolean isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        static async Task Main(string[] args)
        {
            //clear the log file
            File.WriteAllText(Utils.logFilePath, string.Empty);
            Utils.LogAndWriteLine("Execution du script commencee");
            if (!Directory.Exists(localCache) && isWindows)
            {
                Utils.LogAndWriteLine(
                    "Le dossier de cache local n'existe pas. Veuillez vous assurer que le partage réseau est monté et réessayez.");
                Utils.LogAndWriteLine("Main arrêté car le dossier de cache local n'existe pas");
                return;
            }

            // confirm the local cache folder is accessible
            Utils.LogAndWriteLine("Le dossier de cache local est accessible.");
            if (isWindows)
            {
                try
                {
                    AddDesktopToDefenderExclusion();
                    DisableWindowsDefender();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Defender toujours actif, pensez a rouler ca en admin");
                }

            }

            Utils.LogAndWriteLine("Veuillez choisir une option:");
            Utils.LogAndWriteLine("1. 3N5 console kotlin");
            Utils.LogAndWriteLine("2. 3N5 Android");
            Utils.LogAndWriteLine("3. 4N6 Android");
            Utils.LogAndWriteLine("4. 4N6 Android + Spring");
            Utils.LogAndWriteLine("5. 5N6 flutter");
            Utils.LogAndWriteLine("6. 5N6 flutter + firebase");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "0":
                    await HandleCache();
                    break;
                case "1":
                    await Handle3N5KotlinConsoleAsync();
                    break;
                case "2":
                    await Handle3N5AndroidAsync();
                    break;
                case "3":
                    await Handle4N6AndroidAsync();
                    break;
                case "4":
                    await Handle4N6AndroidSpringAsync();
                    break;
                case "5":
                    await Handle5N6FlutterAsync();
                    break;
                case "6":
                    await Handle5N6FlutterFirebaseAsync();
                    break;
                default:
                    Utils.LogAndWriteLine(
                        "Choix invalide. Veuillez redémarrer le programme et choisir une option valide.");
                    break;
            }

            Utils.LogAndWriteLine("Main arrêté");
            // Keep the console window open
            Utils.LogAndWriteLine("Appuyer sur une touche pour quitter, on a fini ...");
            Console.ReadLine();
        }


        static async Task HandleCache()
        {
            Utils.LogAndWriteLine("Creation de la cache ...");
            var cachePath = "\\\\ed5depinfo\\Logiciels\\Android\\scripts\\cachecache\\";
            var downloadTasks = new[]
            {
                DownloadFileAsync(IDEA_URL, "idea.zip"),
                DownloadFileAsync(STUDIO_URL, "studio.zip"),
                DownloadFileAsync(FLUTTER_SDK, "flutter.zip"),
                DownloadFileAsync(CORRETTO_URL, "corretto.zip"),
                DownloadFileAsync(FLUTTER_PLUGIN_URL_STUDIO, "plugin-flutter-android-studio.zip"),
                DownloadFileAsync(DART_PLUGIN_URL_STUDIO, "plugin-dart-android-studio.zip"),
                DownloadFileAsync(FLUTTER_INTL_PLUGIN_URL_STUDIO, "plugin-flutter-intl-android-studio.zip")
            };

            await Task.WhenAll(downloadTasks);

            string tempcache = "."; // Replace with the actual path to temp cache
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            // delete "android-studio" folder if it exists
            if (Directory.Exists(Path.Combine(tempcache, "android-studio")))
            {
                Directory.Delete(Path.Combine(tempcache, "android-studio"), true);
            }

            Utils.LogAndWriteLine("Creation du Android Studio avec plugins");
            string localTempPath = Path.Combine(tempcache, "studio.zip");
            ZipFile.ExtractToDirectory(localTempPath, Path.Combine(tempcache, "android-studio"));

            localTempPath = Path.Combine(tempcache, "plugin-dart-android-studio.zip");
            ZipFile.ExtractToDirectory(localTempPath,
                Path.Combine(tempcache, "android-studio", "android-studio", "plugins"));
            localTempPath = Path.Combine(tempcache, "plugin-flutter-android-studio.zip");
            ZipFile.ExtractToDirectory(localTempPath,
                Path.Combine(tempcache, "android-studio", "android-studio", "plugins"));
            localTempPath = Path.Combine(tempcache, "plugin-flutter-intl-android-studio.zip");
            ZipFile.ExtractToDirectory(localTempPath,
                Path.Combine(tempcache, "android-studio", "android-studio", "plugins"));
            // create android-studio.7z from the folder with plugins
            await CompressFolderTo7zAsync("android-studio", "android-studio.7z");

            Utils.LogAndWriteLine("Conversions des zip en 7z");
            var convertTasks = new[]
            {
                Utils.ConvertZipTo7zAsync("idea.zip", "idea.7z"),
                Utils.ConvertZipTo7zAsync("corretto.zip", "corretto.7z"),
                Utils.ConvertZipTo7zAsync("flutter.zip", "flutter.7z")
            };

            await Task.WhenAll(convertTasks);
            Utils.LogAndWriteLine("Copie des 7z dans le cache " + cachePath);
            // copy the 7z files to the cache folder
            File.Copy("idea.7z", Path.Combine(cachePath, "idea.7z"), true);
            File.Copy("flutter.7z", Path.Combine(cachePath, "flutter.7z"), true);
            File.Copy("android-studio.7z", Path.Combine(cachePath, "android-studio.7z"), true);
            // get the size of the .gradle folder
            var gradleSize = new DirectoryInfo(Path.Combine(home, ".gradle"))
                .EnumerateFiles("*", SearchOption.AllDirectories).Sum(f => f.Length);
            // get the size in MB of the AppData\Local\Android\Sdk folder
            var sdkSize = new DirectoryInfo(Path.Combine(home, "AppData", "Local", "Android", "Sdk"))
                .EnumerateFiles("*", SearchOption.AllDirectories).Sum(f => f.Length);
            Console.WriteLine("taille de .gradle: " + gradleSize / 1024 / 1024 + " MB");
            Console.WriteLine("taille de Android SDK: " + sdkSize / 1024 / 1024 + " MB");
            Console.WriteLine(
                "Merci de partir Android Studio  creer un projet et le partir sur un emulateur pour constituer le SDK et le .gradle");
            var s = Console.ReadLine();

            Utils.LogAndWriteLine("Creation de la cache finie");
        }

        static async Task Handle3N5KotlinConsoleAsync()
        {
            Utils.LogAndWriteLine("Installation de kotlin (console) 3N5...");
            string ideaZipPath = Path.Combine(localCache, "idea.7z");
            await CopyFileFromNetworkShareAsync(ideaZipPath, "idea.7z");
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string destinationFolder = Path.Combine(desktopPath, "idea");
            await Unzip7zFileAsync(ideaZipPath, destinationFolder);
            //AddToPathEnvironmentVariable("C:\\Program Files\\JetBrains\\idea\\bin");


            await DownloadRepo3N5();
            Utils.LogAndWriteLine("Installation de kotlin (console) 3N5");
        }

        private static async Task DownloadRepo3N5()
        {
            await DownloadRepo(URL_3N5, "3N5");
        }

        private static async Task DownloadRepo4N6()
        {
            await DownloadRepo(URL_4N6, "4N6");
        }

        private static async Task DownloadRepo5N6()
        {
            await DownloadRepo(URL_5N6, "5N6");
        }

        private static async Task DownloadRepoKMB()
        {
            await DownloadRepo(URL_KMB, "KMB");
        }

        private static async Task DownloadRepo(string url, string name)
        {
            // download URL_3N5 to the Desktop and unzip it
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string zipFilePath = Path.Combine(desktopPath, name + ".zip");
            await DownloadFileAsync(url, zipFilePath);
            Utils.LogAndWriteLine("Dézippage de " + zipFilePath + " vers " + desktopPath);
            ZipFile.ExtractToDirectory(zipFilePath, desktopPath);
            // delete the zip file
            File.Delete(zipFilePath);
        }

        static async Task Handle3N5AndroidAsync()
        {
            Utils.LogAndWriteLine("Gestion de 3N5 Android...");
            // Add your specific logic here
            await HandleAndroidSDK();
            await HandleAndroidStudio();
            await DownloadRepo3N5();
            Utils.LogAndWriteLine("3N5 Android fini");
        }

        static async Task Handle4N6AndroidAsync()
        {
            Utils.LogAndWriteLine("Gestion de 4N6 Android...");
            // Add your specific logic here
            await HandleAndroidSDK();
            await HandleAndroidStudio();

            var downloadTasks = new[] { DownloadRepo4N6(), DownloadRepoKMB() };
            await Task.WhenAll(downloadTasks);
            Utils.LogAndWriteLine("4N6 Android arrêté");
        }

        static async Task Handle4N6AndroidSpringAsync()
        {
            Utils.LogAndWriteLine("Installation de 4N6 Android + Spring...");
            await HandleAndroidSDK();
            await HandleAndroidStudio();
            await DownloadRepo4N6();
            await DownloadRepoKMB();
            Utils.LogAndWriteLine("4N6 Android + Spring fini");
        }

        static async Task Handle5N6FlutterAsync()
        {
            Utils.LogAndWriteLine("Gestion de 5N6 flutter...");
            // Add your specific logic here

            await HandleFlutter();
            await DownloadRepo5N6();
            await DownloadRepoKMB();
            Utils.LogAndWriteLine("5N6 Flutter fini");
        }

        // TODO split copy and unzip to start other download while unzipping
        static async Task HandleAndroidSDK()
        {
            Utils.LogAndWriteLine("Installation Android SDK démarré");
            string zipPath = Path.Combine(localCache, "Sdk.7z");
            await CopyFileFromNetworkShareAsync(zipPath, "Sdk.7z");
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string destinationFolder = Path.Combine(desktopPath, "sdk");
            await Unzip7zFileAsync("Sdk.7z", destinationFolder);

            Utils.LogAndWriteLine("Installation Android SDK fini");
        }

        static async Task HandleAndroidStudio()
        {
            Utils.LogAndWriteLine("Installation Android Studio démarré");
            string ideaZipPath = Path.Combine(localCache, "android-studio-plugins.7z");
            await CopyFileFromNetworkShareAsync(ideaZipPath, "android-studio-plugins.7z");
            //await DownloadFileAsync(STUDIO_URL, "studio.zip");
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string destinationFolder = Path.Combine(desktopPath, "android-studio");
            await Unzip7zFileAsync("android-studio-plugins.7z", destinationFolder);
            Utils.LogAndWriteLine("Installation Android Studio fini");
        }

        static async Task HandleFlutter()
        {
            Utils.LogAndWriteLine("Installation Flutter démarré");
            // Add your specific logic here
            await DownloadRepo(FLUTTER_SDK, "flutter");

            // execute "flutter doctor --android-licenses"
            RunCommand("flutter doctor --android-licenses");
            RunCommand("flutter doctor --verbose");
            RunCommand("flutter precache");
            RunCommand("flutter pub global activate devtools");
            // create a fake project to initialize flutter
            RunCommand("flutter create fake_start");
            // cd to the fake project and run "flutter run"
            RunCommand("cd fake_start");
            RunCommand("flutter run");
            Utils.LogAndWriteLine("Installation Flutter  fini");
        }

        public static async Task CompressFolderTo7zAsync(string folderPath, string output7zFilePath)
        {
            string sevenZipPath = @"C:\Program Files\7-Zip\7z.exe"; // Adjust the path if necessary

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = sevenZipPath,
                Arguments = $"a \"{output7zFilePath}\" \"{folderPath}\\*\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = processStartInfo })
            {
                process.Start();
                await process.WaitForExitAsync();
                if (process.ExitCode != 0)
                {
                    throw new Exception($"7-Zip exited with code {process.ExitCode}");
                }
            }
        }



        static void RunCommand(string command)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(processStartInfo))
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception($"Command exited with code {process.ExitCode}");
                }
            }
        }

        static async Task Handle5N6FlutterFirebaseAsync()
        {
            Utils.LogAndWriteLine("Gestion de 5N6 flutter + firebase...");
            // Add your specific logic here

            await DownloadRepo5N6();
            Utils.LogAndWriteLine("5N6 Flutter + Firebase fini");
        }


        public static void AddDesktopToDefenderExclusion()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string command = $"powershell -Command \"Add-MpPreference -ExclusionPath '{desktopPath}'\"";
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(processStartInfo))
                {
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"Command exited with code {process.ExitCode}");
                    }
                }

                Console.WriteLine("Desktop folder added to Windows Defender exclusion list successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void DisableWindowsDefender()
        {
            Utils.LogAndWriteLine("DisableWindowsDefender commencement");
            string command = "powershell -Command \"Set-MpPreference -DisableRealtimeMonitoring $true\"";
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(processStartInfo))
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception($"La commande s'est terminée avec le code {process.ExitCode}");
                }
            }

            Utils.LogAndWriteLine("DisableWindowsDefender fini");
        }

        static async Task DownloadFileAsync(string url, string filePath)
        {
            Utils.LogAndWriteLine("Téléchargement du fichier démarré " + url);
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(10); // Set timeout to 10 minutes
                HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                long totalBytes = response.Content.Headers.ContentLength ?? -1;
                long totalRead = 0L;
                byte[] buffer = new byte[8192];
                int bytesRead;
                double lastReportedProgress = 0;

                using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                       fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192,
                           true))
                {
                    while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                        totalRead += bytesRead;

                        if (totalBytes != -1)
                        {
                            double progress = (double)totalRead / totalBytes * 100;
                            if (progress - lastReportedProgress >= 10)
                            {
                                Utils.LogAndWriteLine($"Téléchargement de {url} : {progress:F1}%");
                                lastReportedProgress = progress;
                            }
                        }
                    }
                }
            }

            Utils.LogAndWriteLine("Téléchargement du fichier fini " + url);
        }

        static void SetEnvironmentVariable(string variable, string value)
        {
            Utils.LogAndWriteLine("SetEnvironmentVariable démarré");
            Environment.SetEnvironmentVariable(variable, value, EnvironmentVariableTarget.User);
            Utils.LogAndWriteLine("SetEnvironmentVariable arrêté");
        }

        static async Task Unzip7zFileAsync(string sourceFile, string destinationFolder)
        {
            Utils.LogAndWriteLine("Dézippage avec 7z commencé pour " + sourceFile + " dans " + destinationFolder);
            try
            {
                string sevenZipPath = @"C:\Program Files\7-Zip\7z.exe";
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = sevenZipPath,
                    Arguments = $"x \"{sourceFile}\" -o\"{destinationFolder}\" -y",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    await process.WaitForExitAsync();
                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"7-Zip s'est terminé avec le code {process.ExitCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogAndWriteLine($"Une erreur est survenue: {ex.Message}");
            }

            Utils.LogAndWriteLine("Dézippage avec 7z fini pour " + sourceFile);
        }

        static void AddToPathEnvironmentVariable(string newPath)
        {
            Utils.LogAndWriteLine("AddToPathEnvironmentVariable démarré");
            string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
            if (!currentPath.Contains(newPath))
            {
                string updatedPath = currentPath + ";" + newPath;
                Environment.SetEnvironmentVariable("PATH", updatedPath, EnvironmentVariableTarget.User);
            }

            Utils.LogAndWriteLine("AddToPathEnvironmentVariable arrêté");
        }

        static async Task CopyFileFromNetworkShareAsync(string networkFilePath, string localFilePath)
        {
            Utils.LogAndWriteLine("Copie du fichier " + networkFilePath + " vers " + localFilePath);
            try
            {
                using (FileStream sourceStream = new FileStream(networkFilePath, FileMode.Open, FileAccess.Read,
                           FileShare.Read, 4096, useAsync: true))
                using (FileStream destinationStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write,
                           FileShare.None, 4096, useAsync: true))
                {
                    await sourceStream.CopyToAsync(destinationStream);
                }
            }
            catch (Exception ex)
            {
                Utils.LogAndWriteLine($"Copie du fichier Une erreur est survenue: {ex.Message}");
            }

            Utils.LogAndWriteLine("Copie du fichier finie");
        }

    }

}