using Blue.Agenda.Domain.Interfaces.Servicos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Blue.Agenda.Infra.Servicos
{
    public class ServicoProvedorHttpClient : IServicoProvedorHttpClient
    {
        private readonly IHttpClientFactory _clienteHttp;
        private const string ContentType = "application/json";


        public ServicoProvedorHttpClient(IHttpClientFactory clienteHttp)
        {
            _clienteHttp = clienteHttp;
        }

        public async Task<object> EnviarApi<T>(string uri, T parametros, string uriApi)
        {
            var parametro = new StringContent(JsonConvert.SerializeObject(parametros), Encoding.UTF8, ContentType);
            var retorno = string.Empty;

            try
            {
                var cliente = _clienteHttp.CreateClient("EnvioApi");

                cliente.BaseAddress = new Uri(uriApi);

                HttpResponseMessage resposta = await cliente.PostAsync(uri, parametro);

                if (resposta.StatusCode == HttpStatusCode.BadRequest || resposta.StatusCode == HttpStatusCode.OK)
                {
                    retorno = resposta.Content.ReadAsStringAsync().Result;

                    return await Task.Run(() => JsonConvert.DeserializeObject<object>(retorno));
                }
                else
                {
                    throw new ("A API não está preparada para tratar essa exceção, contate o suporte.");
                }
            }
            catch (Exception ex)
            {
                throw new("Não foi possível enviar o evento de cancelamento, por favor, tente mais tarde.");
            }
        }
    }
}
