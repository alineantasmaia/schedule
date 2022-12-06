using Blue.Core.Entidades.Auxiliares;
using Blue.Core.Eventos.Auxiliares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Blue.Agenda.API.Configuracoes
{
    public static class SwaggerConfiguracoes
    {
        public static void AddSwagerConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddApiVersioning(
               opcoes =>
               {
                   opcoes.AssumeDefaultVersionWhenUnspecified = true;
                   opcoes.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                   opcoes.ReportApiVersions = true;
               });

            services.AddVersionedApiExplorer(opcoes =>
            {
                opcoes.GroupNameFormat = "'v'VVV";
                opcoes.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                options.OperationFilter<HeaderVersionApiFilter>();
                options.OperationFilter<AuthorizeCheckOperationFilter>();


                var dir = AppContext.BaseDirectory;
                var paths = Directory.GetFiles(dir, "*.xml", SearchOption.TopDirectoryOnly);
                foreach (var path in paths) options.IncludeXmlComments(path);


                var scopesList = configuration["Seguranca:ServidorIdentidade:EscopoAcesso"].Split(" ").ToDictionary(x =>
                {
                    return x;
                });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{configuration["Seguranca:ServidorIdentidade:Endpoint"]}/connect/authorize"),
                            Scopes = new Dictionary<string, string>
                            {
                                { configuration["Seguranca:ServidorIdentidade:EscopoAcesso"], "Swagger Tabela" }
                            }
                        },
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri($"{configuration["Seguranca:ServidorIdentidade:Endpoint"]}/connect/token"),
                            Scopes = scopesList
                        }
                    }

                });

            });
        }

        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app, IApiVersionDescriptionProvider provider, IConfiguration configuracao)
        {
            app.UseSwagger(config =>
            {
                config.RouteTemplate = configuracao["Swagger:RotaTemplate"];
                config.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer
                        {
#if !DEBUG
                            Url = $"{configuracao["Seguranca:Gateway"]}{configuracao["Swagger:Rota"]}" 
#else
                             Url = $"http://{httpReq.Host.Value}"
#endif

                        }
                    };
                });
            });

            app.UseSwaggerUI(
              options =>
              {
                  foreach (var description in provider.ApiVersionDescriptions)
                  {
                      //options.SwaggerEndpoint($"{configuracao["Swagger:JsonEndpoint"]}{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                      options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                  }

                  options.DocExpansion(DocExpansion.List);
                  options.RoutePrefix = configuracao["Swagger:Prefixo"];
                  options.OAuthClientId(configuracao["Swagger:OAuth:ClientId"]);
                  options.OAuthClientSecret(configuracao["Swagger:OAuth:ClientSecret"]);
                  options.OAuthAppName(configuracao["Swagger:OAuth:AppName"]);
                  options.OAuthScopeSeparator(" ");
                  options.OAuthUsePkce();
              });

            return app;
        }
    }

    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;
        private readonly IWebHostEnvironment _ambiente;
        private readonly IConfiguration _configuracao;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IWebHostEnvironment ambiente, IConfiguration configuracao)
        {
            this.provider = provider;
            _ambiente = ambiente;
            _configuracao = configuracao;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));

#if !DEBUG
            options.SwaggerGeneratorOptions.Servers = new List<OpenApiServer>()
            {
                 new() {Url = _configuracao["Seguranca:Url"] }
            };
#endif
        }

        OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = $"{_configuracao["Swagger:Titulo"]} ({_ambiente.EnvironmentName} | {Environment.MachineName})",
                Version = description.ApiVersion.ToString(),
                Description = $"{_configuracao["Swagger:Descricao"]}",
                Contact = new OpenApiContact() { Name = "BLUE IT", Email = "sistemas.ti@blue.com.br" }
            };

            if (description.IsDeprecated)
                info.Description += " Esta versão está obsoleta!";

            return info;
        }
    }

    public class SwaggerDefaultValues : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters == null)
                return;

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                if (parameter.Description == null)
                    parameter.Description = description.ModelMetadata?.Description;

                if (parameter.Schema.Default == null && description.DefaultValue != null)
                    parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());

                parameter.Required |= description.IsRequired;
            }
        }
    }

    public class FiltroIgnoraPropriedadeSwagger : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var todosTipos = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetExportedTypes());

            foreach (var definicao in swaggerDoc.Components.Schemas)
            {
                var tipo = todosTipos.FirstOrDefault(x => x.Name == definicao.Key);

                if (tipo != null)
                {
                    var propriedades = tipo.GetProperties();
                    foreach (var prop in propriedades.ToList())
                    {
                        if (Attribute.GetCustomAttribute(prop, typeof(Newtonsoft.Json.JsonIgnoreAttribute)) is Newtonsoft.Json.JsonIgnoreAttribute ignore ||
                            Attribute.GetCustomAttribute(prop, typeof(SwaggerIgnore)) is SwaggerIgnore ignoreInterface ||
                            Serializacao.PropriedadesIgnoradaSwagger.Contains(prop.Name))
                        {
                            definicao.Value.Properties.Remove(prop.Name);

                        }
                    }
                }
            }
        }
    }

    public class HeaderVersionApiFilter : IOperationFilter
    {
        private readonly IConfiguration _configuration;
        public HeaderVersionApiFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Subscription-Key",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString(_configuration["Seguranca:Subscription-Key"]),
                },
                Required = true,
                Description = "Chave que identifica o consumidor da API",

            });
        }
    }

    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"}
                                }
                        ] = new[] { "api-agenda" }
                    }
                };
        }
    }


}
