using System;
using System.Linq;
using System.Collections.Generic;

using TelegramSalaryBot.Message;

namespace TelegramSalaryBot.Client;

public interface IClient
{
    public long LocalID { get; set; } 
    public long TelegramID { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime LastMessageTime { get; set; }
    public MessageIdentifier LastMessageID { get; set; }
}