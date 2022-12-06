using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blue.Core.Repositorios.Interfaces
{
    /// <summary>
    /// Uma instância do IRepositorioBaseEntity permite realizar as operações básicas de 'CRUD' (EntityFrameworkCore)
    /// tendo como base o contexto (banco/conexão) e a entidade (tabela)
    /// informados no momento de sua utilização
    /// </summary>
    /// <typeparam name="T">Entidade (tabela) a ser utilizada pelo repositório</typeparam>
    public interface IRepositorioBaseEntity<TEntidade> where TEntidade : EntidadeBase
    {
        /// <summary>
        /// Executa a rotina de INSERT com base na entidade informada
        /// </summary>
        /// <returns>
        /// Retorna a propria entidade já com o Id inserido.
        /// </returns>
        Task<TEntidade> Adicionar(TEntidade entidade);
        /// <summary>
        /// Executa a rotina de UPDATE com base na entidade informada
        /// </summary>
        /// <returns>
        /// Retorna a propria entidade já com o atualizada.
        /// </returns>
        Task<TEntidade> Atualizar(TEntidade entidade);
        /// <summary>
        /// Atualizar que permite ignorar propriedades no update
        /// </summary>
        /// <param name="entidade"></param>
        /// <param name="ignoraPropriedade"></param>
        /// <returns></returns>
        Task<TEntidade> Atualizar(TEntidade entidade, params Expression<Func<TEntidade, object>>[] ignoraPropriedade);
        /// <summary>
        /// Executa a rotina de UPDATE com base em um T-SQL
        /// </summary>
#if RUNNING_ON_STANDARD_20
        Task Atualizar(string query);
#endif
        /// <summary>
        /// Executa a rotina de DELETE com base na entidade informada
        /// </summary>
        Task Remover(TEntidade entidade);
        /// <summary>
        /// Executa a rotina de DELETE com base em um T-SQL
        /// </summary>
#if RUNNING_ON_STANDARD_20
        Task Remover(string query);
#endif
        /// <summary>
        /// Executa a consulta com base nos filtros informados
        /// </summary>
        /// <returns>
        /// Retorna uma lista da entidade assinada com base no filtro informado
        /// </returns>
        Task<IEnumerable<TEntidade>> ObterLista(Expression<Func<TEntidade, bool>> filtro);
        /// <summary>
        /// Executa a consulta com base nos filtros informados
        /// </summary>
        /// <returns>
        /// Retorna uma entidade assinada com base no filtro informado
        /// </returns>
        Task<TEntidade> Obter(Expression<Func<TEntidade, bool>> filtro);
        /// <summary>
        /// Executa a consulta com base em no id informado buscando da tabela de entidade assinada.
        /// </summary>
        /// <returns>
        /// Retorna o objeto da entidade de acordo com o id informado
        /// </returns>
        Task<TEntidade> ObterPorId(decimal id);

        /// <summary>
        /// Executa a consulta enviada como parametro.
        /// </summary>
        /// <returns>
        /// Entidade assinada com a primeira linha da consulta enviada por parametro.
        /// </returns>

#if RUNNING_ON_STANDARD_20
        Task<IEnumerable<TEntidade>> ObterLista(string consulta);
#endif

        /// <summary>
        /// Executa a consulta enviada como parametro.
        /// </summary>
        /// <returns>
        /// Entidade assinada com uma lista dos dados da consulta enviada por parametro.
        /// </returns>

#if RUNNING_ON_STANDARD_20
        Task<TEntidade> Obter(string consulta);
#endif

        /// <summary>
        /// Executa a rotina de INSERT em massa com base nas entidades informada
        /// </summary>
        /// <returns>
        /// Retorna as entidades já com o Id inserido.
        /// </returns>
        Task<IEnumerable<TEntidade>> AdicionarEmMassa(IEnumerable<TEntidade> entidades);
    }
}
