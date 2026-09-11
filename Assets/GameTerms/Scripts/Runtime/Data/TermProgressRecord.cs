using System;

namespace GameTerms
{
    [Serializable]
    public sealed class TermProgressRecord
    {
        public string TermId;
        public int CorrectCount;
        public int IncorrectCount;
        public int MasteryLevel;
        public long LastReviewedUnix;
        public long NextReviewUnix;
    }
}
