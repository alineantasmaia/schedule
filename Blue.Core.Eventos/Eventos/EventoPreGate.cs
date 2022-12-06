using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Validacoes;
using Flunt.Notifications;
using Flunt.Validations;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Blue.Core.Eventos.Eventos
{

    /// <summary>
    /// Evento que informa ao TOS o momento em que o caminhão passar pelo Pré Gate, após esse evento o caminhão estará na Parking Area indo em direção ao Gate 
    /// </summary>
    public class EventoPreGate : InterfaceCabecalhoDto<PreGate>
    {
        public EventoPreGate(PreGate body)
        {
            Body = body;
            Event = "PreGate";
        }

        public EventoPreGate()
        {
            Body = new PreGate();
            Event = "PreGate";
        }

        public override void Validate()
        {

            AddNotifications(new Contract()
            .Requires()
            .HasMaxLengthIfNotNullOrEmpty(Body.TruckNumber, 8, "TruckNumber", "ERR_NUMERO_TRUCK_N_ENCONTRADO"));

            Body.VisitCodes.ForEach(s =>
            {
                s.Validate();
                AddNotifications(s);
            });

            base.Validate();

        }
    }

    public class PreGate : CorpoEvento
    {
        public PreGate()
        {
            FilaTos = "event.pre.gate.tos";
        }
        public string TruckNumber { get; set; }
        public List<VisitCode> VisitCodes { get; set; }
    }

    public class VisitCode : Notifiable, IValidatable
    {
        [JsonProperty("visitCode")]
        public string Number { get; set; }
        

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsDigitCustom(Number, "VisitCode", "ERR_VALOR_N_NUMERO"));
        }
    }
}
