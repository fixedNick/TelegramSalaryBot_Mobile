using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSalaryBot.Database.Exceptions;


[Serializable]
public class SqlNotInitializedException : Exception
{
    public SqlNotInitializedException() { }
    public SqlNotInitializedException(string message) : base(message) { }
    public SqlNotInitializedException(string message, Exception inner) : base(message, inner) { }
    protected SqlNotInitializedException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
