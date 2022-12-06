using Blue.Core.Entidades.Dtos;
using Flunt.Notifications;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blue.Core.Eventos.Eventos
{
    /// <summary>
    /// Evento que informa ao TOS ou HOST o momento em que é finalizada a inspeção de avaria do container. Este evento é bidirecional, poderá ser 
    /// emitido com a origem Host e também a partir do TOS
    /// </summary>
    public class EventoDamageInspect : InterfaceCabecalhoDto<DamageInspect>
    {
        public EventoDamageInspect(DamageInspect body)
        {
            Body = body;
            Event = "DamageInspect";
        }

        public EventoDamageInspect()
        {
            Body = new DamageInspect();
            Event = "DamageInspect";
        }   

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Body.Container, "Conteiner", "ERR_N_ENCONTRADO_CONTEINER")
                .HasMaxLengthIfNotNullOrEmpty(Body.Container, 12, "Container", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .IsTrue(Body.Damages?.Any() ?? false, "Damages", "ERR_AVAR_ID_DAMAGE_N")
            );

            Body.Damages?.ForEach(s =>
            {
                s.Validate();
                AddNotifications(s);
            });

            base.Validate();

        }
    }
    public class DamageInspect : CorpoEvento
    {
        public DamageInspect()
        {
            FilaHost = "event.damage.inspect.host";
            FilaTos = "event.damage.inspect.tos";
            Exchange = "event.damage.inspect";
        }
        public string Container { get; set; }
        public string VoyageImpBtpId { get; set; }
        public string VoyageExpBtpId { get; set; }
        public List<Damages> Damages { get; set; }
    }

    public class Damages : Notifiable, IValidatable
    {
        public string DamageBtpId { get; set; }
        public string DamageTosCode { get; set; }
        public DateTime Created { get; set; }
        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .AreNotEquals(Created, null, "created", "ERR_DT_AVARIA")
                .HasMaxLengthIfNotNullOrEmpty(DamageBtpId, 3, "damageBtpId", "ERR_NUMERO_MAXIMO_ENCONTRADO")
                .HasMaxLengthIfNotNullOrEmpty(DamageTosCode, 3, "DamageTosCode", "ERR_NUMERO_MAXIMO_ENCONTRADO"));
        }
    }
}
