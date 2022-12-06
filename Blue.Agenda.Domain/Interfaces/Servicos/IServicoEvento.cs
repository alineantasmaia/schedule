using Blue.Core.Entidades.Dtos;
using Blue.Agenda.Domain.Dtos;
using System.Threading.Tasks;

namespace Blue.Agenda.Domain.Interfaces.Servicos
{
    public interface IServicoEvento
    {
        Task Executar(Evento evento);
    }
}
