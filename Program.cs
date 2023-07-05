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
using TelegramSalaryBot.Exceptions;
using Telegram.BotAPI.UpdatingMessages;
using Telegram.BotAPI.InlineMode;
using TelegramSalaryBot.Request;

namespace TelegramSalaryBot;

public static class Program
{
    public static void Main()
    {
        Sql.Initialize(new DatabaseInfo("localhost", "root", "", DBProps.DatabaseName));
        LocalDatabase.Load();
        var token = "6231761371:AAEGisiIfFmQANhG2FhNQIUVFqI3C5MIKvA";
        var bot = new BotClient(token);
        bot.SetMyCommands(new BotCommand("home", "Start Page"), new BotCommand("getsalary", "Get Salary"), new BotCommand("addjob", "Add Job"));
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
                        var message = MessageExtension.GetMessage(update);
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
                                    message.From.FirstName, message.From.LastName)
                { LastMessageID = MessageIdentifier.ShowMenu, LastMessageTime = DateTime.Now };
                Db.SaveClient(client);
            }
            bot.SendMessage(client.TelegramID, "Hello, you successfully have been registered");
        }
        else client = await Db.GetClient(message.From.Id);

        try
        {
            var messageIdentifier = MessageExtension.GetMessageIdentifier(message);
            client.SetupRequest(messageIdentifier);
            var response = await client.CurrentRequest.FillRequest(client, message.Text);
            await bot.SendMessageAsync(client.TelegramID, response.Text, replyMarkup: response.Keyboard ?? new ReplyKeyboardRemove(), parseMode: "HTML");

            if (client.CurrentRequest.IsRequestCompleted && client.CurrentRequest.IsNavigateRequired)
            {
                client.FinishCurrentRequest();
                var firstResponse = await client.CurrentRequest.FillRequest(client, message.Text);

                await client.UpdateLastResponse(firstResponse, client.CurrentRequest.Identifier);
                await Sql.UpdateLastMessageInfo(client);
                
                await bot.SendMessageAsync(client.TelegramID, firstResponse.Text, replyMarkup: firstResponse.Keyboard ?? new ReplyKeyboardRemove(), parseMode: "HTML");
            }
        } 
        catch(Exception e) when (e is UnknownMessageCommandException | e is UnknownCallbackDataException)
        {
            await bot.SendMessageAsync(client.TelegramID, "Sorry, i didn't understand you, please try again");
        }
        finally
        {
            if (message.MessageType == UpdateType.CallbackQuery) 
                await bot.AnswerCallbackQueryAsync(callbackQueryId: message.CallbackQueryId, " ", showAlert: false);
        }
    }
}
