using System;
using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace GameTerms.Editor
{
    public static class GameTermsTestRunnerBatch
    {
        public static void RunEditModeTests()
        {
            RunTests(TestMode.EditMode, "GameTerms.Tests.EditMode", "TestResults-EditMode.xml");
        }

        public static void RunPlayModeTests()
        {
            RunTests(TestMode.EditMode, "GameTerms.Tests.PlayMode", "TestResults-PlayMode.xml");
        }

        private static void RunTests(TestMode mode, string assemblyName, string resultsFileName)
        {
            var api = ScriptableObject.CreateInstance<TestRunnerApi>();
            var callback = new BatchTestCallback(resultsFileName);
            api.RegisterCallbacks(callback);

            var settings = new ExecutionSettings(new Filter
            {
                testMode = mode,
                assemblyNames = new[] { assemblyName }
            });

            api.Execute(settings);
        }

        private sealed class BatchTestCallback : ICallbacks
        {
            private readonly string resultsFileName;
            private bool finished;

            public BatchTestCallback(string resultsFileName)
            {
                this.resultsFileName = resultsFileName;
            }

            public void RunStarted(ITestAdaptor testsToRun)
            {
                Debug.Log($"Starting {testsToRun.TestCaseCount} tests.");
            }

            public void RunFinished(ITestResultAdaptor result)
            {
                finished = true;
                var path = System.IO.Path.Combine(Application.dataPath, "..", resultsFileName);
                Debug.Log($"Tests finished. Passed: {result.PassCount}, Failed: {result.FailCount}, Skipped: {result.SkipCount}. Results: {path}");
                EditorApplication.Exit(result.FailCount > 0 ? 1 : 0);
            }

            public void TestStarted(ITestAdaptor test) { }

            public void TestFinished(ITestResultAdaptor result)
            {
                if (!result.HasChildren && !result.Test.IsSuite)
                {
                    Debug.Log($"{result.Test.Name}: {result.TestStatus}");
                }
            }

            public void RunStarted(ITestAdaptor testsToRun, ExecutionSettings settings) => RunStarted(testsToRun);

            public void RunFinished(ITestResultAdaptor result, ExecutionSettings settings) => RunFinished(result);
        }
    }
}
