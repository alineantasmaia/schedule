using Blue.Core.Logs.Auxiliares;
using Blue.Core.Logs.Enumeradores;
using Blue.Core.Logs.Notificacoes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Blue.Core.Logs.Interfaces
{
    public interface IServicoLogs<TEntidade> where TEntidade : class
    {
        void LogErro(LogErro logErro);
        void LogInfo(LogAde02 logInfo);
        void LogErro(string mensagem, string detalhes, Exception excecao, HttpContext contexto);
        void LogErro(string titulo, string mensagem, IEnumerable<Notificacao> notificacoes);
        void LogErro(string titulo, string mensagem, IEnumerable<string> notificacoes);
        void LogInfo(LogInfo logInfo);
        void LogErro(LogAde02 logErro);
        void LogInfo(string titulo, string mensagem, IEnumerable<string> notificacoes, EnumTipoLog tipoLog = EnumTipoLog.Sistema);
        void LogInfo(string titulo, string mensagem, EnumTipoLog tipoLog = EnumTipoLog.Sistema);
        void LogErro<T>(T log);
        void LogInfo<T>(T log);


    }
}
