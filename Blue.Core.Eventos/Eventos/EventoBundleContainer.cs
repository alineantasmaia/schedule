using Blue.Core.Entidades.Dtos;
using Blue.Core.Eventos.Eventos.DischargeInformationContainer;
using Flunt.Notifications;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blue.Core.Eventos.Eventos
{
    public class EventoBundleContainer : InterfaceCabecalhoDto<BundleContainer>
    {
        public EventoBundleContainer(BundleContainer body)
        {
            Body = body;
            Event = "BundleContainer";
        }

        public EventoBundleContainer()
        {
            Body = new BundleContainer();
            Event = "BundleContainer";
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.Container, "Conteiner", "ERR_N_ENCONTRADO_CONTEINER")
                .HasMaxLengthIfNotNullOrEmpty(Body.Container, 12, "Conteiner", "ERR_N_CONTAINER")
                .HasMaxLengthIfNotNullOrEmpty(Body.VoyageImpBtpId, 38, "VoyageImpBtpId", "ERR_N_VOYAGEIMPBTPID")
                .HasMaxLengthIfNotNullOrEmpty(Body.VoyageExpBtpId, 38, "VoyageExpBtpId", "ERR_N_VOYAGEEXPBTPID"));

            Body.Position.Validate();
            AddNotifications(Body.Position);

            Body.Slaves.ForEach(s =>
            {
                s.Validate();
                AddNotifications(s);
            });

            base.Validate();
        }
    }

    public class BundleContainer : CorpoEvento
    {
        public BundleContainer()
        {
            FilaHost = "event.bundle.container.host";
            FilaTos = "event.bundle.container.tos";
            Exchange = "event.bundle.container";
        }

        public string Container { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public Position Position { get; set; }
        public List<Slaves> Slaves { get; set; }
    }

    public class Slaves : Notifiable, IValidatable
    {
        public string Container { get; set; }
        public string Tier { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .HasMaxLengthIfNotNullOrEmpty(Container, 12, "Conteiner", "ERR_N_CONTAINER")
                .HasMaxLengthIfNotNullOrEmpty(Tier, 12, "Tier", "ERR_N_TIER"));
        }
    }
}
