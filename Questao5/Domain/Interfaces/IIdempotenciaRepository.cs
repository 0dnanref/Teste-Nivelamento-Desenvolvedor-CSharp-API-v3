namespace Questao5.Domain.Interfaces
{
    public interface IIdempotenciaRepository
    {
        Task<Guid?> ObterResultado(string chaveIdempotencia);
        Task SalvarResultado(string chaveIdempotencia, Guid idMovimento);
    }
}
