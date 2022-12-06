using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Validacoes;
using Flunt.Notifications;
using Flunt.Validations;
using System.Collections.Generic;

namespace Blue.Core.Eventos.Eventos
{ 
    public class EventoContainerData : InterfaceCabecalhoDto<ContainerData>
    {
        public EventoContainerData(ContainerData body)
        {
            Body = body;
            Event = "ContainerData";
        }

        public EventoContainerData()
        {
            Body = new ContainerData();
            Event = "ContainerData";
        }

        public override void Validate()
        {
            Body.ContainerDetails.Validate();

            AddNotifications(Body.ContainerDetails);

            Body.Documents?.ForEach(d =>
            {
                d.Validate();
                AddNotifications(d);
            });

            base.Validate();
        }
    }

    public class ContainerData : CorpoEvento
    {
        public ContainerData()
        {
            FilaHost = "event.container.data.host";
            FilaTos = "event.container.data.tos";
            Exchange = "event.container.data";
        }
        public ContainerDetails ContainerDetails { get; set; }
        public List<Document> Documents { get; set; }
        public List<Genset> Gensets { get; set; }
        public Oog Oog { get; set; }
        public bool LoadCancel { get; set; }
    }

    public class ContainerDetails : Notifiable, IValidatable
    {
        public string Container { get; set; }
        public string NewContainerNumber { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public string NewVoyageExpBtpId { get; set; }
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
        public string Temperature { get; set; }
        public bool Vgm { get; set; }
        public bool DirectLoadDischarge { get; set; }
        public bool ShipOwner { get; set; }
        public string GroupCode { get; set; }
        public string GroupCode2 { get; set; }  

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMaxLengthIfNotNullOrEmpty(Container, 12, "Container", "ERR_CONTAINER_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(IsoCode, 4, "IsoCode", "ERR_ISO_CODE_N_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(GrossWeight, 38, "GrossWeight", "ERR_PESO_BRUTO_N_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(FullEmpty, 1, "FullEmpty", "ERR_CHEIO_VAZIO_N_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Category, 2, "Category", "ERR_CATEGORIA_N_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(OriginPortOfLoadingBtpId, 38, "OriginPortOfLoadingBtpId", "ERR_PORTO_ORIGINAL_EMBARQUE_N_ENCONTRADO")
                .IsDigitCustom(OriginPortOfLoadingBtpId, "OriginPortOfLoadingBtpId", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(PortOfLoadingBtpId, 38, "PortOfLoadingBtpId", "ERR_PORTO_EMBARQUE_N_ENCONTRADO")
                .IsDigitCustom(PortOfLoadingBtpId, "PortOfLoadingBtpId", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(PortOfDischargeBtpId, 38, "PortOfDischargeBtpId", "ERR_PORTO_DESCARGA_N_ENCONTRADO")
                .IsDigitCustom(PortOfDischargeBtpId, "PortOfDischargeBtpId", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(FinalPortOfDischargeBtpId, 38, "FinalPortOfDischargeBtpId", "ERR_PORTO_FINAL_DESCARGA_N_ENCONTRADO")
                .IsDigitCustom(FinalPortOfDischargeBtpId, "FinalPortOfDischargeBtpId", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(Liner, 4, "Liner", "ERR_LINER_N_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Temperature, 15, "Temperature", "ERR_TEMPERATURA_N_ENCONTRADO")
                .IsDigitCustom(Temperature, "Temperature", "ERR_VALOR_N_NUMERO"));
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
                .HasMaxLengthIfNotNullOrEmpty(DocumentType, 15, "DocumentType", "ERR_TIPO_DOCUMENTO_N_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(DocumentNumber, 100, "DocumentNumber", "ERR_DOCUMENTO_N_ENCONTRADO"));
        }
    }

    public class Genset : Notifiable, IValidatable
    {
        public string Number { get; set; }
        public void Validate()
        {
            AddNotifications(new Contract()
                .HasMaxLengthIfNotNullOrEmpty(Number, 14, "Number", "ERR_NUMERO_MAXIMO_ENCONTRADO"));
        }
    }

    public class Oog
    {
        public string Left { get; set; }
        public string Right { get; set; }
        public string Top { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
    }
}
