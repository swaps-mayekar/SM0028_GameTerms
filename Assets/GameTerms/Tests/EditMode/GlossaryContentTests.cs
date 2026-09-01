using System.Linq;
using GameTerms.Editor;
using NUnit.Framework;

namespace GameTerms.Tests.Editor
{
    public class GlossaryContentTests
    {
        [Test]
        public void Content_Has38ValidatedTerms()
        {
            var terms = GlossaryContentDefinitions.CreateAllTerms();
            var issues = GlossaryContentValidator.Validate(terms);

            Assert.That(issues, Is.Empty);
            Assert.That(terms.Count, Is.EqualTo(38));
            Assert.That(terms.Select(term => term.Category).Distinct().Count(), Is.EqualTo(10));
        }
    }
}
