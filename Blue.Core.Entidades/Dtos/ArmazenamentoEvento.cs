using Blue.Core.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Text;


namespace Blue.Core.Entidades.Dtos
{
    public class ArmazenamentoEvento
    {
        public string Codigo { get; private set; }
        public string Evento { get; private set; }
        public string Conteudo { get; private set; }
        public DestinoEventoEnum Destino { get; private set; } = DestinoEventoEnum.P_HOST;
        public StatusEventoEnum Status { get; private set; }
        public DateTime? DataReprocessamento { get; private set; }
        public bool Reprocessamento { get; private set; } = false;
        public string Usuario { get; private set; }

        public ArmazenamentoEvento()
        {

        }

        public static ArmazenamentoEvento Criar<T>(InterfaceCabecalhoDto<T> evento) where T : CorpoEvento
        {
            return new ArmazenamentoEvento
            {
                Codigo = evento.Id.ToString(),
                Evento = evento.Event,
                Conteudo = Newtonsoft.Json.JsonConvert.SerializeObject(evento),
                Reprocessamento = evento.Reprocessing
            };
        }
    }
}
