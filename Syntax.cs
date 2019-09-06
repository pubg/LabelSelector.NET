using System.Collections.Generic;

namespace LabelSelector
{
    public class Syntax
    {
    }

    public class ArraySyntax : Syntax
    {
        public readonly IEnumerable<ValueToken> Values;

        public ArraySyntax(IEnumerable<ValueToken> values)
        {
            Values = values;
        }

        public override string ToString()
        {
            return $"[Array] ({string.Join(", ", Values)})";
        }
    }
}