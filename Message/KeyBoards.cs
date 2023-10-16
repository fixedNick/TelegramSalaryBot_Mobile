using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;
using TelegramSalaryBot.Client;
using TelegramSalaryBot.Database.SQL;
using TelegramSalaryBot.Exceptions;
using TelegramSalaryBot.Jobs;
using TelegramSalaryBot.Request;
using static TelegramSalaryBot.Database.SQL.DBProps;

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
                if (step == 0)
                    return await GetJobsButtons(client, "addsalary");
                throw new UndefinedRequestStepException();
            case MessageIdentifier.GetJobs:
                if (step == 0)
                    return await GetJobsButtons(client, "getjobs");
                throw new UndefinedRequestStepException();
            case MessageIdentifier.GetSalary:
                if(step == 0)
                    return await GetJobsButtons(client, "getsalary");
                throw new UndefinedRequestStepException();
            default: return new InlineKeyboardMarkup();
        }
    }

    private static async Task<InlineKeyboardMarkup> GetJobsButtons(IClient client, string prefix)
    {
        var clientJobs = await Sql.GetClientJobs(client);
        var getJobsButtons = new List<InlineKeyboardButton>();
        foreach (var j in clientJobs)
            getJobsButtons.Add(InlineButtonBuilder.SetCallbackData(j.Name, $"{prefix}-jobid_" + j.Id));
        return new InlineKeyboardMarkup
        {
            InlineKeyboard = new InlineKeyboardButton[][]
            {
                            getJobsButtons.ToArray()
            }
        };
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
            case MessageIdentifier.AddSalary:
                if(step == 2)
                {
                    return new ReplyKeyboardMarkup()
                    {
                        Keyboard = new KeyboardButton[][]
                        {
                            new KeyboardButton[]
                            {
                                new ($"{DateTime.Now.Day}/{DateTime.Now.Month}/{DateTime.Now.Year}")
                            }
                        }
                    };
                }
                break;
        }
        return null;
    }

    public static async Task<ReplyMarkup?> GetReply(IClient client, IRequest request)
    {
        return await GetReply(client, request.Identifier, request.CurrentStep);
    }
    public static async Task<InlineKeyboardMarkup> GetInline(IClient client, IRequest request)
    {
        return await GetInline(client, request.Identifier, request.CurrentStep);
    }

}
