using Blue.Core.Eventos;
using Blue.Agenda.Domain.Auxiliar;
using Blue.Agenda.Domain.Interfaces.Repositorios;
using Blue.Agenda.Domain.Interfaces.Servicos;
using Blue.Agenda.Domain.Servicos;
using Blue.Agenda.Infra.Dados.Repositorios;
using Blue.Agenda.Infra.Servicos;
using Microsoft.Extensions.DependencyInjection;

namespace Blue.Agenda.API.Configuracoes
{
    public static class InjecaoDepedenciaConfiguracoes
    {
        public static void AddInjecaoDepedenciaConfig(this IServiceCollection services)
        {
            services.AddEnvioEventos();            
            services.AddScoped(typeof(IServicoTipoErro<>), typeof(ServicoTipoErro<>));            
            services.AddScoped<IServicoProvedorHttpClient, ServicoProvedorHttpClient>();
            //services.AddScoped<IRepositorioTipoErro, RepositorioTipoErro>();
            services.AddSingleton<ITiposErros, TiposErros>();
            //Manutencao
            //services.AddHostedService<ConsumirFila>();
            services.AddScoped<IServicoAgenda,ServicoAgenda>();
            services.AddScoped<IRepositorioAgenda,RepositorioAgenda>();
        }
    }
}