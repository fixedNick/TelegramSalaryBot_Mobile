using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using TelegramSalaryBot.Client;
using System.Threading;

using TelegramSalaryBot.Database.IO;
using TelegramSalaryBot.Database.SQL;
using TelegramSalaryBot.Database.Exceptions;

namespace TelegramSalaryBot.Database;

public class Db
{
    public static async Task<bool> IsClientExist(long tid)
    {
        try
        {
            if (await LocalDatabase.IsClientExist(tid))
                return await Task.FromResult(true);
        }
        catch(ClientNotFoundException) 
        { }

        try
        {
            if (await Sql.IsClientExist(tid))
                return await Task.FromResult(true);
            else return await Task.FromResult(false);
        }
        catch (Exception e) when (e is ClientEmptyResultException || e is ClientNotFoundException)
        {
            return await Task.FromResult(false);
        }
    }

    public static async Task<TelegramClient> GetClient(long clientId)
    {
        // try get local client
        var client = default(TelegramClient);
        try
        {
            client = await LocalDatabase.GetClient(clientId);
            return await Task.FromResult(client);

        }
        catch(ClientNotFoundException)
        {

        }
        // if not exist then try get client online
        // and add locally
        // if locally id is busy then sync local & sql db's

        client = await Sql.GetClient(clientId);

        // trying to get localUser with id of client
        bool isSyncRequired = false;

        LocalDatabase.RegisteredAccounts.ForEach(c => {
            if (c.LocalID == client.LocalID && c.TelegramID != client.TelegramID)
                isSyncRequired = true;
        });

        if (isSyncRequired)
        {
            await SyncLocalAndSqlDatabases();
            client = await LocalDatabase.GetClient(clientId);
        }
        
        return client;
    }
    public static void SaveClient(TelegramClient client)
    {
        LocalDatabase.SaveClient(client);
        Sql.SaveClient(client).GetAwaiter().GetResult();
    }

    private static async Task SyncLocalAndSqlDatabases()
    {
        await Task.Run(async () => {
            LocalDatabase.ClearDatabase();
            (await Sql.GetClients()).ForEach(c =>
            {
                LocalDatabase.SaveClient(c);
            });
        });
    }
}