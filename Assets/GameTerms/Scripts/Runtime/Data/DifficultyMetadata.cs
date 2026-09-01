using UnityEngine;

namespace GameTerms
{
    public static class DifficultyMetadata
    {
        public static string GetLabel(DifficultyLevel difficulty)
        {
            return difficulty switch
            {
                DifficultyLevel.Beginner => "BEGINNER",
                DifficultyLevel.Intermediate => "INTERMEDIATE",
                DifficultyLevel.Advanced => "ADVANCED",
                _ => difficulty.ToString().ToUpperInvariant()
            };
        }

        public static string GetIcon(DifficultyLevel difficulty)
        {
            return difficulty switch
            {
                DifficultyLevel.Beginner => "●",
                DifficultyLevel.Intermediate => "●●",
                DifficultyLevel.Advanced => "●●●",
                _ => "●"
            };
        }

        public static Color GetColor(DifficultyLevel difficulty)
        {
            return difficulty switch
            {
                DifficultyLevel.Beginner => new Color(0.35f, 0.82f, 0.55f),
                DifficultyLevel.Intermediate => new Color(0.95f, 0.72f, 0.28f),
                DifficultyLevel.Advanced => new Color(0.95f, 0.42f, 0.42f),
                _ => Color.white
            };
        }
    }
}
