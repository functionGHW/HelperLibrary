using System;
using System.Collections.Generic;
using System.Data;

namespace HelperLibrary.Core.Db
{
    public static class ExtensionMethods
    {
        public static void AddParameters(this IDbCommand command,
            IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            foreach (var param in parameters)
            {
                AddParameter(command, param.Key, param.Value);
            }
        }

        public static void AddParameter(this IDbCommand command, string paramName, object value)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var param = command.CreateParameter();
            param.ParameterName = paramName;
            param.Value = value;
            command.Parameters.Add(param);
        }

        public static void UpdateParameters(this IDbCommand command,
            IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            foreach (var param in parameters)
            {
                UpdateParameter(command, param.Key, param.Value);
            }
        }

        public static void UpdateParameter(this IDbCommand command, string paramName, object value)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            command.Parameters[paramName] = value;
        }

        public static DataSet ExecuteDataSet(this IDbCommand command, IDbDataAdapter adapter)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (adapter == null)
                throw new ArgumentNullException(nameof(adapter));

            var dataSet = new DataSet();
            adapter.SelectCommand = command;
            adapter.Fill(dataSet);
            return dataSet;
        }

        public static IDbCommand CreateCommand(this IDbConnection connection, string sql, CommandType cmdType = CommandType.Text,
            IDictionary<string, object> parameters = null)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = cmdType;

            if (parameters != null)
            {
                cmd.AddParameters((parameters));
            }
            return cmd;
        }
    }
}
