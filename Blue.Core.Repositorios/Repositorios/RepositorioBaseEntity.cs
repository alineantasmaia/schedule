using Blue.Core.Repositorios.Auxiliares;
using Blue.Core.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blue.Core.Repositorios.Repositorios
{
    public class RepositorioBaseEntity<TEntidade> : IRepositorioBaseEntity<TEntidade> where TEntidade : EntidadeBase
    {
        private readonly DbSet<TEntidade> _dbSet;
        public DbContext _contexto;
        public RepositorioBaseEntity(DbContext contexto)
        {
            _contexto = contexto;
            _dbSet = _contexto.Set<TEntidade>();
        }
        /// <summary>
        /// Executa a rotina de INSERT com base na entidade informada
        /// beta test , change metodo AddAsync para Add- proposito testar upgrade entity e core net6
        /// </summary>
        /// <returns>
        /// Retorna a propria entidade já com o Id inserido.
        /// </returns>
        public async Task<TEntidade> Adicionar(TEntidade entidade)
        {
            _dbSet.Add(entidade);
            await _contexto.SaveChangesAsync();

            return entidade;
        }
        /// <summary>
        /// Executa a rotina de UPDATE com base na entidade informada
        /// </summary>
        /// <returns>
        /// Retorna a propria entidade já atualizada.
        /// </returns>
        public async Task<TEntidade> Atualizar(TEntidade entidade)
        {
            if (entidade == null)
                return await Task.FromResult((TEntidade)null);

            _dbSet.Attach(entidade);
            _contexto.Entry(entidade).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();

            return entidade;
        }

        /// <summary>
        /// Executa a rotina de UPDATE com base na entidade informada
        /// </summary>
        /// <returns>
        /// Retorna a propria entidade já atualizada.
        /// </returns>
        public async Task<TEntidade> Atualizar(TEntidade entidade, params Expression<Func<TEntidade, object>>[] ignoraPropriedade)
        {
            if (entidade == null)
                return await Task.FromResult((TEntidade)null);

            _dbSet.Attach(entidade);
            _contexto.Entry(entidade).State = EntityState.Modified;

            foreach (var prop in ignoraPropriedade)
            {
                var propInfo = HelperObject.ObterInfoPropriedade(entidade, prop);
                _contexto.Entry(entidade).Property(propInfo.Name).IsModified = false;
            }

            await _contexto.SaveChangesAsync();

            return entidade;
        }

        /// <summary>
        /// Executa a rotina de UPDATE com base em um T-SQL
        /// </summary>
#if RUNNING_ON_STANDARD_20
        public virtual async Task Atualizar(string query)
        {
            await _contexto.Database.ExecuteSqlRawAsync(query);
        }
#endif
        /// <summary>
        /// Executa a rotina de DELETE com base na entidade informada
        /// </summary>
        public virtual async Task Remover(TEntidade entidade)
        {
            _dbSet.Attach(entidade);
            _dbSet.Remove(entidade);
            await _contexto.SaveChangesAsync();
        }
        /// <summary>
        /// Executa a rotina de DELETE com base em um T-SQL
        /// </summary>
#if RUNNING_ON_STANDARD_20
        public virtual async Task Remover(string query)
        {
            await _contexto.Database.ExecuteSqlRawAsync(query);
        }
#endif
        /// <summary>
        /// Executa a consulta com base nos filtros informados
        /// </summary>
        /// <returns>
        /// Retorna uma lista da entidade assinada com base no filtro informado
        /// </returns>
        public async Task<IEnumerable<TEntidade>> ObterLista(Expression<Func<TEntidade, bool>> filtro)
        {
            return await _dbSet.AsNoTracking().Where(filtro).ToListAsync();
        }
        /// <summary>
        /// Executa a consulta com base em no id informado buscando da tabela de entidade assinada.
        /// </summary>
        /// <returns>
        /// Retorna o objeto da entidade de acordo com o id informado
        /// </returns>
        public async Task<TEntidade> ObterPorId(decimal id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// Executa a consulta enviada como parametro.
        /// </summary>
        /// <returns>
        /// Entidade assinada com a primeira linha da consulta enviada por parametro.
        /// </returns>

#if RUNNING_ON_STANDARD_20
        public async Task<TEntidade> Obter(string consulta)
        {
            return await _dbSet.FromSqlRaw(consulta).AsNoTracking().FirstOrDefaultAsync();
        }
#endif
        /// <summary>
        /// Executa a consulta enviada como parametro.
        /// </summary>
        /// <returns>
        /// Entidade assinada com uma lista dos dados da consulta enviada por parametro.
        /// </returns>
        /// 

#if RUNNING_ON_STANDARD_20
        public async Task<IEnumerable<TEntidade>> ObterLista(string consulta)
        {
            return await _dbSet.FromSqlRaw(consulta).AsNoTracking().ToListAsync();
        }
#endif

        public async Task<TEntidade> Obter(Expression<Func<TEntidade, bool>> filtro)
        {
            return await _dbSet.FirstOrDefaultAsync(filtro);
        }

        /// <summary>
        /// Executa a rotina de INSERT em massa com base nas entidades informada
        /// </summary>
        /// <returns>
        /// Retorna as entidades já com o Id inserido.
        /// </returns>
        public async Task<IEnumerable<TEntidade>> AdicionarEmMassa(IEnumerable<TEntidade> entidades)
        {
            await _dbSet.AddRangeAsync(entidades);
            await _contexto.SaveChangesAsync();

            return entidades;
        }
    }
}
