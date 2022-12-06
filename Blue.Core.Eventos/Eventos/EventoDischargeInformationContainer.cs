using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Validacoes;
using Blue.Core.Eventos.Auxiliares;
using Blue.Core.Eventos.Validacoes;
using Flunt.Notifications;
using Flunt.Validations;
using System.Collections.Generic;

namespace Blue.Core.Eventos.Eventos.DischargeInformationContainer
{
    /// <summary>
    /// Evento que informa ao TOS as informações de cada container que será descarregado na BTP (Processo de Importação), esse evento será lançado na leitura
    /// do SisCarga no sistema HOST
    /// </summary>
    public class EventoDischargeInformationContainer : InterfaceCabecalhoDto<DischargeInformationContainer>
    {
        public EventoDischargeInformationContainer(DischargeInformationContainer body)
        {
            Body = body;
            Event = "DischargeInformationContainer";
        }

        public EventoDischargeInformationContainer()
        {
            Body = new DischargeInformationContainer();
            Event = "DischargeInformationContainer";
        }

        public override void Validate()
        {
            if (Body.ContainerDetails != null)
            {
                Body.ContainerDetails?.Validate();
                AddNotifications(Body.ContainerDetails);
            }
            else
                AddNotification("Body.ContainerDetails", MensagensAuxiliares.ObjetoNulo);

            Body.Documents?.ForEach(d =>
            {
                d.Validate();
                AddNotifications(d);
            });

            Body.Slaves?.ForEach(s =>
            {
                s.Validate();
                AddNotifications(s);
            });

            Body.Dangerous?.ForEach(d =>
            {
                d.Validate();
                AddNotifications(d);
            });

            base.Validate();    
        }
    }

    public class DischargeInformationContainer : CorpoEvento
    {
        public DischargeInformationContainer()
        {
            FilaTos = "event.discharge.information.container.tos";;
        }
        public ContainerDetails ContainerDetails { get; set; }
        public List<Document> Documents { get; set; }
        public List<Slave> Slaves { get; set; }
        public Oog Oog { get; set; }
        public List<Dangerou> Dangerous { get; set; }

    }

    public class ContainerDetails : Notifiable, IValidatable
    {
        public string Container { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public string IsoCode { get; set; }
        public string GrossWeight { get; set; }
        public string FullEmpty { get; set; }
        public string Category { get; set; }
        public string OriginPortOfLoadingBtpId { get; set; }
        public string PortOfLoadingBtpId { get; set; }
        public string PortOfDischargeBtpId { get; set; }
        public string FinalPortOfDischargeBtpId { get; set; }
        public string LoadListPortOfDischargeBtpId { get; set; }
        public string Liner { get; set; }
        public string GroupCode { get; set; }
        public string GroupCode2 { get; set; }
        public string Temperature { get; set; }
        public bool DirectLoadDischarge { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Container, "Container", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(IsoCode, "IsoCode", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(FullEmpty, "FullEmpty", "ERR_CAMPO_OBRIGATORIO")                
                .IsNotNullOrEmpty(PortOfLoadingBtpId, "PortOfLoadingBtpId", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(PortOfDischargeBtpId, "PortOfDischargeBtpId", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(FinalPortOfDischargeBtpId, "FinalPortOfDischargeBtpId", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Liner, "Liner", "ERR_CAMPO_OBRIGATORIO")
                .HasMaxLengthIfNotNullOrEmpty(Container, 12, "Container", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(IsoCode, 4, "IsoCode", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(GrossWeight, 38, "GrossWeight", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(FullEmpty, 1, "FullEmpty", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Category, 2, "Category", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(PortOfLoadingBtpId, 38, "PortOfLoadingBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(PortOfLoadingBtpId, "PortOfLoadingBtpId", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(PortOfDischargeBtpId, 38, "PortOfDischargeBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(PortOfDischargeBtpId, "PortOfDischargeBtpId", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(FinalPortOfDischargeBtpId, 38, "FinalPortOfDischargeBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(FinalPortOfDischargeBtpId, "FinalPortOfDischargeBtpId", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(Liner, 4, "Liner", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(GroupCode, 10, "GroupCode", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(GroupCode2,10, "GroupCode2", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Temperature, 15, "Temperature", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustomIfNotNullOrEmpty(Temperature, "Temperature", "ERR_VALOR_N_NUMERO"));
        }
    }

    public class Document : Notifiable, IValidatable
    {
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMaxLengthIfNotNullOrEmpty(DocumentType, 15, "DocumentType", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(DocumentNumber, 100, "DocumentNumber", "ERR_NUMERO_MAXIMO_ENCONTRADO"));
        }
    }

    public class Slave : Notifiable, IValidatable
    {
        public string Number { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMaxLengthIfNotNullOrEmpty(Number, 12, "Number", "ERR_NUMERO_MAXIMO_ENCONTRADO"));
        }
    }

    public class Oog : Notifiable, IValidatable
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

    public class Dangerou : Notifiable, IValidatable
    {
        public string ImoBtpId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMaxLengthIfNotNullOrEmpty(ImoBtpId, 38, "ImoBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(ImoBtpId, "ImoBtpId", "ERR_VALOR_N_NUMERO"));
        }
    }
}
