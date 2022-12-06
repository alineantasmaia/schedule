using Blue.Core.Logs.Enumeradores;

namespace Blue.Core.Logs.Auxiliares
{
    /// <summary>
    /// Classe reponsável por padronizar os logs de informação que serão armazenados no ElastichSearch
    /// </summary>
    public class LogInfo : Log
    {
        /// <summary>
        /// Construtor responsável por criar um objeto de informação, possibilitando passar um objeto que represente as notificaçções, além da titulo e dos mensagem
        /// </summary>
        /// <param name="titulo">Titulo do erro</param>
        /// <param name="mensagem">Explicação mais detalhada do erro</param>
        /// <param name="notificacoes">Objeto de terá as notificações acumaladas em um determinado processo</param>
        /// <param name="tipoLog">Indica se o log é nível de sistema ou de negócio</param>
        public LogInfo(string titulo, string mensagem, object notificacoes, EnumTipoLog tipoLog) : base(titulo, mensagem)
        {
            Notificacoes = notificacoes;
            TipoLog = tipoLog;
        }

        /// <summary>
        /// Construto responsável por permitir criar um log de informação apenas com a titulo e com os mensagem
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="mensagem"></param>
        /// <param name="tipoLog">Indica se o log é nível de sistema ou de negócio</param>
        public LogInfo(string titulo, string mensagem, EnumTipoLog tipoLog) : base(titulo, mensagem)
        {
            TipoLog = tipoLog;
        }

        public object Notificacoes { get; set; }
    }
}
