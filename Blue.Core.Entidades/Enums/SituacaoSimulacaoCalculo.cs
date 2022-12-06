using System.ComponentModel;

namespace Blue.Core.Entidades.Enums
{
    public enum SituacaoSimulacaoCalculo
    {
        [Description("NENHUMA")] Nenhuma = 0,

        [Description("REALIZADA")] Realizada = 1,

        [Description("CONFIRMADA")] Confirmada = 2,

        [Description("COMPROVANTE ENVIADO")] ComprovanteEnviado = 3,

        [Description("EM ANALISE")] EmAnalise = 4,

        [Description("APROVADA")] Aprovada = 5,

        [Description("RECUSADA")] Recusada = 6
    }
}
