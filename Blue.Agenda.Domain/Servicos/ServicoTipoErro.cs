using Blue.Core.Logs.Notificacoes;
using Blue.Agenda.Domain.Auxiliar;
using Blue.Agenda.Domain.Dtos;
using Blue.Agenda.Domain.Interfaces.Servicos;
using Flunt.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blue.Agenda.Domain.Servicos
{
    public class ServicoTipoErro<T> : IServicoTipoErro<T> where T : Evento
    {
        private readonly ITiposErros _tipoErro;
        public IList<TipoErroDto<T>> TipoErrosDto { get; private set; }
        public IList<Notificacao> Notificacoes { get; set; }
        public bool Valido => !Notificacoes.Any();
        public bool Invalido => Notificacoes.Any();

        public ServicoTipoErro(ITiposErros tipoErro)
        {
            _tipoErro = tipoErro;
            TipoErrosDto = new List<TipoErroDto<T>>();
            Notificacoes = new List<Notificacao>();
        }

        public void GerarNotificacao(TipoErroDto<T> tipoErro)
        {
            if (tipoErro.ErroIncosistencia)
            {
                tipoErro.Descricao = _tipoErro.TipoErros.FirstOrDefault(p => p.TierConstante == tipoErro.Constante)?
                    .TierDescricao?
                    .Replace("#ValorEsperado", tipoErro.ValorEsperado)
                    .Replace("#ValorEncontrado", tipoErro.ValorEncontrado);
                Notificacoes.Add(new Notificacao(tipoErro.Constante, tipoErro.Descricao, tipoErro.TipoNotificacao));
            }
            else if (tipoErro.ErroConstante)
            {
                var descricaoOriginal = tipoErro.Descricao;
                tipoErro.Descricao =
                    _tipoErro.TipoErros.FirstOrDefault(p => p.TierConstante == tipoErro.Constante)?.TierDescricao ??
                    string.Empty;
                tipoErro.Campos = tipoErro.Campos.Where(p => tipoErro.Descricao.Contains("#" + p.Campo)).ToList();
                foreach (var campo in tipoErro.Campos)
                {
                    tipoErro.Descricao = tipoErro.Descricao?.Replace('#' + campo.Campo, Convert.ToString(campo.Valor));
                    if (!Notificacoes.Any() || Notificacoes.Any(n => n.Mensagem != tipoErro.Descricao))
                        Notificacoes.Add(new Notificacao(tipoErro.Constante, tipoErro.Descricao,
                            tipoErro.TipoNotificacao));
                }

                if (!tipoErro.Campos.Any())
                    Notificacoes.Add(new Notificacao(tipoErro.Constante,
                        tipoErro.ErroConstanteGenerica
                            ? $"{tipoErro.Descricao} - Propriety: {descricaoOriginal}"
                            : tipoErro.Descricao, tipoErro.TipoNotificacao));
            }
        }

        public void AddErros(IReadOnlyCollection<Notification> notificacoes, T evento = null) =>
            notificacoes.ToList().ForEach((notificacao) =>
            {
                GerarNotificacao(new TipoErroDto<T>(notificacao.Message, notificacao.Property, TipoNotificacao.Erro,
                    evento, true));
            });

        public void AddErro(string constante, string descricoes = null, T evento = null) =>
            GerarNotificacao(new TipoErroDto<T>(constante, descricoes, TipoNotificacao.Erro, evento));

        public void AddErroValidacao(string constante, string descricoes = null, T evento = null,
            bool erroConstanteGenerica = false) =>
            GerarNotificacao(new TipoErroDto<T>(constante, descricoes, TipoNotificacao.Validacao, evento,
                erroConstanteGenerica));

        public void AddErroRegraNegocio(string constante, string descricoes = null, T evento = null) =>
            GerarNotificacao(new TipoErroDto<T>(constante, descricoes, TipoNotificacao.RegraNegocio, evento));

        public void AddErroInconsistencia(string constante, string valorEsperado, string valorEncontrado) =>
            GerarNotificacao(new TipoErroDto<T>(constante, TipoNotificacao.Validacao, valorEncontrado, valorEsperado));

        public void AddErros(List<string> constantes, string descricao = null, T evento = null) =>
            constantes.ForEach((constante) =>
            {
                GerarNotificacao(new TipoErroDto<T>(constante, descricao, TipoNotificacao.Erro, evento));
            });

        public void AddErrosValidacao(List<string> constantes, string descricao = null, T evento = null) =>
            constantes.ForEach((constante) =>
            {
                GerarNotificacao(new TipoErroDto<T>(constante, descricao, TipoNotificacao.Validacao, evento));
            });

        public void AddErrosRegraNegocio(List<string> constantes, string descricao = null, T evento = null) =>
            constantes.ForEach((constante) =>
            {
                GerarNotificacao(new TipoErroDto<T>(constante, descricao, TipoNotificacao.RegraNegocio, evento));
            });
    }
}
