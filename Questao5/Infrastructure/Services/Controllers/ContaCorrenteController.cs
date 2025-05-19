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
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Realiza um movimento (crédito ou débito) na conta corrente.
        /// </summary>
        [HttpPost("Movimentar")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(object), 400)]
        public async Task<IActionResult> Movimentar([FromBody] CriarMovimentoCommand command)
        {
            var resultado = await _mediator.Send(command);
            return Ok(CustomResult<Guid>.Result(resultado));          
        }

        /// <summary>
        /// Retorna o saldo atual da conta corrente.
        /// O id é um Guid version uuid 4
        /// </summary>
        [HttpGet("Saldo/{id}")]
        [ProducesResponseType(typeof(SaldoResponse), 200)]
        [ProducesResponseType(typeof(object), 400)]
        public async Task<IActionResult> ConsultarSaldo(Guid id)
        {
            var resultado = await _mediator.Send(new ConsultarSaldoQuery { IdContaCorrente = id });
            return Ok(CustomResult<SaldoResponse>.Result(resultado));           
        }
    }
}
