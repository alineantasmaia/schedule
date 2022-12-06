using Blue.Agenda.Domain.Dtos;
using Blue.Agenda.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blue.Agenda.Domain.Interfaces.Servicos
{
    public interface IServicoAgenda
    {
        Task<IEnumerable<Usuario>> ConsultarAgenda();
    }
}
