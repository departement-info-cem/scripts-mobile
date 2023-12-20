import os

import sys
import urllib.request

import requests

# logic goes as
# install homebrew,
# then install a 3.x ruby with brew
# make this ruby the default ruby playing with the path
# then install cocoapods with gem using this ruby
# then we do not have to sudo all cocoapods commands


userName = os.getlogin()
#userName = "Prof"

def executeAsUser(command):
    currentUser = userName
    os.system("sudo -u " + currentUser + " " + command)

def macupdate():
    print("Installation des mises à jour de MacOS en général Xcode inclus")
    os.system("softwareupdate --install -a")

def flutter():
    currentUser = os.popen("id -un").read().strip()
    print("Installation de flutter as " + currentUser)
    if os.system("which flutter") == 0:
        print("Flutter est déjà installé")
        # sudo -u "$currentUser" defaults write com.apple.dock orientation left
        executeAsUser("flutter upgrade")
    else:
        print("Installation de flutter nouveau")
        telecharge(
            flutterURL,
            "/Users/"+userName+"/Downloads/flutter.zip")
        os.system("mkdir /opt/flutter")
        os.system("chown -R " + userName + " /opt/flutter")
        os.system("chmod 777 -R  /opt/flutter")
        executeAsUser("unzip -qq /Users/"+userName+"/Downloads/flutter.zip -d /opt/")
        os.system("chown -R " + userName + " /opt/flutter")
        os.system("chgrp -R admin /opt/flutter")
        os.system("chmod 777 -R  /opt/flutter")
        # deplacer flutter dans /opt
        # os.system("sudo mv flutter /opt/")
        add_to_system_path("/opt/flutter/bin")
        # ajouter flutter au path
    #print("Etat de l'installation de Flutter")
    #os.system("flutter doctor")


def add_to_system_path(path):
    with open("/etc/paths", "r+") as file:
        for line in file:
            if path in line:
                break
        else:  # not found, we are at the eof
            file.write(path)  # append missing data


def rosetta():
    print("Installation de Rosetta")
    os.system("sudo softwareupdate --install-rosetta --agree-to-license")
def homebrew():
    print("Installation de Brew ou mise à jour")
    # test if brew is installed
    if executeAsUser("which brew") == 0:
        print("Brew est déjà installé")
        executeAsUser("brew update")
    else:
        print("Installation de Brew")
        executeAsUser("/bin/bash -c \"$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)\"")
        add_to_system_path("/opt/homebrew/bin")
        executeAsUser("brew update")
    # install homebrew

def cocoapods():
    print("Installation de cocoapods / mise à jour")
    os.system("sudo gem update")
    print("Mise à jour de cocoapods")
    os.system("sudo gem install cocoapods")
    # cocoapods cannot be run as root
    executeAsUser("pod repo update")

def xcode():
    print("Configure Xcode")
    # configure Xcode
    os.system("sudo xcode-select --switch /Applications/Xcode.app/Contents/Developer")
    os.system("sudo xcodebuild -runFirstLaunch")
    os.system("open -a XCode")

def telecharge(url, destination):
    r = requests.get(url, allow_redirects=True)
    open(destination, 'wb').write(r.content)

def installDansApplication(url, tempFile, volumeName, applicationName):
    # test first if the application is already installed

    os.system("rm -rf \"/Applications/" + applicationName + "\"")
    print("Telechargement de " + applicationName)
    telecharge(url, tempFile)

    mountCommand = "hdiutil attach -mountpoint \"/Volumes/" + volumeName + "\" " + tempFile + ("  -quiet")
    print("Montage du volume de " + mountCommand)
    os.system(mountCommand)

    copyCommand = "cp -Rf \"/Volumes/" + volumeName + "/" + applicationName + "\"" " /Applications"
    print("Copie de l'application " + copyCommand)
    os.system(copyCommand)

    #print("Demontage de l'image \"" + applicationName + "\" dans Applications")
    os.system("hdiutil detach \"/Volumes/" + volumeName+ "\" -quiet ")
    os.system("rm " + tempFile)
    #print("Demarrage de l'application " + applicationName )
    os.system("open -a \"" + applicationName+ "\" ")

flutterLocation = "/opt/flutter"
flutterURL = "https://storage.googleapis.com/flutter_infra_release/releases/stable/macos/flutter_macos_arm64_3.16.1-stable.zip"
intellijURL = "https://download.jetbrains.com/idea/ideaIC-2023.2.5-aarch64.dmg"

androidStudioURL = "https://redirector.gvt1.com/edgedl/android/studio/install/2022.3.1.22/android-studio-2022.3.1.22-mac_arm.dmg"

githubDesktopURL = "https://central.github.com/deployments/desktop/desktop/latest/darwin-arm64"
gitKrakenURL = "https://release.gitkraken.com/darwin-arm64/installGitKraken.dmg"

# Premier truc a faire partir la mise a jour de Xcode
print("ALLER DANS LE MAC APP STORE ET LANCER LA MISE A JOUR DE XCODE, C'EST LONG!!!!!")

def githubDesktop():
    os.system("rm -rf \"/Applications/GitHub Desktop.app\"")
    telecharge(githubDesktopURL, "/Users/"+userName+"/Downloads/githubDesktop.zip")
    os.system("unzip -o /Users/"+userName+"/Downloads/githubDesktop.zip -d /Applications" )
    os.system("rm /Users/"+userName+"/Downloads/githubDesktop.zip")


####  Real calls   ####

installDansApplication(
    intellijURL,
    "/Users/"+userName+"/Downloads/intellij.dmg",
    "IntelliJ IDEA CE",
    "IntelliJ IDEA CE.app")

installDansApplication(
    androidStudioURL,
    "/Users/"+userName+"/Downloads/android.dmg",
    "Android Studio",
    "Android Studio.app")

installDansApplication(
    gitKrakenURL,
    "/Users/"+userName+"/Downloads/gitkraken.dmg",
    "GitKraken",
    "GitKraken.app")

#githubDesktop() TODO fix it is broken
macupdate()
xcode()
homebrew()
cocoapods()
rosetta()
flutter()

os.system("flutter --version")
os.system("pod --version")
os.system("/usr/bin/xcodebuild -version")


