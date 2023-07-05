using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using MySqlConnector;

using TelegramSalaryBot.Client;
using TelegramSalaryBot.Exceptions;
using TelegramSalaryBot.Message;
using TelegramSalaryBot.Jobs;

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
        getClientCommand.CommandText = $"SELECT * FROM `{DBProps.Client.ClientsTable}` WHERE `{DBProps.Client.TelegramId}` = '@telegramIdValue'";
        getClientCommand.Parameters.AddWithValue("@telegramIdValue", tid);
        using var reader = await getClientCommand.ExecuteReaderAsync();

        if (reader.HasRows == false) throw new ClientNotFoundException("Client was not found in database");

        var client = default(TelegramClient);
        while (reader.Read())
        {
            var localId = reader.GetInt64(DBProps.Client.LocalId);
            var clientId = reader.GetInt64(DBProps.Client.TelegramId);
            var domain = reader.GetString(DBProps.Client.TelegramDomain);
            var firstName = reader.GetString(DBProps.Client.TelegramFistName);
            var lastName = reader.GetString(DBProps.Client.TelegramLastName);
            var dateTime = reader.GetDateTime(DBProps.Client.TelegramLastMessageTime);
            var identifier = reader.GetInt32(DBProps.Client.TelegramLastMessageId);

            client = new TelegramClient(clientId, localId, domain, firstName, lastName)
            {
                LastMessageID = (MessageIdentifier)identifier,
                LastMessageTime = dateTime
            };
        }

        await connection.CloseAsync();
        return await Task.FromResult(client ?? throw new ClientEmptyResultException());
    }
    public static async Task<List<TelegramClient>> GetClients()
    {
        using var connection = await OpenConnection();

        using var getClientsCommand = connection.CreateCommand();
        getClientsCommand.CommandText = $"SELECT * FROM `{DBProps.Client.ClientsTable}`";
        using var reader = await getClientsCommand.ExecuteReaderAsync();

        var clients = new List<TelegramClient>();
        while(reader.Read())
        {
            var localId = reader.GetInt64(DBProps.Client.LocalId);
            var telegramId = reader.GetInt64(DBProps.Client.TelegramId);
            var domain = reader.GetString(DBProps.Client.TelegramDomain);
            var firstName = reader.GetString(DBProps.Client.TelegramFistName);
            var lastName = reader.GetString(DBProps.Client.TelegramLastName);
            var dateTime = reader.GetDateTime(DBProps.Client.TelegramLastMessageTime);
            var identifier = reader.GetInt32(DBProps.Client.TelegramLastMessageId);
            clients.Add(new TelegramClient(telegramId, localId, domain, firstName, lastName)
            {
                LastMessageID = (MessageIdentifier)identifier,
                LastMessageTime = dateTime
            });
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
        saveClientCommand.CommandText = $"INSERT INTO `{DBProps.Client.ClientsTable}` " +
            $"(`{DBProps.Client.TelegramId}`,`{DBProps.Client.TelegramDomain}`,`{DBProps.Client.TelegramFistName}`, `{DBProps.Client.TelegramLastName}`) " +
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
        command.CommandText = $"SELECT MAX(ID) FROM `{DBProps.Client.ClientsTable}`";
        var result = await command.ExecuteScalarAsync();

        await connection.CloseAsync();
        return await Task.FromResult(result == DBNull.Value ? 1 : ( Convert.ToInt64(result) + 1) );
    }

    public static async Task UpdateLastMessageInfo(IClient client)
    {
        await Console.Out.WriteLineAsync("UpdateLastMessageInfo started");
        using var connection = await OpenConnection();
        using var updCommand = connection.CreateCommand();
        updCommand.CommandText = $"UPDATE `{DBProps.Client.ClientsTable}` SET `{DBProps.Client.TelegramLastMessageId}` = @lastMessageId, `{DBProps.Client.TelegramLastMessageTime}` = @lastMessageTime WHERE `{DBProps.Client.TelegramId}` = '{client.TelegramID}'";
        updCommand.Parameters.AddWithValue("@lastMessageId", client.LastMessageID);
        updCommand.Parameters.AddWithValue("@lastMessageTime", client.LastMessageTime);

        await updCommand.ExecuteNonQueryAsync();
        await connection.CloseAsync();
        await Console.Out.WriteLineAsync("UpdateLastMessageInfo finished");
    }

    public static async Task AddClientsJob(IClient client, string jobName)
    {
        await Console.Out.WriteLineAsync("Adding job started");

        using var connection = await OpenConnection();

        using var command = connection.CreateCommand();
        command.CommandText = $"INSERT INTO `{DBProps.Job.JobsTable}` (`{DBProps.Job.ClientId}`, `{DBProps.Job.JobName}`) VALUES (@clientId, @jobName)";
        command.Parameters.AddWithValue("@clientId", client.LocalID);
        command.Parameters.AddWithValue("@jobName", jobName);

        await command.ExecuteNonQueryAsync();
        await connection.CloseAsync();

        await Console.Out.WriteLineAsync("Adding job finished");
    }

    public static async Task<List<Job>> GetClientJobs(IClient client)
    {
        await Console.Out.WriteLineAsync("Adding job started");
        using var connection = await OpenConnection();


        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM `{DBProps.Job.JobsTable}` WHERE `{DBProps.Job.ClientId}` = @clientId";
        command.Parameters.AddWithValue("@clientId", client.LocalID);
        using var reader = await command.ExecuteReaderAsync();

        var jobs = new List<Job>();
        while(await reader.ReadAsync())
        {
            var jobName = reader.GetString(DBProps.Job.JobName);
            var jobId = reader.GetInt32(DBProps.Job.Id);
            jobs.Add(new Job(jobId, (int)client.LocalID, jobName));
        }

        await connection.CloseAsync();

        await Console.Out.WriteLineAsync("Adding job finished");
        return jobs;
    }
}
