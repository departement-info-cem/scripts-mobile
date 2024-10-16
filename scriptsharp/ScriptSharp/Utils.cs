using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace ScriptSharp;

public class Utils
{
    public static async Task ConvertZipTo7zAsync(string zipFilePath, string output7zFilePath)
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
        LogSingleton.Get.LogAndWriteLine("Conversion de ZIP en 7z fini pour " + zipFilePath);
    }
    
    
    public static readonly object logLock = new object();

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
                            LogSingleton.Get.LogAndWriteLine($"Téléchargement de {url} : {progress:F1}%");
                            lastReportedProgress = progress;
                        }
                    }
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
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(processStartInfo))
            {
                string currentTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string outputFilePath = Path.Combine(Config.logPath, $"Commande-{currentTime}.txt");
                using (StreamWriter writer = new StreamWriter(outputFilePath))
                {
                    writer.WriteLine("Trace de l'exeuction de la commande:");
                    writer.WriteLine(command);
                    writer.WriteLine(process.StandardOutput.ReadToEnd());
                    writer.WriteLine(process.StandardError.ReadToEnd());
                    writer.Close();
                }
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception($"Command exited with code {process.ExitCode}");
                }
            }
        }catch (Exception ex)
        {
            LogSingleton.Get.LogAndWriteLine($"    ERREUR Une erreur est survenue en executant : {command} :: {ex.Message}");
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
        LogSingleton.Get.LogAndWriteLine("Dézippage avec 7z commencé pour " + sourceFile + " dans " + destinationFolder);
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
            LogSingleton.Get.LogAndWriteLine($"Une erreur est survenue: {ex.Message}");
        }

        LogSingleton.Get.LogAndWriteLine("Dézippage avec 7z fini pour " + sourceFile);
    }

    public static async Task CopyFileFromNetworkShareAsync(string networkFilePath, string localFilePath)
    {
        LogSingleton.Get.LogAndWriteLine("Copie du fichier " + networkFilePath + " vers " + localFilePath);
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
            LogSingleton.Get.LogAndWriteLine($"   ERREUR Copie du fichier Une erreur est survenue: {ex.Message}");
        }
        LogSingleton.Get.LogAndWriteLine("    FAIT Copie du fichier "+localFilePath);
    }
    
    public static string GetSDKPath()
    {
        //C:\Users\joris.deguet\AppData\Local\Android\Sdk
        string sdkPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Android", "Sdk");
        Console.WriteLine("Chemin vers le SDK Android: " + sdkPath);
        return sdkPath;
    }

    public static void CreateDesktopShortcut(string shortcutName, string targetPath)
    {
        var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var linkPath = Path.Combine(desktopFolder, $"{shortcutName}.lnk");
        var commande = "$WshShell = New-Object -ComObject WScript.Shell; " +
                       "$Shortcut = $WshShell.CreateShortcut('"+linkPath+"'); " +
                       "$Shortcut.TargetPath = '"+targetPath+"'; " +
                       "$Shortcut.Save();";
        LogSingleton.Get.LogAndWriteLine("Création du raccourci sur le bureau pour " + targetPath);
        //string commande = "Add-Desktop-Shortcut  \""+targetPath+"\"  \""+shortcutName+"\"";
        //LogSingleton.Get.LogAndWriteLine("path "+ commande);
        RunPowerShellCommand(commande);
        LogSingleton.Get.LogAndWriteLine("    FAIT Raccourci ajouté sur le bureau pour " + targetPath);
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
                throw new Exception($"      ERREUR PowerShell erreur avec code {process.ExitCode}");
            }
        }
    }
    
    public static async Task StartIntellij()
    {
        // start android studio
        LogSingleton.Get.LogAndWriteLine("Démarrage d'Intellij IDEA");
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string path = Path.Combine(desktopPath, "idea","bin","idea64.exe");
        CreateDesktopShortcut("Intellij", path);
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
            LogSingleton.Get.LogAndWriteLine("Intellij n'est pas installé");
        }
    }
    public static async Task StartAndroidStudio()
    {
        // start android studio
        LogSingleton.Get.LogAndWriteLine("Lancement d'Android Studio");
        string androidStudioPath = Program.PathToAndroidStudio();
        if (File.Exists(androidStudioPath))
        {
            CreateDesktopShortcut("Android-Studio", Program.PathToAndroidStudio());
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = androidStudioPath,
                UseShellExecute = true
            };
            Process.Start(processStartInfo);
        }
        else
        {
            LogSingleton.Get.LogAndWriteLine("       ERREUR Android Studio n'est pas installé");
        }
    }

    public static void AddToPath(string binPath)
    {
        string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
        if (currentPath == null)
        {
            SetEnvironmentVariable("PATH", binPath); 
        }
        else if (!currentPath.Contains(binPath))
        {
            LogSingleton.Get.LogAndWriteLine("Ajout au Path de " + binPath);
            string updatedPath = currentPath + ";" + binPath;
            SetEnvironmentVariable("PATH", updatedPath);
        }
    }

    public static void RemoveFromPath(string pattern)
    {
        string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
        
        if (currentPath == null) return;
        
        LogSingleton.Get.LogAndWriteLine("Retrait au Path de " + pattern);
        
        string[] currentPathArray = currentPath.Split(";");
        string[] filteredCurrentPathArray = currentPathArray.Where(path => !path.Contains(pattern)).ToArray();
        string updatedPath = string.Join(";", filteredCurrentPathArray);

        SetEnvironmentVariable("PATH", updatedPath);
    }
    
    public static void StartKMB()
    {
        RunCommand(Program.PathToIntellij()+ " " + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "KickMyB-Server-main"));
    }

    public static void Reset()
    {
        DeleteAll();
        RemoveAllEnv();
    }
    
    public static void DeleteAll()
    {
        DeleteThis(GetSDKPath());
        DeleteThis( Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "AppData", "Local", "Android"));
        DeleteThis( Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "AppData", "Local", "Google", "AndroidStudio*"));
        DeleteThis( Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "AppData", "Roaming", "Google", "AndroidStudio*"));
        DeleteThis(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "*")); 
        DeleteThis(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".android"));
        DeleteThis(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".gradle"));
    }
    
    private static void RemoveAllEnv()
    {
        SetEnvironmentVariable("ANDROID_HOME", null);
        SetEnvironmentVariable("JAVA_HOME", null);
        SetEnvironmentVariable("GRADLE_HOME", null);
        SetEnvironmentVariable("ANDROID_SDK_ROOT", null);
        SetEnvironmentVariable("ANDROID_NDK_HOME", null);
        SetEnvironmentVariable("ANDROID_AVD_HOME", null);
        SetEnvironmentVariable("ANDROID_SDK_HOME", null);
        RemoveFromPath(@"C:\Users\po.brillant\Desktop\flutter\bin");
        RemoveFromPath(@"C:\Users\po.brillant\Desktop\android-studio\bin");
        RemoveFromPath(@"C:\Users\po.brillant\AppData\Local\Android\Sdk\emulator");
        RemoveFromPath(@"C:\Users\po.brillant\AppData\Local\Android\Sdk\cmdline-tools\latest\bin");
        RemoveFromPath(@"C:\Users\po.brillant\Desktop\idea\bin");
        RemoveFromPath(@"C:\Users\po.brillant\Desktop\jdk");
    }

    private static void SetEnvironmentVariable(string name, string value)
    {
        Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.User);
        Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Process);
    }
    
    private static void DeleteThis(string path)
    {
        try
        {
            DirectoryInfo parent = Directory.GetParent(path)!;
            string pattern = new DirectoryInfo(path).Name;
            
            foreach (string dir in Directory.GetDirectories(parent.FullName, pattern))
            {
                Directory.Delete(dir, true);
            }

            foreach (string file in Directory.GetFiles(parent.FullName, pattern))
            {
                File.Delete(file);
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Not found: " + path);
        }
    }
}
    
