using System;
using System.Runtime.Serialization;

namespace Blue.Core.Logs.Excecoes
{
    public class ExcecaoNegocio : Exception
    {
        public ExcecaoNegocio()
        {
        }

        public ExcecaoNegocio(string message) : base(message)
        {
        }

        public ExcecaoNegocio(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExcecaoNegocio(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
