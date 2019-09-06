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
        public readonly ValueToken Key;
        public readonly ArraySyntax Array;

        public InExpression(ValueToken key, ArraySyntax array)
        {
            Key = key;
            Array = array;
        }

        public override string ToString()
        {
            return $"[In] {Key.Value} in {Array}";
        }

        public bool Test(IReadOnlyDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>> labels)
        {
            var key = labels.Keys.FirstOrDefault(labelKey => labelKey.Span.Equals(Key.Value.Span, StringComparison.Ordinal));

            if (!labels.TryGetValue(key, out var value))
            {
                return false;
            }

            return Array.Values.Any(arrayItem => value.Span.Equals(arrayItem.Value.Span, StringComparison.Ordinal));
        }
    }
    public class NotinExpression : IExpression
    {
        public readonly ValueToken Key;
        public readonly ArraySyntax Array;

        public NotinExpression(ValueToken key, ArraySyntax array)
        {
            Key = key;
            Array = array;
        }

        public override string ToString()
        {
            return $"[Notin] {Key.Value} notin {Array}";
        }

        public bool Test(IReadOnlyDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>> labels)
        {
            var key = labels.Keys.FirstOrDefault(labelKey => labelKey.Span.Equals(Key.Value.Span, StringComparison.Ordinal));

            if (!labels.TryGetValue(key, out var value))
            {
                return true;
            }

            return Array.Values.All(arrayItem => !value.Span.Equals(arrayItem.Value.Span, StringComparison.Ordinal));
        }
    }

    public class KeyExpression : IExpression
    {
        public readonly ValueToken Key;
        public KeyExpression(ValueToken key)
        {
            Key = key;
        }

        public override string ToString()
        {
            return $"[Key] {Key.Value}";
        }

        public bool Test(IReadOnlyDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>> labels)
        {
            return labels.Keys.Any(labelKey => labelKey.Span.Equals(Key.Value.Span, StringComparison.Ordinal));
        }
    }

    public class NotExpression : IExpression
    {
        public readonly IExpression InnerExpression;
        public NotExpression(IExpression innerExpression)
        {
            InnerExpression = innerExpression;
        }

        public override string ToString()
        {
            return $"[Not] !{InnerExpression}";
        }

        public bool Test(IReadOnlyDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>> labels)
        {
            return !InnerExpression.Test(labels);
        }
    }
}