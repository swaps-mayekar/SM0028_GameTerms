using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace GameTerms.Editor
{
    public static class GameTermsBuild
    {
        private const string IosBuildPath = "Builds/iOS";

        [MenuItem("Game Terms/Build/iOS")]
        public static void BuildIos()
        {
            EnsureScenes();
            if (!Directory.Exists(IosBuildPath))
            {
                Directory.CreateDirectory(IosBuildPath);
            }

            var options = new BuildPlayerOptions
            {
                scenes = GetScenePaths(),
                locationPathName = IosBuildPath,
                target = BuildTarget.iOS,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(options);
            LogBuildResult(report);
        }

        public static void BuildIosBatch()
        {
            BuildIos();
        }

        private static void EnsureScenes()
        {
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene("Assets/Scenes/0_SplashScene.unity", true),
                new EditorBuildSettingsScene("Assets/GameTerms/Scenes/1_MainScene.unity", true)
            };
        }

        private static string[] GetScenePaths()
        {
            var scenes = EditorBuildSettings.scenes;
            var paths = new string[scenes.Length];
            for (var i = 0; i < scenes.Length; i++)
            {
                paths[i] = scenes[i].path;
            }

            return paths;
        }

        private static void LogBuildResult(BuildReport report)
        {
            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"iOS build succeeded: {report.summary.totalSize} bytes at {IosBuildPath}");
            }
            else
            {
                Debug.LogError($"iOS build failed: {report.summary.result}");
            }
        }
    }
}
