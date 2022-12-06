using Blue.Core.Entidades.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blue.Core.Eventos.Eventos
{
    public class EventoPrintTicket : InterfaceCabecalhoDto<PrintTicket>
    {
        public EventoPrintTicket(PrintTicket body)
        {
            Body = body;
            Event = "PrintTicket";
        }
    }

    public class PrintTicket : CorpoEvento
    {
        public PrintTicket()
        {
        }

        public PrintTicket(string direction, string identificador, bool gate, bool buscarLocalizacaoTos, bool manual = true)
        {
            GetLocationTos = buscarLocalizacaoTos;
            Direction = direction;
            Exchange = "event.ticket.gate";

            Headers.Add("printing-manual", manual);
            
            if (!gate)
            {
                FilaHost = $"event.ticket.gate.btp.{identificador.ToLower()}";
                return;
            }

            switch (Direction)
            {
                case "IN":
                    FilaHost = $"gatein.ticket.{identificador.ToLower()}";
                    break;

                case "OUT":
                    FilaHost = $"gateout.ticket.{identificador.ToLower()}";
                    break;

                default:
                    throw new Exception("Direção no gate não está válida");
            }
        }

        public string VisitCode { get; set; }
        public string Direction { get; set; }
        public bool GetLocationTos { get; set; }
    }
}
