namespace Blue.Core.Logs.Notificacoes
{
    /// <summary>
    /// Classe responsável por representar uma notificação
    /// </summary>
    public sealed class Notificacao
    {
        public Notificacao()
        {

        }

        public Notificacao(string titulo, string mensagem, TipoNotificacao tipoNotificacao)
        {
            Titulo = titulo;
            Mensagem = mensagem;
            TipoNotificacao = tipoNotificacao;
        }

        public string Titulo { get; private set; }
        public string Mensagem { get; private set; }
        public TipoNotificacao TipoNotificacao { get; private set; }

    }
}
