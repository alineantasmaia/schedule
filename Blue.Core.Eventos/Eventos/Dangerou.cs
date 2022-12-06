using Blue.Core.Entidades.Validacoes;
using Flunt.Notifications;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blue.Core.Eventos.Eventos
{
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
