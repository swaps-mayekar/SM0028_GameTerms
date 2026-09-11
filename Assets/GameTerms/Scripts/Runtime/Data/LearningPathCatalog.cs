using System.Collections.Generic;

namespace GameTerms
{
    public static class LearningPathCatalog
    {
        public static IReadOnlyList<LearningPathDefinition> All { get; } = BuildAll();

        public static LearningPathDefinition Get(string pathId)
        {
            foreach (var path in All)
            {
                if (path.Id == pathId)
                {
                    return path;
                }
            }

            return null;
        }

        private static IReadOnlyList<LearningPathDefinition> BuildAll()
        {
            return new List<LearningPathDefinition>
            {
                BuildBeginner(),
                BuildProgrammer(),
                BuildArtist(),
                BuildProducer()
            };
        }

        private static LearningPathDefinition BuildBeginner()
        {
            var lessons = new[]
            {
                ("core-loop", "Core Loop"),
                ("game-loop", "Game Loop"),
                ("onboarding", "Onboarding"),
                ("juice", "Juice"),
                ("progression", "Progression"),
                ("feedback-loop", "Feedback Loop"),
                ("mvp", "MVP"),
                ("playtesting", "Playtesting"),
                ("update-loop", "Update Loop"),
                ("delta-time", "Delta Time"),
                ("retention", "Retention"),
                ("touch-input", "Touch Input")
            };
            return BuildPath(
                "beginner",
                "Beginner Foundations",
                "Start here",
                "Learn the essential loops, feel, and shipping basics every game developer needs.",
                lessons);
        }

        private static LearningPathDefinition BuildProgrammer()
        {
            var lessons = new[]
            {
                ("update-loop", "Update Loop"),
                ("delta-time", "Delta Time"),
                ("state-machine", "State Machine"),
                ("event-system", "Event System"),
                ("object-pooling", "Object Pooling"),
                ("garbage-collection", "Garbage Collection"),
                ("serialization", "Serialization"),
                ("coroutine", "Coroutine"),
                ("fixed-timestep", "Fixed Timestep"),
                ("profiling", "Profiling"),
                ("scriptable-object", "Scriptable Object"),
                ("command-pattern", "Command Pattern")
            };
            return BuildPath(
                "programmer",
                "Programmer Track",
                "Code systems",
                "Build stronger gameplay architecture, performance habits, and engine timing skills.",
                lessons);
        }

        private static LearningPathDefinition BuildArtist()
        {
            var lessons = new[]
            {
                ("albedo-map", "Albedo Map"),
                ("uv-mapping", "UV Mapping"),
                ("sprite-sheet", "Sprite Sheet"),
                ("tileset", "Tileset"),
                ("normal-map", "Normal Map"),
                ("keyframe-animation", "Keyframe Animation"),
                ("animation-blending", "Animation Blending"),
                ("rigging", "Rigging"),
                ("skinning", "Skinning"),
                ("pbr", "PBR"),
                ("mipmap", "Mipmap"),
                ("draw-call", "Draw Call")
            };
            return BuildPath(
                "artist",
                "Artist Track",
                "Art and look",
                "Connect materials, animation, and rendering costs into a practical art pipeline.",
                lessons);
        }

        private static LearningPathDefinition BuildProducer()
        {
            var lessons = new[]
            {
                ("mvp", "MVP"),
                ("game-design-document", "Game Design Document"),
                ("sprint", "Sprint"),
                ("milestone", "Milestone"),
                ("soft-launch", "Soft Launch"),
                ("alpha-beta", "Alpha / Beta"),
                ("release-candidate", "Release Candidate"),
                ("live-ops", "Live Ops"),
                ("retention", "Retention"),
                ("dau", "DAU"),
                ("in-app-purchase", "In-App Purchase"),
                ("playtesting", "Playtesting")
            };
            return BuildPath(
                "producer",
                "Producer Track",
                "Ship and operate",
                "Practice planning, launch readiness, live operations, and growth metrics.",
                lessons);
        }

        private static LearningPathDefinition BuildPath(
            string pathId,
            string title,
            string subtitle,
            string description,
            (string TermId, string Title)[] lessons)
        {
            var steps = new List<LearningPathStep>();
            var allLessonIds = new List<string>();

            for (var i = 0; i < lessons.Length; i++)
            {
                var lessonNumber = i + 1;
                var lesson = lessons[i];
                allLessonIds.Add(lesson.TermId);
                steps.Add(new LearningPathStep
                {
                    Id = $"{pathId}-l{lessonNumber:00}",
                    Type = LearningPathStepType.Lesson,
                    Title = lesson.Title,
                    TermId = lesson.TermId,
                    QuestionCount = 0,
                    PassScore = 0
                });

                if (lessonNumber == 4 || lessonNumber == 8)
                {
                    var checkpointIndex = lessonNumber / 4;
                    var quizPool = new List<string>();
                    for (var q = lessonNumber - 4; q < lessonNumber; q++)
                    {
                        quizPool.Add(lessons[q].TermId);
                    }

                    steps.Add(new LearningPathStep
                    {
                        Id = $"{pathId}-cp{checkpointIndex}",
                        Type = LearningPathStepType.Checkpoint,
                        Title = $"Checkpoint {checkpointIndex}",
                        QuizTermIds = quizPool,
                        QuestionCount = 4,
                        PassScore = 3
                    });
                }
            }

            steps.Add(new LearningPathStep
            {
                Id = $"{pathId}-final",
                Type = LearningPathStepType.FinalAssessment,
                Title = "Final Assessment",
                QuizTermIds = allLessonIds,
                QuestionCount = 10,
                PassScore = 7
            });

            return new LearningPathDefinition
            {
                Id = pathId,
                Title = title,
                Subtitle = subtitle,
                Description = description,
                Steps = steps
            };
        }
    }
}
