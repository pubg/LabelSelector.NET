using System;
using System.Linq;
using System.Text;

namespace LabelSelector
{
    class Program
    {
        static void Main(string[] args)
        {
            var testTextes = new[] {
                "environment in (production, qa)",
                "tier notin (frontend, backend)",
                "partition",
                "!partition",
            };

            foreach (var text in testTextes)
            {
                Console.WriteLine($@"""{text}""");
                var tokens = Tokenizer.Tokenize(text).ToArray();
                foreach (var token in tokens)
                {
                    Console.WriteLine(token);
                }

                Console.WriteLine();

                var expression = Parser.Parse(tokens);

                Console.WriteLine(expression);

                Console.WriteLine();
            }
        }
    }
}
