using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

// TODO try dotnet publish -c Release -r win-x64 --self-contained to build a single exe file and test

namespace ScriptSharp
{
    class Program
    {
        static string localCache = "\\\\ed5depinfo\\cache";
        static string logFilePath = "log.txt";

        // Android Studio
        static string STUDIO_URL = "https://redirector.gvt1.com/edgedl/android/studio/ide-zips/2024.1.1.12/android-studio-2024.1.1.12-windows.zip";
        static string FLUTTER_PLUGIN_URL_STUDIO = "https://plugins.jetbrains.com/plugin/download?rel=true&updateId=582965";
        static string DART_PLUGIN_URL_STUDIO = "https://plugins.jetbrains.com/plugin/download?rel=true&updateId=573248";
        static string FLUTTER_INTL_PLUGIN_URL_STUDIO = "https://plugins.jetbrains.com/plugin/download?rel=true&updateId=541419";

        // Android SDK and sdkmanager
        static string CORRETTO_URL = "https://corretto.aws/downloads/latest/amazon-corretto-17-x64-windows-jdk.zip";

        // IntelliJ
        static string IDEA_URL = "https://download.jetbrains.com/idea/ideaIC-2024.2.1.win.zip";

        // Flutter
        static string FLUTTER_SDK = "https://storage.googleapis.com/flutter_infra_release/releases/stable/windows/flutter_windows_3.24.0-stable.zip";
        static string URL_3N5 = "https://github.com/departement-info-cem/3N5-Prog3/archive/refs/heads/main.zip";
        static string URL_4N6 = "https://github.com/departement-info-cem/4N6-Mobile/archive/refs/heads/master.zip";
        static string URL_5N6 = "https://github.com/departement-info-cem/5N6-mobile-2/archive/refs/heads/main.zip";
        static string URL_KMB = "https://github.com/departement-info-cem/KickMyB-Server/archive/refs/heads/main.zip";

        static Boolean isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        static async Task Main(string[] args)
        {
            //clear the log file
            File.WriteAllText(logFilePath, string.Empty);
            LogAndWriteLine("Main démarré");
            if (!Directory.Exists(localCache) && isWindows) 
            {
                LogAndWriteLine("Le dossier de cache local n'existe pas. Veuillez vous assurer que le partage réseau est monté et réessayez.");
                LogAndWriteLine("Main arrêté car le dossier de cache local n'existe pas");
                return;
            }
            // confirm the local cache folder is accessible
            LogAndWriteLine("Le dossier de cache local est accessible.");
            if (isWindows)
            {
                DisableWindowsDefender();
            }
            LogAndWriteLine("Veuillez choisir une option:");
            LogAndWriteLine("1. 3N5 console kotlin");
            LogAndWriteLine("2. 3N5 Android");
            LogAndWriteLine("3. 4N6 Android");
            LogAndWriteLine("4. 4N6 Android + Spring");
            LogAndWriteLine("5. 5N6 flutter");
            LogAndWriteLine("6. 5N6 flutter + firebase");

            string choice = Console.ReadLine();

            switch (choice)
            {
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
                    LogAndWriteLine("Choix invalide. Veuillez redémarrer le programme et choisir une option valide.");
                    break;
            }
            LogAndWriteLine("Main arrêté");
            // Keep the console window open
            LogAndWriteLine("Appuyer sur une touche pour quitter, on a fini ...");
            Console.ReadLine();
        }

        static async Task Handle3N5KotlinConsoleAsync()
        {
            LogAndWriteLine("Handle3N5KotlinConsoleAsync démarré");
            LogAndWriteLine("Gestion de la console kotlin 3N5...");
            string ideaZipPath = Path.Combine(localCache, "idea.7z");
            await CopyFileFromNetworkShareAsync(ideaZipPath, "idea.7z");
            await Unzip7zFileAsync(ideaZipPath, "C:\\Program Files\\JetBrains\\idea");
            AddToPathEnvironmentVariable("C:\\Program Files\\JetBrains\\idea\\bin");
            
            
            await DownloadRepo3N5();
            LogAndWriteLine("Handle3N5KotlinConsoleAsync arrêté");
        }

        private static async Task DownloadRepo3N5() { await DownloadRepo(URL_3N5, "3N5"); }
        private static async Task DownloadRepo4N6() { await DownloadRepo(URL_4N6, "4N6"); }
        private static async Task DownloadRepo5N6() { await DownloadRepo(URL_5N6, "5N6"); } 
        private static async Task DownloadRepoKMB() { await DownloadRepo(URL_KMB, "KMB"); }

        private static async Task DownloadRepo(string url, string name)
        {
            // download URL_3N5 to the Desktop and unzip it
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string zipFilePath = Path.Combine(desktopPath, name+".zip");
            await DownloadFileAsync(url, zipFilePath);
            LogAndWriteLine("Dézippage de " + zipFilePath + " vers " + desktopPath);
            ZipFile.ExtractToDirectory(zipFilePath, desktopPath);
            // delete the zip file
            File.Delete(zipFilePath);
        }

        static async Task Handle3N5AndroidAsync()
        {
            LogAndWriteLine("Gestion de 3N5 Android...");
            // Add your specific logic here

            await DownloadRepo3N5();
            LogAndWriteLine("3N5 Android fini");
        }

        static async Task Handle4N6AndroidAsync()
        {
            LogAndWriteLine("Gestion de 4N6 Android...");
            // Add your specific logic here
            await HandleAndroidStudio();
            
            var downloadTasks = new[] { DownloadRepo4N6(), DownloadRepoKMB() };
            await Task.WhenAll(downloadTasks);
            LogAndWriteLine("4N6 Android arrêté");
        }

        static async Task Handle4N6AndroidSpringAsync()
        {
            LogAndWriteLine("Gestion de 4N6 Android + Spring...");
            // Add your specific logic here
            await HandleAndroidStudio();
            await DownloadRepo4N6();
            await DownloadRepoKMB();
            LogAndWriteLine("4N6 Android + Spring fini");
        }

        static async Task Handle5N6FlutterAsync()
        {
            LogAndWriteLine("Gestion de 5N6 flutter...");
            // Add your specific logic here

            await HandleFlutter();
            await DownloadRepo5N6();
            await DownloadRepoKMB();
            LogAndWriteLine("5N6 Flutter fini");
        }

        static async Task HandleAndroidStudio()
        {
            LogAndWriteLine("Installation Android Studio démarré");
            // Add your specific logic here
            await DownloadFileAsync(STUDIO_URL, "studio.zip");
            await Unzip7zFileAsync("studio.zip", "C:\\Program Files\\Android Studio");
            AddToPathEnvironmentVariable("C:\\Program Files\\Android Studio\\bin");
            LogAndWriteLine("Installation Android Studio fini");
        }

        static async Task HandleFlutter()
        {
            LogAndWriteLine("Installation Flutter démarré");
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
            LogAndWriteLine("Installation Flutter  fini");
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
            LogAndWriteLine("Gestion de 5N6 flutter + firebase...");
            // Add your specific logic here

            await DownloadRepo5N6();
            LogAndWriteLine("5N6 Flutter + Firebase fini");
        }

        static void DisableWindowsDefender()
        {
            LogAndWriteLine("DisableWindowsDefender démarré");
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
            LogAndWriteLine("DisableWindowsDefender fini");
        }

        static async Task DownloadFileAsync(string url, string filePath)
        {
            LogAndWriteLine("Téléchargement du fichier démarré " + url);
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

                using (Stream contentStream = await response.Content.ReadAsStreamAsync(), fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
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
                                LogAndWriteLine($"Téléchargement de {url} : {progress:F1}%");
                                lastReportedProgress = progress;
                            }
                        }
                    }
                }
            }
            LogAndWriteLine("Téléchargement du fichier fini " + url);
        }

        static void SetEnvironmentVariable(string variable, string value)
        {
            LogAndWriteLine("SetEnvironmentVariable démarré");
            Environment.SetEnvironmentVariable(variable, value, EnvironmentVariableTarget.User);
            LogAndWriteLine("SetEnvironmentVariable arrêté");
        }

        static async Task Unzip7zFileAsync(string sourceFile, string destinationFolder)
        {
            LogAndWriteLine("Dézippage avec 7z commencé pour " + sourceFile + " dans " + destinationFolder);
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
                LogAndWriteLine($"Une erreur est survenue: {ex.Message}");
            }
            LogAndWriteLine("Dézippage avec 7z fini pour " + sourceFile);
        }

        static void AddToPathEnvironmentVariable(string newPath)
        {
            LogAndWriteLine("AddToPathEnvironmentVariable démarré");
            string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
            if (!currentPath.Contains(newPath))
            {
                string updatedPath = currentPath + ";" + newPath;
                Environment.SetEnvironmentVariable("PATH", updatedPath, EnvironmentVariableTarget.User);
            }
            LogAndWriteLine("AddToPathEnvironmentVariable arrêté");
        }

        static async Task CopyFileFromNetworkShareAsync(string networkFilePath, string localFilePath)
        {
            LogAndWriteLine("CopyFileFromNetworkShareAsync démarré");
            try
            {
                using (FileStream sourceStream = new FileStream(networkFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
                using (FileStream destinationStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
                {
                    await sourceStream.CopyToAsync(destinationStream);
                }
            }
            catch (Exception ex)
            {
                LogAndWriteLine($"Une erreur est survenue: {ex.Message}");
            }
            LogAndWriteLine("CopyFileFromNetworkShareAsync arrêté");
        }

        static void LogAndWriteLine(string message)
        {
            Console.WriteLine(message);
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
    }
}