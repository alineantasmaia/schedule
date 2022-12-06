using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Validacoes;
using Flunt.Validations;
using System;

namespace Blue.Core.Eventos.Eventos
{
    /// <summary>
    /// Evento que informa ao TOS ou HOST o momento em que é realizado a confirmação da entrada de um caminhão externo no terminal. Este evento é bidirecional, poderá ser 
    /// emitido com a origem Host e também a partir do TOS
    /// </summary>
    public class EventoGateIn : InterfaceCabecalhoDto<GateIn>
    {
        public EventoGateIn(GateIn body)
        {
            Body = body;
            Event = "GateIn";
        }

        public EventoGateIn()
        {
            Body = new GateIn();
            Event = "GateIn";
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.Container, "Container", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.VisitCode, "VisitCode", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.TruckWeight, "TruckWeight", "ERR_CAMPO_OBRIGATORIO")
                .HasMaxLengthIfNotNullOrEmpty(Body.Container, 12, "Container", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.VisitCode, 38, "VisitCode", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.TruckWeight, 15, "TruckWeight", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.GateNumber, 15, "GateNumber", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.DriverId, 15, "DriverId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.TruckPlate, 8, "TruckPlate", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.TruckPlate2, 8, "TruckPlate2", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                //.AreNotEquals(Body.StartGateIn, null, "StartGateIn", "Não foi informado o valor de StartGateOut")
                .AreNotEquals(Body.GateInTime, null, "GateInTime", "Não foi informado o valor de GateOutTime")
                .IsDigitCustom(Body.GateNumber, "GateNumber", "ERR_VALOR_N_NUMERO")
                .IsDigitCustom(Body.TruckWeight, "TruckWeight", "ERR_VALOR_N_NUMERO")
                .IsFalse(string.IsNullOrEmpty(Body.VoyageExpBtpId) && string.IsNullOrEmpty(Body.VoyageImpBtpId), "VoyageExpBtpId|VoyageImpBtpId", "ERR_TIPO_ESTRUTURA")
                );

            base.Validate();
        }
    }


    public class GateIn : CorpoEvento
    {
        public GateIn()
        {
            FilaHost = "event.gatein.host";
            FilaTos = "event.gatein.tos";
            Exchange = "event.gatein";
        }
        public DateTime? StartGateIn { get; set; }
        public string Container { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public string VisitCode { get; set; }
        public string TruckWeight { get; set; }
        public string GateNumber { get; set; }
        public DateTime GateInTime { get; set; }
        public Location Location { get; set; }
        public string BatNumber { get; set; }
        public string TruckPlate { get; set; }
        public string TruckPlate2 { get; set; }
        public string DriverId { get; set; }
    }

    public class Location
    {
        public string Block { get; set; }
        public string Bay { get; set; }
        public string Row { get; set; }
        public string Tier { get; set; }
    }
}
