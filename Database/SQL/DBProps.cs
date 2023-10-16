using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSalaryBot.Database.SQL;

public static class DBProps
{
    public static readonly string DatabaseName = "TelegramSalary";

    #region Clients Table
    public static class Client
    {
        public static readonly string ClientsTable = "TelegramClients";

        public static readonly string TelegramId = "TelegramId";
        public static readonly string LocalId = "ID";
        public static readonly string TelegramDomain = "Domain";
        public static readonly string TelegramFistName = "FistName";
        public static readonly string TelegramLastName = "LastName";
        public static readonly string TelegramLastMessageTime = "LastMessageTime";
        public static readonly string TelegramLastMessageId = "LastMessageID";
    }
    #endregion

    #region Jobs Table
    public static class Job
    {
        public static readonly string JobsTable = "Jobs";

        public static readonly string ClientId = "ClientId";
        public static readonly string JobName = "JobName";
        public static readonly string JobType = "JobType";
        public static readonly string Id = "ID";
    }
    #endregion

    #region Salary Table
    public static class Salary
    {
        public static readonly string Table = "Salary";

        public static readonly string ClientId = "ClientId";
        public static readonly string JobId = "JobId";
        public static readonly string Income = "Income";
        public static readonly string Date = "Date";
    }
    #endregion
}
