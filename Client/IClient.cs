using System;
using System.Linq;
using System.Collections.Generic;

using TelegramSalaryBot.Message;
using System.Threading.Tasks;
using TelegramSalaryBot.Request;

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
    public IRequest CurrentRequest { get; }
    public void FinishCurrentRequest();
    public Task UpdateLastResponse(ResponseMessage response, MessageIdentifier identifier);
    public void SetupRequest(MessageIdentifier identifier);
}