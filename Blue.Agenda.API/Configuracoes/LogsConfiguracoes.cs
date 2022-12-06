using Blue.Core.Logs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blue.Agenda.API.Configuracoes
{
    public static class LogsConfiguracoes
    {
        public static void AddLogs(this IServiceCollection services)
        {
            services.AdicionarLogs();
        }

        public static void ConfigLogs(this IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.ConfigurarLogs(env.EnvironmentName, loggerFactory, "tabela");
        }
    }
}
