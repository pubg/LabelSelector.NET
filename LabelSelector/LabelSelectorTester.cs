using System;
using System.Collections.Generic;
using System.Linq;

namespace LabelSelector
{
    public static class LabelSelectorTester
    {
        public static bool Test(IReadOnlyDictionary<string, string> labels, IEnumerable<IExpression> expressions)
        {
            var labelsInMemory = labels
                .Select(pair => (pair.Key.AsMemory(), pair.Value.AsMemory()))
                .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

            return expressions.All(expression => expression.Test(labelsInMemory));
        }

        public static bool Test(IReadOnlyDictionary<string, string> labels, string labelSelector)
        {
            var expressions = Parser.Parse(labelSelector);
            return Test(labels, expressions);
        }
    }
}
