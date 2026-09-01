using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTerms
{
    public sealed class SearchResult
    {
        public GlossaryTermData Term;
        public int Score;
        public string MatchedField;
    }

    public sealed class SearchService
    {
        private readonly IGlossaryRepository repository;

        public SearchService(IGlossaryRepository repository)
        {
            this.repository = repository;
        }

        public IReadOnlyList<SearchResult> Search(string query, int maxResults = 100)
        {
            var normalizedQuery = Normalize(query);
            if (string.IsNullOrWhiteSpace(normalizedQuery))
            {
                return Array.Empty<SearchResult>();
            }

            var results = new List<SearchResult>();
            foreach (var term in repository.GetAllTerms())
            {
                var score = ScoreTerm(term, normalizedQuery, out var matchedField);
                if (score > 0)
                {
                    results.Add(new SearchResult
                    {
                        Term = term,
                        Score = score,
                        MatchedField = matchedField
                    });
                }
            }

            return results
                .OrderByDescending(result => result.Score)
                .ThenBy(result => result.Term.Term, StringComparer.OrdinalIgnoreCase)
                .Take(maxResults)
                .ToList();
        }

        private static int ScoreTerm(GlossaryTermData term, string query, out string matchedField)
        {
            matchedField = string.Empty;
            var bestScore = 0;

            bestScore = Math.Max(bestScore, ScoreToken(term.Term, query, "term", ref matchedField));
            bestScore = Math.Max(bestScore, ScoreToken(term.Abbreviation, query, "abbreviation", ref matchedField));

            foreach (var tag in term.Tags ?? Enumerable.Empty<string>())
            {
                bestScore = Math.Max(bestScore, ScoreToken(tag, query, "tag", ref matchedField));
            }

            foreach (var synonym in term.Synonyms ?? Enumerable.Empty<string>())
            {
                bestScore = Math.Max(bestScore, ScoreToken(synonym, query, "synonym", ref matchedField));
            }

            if (ContainsNormalized(term.ShortDefinition, query))
            {
                bestScore = Math.Max(bestScore, 40);
                matchedField = string.IsNullOrEmpty(matchedField) ? "definition" : matchedField;
            }

            return bestScore;
        }

        private static int ScoreToken(string value, string query, string fieldName, ref string matchedField)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            var normalized = Normalize(value);
            if (normalized == query)
            {
                matchedField = fieldName;
                return fieldName == "abbreviation" ? 120 : 100;
            }

            if (normalized.StartsWith(query, StringComparison.Ordinal))
            {
                matchedField = fieldName;
                return fieldName == "abbreviation" ? 95 : 80;
            }

            if (normalized.Contains(query, StringComparison.Ordinal))
            {
                matchedField = fieldName;
                return fieldName == "abbreviation" ? 75 : 60;
            }

            return 0;
        }

        public static string Normalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var builder = new StringBuilder(value.Length);
            var previousWasSpace = false;
            foreach (var character in value.Trim().ToLowerInvariant())
            {
                if (char.IsWhiteSpace(character))
                {
                    if (!previousWasSpace)
                    {
                        builder.Append(' ');
                        previousWasSpace = true;
                    }

                    continue;
                }

                builder.Append(character);
                previousWasSpace = false;
            }

            return builder.ToString();
        }

        private static bool ContainsNormalized(string value, string query)
        {
            return !string.IsNullOrWhiteSpace(value) && Normalize(value).Contains(query, StringComparison.Ordinal);
        }

        public static string HighlightMatches(string text, string query)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(query))
            {
                return text ?? string.Empty;
            }

            var normalizedQuery = Normalize(query);
            var lowerText = text.ToLowerInvariant();
            var index = lowerText.IndexOf(normalizedQuery, StringComparison.Ordinal);
            if (index < 0)
            {
                return text;
            }

            var end = index + normalizedQuery.Length;
            return $"{text[..index]}<color=#5CC8FF>{text[index..end]}</color>{text[end..]}";
        }
    }
}
