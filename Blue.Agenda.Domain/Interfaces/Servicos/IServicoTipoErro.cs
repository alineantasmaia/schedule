using Blue.Core.Logs.Notificacoes;
using Blue.Agenda.Domain.Dtos;
using Flunt.Notifications;
using System.Collections.Generic;

namespace Blue.Agenda.Domain.Interfaces.Servicos
{
    public interface IServicoTipoErro<T> where T : Evento
    {
        /// <summary>
        /// Propriedade que contem todas as notifica inseridas através dos métodos adicionar da classe
        /// </summary>
        IList<Notificacao> Notificacoes { get; set; }

        /// <summary>
        /// Caso tenha alguma notificação na lista de notificações o valor será false e caso não tenha sera true
        /// </summary>
        bool Valido { get; }

        /// <summary>
        /// Caso tenha alguma notificação na lista de notificações o valor será true e caso não tenha sera false
        /// </summary>
        bool Invalido { get; }

        /// <summary>
        /// Adiciona na lista um erro de sistema com base nas notifications para erros genericos, que quando retornado no método GerarListaNotificações será uma notificação do tipo Erro
        /// </summary>
        /// <param name="constante"></param>
        /// <param name="decricao"></param>
        /// <param name="evento"></param>
        void AddErros(IReadOnlyCollection<Notification> notificacoes, T evento = null);
        /// <summary>
        /// Adiciona na lista um erro de sistema, que quando retornado no método GerarListaNotificações será uma notificação do tipo Erro
        /// </summary>
        /// <param name="constante"></param>
        /// <param name="decricao"></param>
        /// <param name="evento"></param>
        void AddErro(string constante, string descricao = null, T evento = null);
        /// <summary>
        /// Adiciona na lista uma lista de erro de sistema, que quando retornado no método GerarListaNotificações será uma notificação do tipo Erro
        /// </summary>
        /// <param name="constante"></param>
        /// <param name="decricao"></param>
        /// <param name="evento"></param>
        void AddErros(List<string> constante, string descricoes = null, T evento = null);

        /// <summary>
        /// Adiciona na lista um erro de validação, que quando retornado no método GerarListaNotificações será uma notificação do tipo Validacao
        /// </summary>
        /// <param name="constante"></param>
        /// <param name="decricao"></param>
        /// <param name="evento"></param>
        /// <param name="erroConstanteGenerica"></param>
        void AddErroValidacao(string constante, string descricao = null, T evento = null, bool erroConstanteGenerica = false);
        /// <summary>
        /// Adiciona na lista uma lista de erro de validação, que quando retornado no método GerarListaNotificações será uma notificação do tipo Validacao
        /// </summary>
        /// <param name="constante"></param>
        /// <param name="decricao"></param>
        /// <param name="evento"></param>
        void AddErrosValidacao(List<string> constante, string descricoes = null, T evento = null);
        /// <summary>
        /// Adiciona na lista um erro de regra de negócio, que quando retornado no método GerarListaNotificações será uma notificação do tipo RegraNegocio
        /// </summary>
        /// <param name="constante"></param>
        /// <param name="decricao"></param>
        /// <param name="evento"></param>
        void AddErroRegraNegocio(string constante, string descricao = null, T evento = null);
        /// <summary>
        /// Adiciona na lista uma lista de erro de regra de negócio, que quando retornado no método GerarListaNotificações será uma notificação do tipo RegraNegocio
        /// </summary>
        /// <param name="constante"></param>
        /// <param name="decricao"></param>
        /// <param name="evento"></param>
        void AddErrosRegraNegocio(List<string> constante, string descricoes = null, T evento = null);

        /// <summary>
        /// Adiciona na lista um erro de validação para valores incosistentes, a constante informa define a origem dos valores, esse erro será retornado no método  
        /// GerarListaNotificações como uma notificação do tipo Validacao
        /// </summary>
        /// <param name="constante"></param>
        /// <param name="valorEsperado"></param>
        /// <param name="valorEncontrado"></param>
        void AddErroInconsistencia(string constante, string valorEsperado, string valorEncontrado);
    }
}
