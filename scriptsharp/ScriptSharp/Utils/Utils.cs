using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace ScriptSharp;

public static class Utils
{
    public static async Task ConvertZipTo7ZAsync(string zipFilePath, string output7ZFilePath)
    {
        LogSingleton.Get.LogAndWriteLine("Conversion de ZIP en 7z commencée pour " + zipFilePath);
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
                Arguments = $"a \"{output7ZFilePath}\" \"{tempDir}\\*\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = new Process();

            process.StartInfo = processStartInfo;
            process.Start();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception($"7-Zip exited with code {process.ExitCode}");
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
        LogSingleton.Get.LogAndWriteLine("Conversion de ZIP en 7z fini pour " + zipFilePath);
    }

    public static async Task DownloadFileAsync(string url, string filePath)
    {
        LogSingleton.Get.LogAndWriteLine("Téléchargement du fichier démarré " + url);
        using (HttpClient client = new HttpClient())
        {
            client.Timeout = TimeSpan.FromMinutes(10); // Set timeout to 10 minutes
            HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            long totalBytes = response.Content.Headers.ContentLength ?? -1;
            long totalRead = 0L;
            byte[] buffer = new byte[8192];
            double lastReportedProgress = 0;
            await using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                         fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192,
                             true))
            {
                int bytesRead;
                while ((bytesRead = await contentStream.ReadAsync(buffer)) > 0)
                {
                    await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
                    totalRead += bytesRead;
                    if (totalBytes == -1) continue;
                    double progress = (double)totalRead / totalBytes * 100;

                    if (!(progress - lastReportedProgress >= 10)) continue;

                    LogSingleton.Get.LogAndWriteLine($"Téléchargement de {url} : {progress:F1}%");
                    lastReportedProgress = progress;
                }
            }
        }
        LogSingleton.Get.LogAndWriteLine("    FAIT Téléchargement du fichier " + url);
    }

    public static void RunCommand(string command)
    {
        LogSingleton.Get.LogAndWriteLine("Execution de la commande: " + command);
        try
        {
            // Récupérer la variable d'environnement PATH
            //string pathVariable = Environment.GetEnvironmentVariable("PATH");
            //Console.WriteLine("La variable d'environnement PATH est:");
            //Console.WriteLine(pathVariable);

            // Configuration de ProcessStartInfo
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "C:\\Windows\\system32\\cmd.exe",
                Arguments = $"/c {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Exécution du processus
            using (Process process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                
                var output = new List<string>();
                var err = new List<string>();
                
                process.OutputDataReceived += (sender, args) =>
                {
                    output.Add(args.Data);
                };
                process.ErrorDataReceived += (sender, args) =>
                {
                    err.Add(args.Data);
                };
                
                process.WaitForExit();
                
                if (output.Count > 0)
                {
                    LogSingleton.Get.LogAndWriteLine("   FAIT Sortie pour  : " + command);
                    foreach (string line in output)
                    {
                        LogSingleton.Get.LogAndWriteLine(line);
                    }
                   
                }
                if (err.Count > 0)
                {
                    LogSingleton.Get.LogAndWriteLine("   ERREUR Sortie pour  : " + command);
                    foreach (string line in err)
                    {
                        LogSingleton.Get.LogAndWriteLine(line);
                    }
                }
                
            }
        }
        catch (Exception ex)
        {
            LogSingleton.Get.LogAndWriteLine($"    ERREUR Une erreur est survenue en executant : {command} :: {ex.Message}");
        }
    }

    public static async Task CompressFolderTo7ZAsync(string folderPath, string output7ZFilePath)
    {
        string sevenZipPath = @"C:\Program Files\7-Zip\7z.exe"; // Adjust the path if necessary

        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = sevenZipPath,
            Arguments = $"a \"{output7ZFilePath}\" \"{folderPath}\\*\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = Environment.CurrentDirectory
        };

        using Process process = new Process();

        process.StartInfo = processStartInfo;
        process.Start();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new Exception($"7-Zip exited with code {process.ExitCode}");
        }
    }
    
    
    public static async Task CompressFolderMonoBlocTo7ZAsync(string folderPath, string output7ZFilePath)
    {
        string sevenZipPath = @"C:\Program Files\7-Zip\7z.exe"; // Adjust the path if necessary

        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = sevenZipPath,
            Arguments = $"a \"{output7ZFilePath}\" \"{folderPath}\"",
            RedirectStandardOutput = false,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = Environment.CurrentDirectory
        };

        using Process process = new Process();

        process.StartInfo = processStartInfo;
        process.Start();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new Exception($"7-Zip exited with code {process.ExitCode}");
        }
    }

    public static async Task Unzip7ZFileAsync(string sourceFile, string destinationFolder)
    {
        LogSingleton.Get.LogAndWriteLine("Dézippage avec 7z commencé pour " + sourceFile + " dans " + destinationFolder);
        try
        {
            const string sevenZipPath = @"C:\Program Files\7-Zip\7z.exe";
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = sevenZipPath,
                Arguments = $"x \"{sourceFile}\" -o\"{destinationFolder}\" -y",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = new Process();

            process.StartInfo = processStartInfo;
            process.Start();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception($"7-Zip s'est terminé avec le code {process.ExitCode}");
            }
        }
        catch (Exception ex)
        {
            LogSingleton.Get.LogAndWriteLine($"Une erreur est survenue: {ex.Message}");
        }

        LogSingleton.Get.LogAndWriteLine("    FAIT Dézippage 7z fini pour " + sourceFile);
    }

    public static async Task CopyFileFromNetworkShareAsync(string networkFilePath, string localFilePath)
    {
        LogSingleton.Get.LogAndWriteLine("Copie du fichier " + networkFilePath + " vers " + localFilePath);
        try
        {
            await using FileStream sourceStream = new FileStream(networkFilePath, FileMode.Open, FileAccess.Read,
                FileShare.Read, 4096, useAsync: true);

            await using FileStream destinationStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write,
                FileShare.None, 4096, useAsync: true);
            await sourceStream.CopyToAsync(destinationStream);
        }
        catch (Exception ex)
        {
            LogSingleton.Get.LogAndWriteLine($"   ERREUR Copie du fichier Une erreur est survenue: {ex.Message}");
        }
        LogSingleton.Get.LogAndWriteLine("    FAIT Copie du fichier " + localFilePath);
    }

    public static string GetSdkPath()
    {
        string sdkPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Android", "Sdk");
        LogSingleton.Get.LogAndWriteLine("Chemin vers le SDK Android: " + sdkPath);
        return sdkPath;
    }

    public static void CreateDesktopShortcut(string shortcutName, string targetPath)
    {
        try
        {
            string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string linkPath = Path.Combine(desktopFolder, $"{shortcutName}.lnk");
            string commande = "$WshShell = New-Object -ComObject WScript.Shell; " +
                              "$Shortcut = $WshShell.CreateShortcut('" +
                              linkPath +
                              "'); " +
                              "$Shortcut.TargetPath = '" +
                              targetPath +
                              "'; " +
                              "$Shortcut.Save();";
            LogSingleton.Get.LogAndWriteLine("Création du raccourci sur le bureau pour " + targetPath);
            RunPowerShellCommand(commande);
            LogSingleton.Get.LogAndWriteLine("    FAIT Raccourci ajouté sur le bureau pour " + targetPath);
        }
        catch
        {
            LogSingleton.Get.LogAndWriteLine("    ERREUR Raccourci pour " + targetPath);
        }
    }

    private static void RunPowerShellCommand(string command)
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = "C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe",
            Arguments = $"-Command \"{command}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process process = Process.Start(processStartInfo)!;

        process.OutputDataReceived += (_, e) => LogSingleton.Get.LogAndWriteLine(e.Data);
        process.BeginOutputReadLine();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"      ERREUR PowerShell erreur avec code {process.ExitCode}");
        }
    }

    public static void AddToPath(string binPath)
    {
        string currentPath = Environment.GetEnvironmentVariable("PATH");
        if (string.IsNullOrWhiteSpace(currentPath))
        {
            SetEnvVariable("PATH", binPath);
        }
        else if (!currentPath.Split(';').Contains(binPath, StringComparer.OrdinalIgnoreCase))
        {
            LogSingleton.Get.LogAndWriteLine("Ajout au Path de " + binPath);
            string updatedPath = binPath + ";" + currentPath;
            SetEnvVariable("PATH", updatedPath);
        }
    }

    public static void RemoveFromPath(string pattern)
    {
        string currentPath = Environment.GetEnvironmentVariable("PATH");

        if (currentPath == null) return;

        LogSingleton.Get.LogAndWriteLine("Retrait au Path de " + pattern);

        string[] currentPathArray = currentPath.Split(";");
        string[] filteredCurrentPathArray = currentPathArray.Where(path => !path.Contains(pattern)).ToArray();
        string updatedPath = string.Join(";", filteredCurrentPathArray);
        SetEnvVariable("PATH", updatedPath);
        
    }

    public static void StartKmb()
    {
        RunCommand(UtilsIntellij.PathToIntellij() + " " + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "KickMyB-Server-main"));
    }

    public static void Reset()
    {
        RemoveAllEnv();
        DeleteAll();
    }

    private static void DeleteAll()
    {
        DeleteThis(GetSdkPath());
        DeleteThis(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "AppData", "Local", "Android"));
        DeleteThis(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "AppData", "Local", "Google", "AndroidStudio*"));
        DeleteThis(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "AppData", "Roaming", "Google", "AndroidStudio*"));
        DeleteThis(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "*"));
        DeleteThis(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".android"));
        DeleteThis(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".gradle"));
    }

    private static void RemoveAllEnv()
    {
        SetEnvVariable("ANDROID_HOME", null);
        SetEnvVariable("JAVA_HOME", null);
        SetEnvVariable("GRADLE_HOME", null);
        SetEnvVariable("ANDROID_SDK_ROOT", null);
        SetEnvVariable("ANDROID_NDK_HOME", null);
        SetEnvVariable("ANDROID_AVD_HOME", null);
        SetEnvVariable("ANDROID_SDK_HOME", null);
        RemoveFromPath(@"Desktop\flutter\bin");
        RemoveFromPath(@"Desktop\android-studio\bin");
        RemoveFromPath(@"Desktop\rider\bin");
        RemoveFromPath(@"AppData\Local\Android\Sdk\emulator");
        RemoveFromPath(@"AppData\Local\Android\Sdk\cmdline-tools\latest\bin");
        RemoveFromPath(@"Desktop\idea\bin");
        RemoveFromPath(@"Desktop\jdk");
    }

    public static void SetEnvVariable(string name, string value)
    {
        Environment.SetEnvironmentVariable(name, value);
        Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.User);
        Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Process);
    }

    private static void DeleteThis(string path)
    {
        DirectoryInfo parent = Directory.GetParent(path)!;

        if (!Directory.Exists(parent.FullName)) return;

        string pattern = new DirectoryInfo(path).Name;

        foreach (string dir in Directory.GetDirectories(parent.FullName, pattern))
        {
            try
            {
                Directory.Delete(dir, true);
                LogSingleton.Get.LogAndWriteLine(dir + " supprimé");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("used by another process"))
                {
                    LogSingleton.Get.LogAndWriteLine("Le fichier est en  : " + dir);
                }
                else
                {
                    LogSingleton.Get.LogAndWriteLine("Impossible de supprimer : " + dir);
                }
            }
        }

        foreach (string file in Directory.GetFiles(parent.FullName, pattern))
        {
            try
            {
                File.Delete(file);
                LogSingleton.Get.LogAndWriteLine(file + " supprimé");
            }
            catch (Exception)
            {
                LogSingleton.Get.LogAndWriteLine("Impossible de supprimer : " + file);
            }
        }
    }

    public static async Task DownloadRepo(string url, string name)
    {
        // download URL_3N5 to the Desktop and unzip it
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string zipFilePath = Path.Combine(desktopPath, name + ".zip");
        await DownloadFileAsync(url, zipFilePath);
        LogSingleton.Get.LogAndWriteLine("Dézippage du repo " + zipFilePath + " vers " + desktopPath);
        ZipFile.ExtractToDirectory(zipFilePath, desktopPath, true);
        try { File.Delete(zipFilePath); }
        catch
        {
            // ignored
        }
    }

    public static async Task DownloadRepoKmb() { await DownloadRepo(Config.UrlKmb, "KMB"); }

    public static void CopyMachinePath()
    {
        string[] machinePaths = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine)!.Split(";");
        string[] userPaths = Environment.GetEnvironmentVariable("Path")!.Split(";");
        string[] allPaths = machinePaths.Concat(userPaths).ToArray();
        string joinedPaths = string.Join(";", allPaths);
        SetEnvVariable("Path", joinedPaths);
    }
}