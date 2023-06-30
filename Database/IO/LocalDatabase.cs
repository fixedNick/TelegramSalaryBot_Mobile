using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using TelegramSalaryBot.Client;
using TelegramSalaryBot.Database.SQL;
using TelegramSalaryBot.Exceptions;

namespace TelegramSalaryBot.Database.IO;

public class LocalDatabase
{
    public static List<TelegramClient> RegisteredAccounts = new List<TelegramClient>();

    public static void Load()
    {
        LocalDatabase.ClearDatabase();
        Sql.GetClients().GetAwaiter().GetResult().ForEach(c => RegisteredAccounts.Add(c));
    }
    public static async Task<bool> IsClientExist(long tid)
        => await Task.FromResult(await GetClient(tid) is not default(TelegramClient));

    public async static Task<TelegramClient> GetClient(long tid)
    {
        return await Task.FromResult(LocalDatabase.RegisteredAccounts
                    .Where(c => c.TelegramID == tid).FirstOrDefault()
                    ?? throw new ClientNotFoundException("Client not found in local database"));
    }

    public static void SaveClient(TelegramClient client)
    {
        RegisteredAccounts.Add(client);
    }
    public static void ClearDatabase()
    {
        LocalDatabase.RegisteredAccounts.Clear();
    }
}