using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blue.Core.Logs.Notificacoes
{
    /// <summary>
    /// Classe reponsável por conter uma lista de notificações, todo objeto que herdar desta classe poderá ter notificações de diversos tipos
    /// </summary>
    public abstract class Notificavel
    {
        private readonly List<Notificacao> _notificacoes;

        protected Notificavel() => _notificacoes = new List<Notificacao>();

        [JsonIgnore]
        public IReadOnlyCollection<Notificacao> Notificacoes { get { return _notificacoes; } }

        /// <summary>
        /// Método que criar uma notificação com base nos parâmetros recebidos
        /// </summary>
        /// <param name="titulo">Título da notificação</param>
        /// <param name="mensagem">Detalhes da notificação</param>
        /// <param name="tipoNotificacao">Tipo de notificação sendo o padrão notificação de erro</param>
        public void AdicionarNotificacao(string titulo, string mensagem, TipoNotificacao tipoNotificacao = TipoNotificacao.Erro)
        {
            _notificacoes.Add(new Notificacao(titulo, mensagem, tipoNotificacao));
        }

        /// <summary>
        /// Método que recebe um objeto de notificação para ser inserido na lista
        /// </summary>
        /// <param name="notificacao"></param>
        public void AdicionarNotificacao(Notificacao notificacao)
        {
            _notificacoes.Add(notificacao);
        }

        /// <summary>
        /// Método que recebe a lista de mensagem que foram adicionadas no momento de Erro da aplicação
        /// </summary>
        /// <param name="notificacoes"></param>
        /// <param name="titulo"></param>
        public void AdicionarNotificacaoErro(IEnumerable<string> notificacoes, string titulo = null)
        {
            foreach (var notificacao in notificacoes)
                _notificacoes.Add(new Notificacao(titulo ?? string.Empty, notificacao, TipoNotificacao.Erro));
        }

        /// <summary>
        /// Método que recebe a lista de mensagem que foram adicionadas no momento de validar o objeto que contém os dados do evento
        /// </summary>
        /// <param name="notificacoes"></param>
        /// <param name="titulo"></param>
        public void AdicionarNotificacaoValidacao(IEnumerable<string> notificacoes, string titulo = null)
        {
            foreach (var notificacao in notificacoes)
                _notificacoes.Add(new Notificacao(titulo ?? string.Empty, notificacao, TipoNotificacao.Validacao));
        }

        /// <summary>
        /// Método que recebe uma mensagem que foram adicionadas no momento de validar o objeto que contém os dados do evento
        /// </summary>
        /// <param name="notificacao"></param>
        /// <param name="titulo"></param>
        public void AdicionarNotificacaoValidacao(string notificacao, string titulo = null) =>
                _notificacoes.Add(new Notificacao(titulo ?? string.Empty, notificacao, TipoNotificacao.Validacao));

        /// <summary>
        /// Método que recebe a lista de mensagem que foram adicionadas no momento de validar regra de negócio
        /// </summary>
        /// <param name="notificacoes"></param>
        /// <param name="titulo"></param>
        public void AdicionarNotificacaoRegraNegocio(IEnumerable<string> notificacoes, string titulo = null)
        {
            foreach (var notificacao in notificacoes)
                _notificacoes.Add(new Notificacao(titulo ?? string.Empty, notificacao, TipoNotificacao.RegraNegocio));
        }

        /// <summary>
        /// Método que recebe a lista de mensagem que foram adicionadas no momento de validar regra de negócio
        /// </summary>
        /// <param name="notificacao"></param>
        /// <param name="titulo"></param>
        public void AdicionarNotificacaoRegraNegocio(string notificacao, string titulo = null) =>
                _notificacoes.Add(new Notificacao(titulo ?? string.Empty, notificacao, TipoNotificacao.RegraNegocio));

        /// <summary>
        /// Método que recebe uma lista de noticações para serem unificadas em uma única lista
        /// </summary>
        /// <param name="notificacoes">Lista com as notificações</param>
        public void AdicionarNotificacoes(IReadOnlyCollection<Notificacao> notificacoes)
        {
            _notificacoes.AddRange(notificacoes);
        }

        /// <summary>
        /// Método que recebe uma lista de noticações para serem unificadas em uma única lista
        /// </summary>
        /// <param name="notificacoes">Lista com as notificações</param>
        public void AdicionarNotificacoes(IList<Notificacao> notificacoes)
        {
            _notificacoes.AddRange(notificacoes);
        }

        /// <summary>
        /// Método que recebe uma lista de noticações para serem unificadas em uma única lista
        /// </summary>
        /// <param name="notificacoes">Lista com as notificações</param>
        public void AdicionarNotificacoes(ICollection<Notificacao> notificacoes)
        {
            _notificacoes.AddRange(notificacoes);
        }

        /// <summary>
        /// Método que recebe um único objeto que herde da classe Notificavel e adiciona na lista de notificações
        /// </summary>
        /// <param name="item">Objeto que contém uma lista de notificações</param>
        public void AdicionarNotificacoes(Notificavel item)
        {
            if (item != null)
                AdicionarNotificacoes(item.Notificacoes);   
        }

        /// <summary>
        /// Método que recebe um ou vários objetos que herdam de Notificavel e acumula todas as notificações inseridas em cada um dos objetos
        /// </summary>
        /// <param name="items">Lista com vários objetos com notificações</param>
        public void AdicionarNotificacoes(params Notificavel[] items)
        {
            foreach (var item in items)
                AdicionarNotificacoes(item);
        }


        /// <summary>
        /// Propriedade que indica se o objeto está inválido
        /// </summary>
        [JsonIgnore]
        public bool Invalido => _notificacoes.Any(n => n.TipoNotificacao == TipoNotificacao.Erro || n.TipoNotificacao == TipoNotificacao.Validacao || n.TipoNotificacao == TipoNotificacao.RegraNegocio);
        [JsonIgnore]
        /// <summary>
        /// Propriedade que indica se o objeto está válido
        /// </summary>
        public bool Valido => !Invalido;
    }
}
