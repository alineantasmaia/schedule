using Blue.Core.Eventos.Interfaces;
using Blue.Core.Eventos.Servicos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blue.Core.Eventos.Auxiliares
{
    public class ConnectionRabbitMQ
    {
        private static IServicoEnvioEvento _servicoEnvioEvento;
        private static IServicoEnvioEvento _servicoEnvioEventoHttp;

        public static string ConnectionName;

        public static IServicoEnvioEvento GetInstance()
        {
            if (_servicoEnvioEvento == null)
            {
                //nomeConexao: ConnectionName
                _servicoEnvioEvento = new ServicoEnvioEvento();
            }

            return _servicoEnvioEvento;
        }

        public static IServicoEnvioEvento GetInstanceHttp()
        {
            if (_servicoEnvioEventoHttp == null)
            {
                //false, nomeConexao: ConnectionName
                _servicoEnvioEventoHttp = new ServicoEnvioEvento();
            }

            return _servicoEnvioEventoHttp;
        }

        public static void Dispose()
        {
            if (_servicoEnvioEvento != null)
                _servicoEnvioEvento.Dispose();
        }
    }
}
