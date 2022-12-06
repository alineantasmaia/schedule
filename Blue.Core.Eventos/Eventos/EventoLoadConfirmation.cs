using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Validacoes;
using Flunt.Validations;

namespace Blue.Core.Eventos.Eventos
{
    /// <summary>
    /// Evento que informa ao TOS ou HOST o momento em que realizado uma confirmação de embarque. Este evento é bidirecional, poderá ser 
    /// emitido com a origem Host e também a partir do TOS 
    /// </summary>
    public class EventoLoadConfirmation : InterfaceCabecalhoDto<LoadConfirmation>
    {
        public EventoLoadConfirmation(LoadConfirmation body)
        {
            Body = body;
            Event = "LoadConfirmation";
        }

        public EventoLoadConfirmation()
        {
            Body = new LoadConfirmation();
            Event = "LoadConfirmation";
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.Container, "Conteiner", "ERR_N_ENCONTRADO_CONTEINER")
                .IsDigitCustom(Body.VoyageExpBtpId, "VoyageExpBtpId", "ERR_VALOR_N_NUMERO"));
            
            base.Validate();

        }
    }
    public class LoadConfirmation : CorpoEvento
    {
        public LoadConfirmation()
        {
            FilaTos = "event.load.confirmation.tos";
        }
        public string Container { get; set; }
        public string VoyageExpBtpId { get; set; }
    }
}
