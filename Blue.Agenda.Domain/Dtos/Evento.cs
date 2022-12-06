using Blue.Core.Entidades.Dtos;
using System;

namespace Blue.Agenda.Domain.Dtos
{
    public class Evento : InterfaceCabecalhoDto<Body>
    {
        // O DTO DO EVENTO DEVERA SER CRIADO E UTILIZADO A PARTIR DO PACOTE DE EVENTOS (Btp.Core.Eventos)
    }

    public class Body : CorpoEvento
    {        
        public DateTime Created { get; set; }
        public DateTime? Clear { get; set; }
    }

}

