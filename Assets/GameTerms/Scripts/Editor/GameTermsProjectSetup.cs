using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameTerms.Bootstrap;
using GameTerms.UI;

namespace GameTerms.Editor
{
    public static class GameTermsProjectSetup
    {
        private const string ThemePath = "Assets/GameTerms/UI/Theme/UiTheme.asset";
        private const string ResourcesFolder = "Assets/GameTerms/Resources";
        private const string SplashScenePath = "Assets/Scenes/0_SplashScene.unity";
        private const string MainScenePath = "Assets/GameTerms/Scenes/1_MainScene.unity";

        [MenuItem("Game Terms/Setup Project")]
        public static void SetupProject()
        {
            GlossaryContentGenerator.Generate();
            GlossaryJsonExporter.Export();
            var fonts = GenerateFontAssets();
            var theme = GenerateTheme(fonts);
            GenerateResources(theme);
            GenerateBrandingIcon();
            SetupScenes(theme);
            ConfigurePlayerSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Game Terms project setup complete.");
        }

        private static (TMP_FontAsset regular, TMP_FontAsset semiBold, TMP_FontAsset bold, TMP_FontAsset mono) GenerateFontAssets()
        {
            return FontAssetGenerator.GenerateAll();
        }

        private static UiTheme GenerateTheme((TMP_FontAsset regular, TMP_FontAsset semiBold, TMP_FontAsset bold, TMP_FontAsset mono) fonts)
        {
            EnsureFolder("Assets/GameTerms/UI/Theme");
            var theme = AssetDatabase.LoadAssetAtPath<UiTheme>(ThemePath);
            if (theme == null)
            {
                theme = ScriptableObject.CreateInstance<UiTheme>();
                AssetDatabase.CreateAsset(theme, ThemePath);
            }

            theme.SansRegular = fonts.regular;
            theme.SansSemiBold = fonts.semiBold;
            theme.SansBold = fonts.bold;
            theme.MonoRegular = fonts.mono;
            EditorUtility.SetDirty(theme);
            return theme;
        }

        private static void GenerateResources(UiTheme theme)
        {
            EnsureFolder("Assets/GameTerms");
            EnsureFolder(ResourcesFolder);

            var database = AssetDatabase.LoadAssetAtPath<GlossaryDatabaseAsset>("Assets/GameTerms/Content/Glossary/GlossaryDatabase.asset");
            if (database != null)
            {
                var databaseCopyPath = $"{ResourcesFolder}/GlossaryDatabase.asset";
                if (AssetDatabase.LoadAssetAtPath<GlossaryDatabaseAsset>(databaseCopyPath) != null)
                {
                    AssetDatabase.DeleteAsset(databaseCopyPath);
                }

                AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(database), databaseCopyPath);
            }

            var themeCopyPath = $"{ResourcesFolder}/UiTheme.asset";
            if (AssetDatabase.LoadAssetAtPath<UiTheme>(themeCopyPath) == null)
            {
                AssetDatabase.CopyAsset(ThemePath, themeCopyPath);
            }
        }

        private static void GenerateBrandingIcon()
        {
            EnsureFolder("Assets/GameTerms/Branding");
            var iconPath = "Assets/GameTerms/Branding/AppIcon.png";
            if (File.Exists(iconPath))
            {
                return;
            }

            var texture = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
            var background = new Color(0.07f, 0.08f, 0.11f);
            var accent = new Color(0.36f, 0.78f, 1f);
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var center = new Vector2(texture.width * 0.5f, texture.height * 0.5f);
                    var distance = Vector2.Distance(new Vector2(x, y), center) / (texture.width * 0.42f);
                    var color = Color.Lerp(accent, background, Mathf.Clamp01(distance));
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            File.WriteAllBytes(iconPath, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);
            AssetDatabase.ImportAsset(iconPath);
        }

        private static void SetupScenes(UiTheme theme)
        {
            SetupSplashScene();
            SetupMainScene(theme);
            var scenes = new[]
            {
                new EditorBuildSettingsScene(SplashScenePath, true),
                new EditorBuildSettingsScene(MainScenePath, true)
            };
            EditorBuildSettings.scenes = scenes;
        }

        private static void SetupSplashScene()
        {
            var scene = EditorSceneManager.OpenScene(SplashScenePath, OpenSceneMode.Single);
            foreach (var root in scene.GetRootGameObjects())
            {
                if (root.name != "Main Camera")
                {
                    Object.DestroyImmediate(root);
                }
            }

            var bootstrap = new GameObject("SplashController");
            bootstrap.AddComponent<SplashController>();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }

        private static void SetupMainScene(UiTheme theme)
        {
            EnsureFolder("Assets/GameTerms/Scenes");
            var scene = File.Exists(MainScenePath)
                ? EditorSceneManager.OpenScene(MainScenePath, OpenSceneMode.Single)
                : EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            foreach (var root in scene.GetRootGameObjects())
            {
                Object.DestroyImmediate(root);
            }

            var cameraGo = new GameObject("Main Camera", typeof(Camera));
            cameraGo.tag = "MainCamera";
            var camera = cameraGo.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = theme.Background;
            camera.orthographic = true;

            var bootstrap = new GameObject("MainSceneBootstrap");
            bootstrap.AddComponent<MainSceneBootstrap>();

            if (!File.Exists(MainScenePath))
            {
                EditorSceneManager.SaveScene(scene, MainScenePath);
            }
            else
            {
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
            }
        }

        private static void ConfigurePlayerSettings()
        {
            PlayerSettings.companyName = "GameTerms";
            PlayerSettings.productName = "Game Terms";
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.allowedAutorotateToPortrait = true;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.allowedAutorotateToLandscapeLeft = false;
            PlayerSettings.allowedAutorotateToLandscapeRight = false;
            PlayerSettings.iOS.showActivityIndicatorOnLoading = iOSShowActivityIndicatorOnLoading.DontShow;
            PlayerSettings.usePlayerLog = false;

            var iconPath = "Assets/GameTerms/Branding/AppIcon.png";
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
            if (icon != null)
            {
                var icons = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.iOS);
                if (icons == null || icons.Length == 0)
                {
                    icons = new Texture2D[1];
                }

                icons[0] = icon;
                PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, icons);
            }
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            var parent = Path.GetDirectoryName(path)?.Replace('\\', '/');
            var folderName = Path.GetFileName(path);
            if (!string.IsNullOrEmpty(parent) && !AssetDatabase.IsValidFolder(parent))
            {
                EnsureFolder(parent);
            }

            AssetDatabase.CreateFolder(parent, folderName);
        }
    }
}
