using Newtonsoft.Json;
using Questao2.Classes;
using Questao2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao2.Servico
{
    public class FutebolClienteApi : IFutebolClienteApi
    {

        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://jsonmock.hackerrank.com/api/football_matches";

        public FutebolClienteApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Partida>> ObterPartidasAsync(int ano, string time, string parametroTime)
        {
            var TodasPartidas = new List<Partida>();
            int pagina = 1;
            int totalPages = 1;

            try
            {
                do
                {
                    var url = $"{BaseUrl}?year={ano}&{parametroTime}={Uri.EscapeDataString(time)}&page={pagina}";
                    var response = await _httpClient.GetStringAsync(url);
                    var apiResponse = JsonConvert.DeserializeObject<RetornoApi>(response) ?? new RetornoApi();
                    totalPages = apiResponse.TotalPaginas;
                    TodasPartidas.AddRange(apiResponse.Partidas ?? new List<Partida>());
                    pagina++;
                } while (pagina <= totalPages);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Falha ao obter dados da API: {ex.Message}");
            }

            return TodasPartidas;
        }
    }
}

