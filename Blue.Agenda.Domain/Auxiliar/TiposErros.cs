using Blue.Core.Entidades.Entidades.BLUE;
using Blue.Agenda.Domain.Interfaces.Repositorios;
using System.Collections.Generic;

namespace Blue.Agenda.Domain.Auxiliar
{
    public class TiposErros : ITiposErros
    {
        public IEnumerable<TipoErro> TipoErros { get; private set; }
        private readonly IRepositorioTipoErro _repositorioTipoErro;

        public TiposErros(IRepositorioTipoErro repositorioTipoErro)
        {
            _repositorioTipoErro = repositorioTipoErro;
            //ObterListaTipoErro();
        }

        //private void ObterListaTipoErro() => TipoErros = _repositorioTipoErro.ObterListaTipoErro().Result;


    }
}
