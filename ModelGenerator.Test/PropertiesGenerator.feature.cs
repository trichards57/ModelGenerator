﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace ModelGenerator.Test
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class GeneratingMultipleCModelPropertiesFeature : Xunit.IClassFixture<GeneratingMultipleCModelPropertiesFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "PropertiesGenerator.feature"
#line hidden
        
        public GeneratingMultipleCModelPropertiesFeature()
        {
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Generating multiple C# model properties", "\tMultiple properties should be generated based on the provided file", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void SetFixture(GeneratingMultipleCModelPropertiesFeature.FixtureData fixtureData)
        {
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="The model should get all properties")]
        [Xunit.TraitAttribute("FeatureTitle", "Generating multiple C# model properties")]
        [Xunit.TraitAttribute("Description", "The model should get all properties")]
        public virtual void TheModelShouldGetAllProperties()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("The model should get all properties", ((string[])(null)));
#line 4
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Type",
                        "IncludeInCreate",
                        "IncludeInDetail",
                        "IncludeInUpdate",
                        "IncludeInSummary"});
            table1.AddRow(new string[] {
                        "N1",
                        "T1",
                        "false",
                        "false",
                        "false",
                        "false"});
            table1.AddRow(new string[] {
                        "N2",
                        "T2",
                        "true",
                        "false",
                        "false",
                        "false"});
            table1.AddRow(new string[] {
                        "N3",
                        "T3",
                        "false",
                        "true",
                        "false",
                        "false"});
            table1.AddRow(new string[] {
                        "N4",
                        "T4",
                        "false",
                        "false",
                        "true",
                        "false"});
            table1.AddRow(new string[] {
                        "N5",
                        "T5",
                        "false",
                        "false",
                        "false",
                        "true"});
            table1.AddRow(new string[] {
                        "N6",
                        "T6",
                        "false",
                        "true",
                        "false",
                        "false"});
#line 5
 testRunner.Given("there are a group of properties", ((string)(null)), table1, "Given ");
#line 13
 testRunner.And("the generator is in Model mode", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
 testRunner.When("you create a set of properties", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 15
 testRunner.Then("the output properties should be N1, N2, N3, N4, N5, N6", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="The create view model should only get IncludeInCreate properties")]
        [Xunit.TraitAttribute("FeatureTitle", "Generating multiple C# model properties")]
        [Xunit.TraitAttribute("Description", "The create view model should only get IncludeInCreate properties")]
        public virtual void TheCreateViewModelShouldOnlyGetIncludeInCreateProperties()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("The create view model should only get IncludeInCreate properties", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Type",
                        "IncludeInCreate",
                        "IncludeInDetail",
                        "IncludeInUpdate",
                        "IncludeInSummary"});
            table2.AddRow(new string[] {
                        "N1",
                        "T1",
                        "false",
                        "false",
                        "false",
                        "false"});
            table2.AddRow(new string[] {
                        "N2",
                        "T2",
                        "true",
                        "false",
                        "false",
                        "false"});
            table2.AddRow(new string[] {
                        "N3",
                        "T3",
                        "false",
                        "true",
                        "false",
                        "false"});
            table2.AddRow(new string[] {
                        "N4",
                        "T4",
                        "false",
                        "false",
                        "true",
                        "false"});
            table2.AddRow(new string[] {
                        "N5",
                        "T5",
                        "false",
                        "false",
                        "false",
                        "true"});
            table2.AddRow(new string[] {
                        "N6",
                        "T6",
                        "false",
                        "true",
                        "false",
                        "false"});
#line 18
 testRunner.Given("there are a group of properties", ((string)(null)), table2, "Given ");
#line 26
 testRunner.And("the generator is in Create mode", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.When("you create a set of properties", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
 testRunner.Then("the output properties should be N2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="The detail view model should only get IncludeInDetail properties")]
        [Xunit.TraitAttribute("FeatureTitle", "Generating multiple C# model properties")]
        [Xunit.TraitAttribute("Description", "The detail view model should only get IncludeInDetail properties")]
        public virtual void TheDetailViewModelShouldOnlyGetIncludeInDetailProperties()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("The detail view model should only get IncludeInDetail properties", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Type",
                        "IncludeInCreate",
                        "IncludeInDetail",
                        "IncludeInUpdate",
                        "IncludeInSummary"});
            table3.AddRow(new string[] {
                        "N1",
                        "T1",
                        "false",
                        "false",
                        "false",
                        "false"});
            table3.AddRow(new string[] {
                        "N2",
                        "T2",
                        "true",
                        "false",
                        "false",
                        "false"});
            table3.AddRow(new string[] {
                        "N3",
                        "T3",
                        "false",
                        "true",
                        "false",
                        "false"});
            table3.AddRow(new string[] {
                        "N4",
                        "T4",
                        "false",
                        "false",
                        "true",
                        "false"});
            table3.AddRow(new string[] {
                        "N5",
                        "T5",
                        "false",
                        "false",
                        "false",
                        "true"});
            table3.AddRow(new string[] {
                        "N6",
                        "T6",
                        "false",
                        "true",
                        "false",
                        "false"});
#line 31
 testRunner.Given("there are a group of properties", ((string)(null)), table3, "Given ");
#line 39
 testRunner.And("the generator is in Details mode", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
 testRunner.When("you create a set of properties", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 41
 testRunner.Then("the output properties should be N3, N6", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="The summary view model should only get IncludeInSummary properties")]
        [Xunit.TraitAttribute("FeatureTitle", "Generating multiple C# model properties")]
        [Xunit.TraitAttribute("Description", "The summary view model should only get IncludeInSummary properties")]
        public virtual void TheSummaryViewModelShouldOnlyGetIncludeInSummaryProperties()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("The summary view model should only get IncludeInSummary properties", ((string[])(null)));
#line 43
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Type",
                        "IncludeInCreate",
                        "IncludeInDetail",
                        "IncludeInUpdate",
                        "IncludeInSummary"});
            table4.AddRow(new string[] {
                        "N1",
                        "T1",
                        "false",
                        "false",
                        "false",
                        "false"});
            table4.AddRow(new string[] {
                        "N2",
                        "T2",
                        "true",
                        "false",
                        "false",
                        "false"});
            table4.AddRow(new string[] {
                        "N3",
                        "T3",
                        "false",
                        "true",
                        "false",
                        "false"});
            table4.AddRow(new string[] {
                        "N4",
                        "T4",
                        "false",
                        "false",
                        "true",
                        "false"});
            table4.AddRow(new string[] {
                        "N5",
                        "T5",
                        "false",
                        "false",
                        "false",
                        "true"});
            table4.AddRow(new string[] {
                        "N6",
                        "T6",
                        "false",
                        "true",
                        "false",
                        "false"});
#line 44
 testRunner.Given("there are a group of properties", ((string)(null)), table4, "Given ");
#line 52
 testRunner.And("the generator is in Summary mode", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
 testRunner.When("you create a set of properties", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 54
 testRunner.Then("the output properties should be N5", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="The update view model should only get IncludeInUpdate properties")]
        [Xunit.TraitAttribute("FeatureTitle", "Generating multiple C# model properties")]
        [Xunit.TraitAttribute("Description", "The update view model should only get IncludeInUpdate properties")]
        public virtual void TheUpdateViewModelShouldOnlyGetIncludeInUpdateProperties()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("The update view model should only get IncludeInUpdate properties", ((string[])(null)));
#line 56
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Type",
                        "IncludeInCreate",
                        "IncludeInDetail",
                        "IncludeInUpdate",
                        "IncludeInSummary"});
            table5.AddRow(new string[] {
                        "N1",
                        "T1",
                        "false",
                        "false",
                        "false",
                        "false"});
            table5.AddRow(new string[] {
                        "N2",
                        "T2",
                        "true",
                        "false",
                        "false",
                        "false"});
            table5.AddRow(new string[] {
                        "N3",
                        "T3",
                        "false",
                        "true",
                        "false",
                        "false"});
            table5.AddRow(new string[] {
                        "N4",
                        "T4",
                        "false",
                        "false",
                        "true",
                        "false"});
            table5.AddRow(new string[] {
                        "N5",
                        "T5",
                        "false",
                        "false",
                        "false",
                        "true"});
            table5.AddRow(new string[] {
                        "N6",
                        "T6",
                        "false",
                        "true",
                        "false",
                        "false"});
#line 57
 testRunner.Given("there are a group of properties", ((string)(null)), table5, "Given ");
#line 65
 testRunner.And("the generator is in Update mode", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
 testRunner.When("you create a set of properties", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 67
 testRunner.Then("the output properties should be N4", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                GeneratingMultipleCModelPropertiesFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                GeneratingMultipleCModelPropertiesFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
