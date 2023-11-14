import os
import ssl
import sys
import urllib.request

def telecharge(url, destination):
    urllib.request.urlretrieve(
        "https://release.gitkraken.com/darwin/installGitKraken.dmg",
        "/Users/Prof/kraken.dmg", context=ssl.SSLContext())
motDePasse = ""
if len(sys.argv) == 0 :
    print("Ouch pas d'arguments")
    motDePasse = "TODO"
else :
    motDePasse = sys.argv[0]
print("Mot de passe admin " + motDePasse)

# TODO faire ca pour IDEA, Android Studio GitKraken GithubDesktop

print("telecharge client GitKraken")
telecharge(
    "https://release.gitkraken.com/darwin/installGitKraken.dmg",
    "/Users/Prof/kraken.dmg")
os.system("hdiutil attach -mountpoint /Volumes/kraken <filename.dmg>")
os.system("cp -R /Volumes/kraken /Applications")
os.system("open -a GitKraken")
# copier dans Applications


print("Installation des mises à jour de MacOS en général Xcode inclus")
os.system("softwareupdate --install -a")

print("Configure Xcode")
# configure Xcode
os.system("sudo xcode-select --switch /Applications/Xcode.app/Contents/Developer")
os.system("sudo xcodebuild -runFirstLaunch")
os.system("open -a XCode")

print("Installation de Brew ou mise à jour")
# test if brew is installed
if os.system("which brew") == 0:
    print("Brew est déjà installé")
else:
    print("Installation de Brew")
    os.system("/bin/bash -c \"$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)\"")
    os.system("brew update")
# install homebrew

# TODO edit path
# TODO modify current PATH

print("Installation de cocoapods / mise à jour")
os.system("sudo gem update")
print("Mise à jour de cocoapods")
os.system("sudo gem install cocoapods")

print("Installation de Rosetta")
os.system("sudo softwareupdate --install-rosetta --agree-to-license")


print("Installation de flutter")
if os.system("which flutter") == 0:
    print("Flutter est déjà installé")
    os.system("flutter upgrade")
else:
    print("Installation de flutter")
    urllib.request.urlretrieve(
        "https://storage.googleapis.com/flutter_infra_release/releases/stable/macos/flutter_macos_arm64_3.13.9-stable.zip",
        "/Users/Prof/flutter.zip")
    os.system("unzip -qq flutter.zip")
    # deplacer flutter dans /opt
    os.system("sudo mv flutter /opt/")
    # ajouter flutter au path

print("Etat de l'installation de Flutter")
os.system("flutter doctor")


