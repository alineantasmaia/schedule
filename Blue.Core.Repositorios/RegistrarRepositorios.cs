using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blue.Core.Repositorios.Auxiliares;
using Blue.Core.Repositorios.Interfaces;
using Blue.Core.Repositorios.Repositorios;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blue.Core.Repositorios
{
    public static class RegistrarRepositorios
    {
        public static IContainer Container;
        /// <summary>
        /// Método de extensão que permite utilizar DI (Injeção de Dependência) para criação das instâncias dos repositórios
        /// </summary>
        /// <param name="services">IServiceCollection auxilia na resolução das dependências</param>
        /// <returns></returns>
        public static IServiceCollection AddRepositoriosBase(this IServiceCollection services)
        {
            services.AddScoped<IRepositorioBaseDapper, RepositorioBaseDapper>();
            services.AddScoped<IRepositorioBaseDapperSync, RepositorioBaseDapper>();
            services.AddScoped(typeof(IRepositorioBaseEntity<>), typeof(RepositorioBaseEntity<>));
            
            //Manutencao
            //services.AddConfigOracleBindVariable();

            return services;
        }

        /// <summary>
        /// Método para que as aplicações que não utlizam IServiceCollection possam também registrar as depedências e utiliza-las
        /// nas aplicações
        /// </summary>
        /// <param name="conexao">String de conexão para acesso a base</param>
        public static void AddRepositoriosBase(string conexao)
        {
            var builder = new ContainerBuilder();
            var serviceCollection = new ServiceCollection();

            ConfigurarDependencias(serviceCollection, builder, conexao);

            Container = builder.Build();
        }

        public static IServiceCollection AddRepositoriosBase(this IServiceCollection services, string conexao)
        {
            services.AddScoped<IRepositorioBaseDapper>(r => new RepositorioBaseDapper(conexao));
            services.AddScoped<IRepositorioBaseDapperSync>(r => new RepositorioBaseDapper(conexao));

            return services;
        }

        /// <summary>
        /// Método para que as aplicações que já configuram um ContainerBuilder possam adicionar as dependências deste pacote
        /// nas aplicações
        /// </summary>
        /// <param name="builder">Instância do container DI (Autofac)</param>
        /// <param name="conexao">String de conexão para acesso a base</param>
        public static void AddRepositoriosBase(this ContainerBuilder builder, string conexao)
        {
            var serviceCollection = new ServiceCollection();

            ConfigurarDependencias(serviceCollection, builder, conexao);

            //Manutencao
            //AddConfigOracleBindVariable(serviceCollection);
        }

        private static void ConfigurarDependencias(ServiceCollection serviceCollection, ContainerBuilder builder, string conexao)
        {
            serviceCollection.AddScoped<IRepositorioBaseDapper>(r => new RepositorioBaseDapper(conexao));
            serviceCollection.AddScoped<IRepositorioBaseDapperSync>(r => new RepositorioBaseDapper(conexao));

            builder.Populate(serviceCollection);                       
        }

        public static IServiceCollection AddConfigOracleBindVariable(this IServiceCollection services)
        {
            Console.WriteLine("Iniciando configuração OracleBindVariable");

            var repositorio = services.BuildServiceProvider().CreateScope().ServiceProvider.GetService<IRepositorioBaseDapperSync>();
                        
            var query = "SELECT ID FROM SYS.AGENDATBL WHERE CHAVE = :p0";

            HelperSQL.EqualOperatorString = repositorio.ObterSync<string>(query, new { p0 = "EQUAL_OPERATOR_STRING_ORACLE" });
            HelperSQL.EqualOperatorNumber = repositorio.ObterSync<string>(query, new { p0 = "EQUAL_OPERATOR_NUMBER_ORACLE" });
            HelperSQL.InOperatorString = repositorio.ObterSync<string>(query, new { p0 = "IN_OPERATOR_STRING_ORACLE" });
            HelperSQL.InOperatorNumber = repositorio.ObterSync<string>(query, new { p0 = "IN_OPERATOR_NUMBER_ORACLE" });
            HelperSQL.IsMatchOperatorIn = repositorio.ObterSync<string>(query, new { p0 = "IS_MATCH_OPERATOR_IN_ORACLE" });
            HelperSQL.ReplaceValue = repositorio.ObterSync<string>(query, new { p0 = "REPLACE_VALUE_ORACLE" });
            HelperSQL.GetValue = repositorio.ObterSync<string>(query, new { p0 = "GET_VALUE_ORACLE" });

            Console.WriteLine("Finalizando configuração OracleBindVariable");

            return services;
        }
    }
}
