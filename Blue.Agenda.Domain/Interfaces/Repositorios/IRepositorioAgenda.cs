using Blue.Agenda.Domain.Entidades;
using System.Threading.Tasks;
using Blue.Agenda.Domain.Dtos;
using System.Collections.Generic;

namespace Blue.Agenda.Domain.Interfaces.Repositorios
{
    public interface IRepositorioAgenda
    {
        Task<IEnumerable<Usuario>> ObterAgenda();
    }
}
