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
                CommonMistake = "Adding side systems that never feed back into the loop, so the main experience feels diluted.",
                PracticePrompt = "Write your game's core loop as 4 to 6 verbs and remove any step that does not create progress or feedback.",
                GameUses = new List<string> { "Session design", "Prototype validation", "Feature prioritization" },
                Tags = new List<string> { "design", "loop", "retention" },
                Synonyms = new List<string> { "primary loop" },
                RelatedTermIds = new List<string> { "game-loop", "progression", "feedback-loop" }
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
                CommonMistake = "Treating the game loop as only combat while ignoring downtime, recovery, and preparation.",
                PracticePrompt = "Map one full cycle of your prototype and mark where feedback arrives after each player action.",
                GameUses = new List<string> { "Combat tuning", "Tutorial pacing", "Progression planning" },
                Tags = new List<string> { "design", "feedback", "systems" },
                RelatedTermIds = new List<string> { "core-loop", "progression", "difficulty-curve", "feedback-loop" }
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
                CommonMistake = "Calling a rough greybox a vertical slice when art, audio, and feel are still placeholders.",
                PracticePrompt = "Define a one-level slice checklist covering gameplay, UI, audio, and reward feedback.",
                GameUses = new List<string> { "Pitch demos", "Milestone reviews", "Production alignment" },
                Tags = new List<string> { "production", "prototype", "milestone" },
                RelatedTermIds = new List<string> { "mvp", "core-loop", "game-design-document" }
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
                CommonMistake = "Front-loading too many unlocks so later hours feel empty.",
                PracticePrompt = "List ten unlocks and schedule them across the first five hours of play.",
                GameUses = new List<string> { "RPG systems", "Level gating", "Live content rollout" },
                Tags = new List<string> { "design", "retention", "rewards" },
                RelatedTermIds = new List<string> { "difficulty-curve", "core-loop", "live-ops", "metagame" }
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
                CommonMistake = "Spiking difficulty without teaching the skills needed to overcome the spike.",
                PracticePrompt = "Playtest one chapter and chart deaths per encounter to reshape the curve.",
                GameUses = new List<string> { "Level design", "Combat balancing", "Tutorial planning" },
                Tags = new List<string> { "balance", "pacing", "challenge" },
                RelatedTermIds = new List<string> { "progression", "game-loop", "balancing", "level-design" }
            });

            Add(new GlossaryTermData
            {
                Id = "juice",
                Term = "Juice",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Exaggerated visual, audio, and haptic feedback that makes actions feel satisfying.",
                SimpleExplanation = "The extra pop, shake, and sound that makes hitting a button or landing a jump feel great.",
                WhyItMatters = "Juice improves game feel and helps players understand cause and effect without extra UI.",
                Example = "Screen shake, particle bursts, and a punchy sound when the player collects a coin.",
                CommonMistake = "Adding effects everywhere until critical feedback becomes hard to read.",
                PracticePrompt = "Pick one player action and layer three feedback channels: visual, audio, and motion.",
                GameUses = new List<string> { "Combat feedback", "UI polish", "Mobile touch response" },
                Tags = new List<string> { "feel", "feedback", "polish" },
                RelatedTermIds = new List<string> { "core-loop", "game-loop", "game-feel" }
            });

            Add(new GlossaryTermData
            {
                Id = "onboarding",
                Term = "Onboarding",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "The guided early experience that teaches core mechanics and motivates continued play.",
                SimpleExplanation = "How the game introduces itself and teaches players what to do in the first minutes.",
                WhyItMatters = "Strong onboarding improves day-one retention and reduces early drop-off.",
                Example = "A tutorial level that teaches movement, combat, and inventory without long text blocks.",
                CommonMistake = "Dumping every system into the first ten minutes instead of teaching one skill at a time.",
                PracticePrompt = "Rewrite your first five minutes so each new input is practiced before the next is introduced.",
                GameUses = new List<string> { "First-time user experience", "Tutorial design", "Retention tuning" },
                Tags = new List<string> { "tutorial", "retention", "ux" },
                RelatedTermIds = new List<string> { "progression", "mvp", "retention", "player-agency" }
            });

            Add(new GlossaryTermData
            {
                Id = "metagame",
                Term = "Metagame",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Long-term goals and systems that sit above individual match or session play.",
                SimpleExplanation = "The bigger goals that keep players coming back after one round ends.",
                WhyItMatters = "A clear metagame turns short sessions into an ongoing habit.",
                Example = "Season ranks, collection goals, and weekly challenges that span many matches.",
                CommonMistake = "Building a metagame before the session loop itself is fun.",
                PracticePrompt = "Define one session goal and one week-long metagame goal that reinforce each other.",
                GameUses = new List<string> { "Live service planning", "Retention design", "Season structure" },
                Tags = new List<string> { "design", "retention", "liveops" },
                RelatedTermIds = new List<string> { "progression", "live-ops", "battle-pass" }
            });

            Add(new GlossaryTermData
            {
                Id = "risk-reward",
                Term = "Risk Reward",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A design pattern where greater potential payoff requires accepting greater danger or cost.",
                SimpleExplanation = "Players can play it safe for less, or take a bigger chance for a better payoff.",
                WhyItMatters = "Risk-reward choices create tension, skill expression, and memorable decisions.",
                Example = "Entering a high-level dungeon for rare loot while carrying a fragile inventory.",
                CommonMistake = "Making the risky option strictly worse so players never choose it.",
                PracticePrompt = "Design three paths through one encounter with different risk and reward profiles.",
                GameUses = new List<string> { "Encounter design", "Economy tuning", "Roguelike systems" },
                Tags = new List<string> { "design", "decision", "balance" },
                RelatedTermIds = new List<string> { "balancing", "economy-design", "player-agency" }
            });

            Add(new GlossaryTermData
            {
                Id = "player-agency",
                Term = "Player Agency",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "The player's ability to make meaningful choices that change outcomes.",
                SimpleExplanation = "Feeling like your decisions actually matter in the game world.",
                WhyItMatters = "Agency creates ownership, replay value, and emotional investment.",
                Example = "Choosing dialogue that permanently alters which ally joins a late-game mission.",
                CommonMistake = "Offering cosmetic choices that look meaningful but never affect gameplay or story.",
                PracticePrompt = "Audit one quest and mark which decisions change systems, narrative, or neither.",
                GameUses = new List<string> { "Narrative design", "Systems design", "UX clarity" },
                Tags = new List<string> { "design", "choice", "ux" },
                RelatedTermIds = new List<string> { "onboarding", "risk-reward", "feedback-loop" }
            });

            Add(new GlossaryTermData
            {
                Id = "game-feel",
                Term = "Game Feel",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "The tactile quality of controls, responsiveness, and physical feedback during play.",
                SimpleExplanation = "How good the game feels in your hands when you move, jump, or shoot.",
                WhyItMatters = "Excellent game feel can make simple mechanics compelling for long sessions.",
                Example = "A platformer jump with coyote time, jump buffering, and soft landing squash.",
                CommonMistake = "Blaming content boredom when the real issue is sluggish or imprecise controls.",
                PracticePrompt = "Record input-to-response delay for jump and attack, then reduce both below 100 ms.",
                GameUses = new List<string> { "Action games", "Mobile controls", "Prototype polish" },
                Tags = new List<string> { "feel", "controls", "juice" },
                RelatedTermIds = new List<string> { "juice", "core-loop", "touch-input" }
            });

            Add(new GlossaryTermData
            {
                Id = "balancing",
                Term = "Balancing",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Tuning numbers and rules so options remain viable without a single dominant strategy.",
                SimpleExplanation = "Adjusting systems so no weapon, character, or tactic wins every time.",
                WhyItMatters = "Balance protects fairness, variety, and long-term engagement.",
                Example = "Nerfing an overused rifle while buffing underplayed shotguns after telemetry review.",
                CommonMistake = "Balancing based only on top-player complaints without looking at overall usage data.",
                PracticePrompt = "Pick three competing options and chart win rate and pick rate after twenty playtests.",
                GameUses = new List<string> { "Combat systems", "Economy design", "Live ops patches" },
                Tags = new List<string> { "balance", "tuning", "systems" },
                RelatedTermIds = new List<string> { "difficulty-curve", "risk-reward", "economy-design", "a-b-testing" }
            });

            Add(new GlossaryTermData
            {
                Id = "economy-design",
                Term = "Economy Design",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "The structure of currencies, sources, sinks, and exchanges that shape player progression.",
                SimpleExplanation = "How money, resources, and rewards enter and leave the game.",
                WhyItMatters = "A healthy economy supports pacing, monetization, and long-term goals without inflation.",
                Example = "Gold earned from quests and spent on upgrades, repairs, and cosmetics.",
                CommonMistake = "Adding infinite earn rates without sinks, so prices and progression break.",
                PracticePrompt = "Draw a source-and-sink diagram for one currency and verify every source has a matching sink.",
                GameUses = new List<string> { "F2P design", "Crafting systems", "Live economy tuning" },
                Tags = new List<string> { "economy", "currency", "systems" },
                RelatedTermIds = new List<string> { "balancing", "progression", "in-app-purchase", "battle-pass" }
            });

            Add(new GlossaryTermData
            {
                Id = "level-design",
                Term = "Level Design",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Crafting spaces, encounters, and routes that teach, challenge, and reward players.",
                SimpleExplanation = "Building the places where gameplay actually happens.",
                WhyItMatters = "Level design translates systems into memorable experiences and readable challenge.",
                Example = "A stealth level with safe paths, risky shortcuts, and a scripted discovery moment.",
                CommonMistake = "Decorating a map before verifying traversal, sightlines, and encounter flow.",
                PracticePrompt = "Greybox one room that teaches a new mechanic using layout alone, with no tutorial text.",
                GameUses = new List<string> { "Encounter layout", "Exploration games", "Tutorial spaces" },
                Tags = new List<string> { "levels", "space", "pacing" },
                RelatedTermIds = new List<string> { "difficulty-curve", "vertical-slice", "onboarding" }
            });

            Add(new GlossaryTermData
            {
                Id = "feedback-loop",
                Term = "Feedback Loop",
                Category = GlossaryCategory.GameDesign,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A system where an action's result encourages or discourages repeating that action.",
                SimpleExplanation = "When doing something makes the next attempt easier, harder, or more tempting.",
                WhyItMatters = "Feedback loops drive mastery, addiction risk, and strategic depth.",
                Example = "Landing combos fills a meter that unlocks stronger finishers.",
                CommonMistake = "Creating runaway positive loops with no counterplay or soft caps.",
                PracticePrompt = "Identify one positive and one negative feedback loop in your prototype and document their caps.",
                GameUses = new List<string> { "Combat meters", "Economy sinks", "Skill expression" },
                Tags = new List<string> { "systems", "feedback", "loop" },
                RelatedTermIds = new List<string> { "core-loop", "game-loop", "risk-reward" }
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
                CommonMistake = "Growing pools forever without a max size, eventually wasting more memory than spawning.",
                PracticePrompt = "Implement a bullet pool with Get and Release methods and compare GC spikes before and after.",
                GameUses = new List<string> { "Shooters", "Particle effects", "Mobile performance" },
                Tags = new List<string> { "performance", "memory", "optimization" },
                Synonyms = new List<string> { "pooling", "object pool" },
                RelatedTermIds = new List<string> { "garbage-collection", "draw-call", "profiling" },
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
                CommonMistake = "Ignoring allocation hotspots and only blaming the GC itself.",
                PracticePrompt = "Profile one gameplay scene and list the top three allocation sources in Update.",
                GameUses = new List<string> { "Performance profiling", "Mobile optimization", "Frame pacing" },
                Tags = new List<string> { "memory", "performance", "runtime" },
                Synonyms = new List<string> { "gc" },
                RelatedTermIds = new List<string> { "object-pooling", "memory-pressure", "profiling" }
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
                CommonMistake = "Allowing multiple conflicting states to be active with no clear ownership.",
                PracticePrompt = "Draw a four-state player controller and define every legal transition.",
                GameUses = new List<string> { "Character controllers", "AI behavior", "UI flows" },
                Tags = new List<string> { "architecture", "ai", "logic" },
                RelatedTermIds = new List<string> { "dependency-injection", "serialization", "update-loop" },
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
                CommonMistake = "Changing save formats without migration, which silently breaks older player files.",
                PracticePrompt = "Save and load one inventory object, then add a version field and a migration path.",
                GameUses = new List<string> { "Save systems", "Networking", "Editor tools" },
                Tags = new List<string> { "data", "save", "pipeline" },
                RelatedTermIds = new List<string> { "state-machine", "dependency-injection", "scriptable-object" }
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
                CommonMistake = "Hiding new dependencies behind static singletons that make tests brittle.",
                PracticePrompt = "Refactor one gameplay class to receive its dependencies through the constructor.",
                GameUses = new List<string> { "Service architecture", "Unit testing", "Modular gameplay code" },
                Tags = new List<string> { "architecture", "testing", "services" },
                Synonyms = new List<string> { "di" },
                RelatedTermIds = new List<string> { "state-machine", "serialization", "ecs", "unit-testing" }
            });

            Add(new GlossaryTermData
            {
                Id = "fixed-timestep",
                Term = "Fixed Timestep",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Updating simulation logic at a constant interval independent of frame rate.",
                SimpleExplanation = "Run physics and gameplay logic on a steady clock so results stay consistent.",
                WhyItMatters = "Fixed timesteps keep physics stable and make networked games easier to synchronize.",
                Example = "Run physics at 50 Hz while rendering as fast as the GPU allows.",
                CommonMistake = "Mixing render-frame movement with physics forces without interpolation.",
                PracticePrompt = "Move a rigidbody with fixed delta time and compare behavior at 30 FPS and 60 FPS.",
                GameUses = new List<string> { "Physics simulation", "Deterministic gameplay", "Multiplayer sync" },
                Tags = new List<string> { "physics", "simulation", "timing" },
                RelatedTermIds = new List<string> { "state-machine", "tick-rate", "delta-time", "update-loop" }
            });

            Add(new GlossaryTermData
            {
                Id = "ecs",
                Term = "Entity Component System",
                Abbreviation = "ecs",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "An architecture where gameplay data lives in components and behavior runs in systems over entities.",
                SimpleExplanation = "Store traits in small data chunks and let systems process matching groups efficiently.",
                WhyItMatters = "ECS can improve performance and scale for large numbers of similar game objects.",
                Example = "A movement system updates every entity that has position and velocity components.",
                CommonMistake = "Stuffing unrelated data into one giant component and losing the data-oriented benefit.",
                PracticePrompt = "Model a projectile as separate transform, velocity, and lifetime components.",
                GameUses = new List<string> { "Large-scale simulation", "Performance-critical gameplay", "Data-oriented design" },
                Tags = new List<string> { "architecture", "performance", "data" },
                Synonyms = new List<string> { "ecs" },
                RelatedTermIds = new List<string> { "dependency-injection", "object-pooling", "data-oriented-design" }
            });

            Add(new GlossaryTermData
            {
                Id = "delta-time",
                Term = "Delta Time",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "The elapsed time since the previous frame, used to keep motion frame-rate independent.",
                SimpleExplanation = "How long the last frame took, so movement stays smooth on fast and slow devices.",
                WhyItMatters = "Using delta time prevents gameplay speed from changing with frame rate.",
                Example = "position += velocity * deltaTime;",
                CommonMistake = "Multiplying forces by delta time twice, or forgetting it in one code path.",
                PracticePrompt = "Move an object with and without delta time at unlocked frame rates and compare distance traveled.",
                GameUses = new List<string> { "Movement code", "Animation timers", "Cooldowns" },
                Tags = new List<string> { "timing", "frames", "movement" },
                RelatedTermIds = new List<string> { "fixed-timestep", "update-loop", "frame-pacing" },
                CodeExample = "transform.position += velocity * Time.deltaTime;"
            });

            Add(new GlossaryTermData
            {
                Id = "event-system",
                Term = "Event System",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A messaging pattern where systems publish events and listeners react without direct references.",
                SimpleExplanation = "One part of the game announces something happened, and other parts can listen and respond.",
                WhyItMatters = "Events reduce tight coupling and help UI, audio, and gameplay stay in sync.",
                Example = "A HealthChanged event updates the HUD and plays a hurt sound.",
                CommonMistake = "Creating anonymous listeners that are never unsubscribed, causing leaks and double callbacks.",
                PracticePrompt = "Add a PlayerDied event with one gameplay listener and one UI listener.",
                GameUses = new List<string> { "UI updates", "Achievement unlocks", "Audio triggers" },
                Tags = new List<string> { "architecture", "messaging", "decoupling" },
                RelatedTermIds = new List<string> { "dependency-injection", "command-pattern", "state-machine" },
                CodeExample = "OnDamaged?.Invoke(amount);"
            });

            Add(new GlossaryTermData
            {
                Id = "command-pattern",
                Term = "Command Pattern",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Encapsulating actions as objects so they can be queued, undone, replayed, or networked.",
                SimpleExplanation = "Turn each player action into a little package that can be stored and replayed.",
                WhyItMatters = "Commands support undo, replays, AI scripting, and deterministic multiplayer inputs.",
                Example = "Store move and attack commands for a turn-based match and replay them later.",
                CommonMistake = "Mutating game state outside the command, so undo cannot restore the previous state.",
                PracticePrompt = "Create UndoableCommand for placing and removing a building tile.",
                GameUses = new List<string> { "Input buffering", "Replay systems", "Turn-based games" },
                Tags = new List<string> { "architecture", "input", "replay" },
                RelatedTermIds = new List<string> { "event-system", "serialization", "client-prediction" },
                CodeExample = "commands.Enqueue(new JumpCommand(player));"
            });

            Add(new GlossaryTermData
            {
                Id = "update-loop",
                Term = "Update Loop",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "The per-frame engine cycle that processes input, simulation, and presentation.",
                SimpleExplanation = "The heartbeat of the game that runs every frame.",
                WhyItMatters = "Knowing what belongs in update versus fixed update prevents timing bugs.",
                Example = "Read input in Update, simulate physics in FixedUpdate, and follow cameras in LateUpdate.",
                CommonMistake = "Doing expensive work every frame that could run on a timer or event.",
                PracticePrompt = "Move one system out of Update into an event or coroutine and measure CPU savings.",
                GameUses = new List<string> { "Engine architecture", "Gameplay timing", "Performance budgets" },
                Tags = new List<string> { "frames", "engine", "timing" },
                RelatedTermIds = new List<string> { "delta-time", "fixed-timestep", "coroutine" }
            });

            Add(new GlossaryTermData
            {
                Id = "coroutine",
                Term = "Coroutine",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A function that can pause and resume over multiple frames without blocking the main thread.",
                SimpleExplanation = "A script that waits and continues later while the game keeps running.",
                WhyItMatters = "Coroutines simplify timed sequences, fades, and async gameplay flows on one thread.",
                Example = "Fade out a UI panel over one second using yield return null each frame.",
                CommonMistake = "Starting many long-lived coroutines without cancellation when the object is disabled.",
                PracticePrompt = "Write a damage-over-time coroutine that stops cleanly when the target dies.",
                GameUses = new List<string> { "Timed sequences", "Spawning waves", "UI transitions" },
                Tags = new List<string> { "async", "timing", "unity" },
                RelatedTermIds = new List<string> { "update-loop", "delta-time", "event-system" },
                CodeExample = "yield return new WaitForSeconds(1f);"
            });

            Add(new GlossaryTermData
            {
                Id = "data-oriented-design",
                Term = "Data Oriented Design",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "An approach that organizes data for cache-friendly processing instead of around objects first.",
                SimpleExplanation = "Arrange data so the CPU can process lots of similar values quickly.",
                WhyItMatters = "Data-oriented design unlocks performance for large simulations and ECS architectures.",
                Example = "Store all enemy health values in one array processed by a single system.",
                CommonMistake = "Keeping object-oriented wrappers that defeat contiguous data layout.",
                PracticePrompt = "Rewrite one particle update to iterate arrays of positions and velocities.",
                GameUses = new List<string> { "Particle systems", "Crowd simulation", "ECS projects" },
                Tags = new List<string> { "performance", "data", "architecture" },
                RelatedTermIds = new List<string> { "ecs", "profiling", "object-pooling" }
            });

            Add(new GlossaryTermData
            {
                Id = "scriptable-object",
                Term = "Scriptable Object",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A Unity data asset that stores shared configuration outside scene objects.",
                SimpleExplanation = "A reusable settings file that lives in the project instead of on every prefab.",
                WhyItMatters = "Scriptable objects make balancing and content iteration safer and faster.",
                Example = "A WeaponStats asset shared by every rifle prefab instance.",
                CommonMistake = "Storing mutable runtime state in assets that accidentally persist between play sessions.",
                PracticePrompt = "Create a ScriptableObject for enemy stats and assign it to three enemy prefabs.",
                GameUses = new List<string> { "Balance data", "Shared configs", "Content pipelines" },
                Tags = new List<string> { "unity", "data", "assets" },
                RelatedTermIds = new List<string> { "serialization", "dependency-injection", "event-system" },
                CodeExample = "[CreateAssetMenu] class WeaponStats : ScriptableObject {}"
            });

            Add(new GlossaryTermData
            {
                Id = "profiling",
                Term = "Profiling",
                Category = GlossaryCategory.Programming,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Measuring where CPU, GPU, memory, and time are spent to guide optimization.",
                SimpleExplanation = "Using tools to find what is actually slow instead of guessing.",
                WhyItMatters = "Profiling prevents wasted optimization effort and protects frame budgets.",
                Example = "Capture a deep profile during combat and sort functions by self time.",
                CommonMistake = "Optimizing code that never appears in the profile hotspots.",
                PracticePrompt = "Capture one play session profile and list three actionable hotspots with owners.",
                GameUses = new List<string> { "Performance triage", "Mobile optimization", "Ship readiness" },
                Tags = new List<string> { "performance", "tools", "optimization" },
                RelatedTermIds = new List<string> { "garbage-collection", "draw-call", "thermal-throttling" }
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
                CommonMistake = "Breaking batches with unique materials when a shared atlas would suffice.",
                PracticePrompt = "Count draw calls in a scene, then atlas two materials and measure the reduction.",
                GameUses = new List<string> { "Mobile optimization", "Batching strategy", "Profiling" },
                Tags = new List<string> { "rendering", "gpu", "performance" },
                Synonyms = new List<string> { "draw" },
                RelatedTermIds = new List<string> { "overdraw", "level-of-detail", "occlusion-culling", "static-batching" }
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
                CommonMistake = "Expecting occlusion culling to fix overdraw from transparent VFX.",
                PracticePrompt = "Bake occlusion data for one interior level and compare GPU timings with it disabled.",
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
                CommonMistake = "Using giant bounding volumes that keep off-screen props in the frustum.",
                PracticePrompt = "Visualize camera frustum bounds and tighten one oversized collider.",
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
                CommonMistake = "Switching LODs so abruptly that popping is obvious to players.",
                PracticePrompt = "Create three LOD meshes for one prop and tune transition distances in play mode.",
                GameUses = new List<string> { "Open worlds", "Crowd scenes", "Mobile rendering" },
                Tags = new List<string> { "lod", "optimization", "meshes" },
                Synonyms = new List<string> { "lod" },
                RelatedTermIds = new List<string> { "draw-call", "texture-compression", "mipmap" },
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
                CommonMistake = "Using huge transparent quads when smaller clipped meshes would suffice.",
                PracticePrompt = "Enable an overdraw debug view and reduce the worst particle effect by half.",
                GameUses = new List<string> { "VFX budgeting", "UI transparency review", "Mobile tuning" },
                Tags = new List<string> { "rendering", "gpu", "fill rate" },
                RelatedTermIds = new List<string> { "draw-call", "occlusion-culling", "post-processing" }
            });

            Add(new GlossaryTermData
            {
                Id = "pbr",
                Term = "Physically Based Rendering",
                Abbreviation = "pbr",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A shading approach that mimics how light interacts with real-world materials.",
                SimpleExplanation = "Materials react to light in a realistic, consistent way across different scenes.",
                WhyItMatters = "PBR helps art look cohesive under varied lighting and reduces guesswork for artists.",
                Example = "Metal reflects sharply while rough wood scatters light softly using the same shader model.",
                CommonMistake = "Mixing PBR maps with non-PBR lighting assumptions, creating plastic-looking materials.",
                PracticePrompt = "Author one material with albedo, roughness, and metallic maps under two lighting setups.",
                GameUses = new List<string> { "Material authoring", "Lighting pipelines", "Cross-platform visuals" },
                Tags = new List<string> { "rendering", "materials", "lighting" },
                Synonyms = new List<string> { "pbr" },
                RelatedTermIds = new List<string> { "normal-map", "draw-call", "albedo-map", "roughness-map" }
            });

            Add(new GlossaryTermData
            {
                Id = "shader",
                Term = "Shader",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A GPU program that calculates how surfaces are drawn, shaded, and transformed.",
                SimpleExplanation = "The little program that tells the GPU how each pixel or vertex should look.",
                WhyItMatters = "Shaders define visual style and often dominate GPU cost.",
                Example = "A toon shader that remaps lighting into hard bands for a comic look.",
                CommonMistake = "Writing complex fragment logic when a cheaper material feature would work.",
                PracticePrompt = "Create a simple unlit color shader and apply it to a test mesh.",
                GameUses = new List<string> { "Custom looks", "Performance tuning", "VFX" },
                Tags = new List<string> { "gpu", "materials", "rendering" },
                RelatedTermIds = new List<string> { "pbr", "render-pipeline", "draw-call" },
                CodeExample = "half4 frag() : SV_Target { return half4(1,0,0,1); }"
            });

            Add(new GlossaryTermData
            {
                Id = "render-pipeline",
                Term = "Render Pipeline",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "The ordered stages and systems that take scene data and produce the final image.",
                SimpleExplanation = "The factory line that turns game objects into pixels on screen.",
                WhyItMatters = "Pipeline choice affects quality features, platform support, and performance characteristics.",
                Example = "Using a lightweight forward pipeline for mobile and a deferred pipeline for consoles.",
                CommonMistake = "Enabling every pipeline feature without checking target device cost.",
                PracticePrompt = "Compare one scene's frame time under two quality pipeline settings.",
                GameUses = new List<string> { "Platform targeting", "Graphics architecture", "Feature planning" },
                Tags = new List<string> { "pipeline", "rendering", "architecture" },
                RelatedTermIds = new List<string> { "shader", "post-processing", "hdr-rendering" }
            });

            Add(new GlossaryTermData
            {
                Id = "static-batching",
                Term = "Static Batching",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Combining non-moving meshes that share materials into fewer draw calls.",
                SimpleExplanation = "Group still objects together so the GPU draws them more efficiently.",
                WhyItMatters = "Static batching is a major win for environment art on constrained devices.",
                Example = "Hundreds of static crates sharing one material drawn as fewer batches.",
                CommonMistake = "Marking dynamic objects as static and breaking their movement or lighting.",
                PracticePrompt = "Enable static batching on a prop group and measure draw-call reduction.",
                GameUses = new List<string> { "Environment optimization", "Mobile scenes", "Open-world props" },
                Tags = new List<string> { "batching", "performance", "meshes" },
                RelatedTermIds = new List<string> { "draw-call", "dynamic-batching", "occlusion-culling" }
            });

            Add(new GlossaryTermData
            {
                Id = "dynamic-batching",
                Term = "Dynamic Batching",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Runtime combining of small moving meshes that share material setup.",
                SimpleExplanation = "Automatically group tiny moving objects to reduce draw requests.",
                WhyItMatters = "Dynamic batching can help simple scenes, but has strict mesh and material limits.",
                Example = "Many small debris pieces with identical materials batched each frame.",
                CommonMistake = "Expecting dynamic batching to save large unique meshes that exceed limits.",
                PracticePrompt = "Create twenty tiny quads and inspect whether they batch in the frame debugger.",
                GameUses = new List<string> { "Debris", "Simple props", "Mobile fill scenes" },
                Tags = new List<string> { "batching", "runtime", "performance" },
                RelatedTermIds = new List<string> { "static-batching", "draw-call", "overdraw" }
            });

            Add(new GlossaryTermData
            {
                Id = "z-buffer",
                Term = "Z Buffer",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A depth buffer that stores per-pixel distance so closer surfaces can occlude farther ones.",
                SimpleExplanation = "A hidden map of how far away each pixel is from the camera.",
                WhyItMatters = "Depth testing is fundamental to correct sorting of opaque geometry.",
                Example = "A character standing in front of a wall correctly hides the wall pixels.",
                CommonMistake = "Fighting transparent sorting issues with z-buffer tricks that create artifacts.",
                PracticePrompt = "Render two overlapping cubes and inspect the depth buffer visualization.",
                GameUses = new List<string> { "Opaque rendering", "Sorting", "Debug views" },
                Tags = new List<string> { "depth", "rendering", "gpu" },
                RelatedTermIds = new List<string> { "overdraw", "occlusion-culling", "shadow-mapping" }
            });

            Add(new GlossaryTermData
            {
                Id = "shadow-mapping",
                Term = "Shadow Mapping",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "A technique that renders the scene from a light's view to determine which pixels are shadowed.",
                SimpleExplanation = "The light takes a depth photo, then the camera checks what is hidden from that light.",
                WhyItMatters = "Shadows add realism but are among the most expensive lighting features.",
                Example = "Cascaded shadow maps covering near detail and far terrain under sunlight.",
                CommonMistake = "Using oversized shadow resolutions on mobile without cascade tuning.",
                PracticePrompt = "Toggle shadows on a key light and measure GPU frame-time impact.",
                GameUses = new List<string> { "Directional lights", "Indoor lamps", "Quality settings" },
                Tags = new List<string> { "lighting", "shadows", "gpu" },
                RelatedTermIds = new List<string> { "z-buffer", "pbr", "adaptive-performance" }
            });

            Add(new GlossaryTermData
            {
                Id = "post-processing",
                Term = "Post Processing",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Full-screen image effects applied after the main scene is rendered.",
                SimpleExplanation = "Filters and polish layered on top of the finished picture.",
                WhyItMatters = "Post effects sell mood and readability, but can dominate GPU fill cost.",
                Example = "Bloom, color grading, and mild chromatic aberration on a cinematic shot.",
                CommonMistake = "Stacking multiple expensive effects that barely change the intended look.",
                PracticePrompt = "Enable only two post effects and A/B the scene for mood versus cost.",
                GameUses = new List<string> { "Cinematics", "Gameplay readability", "Style direction" },
                Tags = new List<string> { "effects", "camera", "polish" },
                RelatedTermIds = new List<string> { "hdr-rendering", "overdraw", "render-pipeline" }
            });

            Add(new GlossaryTermData
            {
                Id = "mipmap",
                Term = "Mipmap",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A chain of progressively smaller texture versions used when sampling distant surfaces.",
                SimpleExplanation = "Smaller copies of a texture that the GPU uses when things are far away.",
                WhyItMatters = "Mipmaps reduce aliasing and improve texture sampling performance.",
                Example = "A brick wall uses a lower mip level when viewed from across a courtyard.",
                CommonMistake = "Disabling mipmaps on world textures and introducing shimmering aliasing.",
                PracticePrompt = "Inspect mip visualization on one material and confirm distant surfaces use lower mips.",
                GameUses = new List<string> { "Texture streaming", "World materials", "Mobile memory" },
                Tags = new List<string> { "textures", "sampling", "optimization" },
                RelatedTermIds = new List<string> { "level-of-detail", "texture-compression", "texel-density" }
            });

            Add(new GlossaryTermData
            {
                Id = "hdr-rendering",
                Term = "HDR Rendering",
                Category = GlossaryCategory.GraphicsAndRendering,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "Rendering with color values beyond the 0-1 display range, then tone-mapping to screen output.",
                SimpleExplanation = "Working with brighter-than-screen light values, then compressing them for the display.",
                WhyItMatters = "HDR enables bloom, realistic lighting contrast, and better exposure control.",
                Example = "Sunlit exteriors and dark interiors shared in one scene with exposure adaptation.",
                CommonMistake = "Enabling HDR without tone mapping, producing clipped or washed-out results.",
                PracticePrompt = "Compare one scene in LDR and HDR with identical lighting intensity.",
                GameUses = new List<string> { "Cinematic lighting", "Day-night systems", "High-end platforms" },
                Tags = new List<string> { "lighting", "color", "pipeline" },
                RelatedTermIds = new List<string> { "post-processing", "pbr", "render-pipeline" }
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
                CommonMistake = "Using a normal map authored for the wrong tangent space, causing lighting seams.",
                PracticePrompt = "Apply a normal map to a flat quad and rotate a light around it to verify detail.",
                GameUses = new List<string> { "Environment art", "Character materials", "Mobile asset budgets" },
                Tags = new List<string> { "texturing", "lighting", "materials" },
                RelatedTermIds = new List<string> { "uv-mapping", "texture-compression", "albedo-map", "pbr" }
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
                CommonMistake = "Overcomplicating controls so animators fight the rig instead of posing.",
                PracticePrompt = "Build a simple biped rig with IK legs and test a walk cycle.",
                GameUses = new List<string> { "Character animation", "Cinematics", "Facial performance" },
                Tags = new List<string> { "animation", "characters", "pipeline" },
                RelatedTermIds = new List<string> { "skinning", "uv-mapping", "inverse-kinematics", "retargeting" }
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
                CommonMistake = "Leaving heavy weight influences across joints that should bend cleanly.",
                PracticePrompt = "Paint weights on a shoulder and compare deformation before and after cleanup.",
                GameUses = new List<string> { "Character setup", "Animation polish", "Runtime deformation" },
                Tags = new List<string> { "animation", "weights", "characters" },
                RelatedTermIds = new List<string> { "rigging", "normal-map", "animation-blending" }
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
                CommonMistake = "Stretching UVs unevenly, which makes textures look warped in-game.",
                PracticePrompt = "Unwrap a crate and check texel density consistency across all faces.",
                GameUses = new List<string> { "Texturing", "Material authoring", "Asset optimization" },
                Tags = new List<string> { "texturing", "pipeline", "art" },
                RelatedTermIds = new List<string> { "normal-map", "texture-compression", "sprite-sheet", "texel-density" }
            });

            Add(new GlossaryTermData
            {
                Id = "sprite-sheet",
                Term = "Sprite Sheet",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "A single image containing multiple frames or icons arranged for efficient rendering.",
                SimpleExplanation = "One texture file that holds many animation frames or UI icons together.",
                WhyItMatters = "Sprite sheets reduce draw calls and simplify 2D animation workflows.",
                Example = "A character walk cycle stored as eight frames in one PNG atlas.",
                CommonMistake = "Packing frames with no padding, causing neighbor bleed when filtering.",
                PracticePrompt = "Pack a six-frame attack animation into one sheet and play it in engine.",
                GameUses = new List<string> { "2D animation", "UI atlasing", "Mobile 2D games" },
                Tags = new List<string> { "2d", "animation", "textures" },
                Synonyms = new List<string> { "texture atlas", "sprite atlas" },
                RelatedTermIds = new List<string> { "uv-mapping", "texture-compression", "draw-call", "tileset" }
            });

            Add(new GlossaryTermData
            {
                Id = "keyframe-animation",
                Term = "Keyframe Animation",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Defining important poses at points in time and interpolating motion between them.",
                SimpleExplanation = "Set the important poses, then let the computer fill in the in-between movement.",
                WhyItMatters = "Keyframes are the foundation of most authored character and prop animation.",
                Example = "A jump keyed at crouch, takeoff, apex, and landing.",
                CommonMistake = "Keying every frame manually and destroying editable timing.",
                PracticePrompt = "Animate a door opening with only four keys and adjust easing.",
                GameUses = new List<string> { "Character acting", "Props", "UI motion" },
                Tags = new List<string> { "animation", "timing", "poses" },
                RelatedTermIds = new List<string> { "animation-blending", "rigging", "motion-capture" }
            });

            Add(new GlossaryTermData
            {
                Id = "animation-blending",
                Term = "Animation Blending",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Mixing multiple animation clips to create smooth transitions or combined motion.",
                SimpleExplanation = "Crossfading or combining animations so movement does not pop.",
                WhyItMatters = "Blending keeps locomotion and combat responsive without hard cuts.",
                Example = "Blend walk and strafe clips based on analog stick direction.",
                CommonMistake = "Using long blend times that make controls feel laggy.",
                PracticePrompt = "Set up a blend tree for idle, walk, and run and tune transition durations.",
                GameUses = new List<string> { "Locomotion", "Combat layers", "Aim offsets" },
                Tags = new List<string> { "animation", "transitions", "locomotion" },
                RelatedTermIds = new List<string> { "keyframe-animation", "skinning", "state-machine" }
            });

            Add(new GlossaryTermData
            {
                Id = "inverse-kinematics",
                Term = "Inverse Kinematics",
                Abbreviation = "ik",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "Calculating joint rotations so an end effector reaches a target position.",
                SimpleExplanation = "Tell the hand or foot where to go, and the limb figures out how to bend.",
                WhyItMatters = "IK improves grounding, aiming, and interaction with uneven surfaces.",
                Example = "Feet plant on stairs while the hips adjust automatically.",
                CommonMistake = "Over-constraining IK so limbs snap into unnatural poses.",
                PracticePrompt = "Add two-bone IK to a character leg and test foot placement on a ramp.",
                GameUses = new List<string> { "Foot planting", "Aiming", "Climbing systems" },
                Tags = new List<string> { "animation", "ik", "rigs" },
                RelatedTermIds = new List<string> { "rigging", "skinning", "animation-blending" }
            });

            Add(new GlossaryTermData
            {
                Id = "motion-capture",
                Term = "Motion Capture",
                Abbreviation = "mocap",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Recording real performer movement and transferring it onto a digital character.",
                SimpleExplanation = "Capture a person's motion with sensors or cameras and apply it to a 3D character.",
                WhyItMatters = "Mo-cap accelerates realistic animation production for cinematic and gameplay needs.",
                Example = "A fight choreography recorded on a stage and cleaned for an in-game finisher.",
                CommonMistake = "Shipping raw capture without cleanup, leaving foot slides and noise.",
                PracticePrompt = "Plan a short capture shot list with cleanup and retargeting steps.",
                GameUses = new List<string> { "Cinematics", "Sports games", "Realistic combat" },
                Tags = new List<string> { "animation", "pipeline", "performance" },
                Synonyms = new List<string> { "mo-cap" },
                RelatedTermIds = new List<string> { "retargeting", "keyframe-animation", "rigging" }
            });

            Add(new GlossaryTermData
            {
                Id = "blend-shape",
                Term = "Blend Shape",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A morph target that stores vertex offsets to reshape a mesh for expressions or variation.",
                SimpleExplanation = "A saved mesh shape you can dial up to smile, frown, or change form.",
                WhyItMatters = "Blend shapes are essential for faces, damage states, and body variation.",
                Example = "A character face uses smile, blink, and brow-raise shapes driven by dialogue.",
                CommonMistake = "Stacking too many high-resolution shapes that blow memory budgets.",
                PracticePrompt = "Create three facial blend shapes and drive them from a simple slider UI.",
                GameUses = new List<string> { "Facial animation", "Character customization", "Damage states" },
                Tags = new List<string> { "animation", "morph", "characters" },
                Synonyms = new List<string> { "morph target" },
                RelatedTermIds = new List<string> { "rigging", "skinning", "keyframe-animation" }
            });

            Add(new GlossaryTermData
            {
                Id = "albedo-map",
                Term = "Albedo Map",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "A base color texture without lighting or specular information baked in.",
                SimpleExplanation = "The flat color of a surface before lighting is applied.",
                WhyItMatters = "Clean albedo maps are required for believable PBR materials.",
                Example = "A wood plank albedo shows grain color without fake shadows.",
                CommonMistake = "Baking ambient occlusion or highlights into albedo and breaking lighting response.",
                PracticePrompt = "Repaint one material albedo to remove baked shadows and compare under two lights.",
                GameUses = new List<string> { "PBR authoring", "Environment art", "Characters" },
                Tags = new List<string> { "texturing", "pbr", "color" },
                RelatedTermIds = new List<string> { "roughness-map", "normal-map", "pbr" }
            });

            Add(new GlossaryTermData
            {
                Id = "roughness-map",
                Term = "Roughness Map",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A texture controlling how sharp or blurry reflections appear across a surface.",
                SimpleExplanation = "Dark areas look glossier and bright areas look more matte.",
                WhyItMatters = "Roughness variation sells material realism more than color alone.",
                Example = "Metal edges stay glossy while worn paint reads rough and chalky.",
                CommonMistake = "Using gloss and roughness maps interchangeably without converting the workflow.",
                PracticePrompt = "Paint a roughness map for a metal panel with scratched edges and preview reflections.",
                GameUses = new List<string> { "PBR materials", "Hero props", "Weapons" },
                Tags = new List<string> { "texturing", "pbr", "reflections" },
                RelatedTermIds = new List<string> { "albedo-map", "normal-map", "pbr" }
            });

            Add(new GlossaryTermData
            {
                Id = "texel-density",
                Term = "Texel Density",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "How many texture pixels cover a given amount of world-space surface.",
                SimpleExplanation = "How sharp or muddy a texture looks for its real-world size.",
                WhyItMatters = "Consistent texel density keeps environments readable and fair across assets.",
                Example = "A door and nearby wall share roughly the same pixels-per-meter.",
                CommonMistake = "UV-scaling hero props denser than architecture, making walls look blurry by comparison.",
                PracticePrompt = "Measure texel density on three props and normalize them to one target value.",
                GameUses = new List<string> { "Environment packing", "LOD planning", "QA art reviews" },
                Tags = new List<string> { "texturing", "uv", "quality" },
                RelatedTermIds = new List<string> { "uv-mapping", "mipmap", "texture-compression" }
            });

            Add(new GlossaryTermData
            {
                Id = "retargeting",
                Term = "Retargeting",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Transferring animation from one skeleton onto another with different proportions.",
                SimpleExplanation = "Reuse a walk cycle on characters that are not built on the exact same skeleton.",
                WhyItMatters = "Retargeting multiplies animation value across a cast of characters.",
                Example = "Apply the same dance clip to a child and adult avatar with mapping adjustments.",
                CommonMistake = "Ignoring bone naming and proportion differences, which creates foot skating.",
                PracticePrompt = "Retarget one clip to a second rig and fix foot contact in the result.",
                GameUses = new List<string> { "Shared animation libraries", "Avatar systems", "Mo-cap cleanup" },
                Tags = new List<string> { "animation", "pipeline", "reuse" },
                RelatedTermIds = new List<string> { "motion-capture", "rigging", "animation-blending" }
            });

            Add(new GlossaryTermData
            {
                Id = "tileset",
                Term = "Tileset",
                Category = GlossaryCategory.ArtAndAnimation,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "A collection of reusable tiles used to construct 2D levels and environments.",
                SimpleExplanation = "A palette of small art pieces you stamp repeatedly to build maps.",
                WhyItMatters = "Tilesets speed 2D production and keep art style consistent across levels.",
                Example = "Grass, water, and cliff tiles assemble into a top-down overworld.",
                CommonMistake = "Designing tiles that only connect in one orientation, limiting level variety.",
                PracticePrompt = "Create a nine-slice terrain tileset and build a small path that uses every tile.",
                GameUses = new List<string> { "2D level art", "Roguelike maps", "Mobile 2D games" },
                Tags = new List<string> { "2d", "levels", "art" },
                RelatedTermIds = new List<string> { "sprite-sheet", "level-design", "uv-mapping" }
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
                CommonMistake = "Reusing one footstep sample on every surface, which breaks believability.",
                PracticePrompt = "Record three surface footsteps and hook them to a movement animation.",
                GameUses = new List<string> { "Character interaction", "Cinematic polish", "Immersion" },
                Tags = new List<string> { "sound design", "sfx", "immersion" },
                RelatedTermIds = new List<string> { "diegetic-audio", "audio-ducking", "sfx-layering" }
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
                CommonMistake = "Playing world sounds without distance attenuation, so they feel detached.",
                PracticePrompt = "Place one diegetic emitter and verify volume falloff as the player walks away.",
                GameUses = new List<string> { "World building", "Horror tension", "Spatial gameplay cues" },
                Tags = new List<string> { "sound design", "immersion", "spatial" },
                RelatedTermIds = new List<string> { "foley", "audio-ducking", "non-diegetic-audio", "spatial-audio" }
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
                CommonMistake = "Ducking too aggressively so the mix feels like it keeps collapsing.",
                PracticePrompt = "Set a ducking rule that lowers music 6 dB during dialogue.",
                GameUses = new List<string> { "Dialogue systems", "UI feedback", "Combat readability" },
                Tags = new List<string> { "mixing", "ui audio", "clarity" },
                RelatedTermIds = new List<string> { "foley", "diegetic-audio", "adaptive-music", "audio-mixing" }
            });

            Add(new GlossaryTermData
            {
                Id = "adaptive-music",
                Term = "Adaptive Music",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Music that changes layers or intensity based on gameplay state.",
                SimpleExplanation = "The soundtrack reacts to combat, exploration, or story moments in real time.",
                WhyItMatters = "Adaptive music keeps emotional pacing aligned with player actions without jarring loops.",
                Example = "Combat drums layer in when enemies appear and fade when the fight ends.",
                CommonMistake = "Hard-cutting between unrelated tracks instead of layered transitions.",
                PracticePrompt = "Design a two-layer exploration and combat bed with a crossfade trigger.",
                GameUses = new List<string> { "Combat pacing", "Exploration ambience", "Cinematic transitions" },
                Tags = new List<string> { "music", "dynamic", "immersion" },
                RelatedTermIds = new List<string> { "audio-ducking", "diegetic-audio", "non-diegetic-audio" }
            });

            Add(new GlossaryTermData
            {
                Id = "spatial-audio",
                Term = "Spatial Audio",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Positioning sound in 3D space so players can locate sources by ear.",
                SimpleExplanation = "Hearing where a sound is coming from around you.",
                WhyItMatters = "Spatial audio supports stealth, awareness, and immersion.",
                Example = "Footsteps approaching from the left warn the player before an enemy appears.",
                CommonMistake = "Using stereo-only clips for critical directional cues.",
                PracticePrompt = "Place three emitters around the player and verify left, right, and rear localization.",
                GameUses = new List<string> { "Stealth games", "VR", "Open worlds" },
                Tags = new List<string> { "3d", "localization", "immersion" },
                RelatedTermIds = new List<string> { "diegetic-audio", "reverb", "audio-mixing" }
            });

            Add(new GlossaryTermData
            {
                Id = "audio-mixing",
                Term = "Audio Mixing",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Balancing buses, levels, and priorities so the soundtrack remains clear under gameplay load.",
                SimpleExplanation = "Organizing and leveling music, dialogue, and effects so nothing fights for attention.",
                WhyItMatters = "A disciplined mix protects readability during chaotic moments.",
                Example = "Separate buses for UI, combat SFX, dialogue, and music with priority rules.",
                CommonMistake = "Leaving every sound at default loudness until the mix becomes unreadable.",
                PracticePrompt = "Build a four-bus mixer and assign five existing sounds into the correct buses.",
                GameUses = new List<string> { "Combat clarity", "Dialogue focus", "Platform loudness" },
                Tags = new List<string> { "mixing", "buses", "loudness" },
                RelatedTermIds = new List<string> { "audio-ducking", "loudness-normalization", "sfx-layering" }
            });

            Add(new GlossaryTermData
            {
                Id = "soundscape",
                Term = "Soundscape",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "The layered ambient audio bed that defines a location's atmosphere.",
                SimpleExplanation = "The background mood of a place made from many quiet sounds.",
                WhyItMatters = "Soundscapes sell setting before the player sees every prop.",
                Example = "Wind, distant traffic, and HVAC hum establish a rooftop at night.",
                CommonMistake = "Looping one short bed that becomes obviously repetitive within seconds.",
                PracticePrompt = "Create a 30-second location bed using at least three overlapping loops.",
                GameUses = new List<string> { "World building", "Exploration", "Horror atmosphere" },
                Tags = new List<string> { "ambient", "world", "mood" },
                RelatedTermIds = new List<string> { "diegetic-audio", "reverb", "spatial-audio" }
            });

            Add(new GlossaryTermData
            {
                Id = "reverb",
                Term = "Reverb",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Simulated reflections that make sounds feel like they exist in a physical space.",
                SimpleExplanation = "Echo and room tone that tell your ears how big a space is.",
                WhyItMatters = "Reverb cues environment scale and can support stealth or horror design.",
                Example = "Footsteps ring longer in a cathedral than in a carpeted hallway.",
                CommonMistake = "Applying one global reverb to every location, flattening spatial contrast.",
                PracticePrompt = "Assign different reverb presets to a cave and a closet and A/B walkthroughs.",
                GameUses = new List<string> { "Environment audio", "VR presence", "Stealth readability" },
                Tags = new List<string> { "space", "effects", "immersion" },
                RelatedTermIds = new List<string> { "spatial-audio", "soundscape", "audio-mixing" }
            });

            Add(new GlossaryTermData
            {
                Id = "audio-middleware",
                Term = "Audio Middleware",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "Specialized tools such as FMOD or Wwise that manage interactive audio outside the game engine.",
                SimpleExplanation = "A dedicated audio engine that designers use to build complex sound behavior.",
                WhyItMatters = "Middleware lets audio teams iterate without constant engineering changes.",
                Example = "An FMOD event randomizes footsteps and adapts to surface parameters.",
                CommonMistake = "Hardcoding complex audio logic in engine code that audio designers cannot edit.",
                PracticePrompt = "Prototype one interactive event in middleware and trigger it from gameplay.",
                GameUses = new List<string> { "Large audio projects", "Adaptive music", "Live tuning" },
                Tags = new List<string> { "tools", "pipeline", "interactive" },
                RelatedTermIds = new List<string> { "adaptive-music", "audio-mixing", "sfx-layering" }
            });

            Add(new GlossaryTermData
            {
                Id = "sfx-layering",
                Term = "SFX Layering",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Combining multiple sound elements into one impactful effect.",
                SimpleExplanation = "Stacking smaller sounds to make one hit, explosion, or UI click feel bigger.",
                WhyItMatters = "Layering creates richness and control without relying on a single recording.",
                Example = "A gunshot built from mechanical click, muzzle blast, and distant tail.",
                CommonMistake = "Layering too many elements so the result becomes muddy and undefined.",
                PracticePrompt = "Build a three-layer UI confirm sound and balance each layer's contribution.",
                GameUses = new List<string> { "Combat impacts", "UI feedback", "Magic spells" },
                Tags = new List<string> { "sfx", "design", "polish" },
                RelatedTermIds = new List<string> { "foley", "audio-mixing", "audio-ducking" }
            });

            Add(new GlossaryTermData
            {
                Id = "voice-over",
                Term = "Voice Over",
                Abbreviation = "vo",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Recorded spoken performance used for characters, narration, or system prompts.",
                SimpleExplanation = "Actors speaking lines that play in the game.",
                WhyItMatters = "Voice over carries story, personality, and critical instructions.",
                Example = "A companion character warns the player about an ambush mid-mission.",
                CommonMistake = "Shipping dialogue without localization or subtitle fallbacks.",
                PracticePrompt = "Write and record three system VO lines with matching subtitle timing.",
                GameUses = new List<string> { "Narrative games", "Tutorials", "Live events" },
                Tags = new List<string> { "dialogue", "narrative", "performance" },
                Synonyms = new List<string> { "VO" },
                RelatedTermIds = new List<string> { "audio-ducking", "localization", "loudness-normalization" }
            });

            Add(new GlossaryTermData
            {
                Id = "audio-compression",
                Term = "Audio Compression",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Reducing audio file size or dynamic range for memory, bandwidth, or mix control.",
                SimpleExplanation = "Making sounds smaller to store or quieter in the loud parts so they sit better in a mix.",
                WhyItMatters = "Compression keeps mobile packages manageable and mixes controlled.",
                Example = "Converting long music beds to a compressed format for a mobile build.",
                CommonMistake = "Over-compressing effects until they lose punch and clarity.",
                PracticePrompt = "Compare file size and quality for one clip at two compression settings.",
                GameUses = new List<string> { "Mobile packaging", "Streaming audio", "Mix dynamics" },
                Tags = new List<string> { "memory", "formats", "mixing" },
                RelatedTermIds = new List<string> { "texture-compression", "loudness-normalization", "memory-pressure" }
            });

            Add(new GlossaryTermData
            {
                Id = "loudness-normalization",
                Term = "Loudness Normalization",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Matching perceived volume across content to platform loudness targets.",
                SimpleExplanation = "Making sure no track or trailer is randomly much louder than another.",
                WhyItMatters = "Normalization prevents player fatigue and meets store or console requirements.",
                Example = "Dialog and music beds both target the same integrated loudness range.",
                CommonMistake = "Normalizing peak level only, which still leaves large loudness swings.",
                PracticePrompt = "Measure integrated loudness for three clips and bring them into the same target range.",
                GameUses = new List<string> { "Trailers", "In-game mix", "Platform compliance" },
                Tags = new List<string> { "loudness", "compliance", "mixing" },
                RelatedTermIds = new List<string> { "audio-mixing", "voice-over", "audio-compression" }
            });

            Add(new GlossaryTermData
            {
                Id = "haptic-feedback",
                Term = "Haptic Feedback",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Using device vibration or force feedback to reinforce game actions through touch.",
                SimpleExplanation = "Making the controller or phone buzz so actions feel physical.",
                WhyItMatters = "Haptics strengthen game feel and accessibility of important cues.",
                Example = "A short rumble when the player lands a heavy attack.",
                CommonMistake = "Firing haptics for every minor event until players turn them off.",
                PracticePrompt = "Design three haptic patterns for hit, miss, and low health warning.",
                GameUses = new List<string> { "Combat feel", "Mobile polish", "Accessibility cues" },
                Tags = new List<string> { "haptics", "feel", "feedback" },
                RelatedTermIds = new List<string> { "juice", "game-feel", "foley" }
            });

            Add(new GlossaryTermData
            {
                Id = "non-diegetic-audio",
                Term = "Non Diegetic Audio",
                Category = GlossaryCategory.Audio,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Sound that exists for the audience and not as a source inside the story world.",
                SimpleExplanation = "Music or narration the characters would not hear.",
                WhyItMatters = "Non-diegetic layers guide emotion and pacing without needing a world emitter.",
                Example = "A tense score swells during a chase even though no radio is playing in-world.",
                CommonMistake = "Mixing non-diegetic score so loudly that gameplay cues become hard to hear.",
                PracticePrompt = "Mark five sounds in your project as diegetic or non-diegetic and justify each choice.",
                GameUses = new List<string> { "Score design", "Trailers", "Narrative emphasis" },
                Tags = new List<string> { "music", "narrative", "score" },
                RelatedTermIds = new List<string> { "diegetic-audio", "adaptive-music", "audio-ducking" }
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
                CommonMistake = "Blaming only ping while ignoring jitter and packet loss.",
                PracticePrompt = "Log round-trip time during a session and note gameplay moments that feel delayed.",
                GameUses = new List<string> { "Netcode tuning", "Competitive modes", "Input responsiveness" },
                Tags = new List<string> { "networking", "ping", "responsiveness" },
                RelatedTermIds = new List<string> { "tick-rate", "client-prediction", "rollback", "jitter" }
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
                CommonMistake = "Raising tick rate without profiling CPU and bandwidth budgets.",
                PracticePrompt = "Compare hit registration quality at two tick rates in a controlled test.",
                GameUses = new List<string> { "Server architecture", "Competitive shooters", "Sync quality" },
                Tags = new List<string> { "server", "simulation", "networking" },
                RelatedTermIds = new List<string> { "latency", "client-prediction", "dedicated-server" }
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
                CommonMistake = "Failing to reconcile when the server rejects the predicted state.",
                PracticePrompt = "Implement predicted movement and log corrections when the server disagrees.",
                GameUses = new List<string> { "Action games", "Platformers online", "Responsive controls" },
                Tags = new List<string> { "netcode", "responsiveness", "sync" },
                RelatedTermIds = new List<string> { "latency", "rollback", "tick-rate", "server-reconciliation" }
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
                CommonMistake = "Rolling back without deterministic simulation, which creates endless desyncs.",
                PracticePrompt = "Prototype a tiny deterministic sim and delay one input to observe rollback repair.",
                GameUses = new List<string> { "Fighting games", "Fast action multiplayer", "Peer-to-peer sessions" },
                Tags = new List<string> { "netcode", "sync", "fighting" },
                Synonyms = new List<string> { "rollback netcode" },
                RelatedTermIds = new List<string> { "client-prediction", "latency", "authoritative-server", "desync" }
            });

            Add(new GlossaryTermData
            {
                Id = "authoritative-server",
                Term = "Authoritative Server",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A server that owns the true game state and validates player actions.",
                SimpleExplanation = "The server decides what really happened, not each player's device.",
                WhyItMatters = "Authority prevents cheating and keeps multiplayer outcomes consistent.",
                Example = "The server confirms a hit before damage is applied, even if a client predicted it locally.",
                CommonMistake = "Trusting client-reported damage values without server validation.",
                PracticePrompt = "Move damage resolution to the server and reject an illegal client claim.",
                GameUses = new List<string> { "Competitive multiplayer", "Anti-cheat design", "Persistent online worlds" },
                Tags = new List<string> { "server", "netcode", "security" },
                RelatedTermIds = new List<string> { "client-prediction", "tick-rate", "latency", "dedicated-server" }
            });

            Add(new GlossaryTermData
            {
                Id = "lag-compensation",
                Term = "Lag Compensation",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "Server-side techniques that account for latency when validating hits or interactions.",
                SimpleExplanation = "The server rewinds history a bit so shots still make sense for high-ping players.",
                WhyItMatters = "Lag compensation improves fairness in shooters without letting clients decide hits.",
                Example = "A hitscan shot is tested against where the target was when the shooter fired.",
                CommonMistake = "Rewinding too far, which creates shoot-behind-cover frustration.",
                PracticePrompt = "Document a rewind window policy and test it at 50 ms and 150 ms latency.",
                GameUses = new List<string> { "FPS netcode", "Hit registration", "Competitive fairness" },
                Tags = new List<string> { "netcode", "shooters", "fairness" },
                RelatedTermIds = new List<string> { "latency", "client-prediction", "server-reconciliation" },
                CodeExample = "server.RewindHitboxes(clientTime);\nbool hit = Raycast(...);"
            });

            Add(new GlossaryTermData
            {
                Id = "server-reconciliation",
                Term = "Server Reconciliation",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "Correcting the local client state when the authoritative server result differs from prediction.",
                SimpleExplanation = "When the server's truth arrives, smoothly fix your local guess.",
                WhyItMatters = "Reconciliation keeps prediction responsive without drifting forever from reality.",
                Example = "A predicted dash is corrected when the server says a wall blocked part of the move.",
                CommonMistake = "Snapping hard to server state every correction, creating visible rubber-banding.",
                PracticePrompt = "Add reconciliation with interpolation so corrections are less noticeable.",
                GameUses = new List<string> { "Movement netcode", "Action games", "Anti-desync" },
                Tags = new List<string> { "netcode", "correction", "sync" },
                RelatedTermIds = new List<string> { "client-prediction", "lag-compensation", "interpolation" },
                CodeExample = "ApplyServerState(state);\nReplayUnackedInputs();"
            });

            Add(new GlossaryTermData
            {
                Id = "entity-replication",
                Term = "Entity Replication",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Copying relevant object state from the authority to remote clients.",
                SimpleExplanation = "Keeping other players' characters and objects updated on your machine.",
                WhyItMatters = "Replication strategy determines bandwidth cost and perceived smoothness.",
                Example = "Only replicate position, health, and team for distant players.",
                CommonMistake = "Replicating every variable every tick regardless of relevance.",
                PracticePrompt = "Define a replication set for one enemy type and cut three unnecessary fields.",
                GameUses = new List<string> { "Shared world state", "Co-op games", "MMO entities" },
                Tags = new List<string> { "networking", "sync", "bandwidth" },
                RelatedTermIds = new List<string> { "interpolation", "tick-rate", "authoritative-server" }
            });

            Add(new GlossaryTermData
            {
                Id = "interpolation",
                Term = "Interpolation",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Smoothing remote entity motion by blending between recently received states.",
                SimpleExplanation = "Fill in the gaps between network updates so remote players look smooth.",
                WhyItMatters = "Interpolation hides tickiness caused by discrete network snapshots.",
                Example = "A remote runner is rendered slightly in the past between two pose updates.",
                CommonMistake = "Interpolating without a buffer, which causes stutter when packets jitter.",
                PracticePrompt = "Buffer two snapshots and interpolate a remote transform between them.",
                GameUses = new List<string> { "Remote players", "Spectators", "Co-op companions" },
                Tags = new List<string> { "smoothing", "netcode", "animation" },
                RelatedTermIds = new List<string> { "entity-replication", "jitter", "client-prediction" },
                CodeExample = "transform.position = Vector3.Lerp(from, to, t);"
            });

            Add(new GlossaryTermData
            {
                Id = "packet-loss",
                Term = "Packet Loss",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Network packets that never arrive and must be recovered or tolerated by the game.",
                SimpleExplanation = "Some messages vanish on the way and never show up.",
                WhyItMatters = "Games must stay playable even when the network drops data.",
                Example = "A lost input packet is resent or reconstructed from later state.",
                CommonMistake = "Assuming a perfect network during design and playtesting only on LAN.",
                PracticePrompt = "Simulate 5 percent packet loss and list which systems break first.",
                GameUses = new List<string> { "Mobile networks", "Competitive play", "Netcode QA" },
                Tags = new List<string> { "networking", "reliability", "qa" },
                RelatedTermIds = new List<string> { "jitter", "latency", "rollback" }
            });

            Add(new GlossaryTermData
            {
                Id = "jitter",
                Term = "Jitter",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Variation in packet arrival time that makes latency unstable.",
                SimpleExplanation = "Ping that jumps around instead of staying steady.",
                WhyItMatters = "Jitter causes hitching even when average latency looks acceptable.",
                Example = "Packets arrive at 40 ms, then 90 ms, then 35 ms in quick succession.",
                CommonMistake = "Tuning only for average ping and ignoring arrival variance.",
                PracticePrompt = "Graph packet arrival deltas during a match and compute jitter range.",
                GameUses = new List<string> { "QoS tuning", "Netcode buffers", "Mobile play" },
                Tags = new List<string> { "networking", "timing", "stability" },
                RelatedTermIds = new List<string> { "latency", "packet-loss", "interpolation" }
            });

            Add(new GlossaryTermData
            {
                Id = "matchmaking",
                Term = "Matchmaking",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Systems that group players into sessions based on rules like skill, region, or party size.",
                SimpleExplanation = "Finding fair opponents or teammates automatically.",
                WhyItMatters = "Matchmaking quality strongly affects retention and perceived fairness.",
                Example = "Players are matched within a skill rating range and the same region.",
                CommonMistake = "Expanding search so far that matches become unfair just to reduce queue time.",
                PracticePrompt = "Write matchmaking rules for skill, latency, and max queue time tradeoffs.",
                GameUses = new List<string> { "Competitive queues", "Casual lobbies", "Party play" },
                Tags = new List<string> { "multiplayer", "retention", "fairness" },
                RelatedTermIds = new List<string> { "latency", "user-acquisition", "retention" }
            });

            Add(new GlossaryTermData
            {
                Id = "nat-traversal",
                Term = "NAT Traversal",
                Abbreviation = "nat",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "Techniques that help peers connect through home routers and network address translation.",
                SimpleExplanation = "Helping two phones or PCs find each other even behind routers.",
                WhyItMatters = "Traversal failures are a common cause of join errors in peer-hosted games.",
                Example = "STUN or relay fallback lets two mobile players connect successfully.",
                CommonMistake = "Assuming all players can host without providing a relay option.",
                PracticePrompt = "Document a connection path: direct, STUN, then TURN relay fallback.",
                GameUses = new List<string> { "Peer-to-peer games", "Mobile multiplayer", "Co-op hosting" },
                Tags = new List<string> { "networking", "connectivity", "mobile" },
                RelatedTermIds = new List<string> { "dedicated-server", "packet-loss", "matchmaking" }
            });

            Add(new GlossaryTermData
            {
                Id = "dedicated-server",
                Term = "Dedicated Server",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A persistent authoritative host that runs independently of any player's device.",
                SimpleExplanation = "A always-on game server that is not tied to one player's phone or console.",
                WhyItMatters = "Dedicated servers improve stability, anti-cheat, and session continuity.",
                Example = "A battle-royale match runs on cloud servers even if the creator disconnects.",
                CommonMistake = "Using listen servers for competitive modes that need stronger authority.",
                PracticePrompt = "Compare cheat risk and disconnect handling for listen versus dedicated hosting.",
                GameUses = new List<string> { "Competitive games", "Live services", "MMO worlds" },
                Tags = new List<string> { "server", "hosting", "authority" },
                RelatedTermIds = new List<string> { "authoritative-server", "tick-rate", "matchmaking" }
            });

            Add(new GlossaryTermData
            {
                Id = "desync",
                Term = "Desync",
                Category = GlossaryCategory.Multiplayer,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "When simulated game states diverge across machines that should match.",
                SimpleExplanation = "Players or systems no longer agree on what is true in the match.",
                WhyItMatters = "Desyncs break multiplayer integrity and are hard to debug without determinism.",
                Example = "One client sees a door closed while another sees it open.",
                CommonMistake = "Ignoring non-deterministic APIs like unordered iteration or unsafe random calls.",
                PracticePrompt = "Add a checksum of critical state each tick and log the first mismatch.",
                GameUses = new List<string> { "Deterministic netcode", "Lockstep games", "QA tools" },
                Tags = new List<string> { "sync", "bugs", "netcode" },
                RelatedTermIds = new List<string> { "rollback", "client-prediction", "serialization" }
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
                CommonMistake = "Blaming one short frame spike for thermal issues that only appear after long sessions.",
                PracticePrompt = "Run a 20-minute combat soak on device and chart FPS against skin temperature.",
                GameUses = new List<string> { "Mobile QA", "Graphics budgeting", "Session length testing" },
                Tags = new List<string> { "mobile", "performance", "hardware" },
                RelatedTermIds = new List<string> { "memory-pressure", "texture-compression", "adaptive-performance", "battery-optimization" }
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
                CommonMistake = "Shipping uncompressed textures to save artist time while blowing memory budgets.",
                PracticePrompt = "Convert one atlas to a platform compressed format and compare memory before and after.",
                GameUses = new List<string> { "Mobile builds", "Asset pipelines", "Memory budgeting" },
                Tags = new List<string> { "textures", "memory", "mobile" },
                RelatedTermIds = new List<string> { "memory-pressure", "normal-map", "uv-mapping", "app-thinning" }
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
                CommonMistake = "Ignoring low-memory warnings until players report unexplained reloads.",
                PracticePrompt = "Force a heavy scene load on a low-end device and log memory warnings.",
                GameUses = new List<string> { "Mobile stability", "Asset streaming", "Crash prevention" },
                Tags = new List<string> { "memory", "mobile", "stability" },
                RelatedTermIds = new List<string> { "garbage-collection", "texture-compression", "thermal-throttling", "addressable-assets" }
            });

            Add(new GlossaryTermData
            {
                Id = "adaptive-performance",
                Term = "Adaptive Performance",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Dynamically lowering visual or simulation quality to maintain stable frame rate on device.",
                SimpleExplanation = "The game quietly reduces effects or resolution when the phone starts struggling.",
                WhyItMatters = "Adaptive performance helps sustain playable frame rates during long or demanding sessions.",
                Example = "Shadow quality drops after sustained GPU load while keeping gameplay at 30 FPS.",
                CommonMistake = "Dropping quality so aggressively that the game becomes visually confusing.",
                PracticePrompt = "Define three quality tiers and automatic triggers based on frame time and thermal state.",
                GameUses = new List<string> { "Mobile optimization", "Thermal management", "Battery-conscious tuning" },
                Tags = new List<string> { "mobile", "performance", "scaling" },
                RelatedTermIds = new List<string> { "thermal-throttling", "memory-pressure", "level-of-detail", "dynamic-resolution" }
            });

            Add(new GlossaryTermData
            {
                Id = "safe-area",
                Term = "Safe Area",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "The screen region guaranteed to be visible and interactive outside notches and system bars.",
                SimpleExplanation = "The part of the phone screen where UI will not be covered by cutouts or home indicators.",
                WhyItMatters = "Respecting safe areas prevents clipped buttons and unreadable HUD on modern devices.",
                Example = "Health bars and menus inset to avoid the notch and rounded corners.",
                CommonMistake = "Hardcoding offsets for one phone model and breaking layouts on others.",
                PracticePrompt = "Place a pause button using safe-area insets and verify on notch and non-notch layouts.",
                GameUses = new List<string> { "Mobile UI", "HUD layout", "Platform compliance" },
                Tags = new List<string> { "ui", "mobile", "layout" },
                RelatedTermIds = new List<string> { "touch-input", "app-store-guidelines", "onboarding" }
            });

            Add(new GlossaryTermData
            {
                Id = "touch-input",
                Term = "Touch Input",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Gameplay and UI control driven by finger contact, gestures, and multi-touch.",
                SimpleExplanation = "Players tap, swipe, and pinch instead of using a mouse or controller.",
                WhyItMatters = "Touch design determines accessibility, precision, and comfort on phones.",
                Example = "A virtual joystick plus swipe-to-dodge mapped to one-handed play.",
                CommonMistake = "Requiring tiny hit targets that fail fat-finger usability.",
                PracticePrompt = "Increase one critical button hit area to at least 44 points and retest one-handed play.",
                GameUses = new List<string> { "Mobile controls", "Gesture combat", "Casual UI" },
                Tags = new List<string> { "input", "mobile", "ux" },
                RelatedTermIds = new List<string> { "safe-area", "game-feel", "haptic-feedback" }
            });

            Add(new GlossaryTermData
            {
                Id = "frame-pacing",
                Term = "Frame Pacing",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Delivering frames at consistent intervals so motion feels smooth even when FPS is capped.",
                SimpleExplanation = "Spacing frames evenly so gameplay does not feel stuttery.",
                WhyItMatters = "Uneven pacing feels worse than a slightly lower but stable frame rate.",
                Example = "Locking presentation to 30 FPS with steady intervals instead of oscillating 45 to 25.",
                CommonMistake = "Chasing peak FPS while ignoring frame-time variance.",
                PracticePrompt = "Capture frame-time graphs before and after enabling consistent pacing.",
                GameUses = new List<string> { "Mobile feel", "Performance QA", "Display sync" },
                Tags = new List<string> { "frames", "performance", "feel" },
                RelatedTermIds = new List<string> { "delta-time", "adaptive-performance", "thermal-throttling" }
            });

            Add(new GlossaryTermData
            {
                Id = "dynamic-resolution",
                Term = "Dynamic Resolution",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "Scaling render resolution up or down at runtime to protect frame rate.",
                SimpleExplanation = "Drawing the game a bit blurrier when needed so it stays smooth.",
                WhyItMatters = "Dynamic resolution is a powerful lever for GPU-bound mobile titles.",
                Example = "Internal resolution drops during particle-heavy fights, then recovers in quieter scenes.",
                CommonMistake = "Scaling so far that UI or targeting readability collapses.",
                PracticePrompt = "Set min and max scale factors and verify readability at the lowest setting.",
                GameUses = new List<string> { "GPU budgeting", "Combat VFX scenes", "Mid-tier devices" },
                Tags = new List<string> { "resolution", "gpu", "mobile" },
                RelatedTermIds = new List<string> { "adaptive-performance", "overdraw", "frame-pacing" }
            });

            Add(new GlossaryTermData
            {
                Id = "battery-optimization",
                Term = "Battery Optimization",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Reducing CPU, GPU, network, and wake work so play sessions drain less power.",
                SimpleExplanation = "Making the game kinder to the phone battery during long sessions.",
                WhyItMatters = "Battery-friendly games get longer sessions and better store ratings.",
                Example = "Lowering tick rates and network polls while the app is backgrounded or idle.",
                CommonMistake = "Keeping full update loops running on menus with no visible benefit.",
                PracticePrompt = "Profile power draw for one combat scene and cut one always-on timer.",
                GameUses = new List<string> { "Idle screens", "Always-online games", "Long sessions" },
                Tags = new List<string> { "battery", "mobile", "performance" },
                RelatedTermIds = new List<string> { "thermal-throttling", "backgrounding", "adaptive-performance" }
            });

            Add(new GlossaryTermData
            {
                Id = "app-thinning",
                Term = "App Thinning",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Shipping only the assets and binaries a specific device needs to reduce install size.",
                SimpleExplanation = "Giving each phone just the slices of the game it actually requires.",
                WhyItMatters = "Smaller installs improve conversion and reduce uninstalls on storage-limited devices.",
                Example = "Delivering ASTC textures for capable devices and alternate formats for others.",
                CommonMistake = "Bundling every language and texture variant into one fat binary.",
                PracticePrompt = "Measure install size before and after enabling device-specific asset slicing.",
                GameUses = new List<string> { "Store conversion", "Asset delivery", "Mobile packaging" },
                Tags = new List<string> { "size", "packaging", "mobile" },
                RelatedTermIds = new List<string> { "texture-compression", "addressable-assets", "app-store-guidelines" }
            });

            Add(new GlossaryTermData
            {
                Id = "backgrounding",
                Term = "Backgrounding",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Handling the app when it leaves the foreground, including pause, save, and resume flows.",
                SimpleExplanation = "What happens when the player switches away from your game and comes back.",
                WhyItMatters = "Correct backgrounding prevents data loss, audio glitches, and network waste.",
                Example = "Autosaving inventory and muting audio when the OS sends a pause event.",
                CommonMistake = "Leaving network sockets and music running while the app is backgrounded.",
                PracticePrompt = "Interrupt a match with a phone call and verify save integrity plus clean resume.",
                GameUses = new List<string> { "Mobile lifecycle", "Save safety", "Audio systems" },
                Tags = new List<string> { "lifecycle", "mobile", "stability" },
                RelatedTermIds = new List<string> { "serialization", "battery-optimization", "push-notifications" }
            });

            Add(new GlossaryTermData
            {
                Id = "device-tiering",
                Term = "Device Tiering",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Grouping target hardware into performance classes with matching quality presets.",
                SimpleExplanation = "Sorting phones into low, mid, and high settings buckets.",
                WhyItMatters = "Tiering lets one build serve a wide audience without overtaxing weak devices.",
                Example = "Low tier disables shadows and caps particles while high tier enables them.",
                CommonMistake = "Shipping one quality preset for all phones and failing on older hardware.",
                PracticePrompt = "Define three tiers with concrete feature flags and map five devices into them.",
                GameUses = new List<string> { "Quality settings", "Market coverage", "Performance QA" },
                Tags = new List<string> { "devices", "quality", "mobile" },
                RelatedTermIds = new List<string> { "adaptive-performance", "dynamic-resolution", "thermal-throttling" }
            });

            Add(new GlossaryTermData
            {
                Id = "app-store-guidelines",
                Term = "App Store Guidelines",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Platform rules that govern submission, privacy, monetization, and content for store approval.",
                SimpleExplanation = "The checklist stores use to accept or reject your game.",
                WhyItMatters = "Guideline violations delay launches and can force redesigns late in production.",
                Example = "Updating privacy disclosures and restore-purchase flows before iOS review.",
                CommonMistake = "Ignoring guideline changes until a rejection blocks the release train.",
                PracticePrompt = "Audit one monetization flow against current store policy and list gaps.",
                GameUses = new List<string> { "Publishing", "IAP compliance", "Privacy reviews" },
                Tags = new List<string> { "publishing", "compliance", "mobile" },
                RelatedTermIds = new List<string> { "in-app-purchase", "platform-certification", "soft-launch" }
            });

            Add(new GlossaryTermData
            {
                Id = "push-notifications",
                Term = "Push Notifications",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Remote or local messages that re-engage players when the app is not open.",
                SimpleExplanation = "Alerts that invite players back with rewards, events, or reminders.",
                WhyItMatters = "Well-timed notifications boost retention, but spam drives uninstalls.",
                Example = "A daily reward reminder arrives at the player's usual play window.",
                CommonMistake = "Sending frequent generic pushes with no personalization or opt-out clarity.",
                PracticePrompt = "Design three notification types with frequency caps and an opt-out path.",
                GameUses = new List<string> { "Live ops", "Retention campaigns", "Event reminders" },
                Tags = new List<string> { "retention", "mobile", "engagement" },
                RelatedTermIds = new List<string> { "retention", "live-ops", "backgrounding" }
            });

            Add(new GlossaryTermData
            {
                Id = "addressable-assets",
                Term = "Addressable Assets",
                Category = GlossaryCategory.MobileDevelopment,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "A content delivery system that loads assets by address from local or remote catalogs.",
                SimpleExplanation = "Ask for content by name and let the system fetch it from disk or the cloud.",
                WhyItMatters = "Addressables enable smaller installs, hot updates, and controlled memory loading.",
                Example = "Seasonal skins download on demand instead of shipping in the base package.",
                CommonMistake = "Hardcoding bundle paths that break when catalogs are updated.",
                PracticePrompt = "Mark one remote group addressable and load it asynchronously in a test scene.",
                GameUses = new List<string> { "Live content", "DLC", "Memory streaming" },
                Tags = new List<string> { "assets", "delivery", "mobile" },
                RelatedTermIds = new List<string> { "app-thinning", "memory-pressure", "live-ops" },
                CodeExample = "var handle = Addressables.LoadAssetAsync<GameObject>(key);\nawait handle.Task;"
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
                CommonMistake = "Only testing the ticket that changed and skipping adjacent critical paths.",
                PracticePrompt = "Add three regression cases for the last patch and run them on the next build.",
                GameUses = new List<string> { "Patch validation", "Live ops releases", "Automation suites" },
                Tags = new List<string> { "qa", "testing", "quality" },
                RelatedTermIds = new List<string> { "smoke-testing", "stress-testing", "ci-cd-pipeline", "unit-testing" }
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
                CommonMistake = "Calling a multi-hour exploratory pass a smoke test and delaying feedback.",
                PracticePrompt = "Write a ten-minute smoke checklist covering boot, save, combat, and quit.",
                GameUses = new List<string> { "Daily builds", "Release gates", "CI validation" },
                Tags = new List<string> { "qa", "build", "sanity" },
                RelatedTermIds = new List<string> { "regression-testing", "build-verification", "ci-cd-pipeline" }
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
                CommonMistake = "Stopping at the first failure instead of documenting the load threshold.",
                PracticePrompt = "Increase concurrent AI agents until FPS drops below 20 and record the count.",
                GameUses = new List<string> { "Server capacity", "Performance QA", "Crash hunting" },
                Tags = new List<string> { "qa", "performance", "stability" },
                RelatedTermIds = new List<string> { "regression-testing", "memory-pressure", "soak-testing", "playtesting" }
            });

            Add(new GlossaryTermData
            {
                Id = "playtesting",
                Term = "Playtesting",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Observing real players use a build to evaluate fun, clarity, and usability.",
                SimpleExplanation = "Watch people play and learn what confuses, frustrates, or delights them.",
                WhyItMatters = "Playtesting surfaces design issues that automated tests and internal reviews miss.",
                Example = "Five testers fail to find the objective until the tutorial arrow is added.",
                CommonMistake = "Interrupting testers with coaching instead of watching natural behavior.",
                PracticePrompt = "Run a silent 20-minute playtest and capture three friction moments with timestamps.",
                GameUses = new List<string> { "UX validation", "Tutorial tuning", "Balance feedback" },
                Tags = new List<string> { "qa", "feedback", "design" },
                RelatedTermIds = new List<string> { "smoke-testing", "onboarding", "a-b-testing", "bug-triage" }
            });

            Add(new GlossaryTermData
            {
                Id = "unit-testing",
                Term = "Unit Testing",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Automated tests that verify a small unit of code in isolation.",
                SimpleExplanation = "Tiny checks that prove one function or class behaves correctly.",
                WhyItMatters = "Unit tests catch logic bugs early and make refactoring safer.",
                Example = "A damage calculator test asserts critical hits multiply correctly.",
                CommonMistake = "Writing tests that depend on full scene setup and fail for unrelated reasons.",
                PracticePrompt = "Add three unit tests around one pure gameplay function and run them in CI.",
                GameUses = new List<string> { "Combat math", "Save serialization", "Economy rules" },
                Tags = new List<string> { "qa", "automation", "code" },
                RelatedTermIds = new List<string> { "integration-testing", "ci-cd-pipeline", "dependency-injection" },
                CodeExample = "Assert.AreEqual(20, Damage.Apply(10, multiplier: 2f));"
            });

            Add(new GlossaryTermData
            {
                Id = "integration-testing",
                Term = "Integration Testing",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Tests that verify multiple systems work correctly together across boundaries.",
                SimpleExplanation = "Checking that connected pieces cooperate, not just each piece alone.",
                WhyItMatters = "Integration tests catch interface mismatches that unit tests miss.",
                Example = "A test starts a match, awards loot, and confirms inventory and analytics both update.",
                CommonMistake = "Treating flaky end-to-end flows as unit tests and ignoring root causes.",
                PracticePrompt = "Write one integration test covering save write plus reload for inventory.",
                GameUses = new List<string> { "Systems handoffs", "Network flows", "Persistence" },
                Tags = new List<string> { "qa", "systems", "automation" },
                RelatedTermIds = new List<string> { "unit-testing", "regression-testing", "ci-cd-pipeline" }
            });

            Add(new GlossaryTermData
            {
                Id = "soak-testing",
                Term = "Soak Testing",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "Running a build under realistic load for a long duration to find slow leaks and drift.",
                SimpleExplanation = "Leave the game running for hours to catch problems that short tests miss.",
                WhyItMatters = "Soak tests reveal memory growth, thermal decay, and rare timing bugs.",
                Example = "An overnight idle lobby run shows steadily rising native memory.",
                CommonMistake = "Only soak on developer machines that never match shipping hardware.",
                PracticePrompt = "Run a two-hour automated combat loop and chart memory every five minutes.",
                GameUses = new List<string> { "Live stability", "Mobile thermal QA", "Server endurance" },
                Tags = new List<string> { "qa", "stability", "endurance" },
                RelatedTermIds = new List<string> { "stress-testing", "memory-pressure", "thermal-throttling" }
            });

            Add(new GlossaryTermData
            {
                Id = "compatibility-testing",
                Term = "Compatibility Testing",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Validating the game across target devices, OS versions, and hardware configurations.",
                SimpleExplanation = "Making sure the game works on the phones and platforms you claim to support.",
                WhyItMatters = "Compatibility gaps cause one-star reviews and support spikes after launch.",
                Example = "Testing login and graphics quality presets on low, mid, and high Android tiers.",
                CommonMistake = "Testing only the newest flagship devices used by the development team.",
                PracticePrompt = "Build a device matrix of five targets and execute the smoke suite on each.",
                GameUses = new List<string> { "Mobile release QA", "Console SKUs", "OS upgrade checks" },
                Tags = new List<string> { "qa", "devices", "coverage" },
                RelatedTermIds = new List<string> { "device-tiering", "smoke-testing", "localization-qa" }
            });

            Add(new GlossaryTermData
            {
                Id = "bug-triage",
                Term = "Bug Triage",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Reviewing, prioritizing, and assigning defects based on severity and impact.",
                SimpleExplanation = "Sorting bugs so the worst problems get fixed first.",
                WhyItMatters = "Good triage protects schedules and focuses engineering on player-facing risk.",
                Example = "A crash on boot is marked blocker while a rare VFX pop is deferred.",
                CommonMistake = "Prioritizing by who filed the bug instead of player impact and frequency.",
                PracticePrompt = "Triage ten open bugs into severity buckets with owners and target milestones.",
                GameUses = new List<string> { "Sprint planning", "Launch readiness", "Live support" },
                Tags = new List<string> { "qa", "process", "priority" },
                RelatedTermIds = new List<string> { "test-plan", "regression-testing", "milestone" }
            });

            Add(new GlossaryTermData
            {
                Id = "test-plan",
                Term = "Test Plan",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "A document describing scope, environments, cases, and exit criteria for testing.",
                SimpleExplanation = "The written plan for what will be tested, how, and when it is done enough.",
                WhyItMatters = "Plans align QA, design, and engineering on risk coverage before a milestone.",
                Example = "A soft-launch plan covers onboarding, IAP, crash rate, and retention dashboards.",
                CommonMistake = "Writing a plan nobody updates, so testing drifts from current risks.",
                PracticePrompt = "Draft a one-page plan for your next milestone with entry and exit criteria.",
                GameUses = new List<string> { "Milestone QA", "Release readiness", "Outsourcing handoff" },
                Tags = new List<string> { "qa", "planning", "process" },
                RelatedTermIds = new List<string> { "smoke-testing", "bug-triage", "build-verification" }
            });

            Add(new GlossaryTermData
            {
                Id = "build-verification",
                Term = "Build Verification",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "Confirming a new package installs, launches, and passes critical checks before wider QA.",
                SimpleExplanation = "Making sure the latest build is worth testing at all.",
                WhyItMatters = "Verification stops broken packages from wasting the whole team's day.",
                Example = "A BVT confirms version number, login, and a match start on a clean install.",
                CommonMistake = "Skipping verification and discovering the build cannot boot an hour later.",
                PracticePrompt = "Automate three BVT checks that must pass before the smoke suite starts.",
                GameUses = new List<string> { "CI gates", "Daily QA", "Release candidates" },
                Tags = new List<string> { "qa", "builds", "gates" },
                RelatedTermIds = new List<string> { "smoke-testing", "ci-cd-pipeline", "release-candidate" }
            });

            Add(new GlossaryTermData
            {
                Id = "golden-master",
                Term = "Golden Master",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "A trusted baseline build or output used to detect unexpected changes.",
                SimpleExplanation = "A known-good reference you compare new results against.",
                WhyItMatters = "Golden masters catch silent regressions in rendering, audio, or deterministic sims.",
                Example = "Screenshot diffs fail when a lighting tweak unexpectedly darkens a tutorial room.",
                CommonMistake = "Updating the golden without review, which hides real regressions.",
                PracticePrompt = "Capture a golden screenshot for one scene and fail CI on pixel-diff thresholds.",
                GameUses = new List<string> { "Visual QA", "Deterministic replay", "Audio mix checks" },
                Tags = new List<string> { "qa", "baseline", "automation" },
                RelatedTermIds = new List<string> { "regression-testing", "unit-testing", "ci-cd-pipeline" }
            });

            Add(new GlossaryTermData
            {
                Id = "a-b-testing",
                Term = "A/B Testing",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Comparing two or more variants with live players to measure which performs better.",
                SimpleExplanation = "Show different versions to different players and keep the winner.",
                WhyItMatters = "A/B tests replace opinion wars with evidence for design and monetization changes.",
                Example = "Half of new users see a shorter tutorial; retention and completion are compared.",
                CommonMistake = "Calling a change validated after too small a sample or too short a window.",
                PracticePrompt = "Define a primary metric and minimum sample size before launching one experiment.",
                GameUses = new List<string> { "Onboarding experiments", "Store UX", "Economy tuning" },
                Tags = new List<string> { "analytics", "experiments", "qa" },
                RelatedTermIds = new List<string> { "playtesting", "retention", "conversion-rate", "cohort-analysis" }
            });

            Add(new GlossaryTermData
            {
                Id = "localization-qa",
                Term = "Localization QA",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Testing translated text, layouts, fonts, and cultural fit across supported languages.",
                SimpleExplanation = "Checking that other languages look correct and still make sense in context.",
                WhyItMatters = "Loc QA prevents truncated UI, broken fonts, and embarrassing mistranslations.",
                Example = "German strings overflow a button and require layout fixes before certification.",
                CommonMistake = "Reviewing spreadsheets only and never verifying strings in the running build.",
                PracticePrompt = "Play one tutorial flow in a long-string language and log every overflow.",
                GameUses = new List<string> { "Global launches", "UI polish", "Console certification" },
                Tags = new List<string> { "qa", "localization", "ui" },
                RelatedTermIds = new List<string> { "localization", "compatibility-testing", "platform-certification" }
            });

            Add(new GlossaryTermData
            {
                Id = "ci-cd-pipeline",
                Term = "CI/CD Pipeline",
                Abbreviation = "cicd",
                Category = GlossaryCategory.QATesting,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Automated continuous integration and delivery that builds, tests, and packages changes.",
                SimpleExplanation = "A robot that builds your game and runs checks every time code lands.",
                WhyItMatters = "Pipelines shorten feedback loops and reduce human error in release packaging.",
                Example = "Every merge triggers unit tests, a player build, and artifact upload.",
                CommonMistake = "Allowing red pipelines to linger so broken main becomes normal.",
                PracticePrompt = "Add one failing-gate test to CI and confirm merges are blocked until it passes.",
                GameUses = new List<string> { "Team velocity", "Release automation", "Quality gates" },
                Tags = new List<string> { "ci", "automation", "devops" },
                Synonyms = new List<string> { "continuous integration", "continuous delivery" },
                RelatedTermIds = new List<string> { "unit-testing", "build-verification", "regression-testing" }
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
                CommonMistake = "Celebrating DAU spikes from bots or duplicated accounts as real growth.",
                PracticePrompt = "Chart DAU for two weeks around an event and annotate content changes.",
                GameUses = new List<string> { "Live ops tracking", "Event measurement", "Growth reporting" },
                Tags = new List<string> { "analytics", "engagement", "kpi" },
                Synonyms = new List<string> { "daily active users" },
                RelatedTermIds = new List<string> { "mau", "ltv", "live-ops", "session-length" }
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
                CommonMistake = "Comparing MAU across unequal month lengths without normalization.",
                PracticePrompt = "Compute DAU/MAU stickiness for the last three months and explain the trend.",
                GameUses = new List<string> { "Portfolio reporting", "Retention analysis", "Investor updates" },
                Tags = new List<string> { "analytics", "engagement", "kpi" },
                Synonyms = new List<string> { "monthly active users" },
                RelatedTermIds = new List<string> { "dau", "ltv", "retention", "churn" }
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
                CommonMistake = "Optimizing only day-one revenue while ignoring long-tail LTV.",
                PracticePrompt = "Estimate 90-day LTV for one cohort and compare it to current CPI.",
                GameUses = new List<string> { "Monetization strategy", "UA budgeting", "Economy tuning" },
                Tags = new List<string> { "analytics", "monetization", "kpi" },
                Synonyms = new List<string> { "lifetime value" },
                RelatedTermIds = new List<string> { "dau", "mau", "soft-launch", "arpu" }
            });

            Add(new GlossaryTermData
            {
                Id = "retention",
                Term = "Retention",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "The rate at which players return to a game after their first session.",
                SimpleExplanation = "How many people come back tomorrow, next week, or next month.",
                WhyItMatters = "Retention shows whether onboarding, content, and updates keep players engaged.",
                Example = "Day-7 retention rises after improving the first-hour tutorial and daily rewards.",
                CommonMistake = "Looking only at install counts while retention quietly collapses.",
                PracticePrompt = "Report D1, D7, and D30 retention for your latest build cohort.",
                GameUses = new List<string> { "Live ops planning", "Onboarding tuning", "KPI reporting" },
                Tags = new List<string> { "analytics", "engagement", "kpi" },
                RelatedTermIds = new List<string> { "dau", "onboarding", "churn", "cohort-analysis" }
            });

            Add(new GlossaryTermData
            {
                Id = "arpu",
                Term = "ARPU",
                Abbreviation = "arpu",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Average Revenue Per User across the active audience in a period.",
                SimpleExplanation = "How much revenue you earn on average from each player.",
                WhyItMatters = "ARPU helps compare monetization strength across builds and markets.",
                Example = "A battle pass season lifts ARPU even when DAU stays flat.",
                CommonMistake = "Raising prices without watching conversion drop enough to hurt total revenue.",
                PracticePrompt = "Calculate ARPU before and after a monetization change for the same cohort window.",
                GameUses = new List<string> { "Monetization reviews", "Market comparisons", "Season analysis" },
                Tags = new List<string> { "revenue", "kpi", "monetization" },
                Synonyms = new List<string> { "average revenue per user" },
                RelatedTermIds = new List<string> { "arpdau", "ltv", "conversion-rate", "in-app-purchase" }
            });

            Add(new GlossaryTermData
            {
                Id = "arpdau",
                Term = "ARPDAU",
                Abbreviation = "arpdau",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Average Revenue Per Daily Active User for a given day.",
                SimpleExplanation = "How much money each daily player is worth on that day.",
                WhyItMatters = "ARPDAU is a sensitive daily pulse for live ops and monetization experiments.",
                Example = "A weekend event doubles ARPDAU while DAU rises only modestly.",
                CommonMistake = "Judging a feature by ARPDAU alone without checking retention side effects.",
                PracticePrompt = "Plot ARPDAU for fourteen days and mark each live ops intervention.",
                GameUses = new List<string> { "Daily live ops", "Offer testing", "Economy health" },
                Tags = new List<string> { "revenue", "daily", "kpi" },
                Synonyms = new List<string> { "average revenue per daily active user" },
                RelatedTermIds = new List<string> { "arpu", "dau", "battle-pass", "in-app-purchase" }
            });

            Add(new GlossaryTermData
            {
                Id = "churn",
                Term = "Churn",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "The rate at which players stop returning within a defined inactivity window.",
                SimpleExplanation = "How quickly players leave and do not come back.",
                WhyItMatters = "Churn diagnosis points teams to broken onboarding, difficulty spikes, or stale content.",
                Example = "Players who skip seven days are marked churned for reactivation campaigns.",
                CommonMistake = "Defining churn so loosely that seasonal players look permanently lost.",
                PracticePrompt = "Identify the top three screens last seen by churned D7 players.",
                GameUses = new List<string> { "Retention war rooms", "Reactivation offers", "Content planning" },
                Tags = new List<string> { "analytics", "retention", "risk" },
                RelatedTermIds = new List<string> { "retention", "cohort-analysis", "session-length", "k-factor" }
            });

            Add(new GlossaryTermData
            {
                Id = "session-length",
                Term = "Session Length",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "How long a typical play session lasts from start to finish.",
                SimpleExplanation = "How many minutes players stick around in one sitting.",
                WhyItMatters = "Session length informs content chunking, ad placement, and mobile comfort.",
                Example = "A mobile puzzler targets five-minute sessions that fit a commute.",
                CommonMistake = "Forcing long sessions with energy walls that frustrate short-session players.",
                PracticePrompt = "Measure median session length before and after changing level length.",
                GameUses = new List<string> { "Content pacing", "Ad timing", "Mobile UX" },
                Tags = new List<string> { "analytics", "sessions", "engagement" },
                RelatedTermIds = new List<string> { "dau", "retention", "core-loop" }
            });

            Add(new GlossaryTermData
            {
                Id = "conversion-rate",
                Term = "Conversion Rate",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "The percentage of players who complete a desired action such as purchase or tutorial finish.",
                SimpleExplanation = "How many people who saw an opportunity actually took it.",
                WhyItMatters = "Conversion rates reveal friction in funnels from install to paying or completing goals.",
                Example = "Store checkout conversion rises after simplifying the purchase confirmation screen.",
                CommonMistake = "Optimizing conversion with dark patterns that damage long-term trust.",
                PracticePrompt = "Map one funnel from store impression to purchase and compute step conversion.",
                GameUses = new List<string> { "IAP funnels", "Tutorial completion", "Ad opt-ins" },
                Tags = new List<string> { "funnels", "monetization", "kpi" },
                RelatedTermIds = new List<string> { "funnel-analysis", "in-app-purchase", "arpu", "a-b-testing" }
            });

            Add(new GlossaryTermData
            {
                Id = "cohort-analysis",
                Term = "Cohort Analysis",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "Grouping players by shared start attributes and comparing their behavior over time.",
                SimpleExplanation = "Track players who started together and see how their journeys differ.",
                WhyItMatters = "Cohorts expose whether changes help new players or only existing whales.",
                Example = "Players who joined during a Halloween event retain differently than organic January installs.",
                CommonMistake = "Averaging all users together and hiding cohort-specific regressions.",
                PracticePrompt = "Compare D7 retention for two install-week cohorts around a tutorial change.",
                GameUses = new List<string> { "Patch evaluation", "UA channel quality", "Season reviews" },
                Tags = new List<string> { "analytics", "cohorts", "insight" },
                RelatedTermIds = new List<string> { "retention", "churn", "funnel-analysis", "user-acquisition" }
            });

            Add(new GlossaryTermData
            {
                Id = "funnel-analysis",
                Term = "Funnel Analysis",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Measuring drop-off across ordered steps toward a goal.",
                SimpleExplanation = "Finding where players abandon a path you wanted them to complete.",
                WhyItMatters = "Funnels turn vague retention problems into specific broken steps.",
                Example = "Sixty percent of players leave between weapon pickup and first enemy kill.",
                CommonMistake = "Tracking funnels without timestamps, so sequence errors are invisible.",
                PracticePrompt = "Instrument a four-step tutorial funnel and prioritize the largest drop-off.",
                GameUses = new List<string> { "Onboarding", "Purchase flows", "FTUE tuning" },
                Tags = new List<string> { "analytics", "funnels", "ux" },
                RelatedTermIds = new List<string> { "conversion-rate", "onboarding", "cohort-analysis", "a-b-testing" }
            });

            Add(new GlossaryTermData
            {
                Id = "in-app-purchase",
                Term = "In-App Purchase",
                Abbreviation = "iap",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "A real-money transaction inside a game for currency, items, or unlocks.",
                SimpleExplanation = "Players pay inside the app for something beyond the free download.",
                WhyItMatters = "IAP design must balance revenue with fairness and platform compliance.",
                Example = "A starter pack offers currency and a cosmetic at a first-purchase discount.",
                CommonMistake = "Gatekeeping core progression behind purchases in ways that violate store rules or trust.",
                PracticePrompt = "List every IAP and mark which are cosmetic versus power-affecting.",
                GameUses = new List<string> { "F2P monetization", "Mobile stores", "Live ops offers" },
                Tags = new List<string> { "monetization", "iap", "stores" },
                Synonyms = new List<string> { "IAP" },
                RelatedTermIds = new List<string> { "battle-pass", "economy-design", "app-store-guidelines", "arpu" }
            });

            Add(new GlossaryTermData
            {
                Id = "battle-pass",
                Term = "Battle Pass",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A time-limited progression track that rewards play, often with a premium paid track.",
                SimpleExplanation = "A seasonal checklist of rewards you unlock by playing during the season.",
                WhyItMatters = "Battle passes monetize engagement while giving free players a clear goal ladder.",
                Example = "A ten-week pass grants cosmetics on free and premium tracks as XP accumulates.",
                CommonMistake = "Requiring impossible grind so premium buyers still cannot finish without more spend.",
                PracticePrompt = "Sketch a twenty-tier pass with earn rates that a median player can complete.",
                GameUses = new List<string> { "Live seasons", "Retention hooks", "Cosmetic monetization" },
                Tags = new List<string> { "monetization", "seasons", "progression" },
                RelatedTermIds = new List<string> { "metagame", "live-ops", "in-app-purchase", "progression" }
            });

            Add(new GlossaryTermData
            {
                Id = "user-acquisition",
                Term = "User Acquisition",
                Abbreviation = "ua",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Paid or organic efforts to attract new players into the game.",
                SimpleExplanation = "How you find and bring in new players.",
                WhyItMatters = "UA efficiency determines whether growth is sustainable against LTV.",
                Example = "A creative test lowers cost per install while preserving D7 retention.",
                CommonMistake = "Scaling spend into channels with good installs but terrible retention.",
                PracticePrompt = "Compare CPI and D7 retention across two creative variants for one channel.",
                GameUses = new List<string> { "Launch marketing", "Live growth", "Soft launch scaling" },
                Tags = new List<string> { "growth", "marketing", "ua" },
                Synonyms = new List<string> { "UA" },
                RelatedTermIds = new List<string> { "ltv", "k-factor", "soft-launch", "retention" }
            });

            Add(new GlossaryTermData
            {
                Id = "k-factor",
                Term = "K-Factor",
                Abbreviation = "k",
                Category = GlossaryCategory.AnalyticsMonetization,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "A viral coefficient estimating how many new players each existing player brings in.",
                SimpleExplanation = "How strongly your players recruit other players.",
                WhyItMatters = "K-factor helps teams understand organic growth beyond paid acquisition.",
                Example = "Invite rewards produce 0.3 new installs per active player in a season.",
                CommonMistake = "Counting installs that would have happened anyway as viral successes.",
                PracticePrompt = "Instrument invite sends and resulting installs, then compute a weekly K-factor.",
                GameUses = new List<string> { "Invite systems", "Social features", "Growth modeling" },
                Tags = new List<string> { "viral", "growth", "analytics" },
                RelatedTermIds = new List<string> { "user-acquisition", "retention", "conversion-rate" }
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
                CommonMistake = "Calling a feature-complete product an MVP and delaying learning for months.",
                PracticePrompt = "Define an MVP feature list that can be playtested in four weeks or less.",
                GameUses = new List<string> { "Prototype validation", "Pitching", "Scope control" },
                Tags = new List<string> { "production", "scope", "prototype" },
                Synonyms = new List<string> { "minimum viable product" },
                RelatedTermIds = new List<string> { "vertical-slice", "soft-launch", "scope-creep", "game-design-document" }
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
                CommonMistake = "Treating soft launch as a tiny marketing push with no instrumentation or tuning time.",
                PracticePrompt = "Write soft-launch success criteria for retention, crash rate, and ARPDAU.",
                GameUses = new List<string> { "Mobile publishing", "Economy tuning", "Live readiness" },
                Tags = new List<string> { "publishing", "launch", "testing" },
                RelatedTermIds = new List<string> { "mvp", "live-ops", "ltv", "user-acquisition" }
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
                CommonMistake = "Shipping calendars so dense that content quality and QA collapse.",
                PracticePrompt = "Plan a four-week live ops calendar with owners, goals, and rollback plans.",
                GameUses = new List<string> { "Service games", "Seasonal content", "Retention strategy" },
                Tags = new List<string> { "operations", "updates", "retention" },
                Synonyms = new List<string> { "live operations", "liveops" },
                RelatedTermIds = new List<string> { "soft-launch", "dau", "progression", "battle-pass" }
            });

            Add(new GlossaryTermData
            {
                Id = "scope-creep",
                Term = "Scope Creep",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "When a project's feature list grows beyond the original plan without matching time or budget.",
                SimpleExplanation = "The game keeps gaining new ideas faster than the team can build them.",
                WhyItMatters = "Scope creep delays launches, burns teams out, and dilutes the core experience.",
                Example = "A small puzzle game adds multiplayer, crafting, and a battle pass before the first release.",
                CommonMistake = "Accepting every stakeholder request without cutting something else.",
                PracticePrompt = "List five proposed features and cut two that do not serve the MVP core loop.",
                GameUses = new List<string> { "Production planning", "Milestone scoping", "Stakeholder alignment" },
                Tags = new List<string> { "production", "scope", "planning" },
                RelatedTermIds = new List<string> { "mvp", "vertical-slice", "milestone", "crunch" }
            });

            Add(new GlossaryTermData
            {
                Id = "game-design-document",
                Term = "Game Design Document",
                Abbreviation = "gdd",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "A living reference that captures vision, systems, constraints, and content goals.",
                SimpleExplanation = "The shared write-up of what the game is and how its systems should work.",
                WhyItMatters = "A clear GDD reduces misalignment across design, art, engineering, and production.",
                Example = "Pillars, core loop, camera rules, and progression charts live in one editable doc.",
                CommonMistake = "Writing a huge unread GDD once, then never updating it as the game changes.",
                PracticePrompt = "Create a two-page GDD covering pillars, core loop, and out-of-scope list.",
                GameUses = new List<string> { "Kickoffs", "Vendor handoff", "Vertical slice planning" },
                Tags = new List<string> { "documentation", "design", "production" },
                Synonyms = new List<string> { "GDD" },
                RelatedTermIds = new List<string> { "mvp", "vertical-slice", "milestone" }
            });

            Add(new GlossaryTermData
            {
                Id = "milestone",
                Term = "Milestone",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "A scheduled production checkpoint with concrete deliverables and review criteria.",
                SimpleExplanation = "A dated goal that shows what must be playable or finished by then.",
                WhyItMatters = "Milestones keep scope honest and create moments to course-correct.",
                Example = "Alpha milestone requires all vertical-slice systems playable end to end.",
                CommonMistake = "Setting date-only milestones with no acceptance criteria.",
                PracticePrompt = "Write acceptance criteria for your next milestone in one checklist page.",
                GameUses = new List<string> { "Production schedules", "Publisher reporting", "Team alignment" },
                Tags = new List<string> { "production", "planning", "schedule" },
                RelatedTermIds = new List<string> { "alpha-beta", "sprint", "vertical-slice", "scope-creep" }
            });

            Add(new GlossaryTermData
            {
                Id = "alpha-beta",
                Term = "Alpha / Beta",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Development phases where alpha proves incomplete systems and beta focuses on polish and scale.",
                SimpleExplanation = "Alpha is still rough but playable; beta is closer to ship and needs wider testing.",
                WhyItMatters = "Phase labels set expectations for content completeness and bug severity targets.",
                Example = "Closed beta expands to thousands of players to validate servers and onboarding.",
                CommonMistake = "Calling a feature-incomplete build beta to satisfy a calendar promise.",
                PracticePrompt = "Define exit criteria that separate your alpha from beta for content and stability.",
                GameUses = new List<string> { "External tests", "Publisher gates", "Server validation" },
                Tags = new List<string> { "production", "phases", "testing" },
                RelatedTermIds = new List<string> { "milestone", "soft-launch", "release-candidate", "playtesting" }
            });

            Add(new GlossaryTermData
            {
                Id = "release-candidate",
                Term = "Release Candidate",
                Abbreviation = "rc",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A build intended to ship if final verification finds no blocking issues.",
                SimpleExplanation = "The almost-final package you will release unless a blocker appears.",
                WhyItMatters = "RC discipline prevents late risky changes from destabilizing launch.",
                Example = "RC1 passes certification smoke tests and waits on a final store screenshot update.",
                CommonMistake = "Continuing to merge non-critical features into an RC branch.",
                PracticePrompt = "Create an RC change policy listing what may and may not land after RC tag.",
                GameUses = new List<string> { "Submission", "Certification", "Launch readiness" },
                Tags = new List<string> { "release", "build", "shipping" },
                Synonyms = new List<string> { "RC" },
                RelatedTermIds = new List<string> { "build-verification", "platform-certification", "alpha-beta" }
            });

            Add(new GlossaryTermData
            {
                Id = "post-mortem",
                Term = "Post Mortem",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "A structured review after a milestone or launch that captures what worked and what failed.",
                SimpleExplanation = "A honest team meeting that turns recent pain into future process improvements.",
                WhyItMatters = "Post-mortems convert expensive mistakes into institutional learning.",
                Example = "After a rocky launch, the team documents root causes for crash triage delays.",
                CommonMistake = "Blaming individuals instead of systems, handoffs, and missing safeguards.",
                PracticePrompt = "Run a blameless post-mortem with timeline, root causes, and three action items.",
                GameUses = new List<string> { "Launch reviews", "Incident response", "Process improvement" },
                Tags = new List<string> { "process", "learning", "production" },
                RelatedTermIds = new List<string> { "milestone", "crunch", "live-ops" }
            });

            Add(new GlossaryTermData
            {
                Id = "localization",
                Term = "Localization",
                Abbreviation = "l10n",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Adapting text, audio, imagery, and cultural details for specific languages and regions.",
                SimpleExplanation = "Preparing the game so players in other countries can understand and enjoy it.",
                WhyItMatters = "Localization expands market reach and is often required for platform deals.",
                Example = "UI, subtitles, and voice lines are adapted for five launch languages.",
                CommonMistake = "Leaving localization until certification week with no string freeze.",
                PracticePrompt = "Create a localization schedule with string freeze, translation, and LQA gates.",
                GameUses = new List<string> { "Global launches", "Console SKUs", "Narrative games" },
                Tags = new List<string> { "loc", "publishing", "content" },
                Synonyms = new List<string> { "L10n" },
                RelatedTermIds = new List<string> { "localization-qa", "voice-over", "platform-certification" }
            });

            Add(new GlossaryTermData
            {
                Id = "platform-certification",
                Term = "Platform Certification",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Advanced,
                ShortDefinition = "Mandatory technical and compliance testing by platform holders before store release.",
                SimpleExplanation = "The official exam your build must pass to appear on a console or storefront.",
                WhyItMatters = "Certification failures can delay launches by weeks if caught late.",
                Example = "A console submission fails for improper suspend-resume handling and must be resubmitted.",
                CommonMistake = "Treating TRC or store checks as optional until the final week.",
                PracticePrompt = "Run an internal certification checklist two milestones before submission.",
                GameUses = new List<string> { "Console publishing", "Store submission", "Compliance" },
                Tags = new List<string> { "certification", "publishing", "platforms" },
                Synonyms = new List<string> { "cert", "TRC", "lotcheck" },
                RelatedTermIds = new List<string> { "release-candidate", "app-store-guidelines", "localization-qa" }
            });

            Add(new GlossaryTermData
            {
                Id = "publisher",
                Term = "Publisher",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "A partner that funds, markets, distributes, or certifies a game in exchange for commercial terms.",
                SimpleExplanation = "The company that helps get your game funded, marketed, and onto platforms.",
                WhyItMatters = "Publisher relationships shape milestones, creative approvals, and revenue shares.",
                Example = "A publisher funds production and handles console submission while the studio focuses on gameplay.",
                CommonMistake = "Signing terms without understanding milestone deliverables and kill criteria.",
                PracticePrompt = "List five questions you would ask a publisher about creative control and recoupment.",
                GameUses = new List<string> { "Funding", "Distribution", "Marketing support" },
                Tags = new List<string> { "business", "publishing", "partnerships" },
                RelatedTermIds = new List<string> { "milestone", "soft-launch", "intellectual-property" }
            });

            Add(new GlossaryTermData
            {
                Id = "sprint",
                Term = "Sprint",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Beginner,
                ShortDefinition = "A short, time-boxed development cycle with a committed set of deliverables.",
                SimpleExplanation = "A one or two week work burst with a clear finish line.",
                WhyItMatters = "Sprints create predictable planning cadence and frequent integration points.",
                Example = "A two-week sprint delivers a new enemy type, related VFX, and automated tests.",
                CommonMistake = "Overcommitting sprint scope and carrying unfinished work indefinitely.",
                PracticePrompt = "Plan one sprint with capacity estimates and a demoable goal at the end.",
                GameUses = new List<string> { "Agile teams", "Milestone breakdown", "Live ops patches" },
                Tags = new List<string> { "agile", "planning", "production" },
                RelatedTermIds = new List<string> { "milestone", "bug-triage", "ci-cd-pipeline" }
            });

            Add(new GlossaryTermData
            {
                Id = "crunch",
                Term = "Crunch",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Extended overtime used to meet deadlines, often signaling planning or scope failure.",
                SimpleExplanation = "Working long extra hours because the schedule and scope no longer match.",
                WhyItMatters = "Chronic crunch harms health, quality, and retention of experienced staff.",
                Example = "A team works nights for three weeks to hit a publisher milestone.",
                CommonMistake = "Normalizing crunch as culture instead of treating it as a planning emergency.",
                PracticePrompt = "Identify one recurring crunch cause and propose a scope or staffing countermeasure.",
                GameUses = new List<string> { "Milestone recovery", "Studio culture", "Risk planning" },
                Tags = new List<string> { "production", "risk", "culture" },
                RelatedTermIds = new List<string> { "scope-creep", "milestone", "post-mortem" }
            });

            Add(new GlossaryTermData
            {
                Id = "intellectual-property",
                Term = "Intellectual Property",
                Abbreviation = "ip",
                Category = GlossaryCategory.ProductionPublishing,
                Difficulty = DifficultyLevel.Intermediate,
                ShortDefinition = "Legal rights over creative assets, brands, code, and licenses used or created by a project.",
                SimpleExplanation = "Who owns the characters, code, music, and brand names.",
                WhyItMatters = "Clear IP ownership prevents legal blocks on shipping, sequels, and merchandising.",
                Example = "A studio confirms music licenses allow gameplay streaming before launch marketing.",
                CommonMistake = "Using unlicensed assets in a vertical slice that later cannot ship.",
                PracticePrompt = "Inventory third-party assets and mark license constraints for commercial release.",
                GameUses = new List<string> { "Contracts", "Licensed games", "Asset audits" },
                Tags = new List<string> { "legal", "ownership", "publishing" },
                Synonyms = new List<string> { "IP" },
                RelatedTermIds = new List<string> { "publisher", "localization", "app-store-guidelines" }
            });


            return terms;
        }
    }
}
