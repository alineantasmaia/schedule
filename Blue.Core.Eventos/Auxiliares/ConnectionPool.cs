using Blue.Core.Entidades.Auxiliares;
using Blue.Core.Entidades.Enums;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blue.Core.Eventos.Auxiliares
{
    internal class ConnectionPool : IDisposable
    {
        internal List<ConnectionDto> PoolConexoes = new List<ConnectionDto>();

        public ConnectionPool(Func<ConfiguracaoRabbitMq, IConnection> criarConexao)
        {
            /*foreach (var config in RegistrarEnvioEventos.PoolConfiguracoes)
            {
                var connection = criarConexao(config);

                PoolConexoes.Add
                (
                    new ConnectionDto
                    {
                        Connection = connection,
                        Model = connection.CreateModel(),
                        VirtualHost = ExtensaoEnum.ObterValorApartirDescricao<VirtualHostEnum>(config.VirtualHost)
                    }
                 );
            }*/
        }

        public IModel GetChannel(string connectionId)
        {
            var conn = ExtensaoEnum.ObterValorApartirDescricao<VirtualHostEnum>(connectionId);

            return PoolConexoes?.FirstOrDefault(p => p.VirtualHost == conn)?.Model;
        }

        public void Dispose()
        {
            foreach (var conn in PoolConexoes)
            {
                conn?.Connection?.Close();
                conn?.Model?.Close();
            }
        }
    }

    internal class ConnectionDto
    {
        public ConnectionDto()
        {

        }

        public ConnectionDto(IConnection connection, IModel model, VirtualHostEnum virtualHost)
        {
            Connection = connection;
            Model = model;
            VirtualHost = virtualHost;
        }

        public IConnection Connection { get; set; }
        public IModel Model { get; set; }
        public VirtualHostEnum VirtualHost { get; set; }
    }
}
