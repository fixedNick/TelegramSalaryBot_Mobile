using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using TelegramSalaryBot.Client;

namespace TelegramSalaryBot.IO
{
    public interface IDatabase 
    {         
		static abstract bool IsClientExist(long clientId);
		static abstract TelegramClient GetClient(long clientId);
		static abstract void SaveClient(TelegramClient client);        
    }
}