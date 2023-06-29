using System;
using System.Linq;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.GettingUpdates;

using TelegramSalaryBot.Client;
using TelegramSalaryBot.Message;
using TelegramSalaryBot.Database;
using TelegramSalaryBot.Database.IO;
using TelegramSalaryBot.Database.SQL;
using TelegramSalaryBot.Database.Exceptions;

namespace TelegramSalaryBot;

public static class Program
{
    public static void Main()
    {
        Sql.Initialize(new DatabaseInfo("localhost", "root", "", DBProps.DatabaseName));
        LocalDatabase.Load();
        var token = "6231761371:AAEGisiIfFmQANhG2FhNQIUVFqI3C5MIKvA";
        var bot = new BotClient(token);
        Listen(bot);
    }

    private static void Listen(BotClient bot)
    {
        var updates = bot.GetUpdates();
        while (true)
        {
            if (updates.Any())
            {
                foreach (var update in updates)
                {
                    try
                    {
                        var message = IMessage.GetMessage(update);
                        ProcessUpdate(bot, message);
                    }
                    catch (EmptyMessageException)
                    {
                        continue;
                    }
                }

                var offset = updates.Last().UpdateId + 1;
                updates = bot.GetUpdates(offset);
            }
            else
            {
                updates = bot.GetUpdates();
            }

        }
    }

    private static readonly object syncObject = new object();
    private static async void ProcessUpdate(BotClient bot, IMessage message)
    {
        TelegramClient? client = default(TelegramClient);

        if (await Db.IsClientExist(message.From.Id) == false)
        {
            lock (syncObject)
            {
                client = new TelegramClient(message.From.Id, Sql.GetNextLocalID().GetAwaiter().GetResult(), message.From.UserName,
                                    message.From.FirstName, message.From.LastName);
                Db.SaveClient(client);
            }
            bot.SendMessage(client.TelegramID, "Hello, you successfully have been registered");
        }
        else client = await Db.GetClient(message.From.Id);
        bot.SendMessage(client.TelegramID, $"Welcome, {client.FirstName}");
    }
}
