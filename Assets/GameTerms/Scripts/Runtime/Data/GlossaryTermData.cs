using System;
using System.Collections.Generic;
using System.Linq;

namespace GameTerms
{
    [Serializable]
    public sealed class GlossaryTermData
    {
        public string Id;
        public string Term;
        public string Abbreviation;
        public GlossaryCategory Category;
        public DifficultyLevel Difficulty;
        public string ShortDefinition;
        public string SimpleExplanation;
        public string WhyItMatters;
        public string Example;
        public string CommonMistake;
        public string PracticePrompt;
        public List<string> GameUses = new();
        public List<string> RelatedTermIds = new();
        public List<string> Tags = new();
        public List<string> Synonyms = new();
        public string CodeExample;
        public bool HasDiagram;
        public DiagramType DiagramType;
        public long AddedAtUnix;

        public GlossaryTermData Clone()
        {
            return new GlossaryTermData
            {
                Id = Id,
                Term = Term,
                Abbreviation = Abbreviation,
                Category = Category,
                Difficulty = Difficulty,
                ShortDefinition = ShortDefinition,
                SimpleExplanation = SimpleExplanation,
                WhyItMatters = WhyItMatters,
                Example = Example,
                CommonMistake = CommonMistake,
                PracticePrompt = PracticePrompt,
                GameUses = new List<string>(GameUses ?? new List<string>()),
                RelatedTermIds = new List<string>(RelatedTermIds ?? new List<string>()),
                Tags = new List<string>(Tags ?? new List<string>()),
                Synonyms = new List<string>(Synonyms ?? new List<string>()),
                CodeExample = CodeExample,
                HasDiagram = HasDiagram,
                DiagramType = DiagramType,
                AddedAtUnix = AddedAtUnix
            };
        }

        public IEnumerable<string> GetSearchTokens()
        {
            if (!string.IsNullOrWhiteSpace(Term))
            {
                yield return Term;
            }

            if (!string.IsNullOrWhiteSpace(Abbreviation))
            {
                yield return Abbreviation;
            }

            foreach (var tag in Tags ?? Enumerable.Empty<string>())
            {
                if (!string.IsNullOrWhiteSpace(tag))
                {
                    yield return tag;
                }
            }

            foreach (var synonym in Synonyms ?? Enumerable.Empty<string>())
            {
                if (!string.IsNullOrWhiteSpace(synonym))
                {
                    yield return synonym;
                }
            }
        }
    }
}
