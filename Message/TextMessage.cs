using System;
using System.Linq;
using System.Collections.Generic;

namespace TelegramSalaryBot.Message;

public class TextMessage : IMessage
{         
	public MessageFrom From { get; set; }
	public string Text { get; set; }
	public TextMessage(MessageFrom from, string text)
	{
		From = from;
		Text = text;
	}
}
