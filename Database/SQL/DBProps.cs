using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSalaryBot.Database.SQL;

public static class DBProps
{
    public static readonly string DatabaseName = "TelegramSalary";

    public static readonly string ClientsTable = "TelegramClients";
    public static readonly string TelegramId = "TelegramId";
    public static readonly string LocalId = "ID";
    public static readonly string TelegramDomain = "Domain";
    public static readonly string TelegramFistName = "FistName";
    public static readonly string TelegramLastName = "LastName";
}
