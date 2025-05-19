using FluentAssertions;
using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Interfaces;
using Xunit;

namespace Questao5.Questao5.Tests
{
    public class ConsultarSaldoQueryHandlerTests
    {
        private readonly IContaCorrenteRepository _contaRepo = Substitute.For<IContaCorrenteRepository>();
        private readonly IMovimentoRepository _movRepo = Substitute.For<IMovimentoRepository>();

        private ConsultarSaldoQueryHandler CreateHandler() =>
            new(_contaRepo, _movRepo);

        [Fact]
        public async Task DeveRetornarSaldoCorretoQuandoContaValida()
        {
           
            var contaId = Guid.Parse("B6BAFC09-6967-ED11-A567-055DFA4A16C9");

            _contaRepo.ObterPorId(contaId).Returns(new ContaCorrente
            {
                Id = contaId,
                Nome = "Katherine Sanchez",
                Numero = 123,
                Ativo = true
            });

            _movRepo.CalcularSaldo(contaId).Returns(150.75m);

            var query = new ConsultarSaldoQuery { IdContaCorrente = contaId };
            var handler = CreateHandler();

            
            var result = await handler.Handle(query, default);

           
            result.Should().NotBeNull();
            result.Saldo.Should().Be(150.75m);
            result.Nome.Should().Be("Katherine Sanchez");
            result.Numero.Should().Be(123);
        }

        [Fact]
        public async Task DeveLancarExcecaoSeContaNaoExiste()
        {
            
            var contaId = Guid.NewGuid();
            _contaRepo.ObterPorId(contaId).Returns((ContaCorrente?)null);

            var query = new ConsultarSaldoQuery { IdContaCorrente = contaId };
            var handler = CreateHandler();

            
            var ex = await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(query, default));

            ex.Tipo.Should().Be(TipoErro.INVALID_ACCOUNT);
            ex.Message.Should().Be("Conta corrente não encontrada.");
        }

        [Fact]
        public async Task DeveLancarExcecaoSeContaEstiverInativa()
        {
            
            var contaId = Guid.NewGuid();

            _contaRepo.ObterPorId(contaId).Returns(new ContaCorrente
            {
                Id = contaId,
                Nome = "João Inativo",
                Numero = 456,
                Ativo = false
            });

            var query = new ConsultarSaldoQuery { IdContaCorrente = contaId };
            var handler = CreateHandler();

            
            var ex = await Assert.ThrowsAsync<BusinessException>(() => handler.Handle(query, default));

            ex.Tipo.Should().Be(TipoErro.INACTIVE_ACCOUNT);
            ex.Message.Should().Be("Conta corrente inativa.");
        }
    }
}

