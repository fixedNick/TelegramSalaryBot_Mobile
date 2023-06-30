using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSalaryBot.Exceptions;

[Serializable]
public class ClientNotFoundException : Exception
{
    public ClientNotFoundException() { }
    public ClientNotFoundException(string message) : base(message) { }
    public ClientNotFoundException(string message, Exception inner) : base(message, inner) { }
    protected ClientNotFoundException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}