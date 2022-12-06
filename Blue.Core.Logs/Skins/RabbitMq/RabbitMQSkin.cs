using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace Blue.Core.Logs.Skins.RabbitMq
{
    /// <summary>
    /// Skin para publicar o log no RabbitMq
    /// </summary>
    internal class RabbitMqSkin : ILogEventSink, IDisposable
    {
        private readonly IConnection _connection;
        private readonly RabbitMQSkinOptions _rabbitMqSkinOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rabbitMqSkinOptions"></param>
        /// <param name="connection"></param>
        public RabbitMqSkin( 
            RabbitMQSkinOptions rabbitMqSkinOptions, 
            IConnection connection)
        {
            
            _rabbitMqSkinOptions = rabbitMqSkinOptions;
            _connection = connection;
        }
        
        /// <summary>
        /// Método chamando o pipeline de ILogEventSink quando o log na aplicação é disparado
        /// </summary>
        /// <param name="event"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Emit(LogEvent @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            if (_connection == null) throw new ArgumentNullException(nameof(_connection));
            if (_rabbitMqSkinOptions == null) throw new ArgumentNullException(nameof(_rabbitMqSkinOptions));

            var logFormatado = TransformarLog(@event);
            if (!_connection.IsOpen)
            {
                Console.WriteLine("**** [ERRO LOGS] - Conexão não está aberta - !****");
                Console.WriteLine($"\n {logFormatado}");
                return;
            }
            
            try
            {
                //var objeto = RemoverPropriedades(logFormatado);
                //if(objeto != null)
                logFormatado = JsonConvert.SerializeObject(logFormatado, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                using (var channel = _connection.CreateModel())
                {
                    channel.BasicPublish(_rabbitMqSkinOptions.Exchange, _rabbitMqSkinOptions.RoutingKey,
                        PropriedadesFila(channel),
                        Encoding.UTF8.GetBytes(logFormatado));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($" [ERRO LOGS] - Não foi possível publicar a mensagem de Log. Erro->: {ex?.Message} - {ex?.InnerException}");
                Console.WriteLine($"\n {logFormatado}");
            }
        }

        /// <summary>
        /// Método chamando o pipeline de ILogEventSink quando o log na aplicação é disparado
        /// </summary>
        /// <param name="event"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private JObject RemoverPropriedades(string logFormatado)
        {
            var json = (JObject) JsonConvert.DeserializeObject(logFormatado, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            json.Property("Timestamp")?.Remove();
            json.Property("MessageTemplate")?.Remove();
            json.Property("Level")?.Remove();

            JObject objetoLog = null;
            if (json.Properties().Any(c => c.Name == "Properties"))
            {
                objetoLog = (JObject)json["Properties"];
                if (objetoLog.Properties().Any(c => c.Name == "log"))
                {
                    objetoLog = (JObject)objetoLog["log"];
                    objetoLog?.Property("_typeTag")?.Remove();
                }

                if (objetoLog.Properties().Any(c => c.Name == "Event"))
                {
                    var objetoEvent = (JObject) objetoLog["Event"];
                    objetoEvent?.Property("_typeTag")?.Remove();
                    objetoEvent?.Property("Reprocessing")?.Remove();
                    objetoEvent?.Property("Notificacoes")?.Remove();
                    objetoEvent?.Property("Notifications")?.Remove();
                    objetoEvent?.Property("Invalid")?.Remove();
                    objetoEvent?.Property("Valid")?.Remove();

                    if (objetoEvent.Properties().Any(c => c.Name == "Body"))
                    {
                        var objetoBody = (JObject)objetoEvent["Body"];
                        objetoBody?.Property("_typeTag")?.Remove();
                        objetoBody?.Property("FilaTos")?.Remove();
                        objetoBody?.Property("FilaHost")?.Remove();
                        objetoBody?.Property("Exchange")?.Remove();
                    }
                }

                foreach (var dado in _rabbitMqSkinOptions.RemoverCampos)
                {
                    if (objetoLog.Properties().Any(c => c.Name == dado.Key))
                    {
                        var objetoRemover = (JObject) objetoLog[dado.Key];
                        foreach (var item in dado.Value)
                            objetoRemover?.Property(item)?.Remove();
                    }
                }
            }
            return objetoLog ?? json;
        }
        private string TransformarLog(LogEvent @event)
        {
            var stringWriter = new StringWriter();
            new JsonFormatter().Format(@event, stringWriter);

            return stringWriter.ToString();
        }
        private IBasicProperties PropriedadesFila(IModel canal)
        {
            IBasicProperties props = canal.CreateBasicProperties();
            props.ContentType = "application/json";
            props.DeliveryMode = 2;
            return props;
        }
        
        /// <summary>
        /// Dispose do Objeto
        /// </summary>
        public void Dispose()
        {
            if(_connection.IsOpen)
                _connection?.Close();

            _connection?.Dispose();
        }
    }
}