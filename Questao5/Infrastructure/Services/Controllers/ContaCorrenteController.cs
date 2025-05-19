using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Exceptions;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Realiza uma movimentação de crédito ou débito em uma conta corrente.
        /// </summary>
        /// <param name="command">Dados da movimentação: id da conta, valor, tipo e chave de idempotência.</param>
        /// <returns>Retorna o ID do movimento criado em caso de sucesso.</returns>
        /// <response code="200">Movimentação realizada com sucesso.</response>
        /// <response code="400">Erro de validação, conta inativa ou inexistente, valor ou tipo inválido.</response>
        [HttpPost("movimentar")]
        [ProducesResponseType(typeof(CustomResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Movimentar([FromBody] CriarMovimentoCommand command)
        {
            var resultado = await _mediator.Send(command);
            return Ok(CustomResult<Guid>.Result(resultado));          
        }

        /// <summary>
        /// Consulta o saldo atual de uma conta corrente ativa.
        /// </summary>
        /// <param name="id">ID da conta corrente (formato UUID v4).</param>
        /// <returns>Retorna os dados da conta e o saldo atual.</returns>
        /// <response code="200">Saldo retornado com sucesso.</response>
        /// <response code="400">Conta inativa ou não encontrada.</response>
        [HttpGet("saldo/{id}")]
        [ProducesResponseType(typeof(CustomResult<SaldoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConsultarSaldo(Guid id)
        {
            var resultado = await _mediator.Send(new ConsultarSaldoQuery { IdContaCorrente = id });
            return Ok(CustomResult<SaldoResponse>.Result(resultado));           
        }
    }
}
