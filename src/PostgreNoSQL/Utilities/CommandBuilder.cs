namespace PostgreNoSQL
{
    using System.Collections.Generic;
    using System.Text;
    using Npgsql;

    public class CommandBuilder
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public string Sql => _stringBuilder.ToString();

        public CommandBuilder Append(string value)
        {
            _stringBuilder.Append(value);
            return this;
        }

        public NpgsqlCommand Build()
        {
            var cmd = new NpgsqlCommand
            {
                CommandText = Sql
            };

            foreach (var param in _parameters)
            {
                cmd.AddNamedParameter(param.Key, param.Value);
            }

            return cmd;
        }

        public string AddParameter(object value)
        {
            string name = "p" + (_parameters.Count + 1);
            _parameters.Add(name, value);
            return name;
        }
    }
}
