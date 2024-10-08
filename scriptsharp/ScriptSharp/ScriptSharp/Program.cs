﻿using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

// dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true
// permet de generer un seul gros .exe avec tout ce qu'il faut dedans

// TODO creer un projet fake en kotlin pour peupler le .gradle Google "gradle init to create new kotlin project"
// TODO ou la meme chose en maven : https://mvnrepository.com/artifact/org.jetbrains.kotlin/kotlin-archetype-jvm
// https://books.sonatype.com/mvnex-book/reference/simple-project-sect-create-simple.html#:~:text=To%20start%20a%20new%20Maven,will%20use%20the%20archetype%20org.
//  gradle init --type kotlin-application --dsl kotlin --test-framework kotlintest --package ca.cem --project-name fake-kotlin  --no-split-project  --java-version 17

namespace ScriptSharp
{
    class Program
    {
        public static string localCache = "\\\\ed5depinfo\\Logiciels\\Android\\scripts\\cache";
        // Android Studio

        public static string URL_3N5 = "https://github.com/departement-info-cem/3N5-Prog3/archive/refs/heads/main.zip";
        public static string URL_4N6 = "https://github.com/departement-info-cem/4N6-Mobile/archive/refs/heads/master.zip";
        public static string URL_5N6 = "https://github.com/departement-info-cem/5N6-mobile-2/archive/refs/heads/main.zip";
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
                    await CacheCreation.HandleCache();
                    break;
                case "1":
                    await Script3N5.Handle3N5KotlinConsoleAsync();
                    break;
                case "2":
                    await Script3N5.Handle3N5AndroidAsync();
                    break;
                case "3":
                    await Script4N6.Handle4N6AndroidAsync();
                    break;
                case "4":
                    await Script4N6.Handle4N6AndroidSpringAsync();
                    break;
                case "5":
                    await Script5N6.Handle5N6FlutterAsync();
                    break;
                case "6":
                    await Script5N6.Handle5N6FlutterFirebaseAsync();
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


        public static async Task DownloadRepoKMB()
        {
            await DownloadRepo(URL_KMB, "KMB");
        }

        public static async Task DownloadRepo(string url, string name)
        {
            // download URL_3N5 to the Desktop and unzip it
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string zipFilePath = Path.Combine(desktopPath, name + ".zip");
            await Utils.DownloadFileAsync(url, zipFilePath);
            Utils.LogAndWriteLine("Dézippage de " + zipFilePath + " vers " + desktopPath);
            ZipFile.ExtractToDirectory(zipFilePath, desktopPath);
            // delete the zip file
            File.Delete(zipFilePath);
        }

        // TODO split copy and unzip to start other download while unzipping
        public static async Task HandleAndroidSDK()
        {
            Utils.LogAndWriteLine("Installation Android SDK démarré");
            string zipPath = Path.Combine(localCache, "Sdk.7z");
            await Utils.CopyFileFromNetworkShareAsync(zipPath, "Sdk.7z");
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string destinationFolder = Path.Combine(desktopPath, "sdk");
            await Utils.Unzip7zFileAsync("Sdk.7z", destinationFolder);

            Utils.LogAndWriteLine("Installation Android SDK fini");
        }

        public static async Task HandleAndroidStudio()
        {
            Utils.LogAndWriteLine("Installation Android Studio démarré");
            string ideaZipPath = Path.Combine(localCache, "android-studio-plugins.7z");
            await Utils.CopyFileFromNetworkShareAsync(ideaZipPath, "android-studio-plugins.7z");
            //await DownloadFileAsync(STUDIO_URL, "studio.zip");
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string destinationFolder = Path.Combine(desktopPath, "android-studio");
            await Utils.Unzip7zFileAsync("android-studio-plugins.7z", destinationFolder);
            Utils.LogAndWriteLine("Installation Android Studio fini");
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

        static void SetEnvironmentVariable(string variable, string value)
        {
            Utils.LogAndWriteLine("SetEnvironmentVariable démarré");
            Environment.SetEnvironmentVariable(variable, value, EnvironmentVariableTarget.User);
            Utils.LogAndWriteLine("SetEnvironmentVariable arrêté");
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
    }

}