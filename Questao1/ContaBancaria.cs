using System;
using System.Globalization;

namespace Questao1
{
    public class ContaBancaria
    {
        private readonly int numeroConta;
        private string titular;
        private double saldo;

        public int NumeroConta
        {
            get { return numeroConta; }
        }

        public string Titular
        {
            get { return titular; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    titular = value;
                }
            }
        }
        public double Saldo
        {
            get { return saldo; }
        }
        public ContaBancaria(int numeroConta, string titular, double depositoInicial = 0)
        {
            this.numeroConta = numeroConta;
            this.Titular = titular;
          
            if (depositoInicial > 0)          
                saldo = depositoInicial;
                      
        }
        private double ValidarValor(string mensagem)
        {
            double valor;
            do
            {
                Console.Write("\n" + mensagem);
                bool sucesso = double.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out valor);
                if (!sucesso || valor <= 0)
                {
                    Console.WriteLine("O valor deve ser um número maior que zero.");                    
                }
            } while (valor <= 0);
            return valor;
        }

        public void Deposito()
        {
            double valor = ValidarValor("Entre um valor para depósito: ");
            saldo += valor;
        }

        public void Saque()
        {
            double valor = ValidarValor("Entre um valor para saque: ");
            saldo -= valor + 3.50;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "Conta {0}, Titular: {1}, Saldo: $ {2:F2}", NumeroConta, Titular, Saldo);
        }
    }
}
