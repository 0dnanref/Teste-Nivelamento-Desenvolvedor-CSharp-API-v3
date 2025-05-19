using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Interfaces;

namespace Questao5.Application.Handlers
{
    public class ConsultarSaldoQueryHandler : IRequestHandler<ConsultarSaldoQuery, SaldoResponse>
    {
        private readonly IContaCorrenteRepository _contaRepo;
        private readonly IMovimentoRepository _movRepo;

        public ConsultarSaldoQueryHandler(IContaCorrenteRepository contaRepo, IMovimentoRepository movRepo)
        {
            _contaRepo = contaRepo;
            _movRepo = movRepo;
        }

        public async Task<SaldoResponse> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepo.ObterPorId(request.IdContaCorrente);
            if (conta == null)
                throw new BusinessException("Conta corrente não encontrada.", TipoErro.INVALID_ACCOUNT);
            if (!conta.Ativo)
                throw new BusinessException("Conta corrente inativa.", TipoErro.INACTIVE_ACCOUNT);

            var saldo = await _movRepo.CalcularSaldo(request.IdContaCorrente);

            return new SaldoResponse
            {
                Numero = conta.Numero,
                Nome = conta.Nome,
                DataConsulta = DateTime.Now,
                Saldo = saldo
            };
        }
    }
}
