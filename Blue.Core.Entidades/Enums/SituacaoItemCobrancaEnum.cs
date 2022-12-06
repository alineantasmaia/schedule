using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Blue.Core.Entidades.Enums
{
    public enum SituacaoItemCobrancaEnum
    {
        [Description("CANCELADO")] Cancelado,
        [Description("SIMULADO")] Simulado,
        [Description("IMPRESSO")] Impresso
    }
}
