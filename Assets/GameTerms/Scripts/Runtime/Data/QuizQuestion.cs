using System;
using System.Collections.Generic;

namespace GameTerms
{
    public enum QuizQuestionType
    {
        DefinitionToTerm
    }

    public sealed class QuizOption
    {
        public string Id;
        public string Label;
        public bool IsCorrect;
    }

    public sealed class QuizQuestion
    {
        public string TermId;
        public QuizQuestionType Type = QuizQuestionType.DefinitionToTerm;
        public string Prompt;
        public List<QuizOption> Options = new();
        public string CorrectOptionId;
        public string Explanation;
    }

    public sealed class QuizAnswerRecord
    {
        public string TermId;
        public string SelectedOptionId;
        public bool IsCorrect;
    }

    public sealed class QuizSessionResult
    {
        public int TotalQuestions;
        public int CorrectCount;
        public List<string> MissedTermIds = new();
        public List<QuizAnswerRecord> Answers = new();
    }
}
