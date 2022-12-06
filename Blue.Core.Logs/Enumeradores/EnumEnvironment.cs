using System.ComponentModel;

namespace Blue.Core.Logs.Enumeradores
{
    public enum EnumEnvironment
    {
        [Description("DEVELOPMENT")]
        DEV = 1,
        [Description("STAGING")]
        QAS = 2,
        [Description("PRODUCTION")]
        PRD = 3
    }

    public static class Extensions
    {
        public static bool IsDevelopment(this EnumEnvironment ambiente)
        {
            return ambiente == EnumEnvironment.DEV;
        }

        public static bool IsStaging(this EnumEnvironment ambiente)
        {
            return ambiente == EnumEnvironment.QAS;
        }

        public static bool IsProduction(this EnumEnvironment ambiente)
        {
            return ambiente == EnumEnvironment.PRD;
        }
    }
}
