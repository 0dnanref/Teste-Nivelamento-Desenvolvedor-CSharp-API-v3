using Microsoft.Data.Sqlite;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Sqlite;
using System.Data;
using Dapper;

namespace Questao5.Infrastructure.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly DatabaseConfig _config;

        public IdempotenciaRepository(DatabaseConfig config)
        {
            _config = config;
        }

        private IDbConnection CreateConnection() => new SqliteConnection(_config.Name);

        public async Task<Guid?> ObterResultado(string chaveIdempotencia)
        {
            const string sql = "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @chave";

            using var connection = CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<string>(sql, new { chave = chaveIdempotencia });

            return Guid.TryParse(result, out var guid) ? guid : null;
        }

        public async Task SalvarResultado(string chaveIdempotencia, Guid idMovimento)
        {
            const string sql = @"INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                                 VALUES (@chave, '', @resultado)";

            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, new
            {
                chave = chaveIdempotencia.ToString(),
                resultado = idMovimento.ToString()
            });
        }
    }
}
