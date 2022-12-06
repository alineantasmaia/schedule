using Blue.Core.Eventos.Eventos;
using Blue.Core.Eventos.Interfaces;
using Blue.Core.Logs.Excecoes;
using Blue.Agenda.Domain.Dtos;
using Blue.Agenda.Domain.Interfaces.Servicos;
using Blue.Agenda.Infra.Servicos.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blue.Agenda.Infra.Servicos
{
    public class ConsumirFila : ServicoConsumidorBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServicoEnvioEvento _servicoEnvioEvento;
        private IServicoTipoErro<Evento> _servicoTipoErro;

        public ConsumirFila(
            IServicoRabbitMq servicoRabbitMq,
            IServiceProvider serviceProvider,
            IServicoEnvioEvento servicoEnvioEvento) : base(servicoRabbitMq)
        {
            _serviceProvider = serviceProvider;
            _servicoEnvioEvento = servicoEnvioEvento;

            _servicoEnvioEvento.CanalComunicacao(servicoRabbitMq.Canal);
        }
        public override async Task<bool> Processar(byte[] mensagem)
        {
            var eventoLogs = new EventoLogs(DateTime.Now);
            var evento = default(Evento);

            try
            {
                using (var escopo = _serviceProvider.CreateScope())
                {
                    var servico = escopo.ServiceProvider.GetRequiredService<IServicoEvento>();
                    _servicoTipoErro = escopo.ServiceProvider.GetRequiredService<IServicoTipoErro<Evento>>();

                    evento = JsonConvert.DeserializeObject<Evento>(Encoding.UTF8.GetString(mensagem));

                    await servico.Executar(evento);
                }
            }
            catch (Exception excecao)
            {
                eventoLogs.ConfigurarExcecao();
                _servicoTipoErro.AddErros(excecao.TratarExcecao());
            }
            finally
            {
                EnviarEventoLogs(eventoLogs, evento);
            }

            return await Task.Run(() => true);
        }

        private void EnviarEventoLogs(EventoLogs eventoLogs, Evento evento)
        {
            eventoLogs.EventoProcessado();
            eventoLogs.AdicionarMensagemOriginal(evento);

            if (_servicoTipoErro.Invalido)
                eventoLogs.AdicionarRemarks(_servicoTipoErro.Notificacoes.Select(s => $"{s.Titulo} - {s.Mensagem} - {s.TipoNotificacao}").ToList());

            _servicoEnvioEvento.PublicarTosHost(eventoLogs);
        }
    }
}
