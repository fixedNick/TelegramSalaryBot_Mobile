using System;
using System.Linq;
using System.Collections.Generic;

using TelegramSalaryBot.Message;

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

    public TelegramClient(long tid, long localId, string domain, string firstName, string lastName)
    {
        TelegramID = tid;
        UserName = domain;
        FirstName = firstName;
        LastName = lastName;
        LastMessageTime = DateTime.Now;
        LocalID = localId;
    }
}