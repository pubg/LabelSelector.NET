using System;
using System.Collections.Generic;
using System.Linq;

namespace LabelSelector
{
    public interface IExpression
    {
        bool Test(IReadOnlyDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>> labels);
    }

    public class InExpression : IExpression
    {
        public readonly ReadOnlyMemory<char> Key;
        public readonly IEnumerable<ReadOnlyMemory<char>> Values;

        public InExpression(ReadOnlyMemory<char> key, IEnumerable<ReadOnlyMemory<char>> values)
        {
            Key = key;
            Values = values;
        }

        public override string ToString()
        {
            return $"{Key} in {string.Join(", ", Values)}";
        }

        public bool Test(IReadOnlyDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>> labels)
        {
            var key = labels.Keys.FirstOrDefault(labelKey => labelKey.Span.Equals(Key.Span, StringComparison.Ordinal));

            if (!labels.TryGetValue(key, out var value))
            {
                return false;
            }

            return Values.Any(arrayItem => value.Span.Equals(arrayItem.Span, StringComparison.Ordinal));
        }
    }
    public class NotInExpression : IExpression
    {
        public readonly ReadOnlyMemory<char> Key;
        public readonly IEnumerable<ReadOnlyMemory<char>> Values;

        public NotInExpression(ReadOnlyMemory<char> key, IEnumerable<ReadOnlyMemory<char>> values)
        {
            Key = key;
            Values = values;
        }

        public override string ToString()
        {
            return $"[NotIn] {Key} notIn {string.Join(", ", Values)}";
        }

        public bool Test(IReadOnlyDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>> labels)
        {
            var key = labels.Keys
                .FirstOrDefault(labelKey => labelKey.Span.Equals(Key.Span, StringComparison.Ordinal));

            if (!labels.TryGetValue(key, out var value))
            {
                return true;
            }

            return Values
                .All(arrayItem => !value.Span.Equals(arrayItem.Span, StringComparison.Ordinal));
        }
    }

    public class ExistsExpression : IExpression
    {
        public readonly ReadOnlyMemory<char> Key;
        public ExistsExpression(ReadOnlyMemory<char> key)
        {
            Key = key;
        }

        public override string ToString()
        {
            return $"[Key] {Key}";
        }

        public bool Test(IReadOnlyDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>> labels)
        {
            return labels.Keys
                .Any(labelKey => labelKey.Span.Equals(Key.Span, StringComparison.Ordinal));
        }
    }

    public class NotExistsExpression : IExpression
    {
        public readonly ReadOnlyMemory<char> Key;
        public NotExistsExpression(ReadOnlyMemory<char> key)
        {
            Key = key;
        }

        public override string ToString()
        {
            return $"[NotExists] !{Key}";
        }

        public bool Test(IReadOnlyDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>> labels)
        {
            return labels.Keys
                .All(labelKey => !labelKey.Span.Equals(Key.Span, StringComparison.Ordinal));
        }
    }
}