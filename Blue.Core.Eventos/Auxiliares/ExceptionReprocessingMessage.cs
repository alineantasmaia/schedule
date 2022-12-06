using System;
using System.Collections.Generic;
using System.Text;

namespace Blue.Core.Eventos.Auxiliares
{
    public class ExceptionReprocessingMessage : Exception
    {
        public ExceptionReprocessingMessage(string message) : base(message)
        {
        }
    }
}
