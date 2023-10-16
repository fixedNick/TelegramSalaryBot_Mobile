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
    public static IMessage GetMessage(Update upd)
    {
        switch (upd.Type)
        {
            case UpdateType.CallbackQuery:
                return new CallbackQueryMessage(
                new MessageFrom(upd.CallbackQuery!.From.Id,
                                upd.CallbackQuery.From.FirstName ?? "",
                                upd.CallbackQuery.From.LastName ?? "",
                                upd.CallbackQuery.From.Username ?? ""),
                upd.CallbackQuery.Data ?? throw new Exception("Data of update is null"), UpdateType.CallbackQuery, upd.CallbackQuery.Id);

            case UpdateType.Message:
                return new TextMessage(
                new MessageFrom(upd.Message!.Chat.Id,
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
                return GetTextMessageIdentifier(cmd);
            case UpdateType.CallbackQuery:
                return GetCallbackQueryIdentifier(cmd);
            default:
                return MessageIdentifier.Text;
        }
    }

    private static MessageIdentifier GetCallbackQueryIdentifier(string cmd)
    {
        if (StrEquals(cmd, "addjob")) return MessageIdentifier.AddJob;
        if (StrContains(cmd, "getjobs")) return MessageIdentifier.GetJobs;
        if (StrContains(cmd, "addsalary")) return MessageIdentifier.AddSalary;
        if (StrContains(cmd, "getsalary")) return MessageIdentifier.GetSalary;
        throw new UnknownMessageCommandException(cmd);
    }
    private static MessageIdentifier GetTextMessageIdentifier(string cmd) 
    {
        if (cmd.Length >= 1 && cmd[0] != '/') return MessageIdentifier.Text;
        if (StrContains(cmd, "start") || StrContains(cmd, "home")) return MessageIdentifier.ShowMenu;
        if (StrContains(cmd, "addjob")) return MessageIdentifier.AddJob;
        if (StrContains(cmd, "getsalary")) return MessageIdentifier.GetSalary;
     
        throw new UnknownMessageCommandException(cmd);
    }

    private static bool StrEquals(string s1, string s2) => s1.ToLower().Equals(s2);
    private static bool StrContains(string cmd, string substr) => cmd.ToLower().Contains(substr);
}