using System.Reflection;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Linq;

namespace MicroServicoUser.Inf.Persistence.Database;

public static class DbParameterHelper
{
<<<<<<< HEAD
    private static readonly Regex ParameterRegex = new(
        @"@\w+",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100)
    );

    public static MySqlCommand PopulateCommandParameters<T>(string query, T model) where T : class
    {
        List<string> parameters;

        try
        {
            parameters = ParameterRegex.Matches(query)
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct()
                .ToList();
        }
        catch (RegexMatchTimeoutException)
        {
            throw new InvalidOperationException("La extracción de parámetros de la consulta excedió el tiempo permitido.");
        }
=======
    public static MySqlCommand PopulateCommandParameters<T>(string query, T model)
    {
        var parameters = Regex.Matches(query, @"@\w+")
            .Cast<Match>()
            .Select(m => m.Value)
            .Distinct()
            .ToList();
>>>>>>> AnalisisSonarEstablishment

        MySqlCommand command = new(query);

        if (model == null)
            return command;

        PropertyInfo[] modelProperties = typeof(T).GetProperties();

        foreach (string paramName in parameters)
        {
            string modelPropName = paramName[1..];
            var property = modelProperties.FirstOrDefault(p => p.Name == modelPropName);

            if (property != null)
            {
                object value = property.GetValue(model) ?? DBNull.Value;
                command.Parameters.Add(new MySqlParameter(paramName, value));
            }
<<<<<<< HEAD
=======
            else
            {
            }
>>>>>>> AnalisisSonarEstablishment
        }

        return command;
    }
}