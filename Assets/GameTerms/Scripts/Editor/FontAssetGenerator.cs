using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace GameTerms.Editor
{
    public static class FontAssetGenerator
    {
        private const string OutputFolder = "Assets/GameTerms/UI/Fonts/TMP";

        [MenuItem("Game Terms/Regenerate Font Assets")]
        public static void RegenerateAll()
        {
            EnsureFolder(OutputFolder);

            var regular = CreateOrRebuild("Assets/GameTerms/UI/Fonts/IBMPlexSans-Regular.ttf", $"{OutputFolder}/IBMPlexSans-Regular SDF.asset");
            var semiBold = CreateOrRebuild("Assets/GameTerms/UI/Fonts/IBMPlexSans-SemiBold.ttf", $"{OutputFolder}/IBMPlexSans-SemiBold SDF.asset");
            var bold = CreateOrRebuild("Assets/GameTerms/UI/Fonts/IBMPlexSans-Bold.ttf", $"{OutputFolder}/IBMPlexSans-Bold SDF.asset");
            var mono = CreateOrRebuild("Assets/GameTerms/UI/Fonts/IBMPlexMono-Regular.ttf", $"{OutputFolder}/IBMPlexMono-Regular SDF.asset");

            UpdateTheme(regular, semiBold, bold, mono);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("IBM Plex TMP font assets regenerated.");
        }

        public static (TMP_FontAsset regular, TMP_FontAsset semiBold, TMP_FontAsset bold, TMP_FontAsset mono) GenerateAll()
        {
            EnsureFolder(OutputFolder);
            return (
                CreateOrRebuild("Assets/GameTerms/UI/Fonts/IBMPlexSans-Regular.ttf", $"{OutputFolder}/IBMPlexSans-Regular SDF.asset"),
                CreateOrRebuild("Assets/GameTerms/UI/Fonts/IBMPlexSans-SemiBold.ttf", $"{OutputFolder}/IBMPlexSans-SemiBold SDF.asset"),
                CreateOrRebuild("Assets/GameTerms/UI/Fonts/IBMPlexSans-Bold.ttf", $"{OutputFolder}/IBMPlexSans-Bold SDF.asset"),
                CreateOrRebuild("Assets/GameTerms/UI/Fonts/IBMPlexMono-Regular.ttf", $"{OutputFolder}/IBMPlexMono-Regular SDF.asset")
            );
        }

        private static TMP_FontAsset CreateOrRebuild(string ttfPath, string assetPath)
        {
            var sourceFont = AssetDatabase.LoadAssetAtPath<Font>(ttfPath);
            if (sourceFont == null)
            {
                Debug.LogWarning($"Missing source font: {ttfPath}");
                return TMP_Settings.defaultFontAsset;
            }

            var existing = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(assetPath);
            if (existing != null && IsValid(existing))
            {
                return existing;
            }

            if (existing != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }

            var fontAsset = TMP_FontAsset.CreateFontAsset(
                sourceFont,
                90,
                9,
                GlyphRenderMode.SDFAA,
                1024,
                1024,
                AtlasPopulationMode.Static);

            fontAsset.atlasPopulationMode = AtlasPopulationMode.Static;
            fontAsset.TryAddCharacters(GetCharacterSet(), out var missing);

            if (missing != null && missing.Length > 0)
            {
                Debug.LogWarning($"Missing glyphs in {assetPath}: {string.Join(string.Empty, missing)}");
            }

            AssetDatabase.CreateAsset(fontAsset, assetPath);

            if (fontAsset.material != null)
            {
                fontAsset.material.name = $"{sourceFont.name} Material";
                AssetDatabase.AddObjectToAsset(fontAsset.material, fontAsset);
            }

            if (fontAsset.atlasTexture != null)
            {
                fontAsset.atlasTexture.name = $"{sourceFont.name} Atlas";
                AssetDatabase.AddObjectToAsset(fontAsset.atlasTexture, fontAsset);
            }

            EditorUtility.SetDirty(fontAsset);
            return fontAsset;
        }

        private static void UpdateTheme(TMP_FontAsset regular, TMP_FontAsset semiBold, TMP_FontAsset bold, TMP_FontAsset mono)
        {
            var themePaths = new[]
            {
                "Assets/GameTerms/UI/Theme/UiTheme.asset",
                "Assets/GameTerms/Resources/UiTheme.asset"
            };

            foreach (var path in themePaths)
            {
                var theme = AssetDatabase.LoadAssetAtPath<UI.UiTheme>(path);
                if (theme == null)
                {
                    continue;
                }

                theme.SansRegular = regular;
                theme.SansSemiBold = semiBold;
                theme.SansBold = bold;
                theme.MonoRegular = mono;
                EditorUtility.SetDirty(theme);
            }
        }

        private static bool IsValid(TMP_FontAsset fontAsset)
        {
            return fontAsset != null
                   && fontAsset.atlasTexture != null
                   && fontAsset.atlasPopulationMode == AtlasPopulationMode.Static
                   && fontAsset.characterTable != null
                   && fontAsset.characterTable.Count > 0;
        }

        private static string GetCharacterSet()
        {
            var builder = new StringBuilder(256);
            for (var c = 32; c <= 126; c++)
            {
                builder.Append((char)c);
            }

            builder.Append("←→★☆⌕•");
            builder.Append("ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÑÒÓÔÕÖØÙÚÛÜÝàáâãäåæçèéêëìíîïñòóôõöøùúûüý");
            return builder.ToString();
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            var parent = System.IO.Path.GetDirectoryName(path)?.Replace('\\', '/');
            var folderName = System.IO.Path.GetFileName(path);
            if (!string.IsNullOrEmpty(parent) && !AssetDatabase.IsValidFolder(parent))
            {
                EnsureFolder(parent);
            }

            AssetDatabase.CreateFolder(parent, folderName);
        }
    }
}
