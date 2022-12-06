using Blue.Core.Entidades.Entidades.BLUE;
using Blue.Core.Repositorios.Interfaces;
using Blue.Agenda.Domain.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blue.Agenda.Infra.Dados.Repositorios
{
    public class RepositorioTipoErro : IRepositorioTipoErro
    {
        private readonly IRepositorioBaseEntity<TipoErro> _repositorioBaseEntity;

        public RepositorioTipoErro(IRepositorioBaseEntity<TipoErro> repositorioBaseEntity)
        {
            _repositorioBaseEntity = repositorioBaseEntity;
        }
        //public async Task<IEnumerable<TipoErro>> ObterListaTipoErro() => await _repositorioBaseEntity.ObterLista("SELECT * FROM TIPO_ERRO");
    }
}
