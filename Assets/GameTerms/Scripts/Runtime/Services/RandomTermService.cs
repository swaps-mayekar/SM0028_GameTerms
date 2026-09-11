using System;
using System.Linq;

namespace GameTerms
{
    public sealed class RandomTermService
    {
        private readonly GlossaryService glossaryService;
        private readonly UserDataService userData;
        private readonly Random random = new();

        public RandomTermService(GlossaryService glossaryService, UserDataService userData)
        {
            this.glossaryService = glossaryService;
            this.userData = userData;
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
                .Where(term => term.Id != userData.Snapshot.LastRandomTermId)
                .ToList();

            if (candidates.Count == 0)
            {
                candidates = terms.ToList();
            }

            var selected = candidates[random.Next(candidates.Count)];
            userData.Snapshot.LastRandomTermId = selected.Id;
            userData.Save();
            return selected;
        }
    }
}
