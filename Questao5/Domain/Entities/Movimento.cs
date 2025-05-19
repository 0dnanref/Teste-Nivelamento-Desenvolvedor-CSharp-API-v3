namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public Guid Id { get; set; }
        public Guid IdContaCorrente { get; set; }
        public string DataMovimento { get; set; } = string.Empty; // DD/MM/YYYY
        public char TipoMovimento { get; set; } // 'C' ou 'D'
        public decimal Valor { get; set; }
    }
}
