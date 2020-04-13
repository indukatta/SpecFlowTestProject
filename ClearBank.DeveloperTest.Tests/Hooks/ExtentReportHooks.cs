using AventStack.ExtentReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;

namespace ClearBank.DeveloperTest.Tests
{
    [Binding]
    public sealed class ExtentReportHooks
    {
        public readonly ScenarioContext scenarioContext;
       // public readonly FeatureContext featureContext;

        ExtentReportHooks(ScenarioContext context)
        {
            //featureContext = fcontext;
            scenarioContext = context;
        }

        private static ExtentTest featureName;
        private static ExtentTest scenario;
        private static ExtentReports extent;
        public static string ReportPath;
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            string path1 = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", "");
            string path = path1 + "\\Report\\index.html";
            ExtentHtmlReporter htmlReporter = new ExtentHtmlReporter(path);
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            //Create dynamic feature name
            featureName = extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
            Console.WriteLine("BeforeFeature");
        }
        [BeforeScenario]
        public void BeforeScenario()
        {
            Console.WriteLine("BeforeScenario");
            scenario = featureName.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }
        [AfterStep]
        public void InsertReportingSteps()
        {
            var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
            if (scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
                else if(stepType == "When")
                                scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text);
                else if(stepType == "Then")
                                scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text);
                else if(stepType == "And")
                                scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text);
            }
            else if(scenarioContext.TestError != null)
            {
                if (stepType == "Given")
                {
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                }
                else if(stepType == "When")
                {
                    scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                }
                else if(stepType == "Then") {
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                }
                else if(stepType == "And")
                {
                    scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text).Fail(scenarioContext.TestError.Message);
                }
            }
        }
        [AfterScenario]
        public void AfterScenario()
        {
            Console.WriteLine("AfterScenario");
            //implement logic that has to run after executing each scenario
        }
        [AfterTestRun]
        public static void AfterTestRun()
        {
            //Flush report once test completes
            extent.Flush();
        }

    }
}
