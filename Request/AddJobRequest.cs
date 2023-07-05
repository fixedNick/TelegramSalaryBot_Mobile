using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramSalaryBot.Client;
using TelegramSalaryBot.Jobs;
using TelegramSalaryBot.Message;

namespace TelegramSalaryBot.Request;

public class AddJobRequest : IRequest
{
    public IRequest NavigateTo { get; }
    public bool IsNavigateRequired { get; } = true;
    public string JobName { get; private set; } = string.Empty;
    public JobType JobType { get; }
    public bool IsRequestCompleted { get; private set; } = false;
    public MessageIdentifier Identifier { get; private set; }

    private int CurrentStep = 0;
    private int TotalSteps = 3;

    public AddJobRequest(IRequest navigateTo)
    {
        Identifier = MessageIdentifier.AddJob;
        NavigateTo = navigateTo;
    }

    public async Task<ResponseMessage> FillRequest(IClient client, string messageText)
    {
        var response = default(ResponseMessage);
        switch(CurrentStep)
        {
            case 0:
                response = new ResponseMessage
                {
                    Text = "You selected to add new job, please enter a name of this job below"
                };
                break;
            case 1:
                if (messageText.Length <= 1)
                    return new ResponseMessage() { Text = "Job Name needs to have at least 2 symbols" };
                JobName = messageText;
                response = new ResponseMessage()
                {
                    Text = "Nice, select type of your job",
                    Keyboard = await KeyBoards.GetReply(client, MessageIdentifier.AddJob, CurrentStep)
                };
                break;
            case 2:
                if (Enum.GetNames(typeof(JobType)).Where(j => j.ToLower().Trim().Equals(messageText.ToLower().Trim())).FirstOrDefault() == default(string)) return new ResponseMessage { Text = "Incorrect type of job" };

                var selectedJobType = (JobType) Enum.Parse(typeof(JobType), messageText);
                response = new ResponseMessage()
                {
                    Text = $"You successfully selected {selectedJobType.ToString()} as jobType for {JobName}"
                };
                break;
            default: throw new Exception();
        }

        CurrentStep++;
        if (CurrentStep == TotalSteps)
            IsRequestCompleted = true;
        return response;
    }
}
