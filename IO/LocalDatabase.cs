using System;
using System.Linq;
using System.Collections.Generic;

using TelegramSalaryBot.Client;

namespace TelegramSalaryBot.IO
{
    public class LocalDatabase : IDatabase 
    {         
		public static bool IsClientExist(long tid)
		{
			return TelegramClient.RegisteredAccounts
						.Where(c => c.TelegramID == tid).FirstOrDefault() is null
						? false : true;
		}
		
		public static TelegramClient GetClient(long tid)
		{
			return TelegramClient.RegisteredAccounts
						.Where(c => c.TelegramID == tid).First();
		}
		
		public static void SaveClient(TelegramClient client)
		{
			if(IsClientExist(client.TelegramID))
				throw new Exception("Client already in local database");
			
			TelegramClient.RegisteredAccounts.Add(client);
		}
    }
}