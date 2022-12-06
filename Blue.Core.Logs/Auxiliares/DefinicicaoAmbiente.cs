using Microsoft.AspNetCore.Hosting;

namespace Blue.Core.Logs.Auxiliares
{
    public static class DefinicaoAmbiente
    {
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

    }
}