using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Blue.Core.Entidades.Enums
{
    public enum StatusEventoEnum
    {
        [Description("PROCESSANDO")] Processando = 0,
        [Description("SUCESSO")] Sucesso = 1,
        [Description("FALHA")] Falha = 2,
        [Description("AGUARDANDO")] Aguardando = 3,
        [Description("AGUARDANDO ENVIO SAP")] AguardandoSAP = 4,
        [Description("PROCESSANDO ENVIO SAP")] ProcessandoSAP = 5
    }
}
