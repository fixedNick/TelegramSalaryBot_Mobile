using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;

namespace TelegramSalaryBot.Message;

internal static class KeyBoards
{
    public static InlineKeyboardMarkup Get(MessageIdentifier identifier)
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
                            InlineButtonBuilder.SetCallbackData("Add Job", "addjob")
                        },
                        new[]
                        {
                            InlineButtonBuilder.SetCallbackData("Add Job", "addjob"),
                            InlineButtonBuilder.SetCallbackData("Add Job", "addjob")
                        }
                    }
                };
            default: return new InlineKeyboardMarkup();
        }
    }
}
