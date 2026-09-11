using System;
using System.Collections.Generic;
using System.Linq;

namespace GameTerms
{
    public enum FlashcardSide
    {
        Front,
        Back
    }

    public sealed class FlashcardService
    {
        private readonly GlossaryService glossary;
        private readonly ProgressService progress;
        private readonly FavoritesService favorites;
        private readonly Random random;

        private StudyConfig config;
        private List<GlossaryTermData> deck = new();
        private int currentIndex;
        private int knownThisSession;
        private int reviewedThisSession;

        public FlashcardService(GlossaryService glossary, ProgressService progress, FavoritesService favorites, int? seed = null)
        {
            this.glossary = glossary;
            this.progress = progress;
            this.favorites = favorites;
            random = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        public bool HasActiveSession => deck.Count > 0 && currentIndex < deck.Count;
        public bool IsComplete => deck.Count > 0 && currentIndex >= deck.Count;
        public int CurrentIndex => currentIndex;
        public int DeckCount => deck.Count;
        public int KnownThisSession => knownThisSession;
        public int ReviewedThisSession => reviewedThisSession;
        public FlashcardSide Side { get; private set; } = FlashcardSide.Front;

        public GlossaryTermData CurrentCard => HasActiveSession ? deck[currentIndex] : null;

        public void StartSession(StudyConfig studyConfig)
        {
            config = studyConfig ?? new StudyConfig { Mode = StudyMode.Flashcards, CardCount = 10 };
            config.CardCount = Math.Max(1, config.CardCount);
            deck = BuildDeck(config);
            currentIndex = 0;
            knownThisSession = 0;
            reviewedThisSession = 0;
            Side = FlashcardSide.Front;
            progress.RecordStudyActivity();
        }

        public void Flip()
        {
            if (!HasActiveSession)
            {
                return;
            }

            Side = Side == FlashcardSide.Front ? FlashcardSide.Back : FlashcardSide.Front;
        }

        public void Reveal()
        {
            if (!HasActiveSession)
            {
                return;
            }

            Side = FlashcardSide.Back;
        }

        public void MarkKnown()
        {
            if (!HasActiveSession)
            {
                return;
            }

            progress.MarkKnown(CurrentCard.Id);
            knownThisSession++;
            reviewedThisSession++;
            Advance();
        }

        public void MarkUnknown()
        {
            if (!HasActiveSession)
            {
                return;
            }

            progress.MarkUnknown(CurrentCard.Id);
            reviewedThisSession++;
            Advance();
        }

        private void Advance()
        {
            currentIndex++;
            Side = FlashcardSide.Front;
        }

        private List<GlossaryTermData> BuildDeck(StudyConfig studyConfig)
        {
            var dueIds = new HashSet<string>(progress.GetDueTermIds(), StringComparer.Ordinal);
            List<GlossaryTermData> pool;

            if (!string.IsNullOrWhiteSpace(studyConfig.FocusTermId))
            {
                var focus = glossary.GetTerm(studyConfig.FocusTermId);
                pool = focus != null ? new List<GlossaryTermData> { focus } : new List<GlossaryTermData>();
            }
            else
            {
                pool = studyConfig.Scope switch
                {
                    StudyScope.Category when studyConfig.Category.HasValue => glossary.GetTermsByCategory(studyConfig.Category.Value).ToList(),
                    StudyScope.Favorites => glossary.GetAllTerms()
                        .Where(term => favorites.GetFavoriteIds().Contains(term.Id))
                        .ToList(),
                    StudyScope.DueForReview => glossary.GetAllTerms()
                        .Where(term => dueIds.Contains(term.Id))
                        .ToList(),
                    _ => glossary.GetAllTerms().ToList()
                };
            }

            if (pool.Count == 0)
            {
                pool = glossary.GetAllTerms().ToList();
            }

            var prioritized = pool
                .OrderBy(term => progress.GetMasteryLevel(term.Id) >= 3 ? 1 : 0)
                .ThenBy(term => dueIds.Contains(term.Id) ? 0 : 1)
                .ThenBy(_ => random.Next())
                .Take(Math.Min(studyConfig.CardCount, pool.Count))
                .ToList();

            return prioritized;
        }
    }
}
