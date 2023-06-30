﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.BotAPI.AvailableTypes;

namespace TelegramSalaryBot.Message;

public class MessageResponse
{
    public string Text { get; set; } = " ";
    public ReplyMarkup? Keyboard { get; set; } = null;
    public MessageIdentifier Identifier { get; set; }
}
