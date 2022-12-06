using Blue.Core.Repositorios;
using Blue.Agenda.Infra.Dados.Contextos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blue.Agenda.API.Configuracoes
{
    public static class RepositorioBaseConfiguracoes
    {
        public static void AddRepositorioBaseConfig(this IServiceCollection services, IConfiguration configuracao)
        {
            services.AddRepositoriosBase();

            //Entity FrameWork
            services.AddDbContext<ContextoEntity>(o => o.UseOracle(configuracao["StringConexao"], c => c.UseOracleSQLCompatibility("11")));            
            services.AddScoped<DbContext, ContextoEntity>();
        }
    }
}
