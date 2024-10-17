using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Sharprompt;

// quelques choix editoriaux,
// - ne pas creer d'emulateur mais avoir une image system dans le sdk
// - tout mettre au niveau du bureau, vu qu'ils sont fan du bureau les etudiants
// - mesurer l'impact de chaque opti pour voir si ca vaut la peine

// dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true
// permet de generer un seul gros .exe avec tout ce qu'il faut dedans

// .gradle pour un projet kotlin tout court                 = 2.03 Go
// .gradle pour un projet Android                           = 2.03 Go
// .gradle pour un projet kotlin avec un projet android     = 4.06 Go
// faut croire que c'est la meme taille mais pas les memes librairies

// Sdk de base apres install de Labybug : 5.01 Go
// Sdk de ed5depinfo                    : 7.52 Go

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

        List<string> choixEtudiants =
        [
            "0. Nettoyage", "1. 3N5 console kotlin", "2. 3N5 Android", "3. 4N6 Android", "4. 4N6 Android + Spring", "5. 5N6 flutter", "6. 5N6 flutter + firebase", "7. Quitter"
        ];
        List<string> choixProfs = ["8. Créer la cache"];
        List<string> choix = [];

        choix.AddRange(choixEtudiants);

        if (Environment.MachineName.EndsWith("00"))
        {
            choix.AddRange(choixProfs);
        }

        while (true)
        {
            string choixChoisi = Prompt.Select("Veuillez choisir une option", choix.ToArray());
            
            switch (choixChoisi)
            {
                case not null when choixChoisi.Contains("0."):
                    Utils.Reset();
                    break;
                case not null when choixChoisi.Contains("1."):
                    await Script3N5.Handle3N5KotlinConsoleAsync();
                    break;
                case not null when choixChoisi.Contains("2."):
                    await Script3N5.Handle3N5AndroidAsync();
                    break;
                case not null when choixChoisi.Contains("3."):
                    await Script4N6.Handle4N6AndroidAsync();
                    break;
                case not null when choixChoisi.Contains("4."):
                    await Script4N6.Handle4N6AndroidSpringAsync();
                    break;
                case not null when choixChoisi.Contains("5."):
                    await Script5N6.Handle5N6FlutterAsync();
                    break;
                case not null when choixChoisi.Contains("6."):
                    TestDebug();
                    await Script5N6.Handle5N6FlutterFirebaseAsync();
                    break;
                case not null when choixChoisi.Contains("7."):
                    return;
                case not null when choixChoisi.Contains("8."):
                    await UtilsCacheCreation.HandleCache();
                    break;
            }
        }
    }

    private static void TestDebug()
    {
        UtilsFirebase.InstallFirebase();
    }


}