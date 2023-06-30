using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSalaryBot.Exceptions;


[Serializable]
public class UnknownMessageCommandException : Exception
{	
	public UnknownMessageCommandException() { }
	public UnknownMessageCommandException(string message) : base(message) { }
	public UnknownMessageCommandException(string message, Exception inner) : base(message, inner) { }
	protected UnknownMessageCommandException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
