using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Interfaces;

namespace Questao5.Application.Handlers
{
    public class CriarMovimentoCommandHandler : IRequestHandler<CriarMovimentoCommand, Guid>
    {
        private readonly IContaCorrenteRepository _contaRepo;
        private readonly IMovimentoRepository _movRepo;
        private readonly IIdempotenciaRepository _idempRepo;

        public CriarMovimentoCommandHandler(
            IContaCorrenteRepository contaRepository,
            IMovimentoRepository movRepository,
            IIdempotenciaRepository idempRepository)
        {
            _contaRepo = contaRepository;
            _movRepo = movRepository;
            _idempRepo = idempRepository;
        }

        public async Task<Guid> Handle(CriarMovimentoCommand request, CancellationToken cancellationToken)
        {
            var resultadoExistente = await _idempRepo.ObterResultado(request.IdRequisicao);
            if (resultadoExistente != null)         
                return resultadoExistente.Value;
            

            var conta = await _contaRepo.ObterPorId(request.IdContaCorrente);
            if (conta == null)
                throw new BusinessException("Conta corrente não encontrada.", TipoErro.INVALID_ACCOUNT);
            if (!conta.Ativo)
                throw new BusinessException("Conta corrente inativa.", TipoErro.INACTIVE_ACCOUNT);
            if (request.Valor <= 0)
                throw new BusinessException("O valor deve ser maior que zero.", TipoErro.INVALID_VALUE);
            if (!Enum.IsDefined(typeof(TipoMovimento), (int)request.TipoMovimento))
                throw new BusinessException("Tipo de movimento inválido.", TipoErro.INVALID_TYPE);

            var movimento = new Movimento
            {
                Id = Guid.NewGuid(),
                IdContaCorrente = request.IdContaCorrente,
                Valor = request.Valor,
                TipoMovimento = request.TipoMovimento,
                DataMovimento = DateTime.Now.ToString("dd/MM/yyyy")
            };

            await _movRepo.Inserir(movimento);
            await _idempRepo.SalvarResultado(request.IdRequisicao, movimento.Id);

            return movimento.Id;
        }
    }
}
