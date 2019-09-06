using System;
using System.Collections.Generic;
using System.Linq;
using LabelSelector;
using LabelSelector.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LabelSelectorTest
{
    [TestClass]
    public class LabelSelectorTester_Json
    {
        [TestMethod]
        public void TestJsonInExpression()
        {
            const int n = 10;

            var randomValues = new List<string>();

            for (var i = 0; i < n; i += 1)
            {
                var randomString = LabelSelectorTester_Test.GenerateRandomString();
                randomValues.Add(randomString);
            }

            var jsonExpressionString = @"
{
    ""key"": ""environment"",
    ""operator"":""In"",
    ""values"": [
        " + string.Join(",\n", randomValues.Select(value => $@"""{value}""")) + @"
    ]
}";
            var expressions = JsonExpressionParser.Parse(jsonExpressionString);

            bool result;
            Dictionary<string, string> labels;

            foreach (var randomValue in randomValues)
            {
                labels = new Dictionary<string, string>
                {
                    ["environment"] = randomValue,
                };

                result = LabelSelectorTester.Test(labels, expressions);
                Assert.IsTrue(result);
            }

            labels = new Dictionary<string, string>
            {
                ["environment"] = LabelSelectorTester_Test.GenerateRandomString(),
            };

            result = LabelSelectorTester.Test(labels, expressions);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestJsonNotInExpression()
        {
            const int n = 10;

            var randomValues = new List<string>();

            for (var i = 0; i < n; i += 1)
            {
                var randomString = LabelSelectorTester_Test.GenerateRandomString();
                randomValues.Add(randomString);
            }

            var jsonExpressionString = @"
{
    ""key"": ""environment"",
    ""operator"":""NotIn"",
    ""values"": [
        " + string.Join(",\n", randomValues.Select(value => $@"""{value}""")) + @"
    ]
}";
            var expressions = JsonExpressionParser.Parse(jsonExpressionString);

            bool result;
            Dictionary<string, string> labels;

            foreach (var randomValue in randomValues)
            {
                labels = new Dictionary<string, string>
                {
                    ["environment"] = randomValue,
                };

                result = LabelSelectorTester.Test(labels, expressions);
                Assert.IsFalse(result);
            }

            labels = new Dictionary<string, string>
            {
                ["environment"] = LabelSelectorTester_Test.GenerateRandomString(),
            };

            result = LabelSelectorTester.Test(labels, expressions);
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void TestJsonExistsExpression()
        {
            var jsonExpressionString = @"
{
    ""key"": ""environment"",
    ""operator"":""Exists""
}";
            var expressions = JsonExpressionParser.Parse(jsonExpressionString);

            var labels = new Dictionary<string, string>
            {
                ["environment"] = LabelSelectorTester_Test.GenerateRandomString(),
            };

            var result = LabelSelectorTester.Test(labels, expressions);
            Assert.IsTrue(result);

            labels = new Dictionary<string, string>
            {
            };

            result = LabelSelectorTester.Test(labels, expressions);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestJsonNotExistsExpression()
        {
            var jsonExpressionString = @"
{
    ""key"": ""environment"",
    ""operator"":""NotExists""
}";
            var expressions = JsonExpressionParser.Parse(jsonExpressionString);

            var labels = new Dictionary<string, string>
            {
                ["environment"] = LabelSelectorTester_Test.GenerateRandomString(),
            };

            var result = LabelSelectorTester.Test(labels, expressions);
            Assert.IsFalse(result);

            labels = new Dictionary<string, string>
            {
            };

            result = LabelSelectorTester.Test(labels, expressions);
            Assert.IsTrue(result);
        }
    }
}
