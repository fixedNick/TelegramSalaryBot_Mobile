using System;
using System.Linq;
using System.Collections.Generic;

namespace TelegramSalaryBot.Message;

public class CallbackQueryMessage : IMessage
{         
	public MessageFrom From { get; set; }
	public string Text { get; set; }
	public CallbackQueryMessage(MessageFrom from, string text)
	{
		From = from;
		Text = text;
	}
}
