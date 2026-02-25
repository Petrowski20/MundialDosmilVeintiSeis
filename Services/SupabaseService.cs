using Supabase;
using Supabase.Postgrest;
using Supabase.Postgrest.Attributes;
using MundialDosmilVeintiSeis.Models;

namespace MundialDosmilVeintiSeis.Services;

public class SupabaseService
{
    private readonly Supabase.Client _client;

    public SupabaseService(IConfiguration configuration)
    {
        var url = configuration["Supabase:Url"] ?? throw new InvalidOperationException("Falta Supabase:Url en appsettings.json");
        var key = configuration["Supabase:Key"] ?? throw new InvalidOperationException("Falta Supabase:Key en appsettings.json");

        var options = new SupabaseOptions { AutoConnectRealtime = true };
        _client = new Supabase.Client(url, key, options);
        try
        {
            _client.InitializeAsync().Wait();
        }
        catch (Exception ex)
        {
            // Log or throw
            throw new InvalidOperationException("Error inicializando Supabase", ex);
        }
    }

    // ==================== PARTIDOS ====================
    public async Task<List<Match>> GetUpcomingMatchesAsync()
    {
        var response = await _client
            .From<Match>()
            .Where(m => m.Status == Models.Enums.MatchStatus.PENDING)
            // CORRECCIÓN: Usar Constants.Ordering.Ascending
            .Order(m => m.MatchDate, Constants.Ordering.Ascending)
            .Get();

        return response.Models;
    }

    public async Task<List<Match>> GetAllMatchesAsync()
    {
        var response = await _client.From<Match>().Get();
        return response.Models;
    }

    // ==================== PREDICCIONES ====================
    public async Task SavePredictionAsync(Prediction prediction)
    {
        await _client
            .From<Prediction>()
            // CORRECCIÓN: Usar QueryOptions en lugar de UpsertOptions
            .Upsert(prediction, new QueryOptions { OnConflict = "profile_id,match_id" });
    }
}