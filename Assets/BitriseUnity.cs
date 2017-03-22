#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using System.IO;

class BitriseUnity
{
	public static void Build()
	{		
		BitriseTools tools = new BitriseTools ();
		tools.PrintInputs ();

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = tools.GetActiveScenes();
		buildPlayerOptions.locationPathName = tools.inputs.buildOutput;

		if (tools.inputs.buildPlatform == BitriseTools.BuildPlatform.android) {
			EditorPrefs.SetString("AndroidSdkRoot", tools.inputs.androidSdkPath);
			buildPlayerOptions.target = BuildTarget.Android;
			//set jdk path?
			//keystore options....
		}else if(tools.inputs.buildPlatform == BitriseTools.BuildPlatform.ios) {
			buildPlayerOptions.target = BuildTarget.iOS;
		}
			
		buildPlayerOptions.options = BuildOptions.None;
		BuildPipeline.BuildPlayer (buildPlayerOptions);
	}
}

public class BitriseTools {

	public Inputs inputs;
	public Logging log;

	public enum BuildPlatform {
		android,
		ios,
	}

	public BitriseTools() {
		inputs = new Inputs ();
		log = new Logging ();
	}

	public string[] GetActiveScenes()
	{
		return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
	}
		
	//inputs
	public class Inputs{
		public string androidSdkPath;
		public string buildOutput;
		public string androidKeystorePath;
		public string androidKeystoreAlias;
		public string androidKeystorePassword;
		public string androidKeystoreAliasPassword;
		public BuildPlatform buildPlatform;

		public Inputs() {
			string[] cmdArgs = Environment.GetCommandLineArgs();
			for(int i=0;i<cmdArgs.Length;i++){
				if (cmdArgs [i].Equals ("-buildplatform"))
					buildPlatform = (BuildPlatform)Enum.Parse(typeof(BuildPlatform),cmdArgs [i + 1]);
				if (cmdArgs [i].Equals ("-androidSdkPath"))
					androidSdkPath = cmdArgs [i + 1];
				if (cmdArgs [i].Equals ("-buildOutput"))
					buildOutput = cmdArgs [i + 1];
				if (cmdArgs [i].Equals ("-androidKeystorePath"))
					androidKeystorePath = cmdArgs [i + 1];
				if (cmdArgs [i].Equals ("-androidKeystoreAlias"))
					androidKeystoreAlias = cmdArgs [i + 1];
				if (cmdArgs [i].Equals ("-androidKeystorePassword"))
					androidKeystorePassword = cmdArgs [i + 1];
				if (cmdArgs [i].Equals ("-androidKeystoreAliasPassword"))
					androidKeystoreAliasPassword = cmdArgs [i + 1];
			}
		}
	}

	// bash logging tools
	public class Logging {
		bool initialized = false;

		void _init()
		{
			if (!initialized) {
				StreamWriter sw = new StreamWriter (Console.OpenStandardOutput (), System.Text.Encoding.ASCII);
				sw.AutoFlush = true;
				Console.SetOut (sw);
				initialized = true;
			}
		}
			
		public void Fail(string message) {_init ();Console.WriteLine("\x1b[31m"+message+"\x1b[0m");}
		public void Done(string message) {_init ();Console.WriteLine("\x1b[32m"+message+"\x1b[0m");}
		public void Info(string message) {_init ();Console.WriteLine("\x1b[34m"+message+"\x1b[0m");}
		public void Warn(string message) {_init ();Console.WriteLine("\x1b[33m"+message+"\x1b[0m");}
		public void Print(string message) {_init ();Console.WriteLine(message);}
	}

	public void PrintInputs() {
		log.Info ("Bitrise Unity build script inputs:");
		log.Print (" -buildOutput: "+inputs.buildOutput);
		log.Print (" -buildPlatform: "+inputs.buildPlatform.ToString());
		log.Print (" -androidSdkPath: "+inputs.androidSdkPath);
		log.Print (" -androidKeystorePath: "+inputs.androidKeystorePath);
		log.Print (" -androidKeystoreAlias: "+(string.IsNullOrEmpty(inputs.androidKeystoreAlias) ? "" : "***"));
		log.Print (" -androidKeystorePassword: "+(string.IsNullOrEmpty(inputs.androidKeystorePassword) ? "" : "***"));
		log.Print (" -androidKeystoreAliasPassword: "+(string.IsNullOrEmpty(inputs.androidKeystoreAliasPassword) ? "" : "***"));
		log.Print ("");
	}
}
#endif