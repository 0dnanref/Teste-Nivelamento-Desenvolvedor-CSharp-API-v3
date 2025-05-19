using Newtonsoft.Json;

namespace Questao2.Classes
{
    public class Partida
    {
        [JsonProperty("team1")]
        public string Time1 { get; set; } = string.Empty;
        [JsonProperty("team2")]
        public string Time2 { get; set; } = string.Empty;
        [JsonProperty("team1goals")]
        public string GolsTime1 { get; set; } = string.Empty;
        [JsonProperty("team2goals")]
        public string GolsTime2 { get; set; } = string.Empty;
    }
}
