using System.ComponentModel;

namespace Blue.Core.Entidades.Enums
{
    public enum TipoInformacaoMemoriaCalculo
    {
        [Description("NENHUMA")] Nenhuma = 0,

        [Description("DIAS ARMAZENADOS")] DiasArmazenados = 1,

        [Description("PERÍODO")] Periodo = 2,

        [Description("DEDUÇÃO")] Deducao = 3,

        [Description("DIAS COBRADOS")] DiasCobrados = 4,

    }
}
