using System;
using System.Collections.Generic;
using Blue.Core.Entidades.Auxiliares;
using Blue.Core.Entidades.Dtos;
using Blue.Core.Eventos.Eventos;
using Flunt.Notifications;
using Newtonsoft.Json.Serialization;

namespace Blue.Core.Eventos.Auxiliares
{
    public static class Serializacao
    {
        /// <summary>
        /// Propriedades que não deverão aparecer na documentação do swagger
        /// </summary>
        public static List<string> PropriedadesIgnoradaSwagger = new List<string>
        {
            "Notifications", "Valid", "Invalid"
        };

        public static ConfiguracaoSerializacao Configuracao()
        {
            var ignoraPropriedadeContrato = new ConfiguracaoSerializacao();

            ignoraPropriedadeContrato.Ignore(typeof(Notifiable), "notifications");
            ignoraPropriedadeContrato.Ignore(typeof(Notifiable), "valid");
            ignoraPropriedadeContrato.Ignore(typeof(Notifiable), "invalid");
            ignoraPropriedadeContrato.Ignore(typeof(EventoLogs), "reprocessing");

            ignoraPropriedadeContrato.NamingStrategy = new CamelCaseNamingStrategy();

            return ignoraPropriedadeContrato;
        }
    }
}