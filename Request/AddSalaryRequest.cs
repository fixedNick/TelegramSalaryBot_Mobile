using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramSalaryBot.Client;
using TelegramSalaryBot.Message;

namespace TelegramSalaryBot.Request;

public class AddSalaryRequest 
{
    public int JobId;
    public int Salary;

    public Task FillRequest(IClient client, MessageIdentifier messageIdentifier)
    {
        throw new NotImplementedException();
    }
}
