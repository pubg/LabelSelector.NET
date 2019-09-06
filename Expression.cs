namespace LabelSelector
{
    public class Expression { }

    public class InExpression : Expression
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
    }
    public class NotinExpression : Expression
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
    }

    public class KeyExpression : Expression
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
    }

    public class NotExpression : Expression
    {
        public readonly Expression InnerExpression;
        public NotExpression(Expression innerExpression)
        {
            InnerExpression = innerExpression;
        }

        public override string ToString()
        {
            return $"[Not] !{InnerExpression}";
        }
    }
}