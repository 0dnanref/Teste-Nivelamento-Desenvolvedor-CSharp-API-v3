using Questao2.Servico;

public class Program
{
    public static async Task Main(string[] args)
    {
        using var httpClient = new HttpClient();
        var apiClient = new FutebolClienteApi(httpClient);
        var goalService = new  CalculaGolServico(apiClient);

        await MostrarGolsDoTime(goalService, "Paris Saint-Germain", 2013);
        await MostrarGolsDoTime(goalService, "Chelsea", 2014);
    }

    private static async Task MostrarGolsDoTime(CalculaGolServico GoalServico, string Time, int Ano)
    {
        try
        {
            int gols = await GoalServico.ObterTotalGoalsAsync(Time, Ano);
            Console.WriteLine($"Team {Time} scored {gols} goals in {Ano}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERRO] Falha ao calcular gols do time {Time}: {ex.Message}");
        }
    }
}



