using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramSalaryBot.Client;
using TelegramSalaryBot.Database.SQL;
using TelegramSalaryBot.Exceptions;
using TelegramSalaryBot.Message;

namespace TelegramSalaryBot.Request.Requests;

public class AddSalaryRequest : IRequest
{
    public int JobId { get; private set; }
    public double Income { get; private set; }
    public DateTime Date { get; private set; }
    public IRequest NavigateTo { get; private set; }
    public bool IsRequestCompleted { get; private set; }
    public MessageIdentifier Identifier { get; private set; }
    public int CurrentStep { get; private set; } = 0;
    public int TotalSteps { get; private set; } = 4;

    public AddSalaryRequest(IRequest? navigateTo = null)
    {
        if (navigateTo is null) NavigateTo = IRequest.Menu;
        else NavigateTo = navigateTo;
        Identifier = MessageIdentifier.AddSalary;
    }

    public async Task<ResponseMessage> FillRequest(IClient client, string messageText)
    {
        // You selected add salary, please select to witch job of the list you want to add an income
        // SHOW LIST
        // You selected {} job, please enter amount of income
        // waiting text
        // If you wish to set past or future date of this income, please enter it in format: dd.mm.yyyy, you can skip month or/and year if its current
        // settings date
        // insert sql
        // finished

        var response = default(ResponseMessage);
        switch (CurrentStep)
        {
            case 0:
                response = new ResponseMessage
                {
                    Text = "You selected add salary, please select to witch job of the list you want to add an income",
                    Keyboard = await KeyBoards.GetInline(client, Identifier, CurrentStep)
                };
                break;
            case 1:
                // get job id
                // get jobs from sql
                // get id from array

                JobId = int.Parse(messageText.Split('_')[1]);
                var selectedJob = (await Sql.GetClientJobs(client)).Where(j => j.Id == JobId).First();
                response = new ResponseMessage
                {
                    Text = $"You selected {selectedJob} job, please enter amount of income"
                };
                break;
            case 2:
                double income = 0;
                if (double.TryParse(messageText, out income) == false && double.TryParse(messageText.Replace(".", ","), out income))
                    return new ResponseMessage { Text = "I cannot understand this amount, please try again" };
                response = new ResponseMessage
                {
                    Text = "If you wish to set past or future date of this income, please enter it in format: dd/mm/yyyy, you can skip month or/and year if its current",
                    Keyboard = await KeyBoards.GetReply(client, this)
                };

                Income = income;
                break;
            case 3:
                var dateParts = messageText.Split('/').Length == 1 ? messageText.Split(".").ToList() : messageText.Split('/').ToList();

                if (dateParts.Count <= 1) dateParts.Add(DateTime.Now.Month.ToString());
                if (dateParts.Count <= 2) dateParts.Add(DateTime.Now.Year.ToString());

                if (!int.TryParse(dateParts[0], out int day) || !int.TryParse(dateParts[1], out int month) || !int.TryParse(dateParts[2], out int year))
                    return new ResponseMessage { Text = "I cannot understand entered date, please use format: day/month/year", Keyboard = await KeyBoards.GetReply(client, this) };

                // ADD INCOME
                Date = new DateTime(year, month, day);

                await Sql.ClientAddSalary(client, this);

                response = new()
                {
                    Text = $"[{Date.ToString("dd-MM-yyyy")}] You successully added {Income} of income to job {(await Sql.GetClientJobs(client)).Where(j => j.Id == JobId).First()}"
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