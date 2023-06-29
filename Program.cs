using System;
using System.Linq;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.GettingUpdates;	

using TelegramSalaryBot.IO;
using TelegramSalaryBot.Client;
using TelegramSalaryBot.Message;

namespace TelegramSalaryBot;

public static class Program
{
	public static void Main()
	{
		var token = "6231761371:AAEGisiIfFmQANhG2FhNQIUVFqI3C5MIKvA";
		var bot = new BotClient(token);
		Listen(bot);
	}
	
	private static void Listen(BotClient bot)
	{
		var updates = bot.GetUpdates(); 
		while(true)
		{
			if(updates.Any())
			{	
				foreach(var update in updates)
				{
					try
					{
						var message = IMessage.GetMessage(update);
						ProcessUpdate(bot, message);
					}
					catch(EmptyMessageException)
					{
                        continue;
                    }
				}
				
				var offset = updates.Last().UpdateId + 1;
				updates = bot.GetUpdates(offset);
			}
			else
			{
				updates = bot.GetUpdates();
			}
			
		}
	}
	private static void ProcessUpdate(BotClient bot, IMessage message)
	{
		TelegramClient? client = default(TelegramClient);
		
		// check local db
		if(LocalDatabase.IsClientExist(message.From.Id))
			client = LocalDatabase.GetClient(message.From.Id);
		// check online db 
		
		if(client is null)
		{	
			client = new TelegramClient(message.From.Id, message.From.UserName,
								message.From.FirstName, message.From.LastName);
			LocalDatabase.SaveClient(client);
			bot.SendMessage(client.TelegramID, "Hello, you successfully have been registered");
		}
		bot.SendMessage(client.TelegramID, $"Welcome, {client.FirstName}");
	}
}
