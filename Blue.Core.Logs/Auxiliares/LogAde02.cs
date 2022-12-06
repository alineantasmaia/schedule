using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Blue.Core.Logs.Auxiliares
{
    public class LogAde02 : Log
    {
        public LogAde02(string mensagem, string detalhes) : base(mensagem, detalhes)
        {  
            this.RetornoErros = new List<RetornoErro>();
        }

        public string NomeEndpoint { get; set; }
        public string IdEvento { get; set; }

        /// <summary>
        /// Atributo genérico quando não há IdMercadoria e IdAgendamento
        /// </summary>
        public string IdDefault { get; set; }

        /// <summary>
        /// Atributo comum à carga
        /// </summary>
        public string IdMercadoria { get; set; }        

        /// <summary>
        /// Atributo comum à acesso
        /// </summary>
        public string IdAgendamento { get; set; }

        /// <summary>
        /// Atributo comum à acesso
        /// </summary>
        public string Documento { get; set; }
        public string DocumentoEstrangeiro { get; set; }
        public string Placa { get; set; }
        public DateTime DataEnvio { get; set; }
        public string Protocolo
        {
            get
            {
                return RetornoSucesso != null ? RetornoSucesso.Protocolo : string.Empty;
            }
        }

        public TipoEvento TipoEvento { get; set; }
        public object PacoteEnvio { get; set; }
        public TipoStatus Status { get; set; }

        //Pode receber valor nulo
        public TipoErro? TipoErro { get; set; }
        public RetornoSucesso RetornoSucesso { get; set; }

        /// <summary>
        /// Atributo retornado em JSON como errosValidação
        /// </summary>
        [JsonProperty(PropertyName = "errosValidacao")]
        public List<RetornoErro> RetornoErros { get; set; }

        public Exception Excecao { get; set; }
    }


    public class RetornoSucesso
    {
        public string DataHoraTransmissao { get; set; }
        public string Protocolo { get; set; }
        public CabecalhoRequisicao CabecalhoRequisicao { get; set; }
    }

    public class CabecalhoRequisicao
    {
        public string TipoOperacao { get; set; }
        public string IdEvento { get; set; }
        public string DataHoraOcorrencia { get; set; }
        public string DataHoraRegistro { get; set; }
        public string CpfOperadorOcorrencia { get; set; }
        public string CpfOperadorRegistro { get; set; }
        public string protocoloEventoRetificadoOuExcluido { get; set; }
        public bool Contingencia { get; set; }
        public string CodigoRecinto { get; set; }
    }




    public class RetornoErro
    {
        public decimal Codigo { get; set; }
        public string Atributo { get; set; }
        public string Detalhes { get; set; }
    }

    public enum TipoEvento
    {
        Carga = 0,
        Acesso = 1
    }

    public enum TipoStatus
    {
        Sucesso,
        Erro,
    }

    public enum TipoErro
    {
        Abtra,
        Negocio,
        Sistema
    }
}
