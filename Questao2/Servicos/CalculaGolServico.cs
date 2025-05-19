using Questao2.Classes;
using Questao2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Questao2.Servico
{
    public class CalculaGolServico
    {
        private readonly IFutebolClienteApi _apiClient;

        public CalculaGolServico(IFutebolClienteApi apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<int> ObterTotalGoalsAsync(string team, int year)
        {
            int totalGoals = 0;

            var partidasTime1 = await _apiClient.ObterPartidasAsync(year, team, "team1");
            totalGoals += SomaGoals(partidasTime1, "team1");

            var partidasTime2 = await _apiClient.ObterPartidasAsync(year, team, "team2");
            totalGoals += SomaGoals(partidasTime2, "team2");

            return totalGoals;
        }

        private int SomaGoals(List<Partida> Partidas, string role)
        {
            int sum = 0;

            foreach (var partida in Partidas)
            {
                string goalsStr = role == "team1" ? partida.GolsTime1 : partida.GolsTime2;
                if (int.TryParse(goalsStr, out int goals))
                {
                    sum += goals;
                }
            }

            return sum;
        }
    }
}
