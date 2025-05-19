using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly DatabaseConfig _config;

        public ContaCorrenteRepository(DatabaseConfig config)
        {
            _config = config;
        }

        private IDbConnection CreateConnection() => new SqliteConnection(_config.Name);

        public async Task<ContaCorrente?> ObterPorId(Guid id)
        {
            const string sql = @"SELECT idcontacorrente,
                                        numero AS Numero,
                                        nome AS Nome,
                                        ativo AS Ativo
                                 FROM contacorrente
                                 WHERE idcontacorrente = @id";

            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { id });
        }
    }
}
