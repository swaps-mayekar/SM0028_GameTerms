using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameTerms
{
    public sealed class ScriptableGlossaryRepository : IGlossaryRepository
    {
        private readonly Dictionary<string, GlossaryTermData> termsById = new();
        private readonly List<GlossaryTermData> allTerms = new();

        public ScriptableGlossaryRepository(GlossaryDatabaseAsset database)
        {
            if (database == null)
            {
                Debug.LogError("Glossary database asset is missing.");
                return;
            }

            foreach (var termAsset in database.Terms)
            {
                if (termAsset == null || termAsset.Data == null || string.IsNullOrWhiteSpace(termAsset.Data.Id))
                {
                    continue;
                }

                var data = termAsset.Data.Clone();
                if (termsById.ContainsKey(data.Id))
                {
                    Debug.LogWarning($"Duplicate glossary term id detected: {data.Id}");
                    continue;
                }

                termsById[data.Id] = data;
                allTerms.Add(data);
            }

            allTerms.Sort((a, b) => string.CompareOrdinal(a.Term, b.Term));
        }

        public IReadOnlyList<GlossaryTermData> GetAllTerms() => allTerms;

        public GlossaryTermData GetTermById(string id)
        {
            return TryGetTerm(id, out var term) ? term : null;
        }

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
