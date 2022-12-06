using Blue.Core.Entidades.Entidades.BLUE;
using System.Collections.Generic;

namespace Blue.Agenda.Domain.Auxiliar
{
    public interface ITiposErros
    {
        IEnumerable<TipoErro> TipoErros { get; }
    }
}
