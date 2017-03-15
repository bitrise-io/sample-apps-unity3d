#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
class Build
{

    static void Android()
    {
        BuildPipeline.BuildPlayer(GetScenes(), "your.apk", BuildTarget.Android, BuildOptions.None);
    }

    static string[] GetScenes()
    {
        return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
    }

	static void iOS () {
		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = GetScenes();
		buildPlayerOptions.locationPathName = "iOSBuild";
		buildPlayerOptions.target = BuildTarget.iOS;
		buildPlayerOptions.options = BuildOptions.None;
		BuildPipeline.BuildPlayer (buildPlayerOptions);
	}
}
#endif