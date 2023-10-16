using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramSalaryBot.Client;
using TelegramSalaryBot.Message;

namespace TelegramSalaryBot.Request.Requests;

public class GetJobsRequest : IRequest
{
    public IRequest NavigateTo { get; }

    public bool IsRequestCompleted { get; } = true;

    public MessageIdentifier Identifier { get; }

    public int CurrentStep { get; private set; } = 0;

    public int TotalSteps { get; private set; } = 0;

    public GetJobsRequest(IRequest? navigateTo = null)
    {
        if (navigateTo is null) NavigateTo = IRequest.Menu;
        else NavigateTo = navigateTo;
        Identifier = MessageIdentifier.GetJobs;
    }

    public async Task<ResponseMessage> FillRequest(IClient client, string messageText)
    {
        return new ResponseMessage
        {
            Text = "Your jobs",
            Keyboard = await KeyBoards.GetInline(client, this)
        };
    }
}
