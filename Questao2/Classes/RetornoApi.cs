using Newtonsoft.Json;

namespace Questao2.Classes
{
    public class RetornoApi
    {
        [JsonProperty("page")]
        public int Pagina { get; set; }
        [JsonProperty("per_page")]
        public int PorPagina { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("total_pages")]
        public int TotalPaginas { get; set; }
        [JsonProperty("data")]
        public List<Partida> Partidas { get; set; } = new List<Partida>();
    }
}
