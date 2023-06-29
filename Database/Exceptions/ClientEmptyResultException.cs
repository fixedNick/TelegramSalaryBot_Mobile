using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSalaryBot.Database.Exceptions;

[Serializable]
public class ClientEmptyResultException : Exception
{
    public ClientEmptyResultException() { }
    public ClientEmptyResultException(string message) : base(message) { }
    public ClientEmptyResultException(string message, Exception inner) : base(message, inner) { }
    protected ClientEmptyResultException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
