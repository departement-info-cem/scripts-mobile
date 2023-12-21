Ce dossier contient les scripts pour la gestion des macs

# suppression des comptes

A FAIRE : chaque fin de session pour s'assurer que seul le compte Prof reste

- cloner le repo de scripts mobile : https://github.com/departement-info-cem/scripts-mobile
- ouvrir un terminal
- se placer dans le dossier du repo, dans le dossier mac
- taper la commande `sudo python3 supprimer_comptes.py`
- vérifier la liste des comptes dans les préférences système
- vérifier l'espace disque disponible qui devrait être grand

# installation / mise à jour

A FAIRE : chaque début de session ou si des problèmes sur un mac

- cloner le repo de scripts mobile : https://github.com/departement-info-cem/scripts-mobile
- ouvrir un terminal
- se placer dans le dossier du repo, dans le dossier mac
- taper la commande `sudo python3 installation.py`
- il est possible qu'un prompt apparaisse pour installer python3
- il faudra peut etre aussi taper "pip3 install requests"
- on peut mettre a jour "python3 -m pip install --upgrade pip"
- on devrait avoir:
  - Android Studio  dans /Applications 
  - Intellij        dans /Applications
  - Github Desktop  dans /Applications
  - GitKraken       dans /Applications
  - flutter installé dans /opt/flutter

