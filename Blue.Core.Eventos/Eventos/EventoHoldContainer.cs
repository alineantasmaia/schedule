using Blue.Core.Entidades.Dtos;
using Flunt.Validations;
using System;

namespace Blue.Core.Eventos.Eventos
{
    /// <summary>
    /// Evento que informa ao TOS ou HOST o momento em que é realizado um bloqueio no container. Este evento é bidirecional, poderá ser 
    /// emitido com a origem Host e também a partir do TOS
    /// </summary>
    public class EventoHoldContainer : InterfaceCabecalhoDto<HoldContainer>
    {
        public EventoHoldContainer(HoldContainer body)
        {
            Body = body;
            Event = "HoldContainer";
        }

        public EventoHoldContainer()
        {
            Body = new HoldContainer();
            Event = "HoldContainer";
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.Container, "Container", "ERR_N_ENCONTRADO_CONTEINER")
                .IsNotNullOrEmpty(Body.HoldCodeBtpId, "HoldCodeBtpId", "ERR_N_ENCONTRADO_BLOQUEIO"));

            base.Validate();
        }
    }

    public class HoldContainer : CorpoEvento
    {
        public HoldContainer()
        {
            FilaHost = "event.hold.container.host";
            FilaTos = "event.hold.container.tos";
            Exchange = "event.hold.container";
        }

        public string Container { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public string HoldCodeBtpId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Clear { get; set; }
    }
}
