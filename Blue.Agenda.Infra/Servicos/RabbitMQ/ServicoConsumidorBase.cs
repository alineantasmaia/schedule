using Blue.Agenda.Domain.Interfaces.Servicos;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blue.Agenda.Infra.Servicos.RabbitMQ
{
    public abstract class ServicoConsumidorBase : IHostedService, IDisposable
    {
        private readonly IServicoRabbitMq _servicoRabbitMq;

        protected ServicoConsumidorBase(
            IServicoRabbitMq servicoRabbitMq)
        {
            _servicoRabbitMq = servicoRabbitMq;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
#if !DEBUG
            RegistrarConsumidor("");
#endif
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _servicoRabbitMq.FecharConexao();
            return Task.CompletedTask;
        }

        public abstract Task<bool> Processar(byte[] message);

        public void RegistrarConsumidor(string nomeFila)
        {
            _servicoRabbitMq.ConfigurarFila(nomeFila);

            var consumidor = new EventingBasicConsumer(_servicoRabbitMq.Canal);
            consumidor.Received += (model, ea) =>
            {
                Processar(ea.Body.ToArray());
                _servicoRabbitMq.Canal.BasicAck(ea.DeliveryTag, false);
            };

            _servicoRabbitMq.Canal.BasicConsume(queue: nomeFila, consumer: consumidor, autoAck: false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            _servicoRabbitMq.Dispose();
        }
    }
}
