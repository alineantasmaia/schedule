using Blue.Core.Entidades.Dtos;
using Blue.Core.Eventos.Auxiliares;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Blue.Core.Eventos.Eventos
{
    public class EventoLogs : CorpoEvento
    {
        public EventoLogs() =>
            ConfigurarFila();

        public EventoLogs(DateTime eventoRecebido)
        {
            Received = eventoRecebido;
            ConfigurarFila();
        }

        public EventoLogs(DateTime eventoRecebido, string eventoJson)
        {
            Received = eventoRecebido;
            EventoJson = eventoJson;
            ConfigurarFila();
        }

        public string Owner { get; set; } = "HOST";
        public List<string> Remarks { get; set; } = new List<string>();
        public bool Success { get; set; } = true;
        public DateTime Received { get; set; }
        public DateTime Processed { get; set; }
        public bool Reprocessing { get; set; }
        public JObject OriginalMsg { get; set; }
        public bool Excecao { get; set; }
        /// <summary>
        /// Evento em formato de string para ser utilizado quando ocorrer erro de conversão do evento enviado pelo TOS,
        /// desta forma o evento enviado estará no log para posterior analise
        /// </summary>
        private string EventoJson { get; set; }
        public void EventoReprocessado() => Reprocessing = true;
        public void EventoRecebido() => Received = DateTime.Now;
        public void EventoProcessado() => Processed = DateTime.Now;
        public void ConfigurarExcecao() => Excecao = true;
        public bool HouveExcecao() => Excecao;
        
        public void AdicionarMensagemOriginal<T>(InterfaceCabecalhoDto<T> evento) where T : CorpoEvento
        {
            if (evento == null)
            {
                AdicionarRemark($"JSON Inválido - {EventoJson}");
                return;
            }

            OriginalMsg = JObject.FromObject(new
            {
                evento.Id,
                evento.Event,
                evento.Message,
                evento.ProcessType,
                evento.Created,
                evento.User,
                evento.IpMachine,
                evento.AdmProcess,
                evento.Body
            }, new JsonSerializer { ContractResolver = Serializacao.Configuracao(), ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public void AdicionarRemarks(List<string> notificacoes)
        {
            Remarks = notificacoes;
            Success = false;
        }

        public void AdicionarRemark(string notificacao)
        {
            Remarks.Add(notificacao);
            Success = false;
        }

        private void ConfigurarFila()
        {
            FilaHost = "event.log.host";
            FilaTos = "event.log.tos";
            Exchange = "event.log.host.fanout";
        }
    }
}