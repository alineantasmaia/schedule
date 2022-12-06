using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Validacoes;
using Blue.Core.Eventos.Validacoes;
using Flunt.Notifications;
using Flunt.Validations;
using System;
using System.Collections.Generic;

namespace Blue.Core.Eventos.Eventos.PreAdvice
{
    /// <summary>
    /// Evento que informa ao TOS o momento que é confirmado um agendamento no TAS (Truck Appointment System) 
    /// </summary>
    public class EventoPreAdvice : InterfaceCabecalhoDto<PreAdvice>
    {
        public EventoPreAdvice(PreAdvice body)
        {
            Body = body;
            Event = "PreAdvice";
        }

        public EventoPreAdvice()
        {
            Body = new PreAdvice();
            Event = "PreAdvice";
        }

        public bool IsTsExport() => "TX".Equals(Body.ContainerDetails.Category);

        public void SetVoyageExport(string voyageExpBtpId) => Body.ContainerDetails.VoyageExpBtpId = voyageExpBtpId;

        public void SetTransshipmentInterTerminal()
        {
            Body.ContainerDetails.PortOfDischargeBtpId = Body.ContainerDetails.LoadListPortOfDischargeBtpId;
            Body.ContainerDetails.TransshipmentInterTerminal = true;
        }

        public override void Validate()
        {
            Body.ContainerDetails?.Validate();

            AddNotifications(Body.ContainerDetails);

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

            Body.TruckAppointment?.Validate();

            Body.Gensets?.ForEach(d =>
            {
                d.Validate();
                AddNotifications(d);
            });

            AddNotifications(Body.TruckAppointment);

            base.Validate();
        }
    }

    public class PreAdvice : CorpoEvento
    {
        public PreAdvice()
        {
            FilaTos = "event.pre.advice.tos";
        }
        public ContainerDetails ContainerDetails { get; set; }
        public List<Document> Documents { get; set; }
        public List<Slave> Slaves { get; set; }
        public Oog Oog { get; set; }
        public List<Dangerou> Dangerous { get; set; }
        public TruckAppointment TruckAppointment { get; set; }
        public List<Eventos.Genset> Gensets { get; set; }
    }

    public class TruckAppointment : Notifiable, IValidatable
    {
        public string VisitCode { get; set; }
        public string TruckNumber { get; set; }
        public string TruckPlate { get; set; }
        public string TruckPlate2 { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool VoyageTwin { get; set; }
        public string DriverId { get; set; }
        public string WeightLimit { get; set; }
        public bool LengthRestricted { get; set; }
        public string AppointmentType { get; set; }
 
        public bool BitremCheck { get; set; }
        public string Cntrtruckposition { get; set; }

        public bool IsImportEmptyTOS() => "EO".Equals(AppointmentType);

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMaxLengthIfNotNullOrEmpty(VisitCode, 38, "VisitCode", "ERR_AGENDAMENTO_N_ENCONTRADO")
                .IsDigitCustom(VisitCode, "VisitCode", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(TruckNumber, 8, "TruckNumber", "ERR_NUMERO_TRUCK_N_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(DriverId, 15, "DriverId", "ERR_MOTORISTA_N_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(WeightLimit, 6, "WeightLimit", "ERR_PESO_N_ENCONTRDO")
                .IsDigitCustom(WeightLimit, "WeightLimit", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(AppointmentType, 2, "AppointmentType", "ERR_TIPO_AGENDAMENTO_TOS_N_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(TruckPlate, 8, "TruckPlate", "ERR_NUMERO_CARRETA_N_ENCONTRADO"));
        }
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
        public bool DirectDischargeLoad { get; set; }
        public string Temperature { get; set; }
        public string GroupCode { get; set; }
        public string GroupCode2 { get; set; }
        public bool TransshipmentInterTerminal { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMaxLengthIfNotNullOrEmpty(Container, 12, "Container", "ERR_CONTAINER_N_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(IsoCode, 4, "IsoCode", "ERR_ISO_CODE_N_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(GrossWeight, 38, "GrossWeight", "ERR_PESO_BRUTO_N_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(FullEmpty, 1, "FullEmpty", "ERR_CHEIO_VAZIO_N_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Category, 2, "Category", "ERR_CATEGORIA_N_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(PortOfLoadingBtpId, 38, "PortOfLoadingBtpId", "ERR_PORTO_EMBARQUE_N_ENCONTRADO")
                .IsDigitCustom(PortOfLoadingBtpId, "PortOfLoadingBtpId", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(PortOfDischargeBtpId, 38, "PortOfDischargeBtpId", "ERR_PORTO_DESCARGA_N_ENCONTRADO")
                .IsDigitCustom(PortOfDischargeBtpId, "PortOfDischargeBtpId", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(FinalPortOfDischargeBtpId, 38, "FinalPortOfDischargeBtpId", "ERR_PORTO_FINAL_DESCARGA_N_ENCONTRADO")
                .IsDigitCustomIfNotNullOrEmpty(FinalPortOfDischargeBtpId, "FinalPortOfDischargeBtpId", "ERR_VALOR_N_NUMERO")
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

    public class Slave : Notifiable, IValidatable
    {
        public string Number { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMaxLengthIfNotNullOrEmpty(Number, 12, "Number", "Número slave não foi encontrado"));
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

    public class Dangerou : Notifiable, IValidatable
    {
        public string ImoBtpId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .IsDigitCustom(ImoBtpId, "ImoBtpId", "ERR_VALOR_N_NUMERO"));
        }
    }
}
