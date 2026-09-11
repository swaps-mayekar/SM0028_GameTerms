using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameTerms.Editor
{
    public static class GlossaryContentGenerator
    {
        private const string ContentFolder = "Assets/GameTerms/Content/Glossary";
        private const string DatabasePath = ContentFolder + "/GlossaryDatabase.asset";
        private const string ResourcesDatabasePath = "Assets/GameTerms/Resources/GlossaryDatabase.asset";

        [MenuItem("Game Terms/Generate Glossary Content")]
        public static void Generate()
        {
            EnsureFolder(ContentFolder);
            EnsureFolder("Assets/GameTerms/Resources");

            var terms = GlossaryContentDefinitions.CreateAllTerms();
            var issues = GlossaryContentValidator.Validate(terms);
            if (issues.Count > 0)
            {
                foreach (var issue in issues)
                {
                    Debug.LogError($"[Glossary Validation] {issue}");
                }

                EditorUtility.DisplayDialog("Glossary Validation Failed", string.Join("\n", issues.Take(8)), "OK");
                return;
            }

            var validIds = terms.Select(term => term.Id).ToHashSet();
            foreach (var assetPath in Directory.GetFiles(ContentFolder, "*.asset"))
            {
                var normalized = assetPath.Replace('\\', '/');
                if (normalized.EndsWith("GlossaryDatabase.asset"))
                {
                    continue;
                }

                var fileName = Path.GetFileNameWithoutExtension(normalized);
                if (!validIds.Contains(fileName))
                {
                    AssetDatabase.DeleteAsset(normalized);
                }
            }

            var termAssets = new System.Collections.Generic.List<GlossaryTermAsset>();
            foreach (var term in terms)
            {
                var assetPath = $"{ContentFolder}/{term.Id}.asset";
                var asset = AssetDatabase.LoadAssetAtPath<GlossaryTermAsset>(assetPath);
                if (asset == null)
                {
                    asset = ScriptableObject.CreateInstance<GlossaryTermAsset>();
                    AssetDatabase.CreateAsset(asset, assetPath);
                }

                asset.SetData(term);
                EditorUtility.SetDirty(asset);
                termAssets.Add(asset);
            }

            var database = AssetDatabase.LoadAssetAtPath<GlossaryDatabaseAsset>(DatabasePath);
            if (database == null)
            {
                database = ScriptableObject.CreateInstance<GlossaryDatabaseAsset>();
                AssetDatabase.CreateAsset(database, DatabasePath);
            }

            database.SetTerms(termAssets);
            EditorUtility.SetDirty(database);

            if (AssetDatabase.LoadAssetAtPath<GlossaryDatabaseAsset>(ResourcesDatabasePath) != null)
            {
                AssetDatabase.DeleteAsset(ResourcesDatabasePath);
            }

            AssetDatabase.CopyAsset(DatabasePath, ResourcesDatabasePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Generated {termAssets.Count} glossary terms and synced Resources database.");
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
