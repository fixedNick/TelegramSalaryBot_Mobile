using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSalaryBot.Exceptions;

[Serializable]
public class UndefinedRequestStepException : Exception
{
	public UndefinedRequestStepException() { }
	public UndefinedRequestStepException(string message) : base(message) { }
	public UndefinedRequestStepException(string message, Exception inner) : base(message, inner) { }
	protected UndefinedRequestStepException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
