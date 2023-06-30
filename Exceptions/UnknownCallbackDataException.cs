using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSalaryBot.Exceptions;


[Serializable]
public class UnknownCallbackDataException : Exception
{
	public UnknownCallbackDataException() { }
	public UnknownCallbackDataException(string message) : base(message) { }
	public UnknownCallbackDataException(string message, Exception inner) : base(message, inner) { }
	protected UnknownCallbackDataException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
