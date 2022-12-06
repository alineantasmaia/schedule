using Blue.Core.Entidades.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blue.Core.Eventos.Eventos
{
    public class EventoGateOutExpressResult : InterfaceCabecalhoDto<GateOutExpressResult>
    {
        public EventoGateOutExpressResult()
        {
            Event = "GateOutExpressResult";
        }

        public EventoGateOutExpressResult(GateOutExpressResult body)
        {
            Body = body;
            Event = "GateOutExpressResult";
        }
    }

    public class GateOutExpressResult : CorpoEvento
    {
        public GateOutExpressResult()
        {
            Headers = new Dictionary<string, object> { { "METHOD_NAME", "SendMessage" } };
        }

        public string GateNumber { get; set; }
        public DateTime Occurence { get; set; }
        public ResultKiosk Message { get; set; }
        public string SenderIdentity { get; set; }

        public void SetupQueue(string exchangeName, string queueName)
        {
            Exchange = exchangeName;
            FilaHost = queueName;
        }


    }

    public class ResultKiosk
    {
        public ResultKiosk()
        {

        }

        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class EventoGateOutExpressNotFoundTruck : InterfaceCabecalhoDto<GateOutExpressNotFoundTruck>
    {
        public EventoGateOutExpressNotFoundTruck(GateOutExpressNotFoundTruck body)
        {
            Body = body;
            Event = "GateOutExpressNotFoundTruck";
        }

        public EventoGateOutExpressNotFoundTruck()
        {
            Event = "GateOutExpressNotFoundTruck";
        }
    }

    public class GateOutExpressNotFoundTruck : CorpoEvento
    {
        public GateOutExpressNotFoundTruck()
        {
            Headers = new Dictionary<string, object> { { "METHOD_NAME", "SendNotFoundTruck" } };
        }
        public string GateNumber { get; set; }

        public void SetupQueue(string exchangeName, string queueName)
        {
            Exchange = exchangeName;
            FilaHost = queueName;
        }
    }
}
