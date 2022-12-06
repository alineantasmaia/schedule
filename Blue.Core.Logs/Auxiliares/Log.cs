using Blue.Core.Logs.Enumeradores;
using System;

namespace Blue.Core.Logs.Auxiliares
{
    public abstract class Log
    {
        protected Log(string mensagem, string detalhes)
        {
            Mensagem = mensagem;
            Detalhes = detalhes;
            Ocorrencia = DateTime.Now;
        }

        public DateTime Ocorrencia { get; private set; }
        public string Mensagem { get; private set; }
        public string Detalhes { get; private set; }
        public EnumTipoLog TipoLog { get; protected set; }
    }
}
