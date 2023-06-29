using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSalaryBot.Message;

[Serializable]
public class EmptyMessageException : Exception
{
    public EmptyMessageException() { }
    public EmptyMessageException(string message) : base(message) { }
    public EmptyMessageException(string message, Exception inner) : base(message, inner) { }
    protected EmptyMessageException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

