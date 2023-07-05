﻿using System;
using System.Linq;
using System.Collections.Generic;

using TelegramSalaryBot.Message;
using System.Threading.Tasks;
using TelegramSalaryBot.Request;

namespace TelegramSalaryBot.Client;

public class TelegramClient : IClient
{
    public long LocalID { get; set; }
    public long TelegramID { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime LastMessageTime { get; set; }
    public MessageIdentifier LastMessageID { get; set; }
    public IRequest CurrentRequest { get; private set; } = IRequest.None;

    public TelegramClient(long tid, long localId, string domain, string firstName, string lastName)
    {
        TelegramID = tid;
        UserName = domain;
        FirstName = firstName;
        LastName = lastName;
        LastMessageTime = DateTime.Now;
        LocalID = localId;
    }

    public void FinishCurrentRequest()
    {
        CurrentRequest = CurrentRequest.NavigateTo;
    }

    public async Task UpdateLastResponse(ResponseMessage response, MessageIdentifier identifier)
    {
        
        await Console.Out.WriteLineAsync("UpdateLastResponse started..");
        LastMessageID = identifier;
        LastMessageTime = DateTime.Now;
        await Console.Out.WriteLineAsync("UpdateLastResponse finished..");
    }
    public void SetupRequest(MessageIdentifier identifier)
    {
        switch(identifier) {
            case MessageIdentifier.AddJob:
                CurrentRequest = new AddJobRequest(IRequest.None);
                break;
            case MessageIdentifier.ShowMenu:
                CurrentRequest = new MenuRequest();
                break;
        }
    }
}