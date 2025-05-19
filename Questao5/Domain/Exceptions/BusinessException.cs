using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public TipoErro Tipo { get; }

        public BusinessException(string mensagem, TipoErro tipo) : base(mensagem)
        {
            Tipo = tipo;
        }
    }
}
