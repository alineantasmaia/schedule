using Blue.Core.Entidades.Dtos;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blue.Core.Eventos.Eventos
{
    public class EventoDangerousContainer : InterfaceCabecalhoDto<DangerousContainer>
    {
        public EventoDangerousContainer(DangerousContainer body)
        {
            Body = body;
            Event = "DangerousContainer";
        }

        public EventoDangerousContainer()
        {
            Body = new DangerousContainer();
            Event = "DangerousContainer";
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.Container, "Conteiner", "ERR_N_ENCONTRADO_CONTEINER"));

            Body?.Dangerous?.ForEach(s =>
            {
                s?.Validate();

                if (s != null)
                    AddNotifications(s);
            });

            base.Validate();
        }
    }

    public class DangerousContainer : CorpoEvento
    {
        public DangerousContainer()
        {
            FilaHost = "event.dangerous.container.host";
            FilaTos = "event.dangerous.container.tos";
            Exchange = "event.dangerous.container";
        }

        public string Container { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public List<Dangerou> Dangerous { get; set; }
        public string ImoId { get; set; }
    }
}
