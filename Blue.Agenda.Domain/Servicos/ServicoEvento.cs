using Blue.Agenda.Domain.Dtos;
using Blue.Agenda.Domain.Interfaces.Servicos;
using System.Threading.Tasks;

namespace Blue.Agenda.Domain.Servicos
{
    public class ServicoEvento : IServicoEvento
    {
        private readonly IServicoTipoErro<Evento> _servicoTipoErro;
        public ServicoEvento(IServicoTipoErro<Evento> servicoTipoErro)
        {
            _servicoTipoErro = servicoTipoErro;
        }

        public async Task Executar(Evento evento)
        {
            if (!evento.Valido())
            {
                _servicoTipoErro.AddErros(evento.Notifications, evento);
                return;
            }
        }
    }
}
