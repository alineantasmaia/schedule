using Blue.Agenda.Domain.Interfaces.Servicos;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace Blue.Agenda.Infra.Servicos.RabbitMQ
{
    public class ServicoRabbitMq : IServicoRabbitMq
    {
        private IModel _canal;
        private IConnection _conexao;
        public ServicoRabbitMq(IConfiguration configuracao)
        {
            var fabrica = new ConnectionFactory()
            {
                HostName = configuracao["Mensageria:HostName"],
                VirtualHost = configuracao["Mensageria:VirtualHost"],
                UserName = configuracao["Mensageria:Usuario"],
                Password = configuracao["Mensageria:Senha"],
                Port = Convert.ToInt16(configuracao["Mensageria:Porta"])
            };
            _conexao = fabrica.CreateConnection(configuracao["Mensageria:NomeConexao"]);
            CriarCanal();
        }
        public IModel Canal => _canal;

        private void CriarCanal()
        {
            _canal = _conexao.CreateModel();
            _canal.BasicQos(0, 5, false);
        }

        /// <summary>
        /// Metodo para Configurar a fila como durable, nao exclusiva e autoDelete falso, ja vinculada a exchange direct
        /// </summary>
        /// <param name="routingKey">routingKey da fila</param>
        /// <param name="nomeFila">Nome da fila associada a exchange</param>
        /// <param name="exchange">Nome da Exchange</param>
        public void ConfigurarFila(string routingKey, string nomeFila, string exchange)
        {
            _canal.ExchangeDeclare(exchange: exchange, type: "direct", durable: true);
            _canal.QueueDeclare(queue: nomeFila, durable: true, exclusive: false, autoDelete: false);
            _canal.QueueBind(queue: nomeFila,
                exchange: exchange,
                routingKey: routingKey);
        }

        /// <summary>
        /// Metodo para Configurar a fila como durable, nao exclusiva e autoDelete falso, já vinculada a exchange default
        /// </summary>
        /// <param name="nomeFila">Nome da fila</param>
        public void ConfigurarFila(string nomeFila)
        {
            _canal.QueueDeclare(queue: nomeFila, durable: true, exclusive: false, autoDelete: false);
        }

        public void FecharConexao()
        {
            _conexao?.Close();
            _canal?.Close();
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            _canal?.Dispose();
            _conexao?.Dispose();
        }
    }
}
