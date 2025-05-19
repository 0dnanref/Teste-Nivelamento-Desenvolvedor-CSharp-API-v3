using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Commands.Requests
{
    public class CriarMovimentoCommand : IRequest<Guid>
    {
        public string IdRequisicao { get; set; } = string.Empty;
        public Guid IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public char TipoMovimento { get; set; }
    }
}
