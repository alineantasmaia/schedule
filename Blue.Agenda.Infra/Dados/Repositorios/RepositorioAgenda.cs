using Blue.Core.Repositorios;
using Blue.Core.Repositorios.Interfaces;
using Blue.Agenda.Domain.Dtos;
using Blue.Agenda.Domain.Entidades;
using Blue.Agenda.Domain.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Blue.Agenda.Infra.Dados.Repositorios
{
    public class RepositorioAgenda : IRepositorioAgenda
    {
        private readonly IRepositorioBaseDapper _repositorioAgendaDapper;

        public RepositorioAgenda(IRepositorioBaseDapper repositorioScoreDapper)
        {
            _repositorioAgendaDapper = repositorioScoreDapper;
        }

        #region Consultas
        public Task<IEnumerable<Usuario>> ObterAgenda()
        {
            var result = Task.FromResult(_repositorioAgendaDapper.ObterLista<Usuario>($@"SELECT ID ID,NOME NOME,EMAIL EMAIL,FONE FONE FROM SYS.AGENDATBL")).Result;

            return result;
        }
        #endregion

    }

}
