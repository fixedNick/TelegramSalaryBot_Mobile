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
				upd.CallbackQuery.Data ?? throw new Exception("Data of update is null"), UpdateType.CallbackQuery, upd.CallbackQuery.Id);

            case UpdateType.Message:
                return new TextMessage(
				new MessageFrom(upd.Message.Chat.Id,
								upd.Message.Chat.FirstName ?? "",
								upd.Message.Chat.LastName ?? "",
								upd.Message.Chat.Username ?? ""),
				upd.Message.Text?? throw new Exception("Data of update is null"), UpdateType.Message);
        }

		throw new EmptyMessageException();
	}
#pragma warning restore CS8602 // Dereference of a possibly null reference.

    public static MessageResponse ProceedMessage(MessageIdentifier identifier, string text, MessageIdentifier lastClientIdentifier)
    {
        MessageResponse? response = default(MessageResponse);
        switch (identifier)
        {
            case MessageIdentifier.ShowMenu:

                response = new MessageResponse
                {
                    Text = "<b> Main Menu </b>",
                    Identifier = identifier
                };
                break;
            case MessageIdentifier.AddJob:
                response = new MessageResponse
                {
                    Text = "Enter name of your job",
                    Identifier = identifier
                };
                break;

            case MessageIdentifier.Text:

                switch(lastClientIdentifier)
                {
                    case MessageIdentifier.AddJob:
                        response = new MessageResponse
                        {
                            Text = $"You successfully addded job with name: {text}",
                            Identifier = MessageIdentifier.ShowMenu
                        };
                        break;
                    default: throw new Exception();
                }
                break;
            default: throw new Exception();
        }
        var keyboard = KeyBoards.Get(identifier);
        response.Keyboard = keyboard;
        return response;
    }

    public static MessageIdentifier GetMessageIdentifier(IMessage message)
    {
        var cmd = message.Text;
        switch (message.MessageType)
        {
            case UpdateType.Message:
                if (cmd.Length >= 1 && cmd[0] != '/') return MessageIdentifier.Text;
                if (StrContains(cmd, "start") || StrContains(cmd, "home")) return MessageIdentifier.ShowMenu;
                throw new UnknownMessageCommandException(cmd);
            case UpdateType.CallbackQuery:
                if (StrEquals(cmd, "addjob")) return MessageIdentifier.AddJob;
                throw new UnknownMessageCommandException(cmd);
            default:
                return MessageIdentifier.Text;
        }
    }
    private static bool StrEquals(string s1, string s2) => s1.ToLower().Equals(s2);
    private static bool StrContains(string cmd, string substr) => cmd.ToLower().Contains("/" + substr);

    public static MessageResponse GetClientLasResponse(TelegramClient client)
    {
        throw new NotImplementedException();
    }
}
