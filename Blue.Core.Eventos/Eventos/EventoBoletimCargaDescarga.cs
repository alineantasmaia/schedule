using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Validacoes;
using Flunt.Validations;

namespace Blue.Core.Eventos.Eventos
{
    /// <summary>
    /// Evento que envia o Boletim de carga ou descarga.
    /// </summary>
    public class EventoBoletimCargaDescarga : InterfaceCabecalhoDto<BoletimCargaDescarga>
    {
        public EventoBoletimCargaDescarga(BoletimCargaDescarga body)
        {
            Body = body;
            Event = "BoletimCargaDescarga";
            ProcessType = "I";
        }
        public EventoBoletimCargaDescarga()
        {
            Body = new BoletimCargaDescarga();
            Event = "BoletimCargaDescarga";
            ProcessType = "I";
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.NumeroConteiner, "Conteiner", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.IdEscala, "Escala", "ERR_CAMPO_OBRIGATORIO"));

            base.Validate();
        }
    }

    public class BoletimCargaDescarga : CorpoEvento
    {
        public BoletimCargaDescarga()
        {
            FilaHost = "event.boletim.cargadescarga.host";
        }
        public string IdEscala { get; set; }
        public bool Carga { get; set; }
        public bool Descarga { get; set; }
        public string NumeroConteiner { get; set; }
    }
}
