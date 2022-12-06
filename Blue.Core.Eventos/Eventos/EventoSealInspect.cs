using Blue.Core.Entidades.Dtos;
using Flunt.Notifications;
using Flunt.Validations;
using System.Collections.Generic;

namespace Blue.Core.Eventos.Eventos
{
    /// <summary>
    /// Evento que informa ao TOS ou o HOST o momento em que será realizada a inspeção de lacre no container que está preste a passar pelo gate. Este evento é bidirecional, poderá ser 
    /// emitido com a origem Host e também a partir do TOS
    /// </summary>
    public class EventoSealInspect : InterfaceCabecalhoDto<SealInspect>
    {
        public EventoSealInspect(SealInspect body)
        {
            Body = body;
            Event = "SealInspect";
        }

        public EventoSealInspect()
        {
            Body = new SealInspect();
            Event = "SealInspect";
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.Container, "Conteiner", "ERR_N_ENCONTRADO_CONTEINER")
                .AreNotEquals(Body.Seals.Count, 0, "Quantidade de Lacres", "A quantidade de lacres deve ser maior que 0"));

            Body.Seals.ForEach(s =>
            {
                s.Validate();
                AddNotifications(s);
            });

            if(string.IsNullOrEmpty(Body.VoyageExpBtpId) && string.IsNullOrEmpty(Body.VoyageImpBtpId))
                AddNotification("VoyageExpBtpId, VoyageImpBtpId", "Obrigatório informar a viagem.");

            base.Validate();
        }
    }

    public class SealInspect : CorpoEvento
    {
        public SealInspect()
        {
            FilaHost = "event.seal.inspect.host";
            FilaTos = "event.seal.inspect.tos";
        }

        public string Container { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public List<Seal> Seals { get; set; }
    }

    public class Seal : Notifiable, IValidatable
    {
        public string SealNumber { get; set; }
        public string SealType { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(SealNumber, "SealNumber", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(SealType, "SealType", "ERR_CAMPO_OBRIGATORIO")
                .HasMaxLengthIfNotNullOrEmpty(SealNumber, 14, "Número Lacre", "ERR_LACRE_TAMANHO_INCORRETO")
                .HasMaxLengthIfNotNullOrEmpty(SealType, 3, "Tipo Lacre", "ERR_TIPO_LACRE_TAMANHO_INCORRETO"));
        }
    }
}
