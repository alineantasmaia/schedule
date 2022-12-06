using Blue.Core.Entidades.Dtos;
using Flunt.Notifications;
using Flunt.Validations;
using System;
using System.Collections.Generic;

namespace Blue.Core.Eventos.Eventos
{
    /// <summary>
    /// Evento que informa ao TOS ou o HOST o momento em que será aberto ou finalizado um special handling. Este evento é bidirecional, poderá ser 
    /// emitido com a origem Host e também a partir do TOS
    /// </summary>
    public class EventoSpecialService : InterfaceCabecalhoDto<SpecialService>
    {
        public EventoSpecialService(SpecialService body)
        {
            Body = body;
            Event = "SpecialService";
        }

        public EventoSpecialService()
        {
            Body = new SpecialService();
            Event = "SpecialService";
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.Container, "Conteiner", "ERR_N_ENCONTRADO_CONTEINER"));

                 Body.ContainerInstructionCodes.ForEach(s =>
                 {
                     s.Validate();
                     AddNotifications(s);
                 });

            base.Validate();
        }
    }

    public class SpecialService : CorpoEvento
    {
        public SpecialService()
        {
            FilaHost = "event.special.service.host";
            FilaTos = "event.special.service.tos";
            Exchange = "event.special.service";
        }

        public string Container { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public List<ContainerInstruction> ContainerInstructionCodes { get; set; }
    }

    public class ContainerInstruction : Notifiable, IValidatable
    {
        public string ContainerInstructionCode { get; set; }
        public string ContainerInstructionBtpId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Clear { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMaxLengthIfNotNullOrEmpty(ContainerInstructionCode, 5, "ContainerInstructionCode", "ERR_N_CODIGO_SH")
                .IsNotNullOrEmpty(ContainerInstructionCode, "ContainerInstructionCode", "ERR_N_CODIGO_SH")
                .IsNotNull(Created, "Criação Special Handling", "Data de criação do special handling não informada")
                .IsNotNullOrEmpty(ContainerInstructionBtpId, "Id Special Handling", "ERR_ID_SH"));
        }
    }
}
