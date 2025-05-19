using Questao2.Classes;

namespace Questao2.Interfaces
{
    public interface IFutebolClienteApi
    {
        Task<List<Partida>> ObterPartidasAsync(int ano, string time, string parametrosTimes);
    }
}
