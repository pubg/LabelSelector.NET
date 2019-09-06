using System;
using System.Collections.Generic;
using System.Linq;
using LabelSelector;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LabelSelectorTest
{
    [TestClass]
    public class LabelSelectorTester_Test
    {
        [TestMethod]
        public void TestInExpression_FirstItemMatch()
        {
            var labelSelector = "environment in (production, qa)";

            var labels = new Dictionary<string, string>
            {
                ["environment"] = "production",
            };

            var result = LabelSelectorTester.Test(labels, labelSelector);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestInExpression_SecondItemMatch()
        {
            var labelSelector = "environment in (production, qa)";

            var labels = new Dictionary<string, string>
            {
                ["environment"] = "qa",
            };

            var result = LabelSelectorTester.Test(labels, labelSelector);
            Assert.IsTrue(result);
        }

        private static string GenerateRandomString() => Guid.NewGuid().ToString().Substring(0, 6);

        [TestMethod]
        public void TestInExpression()
        {
            const int n = 10;

            var randomValues = new List<string>();

            for (var i = 0; i < n; i += 1)
            {
                var randomString = GenerateRandomString();
                randomValues.Add(randomString);
            }
            
            var labelSelector = $"environment in ({string.Join(", ", randomValues)})";

            foreach (var randomValue in randomValues)
            {
                var labels = new Dictionary<string, string>
                {
                    ["environment"] = randomValue,
                };

                var result = LabelSelectorTester.Test(labels, labelSelector);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestInExpression_False()
        {
            const int n = 10;

            var randomValues = new List<string>();

            for (var i = 0; i < n; i += 1)
            {
                var randomString = GenerateRandomString();
                randomValues.Add(randomString);
            }

            foreach (var randomValue in randomValues)
            {
                var labelSelector = $"environment in ({string.Join(", ", randomValues.Where(value => value != randomValue))})";

                var labels = new Dictionary<string, string>
                {
                    ["environment"] = randomValue,
                };

                var result = LabelSelectorTester.Test(labels, labelSelector);
                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public void TestNotinExpression_False()
        {
            const int n = 10;

            var randomValues = new List<string>();

            for (var i = 0; i < n; i += 1)
            {
                var randomString = GenerateRandomString();
                randomValues.Add(randomString);
            }

            var labelSelector = $"environment notin ({string.Join(", ", randomValues)})";

            foreach (var randomValue in randomValues)
            {
                var labels = new Dictionary<string, string>
                {
                    ["environment"] = randomValue,
                };

                var result = LabelSelectorTester.Test(labels, labelSelector);
                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public void TestNotinExpression()
        {
            const int n = 10;

            var randomValues = new List<string>();

            for (var i = 0; i < n; i += 1)
            {
                var randomString = GenerateRandomString();
                randomValues.Add(randomString);
            }

            foreach (var randomValue in randomValues)
            {
                var labelSelector = $"environment notin ({string.Join(", ", randomValues.Where(value => value != randomValue))})";

                var labels = new Dictionary<string, string>
                {
                    ["environment"] = randomValue,
                };

                var result = LabelSelectorTester.Test(labels, labelSelector);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void TestNotinExpression_NoKey()
        {
            const int n = 10;

            var randomValues = new List<string>();

            for (var i = 0; i < n; i += 1)
            {
                var randomString = GenerateRandomString();
                randomValues.Add(randomString);
            }

            var labelSelector = $"environment notin ({string.Join(", ", randomValues)})";

            var labels = new Dictionary<string, string>
            {
            };

            var result = LabelSelectorTester.Test(labels, labelSelector);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestExistsExpression()
        {
            var labelSelector = $"environment";

            var labels = new Dictionary<string, string>
            {
                ["environment"] = GenerateRandomString(),
            };

            var result = LabelSelectorTester.Test(labels, labelSelector);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestExistsExpression_False()
        {
            var labelSelector = $"environment";

            var labels = new Dictionary<string, string>
            {
                [GenerateRandomString()] = GenerateRandomString(),
            };

            var result = LabelSelectorTester.Test(labels, labelSelector);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestNotExistsExpression_Exists()
        {
            var labelSelector = $"!environment";

            var labels = new Dictionary<string, string>
            {
                [GenerateRandomString()] = GenerateRandomString(),
            };

            var result = LabelSelectorTester.Test(labels, labelSelector);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestComma_False()
        {
            const int n = 10;

            var randomValues = new List<string>();

            for (var i = 0; i < n; i += 1)
            {
                var randomString = GenerateRandomString();
                randomValues.Add(randomString);
            }

            var labelSelector = $"environment, environment notin ({string.Join(", ", randomValues)})";

            var labels = new Dictionary<string, string>
            {
            };

            var result = LabelSelectorTester.Test(labels, labelSelector);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestComma_TwoExpression()
        {
            const int n = 10;

            var randomValues = new List<string>();

            for (var i = 0; i < n; i += 1)
            {
                var randomString = GenerateRandomString();
                randomValues.Add(randomString);
            }

            var labelSelector = $"environment, environment notin ({string.Join(", ", randomValues)})";

            var labels = new Dictionary<string, string>
            {
                ["environment"] = GenerateRandomString(),
            };

            var result = LabelSelectorTester.Test(labels, labelSelector);
            Assert.IsTrue(result);
        }
    }
}
