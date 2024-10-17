using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

// quelques choix editoriaux,
// - ne pas creer d'emulateur mais avoir une image system dans le sdk
// - tout mettre au niveau du bureau, vu qu'ils sont fan du bureau les etudiants
// - mesurer l'impact de chaque opti pour voir si ca vaut la peine

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
 * 3 min 8h11 debut SDK
 * 7 min 8h14 debut sync gradle (on parle essentiellement de plein de tout petit telechargement)
 * 2 min 8h21 debut creation emulateur qui doit telecharger une image android 8h23
 * 2 min 8h23 premier run du projet vide 8h25
 * En tout 14 minutes depuis toolbox jusqu'au projet parti avec plusieurs manip
 *
 * Install avec appli. Sdk=on emu=off .gradle=off
 * 2 min 8h31 debut install 8h33
 * 7 min 8h33 sync gradle 8h40
 * 1 min 8h40 creation emulateur (image est deja dans le SDK) 8h41
 * 2 min build et run 8h43
 * En tout 12 minutes
 *
 * Install avec appli. Sdk=on emu=off .gradle=on
 * le gradle sync passe de 7 min a 30 sec
 * 3 minutes pour l'installation
 */

// C:\Users\joris.deguet\AppData\Local\Google\AndroidStudio2024.2

namespace ScriptSharp;

static class Program
{

    private static async Task Main()
    {
        Directory.SetCurrentDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        //clear the log file
        LogSingleton.Get.LogAndWriteLine("Bienvenue dans l'installeur pour les cours de mobile");
        LogSingleton.Get.LogAndWriteLine("ATTENTION DE BIEN ATTENDRE LA FIN DE L'INSTALLATION AVANT D'OUVRIR UN PROJET");
        LogSingleton.Get.LogAndWriteLine("Une fois un projet ouvert, surtout choisir Automatically si on vous propose de configurer Defender");
        LogSingleton.Get.LogAndWriteLine("Un fichier de log de l'installation est dispo sur le bureau, dossier log ");
        if (!Directory.Exists(Config.LocalCache))
        {
            LogSingleton.Get.LogAndWriteLine(
                "Le dossier de cache local n'existe pas. Veuillez vous assurer que le partage réseau est monté et réessayez.");
            LogSingleton.Get.LogAndWriteLine("Main arrêté car le dossier de cache local n'existe pas");
            return;
        }

        LogSingleton.Get.LogAndWriteLine("Veuillez choisir une option:");
        LogSingleton.Get.LogAndWriteLine("1. 3N5 console kotlin");
        LogSingleton.Get.LogAndWriteLine("2. 3N5 Android");
        LogSingleton.Get.LogAndWriteLine("3. 4N6 Android");
        LogSingleton.Get.LogAndWriteLine("4. 4N6 Android + Spring");
        LogSingleton.Get.LogAndWriteLine("5. 5N6 flutter");
        LogSingleton.Get.LogAndWriteLine("6. 5N6 flutter + firebase");
        LogSingleton.Get.LogAndWriteLine("7. supprimer  .gradle SDK .android  bureau");
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
            case "7":
                Utils.Reset();
                break;
            case "9":
                TestDebug();
                break;
            default:
                LogSingleton.Get.LogAndWriteLine(
                    "Choix invalide. Veuillez redémarrer le programme et choisir une option valide.");
                break;
        }
        LogSingleton.Get.LogAndWriteLine("Installation finie");
        LogSingleton.Get.LogAndWriteLine("Appuyer sur la touche Entrée pour quitter, on a fini ...");
        Console.ReadLine();
    }

    private static void TestDebug()
    {
        Utils.CreateDesktopShortcut("gna", "C:\\Program Files\\7-Zip\\7z.exe");
        Utils.CreateDesktopShortcut("gni", "C:\\Program Files\\7-Zip\\plop.exe");
    }

    public static async Task InstallJava()
    {
        LogSingleton.Get.LogAndWriteLine("Installation de Java Dev Kit");
        await Utils.CopyFileFromNetworkShareAsync(
            Path.Combine(Config.LocalCache, "jdk.7z"),
            Path.Combine(Config.LocalTemp, "jdk.7z"));
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string destinationFolder = Path.Combine(desktopPath, "jdk");
        await Utils.Unzip7ZFileAsync(Path.Combine(Config.LocalTemp, "jdk.7z"), destinationFolder);
        string jdkPath = Path.Combine(desktopPath, "jdk");
        DirectoryInfo jdkDirectory = new DirectoryInfo(jdkPath);
        string jdkVersion = jdkDirectory.GetDirectories()[0].Name;
        string javaHome = Path.Combine(jdkPath, jdkVersion);
        Utils.AddToPath(Path.Combine(javaHome, "bin"));
        Environment.SetEnvironmentVariable("JAVA_HOME", javaHome, EnvironmentVariableTarget.User);

        LogSingleton.Get.LogAndWriteLine("    FAIT Installation Java");
    }

    public static async Task DownloadRepoKmb() { await DownloadRepo(Config.UrlKmb, "KMB"); }

    public static async Task DownloadRepo(string url, string name)
    {
        // download URL_3N5 to the Desktop and unzip it
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string zipFilePath = Path.Combine(desktopPath, name + ".zip");
        await Utils.DownloadFileAsync(url, zipFilePath);
        LogSingleton.Get.LogAndWriteLine("Dézippage du repo " + zipFilePath + " vers " + desktopPath);
        ZipFile.ExtractToDirectory(zipFilePath, desktopPath, true);
        try { File.Delete(zipFilePath); }
        catch
        {
            // ignored
        }
    }

    // TODO split copy and unzip to start other download while unzipping
    public static async Task InstallAndroidSdk()
    {
        LogSingleton.Get.LogAndWriteLine("Installation Android SDK démarré");
        string sdkPath = Utils.GetSdkPath();
        string androidSdkRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Android", "Sdk");
        Utils.AddToPath(Path.Combine(androidSdkRoot, "cmdline-tools", "latest", "bin"));
        Utils.AddToPath(Path.Combine(androidSdkRoot, "emulator"));
        // get the parent directory of the SDK path
        string sdkParentPath = Directory.GetParent(sdkPath)?.FullName;
        await Utils.Unzip7ZFileAsync(Path.Combine(Config.LocalTemp, "Sdk.7z"), sdkParentPath);
        // Add environment variables
        SetEnvironmentVariable("ANDROID_SDK_ROOT", androidSdkRoot);
        SetEnvironmentVariable("ANDROID_HOME", androidSdkRoot);

        // Append to PATH

        LogSingleton.Get.LogAndWriteLine("    FAIT Installation Android SDK complet");
    }

    public static async Task InstallAndroidStudio()
    {
        LogSingleton.Get.LogAndWriteLine("Installation Android Studio démarré");
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        Utils.AddToPath(Path.Combine(desktopPath, "android-studio", "bin"));
        string destinationFolder = Path.Combine(desktopPath);
        await Utils.Unzip7ZFileAsync(Path.Combine(Config.LocalTemp, "android-studio.7z"), destinationFolder);
        // TODO add shortcut    

        LogSingleton.Get.LogAndWriteLine("    FAIT Installation Android Studio fini");
    }

    private static void SetEnvironmentVariable(string variable, string value)
    {
        LogSingleton.Get.LogAndWriteLine("SetEnvironmentVariable démarré");
        Environment.SetEnvironmentVariable(variable, value, EnvironmentVariableTarget.User);
        LogSingleton.Get.LogAndWriteLine("SetEnvironmentVariable arrêté");
    }


    public static string PathToAndroidStudio()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "android-studio", "bin", "studio64.exe");
    }

    public static string PathToIntellij()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "idea", "bin", "idea64.exe");
    }
    public static string PathToFlutter()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "flutter", "bin", "flutter");
    }
}