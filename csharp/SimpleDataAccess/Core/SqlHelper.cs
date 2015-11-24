using System.Data;

namespace SimpleDataAccess.Core
{
    public static class SqlHelper
    {
        // Execute a Command as Reader
        //public static SqlDataReader ExecuteReader(ConnectionType connType, string cmdStr, params SqlParameter[] sqlParams)
        //{
        //    SqlCommand cmd = new SqlCommand(cmdStr);
        //    if (sqlParams != null && sqlParams.Length > 0)
        //    {
        //        cmd.Parameters.AddRange(sqlParams);
        //    }
        //    return ExecuteReader(connType, cmd);
        //}
        //public static SqlDataReader ExecuteReader(ConnectionType connType, SqlCommand command)
        //{
        //    command.Connection = GetOpenConnection(connType);
        //    return ExecuteReader(command);
        //}

        public static IDataReader ExecuteReader(IDbCommand command)
        {
            try
            {
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                Cleanup(command);
                throw;
            }
            finally
            {
                // Don't Close the Reader! Do it yourself in your Code!
            }
        }

        public static void Cleanup(IDbCommand command)
        {
            if (command != null)
            {
                if (command.Connection != null && command.Connection.State == ConnectionState.Open)
                {
                    // Close and Dispose Connection
                    command.Connection.Close();
                    command.Connection.Dispose();
                }
                // Dispose Command
                command.Dispose();
            }
        }
    }
}