using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramSalaryBot.Client;
using TelegramSalaryBot.Message;

namespace TelegramSalaryBot.Request;

public interface IRequest
{
    static IRequest None = new MenuRequest();
    public IRequest NavigateTo { get; }
    public bool IsNavigateRequired { get; }
    public bool IsRequestCompleted { get; }
    public MessageIdentifier Identifier { get; }
    Task<ResponseMessage> FillRequest(IClient client, string messageText);
}
