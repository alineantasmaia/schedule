using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blue.Core.Logs.Notificacoes
{
    /// <summary>
    /// Enumerador que representa quais tipos de notificações é possível criar
    /// </summary>
    public enum TipoNotificacao
    {
        Erro = 1,
        Informacao = 2,
        Validacao = 3,
        RegraNegocio = 4
    }
}
