using System;
using System.Collections.Generic;

namespace GameTerms
{
    [Serializable]
    public sealed class PathProgressRecord
    {
        public string PathId;
        public List<string> CompletedStepIds = new();
        public int FinalQuizBestScore;
        public int FinalQuizBestTotal;
        public int FinalQuizAttempts;
        public long LastActivityUnix;
    }
}
