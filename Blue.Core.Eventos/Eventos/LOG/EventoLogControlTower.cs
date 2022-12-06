using Blue.Core.Entidades.Auxiliares;
using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Enums;
using Flunt.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blue.Core.Eventos.Eventos.LOG
{
    public class EventoLogControlTower : InterfaceCabecalhoDto<LogControlTower>
    {
        public EventoLogControlTower()
        {
            ProcessType = "I";
            Event = "LogControlTower";
            Body = new LogControlTower();
        }

        public void FinalizaLog(List<decimal> idsAgendamentos, IReadOnlyCollection<Notification> notifications)
        {
            Body.Sucesso = !notifications.Any();
            Body.Erros = notifications.Select(n => n.Message).ToList();
            Body.IdsAgendamentos = idsAgendamentos;
        }
    }

    public class LogControlTower : CorpoEvento
    {
        public LogControlTower()
        {
            FilaHost = "event.log.host";
            Headers = new Dictionary<string, object> { { "x-log-type", LogTypeEnum.LOG_CONTROL_TOWER.ObterDescricao() } };
        }

        public bool Sucesso { get; set; }
        public List<decimal> IdsAgendamentos { get; set; }
        public List<string> Erros { get; set; }
    }

}
