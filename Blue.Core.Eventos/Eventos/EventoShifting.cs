using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Validacoes;
using Flunt.Notifications;
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
    public class EventoShifting : InterfaceCabecalhoDto<Shifting>
    {
        public EventoShifting(Shifting body)
        {
            Body = body;
            Event = "Shifting";
        }

        public EventoShifting()
        {
            Body = new Shifting();
            Event = "Shifting";

            Body.ContainerActivity = Body.ContainerActivity ?? new List<ContainerActivity>();
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.Container, "Conteiner", "ERR_N_ENCONTRADO_CONTEINER")
                .IsNotNullOrEmpty(Body.ActionCode, "ActionCode", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.OriginStowageVessel, "OriginStowageVessel", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.FullEmpty, "FullEmpty", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.GrossWeight, "GrossWeight", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.Iso, "Iso", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.Category, "Category", "ERR_CAMPO_OBRIGATORIO")
                );

            Body?.ContainerActivity?.ForEach(d =>
            {
                d.Validate();
                AddNotifications(d);
            });

            Body.Oog?.Validate();
            if (Body.Oog != null) AddNotifications(Body.Oog);

            Body?.Dangerous?.ForEach(d =>
            {
                d?.Validate();
                AddNotifications(d);
            });

            base.Validate();
        }
    }

    public class Shifting : CorpoEvento
    {
        public Shifting()
        {
            FilaHost = "event.shifting.host";
            FilaTos = "event.shifting.tos";
            Exchange = "event.shifting";
        }
        public string Container { get; set; }
        public string ActionCode { get; set; }
        public string Category { get; set; }
        public string FullEmpty { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public string Iso { get; set; }
        public string Liner { get; set; }
        public string GrossWeight { get; set; }
        public string OriginPortOfLoadingBtpId { get; set; }
        public string PortOfLoadingBtpId { get; set; }
        public string PortOfDischargeBtpId { get; set; }
        public string FinalPortOfDischargeBtpId { get; set; }
        public string LoadListPortOfDischargeBtpId { get; set; }
        public DateTime? DischargeTime { get; set; }
        public DateTime? LoadTime { get; set; }
        public DateTime? ShiftTime { get; set; }
        public string OriginStowageVessel { get; set; }
        public string TargetStowageVessel { get; set; }
        public bool Twin { get; set; }
        public string Temperature { get; set; }
        public OogShifting Oog { get; set; }
        public List<Dangerou> Dangerous { get; set; }
        public List<ContainerActivity> ContainerActivity { get; set; }
    }
}

public class OogShifting : Notifiable, IValidatable
{
    public string Left { get; set; }
    public string Right { get; set; }
    public string Top { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
    public void Validate()
    {
        AddNotifications(new Contract()
            .Requires()
            .HasMaxLengthIfNotNullOrEmpty(Left, 12, "Left", "ERR_NUMERO_MAXIMO_ENCONTRADO")
            .IsDigitCustom(Left, "Left", "ERR_VALOR_N_NUMERO")

            .HasMaxLengthIfNotNullOrEmpty(Right, 12, "Right", "ERR_NUMERO_MAXIMO_ENCONTRADO")
            .IsDigitCustom(Right, "Right", "ERR_VALOR_N_NUMERO")

            .HasMaxLengthIfNotNullOrEmpty(Top, 12, "Top", "ERR_NUMERO_MAXIMO_ENCONTRADO")
            .IsDigitCustom(Top, "Top", "ERR_VALOR_N_NUMERO")

            .HasMaxLengthIfNotNullOrEmpty(Front, 12, "Front", "ERR_NUMERO_MAXIMO_ENCONTRADO")
            .IsDigitCustom(Front, "Front", "ERR_VALOR_N_NUMERO")

            .HasMaxLengthIfNotNullOrEmpty(Back, 12, "Back", "ERR_NUMERO_MAXIMO_ENCONTRADO")
            .IsDigitCustom(Back, "Back", "ERR_VALOR_N_NUMERO"));
    }
}