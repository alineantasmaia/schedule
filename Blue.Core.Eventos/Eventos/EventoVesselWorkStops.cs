using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Validacoes;
using Flunt.Validations;
using System; 

namespace Blue.Core.Eventos.Eventos
{
    public class EventoVesselWorkStops : InterfaceCabecalhoDto<VesselWorkStops>
    {
        public EventoVesselWorkStops()
        {
            Body = new VesselWorkStops();
            Event = "VesselWorkStops";
        }

        public EventoVesselWorkStops(VesselWorkStops body)
        {
            Body = body;
            Event = "VesselWorkStops";
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.VoyageImpBtpId, "VoyageImpBtpId", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.VoyageExpBtpId, "VoyageExpBtpId", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.VesselId, "VesselId", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.StopCodeBtpId, "StopCodeBtpId", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.CheBtpId, "CheBtpId", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.CheType, "CheType", "ERR_CAMPO_OBRIGATORIO")
                .HasMaxLengthIfNotNullOrEmpty(Body.VesselId, 38, "VesselId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.Quantity, 3, "Quantity", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.CheBtpId, 38, "CheBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.CheType, 15, "CheType", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(Body.StopCodeBtpId, "StopCodeBtpId", "ERR_VALOR_N_NUMERO")
                .IsDigitCustom(Body.CheBtpId, "CheBtpId", "ERR_VALOR_N_NUMERO")
            );

            base.Validate();
        }
    }

    public class VesselWorkStops : CorpoEvento
    {
        public VesselWorkStops()
        {
            FilaHost = "event.vessel.work.stops.host";
        }
        public string VesselId { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public string StopCodeBtpId { get; set; }
        public string Quantity { get; set; }
        public string CheBtpId { get; set; }
        public string CheType { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Clear { get; set; }
    }
}
