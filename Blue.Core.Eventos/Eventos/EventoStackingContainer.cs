using Blue.Core.Entidades.Dtos;
using Flunt.Notifications;
using Flunt.Validations;
using System.Collections.Generic;
using System.Linq;
using Blue.Core.Entidades.Validacoes;
using Blue.Core.Eventos.Validacoes;

namespace Blue.Core.Eventos.Eventos
{
    /// <summary>
    /// Evento que informa ao HOST os movimentos de conteiners executados no terminal.
    /// emitido com a origem TOS e enviado para o HOST
    /// </summary>
    public class EventoStackingContainer : InterfaceCabecalhoDto<StackingContainer>
    {
        public EventoStackingContainer()
        {
            Body = new StackingContainer();
            Event = "StackingContainer";
        }

        public EventoStackingContainer(StackingContainer body)
        {
            Body = body;
            Event = "StackingContainer";
        }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.ActionCode, "ActionCode", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.Container, "Container", "ERR_CAMPO_OBRIGATORIO")
                .HasMaxLengthIfNotNullOrEmpty(Body.ActionCode, 2, "ActionCode", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.Container, 12, "Container", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.TruckPlate, 8, "TruckPlate", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.CurrentMove, 10, "CurrentMove", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.VisitCode, 38, "VisitCode", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(Body.MovementOrderBtpId, "MovementOrderBtpId", "ERR_VALOR_N_NUMERO")
                .IsFalse(string.IsNullOrEmpty(Body.CurrentMove) && Body.Position == null, "CurrentMove|Position", "ERR_TIPO_ESTRUTURA")
                .IsFalse(Body.CurrentMove != "T" && Body.Position == null, "Position", "ERR_TIPO_ESTRUTURA")
                .IsFalse(string.IsNullOrEmpty(Body.VoyageExpBtpId) && string.IsNullOrEmpty(Body.VoyageImpBtpId), "VoyageExpBtpId|VoyageImpBtpId", "ERR_TIPO_ESTRUTURA")
                .IsDigitCustomIfNotNullOrEmpty(Body.VoyageExpBtpId, "VoyageExpBtpId", "O Valor de VoyageExpBtpId não é um Digito")
                .IsDigitCustomIfNotNullOrEmpty(Body.VoyageImpBtpId, "VoyageImpBtpId", "O Valor de VoyageImpBtpId não é um Digito")
            );

            if (!this.AdmProcess && !(Body.ContainerActivity?.Any() ?? false) && !(Body?.Position?.IsHeapArea() ?? false))
                AddNotification("ContainerActivity", "ERR_ATIVIDADE_CTR_N_ENCONTRADO");

            Body.ContainerActivity?.ForEach(d =>
            {
                d.Validate();
                AddNotifications(d);
            });

            if (Body.CurrentMove != "T") //Truck
                Body.Position?.Validate();

            base.Validate();
        }
    }

    public class StackingContainer : CorpoEvento
    {
        public StackingContainer()
        {
            FilaHost = "event.stacking.container.host";
        }

        public string ActionCode { get; set; }
        public string Container { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public bool Twin { get; set; }
        public Position Position { get; set; }
        public string TruckPlate { get; set; }
        public string VisitCode { get; set; }
        public string MovementOrderBtpId { get; set; }
        public string CurrentMove { get; set; }
        public List<ContainerActivity> ContainerActivity { get; set; }
    }

    public class Position : Notifiable, IValidatable
    {
        public string Bay { get; set; }
        public string Block { get; set; }
        public string Row { get; set; }
        public string Tier { get; set; }
        public void Validate()
        {
           AddNotifications(
               new Contract()
                   .Requires()
                   .IsNotNullOrEmpty(Bay, "Bay", "ERR_CAMPO_OBRIGATORIO")
                   .IsNotNullOrEmpty(Block, "Block", "ERR_CAMPO_OBRIGATORIO")
                   .IsNotNullOrEmpty(Row, "Row", "ERR_CAMPO_OBRIGATORIO")
                   .IsNotNullOrEmpty(Tier, "Tier", "ERR_CAMPO_OBRIGATORIO")
                   .HasMaxLengthIfNotNullOrEmpty(Bay, 6, "Bay", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                   .HasMaxLengthIfNotNullOrEmpty(Block, 6, "Block", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                   .HasMaxLengthIfNotNullOrEmpty(Row, 6, "Row", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                   .HasMaxLengthIfNotNullOrEmpty(Tier, 6, "Tier", "ERR_NUMERO_MAXIMO_ENCONTRADO"));
        }

        public bool IsHeapArea()
            => !string.IsNullOrEmpty(Block) && string.IsNullOrEmpty(Bay) && string.IsNullOrEmpty(Row) && string.IsNullOrEmpty(Tier);

    }
}
