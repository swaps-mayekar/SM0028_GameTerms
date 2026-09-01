using System;
using System.Collections.Generic;
using System.Linq;
using GameTerms.Persistence;

namespace GameTerms
{
    public sealed class RandomTermService
    {
        private readonly GlossaryService glossaryService;
        private readonly IUserDataStore dataStore;
        private readonly Random random = new();
        private UserDataSnapshot snapshot;

        public RandomTermService(GlossaryService glossaryService, IUserDataStore dataStore)
        {
            this.glossaryService = glossaryService;
            this.dataStore = dataStore;
            snapshot = dataStore.Load();
        }

        public GlossaryTermData GetRandomTerm()
        {
            var terms = glossaryService.GetAllTerms();
            if (terms.Count == 0)
            {
                return null;
            }

            if (terms.Count == 1)
            {
                return terms[0];
            }

            var candidates = terms
                .Where(term => term.Id != snapshot.LastRandomTermId)
                .ToList();

            if (candidates.Count == 0)
            {
                candidates = terms.ToList();
            }

            var selected = candidates[random.Next(candidates.Count)];
            snapshot.LastRandomTermId = selected.Id;
            dataStore.Save(snapshot);
            return selected;
        }
    }
}
