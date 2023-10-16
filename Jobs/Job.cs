using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSalaryBot.Jobs;

public class Job
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ClientId { get; set; }

    public Job() { }
    public Job(int id, int cid, string name)
    {
        Id = id;
        Name = name;
        ClientId = cid;
    }

    public override string ToString()
    {
        return Name;
    }
}
