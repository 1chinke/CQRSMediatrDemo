using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Demo.Repository;

public class ConnectionManager
{
    private readonly string connString = "Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = teknosrv)(PORT = 1521))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = TEK1)));" +
                                          "User Id = ypyi; Password = ypyi; " +
           "Min Pool Size=2; Connection Lifetime=100000;Connection Timeout=60; Incr Pool Size = 5; Decr Pool Size = 2;";

    private readonly IDbConnection _conn;

    private ConnectionManager()
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        _conn = new OracleConnection(connString);

    }

    private static readonly ConnectionManager _instance = new();


    public static ConnectionManager GetInstance() => _instance;

    public IDbConnection GetConnection()
    {
        if (_conn.State == ConnectionState.Closed || _conn.State == ConnectionState.Broken)
        {
            _conn.Open();
        }

        return _conn;

    }
}
