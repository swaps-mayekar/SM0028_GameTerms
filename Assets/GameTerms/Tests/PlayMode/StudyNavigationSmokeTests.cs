using System.Collections;
using GameTerms.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace GameTerms.Tests
{
    public class StudyNavigationSmokeTests
    {
        [UnityTest]
        public IEnumerator StudyHub_StartsQuizAndReachesResults()
        {
            var load = SceneManager.LoadSceneAsync("1_MainScene", LoadSceneMode.Single);
            while (load != null && !load.isDone)
            {
                yield return null;
            }

            yield return null;
            yield return null;

            var shell = Object.FindFirstObjectByType<AppShellController>();
            Assert.That(shell, Is.Not.Null, "AppShellController should exist in the main scene.");

            var navigatorField = typeof(AppShellController).GetField("navigator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var servicesField = typeof(AppShellController).GetField("services", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.That(navigatorField, Is.Not.Null);
            Assert.That(servicesField, Is.Not.Null);

            var navigator = (AppNavigator)navigatorField.GetValue(shell);
            var services = (AppServices)servicesField.GetValue(shell);
            Assert.That(navigator, Is.Not.Null);
            Assert.That(services, Is.Not.Null);

            navigator.ShowStudyHub();
            yield return null;
            Assert.That(navigator.CurrentScreen, Is.EqualTo(AppScreen.StudyHub));

            navigator.StartQuiz(new StudyConfig
            {
                Mode = StudyMode.Quiz,
                QuestionCount = 1,
                Scope = StudyScope.All
            });
            yield return null;
            Assert.That(navigator.CurrentScreen, Is.EqualTo(AppScreen.QuizSession));
            Assert.That(services.Quiz.HasActiveSession, Is.True);

            var question = services.Quiz.GetCurrentQuestion();
            Assert.That(question, Is.Not.Null);
            services.Quiz.SubmitAnswer(question.CorrectOptionId);
            services.Quiz.Advance();
            navigator.ShowQuizResults();
            yield return null;

            Assert.That(navigator.CurrentScreen, Is.EqualTo(AppScreen.QuizResults));
            Assert.That(services.Quiz.GetResults().CorrectCount, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator StudyHub_OpensLearningPathDetail()
        {
            var load = SceneManager.LoadSceneAsync("1_MainScene", LoadSceneMode.Single);
            while (load != null && !load.isDone)
            {
                yield return null;
            }

            yield return null;
            yield return null;

            var shell = Object.FindFirstObjectByType<AppShellController>();
            Assert.That(shell, Is.Not.Null);

            var navigatorField = typeof(AppShellController).GetField("navigator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var servicesField = typeof(AppShellController).GetField("services", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var navigator = (AppNavigator)navigatorField.GetValue(shell);
            var services = (AppServices)servicesField.GetValue(shell);

            navigator.ShowStudyHub();
            yield return null;
            Assert.That(services.LearningPaths.GetPaths().Count, Is.EqualTo(4));

            navigator.ShowPath("beginner");
            yield return null;
            Assert.That(navigator.CurrentScreen, Is.EqualTo(AppScreen.PathDetail));
            Assert.That(navigator.SelectedPathId, Is.EqualTo("beginner"));

            var firstLesson = services.LearningPaths.GetPath("beginner").Steps[0];
            navigator.ShowTermFromPath("beginner", firstLesson.Id, firstLesson.TermId);
            yield return null;
            Assert.That(navigator.CurrentScreen, Is.EqualTo(AppScreen.TermDetail));
            Assert.That(navigator.SelectedPathId, Is.EqualTo("beginner"));
        }
    }
}
