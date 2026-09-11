using System.Linq;
using GameTerms.Editor;
using NUnit.Framework;
using UnityEngine;

namespace GameTerms.Tests.Editor
{
    public class GlossaryContentTests
    {
        [Test]
        public void Content_Has150ValidatedTerms()
        {
            var terms = GlossaryContentDefinitions.CreateAllTerms();
            var issues = GlossaryContentValidator.Validate(terms);

            Assert.That(issues, Is.Empty, string.Join("\n", issues));
            Assert.That(terms.Count, Is.EqualTo(150));
            Assert.That(terms.Select(term => term.Category).Distinct().Count(), Is.EqualTo(10));

            foreach (var group in terms.GroupBy(term => term.Category))
            {
                Assert.That(group.Count(), Is.EqualTo(15), $"Category {group.Key} should have 15 terms.");
            }

            Assert.That(terms.All(term => !string.IsNullOrWhiteSpace(term.CommonMistake)), Is.True);
            Assert.That(terms.All(term => !string.IsNullOrWhiteSpace(term.PracticePrompt)), Is.True);
            Assert.That(terms.All(term => term.GameUses != null && term.GameUses.Count >= 3), Is.True);
        }

        [Test]
        public void GlossaryJson_ContainsExpandedCatalogAndTeachingFields()
        {
            var glossary = Resources.Load<TextAsset>("glossary");
            Assert.That(glossary, Is.Not.Null);

            var catalog = JsonUtility.FromJson<GlossaryCatalogJson>(glossary.text);
            Assert.That(catalog, Is.Not.Null);
            Assert.That(catalog.Terms, Is.Not.Null);
            Assert.That(catalog.Terms.Count, Is.EqualTo(150));
            Assert.That(catalog.Terms.Count(term => !string.IsNullOrWhiteSpace(term.CommonMistake)), Is.EqualTo(150));
            Assert.That(catalog.Terms.Any(term => term.Id == "addressable-assets"), Is.True);
            Assert.That(catalog.Terms.Any(term => term.Id == "metagame"), Is.True);
        }
    }
}
