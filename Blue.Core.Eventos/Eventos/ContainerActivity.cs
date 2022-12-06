using Flunt.Notifications;
using Flunt.Validations;
using System;

namespace Blue.Core.Eventos.Eventos
{
    public class ContainerActivity : Notifiable, IValidatable
    {
        public DateTime? ActivityDate { get; set; }
        public string CheBtpId { get; set; }
        public string CheType { get; set; }
        public string CheUserId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                 .Requires()
                 .IsNotNull(ActivityDate, "ActivityDate", "ERR_CAMPO_OBRIGATORIO")
                 .IsNotNullOrEmpty(CheBtpId, "CheBtpId", "ERR_CAMPO_OBRIGATORIO")
                 .IsNotNullOrEmpty(CheType, "CheType", "ERR_CAMPO_OBRIGATORIO"));
        }
    }
}
