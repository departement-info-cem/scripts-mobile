﻿using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;


namespace ScriptSharp;

public class Utils
{
    public static async Task ConvertZipTo7zAsync(string zipFilePath, string output7zFilePath)
    {
        LogAndWriteLine("Conversion de ZIP en 7z commencée pour " + zipFilePath);
        string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        try
        {
            // Extract ZIP file to temporary directory
            ZipFile.ExtractToDirectory(zipFilePath, tempDir);

            // Create 7z archive from the temporary directory
            string sevenZipPath = @"C:\Program Files\7-Zip\7z.exe";
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = sevenZipPath,
                Arguments = $"a \"{output7zFilePath}\" \"{tempDir}\\*\"",
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
        finally
        {
            // Delete the temporary directory
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
        LogAndWriteLine("Conversion de ZIP en 7z fini pour " + zipFilePath);
    }
    
    
    public static readonly object logLock = new object();
    public static string logFilePath = "log.txt";
    
    public static void LogAndWriteLine(string message)
    {
        lock (logLock)
        {
            Console.WriteLine(message);
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
    }

    public static async Task DownloadFileAsync(string url, string filePath)
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
                            LogAndWriteLine($"Téléchargement de {url} : {progress:F1}%");
                            lastReportedProgress = progress;
                        }
                    }
                }
            }
        }
        LogAndWriteLine("Téléchargement du fichier fini " + url);
    }

    public static void RunCommand(string command)
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
            process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            process.BeginOutputReadLine();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception($"Command exited with code {process.ExitCode}");
            }
        }
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

    public static async Task Unzip7zFileAsync(string sourceFile, string destinationFolder)
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

    public static async Task CopyFileFromNetworkShareAsync(string networkFilePath, string localFilePath)
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
    
    public static string GetSDKPath()
    {
        //C:\Users\joris.deguet\AppData\Local\Android\Sdk
        string sdkPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Android", "Sdk");
        Console.WriteLine("Chemin vers le SDK Android: " + sdkPath);
        return sdkPath;
    }
    
    public static async Task InstallGradleAsync(string gradleVersion, string installPath)
    {
        LogAndWriteLine("Installation de Gradle commencée");
        string gradleUrl = $"https://services.gradle.org/distributions/gradle-{gradleVersion}-bin.zip";
        string zipFilePath = Path.Combine(Path.GetTempPath(), $"gradle-{gradleVersion}-bin.zip");
        string extractPath = Path.Combine(installPath);

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(gradleUrl);
            response.EnsureSuccessStatusCode();
            using (FileStream fs = new FileStream(zipFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await response.Content.CopyToAsync(fs);
            }
        }
        // if extractPath exists, delete it with all its contents
        var targetDirectory = Path.Combine(extractPath, $"gradle-{gradleVersion}");
        
        ZipFile.ExtractToDirectory(zipFilePath, extractPath, true);
        File.Delete(zipFilePath);

        string gradleBinPath = Path.GetFullPath(Path.Combine(extractPath, $"gradle-{gradleVersion}", "bin"));
        string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
        LogAndWriteLine("gradle bin path: " + gradleBinPath);
        if (!currentPath.Contains(gradleBinPath))
        {
            string updatedPath = currentPath + ";" + gradleBinPath;
            Environment.SetEnvironmentVariable("PATH", updatedPath, EnvironmentVariableTarget.User);
        }

        LogAndWriteLine($"Gradle {gradleVersion} installed successfully at {extractPath}");
    }

    public static void CreateDesktopShortcut(string shortcutName, string targetPath)
    {
        var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var linkPath = Path.Combine(desktopFolder, $"{shortcutName}.lnk");
        var commande = "$WshShell = New-Object -ComObject WScript.Shell; " +
                       "$Shortcut = $WshShell.CreateShortcut('"+linkPath+"'); " +
                       "$Shortcut.TargetPath = '"+targetPath+"'; " +
                       "$Shortcut.Save();";
        LogAndWriteLine("Création du raccourci sur le bureau pour " + targetPath);
        //string commande = "Add-Desktop-Shortcut  \""+targetPath+"\"  \""+shortcutName+"\"";
        //LogAndWriteLine("path "+ commande);
        RunPowerShellCommand(commande);
        LogAndWriteLine("Raccourci ajouté sur le bureau pour " + targetPath);
    }
    
    public static void RunPowerShellCommand(string command)
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-Command \"{command}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(processStartInfo))
        {
            process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            process.BeginOutputReadLine();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new Exception($"PowerShell command exited with code {process.ExitCode}");
            }
        }
    }
    
    public static void DeleteGradle()
    {
        LogAndWriteLine("Suppression du .gradle commencee");
        // delete the .gradle folder in the user's home directory
        string gradlePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".gradle");
        if (Directory.Exists(gradlePath))
        {
            Directory.Delete(gradlePath, true);
        }
        LogAndWriteLine("Suppression du .gradle finie");
    }

    public static void deleteSDK()
    {
        Utils.LogAndWriteLine("Suppression du SDK Android démarrée");
        string sdkPath = Utils.GetSDKPath();
        if (Directory.Exists(sdkPath))
        {
            Directory.Delete(sdkPath, true);
            Utils.LogAndWriteLine("Suppression du SDK Android finie");
        }
        else { Utils.LogAndWriteLine("Le SDK Android n'existe pas."); }
    }

    
    public static async Task StartIntellij()
    {
        // start android studio
        LogAndWriteLine("Démarrage d'Intellij IDEA");
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string path = Path.Combine(desktopPath, "idea","bin","idea64.exe");
        if (File.Exists(path))
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            };
            Process.Start(processStartInfo);
        }
        else
        {
            LogAndWriteLine("Intellij n'est pas installé");
        }
    }
    public static async Task StartAndroidStudio()
    {
        // start android studio
        LogAndWriteLine("Démarrage d'Android Studio");
        // Path to Android Studio executable is Desktop/android-studio/bin/studio64.exe
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string androidStudioPath = Path.Combine(desktopPath, "android-studio","bin","studio64.exe");
        if (File.Exists(androidStudioPath))
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = androidStudioPath,
                UseShellExecute = true
            };
            Process.Start(processStartInfo);
        }
        else
        {
            LogAndWriteLine("Android Studio n'est pas installé");
        }
    }
}
    
