using System;
using System.Text;

namespace Blue.Core.Eventos.Auxiliares
{
    public class ConfiguracaoRabbitMq
    {
        public ConfiguracaoRabbitMq(string hostName, string usuario, string senha, int porta, string nomeConexao, string virtualHost = null, string urlIntegracaoEvento = null)
        {
            HostName = hostName;
            Usuario = usuario;
            Senha = senha;
            Porta = porta;
            NomeConexao = nomeConexao;
            UrlIntegracaoEvento = urlIntegracaoEvento;
            if (!string.IsNullOrEmpty(virtualHost))
                VirtualHost = virtualHost;
        }


        public ConfiguracaoRabbitMq(string usuario, string senha, string virtualHost, string nomeConexao)
        {
            Usuario = usuario;
            Senha = senha;
            NomeConexao = nomeConexao;
            if (!string.IsNullOrEmpty(virtualHost))
                VirtualHost = virtualHost;
        }

        /// <summary>
        /// Virtual Host é responsável por isolar os componentes do RabbitMq, foi criado um virtual host para armazenar todos as exchanges, filas, usuários envolvidos na interface
        /// </summary>
        public string VirtualHost { get; private set; } = "interface";
        public string HostName { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public int Porta { get; set; }
        public string NomeConexao { get; set; }
        public string UrlIntegracaoEvento { get; set; }

        public void ConfigurarCredenciais(string usuario, string senha, string virtualHost, string nomeConexao)
        {
            Usuario = usuario;
            Senha = senha;
            VirtualHost = virtualHost;
            NomeConexao = nomeConexao;
        }
    }
}
