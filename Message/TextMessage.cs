using System;
using System.Linq;
using System.Collections.Generic;
using Telegram.BotAPI;

namespace TelegramSalaryBot.Message;

public class TextMessage : IMessage
{         
	public MessageFrom From { get; set; }
	public string Text { get; set; }
    public UpdateType MessageType { get; set; }
	public string CallbackQueryId { get; set; } = "";

    public TextMessage(MessageFrom from, string text, UpdateType messageType, string callbackQueryId = "")
	{
		From = from;
		Text = text;
		MessageType = messageType;
	}
}
