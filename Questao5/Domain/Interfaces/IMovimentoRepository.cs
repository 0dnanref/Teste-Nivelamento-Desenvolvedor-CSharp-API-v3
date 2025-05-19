using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces
{
    public interface IMovimentoRepository
    {
        Task Inserir(Movimento movimento);
        Task<decimal> CalcularSaldo(Guid idContaCorrente);
        Task<Movimento?> ObterPorId(Guid idMovimento);
    }
}
