using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LabelSelector.Json
{
    //interface Expression {}

    //interface InExpression extends Expression {
    //    operator: "In",
    //    key: string,
    //    values: string[],
    //}

    //interface NotInExpression extends Expression {
    //    operator: "NotIn",
    //    key: string,
    //    values: string[],
    //}

    //interface ExistsExpression extends Expression {
    //    operator: "Exists",
    //    key: string;
    //}

    //interface NotExpression extends Expression {
    //    operator: "NotExists",
    //    key: string;
    //}


    public class JsonExpression
    {
        public string Operator { get; set; }
        public string Key { get; set; }
        public string[] Values { get; set; }
    }

    public static class JsonExpressionParser
    {
        public static IEnumerable<IExpression> Parse(string jsonExpressionString)
        {
            var jsonExpression = JsonSerializer.Deserialize<JsonExpression>(
                jsonExpressionString,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });

            switch (jsonExpression.Operator.ToLowerInvariant())
            {
                case "in":
                    yield return new InExpression(
                        jsonExpression.Key.AsMemory(),
                        jsonExpression.Values.Select(value => value.AsMemory()));
                    yield break;
                case "notin":
                    yield return new NotInExpression(
                        jsonExpression.Key.AsMemory(),
                        jsonExpression.Values.Select(value => value.AsMemory()));
                    yield break;
                case "exists":
                    yield return new ExistsExpression(jsonExpression.Key.AsMemory());
                    yield break;
                case "notexists":
                    yield return new NotExistsExpression(jsonExpression.Key.AsMemory());
                    yield break;
                default:
                    throw new Exception($"Unknown Operator {jsonExpression.Operator}");
            }
        }
    }
}
