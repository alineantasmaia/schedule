using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Validacoes;
using Flunt.Validations;
using System;

namespace Blue.Core.Eventos.Eventos
{
    /// <summary>
    /// Evento que informa ao TOS ou HOST o momento em que é realizado a confirmação da saída de um caminhão externo do terminal. Este evento é bidirecional, poderá ser 
    /// emitido com a origem Host e também a partir do TOS
    /// </summary>
    public class EventoGateOut : InterfaceCabecalhoDto<GateOut>
    {
        public EventoGateOut(GateOut body)
        {
            Body = body;
            Event = "GateOut";
        }

        public EventoGateOut()
        {
            Body = new GateOut();
            Event = "GateOut";
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.VisitCode, "VisitCode", "ERR_CAMPO_OBRIGATORIO")
                .HasMaxLengthIfNotNullOrEmpty(Body.Container, 12, "Container", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.VisitCode, 38, "VisitCode", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.TruckWeight, 15, "TruckWeight", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.GateNumber, 15, "GateNumber", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.DriverId, 15, "DriverId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.TruckPlate, 8, "TruckPlate", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.TruckPlate2, 8, "TruckPlate2", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .AreNotEquals(Body.StartGateOut, null, "StartGateOut", "Não foi informado o valor de StartGateOut")
                .AreNotEquals(Body.GateOutTime, null, "GateOutTime", "Não foi informado o valor de GateOutTime")
                .IsDigitCustom(Body.GateNumber, "GateNumber", "ERR_VALOR_N_NUMERO")
                .IsDigitCustom(Body.TruckWeight, "TruckWeight", "ERR_VALOR_N_NUMERO")
                .IsFalse(string.IsNullOrEmpty(Body.VoyageExpBtpId) && string.IsNullOrEmpty(Body.VoyageImpBtpId), "VoyageExpBtpId|VoyageImpBtpId", "ERR_TIPO_ESTRUTURA")
                );

            base.Validate();
        }
    }

    public class GateOut : CorpoEvento
    {
        public GateOut()
        {
            FilaHost = "event.gateout.host";
            FilaTos = "event.gateout.tos";
            Exchange = "event.gateout";
        }
        public DateTime StartGateOut { get; set; }
        public string Container { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public string VisitCode { get; set; }
        public string TruckWeight { get; set; }
        public string GateNumber { get; set; }
        public DateTime GateOutTime { get; set; }
        public string TruckPlate { get; set; }
        public string TruckPlate2 { get; set; }
        public string DriverId { get; set; }

    }
}
