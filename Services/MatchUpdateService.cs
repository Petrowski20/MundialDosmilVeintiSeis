using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MundialDosmilVeintiSeis.Models;
using MundialDosmilVeintiSeis.Services;

namespace MundialDosmilVeintiSeis.Services;

public class MatchUpdateService : BackgroundService
{
    private readonly SupabaseService _supabase;
    private readonly ILogger<MatchUpdateService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public MatchUpdateService(
        SupabaseService supabase,
        ILogger<MatchUpdateService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _supabase = supabase;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("✅ MatchUpdateService iniciado - actualizará resultados cada 24h");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UpdateMatchesAndTeamsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error en actualización diaria");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task UpdateMatchesAndTeamsAsync()
    {
        _logger.LogInformation("🔄 Iniciando actualización diaria de partidos...");

        // Aquí iría la llamada a la API (football-data.org o API-Football)
        // Por ahora solo log para que compiles y pruebes
        _logger.LogInformation("✅ Placeholder: API actualizada (implementaremos la llamada real en el siguiente paso)");

        // TODO: Actualizar matches y teams stats + recalcular puntos de predicciones
    }
}