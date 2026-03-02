using Supabase;
using Supabase.Postgrest;
using Supabase.Postgrest.Attributes;
using MundialDosmilVeintiSeis.Models;

namespace MundialDosmilVeintiSeis.Services;

public class SupabaseService
{
    private readonly Supabase.Client _client;

    public SupabaseService(Supabase.Client client)
    {
        _client = client;
    }

    public async Task<List<Match>> GetUpcomingMatchesAsync()
    {
        var response = await _client
            .From<Match>()
            // SOLUCIÓN: Usamos Filter() y .ToString() para que envíe "PENDING" en la URL en lugar de un "0"
            .Filter("status", Constants.Operator.Equals, Models.Enums.MatchStatus.PENDING.ToString())
            .Order(m => m.MatchDate, Constants.Ordering.Ascending)
            .Get();

        return response.Models;
    }

    public async Task<List<Match>> GetAllMatchesAsync()
    {
        var response = await _client.From<Match>().Get();
        return response.Models;
    }

    public async Task SavePredictionAsync(Prediction prediction)
    {
        await _client
            .From<Prediction>()
            // CORRECCIÓN: Usar QueryOptions en lugar de UpsertOptions
            .Upsert(prediction, new QueryOptions { OnConflict = "profile_id,match_id" });
    }

    public async Task<List<Team>> GetAllTeamsAsync()
    {
        var response = await _client.From<Team>().Get();
        return response.Models;
    }
}