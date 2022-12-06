using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Validacoes;
using Flunt.Validations;

namespace Blue.Core.Eventos.Eventos
{
    public class EventoWeightContainer : InterfaceCabecalhoDto<WeightContainer>
    {
        public EventoWeightContainer(WeightContainer body)
        {
            Body = body;
            Event = "WeightContainer";
        }

        public EventoWeightContainer()
        {
            Body = new WeightContainer();
            Event = "WeightContainer";
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.Container, "Container", "ERR_CAMPO_OBRIGATORIO")
                .HasMaxLengthIfNotNullOrEmpty(Body.Container, 12, "Container", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.VerifiedGrossWeight, 38, "VerifiedGrossWeight", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.ManifestGrossWeight, 38, "ManifestGrossWeight", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.VgmWeight, 38, "VgmWeight", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(Body.VerifiedGrossWeight, "VerifiedGrossWeight", "ERR_VALOR_N_NUMERO")
                .IsDigitCustom(Body.ManifestGrossWeight, "ManifestGrossWeight", "ERR_VALOR_N_NUMERO")
                .IsDigitCustom(Body.VgmWeight, "VgmWeight", "ERR_VALOR_N_NUMERO"));

            base.Validate();
        }
    }

    public class WeightContainer : CorpoEvento
    {
        public WeightContainer()
        {
            FilaHost = "event.weight.container.host";
            FilaTos = "event.weight.container.tos";
            Exchange = "event.weight.container";
        }

        public string Container { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public string VgmWeight { get; set; }
        public string ManifestGrossWeight { get; set; }
        public string VerifiedGrossWeight { get; set; }
        public string CheBtpId { get; set; }
    }
}
