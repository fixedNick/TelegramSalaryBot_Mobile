﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSalaryBot.Message;

public enum MessageIdentifier : int
{
    Text = 0,
    ShowMenu,
    AddJob,
    GetJobs,
    AddSalary,
    GetSalary,
}