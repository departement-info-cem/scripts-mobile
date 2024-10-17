namespace ScriptSharp;

public class UtilsFirebase
{

    public static void InstallFirebase()
    {
        Utils.RunCommand("npm install -g firebase-tools");
    }
    public static void InstallFlutterFire()
    {
        Utils.RunCommand(UtilsFlutter.PathToDart()+" pub global activate flutterfire_cli");
    }
}