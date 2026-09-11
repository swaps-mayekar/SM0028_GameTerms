using System;
using System.Collections.Generic;

namespace GameTerms
{
    public enum LearningPathStepType
    {
        Lesson,
        Checkpoint,
        FinalAssessment
    }

    [Serializable]
    public sealed class LearningPathStep
    {
        public string Id;
        public LearningPathStepType Type;
        public string Title;
        public string TermId;
        public List<string> QuizTermIds = new();
        public int QuestionCount;
        public int PassScore;
    }

    [Serializable]
    public sealed class LearningPathDefinition
    {
        public string Id;
        public string Title;
        public string Subtitle;
        public string Description;
        public List<LearningPathStep> Steps = new();
    }
}
