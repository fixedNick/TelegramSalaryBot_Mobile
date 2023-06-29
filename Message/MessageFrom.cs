using System;
using System.Linq;
using System.Collections.Generic;

namespace TelegramSalaryBot.Message;

public struct MessageFrom 
{         
	public long Id;
	public string FirstName;
	public string LastName;
	public string UserName;
	
	public MessageFrom(long id, string firstName, string lastName, string userName)
	{
		Id = id;
		FirstName = firstName;
		LastName = lastName;
		UserName = userName;
	}
}
