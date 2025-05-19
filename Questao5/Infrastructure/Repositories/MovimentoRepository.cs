using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Sqlite;
using System.Data;
using Dapper;

namespace Questao5.Infrastructure.Repositories
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly DatabaseConfig _config;

        public MovimentoRepository(DatabaseConfig config)
        {
            _config = config;
        }

        private IDbConnection CreateConnection() => new SqliteConnection(_config.Name);

        public async Task Inserir(Movimento movimento)
        {
            const string sql = @"
                                 INSERT INTO movimento 
                                 (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                                 VALUES 
                                 (@Id, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";

            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, movimento);
        }

        public async Task<decimal> CalcularSaldo(Guid idContaCorrente)
        {
            const string sql = @"
                                SELECT 
                                    COALESCE(SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE 0 END), 0) -
                                    COALESCE(SUM(CASE WHEN tipomovimento = 'D' THEN valor ELSE 0 END), 0) AS Saldo
                                FROM movimento
                                WHERE idcontacorrente = @id";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<decimal>(sql, new { id = idContaCorrente });
        }

        public async Task<Movimento?> ObterPorId(Guid idMovimento)
        {
            const string sql = @"
                                SELECT
                                    CAST(idmovimento AS TEXT) AS Id,
                                    idcontacorrente AS IdContaCorrente,
                                    tipomovimento AS TipoMovimento,
                                    datamovimento AS DataMovimento,
                                    valor AS Valor
                                FROM movimento
                                WHERE idmovimento = @id";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Movimento>(sql, new { id = idMovimento });
        }

    }
}
