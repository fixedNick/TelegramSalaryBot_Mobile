using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramSalaryBot.Client;
using TelegramSalaryBot.Exceptions;
using TelegramSalaryBot.Jobs;
using TelegramSalaryBot.Message;
using TelegramSalaryBot.Database.SQL;

namespace TelegramSalaryBot.Request.Requests;

public class AddJobRequest : IRequest
{
    public IRequest NavigateTo { get; }
    public string JobName { get; private set; } = string.Empty;
    public JobType JobType { get; private set; }
    public bool IsRequestCompleted { get; private set; } = false;
    public MessageIdentifier Identifier { get; private set; }

    public int CurrentStep { get; private set; } = 0;
    public int TotalSteps { get; private set; } = 3;

    public AddJobRequest(IRequest? navigateTo = null)
    {
        if (navigateTo == null) NavigateTo = IRequest.Menu;
        else NavigateTo = navigateTo;

        Identifier = MessageIdentifier.AddJob;
    }

    public async Task<ResponseMessage> FillRequest(IClient client, string messageText)
    {
        var response = default(ResponseMessage);
        switch (CurrentStep)
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
                if (Enum.GetNames(typeof(JobType)).Where(j => j.ToLower().Trim().Equals(messageText.ToLower().Trim())).FirstOrDefault() == default) return new ResponseMessage { Text = "Incorrect type of job" };

                var selectedJobType = (JobType)Enum.Parse(typeof(JobType), messageText);
                JobType = selectedJobType;
                await Sql.AddClientsJob(client, JobName, JobType);
                response = new ResponseMessage()
                {
                    Text = $"You successfully selected {selectedJobType.ToString()} as jobType for {JobName}.\nList of your jobs has been updated"
                };
                break;
            default: throw new UndefinedRequestStepException();
        }

        CurrentStep++;
        if (CurrentStep == TotalSteps)
            IsRequestCompleted = true;
        return response;
    }
}
