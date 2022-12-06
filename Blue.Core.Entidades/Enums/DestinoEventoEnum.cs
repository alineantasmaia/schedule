using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Blue.Core.Entidades.Enums
{
    public enum DestinoEventoEnum
    {
        /// <summary>
        /// Valor que indica que o evento foi publicado SOMENTE na fila do HOST
        /// </summary>
        [Description("P_HOST")]
        P_HOST = 0,
        /// <summary>
        /// Valor que indica que o evento foi publicado SOMENTE na fila no TOS
        /// </summary>
        [Description("P_TOS")]
        P_TOS = 1,
        /// <summary>
        /// Valor que indica que o evento foi publicado para AMBAS as filas, TOS e HOST
        /// </summary>
        [Description("P_TOS_HOST")]
        P_TOS_HOST = 2,

    }
}
