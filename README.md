# Scripts d'installation des IDE pour le mobile

Les scripts suivants permettent d'installer:
- Android Studio et les différents plugins
- Le SDK d'Android et le mettre à jour
- Intellij Idea et les différents plugins
- Le SDK de Flutter

## Procédure

1. Télécharger le script suivant : [script PowerShell](https://raw.githubusercontent.com/departement-info-cem/scripts-mobile/main/installation-mobile.ps1 "download")
2. Si le fichier s'ouvre dans le navigateur, faire CTRL+S pour l'enregistrer, ou cliquer droit sur le lien et faire "Enregistrer sous"
3. Cliquer droit sur le fichier téléchargé
4. Choisir "Exécuter avec PowerShell"

## Diagramme
```mermaid
flowchart TD
    clone["Clone repo"]
    dJDK["Get JDK"]
    unzipJDK["Unzip and Install JDK"]
    dSDK["Get SDK"]
    unzipSDK["Unzip and install SDK"]
    dIDEA["Get Intellij"]
    unzipIDEA["Unzip and install Intellij"]
    dAS["Get Android Studio"]
    unzipAS["Unzip Android Studio"]
    startandroidstudio["Partir Android Studio"]
    dSDK["Get Android SDK"]
    unzipSDK["Unzip Android SDK"]
    dFLUTTER["Download Flutter"]
    unzipFLUTTER["Unzip & Install Flutter"]
    repos["Download repos 3N5 4N6 5N6 serveur"]
    projetflutter["Première run Flutter"]
    emulator["Lancer émulateur"]
    subgraph téléchargements
        direction LR
        clone ==> dJDK
        dSDK ==> dAS
        dAS ==> dIDEA
        dFLUTTER ==> repos
        dIDEA ==> dFLUTTER
    end
    dJDK --> unzipJDK
    unzipJDK --> dSDK
    dSDK --> unzipSDK
    dAS --> unzipAS
    
    dIDEA --> unzipIDEA
    dFLUTTER --> unzipFLUTTER
    
    unzipAS --> startandroidstudio
    unzipSDK --> startandroidstudio
    unzipSDK --> emulator
    emulator --> projetflutter
    unzipFLUTTER --> projetflutter
```

## Pour les profs pour changer les versions
1. Ouvrir et modifier https://github.com/departement-info-cem/scripts-mobile/blob/main/sub-scripts/urls-et-versions.ps1
2. Pour forcer le téléchargement des nouvelles versions, il faut vider le dossier contenant la cache de ZIP
3. Partir le script principal. Le script va détecter les éléments manquants et les télécharger.

## Pour les profs, comment travailler en mode dev
1. TODO
2. TODO
3. TODO

## Choses potentielles à améliorer
- vérifier les installations de firebase CLI et flutterfire
- intégrer le clonage du serveur de KMB et ouvrir intellij directement dessus
- créer un fichier de log qui liste ce qui a marché ou pas, pourquoi et à quelle heure
- gérer l'installation à la maison (poste Windows 10 vierge)
- transformer en jobs powershell

