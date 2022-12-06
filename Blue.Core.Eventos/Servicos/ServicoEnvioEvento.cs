using Blue.Core.Entidades.Auxiliares;
using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Enums;
using Blue.Core.Eventos.Auxiliares;
using Blue.Core.Eventos.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Blue.Core.Eventos.Servicos
{
    public class ServicoEnvioEvento : IServicoEnvioEvento
    {
        private readonly ConnectionPool _connectionPool;
        private readonly ConfiguracaoRabbitMq _configuracaoRabbitMq;
        private IConnection conexao;
        private string connectionId;
        private object lockCanal = new object();

        private IModel canal;

        public IModel Canal
        {
            get { return RegistrarEnvioEventos.MultiConexoes ? _connectionPool.GetChannel(connectionId) : canal; }
            set { canal = value; }
        }

        public ServicoEnvioEvento(bool criarNovaConexao = true, string nomeConexao = "")
        {
            if (!criarNovaConexao)
            {
                _configuracaoRabbitMq = RegistrarEnvioEventos.Configuracao;
                return;
            }

            if (RegistrarEnvioEventos.MultiConexoes)
                _connectionPool = new ConnectionPool(CriarComunicacao);

            else
            {
                _configuracaoRabbitMq = RegistrarEnvioEventos.Configuracao;
                conexao = CriarComunicacao(_configuracaoRabbitMq);
                Canal = conexao.CreateModel();
            }
        }

        private IConnection CriarComunicacao(ConfiguracaoRabbitMq configuracao)
        {
            var fabrica = new ConnectionFactory()
            {
                HostName = configuracao.HostName,
                VirtualHost = configuracao.VirtualHost,
                UserName = configuracao.Usuario,
                Password = configuracao.Senha,
                Port = configuracao.Porta
            };

            return fabrica.CreateConnection(configuracao.NomeConexao);

        }

        public void PublicarHost<T>(InterfaceCabecalhoDto<T> evento) where T : CorpoEvento
        {
            if (!evento.Valido())
                throw new ExcecaoEvento($"Evento {evento.Event} não está atendendo todas as regras de validações, não foi possível publicar evento", evento.Notifications);

            if (evento.PublishAsHttp)
            {
                PublicarViaHttp(evento, DestinoEventoEnum.P_HOST);
            }
            else
            {
                lock (lockCanal)
                {
                    connectionId = evento.ObterVirtualHost();
                    EnviarMensagem(evento, true);
                }
            }
        }

        public void PublicarTos<T>(InterfaceCabecalhoDto<T> evento) where T : CorpoEvento
        {
            if (!evento.Valido())
                throw new ExcecaoEvento($"Evento {evento.Event} não está atendendo todas as regras de validações, não foi possível publicar o evento", evento.Notifications);

            if (evento.PublishAsHttp)
            {
                PublicarViaHttp(evento, DestinoEventoEnum.P_TOS);
            }
            else
            {
                lock (lockCanal)
                {
                    connectionId = evento.ObterVirtualHost();
                    EnviarMensagem(evento);
                }
            }
        }

        public void PublicarTosHost<T>(InterfaceCabecalhoDto<T> evento) where T : CorpoEvento
        {
            if (!evento.Valido())
                throw new ExcecaoEvento($"Evento {evento.Event} não está atendendo todas as regras de validações, não foi possível publicar o evento", evento.Notifications);

            if (evento.PublishAsHttp)
            {
                PublicarViaHttp(evento, DestinoEventoEnum.P_TOS_HOST);
            }
            else
            {
                lock (lockCanal)
                {
                    connectionId = evento.ObterVirtualHost();
                    EnviarMensagemTosHost(evento);
                }
            }
        }

        public void PublicarTosHost<T>(T evento) where T : CorpoEvento
        {
            CriarBindFanout(evento.FilaHost, evento.FilaTos, evento.Exchange);

            var eventoSerializado = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evento, RegistrarEnvioEventos.JsonSettings));

            Canal.BasicPublish(exchange: evento.Exchange, string.Empty, basicProperties: PropriedadesFila(), body: eventoSerializado);
        }

        private void PublicarViaHttp<T>(InterfaceCabecalhoDto<T> evento, DestinoEventoEnum destino) where T : CorpoEvento
        {
            var retorno = string.Empty;
            var resultado = new ResultadoApi();

            const string uri = "eventos-xx";
            const string tipoConteudo = "text/plain";

            if (!evento.Valido())
                throw new ExcecaoEvento($"Evento {evento.Event} não está atendendo todas as regras de validações, não foi possível publicar o evento", evento.Notifications);

            using (var clienteHttp = new HttpClient())
            {
                var parametro = new StringContent(JsonConvert.SerializeObject(evento, RegistrarEnvioEventos.JsonSettings),
                    Encoding.UTF8, tipoConteudo);

                clienteHttp.DefaultRequestHeaders.Add("X-Destino", destino.ObterDescricao());
                parametro.Headers.ContentType = new MediaTypeHeaderValue(tipoConteudo);

                clienteHttp.BaseAddress = new Uri($"http://{_configuracaoRabbitMq.UrlIntegracaoEvento}/");

                HttpResponseMessage resposta = clienteHttp.PostAsync(uri, parametro).Result;

                retorno = resposta.Content.ReadAsStringAsync().Result;

                resultado = JsonConvert.DeserializeObject<ResultadoApi>(retorno);

                if (!resultado.Sucesso)
                    throw new ExceptionHttp("Erro ao tentar publicar evento via HTTP", resultado.Notificacoes);
            }
        }

        private void EnviarMensagemTosHost<T>(InterfaceCabecalhoDto<T> evento) where T : CorpoEvento
        {
            CriarBindFanout(evento.Body.FilaHost, evento.Body.FilaTos, evento.Body.Exchange);

            var eventoSerializada = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evento, RegistrarEnvioEventos.JsonSettings));

            Canal.BasicPublish(exchange: evento.Body.Exchange, string.Empty, basicProperties: PropriedadesFila(), body: eventoSerializada);
        }

        public void PublicarAde<T>(InterfaceCabecalhoDto<T> evento) where T : CorpoEvento
        {
            if (!evento.Valido())
                throw new ExcecaoEvento($"Evento {evento.Event} não está atendendo todas as regras de validações, não foi possível publicar evento para o HOST", evento.Notifications);

            EnviarMensagem(evento, true);
        }

        private void EnviarMensagem<T>(InterfaceCabecalhoDto<T> evento, bool host = false) where T : CorpoEvento
        {
            var fila = host ? evento.Body.FilaHost : evento.Body.FilaTos;
            var exchange = "amq.direct";

            CriarBindDirect(fila, exchange, evento.Argumentos);

            var jsonSerealizado = JsonConvert.SerializeObject(evento, RegistrarEnvioEventos.JsonSettings);

            var eventoSerializada = Encoding.UTF8.GetBytes(jsonSerealizado);

            var propriedades = PropriedadesFila();
            propriedades.Headers = evento.Body.Headers;

            if (evento.Body.Headers.ContainsKey("priority"))
                propriedades.Priority = Convert.ToByte(evento.Body.Headers["priority"].ToString());

            Canal.BasicPublish(exchange: exchange, routingKey: fila, basicProperties: propriedades, body: eventoSerializada);
        }

        private void CriarBindDirect(string fila, string exchange, Dictionary<string, object> argumentos = null)
        {
            if (string.IsNullOrEmpty(fila))
                throw new Exception($"A fila devem ser configuradas");

            Canal.QueueDeclare(queue: fila, durable: true, exclusive: false, autoDelete: false, arguments: argumentos);
            Canal.ExchangeDeclare(exchange: exchange, type: "direct", durable: true, autoDelete: false);

            Canal.QueueBind(queue: fila, exchange, routingKey: fila);
        }

        private void CriarBindFanout(string filaHost, string filaTos, string exchange)
        {
            if (string.IsNullOrEmpty(exchange))
                throw new ArgumentNullException(exchange, $"Para publicar o evento uma exchange deve ser configurada");

            if (string.IsNullOrEmpty(filaHost))
                throw new ArgumentNullException(filaHost, $"As filas devem ser configuradas");

            if (string.IsNullOrEmpty(filaTos))
                throw new ArgumentNullException(filaTos, $"As filas devem ser configuradas");

            Canal.QueueDeclare(queue: filaHost, durable: true, exclusive: false, autoDelete: false);
            Canal.QueueDeclare(queue: filaTos, durable: true, exclusive: false, autoDelete: false);

            Canal.ExchangeDeclare(exchange: exchange, type: "fanout", durable: true);
            Canal.QueueBind(queue: filaHost, exchange: exchange, routingKey: string.Empty);

            Canal.ExchangeDeclare(exchange: exchange, type: "fanout", durable: true);
            Canal.QueueBind(queue: filaTos, exchange: exchange, routingKey: string.Empty);
        }

        private IBasicProperties PropriedadesFila()
        {
            IBasicProperties props = Canal.CreateBasicProperties();
            props.ContentType = "application/json";
            props.DeliveryMode = 2;
            return props;
        }

        private void FecharConexao()
        {
            conexao?.Close();
            canal?.Close();
            _connectionPool?.Dispose();
        }

        

        public void CanalComunicacao(IModel canalComunicacao) =>
            canal = canalComunicacao;
                

        public void Dispose()
        {
            FecharConexao();
        }

    }
}
