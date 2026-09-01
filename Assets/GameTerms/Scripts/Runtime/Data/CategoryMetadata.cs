using System.Collections.Generic;

namespace GameTerms
{
    public static class CategoryMetadata
    {
        private static readonly Dictionary<GlossaryCategory, (string DisplayName, string Icon)> Data =
            new()
            {
                { GlossaryCategory.GameDesign, ("Game Design", "GD") },
                { GlossaryCategory.Programming, ("Programming", "PR") },
                { GlossaryCategory.GraphicsAndRendering, ("Graphics & Rendering", "GR") },
                { GlossaryCategory.ArtAndAnimation, ("Art & Animation", "AA") },
                { GlossaryCategory.Audio, ("Audio", "AU") },
                { GlossaryCategory.Multiplayer, ("Multiplayer", "MP") },
                { GlossaryCategory.MobileDevelopment, ("Mobile Development", "MO") },
                { GlossaryCategory.QATesting, ("QA & Testing", "QA") },
                { GlossaryCategory.AnalyticsMonetization, ("Analytics & Monetization", "AN") },
                { GlossaryCategory.ProductionPublishing, ("Production & Publishing", "PP") }
            };

        public static string GetDisplayName(GlossaryCategory category)
        {
            return Data.TryGetValue(category, out var value) ? value.DisplayName : category.ToString();
        }

        public static string GetIconLabel(GlossaryCategory category)
        {
            return Data.TryGetValue(category, out var value) ? value.Icon : "?";
        }

        public static IReadOnlyList<GlossaryCategory> AllCategories { get; } = new[]
        {
            GlossaryCategory.GameDesign,
            GlossaryCategory.Programming,
            GlossaryCategory.GraphicsAndRendering,
            GlossaryCategory.ArtAndAnimation,
            GlossaryCategory.Audio,
            GlossaryCategory.Multiplayer,
            GlossaryCategory.MobileDevelopment,
            GlossaryCategory.QATesting,
            GlossaryCategory.AnalyticsMonetization,
            GlossaryCategory.ProductionPublishing
        };
    }
}
