using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Blue.Core.Entidades.Enums
{
    public enum TipoEventoEnum
    {
        [Description("APONTAMENTO_NAVIO")] ApontamentoNavio = 0,
        [Description("ENVIO_NOTA_SAP")] EnvioNotaSap = 1,
        [Description("GERA_NOTA_COBRANCA")] GeraNotaCobranca = 2,
    }
}
