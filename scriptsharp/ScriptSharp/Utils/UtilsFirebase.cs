namespace ScriptSharp;

public class UtilsFirebase
{

    public static void InstallFirebase()
    {
        Utils.RunCommand("npm install -g firebase-tools");
    }
    public static void InstallFlutterFire()
    {
        // Utils.RunCommand("echo %path%");
        Utils.RunCommand("dart pub global activate flutterfire_cli");
    }
}