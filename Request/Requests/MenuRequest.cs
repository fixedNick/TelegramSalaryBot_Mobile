using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramSalaryBot.Client;
using TelegramSalaryBot.Message;

namespace TelegramSalaryBot.Request;

public class MenuRequest : IRequest
{
    public IRequest NavigateTo { get; }

    public bool IsRequestCompleted => true;

    public MessageIdentifier Identifier { get; }
    public int TotalSteps { get; } = 0;
    public int CurrentStep { get; } = 0;

    public MenuRequest()
    {
        NavigateTo = this;
        Identifier = MessageIdentifier.ShowMenu;
    }

    public async Task<ResponseMessage> FillRequest(IClient client, string messageText)
    {
        return new ResponseMessage
        {
            Text = "Main Menu",
            Keyboard = await KeyBoards.GetInline(client, Identifier)
        };
    }
}
