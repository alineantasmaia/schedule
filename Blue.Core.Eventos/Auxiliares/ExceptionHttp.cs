using System;
using System.Collections.Generic;
using System.Text;

namespace Blue.Core.Eventos.Auxiliares
{
    public class ExceptionHttp : Exception
    {
        public List<string> Notifications { get; set; }

        public ExceptionHttp(string message, List<string> notifications)
        {
            Notifications = notifications;
        }
    }
}
