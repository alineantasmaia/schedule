using Blue.Core.Logs.Enumeradores;
using Blue.Core.Logs.Excecoes;
using Blue.Core.Logs.Notificacoes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Blue.Core.Logs.Auxiliares
{
    public class LogErro : Log
    {
        public LogErro(string mensagem, string detalhes, Exception excecao, HttpContext contexto) : base(mensagem, detalhes)
        {
            Excecao = excecao;
            ValoresRequisicao = ObterValoresDaRequisicao(contexto.Request);
            TipoLog = excecao is ExcecaoNegocio ? EnumTipoLog.Negocio : EnumTipoLog.Sistema;
        }

        public LogErro(string mensagem, string detalhes, Exception excecao) : base(mensagem, detalhes)
        {
            Excecao = excecao;
            TipoLog = excecao is ExcecaoNegocio ? EnumTipoLog.Negocio : EnumTipoLog.Sistema;
        }

        public LogErro(string mensagem, string detalhes, IEnumerable<Notificacao> notificacoes) : base(mensagem, detalhes)
        {
            Notificacoes = notificacoes;
            TipoLog = EnumTipoLog.Sistema;
        }

        public Exception Excecao { get; set; }
        public HttpContext Contexto { get; set; }
        public Dictionary<string, string> ValoresRequisicao { get; private set; }
        public IEnumerable<Notificacao> Notificacoes { get; set; }

        private Dictionary<string, string> ObterValoresDaRequisicao(HttpRequest requisicao)
        {
            var corpo = "N/A";
            var parametros = "N/A";
            var metodo = requisicao.Method;

            if (requisicao.ContentLength > 2 && !requisicao.HasFormContentType)
            {
                var streamCorpo = new StreamReader(requisicao.Body, Encoding.UTF8);
                corpo = streamCorpo.ReadToEnd();
            }

            if (requisicao.QueryString.HasValue)
                parametros = requisicao.QueryString.Value;

            return new Dictionary<string, string>()
            {
                { "metodo", metodo },
                { "corpo", corpo },
                { "parametrosUrl", parametros },
            };
        }
    }
}
