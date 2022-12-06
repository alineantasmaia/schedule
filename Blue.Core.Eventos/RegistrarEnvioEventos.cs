using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Blue.Core.Eventos.Auxiliares;
using Blue.Core.Eventos.Interfaces;
using Blue.Core.Eventos.Servicos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Blue.Core.Eventos
{
    public static class RegistrarEnvioEventos
    {
        /// <summary>
        /// No dicionário abaixo será parametrizado a conexão com o RabbitMq em cada um dos ambientes
        /// </summary>
        private static IDictionary<string, ConfiguracaoRabbitMq> Configuracoes = new Dictionary<string, ConfiguracaoRabbitMq>()
        {
            { DefinicaoAmbiente.DEV, new ConfiguracaoRabbitMq("rabbitmq-dev.local", "", "", 30231, string.Empty, string.Empty, "api-integracaoevento-dev.com.br") },
            { DefinicaoAmbiente.QAS, new ConfiguracaoRabbitMq("rabbitmq-qas.local", "", "", 31608, string.Empty, string.Empty, "api-integracaoevento-qas.com.br") },
            { DefinicaoAmbiente.PRD, new ConfiguracaoRabbitMq("rabbitmq.local", "", "", 32605, string.Empty, string.Empty, "api-integracaoevento.com.br") },
        };

        public static ConfiguracaoRabbitMq Configuracao;
        public static IEnumerable<ConfiguracaoRabbitMq> PoolConfiguracoes;

        public static bool MultiConexoes
        {
            get
            {
                if (Configuracao != null && PoolConfiguracoes != null)
                    throw new Exception("Apenas uma estratégia de conexão é permitida");

                return PoolConfiguracoes != null;
            }
        }
        public static JsonSerializerSettings JsonSettings { get; set; } = new
                JsonSerializerSettings
        {
            ContractResolver = Serializacao.Configuracao()
        };

        public static IContainer Container;

        /// <summary>
        /// Método para ser utilizado em aplicações legadas, esse método deve ser chamado a classe principal da aplicação (normalmente é a Program)
        /// mas essa classe principal pode variar dependendo da arquitetura da aplicação. Utilize a classe DefinicaoAmbiente para escolher o ambiente desejado
        /// </summary>
        /// <param name="ambiente">Esse parâmetro indicará qual conexão do RabbitMQ será utilizado dependendo do ambiente, os valores para permitidos são Dev, Qas e Prd</param>
        /// <param name="configuracao">Esse parâmetro deve ser uma instância da classe ConfiguracaoRabbitMq para que possa ser utilizado uma conexão personalizada</param>

        public static void AddEnvioEventos(string ambiente, ConfiguracaoRabbitMq configuracao = null)
        {
            var builder = new ContainerBuilder();
            var serviceCollection = new ServiceCollection();
            var ambientePadrao = Configuracoes[ambiente.ToLower()];

            Configuracao = configuracao ?? ambientePadrao;

            if (!string.IsNullOrEmpty(configuracao?.Usuario))
                Configuracao.ConfigurarCredenciais(configuracao.Usuario, configuracao.Senha, configuracao.VirtualHost, configuracao.NomeConexao);

            if (string.IsNullOrEmpty(Configuracao.HostName))
            {
                Configuracao.HostName = ambientePadrao.HostName;
                Configuracao.Porta = ambientePadrao.Porta;
                Configuracao.UrlIntegracaoEvento = ambientePadrao.UrlIntegracaoEvento;
            }

            builder.Populate(serviceCollection);

            builder.RegisterType<ServicoEnvioEvento>()
                   .As<IServicoEnvioEvento>()
                   .EnableInterfaceInterceptors();

            Container = builder.Build();
        }

        public static IServiceCollection AddEnvioEventos(this IServiceCollection services, string ambiente, IEnumerable<ConfiguracaoRabbitMq> poolConfiguracoes, bool criarNovaConexao = true)
        {
            var builder = new ContainerBuilder();
            var serviceCollection = new ServiceCollection();

            var siglaAmbiente = DefinicaoAmbiente.IsDevelopment(ambiente) ? DefinicaoAmbiente.DEV : DefinicaoAmbiente.IsStaging(ambiente) ? DefinicaoAmbiente.QAS : DefinicaoAmbiente.PRD;

            var ambientePadrao = Configuracoes[siglaAmbiente];

            PoolConfiguracoes = poolConfiguracoes;

            foreach (var config in PoolConfiguracoes)
            {
                if (!string.IsNullOrEmpty(config?.Usuario))
                    config.ConfigurarCredenciais(config.Usuario, config.Senha, config.VirtualHost, config.NomeConexao);

                if (string.IsNullOrEmpty(config.HostName))
                {
                    config.HostName = ambientePadrao.HostName;
                    config.Porta = ambientePadrao.Porta;
                }
            }

            builder.Populate(serviceCollection);

            services.AddSingleton<IServicoEnvioEvento>(new ServicoEnvioEvento(criarNovaConexao));

            Container = builder.Build();

            return services;
        }


        /// <summary>
        /// Método para ser utilizado em aplicações que suportam DI (Injeção de Dependência) para que possa ser configurado uma conexão com o RabbitMq, deve ser utilizado para aplicações
        /// que consomem eventos do Rabbit
        /// </summary>
        /// <param name="services">IServiceCollection auxilia na resolução das dependências</param>
        /// <param name="ambiente">Esse parâmetro indicará qual conexão será utilizado dependendo do ambiente</param>
        /// <param name="configuracao">Esse parâmetro deve ser uma instância da classe ConfiguracaoRabbitMq para que possa ser utilizado uma conexão personalizada</param>
        /// <returns></returns>
        public static IServiceCollection AddEnvioEventos(this IServiceCollection services, IHostingEnvironment ambiente, IConfiguration arquivoConfiguracao, bool criarNovaConexao = false, ConfiguracaoRabbitMq configuracao = null)
        {
            var siglaAmbiente = ambiente.IsDevelopmentCustom() ? DefinicaoAmbiente.DEV : ambiente.IsStagingCustom() ? DefinicaoAmbiente.QAS : DefinicaoAmbiente.PRD;

            Configuracao = configuracao ?? Configuracoes[siglaAmbiente];

            Configuracao.ConfigurarCredenciais(arquivoConfiguracao["Mensageria:Usuario"], arquivoConfiguracao["Mensageria:Senha"],
                                        arquivoConfiguracao["Mensageria:VirtualHost"], arquivoConfiguracao["Mensageria:NomeConexao"]);
            services.AddSingleton<IServicoEnvioEvento>(new ServicoEnvioEvento(criarNovaConexao));

            return services;
        }

        /// <summary>
        /// Método para ser utilizado em aplicações que suportam DI (Injeção de Dependência) para que possa ser utilizada uma conexão com o RabbitMq já existente.
        /// </summary>
        /// <param name="services">IServiceCollection auxilia na resolução das dependências</param>
        /// <param name="criarNovaConexao">Determina se será utilizado um nova conexão dentro do pacote ou se será configurado o Canal de conexão com o RabbitMQ dentro do pacote</param>
        /// <returns></returns>
        public static IServiceCollection AddEnvioEventos(this IServiceCollection services, bool criarNovaConexao = false)
        {
            services.AddSingleton<IServicoEnvioEvento>(new ServicoEnvioEvento(criarNovaConexao));
            return services;
        }

        /// <summary>
        /// Este método de configução deve ser utilizando para aplicações que querem somente publicar eventos para o RabbitMQ 
        /// </summary>
        /// <param name="services">IServiceCollection auxilia na resolução das dependências</param>
        /// <param name="builder">Instancia de ContainerBuild para configurar o interceptador de armazenamento eventos</param>
        /// <param name="ambiente">Esse parâmetro indicará qual conexão será utilizado dependendo do ambiente</param>
        /// <param name="configuracao">Esse parâmetro deve ser uma instância da classe ConfiguracaoRabbitMq para que possa ser utilizado uma conexão personalizada</param>
        /// <returns></returns>
        public static IServiceCollection AddEnvioEventos(this IServiceCollection services, ContainerBuilder builder, IHostingEnvironment ambiente, ConfiguracaoRabbitMq configuracao = null)
        {
            builder.RegisterType<ServicoEnvioEvento>()
                   .As<IServicoEnvioEvento>()
                   .EnableInterfaceInterceptors();

            var siglaAmbiente = ambiente.IsDevelopmentCustom() ? DefinicaoAmbiente.DEV : ambiente.IsStagingCustom() ? DefinicaoAmbiente.QAS : DefinicaoAmbiente.PRD;

            Configuracao = configuracao ?? Configuracoes[siglaAmbiente];

            return services;
        }

        /// <summary>
        /// Este método de configução deve ser utilizando para aplicações que querem somente publicar eventos para o RabbitMQ 
        /// </summary>
        /// <param name="services">IServiceCollection auxilia na resolução das dependências</param>
        /// <param name="ambiente">Esse parâmetro indicará qual conexão será utilizado dependendo do ambiente</param>
        /// <param name="configuracao">Esse parâmetro deve ser uma instância da classe ConfiguracaoRabbitMq para que possa ser utilizado uma conexão personalizada</param>
        /// <param name="criarNovaConexao">Indica se será necessário a criação de uma nova conexão</param>
        /// <returns></returns>
        public static IServiceCollection AddEnvioEventos(this IServiceCollection services, string ambiente, IConfiguration arquivoConfiguracao, bool criarNovaConexao = true, ConfiguracaoRabbitMq configuracao = null, JsonSerializerSettings jsonSettings = null)
        {
            if (jsonSettings != null)
            {
                JsonSettings = jsonSettings;
            }

            var siglaAmbiente = DefinicaoAmbiente.IsDevelopment(ambiente) ? DefinicaoAmbiente.DEV : DefinicaoAmbiente.IsStaging(ambiente) ? DefinicaoAmbiente.QAS : DefinicaoAmbiente.PRD;

            Configuracao = configuracao ?? Configuracoes[siglaAmbiente];

            Configuracao.ConfigurarCredenciais(arquivoConfiguracao["Mensageria:Usuario"], arquivoConfiguracao["Mensageria:Senha"],
                                        arquivoConfiguracao["Mensageria:VirtualHost"], arquivoConfiguracao["Mensageria:NomeConexao"]);

            services.AddSingleton<IServicoEnvioEvento>(new ServicoEnvioEvento(criarNovaConexao));

            return services;
        }
    }
}
