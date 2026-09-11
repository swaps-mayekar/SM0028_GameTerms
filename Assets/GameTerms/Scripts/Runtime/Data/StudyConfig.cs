namespace GameTerms
{
    public enum StudyScope
    {
        All,
        Category,
        Favorites,
        DueForReview,
        TermList
    }

    public enum StudyMode
    {
        Flashcards,
        Quiz
    }

    public sealed class StudyConfig
    {
        public StudyMode Mode = StudyMode.Quiz;
        public StudyScope Scope = StudyScope.All;
        public GlossaryCategory? Category;
        public int QuestionCount = 5;
        public int CardCount = 10;
        public string FocusTermId;
        public System.Collections.Generic.List<string> TermIds = new();
        public string PathId;
        public string PathStepId;
    }
}
