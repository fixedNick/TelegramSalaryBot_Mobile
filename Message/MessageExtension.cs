using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.BotAPI.GettingUpdates;
using Telegram.BotAPI;
using TelegramSalaryBot.Client;
using TelegramSalaryBot.Exceptions;
using TelegramSalaryBot.Database.SQL;
using TelegramSalaryBot.Jobs;

namespace TelegramSalaryBot.Message;

public static class MessageExtension
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    public static IMessage GetMessage(Update upd)
    {
        switch (upd.Type)
        {
            case UpdateType.CallbackQuery:
                return new CallbackQueryMessage(
                new MessageFrom(upd.CallbackQuery.From.Id,
                                upd.CallbackQuery.From.FirstName ?? "",
                                upd.CallbackQuery.From.LastName ?? "",
                                upd.CallbackQuery.From.Username ?? ""),
                upd.CallbackQuery.Data ?? throw new Exception("Data of update is null"), UpdateType.CallbackQuery, upd.CallbackQuery.Id);

            case UpdateType.Message:
                return new TextMessage(
                new MessageFrom(upd.Message.Chat.Id,
                                upd.Message.Chat.FirstName ?? "",
                                upd.Message.Chat.LastName ?? "",
                                upd.Message.Chat.Username ?? ""),
                upd.Message.Text?? throw new Exception("Data of update is null"), UpdateType.Message);
        }

        throw new EmptyMessageException();
    }
    public static MessageIdentifier GetMessageIdentifier(IMessage message)
    {
        var cmd = message.Text;
        switch (message.MessageType)
        {
            case UpdateType.Message:
                if (cmd.Length >= 1 && cmd[0] != '/') return MessageIdentifier.Text;
                if (StrContains(cmd, "start") || StrContains(cmd, "home")) return MessageIdentifier.ShowMenu;
                if (StrContains(cmd, "addjob")) return MessageIdentifier.AddJob;
                if (StrContains(cmd, "getsalary")) return MessageIdentifier.GetSalary;
                throw new UnknownMessageCommandException(cmd);
            case UpdateType.CallbackQuery:
                if (StrEquals(cmd, "addjob")) return MessageIdentifier.AddJob;
                if (StrEquals(cmd, "getjobs")) return MessageIdentifier.GetJobs;
                if (StrEquals(cmd, "addsalary")) return MessageIdentifier.AddSalary;
                if (StrEquals(cmd, "getsalary")) return MessageIdentifier.GetSalary;
                throw new UnknownMessageCommandException(cmd);
            default:
                return MessageIdentifier.Text;
        }
    }
    private static bool StrEquals(string s1, string s2) => s1.ToLower().Equals(s2);
    private static bool StrContains(string cmd, string substr) => cmd.ToLower().Contains("/" + substr);
}