using Questao1;
using System;
using System.Globalization;

namespace Questao1
{
    class Program
    {
        static void Main(string[] args)
        {
            ContaBancaria conta;

            Console.Write("Entre o número da conta: ");
            int numero = int.Parse(Console.ReadLine());

            Console.Write("Entre o titular da conta: ");
            string titular = Console.ReadLine();

            Console.Write("Haverá depósito inicial (s/n)? ");
            string resposta = Console.ReadLine().Trim().ToLower();

            double depositoInicial = 0;
            if (resposta == "s")
            {
                Console.Write("Entre o valor de depósito inicial: ");
                depositoInicial = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                conta = new ContaBancaria(numero, titular, depositoInicial);
            }
            else
            {
                 conta = new ContaBancaria(numero, titular);
            }

            Console.WriteLine("\nDados da conta:");
            Console.WriteLine(conta);

            conta.Deposito();

            Console.WriteLine("Dados da conta atualizados:");
            Console.WriteLine(conta);

            conta.Saque();

            Console.WriteLine("Dados da conta atualizados:");
            Console.WriteLine(conta);
        }
    }
}
