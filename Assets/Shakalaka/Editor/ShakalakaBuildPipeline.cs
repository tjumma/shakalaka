using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Shakalaka
{
    [CreateAssetMenu(menuName = "Shakalaka/BuildPipeline", fileName = "BuildPipeline")]
    public class ShakalakaBuildPipeline : ScriptableObject
    {
        [SerializeField] private BuildConfig clientBuildConfig;
        [SerializeField] private BuildConfig dgsBuildConfig;
        
        [MenuItem("Build/Client")]
        private static void BuildClient() {
            Debug.Log("Build Client");

            string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = new[] { "Assets/Shakalaka/Scenes/_Startup.unity", "Assets/Shakalaka/Scenes/MainMenu.unity", "Assets/Shakalaka/Scenes/PreGame.unity", "Assets/Shakalaka/Scenes/Game.unity" };
            buildPlayerOptions.locationPathName = path + "/Shakalaka.exe";
            
            // buildPlayerOptions.locationPathName = path;
            buildPlayerOptions.target = BuildTarget.StandaloneWindows;
            buildPlayerOptions.options = BuildOptions.None;
            
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }
        
        [MenuItem("Build/DGS")]
        private static void BuildDGS() {
            Debug.Log("Build DGS");
        }
    }
}