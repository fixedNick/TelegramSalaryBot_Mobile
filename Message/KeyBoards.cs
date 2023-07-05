using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;
using TelegramSalaryBot.Client;
using TelegramSalaryBot.Database.SQL;
using TelegramSalaryBot.Jobs;

namespace TelegramSalaryBot.Message;

internal static class KeyBoards
{
    public static async Task<InlineKeyboardMarkup> GetInline(IClient client, MessageIdentifier identifier, int step = 0)
    {
        switch (identifier)
        {
            case MessageIdentifier.ShowMenu:
                return new InlineKeyboardMarkup
                {
                    InlineKeyboard = new InlineKeyboardButton[][]
                    {
                        new []
                        {
                            InlineButtonBuilder.SetCallbackData("Add Job", "addjob"),
                            InlineButtonBuilder.SetCallbackData("Get Jobs", "getjobs")
                        },
                        new[]
                        {
                            InlineButtonBuilder.SetCallbackData("Add Salary", "addsalary"),
                            InlineButtonBuilder.SetCallbackData("Get Salary", "getsalary")
                        }
                    }
                };
            case MessageIdentifier.AddSalary:
                var jobs = await Sql.GetClientJobs(client);
                var buttons = new List<InlineKeyboardButton>();
                foreach (var j in jobs)
                    buttons.Add(InlineButtonBuilder.SetCallbackData(j.Name, "jobid_" + j.Id));
                return new InlineKeyboardMarkup
                {
                    InlineKeyboard = new InlineKeyboardButton[][]
                    {
                        buttons.ToArray()
                    }
                };
            default: return new InlineKeyboardMarkup();
        }
    }
    public static async Task<ReplyMarkup?> GetReply(IClient client, MessageIdentifier identifier, int step = 0)
    {
        switch(identifier)
        {
            case MessageIdentifier.AddJob:
                switch(step)
                {
                    case 1:
                        var jobTypes = Enum.GetNames(typeof(JobType)).Select(n => new KeyboardButton(n)).Skip(1).ToArray();
                        var keyboard = new ReplyKeyboardMarkup
                        {
                            Keyboard = new KeyboardButton[][]
                            {
                                jobTypes
                            }
                        };
                        return keyboard;

                }
                break;
        }
        return null;
    }
}
