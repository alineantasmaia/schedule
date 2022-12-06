using Blue.Core.Logs.Skins.RabbitMq;
using RabbitMQ.Client;
using Serilog;
using Serilog.Configuration;

namespace Blue.Core.Logs.Skins.Extensions
{
    /// <summary>
    /// SkinExtensions para RabbiMQ reutilizando a mesma conexão
    /// </summary>
    public static class SkinExtensions
    {
        /// <summary>
        /// SkinExtensions para RabbiMQ reutilizando a mesma conexão
        /// <param name="options">Configuração de Exchange e Fila para envio do log</param>
        /// <param name="connection">Conexao com o RabbitMQ</param>
        /// </summary>
        public static LoggerConfiguration RabbitMQ(this LoggerSinkConfiguration loggerConfiguration,
            RabbitMQSkinOptions options,
            IConnection connection)
        {
            return loggerConfiguration.Sink(new RabbitMqSkin(options, connection));
        }
    }
}