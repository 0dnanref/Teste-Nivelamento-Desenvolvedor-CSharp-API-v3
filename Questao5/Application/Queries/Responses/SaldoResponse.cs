﻿namespace Questao5.Application.Queries.Responses
{
    public class SaldoResponse
    {
        public int Numero { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataConsulta { get; set; }
        public decimal Saldo { get; set; }
    }
}
