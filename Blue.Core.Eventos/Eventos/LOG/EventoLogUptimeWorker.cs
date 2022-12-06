using Blue.Core.Entidades.Auxiliares;
using Blue.Core.Entidades.Dtos;
using Blue.Core.Entidades.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blue.Core.Eventos.Eventos.LOG
{
    public class EventoLogUptimeWorker : InterfaceCabecalhoDto<LogUptimeWorker>
    {
        public EventoLogUptimeWorker()
        {
            ProcessType = "I";
            Event = "LogUptimeWorker";
            Body = new LogUptimeWorker();
        }
    }

    public class LogUptimeWorker : CorpoEvento
    {
        public LogUptimeWorker()
        {
            FilaHost = "event.log.host";
            Headers = new Dictionary<string, object> { { "x-log-type", LogTypeEnum.LOG_UPTIME_WORKER.ObterDescricao() } };
        }

        public string WorkerName { get; set; }
        public JObject HealthReport { get; set; }

    }
}
