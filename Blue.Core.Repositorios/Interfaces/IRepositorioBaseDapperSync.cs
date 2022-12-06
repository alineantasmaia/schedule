using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blue.Core.Repositorios.Interfaces
{
    public interface IRepositorioBaseDapperSync
    {
        /// <summary>
        /// Executa a consulta enviada como parametro (A consulta deve retornar apenas UMA linha)
        /// </summary>
        /// <returns>
        /// Retorna o objeto passado como parametro com o resultado da consulta.
        /// </returns>
        TResultado ObterSync<TResultado>(string consulta);
        /// <summary>
        /// Executa a consulta enviada como parametro utilizando Parametros Dinâmicos(A consulta deve retornar apenas UMA linha)
        /// </summary>
        /// <returns>
        /// Retorna o objeto passado como parametro com o resultado da consulta.
        /// </returns>
        TResultado ObterSync<TResultado>(string consulta, object parametros);
        /// <summary>
        /// Executa a consulta enviada como parametro (A consulta deve retornar varias linha) 
        /// </summary>
        /// <returns>
        /// Retorna um IEnumerable do objeto passado como parametro com o resultado da consulta.
        /// </returns>
        IEnumerable<TResultado> ObterListaSync<TResultado>(string consulta);
        /// <summary>
        /// Executa a consulta enviada como parametro utilizando Parametros Dinâmicos (A consulta deve retornar varias linha) 
        /// </summary>
        /// <returns>
        /// Retorna um IEnumerable do objeto passado como parametro com o resultado da consulta.
        /// </returns>
        IEnumerable<TResultado> ObterListaSync<TResultado>(string consulta, object parametros);
        /// <summary>
        /// Executa a consulta enviada como parametro (Verifica se a consulta obteve resultado)
        /// </summary>
        /// <returns>
        /// Retorna bool de acordo com o resultado (TRUE - Existe, FALSE - Nao)
        /// </returns>
        bool ExisteSync(string consulta);
        /// <summary>
        /// Executa a consulta enviada como parametro utilizando Parametros Dinâmicos (Verifica se a consulta obteve resultado)
        /// </summary>
        /// <returns>
        /// Retorna bool de acordo com o resultado (TRUE - Existe, FALSE - Nao)
        /// </returns>
        bool ExisteSync(string consulta, object parametros);
    }
}
