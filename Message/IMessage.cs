using System;
using System.Linq;
using System.Collections.Generic;

using Telegram.BotAPI;
using TelegramSalaryBot.Exceptions;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.GettingUpdates;
using TelegramSalaryBot.Client;

namespace TelegramSalaryBot.Message;

public interface IMessage 
{
    public string CallbackQueryId { get; set; }
	public UpdateType MessageType { get; set; }
	public MessageFrom From { get; set; }
	public string Text { get; set; }	
}
