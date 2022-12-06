using Blue.Core.Entidades.Dtos;
using Blue.Core.Eventos.Validacoes;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blue.Core.Eventos.Eventos
{
    /// <summary>
    /// Evento que informa ao TOS ou HOST o momento em que é efetuado um movimento de descarga. Este evento é bidirecional, poderá ser 
    /// emitido com a origem Host e também a partir do TOS
    /// </summary>
    public class EventoDischarge : InterfaceCabecalhoDto<Discharge>
    {
        public EventoDischarge(Discharge body)
        {
            Body = body;
            Event = "Discharge";
        }

        public EventoDischarge()
        {
            Body = new Discharge();
            Event = "Discharge";
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.Container, "Conteiner", "ERR_N_ENCONTRADO_CONTEINER")
                .IsNotNullOrEmpty(Body.ActionCode, "ActionCode", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.Category, "Category", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullDateIfNoProcessTypeCancel(ProcessType, Body.DischargeTime, "DischargeTime", "ERR_CAMPO_OBRIGATORIO"));

            Body.ContainerActivity?.ForEach(d =>
            {
                if (!d.ActivityDate.HasValue)
                    d.ActivityDate = Created;

                d.Validate();
                AddNotifications(d);
            });

            base.Validate();
        }
    }

    public class Discharge : CorpoEvento
    {
        public Discharge()
        {
            FilaHost = "event.discharge.host";
            FilaTos = "event.discharge.tos";
            Exchange = "event.discharge";
        }
        public string ActionCode { get; set; }
        public string Container { get; set; }
        public string Category { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public bool Twin { get; set; }
        public bool Vgm { get; set; }
        public string StowageVessel { get; set; }

        public DateTime? DischargeTime { get; set; }
        public List<ContainerActivity> ContainerActivity { get; set; }
    }
}
