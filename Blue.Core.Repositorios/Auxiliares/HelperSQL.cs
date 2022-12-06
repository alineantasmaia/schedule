using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Blue.Core.Repositorios.Auxiliares
{
    
    public class HelperSQL : IDisposable
    {
        public ExpandoObject ParametersOracle { get; private set; }

        public static string EqualOperatorString;
        public static string EqualOperatorNumber;
        public static string InOperatorString;
        public static string InOperatorNumber;
        public static string IsMatchOperatorIn;
        public static string ReplaceValue;
        public static string GetValue;

        public HelperSQL()
        {
        }

        /// <summary>
        /// Responsável por tratar as consultas que são passadas para serem executadas na base, com isso, todas as consultas que não passarem explicitamente os parâmetros, este método
        /// irá converter para que a consulta utilize bind variable, evitando assim problemas de performance referente a hard parse
        /// </summary>
        public string TryParseOracle(string query)
        {
            ParametersOracle = new ExpandoObject();

            var regex = $"{EqualOperatorString}|{EqualOperatorNumber}|{InOperatorString}|{InOperatorNumber}";

            var groups = Regex.Matches(query, regex);

            var queryFormatted = Regex.Replace(query, regex, ":parameter");

            var lines = queryFormatted.SplitAndKeep(":parameter").ToList();
            var extractValue = $"{GetValue}";

            var parameterIndex = 0;

            for (int i = 0; i <= lines.Count - 1; i++)
            {
                if (!lines[i].Contains(":parameter"))
                    continue;

                var parameterName = $"p{parameterIndex}";

                if (Regex.IsMatch(groups[i].Value, $"{IsMatchOperatorIn}"))
                {
                    var valueIn = Regex.Matches(groups[i].Value, extractValue);
                    var expression = groups[i].Value;

                    foreach (Match v in valueIn)
                    {
                        var parameterNameIn = $"p{parameterIndex}";

                        var value = ReplaceValue.Replace("{value}", $"{v.Value}");

                        expression = Regex.Replace(expression, $@"{value}", $":{parameterNameIn}");
                        ((IDictionary<string, object>)ParametersOracle).Add(parameterNameIn, v.Value.Replace("\'", ""));

                        parameterIndex++;
                    }
                    lines[i] = lines[i].Replace(":parameter", $"{expression}");

                }
                else
                {
                    if (Regex.IsMatch(groups[i].Value, @"WHERE\s+1\s*=\s*1"))
                    {
                        lines[i] = lines[i].Replace(":parameter", $"{groups[i].Value}");
                        continue;
                    }

                    var value = Regex.Match(groups[i].Value, extractValue).Value;

                    var expression = Regex.Replace(groups[i].Value, extractValue, "{value}");
                    lines[i] = lines[i].Replace(":parameter", $"{expression.Replace("{value}", $":{parameterName}")}");

                    ((IDictionary<string, object>)ParametersOracle).Add(parameterName, value.Replace("\'", ""));
                }

                parameterIndex++;
            }

            return string.Join("", lines);
        }

        public ExpandoObject ConvertParameters(List<object> parameters)
        {
            var parametersOracle = new ExpandoObject();

            parameters.Cast<OracleParameter>().ToList().ForEach(p =>
            {
                ((IDictionary<string, object>)parametersOracle).Add(p.ParameterName, p.Value);
            });

            return parametersOracle;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    internal static class Extensions
    {
        public static IEnumerable<string> SplitAndKeep(this string s, params string[] delims)
        {
            var rows = new List<string>() { s };
            foreach (string delim in delims)
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    int index = rows[i].IndexOf(delim);
                    if (index > -1
                        && rows[i].Length > index + 1)
                    {
                        string leftPart = rows[i].Substring(0, index + delim.Length);
                        string rightPart = rows[i].Substring(index + delim.Length);
                        rows[i] = leftPart;
                        rows.Insert(i + 1, rightPart);
                    }
                }
            }

            return rows;
        }
    }
}