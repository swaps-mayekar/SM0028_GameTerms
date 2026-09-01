using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameTerms.Editor
{
    public static class GlossaryContentValidator
    {
        public static List<string> Validate(IReadOnlyList<GlossaryTermData> terms)
        {
            var issues = new List<string>();
            var ids = new HashSet<string>();

            foreach (var term in terms)
            {
                if (string.IsNullOrWhiteSpace(term.Id))
                {
                    issues.Add("A term is missing an id.");
                    continue;
                }

                if (!ids.Add(term.Id))
                {
                    issues.Add($"Duplicate id: {term.Id}");
                }

                if (string.IsNullOrWhiteSpace(term.Term))
                {
                    issues.Add($"Term '{term.Id}' is missing a name.");
                }

                if (string.IsNullOrWhiteSpace(term.ShortDefinition))
                {
                    issues.Add($"Term '{term.Id}' is missing a short definition.");
                }
            }

            var idLookup = ids.ToHashSet();
            foreach (var term in terms)
            {
                foreach (var relatedId in term.RelatedTermIds ?? new List<string>())
                {
                    if (!idLookup.Contains(relatedId))
                    {
                        issues.Add($"Term '{term.Id}' references missing related id '{relatedId}'.");
                    }
                }
            }

            return issues;
        }
    }
}
