using Blue.Core.Entidades.Auxiliares;
using Blue.Core.Entidades.Enums;
using Flunt.Notifications;
using Flunt.Validations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blue.Core.Entidades.Dtos
{
    public class InterfaceCabecalhoDto<T> : Notifiable, IValidatable where T : CorpoEvento
    {
        [SwaggerIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Event { get; set; }
        public string ProcessType { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string User { get; set; }
        public string IpMachine { get; set; }
        public bool AdmProcess { get; set; }
        public T Body { get; set; }

        [JsonIgnore]
        public bool Reprocessing { get; private set; }
        [JsonIgnore]
        public Dictionary<string, object> Argumentos { get; set; } = new Dictionary<string, object>();
        /// <summary>
        /// Propriedade que indica se a publicação do evento deve ser feita via HTTP
        /// </summary>
        [InterfaceIgnore]
        [SwaggerIgnore]
        public bool PublishAsHttp { get; set; }
        /// <summary>
        /// Propriedade que indica se o evento está sendo consumido via HTTP
        /// </summary>
        [InterfaceIgnore]
        [SwaggerIgnore]
        public bool ConsumeAsHttp { get; set; }
        public virtual bool Insert() => "I".Equals(ProcessType);

        public virtual bool Update() => "U".Equals(ProcessType);

        public virtual bool Cancel() => "C".Equals(ProcessType);

        public virtual bool Delete() => "D".Equals(ProcessType);

        public void ConfigurarConsumoHttp() => ConsumeAsHttp = true;

        public void ConfigurarPublicacaoHttp() => PublishAsHttp = true;

        public bool Valido()
        {
            Validate();
            return Valid;
        }
        /// <summary>
        /// Método responsável por obter um virtual com base no namespace para cada integração 
        /// </summary>
        /// <returns>Nome do virtual host para ser usado no momento de criar a conexão com RabbitMQ</returns>
        public string ObterVirtualHost()
        {
            var namespaceEvento = this.Body.GetType().Namespace;

            switch (namespaceEvento)
            {                
                case string nome when nome.Contains("BLUE"):
                    return VirtualHostEnum.BFS.ObterDescricao();

                default:
                    return VirtualHostEnum.INTERFACETOS.ObterDescricao();
            }
        }

        public void Reprocessar(string user)
        {
            User = user;
            Reprocessing = true;
            AdmProcess = true;
        }
        public virtual void Validate()
        {
            AddNotifications(new Contract()
               .Requires()
               .IsNotNullOrEmpty(User, "Usuário", "ERR_N_ENCONTRADO_USUARIO")
               .IsNotNullOrEmpty(ProcessType, "Tipo processo", "ERR_N_TIPO_PROCESSO")
               .IsNotEmpty(Id, "Id Evento", "ERR_N_ID_EVENTO")
               .IsNotNullOrEmpty(IpMachine, "Ip Máquina", "ERR_N_IP_EVENTO")
               .IsNotNullOrEmpty(Event, "Nome evento", "ERR_N_NOME_EVENTO"));
        }

        [JsonIgnore]
        public List<string> Notificacoes => Notifications.Select(p => p.Message).ToList();
        
    }

    public class CorpoEvento
    {
        [JsonIgnore]
        public string FilaTos { get; protected set; }
        [JsonIgnore]
        public string FilaHost { get; protected set; }
        [JsonIgnore]
        public string Exchange { get; protected set; } = null;
        [JsonIgnore]
        public Dictionary<string, object> Headers { get; set; } = new Dictionary<string, object>();
    }

}
