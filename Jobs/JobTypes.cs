using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSalaryBot.Jobs;

public enum JobType : int
{
    None = 0,
    Day,
    Night,
    Halfday
}
