using Blue.Core.Logs.Auxiliares;
using Blue.Core.Logs.Enumeradores;
using Blue.Core.Logs.Interfaces;
using Blue.Core.Logs.Servicos;
using Blue.Core.Logs.Skins;
using Destructurama;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using System;
using Blue.Core.Logs.Skins.Extensions;
using Blue.Core.Logs.Skins.RabbitMq;
using RabbitMQ.Client;
using Log = Serilog.Log;
using Serilog.Events;

namespace Blue.Core.Logs
{
    public static class RegistrarLogs
    {
        private const string ElasticUriDev = "https://elastic-dev.com.br:9200/";
        private const string ElasticUriProd = "http://elastic.com.br:9200/";
        private const string ElasticUriQas = "http://elastic-qas.com.br:9200/";
        private const string ElasticUserName = "elastic";
        private const string ElasticPassword = "tcukG0NDudoGF03giCSV";
        private const string ConstanteLog = "logs_";
        private const string ChaveValidacao = "event_name";

        /// <summary>
        /// Método de extensão que permite registrar a DI (Injeção de Dependência) para registrar Logs de aplicações via Elasticsearch 
        /// </summary>
        /// <param name="services">IServiceCollection auxilia na resolução das dependências</param>
        /// <returns></returns>
        public static IServiceCollection AdicionarLogs(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IServicoLogs<>), typeof(ServicoLogs<>));
            return services;
        }

        /// <summary>
        /// Método de extensão que configura os Logs de aplicações via Elasticsearch
        /// Url Prod:"https://elastic.com.br:9200"
        /// Url Dev: "https://elastic-dev.com.br:9200"
        /// Url QAS: "https://elastic-qas.com.br:9200"
        /// </summary>
        /// <param name="services">IServiceCollection auxilia na resolução das dependências</param>
        /// <param name="app">Injecao do IApplicationBuilder</param>
        /// <param name="ambiente">IHostingEnvironment para identificar o ambiente em execução</param>
        /// <param name="loggerFactory">ILoggerFactory para registrar o uso do Serilog</param>
        /// <param name="indexName">Nome do Indice da aplicação para agrupar os logs gerados no Elasc/Kibana.
        /// Caso seja informado o nome junto com -{0:yyyy.MM.dd} sera criado (agrupamento) um index por dia Ex: "meuindice-{0:yyyy.MM.dd}"</param>
        /// <param name="elasticsearch">URL para sobreescrever a utilização das URLs Default</param>
        /// <returns></returns>
        public static IApplicationBuilder ConfigurarLogs(this IApplicationBuilder app, IHostingEnvironment ambiente, ILoggerFactory loggerFactory, string indexName, string elasticsearch = "")
        {
            elasticsearch = string.IsNullOrEmpty(elasticsearch) ? ObterStringConexaoElastic(ambiente) : elasticsearch;

            CriarLogger(indexName, elasticsearch);

            loggerFactory.AddSerilog();
            return app;
        }

        /// <summary>
        /// Método de extensão que configura os Logs de aplicações via Elasticsearch
        /// Url Prod:"https://elastic.com.br:9200"
        /// Url Dev: "https://elastic-dev.com.br:9200"
        /// Url QAS: "https://elastic-qas.com.br:9200"
        /// </summary>
        /// <param name="services">IServiceCollection auxilia na resolução das dependências</param>
        /// <param name="app">Injecao do IApplicationBuilder</param>
        /// <param name="ambiente">string para identificar o ambiente em execução (Development, Staging ou Production) Exemplo - env.EnvironmentName</param>
        /// <param name="loggerFactory">ILoggerFactory para registrar o uso do Serilog</param>
        /// <param name="indexName">Nome do Indice da aplicação para agrupar os logs gerados no Elasc/Kibana.
        /// Caso seja informado o nome junto com -{0:yyyy.MM.dd} sera criado (agrupamento) um index por dia Ex: "meuindice-{0:yyyy.MM.dd}"</param>
        /// <param name="elasticsearch">URL para sobreescrever a utilização das URLs Default</param>
        /// <returns></returns>
        public static IApplicationBuilder ConfigurarLogs(this IApplicationBuilder app, string ambiente, ILoggerFactory loggerFactory, string indexName, string elasticsearch = "")
        {
            elasticsearch = string.IsNullOrEmpty(elasticsearch)
                ? ObterStringConexaoElastic(ExtensaoEnum.ObterValorApartirDescricao<EnumEnvironment>(ambiente.ToUpper()))
                : elasticsearch;

            CriarLogger(indexName, elasticsearch);

            loggerFactory.AddSerilog();
            return app;
        }

        /// <summary>
        /// Este método será utilizado para configurar o log para aplicações diferente de web que não utilizam IApplicationBuilder
        /// </summary>
        /// <param name="logging">Objeto que representa a configuração do log da aplicação</param>
        /// <param name="ambiente">Os valores desse parâmetro deve ser preenchido como Development, Staging ou Production</param>
        /// <param name="indexName">Nome do Indice da aplicação para agrupar os logs gerados no Elasc/Kibana.
        /// Caso seja informado o nome junto com -{0:yyyy.MM.dd} sera criado (agrupamento) um index por dia Ex: "meuindice-{0:yyyy.MM.dd}"</param>
        /// <param name="elasticsearch">URL para sobreescrever a utilização das URLs Default</param>
        /// <returns></returns>
        public static ILoggingBuilder ConfigurarLogs(this ILoggingBuilder logging, string ambiente, string indexName, string elasticsearch = "")
        {
            elasticsearch = string.IsNullOrEmpty(elasticsearch) ?
                ObterStringConexaoElastic(ExtensaoEnum.ObterValorApartirDescricao<EnumEnvironment>(ambiente.ToUpper())) :
                elasticsearch;

            CriarLogger(indexName, elasticsearch);

            logging.AddSerilog();
            return logging;
        }

        /// <summary>
        /// Método de extensão que configura os Logs de aplicações via RabbitMq
        /// </summary>
        /// <param name="app">Injecao do IApplicationBuilder</param>
        /// <param name="loggerFactory">ILoggerFactory para registrar o uso do Serilog</param>
        /// <param name="connection">IConnection do RabbitMq.Client para aceitar a conexão corrente</param>
        /// <param name="options">Objeto com a configuração de Exchange e RoutingKey</param>
        /// <returns>IApplicationBuilder</returns>
        public static IApplicationBuilder ConfigurarLogs(this IApplicationBuilder app,
            ILoggerFactory loggerFactory,
            IConnection connection, RabbitMQSkinOptions options)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .Enrich.With(new RemoverPropriedadesPadrao())
                .WriteTo.RabbitMQ(options, connection)
                .CreateLogger();

            loggerFactory.AddSerilog();
            return app;
        }

        private static ElasticsearchSinkOptions ConfiguraEnvioElastic(string indexName, string elasticsearch)
        {
            return new ElasticsearchSinkOptions(new Uri(elasticsearch))
            {
                ModifyConnectionSettings = c => c.BasicAuthentication(ElasticUserName, ElasticPassword),
                FormatStackTraceAsArray = true,
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                FailureCallback = e =>
                {
                    Console.WriteLine($"******** Não foi possível enviar os logs para o Elastic {elasticsearch} ********" +
                                  $"\nMessage: {e.Exception?.Message}" +
                                  $"\nInnerException: {e.Exception?.InnerException} \n" +
                                  $"\nEvent: {e.MessageTemplate} \n");
                },
                EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                       EmitEventFailureHandling.RaiseCallback |
                                       EmitEventFailureHandling.WriteToFailureSink,
                CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                IndexFormat = indexName.ToLower(),
                TypeName = "doc",
                FailureSink = new ConsoleSkin(new JsonFormatter())
            };
        }

        private static string ObterStringConexaoElastic(IHostingEnvironment ambiente) => ambiente.IsDevelopmentCustom() ? ElasticUriDev
            : ambiente.IsStagingCustom()
                ? ElasticUriQas
                : ElasticUriProd;

        private static string ObterStringConexaoElastic(EnumEnvironment ambiente) => ambiente.IsDevelopment() ? ElasticUriDev
            : ambiente.IsStaging()
                ? ElasticUriQas
                : ElasticUriProd;

        private static void CriarLogger(string indexName, string elasticsearch)
        {
            Log.Logger = new LoggerConfiguration()
                .Destructure.JsonNetTypes()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.PreGate.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.PreGate}s", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.BookingData.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.BookingData}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.BundleContainer.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.BundleContainer}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.ContainerData.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.ContainerData}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.DamageInspect.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.DamageInspect}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.DangerousContainer.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.DangerousContainer}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.Discharge.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.Discharge}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.DischargeInformationContainer.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.DischargeInformationContainer}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.DischargePlan.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.DischargePlan}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !(IndicesElk.Gatein.Equals((@event as ScalarValue)?.Value as string) ||
                            IndicesElk.Gateout.Equals((@event as ScalarValue)?.Value as string)))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}gatein_out", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.HoldContainer.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.HoldContainer}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.Load.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.Load}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.LoadConfirmation.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.LoadConfirmation}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.PreAdvice.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.PreAdvice}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.SealInspect.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.SealInspect}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.Shifting.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.Shifting}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.SpecialService.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.SpecialService}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.Stacking.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.Stacking}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.VesselWorkstops.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.VesselWorkstops}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.WeightContainer.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.WeightContainer}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.PrintTicket.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.PrintTicket}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.ControlTower.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.ControlTower}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.UptimeWorker.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.UptimeWorker}", elasticsearch));
                })
                .WriteTo.Logger(x =>
                {
                    x.Filter.ByIncludingOnly(e => !e.Properties.ContainsKey(ChaveValidacao));
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic(indexName, elasticsearch));
                }).WriteTo.Logger(x =>
                {
                    LogEventPropertyValue @event;
                    x.Filter.ByIncludingOnly(evt =>
                    {
                        if (!evt.Properties.TryGetValue(ChaveValidacao, out @event) || !IndicesElk.TabelaPreco.Equals((@event as ScalarValue)?.Value as string))
                            return false;

                        return true;
                    });
                    x.WriteTo.Elasticsearch(ConfiguraEnvioElastic($"{ConstanteLog}{IndicesElk.TabelaPreco}", elasticsearch));
                })
                .CreateLogger();
        }
    }
}
