using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramSalaryBot.Client;
using TelegramSalaryBot.Database.SQL;
using TelegramSalaryBot.Exceptions;
using TelegramSalaryBot.Jobs;
using TelegramSalaryBot.Message;

namespace TelegramSalaryBot.Request.Requests;

public class GetSalaryRequest : IRequest
{
    private Job SelectedJob = new Job();
    private int Month;

    public IRequest NavigateTo { get; }

    public bool IsRequestCompleted { get; private set; } = false;

    public MessageIdentifier Identifier { get; }

    public int CurrentStep { get; private set; } = 0;

    public int TotalSteps { get; } = 3;

    public GetSalaryRequest(IRequest? navigateTo = null)
    {
        if (navigateTo is null) NavigateTo = IRequest.Menu;
        else NavigateTo = navigateTo;
        Identifier = MessageIdentifier.GetJobs;
    }

    public async Task<ResponseMessage> FillRequest(IClient client, string messageText)
    {
        // Select job
        // select month
        var response = default(ResponseMessage);

        switch(CurrentStep)
        {
            case 0:
                response = new()
                {
                    Text = "Select job",
                    Keyboard = await KeyBoards.GetInline(client, this)
                };
                break;
            case 1:
                var jobId = int.Parse(messageText.Split("_").Last());
                SelectedJob = (await Sql.GetClientJobs(client)).Where(j => j.Id == jobId).First();
                response = new()
                {
                    Text = $"Selected job: {SelectedJob}.\nEnter month of CURRENT year"
                };
                break;
            case 2:
                if (int.TryParse(messageText, out int month) == false) return new() { Text = "Incorrect month, enter 1 - 12 value" };
                var salary = await Sql.GetClientMonthSalary(client, month, SelectedJob.Id);
                response = new()
                {
                    Text = $"Your income on {SelectedJob.Name} for month {month} is {salary}"
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
