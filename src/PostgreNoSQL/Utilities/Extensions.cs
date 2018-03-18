namespace PostgreNoSQL
{
    using System;
    using System.Reflection;
    using Npgsql;

    internal  static class Extensions
    {
        public static bool IsStatic(this PropertyInfo property) => (property.GetMethod ?? property.SetMethod).IsStatic;

        public static NpgsqlParameter AddNamedParameter(this NpgsqlCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);

            return parameter;
        }
    }
}
