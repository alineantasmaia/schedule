using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Validacoes;
using Flunt.Notifications;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using Blue.Core.Eventos.Auxiliares;
using Newtonsoft.Json;

namespace Blue.Core.Eventos.Eventos
{
    /// <summary>
    /// Evento que informa ao HOST a confirmação do planejamento do contêiner a ser desembarcado durante a operação do navio.
    /// </summary>
    public class EventoDischargePlan : InterfaceCabecalhoDto<DischargePlan>
    {
       
        public EventoDischargePlan(DischargePlan body)
        {
            Body = body;
            Event = "DischargePlan";
        }
        public EventoDischargePlan()
        {
            Body = new DischargePlan();
            Event = "DischargePlan";
        }
        public override void Validate()
        {
            Body.ContainerDetails?.Validate();
            if (Body.ContainerDetails != null)
                AddNotifications(Body.ContainerDetails);
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
            
            Body.Temperature?.Validate();
            if (Body.Temperature != null) AddNotifications(Body.Temperature);

           
            Body.Oog?.Validate();
            if (Body.Oog != null) AddNotifications(Body.Oog);

            Body.Dangerous?.ForEach(d =>
            {
                d.Validate();
                AddNotifications(d);
            });

            base.Validate();
        }
    }
    public class DischargePlan : CorpoEvento
    {
        public DischargePlan()
        {
            FilaHost = "event.discharge.plan.host";
        }
        public ContainerDischargePlan ContainerDetails { get; set; }
        public List<DocumentDischargePlan> Documents { get; set; }
        public List<SlaveDischargePlan> Slaves { get; set; }
        [JsonConverter(typeof(ConvertTemperature))]
        public TemperatureDischargePlan Temperature { get; set; }
        public OogDischargePlan Oog { get; set; }
        public List<DangerousDischargePlan> Dangerous { get; set; }
    }

    public class ConvertTemperature : JsonConverter<TemperatureDischargePlan>
    {
        public override TemperatureDischargePlan ReadJson(JsonReader reader, Type objectType, TemperatureDischargePlan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var temperatureDischargePlan = new TemperatureDischargePlan();

            if (reader.TokenType == JsonToken.Null)
                return null;

            serializer.Populate(reader, temperatureDischargePlan);

            return temperatureDischargePlan;
        }

        public override void WriteJson(JsonWriter writer, TemperatureDischargePlan value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, new { TemperatureDischargePlan = value });
        }

    }

    public class ContainerDischargePlan : Notifiable, IValidatable
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
        public bool DirectDischarge { get; set; }
        public bool Twin { get; set; }
        public bool Vgm { get; set; }
        public string CheBtpId { get; set; }
        public string CheType { get; set; }
        public string StowageVessel { get; set; }
        public DateTime? EstimatedTime { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Container, "Container", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(IsoCode, "IsoCode", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(FullEmpty, "FullEmpty", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Category, "Category", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(PortOfLoadingBtpId, "PortOfLoadingBtpId", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(PortOfDischargeBtpId, "PortOfDischargeBtpId", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Liner, "Liner", "ERR_CAMPO_OBRIGATORIO")
                .HasMaxLengthIfNotNullOrEmpty(Container, 12, "Container", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(IsoCode, 4, "IsoCode", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(GrossWeight, 38, "GrossWeight", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(FullEmpty, 1, "FullEmpty", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Category, 2, "Category", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(OriginPortOfLoadingBtpId, 38, "OriginPortOfLoadingBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(OriginPortOfLoadingBtpId, "OriginPortOfLoadingBtpId", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(PortOfLoadingBtpId, 38, "PortOfLoadingBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(PortOfLoadingBtpId, "PortOfLoadingBtpId", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(PortOfDischargeBtpId, 38, "PortOfDischargeBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(PortOfDischargeBtpId, "PortOfDischargeBtpId", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(FinalPortOfDischargeBtpId, 38, "FinalPortOfDischargeBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(FinalPortOfDischargeBtpId, "FinalPortOfDischargeBtpId", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(Liner, 4, "Liner", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(GroupCode, 9, "GroupCode", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(CheBtpId, 38, "CheBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(CheType, 15, "CheType", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(StowageVessel, 6, "StowageVessel", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsNotNull(EstimatedTime, "EstimatedTime", "Não foi informado o valor de EstimatedTime"));
        }
    }

    public class DocumentDischargePlan: Notifiable, IValidatable
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

    public class SlaveDischargePlan: Notifiable, IValidatable
    {
        public string Number { get; set; }
        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMaxLengthIfNotNullOrEmpty(Number, 12, "Number", "ERR_NUMERO_MAXIMO_ENCONTRADO"));
        }
    }

    public class TemperatureDischargePlan: Notifiable, IValidatable
    {
        public string Temperature { get; set; }
        public string Ventilation { get; set; }
        public string Humidity { get; set; }
        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMaxLengthIfNotNullOrEmpty(Temperature, 15, "Temperature", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(Temperature, "Temperature", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(Ventilation, 15, "Ventilation", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(Ventilation, "Ventilation", "ERR_VALOR_N_NUMERO")
                .HasMaxLengthIfNotNullOrEmpty(Humidity, 15, "Humidity", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(Humidity, "Humidity", "ERR_VALOR_N_NUMERO"));
        }
    }

    public class OogDischargePlan: Notifiable, IValidatable
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
                .IsDigitCustom(Back, "Back", "ERR_VALOR_N_NUMERO")
                .AreNotEqualsIsNotIsNullOrEmpty(Left, "0", "Left", "ERR_VALOR_ZERO_ENCONTRADO")
                .AreNotEqualsIsNotIsNullOrEmpty(Right, "0", "Right", "ERR_VALOR_ZERO_ENCONTRADO")
                .AreNotEqualsIsNotIsNullOrEmpty(Top, "0", "Top", "ERR_VALOR_ZERO_ENCONTRADO")
                .AreNotEqualsIsNotIsNullOrEmpty(Front, "0", "Front", "ERR_VALOR_ZERO_ENCONTRADO")
                .AreNotEqualsIsNotIsNullOrEmpty(Back, "0", "Back", "ERR_VALOR_ZERO_ENCONTRADO"));
        }
    }

    public class DangerousDischargePlan: Notifiable, IValidatable
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
