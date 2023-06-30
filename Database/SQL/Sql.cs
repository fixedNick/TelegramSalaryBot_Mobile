using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using MySqlConnector;

using TelegramSalaryBot.Client;
using TelegramSalaryBot.Exceptions;

namespace TelegramSalaryBot.Database.SQL;

public static class Sql
{
    private static DatabaseInfo Info = new DatabaseInfo("localhost", "root", "", DBProps.DatabaseName);
    private static bool IsInitialized = false;
    public static void Initialize(DatabaseInfo info)
    {
        Info = info;
        IsInitialized = true;
    }
    public static async Task<bool> IsClientExist(long tid)
        => await Task.FromResult(await GetClient(tid) is not default(TelegramClient));
    public static async Task<TelegramClient> GetClient(long tid)
    {
        using var connection = await OpenConnection();

        using var getClientCommand = connection.CreateCommand();
        getClientCommand.CommandText = $"SELECT * FROM `{DBProps.ClientsTable}` WHERE `{DBProps.TelegramId}` = '@telegramIdValue'";
        getClientCommand.Parameters.AddWithValue("@telegramIdValue", tid);
        using var reader = await getClientCommand.ExecuteReaderAsync();

        if (reader.HasRows == false) throw new ClientNotFoundException("Client was not found in database");

        var client = default(TelegramClient);
        while (reader.Read())
        {
            var localId = reader.GetInt64(DBProps.LocalId);
            var clientId = reader.GetInt64(DBProps.TelegramId);
            var domain = reader.GetString(DBProps.TelegramDomain);
            var firstName = reader.GetString(DBProps.TelegramFistName);
            var lastName = reader.GetString(DBProps.TelegramLastName);

            client = new TelegramClient(clientId, localId, domain, firstName, lastName);
        }

        await connection.CloseAsync();
        return await Task.FromResult(client ?? throw new ClientEmptyResultException());
    }
    public static async Task<List<TelegramClient>> GetClients()
    {
        using var connection = await OpenConnection();

        using var getClientsCommand = connection.CreateCommand();
        getClientsCommand.CommandText = $"SELECT * FROM `{DBProps.ClientsTable}`";
        using var reader = await getClientsCommand.ExecuteReaderAsync();

        var clients = new List<TelegramClient>();
        while(reader.Read())
        {
            var localId = reader.GetInt64(DBProps.LocalId);
            var telegramId = reader.GetInt64(DBProps.TelegramId);
            var domain = reader.GetString(DBProps.TelegramDomain);
            var firstName = reader.GetString(DBProps.TelegramFistName);
            var lastName = reader.GetString(DBProps.TelegramLastName);
            clients.Add(new TelegramClient(telegramId, localId, domain, firstName, lastName));
        }

        await connection.CloseAsync();
        return await Task.FromResult(clients);
    }
    private static async Task<MySqlConnection> OpenConnection()
    {
        if (IsInitialized == false)
            throw new SqlNotInitializedException("Initialize sql first");
        var builder = new MySqlConnectionStringBuilder()
        {
            Server = Info.Host,
            UserID = Info.User,
            Password = Info.Password,
            Database = Info.Db
        };
        var connection = new MySqlConnection(builder.ConnectionString);
        await connection.OpenAsync();
        return await Task.FromResult(connection);
    }
    public static async Task SaveClient(TelegramClient client)
    {
        using var connection = await OpenConnection();

        using var saveClientCommand = connection.CreateCommand();
        saveClientCommand.CommandText = $"INSERT INTO `{DBProps.ClientsTable}` " +
            $"(`{DBProps.TelegramId}`,`{DBProps.TelegramDomain}`,`{DBProps.TelegramFistName}`, `{DBProps.TelegramLastName}`) " +
            $"VALUES (@tid, @domainVal,@fistNameVal,@lastNameVal)";

        saveClientCommand.Parameters.AddWithValue("@tid", client.TelegramID);
        saveClientCommand.Parameters.AddWithValue("@domainVal", client.UserName);
        saveClientCommand.Parameters.AddWithValue("@fistNameVal", client.FirstName);
        saveClientCommand.Parameters.AddWithValue("@lastNameVal", client.LastName);

        await saveClientCommand.ExecuteNonQueryAsync();
        
        await connection.CloseAsync();
    }
    public static async Task<long> GetNextLocalID()
    { 
        using var connection = await OpenConnection();

        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT MAX(ID) FROM `{DBProps.ClientsTable}`";
        var result = await command.ExecuteScalarAsync();

        await connection.CloseAsync();
        return await Task.FromResult(result == DBNull.Value ? 1 : ( Convert.ToInt64(result) + 1) );
    }
}
