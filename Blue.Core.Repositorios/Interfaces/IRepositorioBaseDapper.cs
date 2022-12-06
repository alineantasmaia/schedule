using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Threading.Tasks;

namespace Blue.Core.Repositorios.Interfaces
{
    public interface IRepositorioBaseDapper:IDisposable
    {
        /// <summary>
        /// Executa a consulta enviada como parametro (A consulta deve retornar apenas UMA linha)
        /// </summary>
        /// <returns>
        /// Retorna o objeto passado como parametro com o resultado da consulta.
        /// </returns>
        Task<TResultado> Obter<TResultado>(string consulta);

        /// <summary>
        /// Executa a consulta enviada como parametro utilizando parâmetros dinâmicos (A consulta deve retornar apenas UMA linha)
        /// </summary>
        /// <returns>
        /// Retorna o objeto passado como parametro com o resultado da consulta.
        /// </returns>
        Task<TResultado> Obter<TResultado>(string consulta, object parametros);

        /// <summary>
        /// Executa a consulta enviada como parametro (A consulta deve retornar varias linha) 
        /// </summary>
        /// <returns>
        /// Retorna um IEnumerable do objeto passado como parametro com o resultado da consulta.
        /// </returns>
        Task<IEnumerable<TResultado>> ObterLista<TResultado>(string consulta);

        /// <summary>
        /// Executa a consulta enviada como parametro utilizando parâmetros dinâmicos (A consulta deve retornar varias linha) 
        /// </summary>
        /// <returns>
        /// Retorna um IEnumerable do objeto passado como parametro com o resultado da consulta.
        /// </returns>
        Task<IEnumerable<TResultado>> ObterLista<TResultado>(string consulta, object parametros);

        /// <summary>
        /// Executa a consulta enviada como parametro (Verifica se a consulta obteve resultado)
        /// </summary>
        /// <returns>
        /// Retorna bool de acordo com o resultado (TRUE - Existe, FALSE - Nao)
        /// </returns>
        Task<bool> Existe(string consulta);

        /// <summary>
        /// Executa a consulta enviada como parametro utilizando parâmetros dinâmicos (Verifica se a consulta obteve resultado)
        /// </summary>
        /// <returns>
        /// Retorna bool de acordo com o resultado (TRUE - Existe, FALSE - Nao)
        /// </returns>
        Task<bool> Existe(string consulta, object parametros);

        /// <summary>
        /// Executa a consulta enviada como parametro (A consulta deve retornar apenas UMA linha)
        /// </summary>
        /// <returns>
        /// Retorna o objeto passado como parametro com o resultado da consulta.
        /// </returns>
        Task<DataTable> Obter(string consulta);

        /// <summary>
        /// Executa a consulta enviada como parametro utilizando parâmetros dinâmicos (A consulta deve retornar apenas UMA linha)
        /// </summary>
        /// <returns>
        /// Retorna o objeto passado como parametro com o resultado da consulta.
        /// </returns>
        Task<DataTable> Obter(string consulta, object parametros);

        IDbConnection ObterConexao();
    }
}