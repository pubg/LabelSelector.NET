﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabelSelector
{
    public static class LabelSelectorTester
    {
        public static bool Test(IReadOnlyDictionary<string, string> labels, string labelSelector)
        {
            var expression = Parser.Parse(labelSelector);

            var labelsInMemory = labels
                .Select(pair => (pair.Key.AsMemory(), pair.Value.AsMemory()))
                .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

            return expression.Test(labelsInMemory);
        }
    }
}