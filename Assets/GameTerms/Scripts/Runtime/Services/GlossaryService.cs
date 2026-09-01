using System;
using System.Collections.Generic;
using System.Linq;

namespace GameTerms
{
    public enum TermSortMode
    {
        Alphabetical,
        Difficulty,
        RecentlyAdded
    }

    public sealed class GlossaryService
    {
        private readonly IGlossaryRepository repository;

        public GlossaryService(IGlossaryRepository repository)
        {
            this.repository = repository;
        }

        public IReadOnlyList<GlossaryTermData> GetAllTerms() => repository.GetAllTerms();

        public GlossaryTermData GetTerm(string id) => repository.GetTermById(id);

        public IReadOnlyList<GlossaryTermData> GetTermsByCategory(GlossaryCategory category)
        {
            return SortTerms(repository.GetTermsByCategory(category), TermSortMode.Alphabetical);
        }

        public IReadOnlyList<GlossaryTermData> SortTerms(IEnumerable<GlossaryTermData> terms, TermSortMode sortMode)
        {
            return sortMode switch
            {
                TermSortMode.Difficulty => terms
                    .OrderBy(term => term.Difficulty)
                    .ThenBy(term => term.Term, StringComparer.OrdinalIgnoreCase)
                    .ToList(),
                TermSortMode.RecentlyAdded => terms
                    .OrderByDescending(term => term.AddedAtUnix)
                    .ThenBy(term => term.Term, StringComparer.OrdinalIgnoreCase)
                    .ToList(),
                _ => terms
                    .OrderBy(term => term.Term, StringComparer.OrdinalIgnoreCase)
                    .ToList()
            };
        }

        public int GetCategoryCount(GlossaryCategory category)
        {
            return repository.GetTermsByCategory(category).Count;
        }

        public IReadOnlyList<GlossaryTermData> GetRelatedTerms(GlossaryTermData term)
        {
            if (term?.RelatedTermIds == null)
            {
                return Array.Empty<GlossaryTermData>();
            }

            var related = new List<GlossaryTermData>();
            foreach (var relatedId in term.RelatedTermIds)
            {
                if (repository.TryGetTerm(relatedId, out var relatedTerm))
                {
                    related.Add(relatedTerm);
                }
            }

            return related;
        }
    }
}
