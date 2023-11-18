import os

def utilisateursASupprimer(liste):
    listeASupprimer = []
    for user in liste:
        if user == "Prof" :
            continue
        if "joris" in user :
            continue
        if user == "Shared" :
            continue
        if user == "Guest" :
            continue
        listeASupprimer.append(user)
    return listeASupprimer

# lister tous les dossiers

tousUsers = os.popen("dscl . -list /Users").read()
dossiersUsers = os.popen("ls /Users/").read()

print(tousUsers)
print(dossiersUsers)
aSupprimer = utilisateursASupprimer(dossiersUsers.splitlines())
print(aSupprimer)
if (len(aSupprimer) == 0) :
    print("Aucun compte à supprimer")
else :
    for compte in aSupprimer:
        print("Suppression de " + compte)
        os.system("dscl . -delete /Users/" + compte)
        print("Suppression du dosser /Users/" + compte)
        os.system("rm -rf /Users/" + compte)

# lister les dossiers dans /Users/

# supprimer les comptes qui ne sont pas Prof

