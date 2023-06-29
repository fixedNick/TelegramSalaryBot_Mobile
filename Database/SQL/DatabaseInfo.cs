using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSalaryBot.Database.SQL;
public struct DatabaseInfo
{
    public string Host { get; private set; } = string.Empty;
    public string User { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string Db { get; private set; } = string.Empty;
    public DatabaseInfo(string host, string user, string pass, string db)
    {
        Host = host;
        User = user;
        Password = pass;
        Db = db;
    }
}