using FluentAssertions;
using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Interfaces;
using Xunit;

namespace Questao5.Questao5.Tests
{
    public class CriarMovimentoCommandHandlerTests
    {
        private readonly IContaCorrenteRepository _contaRepo = Substitute.For<IContaCorrenteRepository>();
        private readonly IMovimentoRepository _movRepo = Substitute.For<IMovimentoRepository>();
        private readonly IIdempotenciaRepository _idempRepo = Substitute.For<IIdempotenciaRepository>();

        private CriarMovimentoCommandHandler CreateHandler() =>
            new(_contaRepo, _movRepo, _idempRepo);

        [Fact]
        public async Task DeveRetornarIdExistenteSeRequisicaoJaProcessada()
        {
            var idExistenteRequisicao = "11875d2d-452d-4a10-8774-6e5f3245a440";
            var command = new CriarMovimentoCommand
            {
                IdRequisicao = idExistenteRequisicao,
                IdContaCorrente = Guid.Parse("B6BAFC09-6967-ED11-A567-055DFA4A16C9"),
                Valor = 100,
                TipoMovimento = 'C'
            };

            _idempRepo.ObterResultado(command.IdRequisicao).Returns(Guid.Parse(idExistenteRequisicao));

            var handler = CreateHandler();

            var resultado = await handler.Handle(command, default);

            resultado.Should().Be(idExistenteRequisicao);
            await _movRepo.DidNotReceive().Inserir(Arg.Any<Movimento>());
        }

        [Fact]
        public async Task DeveLancarExcecaoSeContaNaoExiste()
        {
            var command = new CriarMovimentoCommand
            {
                IdRequisicao = "616663a4-7641-4b1a-8171-c9d80140c867",
                IdContaCorrente = Guid.NewGuid(),
                Valor = 100,
                TipoMovimento = 'C'
            };

            _idempRepo.ObterResultado(command.IdRequisicao).Returns((Guid?)null);
            _contaRepo.ObterPorId(command.IdContaCorrente).Returns((ContaCorrente?)null);

            var handler = CreateHandler();

            var act = () => handler.Handle(command, default);
            await act.Should().ThrowAsync<BusinessException>()
                .Where(ex => ex.Tipo == TipoErro.INVALID_ACCOUNT);
        }

        [Fact]
        public async Task DeveLancarExcecaoSeContaInativa()
        {
            var command = new CriarMovimentoCommand
            {
                IdRequisicao = "be20ff99-055b-4352-917b-bc9d09c37338",
                IdContaCorrente = Guid.Parse("F475F943-7067-ED11-A06B-7E5DFA4A16C9"),
                Valor = 100,
                TipoMovimento = 'C'
            };

            _idempRepo.ObterResultado(command.IdRequisicao).Returns((Guid?)null);
            _contaRepo.ObterPorId(command.IdContaCorrente).Returns(new ContaCorrente
            {
                Id = command.IdContaCorrente,
                Ativo = false
            });

            var handler = CreateHandler();

            var act = () => handler.Handle(command, default);
            await act.Should().ThrowAsync<BusinessException>()
                .Where(ex => ex.Tipo == TipoErro.INACTIVE_ACCOUNT);
        }

        [Fact]
        public async Task DeveLancarExcecaoSeValorInvalido()
        {
            var command = new CriarMovimentoCommand
            {
                IdRequisicao = "REQ333",
                IdContaCorrente = Guid.NewGuid(),
                Valor = 0,
                TipoMovimento = 'C'
            };

            _idempRepo.ObterResultado(command.IdRequisicao).Returns((Guid?)null);
            _contaRepo.ObterPorId(command.IdContaCorrente).Returns(new ContaCorrente
            {
                Id = command.IdContaCorrente,
                Ativo = true
            });

            var handler = CreateHandler();

            var act = () => handler.Handle(command, default);
            await act.Should().ThrowAsync<BusinessException>()
                .Where(ex => ex.Tipo == TipoErro.INVALID_VALUE);
        }

        [Fact]
        public async Task DeveLancarExcecaoSeTipoMovimentoInvalido()
        {
            var command = new CriarMovimentoCommand
            {
                IdRequisicao = "REQ444",
                IdContaCorrente = Guid.NewGuid(),
                Valor = 100,
                TipoMovimento = 'X'
            };

            _idempRepo.ObterResultado(command.IdRequisicao).Returns((Guid?)null);
            _contaRepo.ObterPorId(command.IdContaCorrente).Returns(new ContaCorrente
            {
                Id = command.IdContaCorrente,
                Ativo = true
            });

            var handler = CreateHandler();

            var act = () => handler.Handle(command, default);
            await act.Should().ThrowAsync<BusinessException>()
                .Where(ex => ex.Tipo == TipoErro.INVALID_TYPE);
        }

        [Fact]
        public async Task DeveCriarMovimentoQuandoDadosValidos()
        {
            var command = new CriarMovimentoCommand
            {
                IdRequisicao = "638e0a13-7070-4be8-b77f-f3aa7d5646b6",
                IdContaCorrente = Guid.Parse("382D323D-7067-ED11-8866-7D5DFA4A16C9"),
                Valor = 200,
                TipoMovimento = 'C'
            };

            _idempRepo.ObterResultado(command.IdRequisicao).Returns((Guid?)null);
            _contaRepo.ObterPorId(command.IdContaCorrente).Returns(new ContaCorrente
            {
                Id = command.IdContaCorrente,
                Ativo = true
            });

            var handler = CreateHandler();

            var result = await handler.Handle(command, default);

            result.Should().NotBeEmpty();
            await _movRepo.Received(1).Inserir(Arg.Is<Movimento>(m =>
                m.IdContaCorrente == command.IdContaCorrente &&
                m.Valor == command.Valor &&
                m.TipoMovimento == command.TipoMovimento
            ));

            await _idempRepo.Received(1).SalvarResultado(command.IdRequisicao, result);
        }
    }
}
