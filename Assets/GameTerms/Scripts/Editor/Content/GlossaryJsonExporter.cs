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
                Debug.LogError(string.Join("\n", issues));
                return;
            }

            if (!AssetDatabase.IsValidFolder("Assets/GameTerms/Resources"))
            {
                AssetDatabase.CreateFolder("Assets/GameTerms", "Resources");
            }

            var json = JsonUtility.ToJson(catalog, true);
            System.IO.File.WriteAllText(JsonPath, json);
            AssetDatabase.ImportAsset(JsonPath);
            Debug.Log($"Exported glossary JSON to {JsonPath}");
        }
    }
}
