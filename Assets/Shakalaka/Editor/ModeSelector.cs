using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Shakalaka
{
    public class ModeSelector
    {
        [MenuItem("Mode/Client")]
        private static void ModeClient()
        {
            Debug.Log("Mode P2P");
            
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
            
            var scenePaths = new[] { "Assets/Shakalaka/Scenes/_Startup.unity", "Assets/Shakalaka/Scenes/MainMenu.unity", "Assets/Shakalaka/Scenes/PreGame.unity", "Assets/Shakalaka/Scenes/Game.unity" };
            
            foreach (var scenePath in scenePaths)
            {
                if (!string.IsNullOrEmpty(scenePath))
                    editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
            }

            // Set the Build Settings window Scene list
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }
        
        [MenuItem("Mode/Server")]
        private static void ModeServer()
        {
            Debug.Log("Mode DGS");
            
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
            
            var scenePaths = new[] { "Assets/Shakalaka/Scenes/_ServerStartup.unity", "Assets/Shakalaka/Scenes/PreGame.unity", "Assets/Shakalaka/Scenes/Game.unity" };
            
            foreach (var scenePath in scenePaths)
            {
                if (!string.IsNullOrEmpty(scenePath))
                    editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
            }

            // Set the Build Settings window Scene list
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }

        [MenuItem("Mode/Single")]
        private static void ModeSingle()
        {
            Debug.Log("Mode Single");
        }
    }
}