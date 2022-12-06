using Blue.Core.Repositorios.Auxiliares;
using Blue.Core.Repositorios.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Threading.Tasks;

namespace Blue.Core.Repositorios.Repositorios
{
    public class RepositorioBaseDapper : IRepositorioBaseDapper,IRepositorioBaseDapperSync
    {
        private readonly IDbConnection _connection;
        private readonly ILogger _logger;
        public RepositorioBaseDapper(DbContext contexto, ILogger<IRepositorioBaseDapper> logger)
        {
            _connection = contexto.Database.GetDbConnection();
            _logger = logger;
        }

        public RepositorioBaseDapper(string conexao)
        {
            _connection = new OracleConnection(conexao);
        }

        public IDbConnection ObterConexao() => _connection;

        /// <summary>
        /// Executa a consulta enviada como parametro (Verifica se a consulta obteve resultado)
        /// </summary>
        /// <returns>
        /// Retorna bool de acordo com o resultado (TRUE - Existe, FALSE - Nao)
        /// </returns>
        public async Task<bool> Existe(string consulta)
        {
            string consultaOriginal = string.Empty;
            object resultado = null;
            try
            {
                consultaOriginal = (string)consulta?.Clone();

                var helperSQL = new HelperSQL();
                consulta = helperSQL.TryParseOracle(consulta);

                resultado = await _connection.QueryFirstOrDefaultAsync<object>(consulta, helperSQL.ParametersOracle);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Erro ao tentar utilizar bind variable, sua consulta será utilizada diretamente");

                resultado = await _connection.QueryFirstOrDefaultAsync<object>(consultaOriginal);
            }

            return resultado != null;
        }
        /// <summary>
        /// Executa a consulta enviada como parametro (A consulta deve retornar apenas UMA linha)
        /// </summary>
        /// <returns>
        /// Retorna o objeto passado como parametro com o resultado da consulta.
        /// </returns>
        public async Task<TResultado> Obter<TResultado>(string consulta)
        {
            string consultaOriginal = string.Empty;

            try
            {
                consultaOriginal = (string)consulta?.Clone();

                var helperSQL = new HelperSQL();
                consulta = helperSQL.TryParseOracle(consulta);

                return await _connection.QueryFirstOrDefaultAsync<TResultado>(consulta, helperSQL.ParametersOracle);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Erro ao tentar utilizar bind variable, sua consulta será utilizada diretamente");

                return await _connection.QueryFirstOrDefaultAsync<TResultado>(consultaOriginal);
            }
        }
        /// <summary>
        /// Executa a consulta enviada como parametro (A consulta deve retornar varias linha) 
        /// </summary>
        /// <returns>
        /// Retorna um IEnumerable do objeto passado como parametro com o resultado da consulta.
        /// </returns>
        public async Task<IEnumerable<TResultado>> ObterLista<TResultado>(string consulta)
        {
            string consultaOriginal = string.Empty;

            try
            {
                consultaOriginal = (string)consulta?.Clone();

                var helperSQL = new HelperSQL();
                consulta = helperSQL.TryParseOracle(consulta);

                //Manutencao
                var cns = _connection.ConnectionString.ToString();
                _connection.Open();
                
                var trns = _connection.BeginTransaction();
                
                var res = await _connection.QueryAsync<TResultado>(consulta, helperSQL.ParametersOracle);
                return res;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Erro ao tentar utilizar bind variable, sua consulta será utilizada diretamente");

                return await _connection.QueryAsync<TResultado>(consultaOriginal);
            }
        }

        public async Task<DataTable> Obter(string consulta)
        {
            var helperSQL = new HelperSQL();
            consulta = helperSQL.TryParseOracle(consulta);

            var dataTable = new DataTable();
            var dataReader = await _connection.ExecuteReaderAsync(consulta, helperSQL.ParametersOracle);

            dataTable.Load(dataReader);

            return dataTable;
        }

        public TResultado ObterSync<TResultado>(string consulta)
        {
            string consultaOriginal = string.Empty;

            try
            {
                consultaOriginal = (string)consulta?.Clone();

                var helperSQL = new HelperSQL();
                consulta = helperSQL.TryParseOracle(consulta);

                return _connection.QueryFirstOrDefault<TResultado>(consulta, helperSQL.ParametersOracle);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Erro ao tentar utilizar bind variable, sua consulta será utilizada diretamente");

                return _connection.QueryFirstOrDefault<TResultado>(consultaOriginal);
            }
        }

        public IEnumerable<TResultado> ObterListaSync<TResultado>(string consulta)
        {
            string consultaOriginal = string.Empty;

            try
            {
                consultaOriginal = (string)consulta?.Clone();

                var helperSQL = new HelperSQL();
                consulta = helperSQL.TryParseOracle(consulta);

                return _connection.Query<TResultado>(consulta, helperSQL.ParametersOracle);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Erro ao tentar utilizar bind variable, sua consulta será utilizada diretamente");

                return _connection.Query<TResultado>(consultaOriginal);
            }
        }

        public bool ExisteSync(string consulta)
        {
            string consultaOriginal = string.Empty;
            object resultado = null;

            try
            {
                consultaOriginal = (string)consulta?.Clone();

                var helperSQL = new HelperSQL();
                consulta = helperSQL.TryParseOracle(consulta);

                resultado = _connection.QueryFirstOrDefault<object>(consulta);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Erro ao tentar utilizar bind variable, sua consulta será utilizada diretamente");

                resultado = _connection.QueryFirstOrDefault<object>(consultaOriginal);
            }

            return resultado != null;
        }

        public async Task<TResultado> Obter<TResultado>(string consulta, object parametros)
        {
            return await _connection.QueryFirstOrDefaultAsync<TResultado>(consulta, parametros);
        }

        public async Task<DataTable> Obter(string consulta, object parametros)
        {
            var dataTable = new DataTable();
            var dataReader = await _connection.ExecuteReaderAsync(consulta, parametros);

            dataTable.Load(dataReader);

            return dataTable;
        }

        public TResultado ObterSync<TResultado>(string consulta, object parametros)
        {
            return _connection.QueryFirstOrDefault<TResultado>(consulta, parametros);
        }

        public async Task<IEnumerable<TResultado>> ObterLista<TResultado>(string consulta, object parametros)
        {
            return await _connection.QueryAsync<TResultado>(consulta, parametros);
        }

        public IEnumerable<TResultado> ObterListaSync<TResultado>(string consulta, object parametros)
        {
            return _connection.Query<TResultado>(consulta, parametros);
        }

        public async Task<bool> Existe(string consulta, object parametros)
        {
            var resultado = await _connection.QueryFirstOrDefaultAsync<object>(consulta, parametros);

            return resultado != null;
        }

        public bool ExisteSync(string consulta, Object parametros)
        {
            var resultado = _connection.QueryFirstOrDefault<object>(consulta, parametros);

            return resultado != null;
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
