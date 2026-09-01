using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameTerms
{
    [Serializable]
    public sealed class GlossaryCatalogJson
    {
        public List<GlossaryTermData> Terms = new();
    }

    public sealed class JsonGlossaryRepository : IGlossaryRepository
    {
        private readonly Dictionary<string, GlossaryTermData> termsById = new();
        private readonly List<GlossaryTermData> allTerms = new();

        public JsonGlossaryRepository(TextAsset jsonAsset)
        {
            if (jsonAsset == null)
            {
                Debug.LogError("Glossary JSON asset is missing.");
                return;
            }

            var catalog = JsonUtility.FromJson<GlossaryCatalogJson>(jsonAsset.text);
            if (catalog?.Terms == null)
            {
                Debug.LogError("Glossary JSON could not be parsed.");
                return;
            }

            foreach (var term in catalog.Terms)
            {
                if (term == null || string.IsNullOrWhiteSpace(term.Id))
                {
                    continue;
                }

                var data = term.Clone();
                if (termsById.ContainsKey(data.Id))
                {
                    continue;
                }

                termsById[data.Id] = data;
                allTerms.Add(data);
            }

            allTerms.Sort((a, b) => string.CompareOrdinal(a.Term, b.Term));
        }

        public IReadOnlyList<GlossaryTermData> GetAllTerms() => allTerms;

        public GlossaryTermData GetTermById(string id) => TryGetTerm(id, out var term) ? term : null;

        public IReadOnlyList<GlossaryTermData> GetTermsByCategory(GlossaryCategory category)
        {
            return allTerms.Where(term => term.Category == category).ToList();
        }

        public bool TryGetTerm(string id, out GlossaryTermData term)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                term = null;
                return false;
            }

            return termsById.TryGetValue(id, out term);
        }
    }
}
