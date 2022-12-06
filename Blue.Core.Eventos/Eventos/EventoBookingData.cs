using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Validacoes;
using Blue.Core.Eventos.Validacoes;
using Flunt.Notifications;
using Flunt.Validations;
using System.Collections.Generic;

namespace Blue.Core.Eventos.Eventos
{
    /// <summary>
    ///O evento pode ser acionado por TOS e HOST e se destina a fornecer informações relacionadas à reserva(Booking).
    /// </summary>
    public class EventoBookingData : InterfaceCabecalhoDto<BookingData>
    {
        public EventoBookingData(BookingData body)
        {
            Body = body;
            Event = "BookingData";
        }

        public EventoBookingData()
        {
            Body = new BookingData();
            Event = "BookingData";
        }

        public override void Validate()
        {

            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.BookingNumber, "BookingNumber", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.FullEmpty, "FullEmpty", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.VoyageExpBtpId, "VoyageExpBtpId", "ERR_CAMPO_OBRIGATORIO")
                //.IsDigitCustomIfNotNullOrEmpty(Body.OriginPortOfLoadingBtpId, "OriginPortOfLoadingBtpId", "ERR_VALOR_N_NUMERO")
                //.IsNotNullOrEmpty(Body.PortOfLoadingBtpId, "PortOfLoadingBtpId", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Body.PortOfDischargeBtpId, "PortOfDischargeBtpId", "ERR_CAMPO_OBRIGATORIO")
                .IsDigitCustomIfNotNullOrEmpty(Body.FinalPortOfDischargeBtpId, "FinalPortOfDischargeBtpId", "ERR_VALOR_N_NUMERO")
                .IsNotNullOrEmpty(Body.Liner, "Liner", "ERR_CAMPO_OBRIGATORIO")
                .HasMaxLengthIfNotNullOrEmpty(Body.BookingNumber, 30, "Container", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.FullEmpty, 1, "FullEmpty", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.VoyageExpBtpId, 38, "VoyageExpBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                //.HasMaxLengthIfNotNullOrEmpty(Body.OriginPortOfLoadingBtpId, 38, "OriginPortOfLoadingBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                //.HasMaxLengthIfNotNullOrEmpty(Body.PortOfLoadingBtpId, 38, "PortOfLoadingBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.PortOfDischargeBtpId, 38, "PortOfDischargeBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.FinalPortOfDischargeBtpId, 38, "FinalPortOfDischargeBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.LoadListPortOfDischargeBtpId, 38, "LoadListPortOfDischargeBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Body.Liner, 4, "Liner", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(Body.VoyageExpBtpId, "VoyageExpBtpId", "ERR_VALOR_N_NUMERO")
                //.IsDigitCustom(Body.PortOfLoadingBtpId, "PortOfLoadingBtpId", "ERR_VALOR_N_NUMERO")
                .IsDigitCustom(Body.PortOfDischargeBtpId, "PortOfDischargeBtpId", "ERR_VALOR_N_NUMERO")
            );

            Body.BookingDetails.ForEach(d =>
            {
                d?.Validate();

                if (d != null)
                    AddNotifications(d);
            });

            base.Validate();
        }

    }

    public class BookingData : CorpoEvento
    {
        public BookingData()
        {
            FilaHost = "event.booking.data.host";
            FilaTos = "event.booking.data.tos";
            Exchange = "event.booking.data";
        }
        public string BookingNumber { get; set; }
        public string FullEmpty { get; set; }
        public string VoyageExpBtpId { get; set; }
        public string OriginPortOfLoadingBtpId { get; set; }
        public string PortOfLoadingBtpId { get; set; }
        public string PortOfDischargeBtpId { get; set; }
        public string FinalPortOfDischargeBtpId { get; set; }
        public string LoadListPortOfDischargeBtpId { get; set; }
        public string Liner { get; set; }
        public List<BookingDetails> BookingDetails { get; set; }
    }

    public class BookingDetails : Notifiable, IValidatable
    {
        public bool Vgm { get; set; }
        public string Quantity { get; set; }
        public string Iso { get; set; }
        public string GrossWeight { get; set; }
        public string StowageInstructionCode { get; set; }
        public BookingOog Oog { get; set; }
        public BookingTemperature Temperature { get; set; }
        public List<BookingDangerous> Dangerous { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Quantity, "Quantity", "ERR_CAMPO_OBRIGATORIO")
                .IsNotNullOrEmpty(Iso, "Iso", "ERR_CAMPO_OBRIGATORIO")
                .HasMaxLengthIfNotNullOrEmpty(Quantity, 4, "Quantity", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Iso, 4, "Iso", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                //.HasMaxLengthIfNotNullOrEmpty(GrossWeight, 38, "GrossWeight", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(StowageInstructionCode, 5, "StowageInstructionCode", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(Quantity, "Quantity", "ERR_VALOR_N_NUMERO"));

            Temperature?.Validate();

            if (Temperature != null)
                AddNotifications(Temperature);

            Oog?.Validate();

            if (Oog != null)
                AddNotifications(Oog);

            Dangerous?.ForEach(d =>
            {
                d?.Validate();

                if (d != null)
                    AddNotifications(d);
            });
        }
    }

    public class BookingOog : Notifiable, IValidatable
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
                .HasMaxLengthIfNotNullOrEmpty(Right, 12, "Right", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Top, 12, "Top", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Front, 12, "Front", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Back, 12, "Back", "ERR_NUMERO_MAXIMO_ENCONTRADO")

                .IsDigitCustom(Left, "Left", "ERR_VALOR_N_NUMERO")
                .IsDigitCustom(Right, "Right", "ERR_VALOR_N_NUMERO")
                .IsDigitCustom(Top, "Top", "ERR_VALOR_N_NUMERO")
                .IsDigitCustom(Front, "Front", "ERR_VALOR_N_NUMERO")
                .IsDigitCustom(Back, "Back", "ERR_VALOR_N_NUMERO"));
        }
    }

    public class BookingTemperature : Notifiable, IValidatable
    {
        public string Temperature { get; set; }
        public string Ventilation { get; set; }
        public string Humidity { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMaxLengthIfNotNullOrEmpty(Temperature, 15, "Temperature", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Ventilation, 15, "Ventilation", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(Humidity, 15, "Humidity", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsDigitCustom(Temperature, "Temperature", "ERR_VALOR_N_NUMERO")
                .IsDigitCustom(Ventilation, "Ventilation", "ERR_VALOR_N_NUMERO")
                .IsDigitCustom(Humidity, "Humidity", "ERR_VALOR_N_NUMERO"));
        }
    }

    public class BookingDangerous : Notifiable, IValidatable
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
