using System;
using System.Collections.Generic;
using System.Linq;

namespace GameTerms
{
    public sealed class DailyTermService
    {
        private readonly GlossaryService glossaryService;

        public DailyTermService(GlossaryService glossaryService)
        {
            this.glossaryService = glossaryService;
        }

        public GlossaryTermData GetTermOfTheDay(DateTime? date = null)
        {
            var terms = glossaryService.GetAllTerms();
            if (terms.Count == 0)
            {
                return null;
            }

            var targetDate = (date ?? DateTime.Now).Date;
            var ordered = terms.OrderBy(term => term.Id, StringComparer.Ordinal).ToList();
            var dayIndex = (int)(targetDate.ToUniversalTime() - new DateTime(2020, 1, 1)).TotalDays;
            if (dayIndex < 0)
            {
                dayIndex = Math.Abs(dayIndex);
            }

            var index = dayIndex % ordered.Count;
            return ordered[index];
        }
    }
}
