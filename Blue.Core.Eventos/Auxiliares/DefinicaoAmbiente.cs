using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Blue.Core.Eventos.Auxiliares
{
    public static class DefinicaoAmbiente
    {
        public static string DEV { get; private set; } = "dev";
        public static string QAS { get; private set; } = "qas";
        public static string PRD { get; private set; } = "prd";

        public static bool IsDevelopmentCustom(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsDevelopment() 
                || "dev".Equals(hostingEnvironment.EnvironmentName.ToLower()) 
                || "local".Equals(hostingEnvironment.EnvironmentName.ToLower())
                || "localhost".Equals(hostingEnvironment.EnvironmentName.ToLower());
        }

        public static bool IsStagingCustom(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsStaging() || "qas".Equals(hostingEnvironment.EnvironmentName.ToLower()) || "qas".Equals(hostingEnvironment.EnvironmentName.ToLower());
        }

        public static bool IsProductionCustom(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsStaging() || "prd".Equals(hostingEnvironment.EnvironmentName.ToLower()) || "prod".Equals(hostingEnvironment.EnvironmentName.ToLower());
        }

        public static bool IsDevelopment(string ambiente)
        {
            return "dev".Equals(ambiente.ToLower())
                || "development".Equals(ambiente.ToLower())
                || "local".Equals(ambiente.ToLower())
                || "localhost".Equals(ambiente.ToLower());
        }

        public static bool IsStaging(string ambiente)
        {
            return "qas".Equals(ambiente.ToLower())
                || "staging".Equals(ambiente.ToLower());
        }

        public static bool IsProduction(string ambiente)
        {
            return "prd".Equals(ambiente.ToLower())
                || "prod".Equals(ambiente.ToLower())
                || "production".Equals(ambiente.ToLower());
        }
    }
}
