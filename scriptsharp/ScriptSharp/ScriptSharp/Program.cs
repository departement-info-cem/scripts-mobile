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


// .gradle pour un projet kotlin tout court                 = 2.03 Go
// .gradle pour un projet Android                           = 2.03 Go
// .gradle pour un projet kotlin avec un projet android     = 4.06 Go
// faut croire que c'est la meme taille mais pas les memes librairies

// Sdk de base apres install de Labybug : 5.01 Go
// Sdk de ed5depinfo                    : 7.52 Go

// TODO https://www.jetbrains.com/help/idea/install-plugins-from-the-command-line.html

// TODO bug pour les shortcut 
// TODO mettre le Sdk a la bonne place

/** Install JetBrains 8h10 debut
 * 1 min 8h11 debut SDK
 * 7 min 8h14 debut sync gradle (on parle essentiellement de plein de tout petit telechargement)
 * 2 min 8h21 debut creation emulateur qui doit telecharger une image android 8h23
 * 2 min 8h23 premier run du projet vide 8h25
 * En tout 15 minutes depuis toolbox jusqu'au projet parti avec plusieurs manip
 *
 * Install avec appli.
 */   

namespace ScriptSharp
{
    class Program
    {
        static Boolean isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        static async Task Main(string[] args)
        {
            //clear the log file
            File.WriteAllText(Utils.logFilePath, string.Empty);
            Utils.LogAndWriteLine("Bienvenue dans l'installeur pour les cours de mobile");
            
            if (!Directory.Exists(Config.localCache) && isWindows)
            {
                Utils.LogAndWriteLine(
                    "Le dossier de cache local n'existe pas. Veuillez vous assurer que le partage réseau est monté et réessayez.");
                Utils.LogAndWriteLine("Main arrêté car le dossier de cache local n'existe pas");
                return;
            }
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
            Utils.LogAndWriteLine("7. supprimer le SDK Android");
            Utils.LogAndWriteLine("8. supprimer le .gradle");

            string choice = Console.ReadLine();
            await InstallJava();
            switch (choice)
            {
                case "0": await CacheCreation.HandleCache(); break;
                case "1": await Script3N5.Handle3N5KotlinConsoleAsync(); break;
                case "2": await Script3N5.Handle3N5AndroidAsync(); break;
                case "3": await Script4N6.Handle4N6AndroidAsync(); break;
                case "4": await Script4N6.Handle4N6AndroidSpringAsync(); break;
                case "5": await Script5N6.Handle5N6FlutterAsync(); break;
                case "6": await Script5N6.Handle5N6FlutterFirebaseAsync(); break;
                case "7": Utils.deleteSDK(); break;
                case "8": Utils.DeleteGradle(); break;
                default:
                    Utils.LogAndWriteLine(
                        "Choix invalide. Veuillez redémarrer le programme et choisir une option valide.");
                    break;
            }
            Utils.LogAndWriteLine("Installation finie");
            Utils.LogAndWriteLine("Appuyer sur une touche 2 fois pour quitter, on a fini ...");
            Console.ReadLine();
        }

        private static async Task InstallJava()
        {
            Utils.LogAndWriteLine("Copie de Java commencee");
            string javaPath = Path.Combine(Config.localCache, "jdk.7z");
            await Utils.CopyFileFromNetworkShareAsync(javaPath, "jdk.7z");
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string destinationFolder = Path.Combine(desktopPath, "jdk");
            await Utils.Unzip7zFileAsync("jdk.7z", destinationFolder);
            Utils.LogAndWriteLine("Copie de Java finie");
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string jdkPath = Path.Combine(desktop, "jdk");
            DirectoryInfo jdkDirectory = new DirectoryInfo(jdkPath);
            string jdkVersion = jdkDirectory.GetDirectories()[0].Name;
            string javaHome = Path.Combine(jdkPath, jdkVersion);
            Environment.SetEnvironmentVariable("JAVA_HOME", javaHome, EnvironmentVariableTarget.User);
            Utils.AddToPath(Path.Combine(javaHome, "bin"));
        }

        public static async Task DownloadRepoKMB() { await DownloadRepo(Config.URL_KMB, "KMB"); }

        public static async Task DownloadRepo(string url, string name)
        {
            // download URL_3N5 to the Desktop and unzip it
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string zipFilePath = Path.Combine(desktopPath, name + ".zip");
            await Utils.DownloadFileAsync(url, zipFilePath);
            Utils.LogAndWriteLine("Dézippage du repo " + zipFilePath + " vers " + desktopPath);
            ZipFile.ExtractToDirectory(zipFilePath, desktopPath, true);
            File.Delete(zipFilePath);
        }

        // TODO split copy and unzip to start other download while unzipping
        public static async Task HandleAndroidSDK()
        {
            Utils.LogAndWriteLine("Installation Android SDK démarré");
            string sdkPath = Utils.GetSDKPath();
            // get the parent directory of the SDK path
            string sdkParentPath = Directory.GetParent(sdkPath).FullName;
            await Utils.Unzip7zFileAsync("Sdk.7z", sdkParentPath);
            Utils.LogAndWriteLine("Installation Android SDK fini");
        }

        public static async Task HandleAndroidStudio()
        {
            Utils.LogAndWriteLine("Installation Android Studio démarré");
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string destinationFolder = Path.Combine(desktopPath);
            await Utils.Unzip7zFileAsync("android-studio.7z", destinationFolder);
            // TODO add shortcut    
            Utils.CreateDesktopShortcut("Android-Studio", Path.Combine(desktopPath, "android-studio", "bin", "studio64.exe"));
            Utils.AddToPath(Path.Combine(desktopPath, "android-studio", "bin"));
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
            { Console.WriteLine($"An error occurred: {ex.Message}"); }
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