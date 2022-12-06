using RabbitMQ.Client;
using System;

namespace Blue.Agenda.Domain.Interfaces.Servicos
{
    public interface IServicoRabbitMq : IDisposable
    {
        IModel Canal { get; }
        void ConfigurarFila(string nomeFila);
        void ConfigurarFila(string routingKey, string nomeFila, string exchange);
        void FecharConexao();
    }
}
