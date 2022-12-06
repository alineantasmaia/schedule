using System.ComponentModel;

namespace Blue.Core.Entidades.Enums
{
    public enum FormaCalculoMemoriaCalculo
    {
        [Description("NENHUMA")] Nenhuma = 0,

        [Description("FIXO")] Fixo = 1,

        [Description("CIF")] Cif = 2,

        [Description("PRO RATA")] ProRata = 3
    }
}
