using System;
using System.Linq;
using System.Collections.Generic;

using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.GettingUpdates;
using System.Threading;

namespace TelegramSalaryBot.Message;

public interface IMessage 
{
	public MessageFrom From { get; set; }
	public string Text { get; set; }	
	
#pragma warning disable CS8602 // Dereference of a possibly null reference.
	public static IMessage GetMessage(Update upd)
	{
		switch(upd.Type)
		{
			case UpdateType.CallbackQuery:
                return new CallbackQueryMessage(
				new MessageFrom(upd.CallbackQuery.From.Id,
								upd.CallbackQuery.From.FirstName ?? "",
								upd.CallbackQuery.From.LastName ?? "",
								upd.CallbackQuery.From.Username ?? ""),
				upd.CallbackQuery.Data ?? throw new Exception("Data of update is null"));

            case UpdateType.Message:
                return new TextMessage(
				new MessageFrom(upd.Message.Chat.Id,
								upd.Message.Chat.FirstName ?? "",
								upd.Message.Chat.LastName ?? "",
								upd.Message.Chat.Username ?? ""),
				upd.Message.Text?? throw new Exception("Data of update is null"));
        }

		throw new EmptyMessageException();
	}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}
