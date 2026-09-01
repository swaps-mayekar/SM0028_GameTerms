using System;
using System.Collections.Generic;

namespace GameTerms.Editor
{
    public static class GlossaryContentDefinitions
    {
        public static IReadOnlyList<GlossaryTermData> CreateAllTerms()
        {
            var baseDate = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();
            var terms = new List<GlossaryTermData>();
            var index = 0;

            void Add(GlossaryTermData term)
            {
                term.AddedAtUnix = baseDate + index * 86400L;
                terms.Add(term);
                index++;
            }

            // Game Design
            Add(new GlossaryTermData
            {
                Id = "core-loop",
                Term = "Core Loop",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "The repeating cycle of actions players perform most often during play.",
                SimpleExplanation = "The main thing players do again and again because it feels good and moves them forward.",
                WhyItMatters = "A clear core loop keeps sessions focused and helps teams prioritize features that reinforce the primary experience.",
                Example = "In a farming game, plant crops, wait, harvest, sell, and upgrade tools.",
                GameUses = new List<string> { "Session design", "Prototype validation", "Feature prioritization" },
                Tags = new List<string> { "design", "loop", "retention" },
                Synonyms = new List<string> { "primary loop" },
                RelatedTermIds = new List<string> { "game-loop", "progression" }
            });

            Add(new GlossaryTermData
            {
                Id = "game-loop",
                Term = "Game Loop",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "The full cycle of player input, game response, feedback, and state change.",
                SimpleExplanation = "What the player does, what the game does back, and how the world updates before the next action.",
                WhyItMatters = "Understanding the loop helps designers tune pacing, rewards, and failure recovery.",
                Example = "Move, attack, receive damage feedback, earn loot, upgrade gear, repeat.",
                GameUses = new List<string> { "Combat tuning", "Tutorial pacing", "Progression planning" },
                Tags = new List<string> { "design", "feedback", "systems" },
                RelatedTermIds = new List<string> { "core-loop", "progression", "difficulty-curve" }
            });

            Add(new GlossaryTermData
            {
                Id = "vertical-slice",
                Term = "Vertical Slice",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A polished sample that demonstrates core gameplay across multiple systems at near-final quality.",
                SimpleExplanation = "A small playable section that shows how the full game should look, feel, and work.",
                WhyItMatters = "Vertical slices align teams around a shared quality bar before scaling production.",
                Example = "One combat encounter with final animation, audio, UI, and progression rewards.",
                GameUses = new List<string> { "Pitch demos", "Milestone reviews", "Production alignment" },
                Tags = new List<string> { "production", "prototype", "milestone" },
                RelatedTermIds = new List<string> { "mvp", "core-loop" }
            });

            Add(new GlossaryTermData
            {
                Id = "progression",
                Term = "Progression",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "How players unlock new abilities, content, or power over time.",
                SimpleExplanation = "The path that makes players feel stronger, smarter, or more capable as they keep playing.",
                WhyItMatters = "Strong progression creates motivation and helps structure content delivery.",
                Example = "Unlock new biomes after defeating bosses and crafting better gear.",
                GameUses = new List<string> { "RPG systems", "Level gating", "Live content rollout" },
                Tags = new List<string> { "design", "retention", "rewards" },
                RelatedTermIds = new List<string> { "difficulty-curve", "core-loop", "live-ops" }
            });

            Add(new GlossaryTermData
            {
                Id = "difficulty-curve",
                Term = "Difficulty Curve",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "How challenge rises across a game to match growing player skill.",
                SimpleExplanation = "The shape of how hard the game gets from start to finish.",
                WhyItMatters = "A well-shaped curve prevents boredom early and frustration late.",
                Example = "Introduce one new enemy type per chapter while gradually reducing reaction time windows.",
                GameUses = new List<string> { "Level design", "Combat balancing", "Tutorial planning" },
                Tags = new List<string> { "balance", "pacing", "challenge" },
                RelatedTermIds = new List<string> { "progression", "game-loop" }
            });

            // Programming
            Add(new GlossaryTermData
            {
                Id = "object-pooling",
                Term = "Object Pooling",
                Abbreviation = "pool",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A technique where reusable objects are created in advance and activated when needed instead of repeatedly creating and destroying them.",
                SimpleExplanation = "Keep a collection of reusable objects instead of constantly creating new ones.",
                WhyItMatters = "Pooling reduces allocation spikes and garbage collection pressure during intense gameplay.",
                Example = "Reuse bullet instances by disabling them after impact and returning them to a pool.",
                GameUses = new List<string> { "Shooters", "Particle effects", "Mobile performance" },
                Tags = new List<string> { "performance", "memory", "optimization" },
                Synonyms = new List<string> { "pooling", "object pool" },
                RelatedTermIds = new List<string> { "garbage-collection", "draw-call" },
                CodeExample = "GameObject obj = pool.Get();\nobj.SetActive(true);",
                HasDiagram = true,
                DiagramType = DiagramType.ObjectPooling
            });

            Add(new GlossaryTermData
            {
                Id = "garbage-collection",
                Term = "Garbage Collection",
                Abbreviation = "gc",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Automatic memory cleanup that reclaims unused objects during runtime.",
                SimpleExplanation = "The system that finds memory you are no longer using and frees it for you.",
                WhyItMatters = "Collection pauses can cause frame hitches, especially on mobile and console targets.",
                Example = "Allocating new strings every frame can trigger frequent GC spikes.",
                GameUses = new List<string> { "Performance profiling", "Mobile optimization", "Frame pacing" },
                Tags = new List<string> { "memory", "performance", "runtime" },
                Synonyms = new List<string> { "gc" },
                RelatedTermIds = new List<string> { "object-pooling", "memory-pressure" }
            });

            Add(new GlossaryTermData
            {
                Id = "state-machine",
                Term = "State Machine",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A model where an object exists in one state at a time and transitions based on rules.",
                SimpleExplanation = "A system that switches behavior cleanly between modes like idle, run, jump, or attack.",
                WhyItMatters = "State machines make complex character and AI behavior easier to debug and extend.",
                Example = "An enemy transitions from patrol to chase when the player enters detection range.",
                GameUses = new List<string> { "Character controllers", "AI behavior", "UI flows" },
                Tags = new List<string> { "architecture", "ai", "logic" },
                RelatedTermIds = new List<string> { "dependency-injection", "serialization" },
                HasDiagram = true,
                DiagramType = DiagramType.StateMachine
            });

            Add(new GlossaryTermData
            {
                Id = "serialization",
                Term = "Serialization",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Converting runtime data into a storable or transmittable format and back again.",
                SimpleExplanation = "Turning game data into save files, network packets, or config files.",
                WhyItMatters = "Reliable serialization is essential for saves, multiplayer sync, and content pipelines.",
                Example = "Write player inventory to JSON before closing the app.",
                GameUses = new List<string> { "Save systems", "Networking", "Editor tools" },
                Tags = new List<string> { "data", "save", "pipeline" },
                RelatedTermIds = new List<string> { "state-machine", "dependency-injection" }
            });

            Add(new GlossaryTermData
            {
                Id = "dependency-injection",
                Term = "Dependency Injection",
                Abbreviation = "di",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "A pattern where required services are provided to a class instead of created inside it.",
                SimpleExplanation = "Pass in the tools a class needs rather than building them internally.",
                WhyItMatters = "Injection improves testability and reduces tight coupling between systems.",
                Example = "Inject an audio service into a weapon class instead of calling a global singleton.",
                GameUses = new List<string> { "Service architecture", "Unit testing", "Modular gameplay code" },
                Tags = new List<string> { "architecture", "testing", "services" },
                Synonyms = new List<string> { "di" },
                RelatedTermIds = new List<string> { "state-machine", "serialization" }
            });

            // Graphics & Rendering
            Add(new GlossaryTermData
            {
                Id = "draw-call",
                Term = "Draw Call",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A command that tells the GPU to render a batch of geometry with a specific material setup.",
                SimpleExplanation = "Each request sent to the GPU to draw something on screen.",
                WhyItMatters = "Too many draw calls can bottleneck rendering, especially on mobile GPUs.",
                Example = "Ten unique materials on ten objects may produce ten separate draw calls.",
                GameUses = new List<string> { "Mobile optimization", "Batching strategy", "Profiling" },
                Tags = new List<string> { "rendering", "gpu", "performance" },
                Synonyms = new List<string> { "draw" },
                RelatedTermIds = new List<string> { "overdraw", "level-of-detail", "occlusion-culling" }
            });

            Add(new GlossaryTermData
            {
                Id = "occlusion-culling",
                Term = "Occlusion Culling",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A rendering optimization that skips objects hidden behind other geometry.",
                SimpleExplanation = "Do not render things the player cannot see.",
                WhyItMatters = "Rendering invisible objects wastes GPU resources.",
                Example = "Rooms behind a closed door are excluded from the render list.",
                GameUses = new List<string> { "Large environments", "Indoor levels", "Mobile optimization" },
                Tags = new List<string> { "culling", "optimization", "visibility" },
                RelatedTermIds = new List<string> { "frustum-culling", "level-of-detail", "draw-call" },
                HasDiagram = true,
                DiagramType = DiagramType.OcclusionCulling
            });

            Add(new GlossaryTermData
            {
                Id = "frustum-culling",
                Term = "Frustum Culling",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Skipping objects outside the camera view volume before rendering.",
                SimpleExplanation = "Ignore objects that are off-screen or behind the camera.",
                WhyItMatters = "It avoids wasting GPU work on geometry the player will never see in the current frame.",
                Example = "Trees behind the camera are removed from the render queue.",
                GameUses = new List<string> { "Open worlds", "Large scenes", "Performance budgets" },
                Tags = new List<string> { "culling", "camera", "rendering" },
                RelatedTermIds = new List<string> { "occlusion-culling", "draw-call" },
                HasDiagram = true,
                DiagramType = DiagramType.FrustumCulling
            });

            Add(new GlossaryTermData
            {
                Id = "level-of-detail",
                Term = "Level of Detail",
                Abbreviation = "lod",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Using simpler versions of assets at greater distances to save rendering cost.",
                SimpleExplanation = "Show a detailed model up close and a cheaper one far away.",
                WhyItMatters = "LOD preserves visual quality near the player while protecting frame rate.",
                Example = "A hero mesh swaps to a lower polygon version beyond 30 meters.",
                GameUses = new List<string> { "Open worlds", "Crowd scenes", "Mobile rendering" },
                Tags = new List<string> { "lod", "optimization", "meshes" },
                Synonyms = new List<string> { "lod" },
                RelatedTermIds = new List<string> { "draw-call", "texture-compression" },
                HasDiagram = true,
                DiagramType = DiagramType.LevelOfDetail
            });

            Add(new GlossaryTermData
            {
                Id = "overdraw",
                Term = "Overdraw",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "Rendering the same screen pixel multiple times due to overlapping transparent or layered geometry.",
                SimpleExplanation = "Drawing on top of the same pixel again and again.",
                WhyItMatters = "High overdraw can crush fill-rate performance, especially on mobile GPUs.",
                Example = "Multiple fullscreen particle layers stacking in the same view.",
                GameUses = new List<string> { "VFX budgeting", "UI transparency review", "Mobile tuning" },
                Tags = new List<string> { "rendering", "gpu", "fill rate" },
                RelatedTermIds = new List<string> { "draw-call", "occlusion-culling" }
            });

            // Art & Animation
            Add(new GlossaryTermData
            {
                Id = "normal-map",
                Term = "Normal Map",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A texture that fakes fine surface detail by altering how light interacts with a mesh.",
                SimpleExplanation = "A flat image that makes a surface look bumpy or detailed without extra geometry.",
                WhyItMatters = "Normal maps add visual richness while keeping polygon counts practical.",
                Example = "Brick wall detail is painted into a normal map on a simple plane.",
                GameUses = new List<string> { "Environment art", "Character materials", "Mobile asset budgets" },
                Tags = new List<string> { "texturing", "lighting", "materials" },
                RelatedTermIds = new List<string> { "uv-mapping", "texture-compression" }
            });

            Add(new GlossaryTermData
            {
                Id = "rigging",
                Term = "Rigging",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Building the bone and control structure that deforms a character model during animation.",
                SimpleExplanation = "Creating the skeleton and handles that make a model move.",
                WhyItMatters = "A clean rig determines how expressive, stable, and reusable animations can be.",
                Example = "Shoulder, spine, and finger controls are added to a hero character mesh.",
                GameUses = new List<string> { "Character animation", "Cinematics", "Facial performance" },
                Tags = new List<string> { "animation", "characters", "pipeline" },
                RelatedTermIds = new List<string> { "skinning", "uv-mapping" }
            });

            Add(new GlossaryTermData
            {
                Id = "skinning",
                Term = "Skinning",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Binding mesh vertices to bones so the model follows skeletal animation.",
                SimpleExplanation = "Connecting a character mesh to its skeleton so it bends correctly.",
                WhyItMatters = "Poor skin weights create ugly deformation and break immersion.",
                Example = "Elbow vertices follow upper and lower arm bones with blended influence.",
                GameUses = new List<string> { "Character setup", "Animation polish", "Runtime deformation" },
                Tags = new List<string> { "animation", "weights", "characters" },
                RelatedTermIds = new List<string> { "rigging", "normal-map" }
            });

            Add(new GlossaryTermData
            {
                Id = "uv-mapping",
                Term = "UV Mapping",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Flattening a 3D surface into 2D texture coordinates for painting and shading.",
                SimpleExplanation = "Unwrapping a model so textures can be painted on it like wrapping paper.",
                WhyItMatters = "Good UV layouts improve texture clarity and reduce visible seams.",
                Example = "A character torso is unwrapped so armor details align across mesh islands.",
                GameUses = new List<string> { "Texturing", "Material authoring", "Asset optimization" },
                Tags = new List<string> { "texturing", "pipeline", "art" },
                RelatedTermIds = new List<string> { "normal-map", "texture-compression" }
            });

            // Audio
            Add(new GlossaryTermData
            {
                Id = "foley",
                Term = "Foley",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Everyday sound effects recorded to match on-screen physical actions.",
                SimpleExplanation = "Custom recorded sounds for footsteps, cloth, impacts, and object handling.",
                WhyItMatters = "Foley makes interactions feel tactile and grounded in the game world.",
                Example = "Recording leather creaks and metal clanks for a character gear swap.",
                GameUses = new List<string> { "Character interaction", "Cinematic polish", "Immersion" },
                Tags = new List<string> { "sound design", "sfx", "immersion" },
                RelatedTermIds = new List<string> { "diegetic-audio", "audio-ducking" }
            });

            Add(new GlossaryTermData
            {
                Id = "diegetic-audio",
                Term = "Diegetic Audio",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Sound that exists within the game world and can be heard by characters.",
                SimpleExplanation = "Audio that comes from something in the scene, like a radio or monster roar.",
                WhyItMatters = "Diegetic audio strengthens world believability and spatial storytelling.",
                Example = "An alarm siren inside a facility that characters react to.",
                GameUses = new List<string> { "World building", "Horror tension", "Spatial gameplay cues" },
                Tags = new List<string> { "sound design", "immersion", "spatial" },
                RelatedTermIds = new List<string> { "foley", "audio-ducking" }
            });

            Add(new GlossaryTermData
            {
                Id = "audio-ducking",
                Term = "Audio Ducking",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Temporarily lowering one sound layer so another can be heard clearly.",
                SimpleExplanation = "Turning background audio down when something important needs attention.",
                WhyItMatters = "Ducking preserves clarity for dialogue, warnings, and critical gameplay cues.",
                Example = "Music volume dips while a mission briefing voice line plays.",
                GameUses = new List<string> { "Dialogue systems", "UI feedback", "Combat readability" },
                Tags = new List<string> { "mixing", "ui audio", "clarity" },
                RelatedTermIds = new List<string> { "foley", "diegetic-audio" }
            });

            // Multiplayer
            Add(new GlossaryTermData
            {
                Id = "latency",
                Term = "Latency",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "The delay between sending an action and seeing its result across a network.",
                SimpleExplanation = "How long it takes for your input to reach the game and come back.",
                WhyItMatters = "High latency makes controls feel sluggish and can break competitive fairness.",
                Example = "A player fires on an enemy that has already moved on the server.",
                GameUses = new List<string> { "Netcode tuning", "Competitive modes", "Input responsiveness" },
                Tags = new List<string> { "networking", "ping", "responsiveness" },
                RelatedTermIds = new List<string> { "tick-rate", "client-prediction", "rollback" }
            });

            Add(new GlossaryTermData
            {
                Id = "tick-rate",
                Term = "Tick Rate",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "How many times per second the server updates simulation and networking state.",
                SimpleExplanation = "How often the server processes the game world each second.",
                WhyItMatters = "Higher tick rates can improve precision but increase server and bandwidth cost.",
                Example = "A shooter server running 60 simulation ticks per second.",
                GameUses = new List<string> { "Server architecture", "Competitive shooters", "Sync quality" },
                Tags = new List<string> { "server", "simulation", "networking" },
                RelatedTermIds = new List<string> { "latency", "client-prediction" }
            });

            Add(new GlossaryTermData
            {
                Id = "client-prediction",
                Term = "Client Prediction",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "Letting the local client simulate actions immediately before server confirmation.",
                SimpleExplanation = "Move on your screen right away even though the server has not confirmed yet.",
                WhyItMatters = "Prediction hides network delay and keeps movement feeling responsive.",
                Example = "A player jumps locally while waiting for the authoritative server position.",
                GameUses = new List<string> { "Action games", "Platformers online", "Responsive controls" },
                Tags = new List<string> { "netcode", "responsiveness", "sync" },
                RelatedTermIds = new List<string> { "latency", "rollback", "tick-rate" }
            });

            Add(new GlossaryTermData
            {
                Id = "rollback",
                Term = "Rollback",
                Abbreviation = "ggpo",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "Rewinding and replaying simulation frames when late network inputs arrive.",
                SimpleExplanation = "Undo a few frames and replay them with the correct inputs once they arrive.",
                WhyItMatters = "Rollback enables responsive fighting and action games over real-world networks.",
                Example = "A fighting game corrects a hit after the remote input finally reaches the host.",
                GameUses = new List<string> { "Fighting games", "Fast action multiplayer", "Peer-to-peer sessions" },
                Tags = new List<string> { "netcode", "sync", "fighting" },
                Synonyms = new List<string> { "rollback netcode" },
                RelatedTermIds = new List<string> { "client-prediction", "latency" }
            });

            // Mobile Development
            Add(new GlossaryTermData
            {
                Id = "thermal-throttling",
                Term = "Thermal Throttling",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "When a device reduces CPU or GPU performance to prevent overheating.",
                SimpleExplanation = "The phone slows itself down because it is getting too hot.",
                WhyItMatters = "Throttling can cause sudden frame drops during long play sessions.",
                Example = "A game holds 60 FPS for ten minutes, then drops after sustained GPU load.",
                GameUses = new List<string> { "Mobile QA", "Graphics budgeting", "Session length testing" },
                Tags = new List<string> { "mobile", "performance", "hardware" },
                RelatedTermIds = new List<string> { "memory-pressure", "texture-compression" }
            });

            Add(new GlossaryTermData
            {
                Id = "texture-compression",
                Term = "Texture Compression",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Reducing texture memory footprint using GPU-friendly compressed formats.",
                SimpleExplanation = "Shrink image files so they use less RAM and load faster on device.",
                WhyItMatters = "Compression is critical for fitting art within mobile memory budgets.",
                Example = "Authoring albedo maps in ASTC for iOS and ETC2 for Android targets.",
                GameUses = new List<string> { "Mobile builds", "Asset pipelines", "Memory budgeting" },
                Tags = new List<string> { "textures", "memory", "mobile" },
                RelatedTermIds = new List<string> { "memory-pressure", "normal-map", "uv-mapping" }
            });

            Add(new GlossaryTermData
            {
                Id = "memory-pressure",
                Term = "Memory Pressure",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "When available device memory is low enough that the OS may reclaim app resources.",
                SimpleExplanation = "The device is running low on RAM and may force your app to free memory or close.",
                WhyItMatters = "Memory pressure causes crashes, reloads, and degraded performance on mobile.",
                Example = "Background apps are purged and your game reloads textures after a large scene load.",
                GameUses = new List<string> { "Mobile stability", "Asset streaming", "Crash prevention" },
                Tags = new List<string> { "memory", "mobile", "stability" },
                RelatedTermIds = new List<string> { "garbage-collection", "texture-compression", "thermal-throttling" }
            });

            // QA & Testing
            Add(new GlossaryTermData
            {
                Id = "regression-testing",
                Term = "Regression Testing",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Checking that recent changes did not break existing features.",
                SimpleExplanation = "Re-test old features after new work to make sure nothing got worse.",
                WhyItMatters = "Regression coverage protects live quality as teams ship frequent updates.",
                Example = "Re-run login, inventory, and combat flows after a patch changes save data.",
                GameUses = new List<string> { "Patch validation", "Live ops releases", "Automation suites" },
                Tags = new List<string> { "qa", "testing", "quality" },
                RelatedTermIds = new List<string> { "smoke-testing", "stress-testing" }
            });

            Add(new GlossaryTermData
            {
                Id = "smoke-testing",
                Term = "Smoke Testing",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "A quick pass over critical paths to confirm a build is basically functional.",
                SimpleExplanation = "A fast sanity check before deeper testing begins.",
                WhyItMatters = "Smoke tests catch catastrophic issues early and save QA time.",
                Example = "Launch the build, start a match, complete a purchase flow, and exit cleanly.",
                GameUses = new List<string> { "Daily builds", "Release gates", "CI validation" },
                Tags = new List<string> { "qa", "build", "sanity" },
                RelatedTermIds = new List<string> { "regression-testing", "stress-testing" }
            });

            Add(new GlossaryTermData
            {
                Id = "stress-testing",
                Term = "Stress Testing",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "Pushing a system beyond normal load to find breaking points and stability issues.",
                SimpleExplanation = "Hammer the game harder than players normally would to see what fails.",
                WhyItMatters = "Stress testing reveals crashes, memory leaks, and server limits before launch.",
                Example = "Spawn thousands of projectiles and AI units to measure frame time collapse.",
                GameUses = new List<string> { "Server capacity", "Performance QA", "Crash hunting" },
                Tags = new List<string> { "qa", "performance", "stability" },
                RelatedTermIds = new List<string> { "regression-testing", "memory-pressure" }
            });

            // Analytics & Monetization
            Add(new GlossaryTermData
            {
                Id = "dau",
                Term = "DAU",
                Abbreviation = "dau",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Daily Active Users: unique players who engage with a game on a given day.",
                SimpleExplanation = "How many different people played today.",
                WhyItMatters = "DAU helps teams track short-term engagement and the impact of updates or events.",
                Example = "An event boosts DAU from 120,000 to 180,000 on launch day.",
                GameUses = new List<string> { "Live ops tracking", "Event measurement", "Growth reporting" },
                Tags = new List<string> { "analytics", "engagement", "kpi" },
                Synonyms = new List<string> { "daily active users" },
                RelatedTermIds = new List<string> { "mau", "ltv", "live-ops" }
            });

            Add(new GlossaryTermData
            {
                Id = "mau",
                Term = "MAU",
                Abbreviation = "mau",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Monthly Active Users: unique players who engage within a rolling month.",
                SimpleExplanation = "How many different people played at least once this month.",
                WhyItMatters = "MAU shows broader audience health beyond day-to-day volatility.",
                Example = "A seasonal campaign lifts MAU even when daily numbers fluctuate.",
                GameUses = new List<string> { "Portfolio reporting", "Retention analysis", "Investor updates" },
                Tags = new List<string> { "analytics", "engagement", "kpi" },
                Synonyms = new List<string> { "monthly active users" },
                RelatedTermIds = new List<string> { "dau", "ltv" }
            });

            Add(new GlossaryTermData
            {
                Id = "ltv",
                Term = "LTV",
                Abbreviation = "ltv",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Lifetime Value: estimated revenue or value a player generates over their relationship with a game.",
                SimpleExplanation = "How much a player is worth across their entire time with the game.",
                WhyItMatters = "LTV guides acquisition spend, pricing, and retention investment.",
                Example = "If average LTV is $12, paid campaigns must stay below that to remain profitable.",
                GameUses = new List<string> { "Monetization strategy", "UA budgeting", "Economy tuning" },
                Tags = new List<string> { "analytics", "monetization", "kpi" },
                Synonyms = new List<string> { "lifetime value" },
                RelatedTermIds = new List<string> { "dau", "mau", "soft-launch" }
            });

            // Production & Publishing
            Add(new GlossaryTermData
            {
                Id = "mvp",
                Term = "MVP",
                Abbreviation = "mvp",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Minimum Viable Product: the smallest version that proves core value to players.",
                SimpleExplanation = "The leanest playable version that tests whether the idea works.",
                WhyItMatters = "An MVP reduces wasted production by validating fun and market fit early.",
                Example = "Ship one dungeon loop with basic progression before building the full world map.",
                GameUses = new List<string> { "Prototype validation", "Pitching", "Scope control" },
                Tags = new List<string> { "production", "scope", "prototype" },
                Synonyms = new List<string> { "minimum viable product" },
                RelatedTermIds = new List<string> { "vertical-slice", "soft-launch" }
            });

            Add(new GlossaryTermData
            {
                Id = "soft-launch",
                Term = "Soft Launch",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Releasing a game in limited regions or audiences to gather data before global launch.",
                SimpleExplanation = "A practice launch in a smaller market before going worldwide.",
                WhyItMatters = "Soft launches reveal retention, monetization, and stability issues with lower risk.",
                Example = "Launch in two countries, tune onboarding for two weeks, then expand globally.",
                GameUses = new List<string> { "Mobile publishing", "Economy tuning", "Live readiness" },
                Tags = new List<string> { "publishing", "launch", "testing" },
                RelatedTermIds = new List<string> { "mvp", "live-ops", "ltv" }
            });

            Add(new GlossaryTermData
            {
                Id = "live-ops",
                Term = "Live Ops",
                Abbreviation = "liveops",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Ongoing updates, events, and content that keep a shipped game active and healthy.",
                SimpleExplanation = "Running the game after launch with events, fixes, and fresh content.",
                WhyItMatters = "Live ops sustains engagement and revenue long after initial release.",
                Example = "Weekly events, seasonal passes, and balance patches across the first year.",
                GameUses = new List<string> { "Service games", "Seasonal content", "Retention strategy" },
                Tags = new List<string> { "operations", "updates", "retention" },
                Synonyms = new List<string> { "live operations", "liveops" },
                RelatedTermIds = new List<string> { "soft-launch", "dau", "progression" }
            });

            return terms;
        }
    }
}
