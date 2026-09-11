using System;
using System.Collections.Generic;
using System.Linq;

namespace GameTerms.Editor
{
    public static class GlossaryContentValidator
    {
        public const int ExpectedTermCount = 150;
        public const int ExpectedCategories = 10;
        public const int ExpectedTermsPerCategory = 15;

        public static List<string> Validate(IReadOnlyList<GlossaryTermData> terms)
        {
            var issues = new List<string>();
            var ids = new HashSet<string>(StringComparer.Ordinal);
            var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (terms == null || terms.Count == 0)
            {
                issues.Add("Glossary content is empty.");
                return issues;
            }

            if (terms.Count != ExpectedTermCount)
            {
                issues.Add($"Expected {ExpectedTermCount} terms but found {terms.Count}.");
            }

            foreach (var term in terms)
            {
                if (term == null)
                {
                    issues.Add("A null term entry was found.");
                    continue;
                }

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
                else if (!names.Add(term.Term.Trim()))
                {
                    issues.Add($"Duplicate term name: {term.Term}");
                }

                if (string.IsNullOrWhiteSpace(term.ShortDefinition))
                {
                    issues.Add($"Term '{term.Id}' is missing a short definition.");
                }

                if (string.IsNullOrWhiteSpace(term.SimpleExplanation))
                {
                    issues.Add($"Term '{term.Id}' is missing a simple explanation.");
                }

                if (string.IsNullOrWhiteSpace(term.WhyItMatters))
                {
                    issues.Add($"Term '{term.Id}' is missing why-it-matters content.");
                }

                if (string.IsNullOrWhiteSpace(term.Example))
                {
                    issues.Add($"Term '{term.Id}' is missing an example.");
                }

                if (string.IsNullOrWhiteSpace(term.CommonMistake))
                {
                    issues.Add($"Term '{term.Id}' is missing a common mistake.");
                }

                if (string.IsNullOrWhiteSpace(term.PracticePrompt))
                {
                    issues.Add($"Term '{term.Id}' is missing a practice prompt.");
                }

                if (term.GameUses == null || term.GameUses.Count < 3 || term.GameUses.Any(string.IsNullOrWhiteSpace))
                {
                    issues.Add($"Term '{term.Id}' needs at least three game uses.");
                }

                if (term.Tags == null || term.Tags.Count == 0 || term.Tags.Any(string.IsNullOrWhiteSpace))
                {
                    issues.Add($"Term '{term.Id}' needs at least one tag.");
                }
            }

            var idLookup = ids.ToHashSet(StringComparer.Ordinal);
            foreach (var term in terms.Where(t => t != null))
            {
                foreach (var relatedId in term.RelatedTermIds ?? new List<string>())
                {
                    if (!idLookup.Contains(relatedId))
                    {
                        issues.Add($"Term '{term.Id}' references missing related id '{relatedId}'.");
                    }
                }
            }

            foreach (GlossaryCategory category in Enum.GetValues(typeof(GlossaryCategory)))
            {
                var count = terms.Count(term => term != null && term.Category == category);
                if (count != ExpectedTermsPerCategory)
                {
                    issues.Add($"Category {category} has {count} terms; expected {ExpectedTermsPerCategory}.");
                }
            }

            var distinctCategories = terms.Where(term => term != null).Select(term => term.Category).Distinct().Count();
            if (distinctCategories != ExpectedCategories)
            {
                issues.Add($"Expected {ExpectedCategories} categories but found {distinctCategories}.");
            }

            return issues;
        }
    }
}
