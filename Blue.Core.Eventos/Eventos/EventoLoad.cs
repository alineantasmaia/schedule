using Blue.Core.Entidades.Dtos;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blue.Core.Eventos.Eventos
{
    /// <summary>
    /// Evento que informa ao TOS ou HOST o momento em que é realizado um movimento de embarque. Este evento é bidirecional, poderá ser 
    /// emitido com a origem Host e também a partir do TOS.
    /// </summary>
    public class EventoLoad : InterfaceCabecalhoDto<Load>
    {
        public EventoLoad(Load body)
        {
            Body = body;
            Event = "Load";
        }

        public EventoLoad()
        {
            Body = new Load();
            Event = "Load";
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.Container, "Conteiner", "ERR_N_ENCONTRADO_CONTEINER")
                .IsNotNullOrEmpty(Body.ActionCode, "ActionCode", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.VoyageExpBtpId, "VoyageExpBtpId", "ERR_VG_EXP_N_ENCONTRADO")
                .IsTrue(Body.ContainerActivity?.Any() ?? false, "ContainerActivity", "ERR_ATIVIDADE_CTR_N_ENCONTRADO")
                .IsFalse(!"C".Equals(ProcessType) && !Body.LoadTime.HasValue, "LoadTime", "ERR_LOADTIME_N_ENCONTRADO")
                );

            Body.ContainerActivity?.ForEach(d =>
            {
                if (!d.ActivityDate.HasValue && AdmProcess)
                    d.ActivityDate = Created;

                d.Validate();
                AddNotifications(d);
            });

            base.Validate();
        }
    }

    public class Load : CorpoEvento
    {
        public Load()
        {
            FilaHost = "event.load.host";
            FilaTos = "event.load.tos";
            Exchange = "event.load";
        }
        public string ActionCode { get; set; }
        public string Container { get; set; }
        public string Category { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public DateTime? LoadTime { get; set; }
        public bool Twin { get; set; }
        public string StowageVessel { get; set; }
        public List<ContainerActivity> ContainerActivity { get; set; }
    }
}
