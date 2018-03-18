namespace PostgreNoSQL.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Npgsql;

    public class CommandBuilder
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public CommandBuilder Append(string value)
        {
            _stringBuilder.Append(value);
            return this;
        }

        public NpgsqlCommand Build()
        {
            string sql = _stringBuilder.ToString();
            return null;
        }

        public string AddParameter(object value)
        {
            string name = "p" + (_parameters.Count + 1);
            _parameters.Add(name, value);
            return name;
        }
    }
}
