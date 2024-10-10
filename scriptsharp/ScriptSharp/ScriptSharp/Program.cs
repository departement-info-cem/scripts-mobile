using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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

        private static string URL_3N5 = "https://github.com/departement-info-cem/3N5-Prog3/archive/refs/heads/main.zip";

        private static string URL_4N6 =
            "https://github.com/departement-info-cem/4N6-Mobile/archive/refs/heads/master.zip";

        private static string URL_5N6 =
            "https://github.com/departement-info-cem/5N6-mobile-2/archive/refs/heads/main.zip";

        private static string URL_KMB =
            "https://github.com/departement-info-cem/KickMyB-Server/archive/refs/heads/main.zip";

        static Boolean isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        static async Task Main(string[] args)
        {
            //clear the log file
            File.WriteAllText(logFilePath, string.Empty);
            LogAndWriteLine("Main started");
            if (!Directory.Exists(localCache) && isWindows) // TODO remove false
            {
                LogAndWriteLine("The local cache folder does not exist. Please make sure the network share is mounted and try again.");
                LogAndWriteLine("Main stopped because the local cache folder does not exist");
                return;
            }
            // confirm the local cache folder is accessible
            LogAndWriteLine("The local cache folder is accessible.");
            if (isWindows)
            {
                DisableWindowsDefender();
            }
            LogAndWriteLine("Please choose an option:");
            LogAndWriteLine("1. 3N5 kotlin console");
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
                    LogAndWriteLine("Invalid choice. Please restart the program and choose a valid option.");
                    break;
            }
            LogAndWriteLine("Main stopped");
            // Keep the console window open
            LogAndWriteLine("Appuyer sur une touche pour quitter, on a fini ...");
            Console.ReadLine();
        }

        static async Task Handle3N5KotlinConsoleAsync()
        {
            LogAndWriteLine("Handle3N5KotlinConsoleAsync started");
            LogAndWriteLine("Handling 3N5 kotlin console...");
            string ideaZipPath = Path.Combine(localCache, "idea.7z");
            await CopyFileFromNetworkShareAsync(ideaZipPath, "idea.7z");
            await Unzip7zFileAsync(ideaZipPath, "C:\\Program Files\\JetBrains\\idea");
            AddToPathEnvironmentVariable("C:\\Program Files\\JetBrains\\idea\\bin");

            await DownloadRepo3N5();
            LogAndWriteLine("Handle3N5KotlinConsoleAsync stopped");
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

        private static async Task DownloadRepo(string url, string name)
        {
            // download URL_3N5 to the Desktop and unzip it
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string zipFilePath = Path.Combine(desktopPath, name+".zip");
            await DownloadFileAsync(url, zipFilePath);
            string destinationFolder = Path.Combine(desktopPath, name);
            // use the standard library function to unzip the file
            LogAndWriteLine(destinationFolder + " < " + zipFilePath);
            ZipFile.ExtractToDirectory(zipFilePath, destinationFolder);
        }

        static async Task Handle3N5AndroidAsync()
        {
            LogAndWriteLine("Handle3N5AndroidAsync started");
            LogAndWriteLine("Handling 3N5 Android...");
            // Add your specific logic here

            await DownloadRepo3N5();
            LogAndWriteLine("Handle3N5AndroidAsync stopped");
        }

        static async Task Handle4N6AndroidAsync()
        {
            LogAndWriteLine("Handle4N6AndroidAsync started");
            LogAndWriteLine("Handling 4N6 Android...");
            // Add your specific logic here

            await DownloadRepo4N6();
            LogAndWriteLine("Handle4N6AndroidAsync stopped");
        }

        static async Task Handle4N6AndroidSpringAsync()
        {
            LogAndWriteLine("Handle4N6AndroidSpringAsync started");
            LogAndWriteLine("Handling 4N6 Android + Spring...");
            // Add your specific logic here

            await DownloadRepo4N6();
            LogAndWriteLine("Handle4N6AndroidSpringAsync stopped");
        }

        static async Task Handle5N6FlutterAsync()
        {
            LogAndWriteLine("Handle5N6FlutterAsync started");
            LogAndWriteLine("Handling 5N6 flutter...");
            // Add your specific logic here

            await DownloadRepo5N6();
            LogAndWriteLine("Handle5N6FlutterAsync stopped");
        }

        static async Task Handle5N6FlutterFirebaseAsync()
        {
            LogAndWriteLine("Handle5N6FlutterFirebaseAsync started");
            LogAndWriteLine("Handling 5N6 flutter + firebase...");
            // Add your specific logic here

            await DownloadRepo5N6();
            LogAndWriteLine("Handle5N6FlutterFirebaseAsync stopped");
        }

        static void DisableWindowsDefender()
        {
            LogAndWriteLine("DisableWindowsDefender started");
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
                    throw new Exception($"Command exited with code {process.ExitCode}");
                }
            }
            LogAndWriteLine("DisableWindowsDefender stopped");
        }

        static async Task DownloadFileAsync(string url, string filePath)
        {
            LogAndWriteLine("DownloadFileAsync started");
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(10); // Set timeout to 10 minutes
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(filePath, fileBytes);
            }
            LogAndWriteLine("DownloadFileAsync stopped");
        }

        static void SetEnvironmentVariable(string variable, string value)
        {
            LogAndWriteLine("SetEnvironmentVariable started");
            Environment.SetEnvironmentVariable(variable, value, EnvironmentVariableTarget.User);
            LogAndWriteLine("SetEnvironmentVariable stopped");
        }

        static async Task Unzip7zFileAsync(string sourceFile, string destinationFolder)
        {
            LogAndWriteLine("Unzip7zFileAsync started");
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
                        throw new Exception($"7-Zip exited with code {process.ExitCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndWriteLine($"An error occurred: {ex.Message}");
            }
            LogAndWriteLine("Unzip7zFileAsync stopped");
        }

        static void AddToPathEnvironmentVariable(string newPath)
        {
            LogAndWriteLine("AddToPathEnvironmentVariable started");
            string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
            if (!currentPath.Contains(newPath))
            {
                string updatedPath = currentPath + ";" + newPath;
                Environment.SetEnvironmentVariable("PATH", updatedPath, EnvironmentVariableTarget.User);
            }
            LogAndWriteLine("AddToPathEnvironmentVariable stopped");
        }

        static async Task CopyFileFromNetworkShareAsync(string networkFilePath, string localFilePath)
        {
            LogAndWriteLine("CopyFileFromNetworkShareAsync started");
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
                LogAndWriteLine($"An error occurred: {ex.Message}");
            }
            LogAndWriteLine("CopyFileFromNetworkShareAsync stopped");
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