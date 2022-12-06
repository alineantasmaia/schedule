using Blue.Core.Logs.Auxiliares;
using Blue.Core.Logs.Enumeradores;
using Blue.Core.Logs.Interfaces;
using Blue.Core.Logs.Notificacoes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blue.Core.Logs.Servicos
{
    /// <summary>
    /// Classe responsável por implementar as rotinas de logs para persistir no ElastichSearch
    /// </summary>
    /// <typeparam name="TEntidade">Classe atual que está fazendo o uso do serviço de log</typeparam>
    public class ServicoLogs<TEntidade> : IServicoLogs<TEntidade> where TEntidade : class
    {
        private readonly ILogger<TEntidade> _logger;

        public ServicoLogs(ILogger<TEntidade> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Método que recebe uma instância do objeto LogErro para ser persistido
        /// </summary>
        /// <param name="log">Objeto que representa um log de erro</param>
        public void LogErro(LogErro log)
        {
            _logger.LogError("{@log}", log);
        }


        /// <summary>
        /// Método que recebe uma instância do objeto LogErro para ser persistido
        /// </summary>
        /// <param name="log">Objeto que representa um log de Erro</param>
        public void LogErro(LogAde02 log)
        {
            _logger.LogError("{@log}", log);
        }

        /// <summary>
        /// Método responsável por receber os parâmetros necessários para que o objeto de LogErro seja criado
        /// </summary>
        /// <param name="titulo">Título do log</param>
        /// <param name="mensagem">Mensagem mais detalhada do log</param>
        /// <param name="excecao">Objeto que contém a exceção ocorrida</param>
        /// <param name="contexto">Contexto para que seja possível extrair os parâmetros da requisição em casos de aplicações WEB</param>
        public void LogErro(string titulo, string mensagem, Exception excecao, HttpContext contexto)
        {
            var log = new LogErro(titulo, mensagem, excecao, contexto);
            _logger.LogError("{@log}", log);
        }

        public void LogErro(string titulo, string mensagem, IEnumerable<Notificacao> notificacoes)
        {
            if (!notificacoes.Any())
                return;

            var log = new LogErro(titulo, mensagem, notificacoes);
            _logger.LogError("{@log}", log);
        }

        /// <summary>
        /// Método responsável por criar um log de Error
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="mensagem"></param>
        /// <param name="notificacoes"></param>
        public void LogErro(string titulo, string mensagem, IEnumerable<string> notificacoes)
        {
            if (!notificacoes.Any())
                return;

            var notificacoesLog = notificacoes.ToList().Select(n => new Notificacao(string.Empty, n, TipoNotificacao.Erro));
            var log = new LogErro(titulo, mensagem, notificacoesLog);
            _logger.LogError("{@log}", log);
        }


        /// <summary>
        /// Método que recebe uma instância do objeto LogErro para ser persistido
        /// </summary>
        /// <param name="log">Objeto que representa um log de erro</param>
        public void LogInfo(LogInfo log)
        {
            _logger.LogInformation("{@log}", log);
        }

        /// <summary>
        /// Método que recebe uma instância do objeto LogErro para ser persistido
        /// </summary>
        /// <param name="log">Objeto que representa um log de erro</param>
        public void LogInfo(LogAde02 log)
        {
            _logger.LogInformation("{@log}", log);
        }

        /// <summary>
        /// Método responsável por receber os parâmetros necessários para que o objeto de LogErro seja criado
        /// </summary>
        /// <param name="titulo">Título do log</param>
        /// <param name="mensagem">Mensagem mais detalhada do log</param>
        /// <param name="notificacoes">Objeto que contém as notificações retornadas para usuário</param>
        /// <param name="tipoLog">Indica se o log é nível de sistema ou de negócio</param>
        public void LogInfo(string titulo, string mensagem, IEnumerable<string> notificacoes, EnumTipoLog tipoLog = EnumTipoLog.Sistema)
        {
            if (!notificacoes.Any())
                return;

            var notificacoesLog = notificacoes.ToList().Select(n => new Notificacao(string.Empty, n, TipoNotificacao.Informacao));
            var log = new LogInfo(titulo, mensagem, notificacoesLog, tipoLog);
            _logger.LogInformation("{@log}", log);
        }

        /// <summary>
        /// Método responsável por receber os parâmetros necessários para que o objeto LogInfo seja criado
        /// </summary>
        /// <param name="titulo">Título do log</param>
        /// <param name="mensagem">Mensagem mais detalhada do log</param>
        /// <param name="tipoLog">Indica se o log é nível de sistema ou de negócio</param>
        public void LogInfo(string titulo, string mensagem, EnumTipoLog tipoLog = EnumTipoLog.Sistema)
        {
            var log = new LogInfo(titulo, mensagem, tipoLog);
            _logger.LogInformation("{@log}", log);
        }

        /// <summary>
        /// Método responsável por receber um objeto genérico e persisti no ElasticSearch
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="log">instância do objeto que será gravado</param>
        public void LogErro<T>(T log)
        {
           _logger.LogError("{@log}", log);
        }

        public void LogInfo<T>(T log)
        {
            _logger.LogInformation("{@log}", log);
        }
    }
}
