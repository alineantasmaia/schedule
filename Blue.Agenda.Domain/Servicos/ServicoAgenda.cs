using Blue.Agenda.Domain.Dtos;
using Blue.Agenda.Domain.Entidades;
using Blue.Agenda.Domain.Interfaces.Repositorios;
using Blue.Agenda.Domain.Interfaces.Servicos;
using System.Collections.Generic;
//using Blue.Agenda.Domain.Auxiliar.Adapters;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Blue.Agenda.Domain.Servicos
{
    public class ServicoAgenda : IServicoAgenda
    {
        IRepositorioAgenda _repositorioAgenda;

        public ServicoAgenda(IRepositorioAgenda repositorioAgenda)
        {
            _repositorioAgenda = repositorioAgenda;
        }
        public Task<IEnumerable<Usuario>> ConsultarAgenda()
        {
            return _repositorioAgenda.ObterAgenda();
        }
    }
}
