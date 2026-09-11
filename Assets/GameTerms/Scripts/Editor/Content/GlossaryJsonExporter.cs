using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameTerms.Editor
{
    public static class GlossaryJsonExporter
    {
        private const string JsonPath = "Assets/GameTerms/Resources/glossary.json";

        [MenuItem("Game Terms/Export Glossary JSON")]
        public static void Export()
        {
            var catalog = new GlossaryCatalogJson
            {
                Terms = GlossaryContentDefinitions.CreateAllTerms().ToList()
            };

            var issues = GlossaryContentValidator.Validate(catalog.Terms);
            if (issues.Count > 0)
            {
                foreach (var issue in issues)
                {
                    Debug.LogError($"[Glossary Validation] {issue}");
                }

                return;
            }

            if (!AssetDatabase.IsValidFolder("Assets/GameTerms/Resources"))
            {
                AssetDatabase.CreateFolder("Assets/GameTerms", "Resources");
            }

            var json = JsonUtility.ToJson(catalog, true);
            System.IO.File.WriteAllText(JsonPath, json);
            AssetDatabase.ImportAsset(JsonPath);
            Debug.Log($"Exported glossary JSON to {JsonPath} ({catalog.Terms.Count} terms).");
        }

        public static void GenerateAndExportBatch()
        {
            GlossaryContentGenerator.Generate();
            Export();
            var terms = GlossaryContentDefinitions.CreateAllTerms();
            var issues = GlossaryContentValidator.Validate(terms);
            EditorApplication.Exit(issues.Count > 0 ? 1 : 0);
        }
    }
}
