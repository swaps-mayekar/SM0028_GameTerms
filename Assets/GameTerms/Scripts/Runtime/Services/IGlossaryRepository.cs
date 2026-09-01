using System.Collections.Generic;

namespace GameTerms
{
    public interface IGlossaryRepository
    {
        IReadOnlyList<GlossaryTermData> GetAllTerms();
        GlossaryTermData GetTermById(string id);
        IReadOnlyList<GlossaryTermData> GetTermsByCategory(GlossaryCategory category);
        bool TryGetTerm(string id, out GlossaryTermData term);
    }
}
