using Flunt.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blue.Core.Eventos.Auxiliares
{
    /// <summary>
    /// Exception tem o objetivo de adicionar ao stackTrace detalhes de quais validações de erros que ocorreram na publicação dos eventos,
    /// com isso irá facilitar o desenvolver a identificar o que houve de errado na chamada da classe ServicoEnvioEvento.cs
    /// </summary>
    public class ExcecaoEvento : Exception
    {
        private readonly string stackTrace;
        public ExcecaoEvento(string mensagem, IReadOnlyCollection<Notification> notificacoes) : base(mensagem)
        {
            stackTrace = string.Join(",", notificacoes?.Select(x => $"\n [{x.Property} - {x.Message}]"));
        }

        public override string StackTrace => $"{base.StackTrace} \n ========== Notificações de Validações dos Eventos ========== {stackTrace}";
    }
}
