using System.Collections.Generic;

namespace Blue.Core.Logs.Skins.RabbitMq
{
    /// <summary>
    /// Configurações da Exchange/Fila de Logs
    /// </summary>
    public class RabbitMQSkinOptions
    {
        /// <summary>
        /// Exchange que faz o Bind com a fila (RoutingKey)
        /// </summary>
        public string Exchange { get; set; } = string.Empty;
        /// <summary>
        /// RoutingKey em referência a fila(Via Bind com a Exchange ou não)
        /// </summary>
        public string RoutingKey { get; set; } = string.Empty;
        /// <summary>
        /// Dicionario com os campos do Objeto(Key) a serem removidos(Lista dos campos)
        /// <remarks>
        /// Exemplo do Objeto (Com Case Sensitive):
        ///
        ///     "Event":{
        ///         "Campo1":"Teste",
        ///         "Campo2":"Meu valor 
        ///      }
        /// Dicionário:
        ///     "key":"Event,
        ///     "List: Campo1, Campo2
        /// </remarks>
        /// </summary>
        public Dictionary<string, List<string>> RemoverCampos { get; set; } = new Dictionary<string, List<string>>();
    }
}