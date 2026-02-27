using Microsoft.AspNetCore.Components;
using MundialDosmilVeintiSeis.Models;
using MundialDosmilVeintiSeis.Services;

namespace MundialDosmilVeintiSeis.Components.Pages;

public partial class Index : ComponentBase
{
    [Inject] protected SupabaseService Supabase { get; set; } = default!;

    protected bool _isLoading = true;
    protected List<Match> allMatches = new();
    protected List<Team> allTeams = new();
    protected List<Match> displayedMatches = new();

    protected List<DateTime> visibleDates = new();
    protected DateTime selectedDate = DateTime.Today;
    private int _selectedTeamId = 0; // 0 = Todos
    protected Dictionary<int, Prediction> predictions = new();
    protected int selectedTeamId
    {
        get => _selectedTeamId;
        set
        {
            if (_selectedTeamId != value)
            {
                _selectedTeamId = value;
                FilterMatches();
            }
        }
    }


    protected override async Task OnInitializedAsync()
    {
        UpdateVisibleDates(selectedDate);

        var matches = await Supabase.GetAllMatchesAsync();
        allTeams = await Supabase.GetAllTeamsAsync();

        if (matches != null)
        {
            allMatches = matches;
            
            FilterMatches();

            foreach (var match in allMatches)
                predictions[match.Id] = new Prediction { MatchId = match.Id };
        }

        _isLoading = false;
    }

    protected void UpdateVisibleDates(DateTime centerDate)
    {
        visibleDates.Clear();
        for (int i = -3; i <= 3; i++)
            visibleDates.Add(centerDate.AddDays(i));
    }

    protected void ChangeDate(DateTime newDate)
    {
        selectedDate = newDate;
        UpdateVisibleDates(selectedDate);
        FilterMatches();
    }

    protected void FilterMatches()
    {
        var query = allMatches.AsQueryable();
        query = query.Where(m => m.MatchDate.LocalDateTime.Date == selectedDate.Date);
        if (selectedTeamId > 0)
            query = query.Where(m => m.HomeTeamId == selectedTeamId || m.AwayTeamId == selectedTeamId);

        displayedMatches = query
            .OrderBy(m => m.MatchDate)
            .ToList();
    }

    protected async Task SavePrediction(int matchId)
    {
        var predictionToSave = predictions[matchId];
        predictionToSave.ProfileId = Guid.NewGuid();
        await Supabase.SavePredictionAsync(predictionToSave);
    }
    // evento desplegable
    protected void OnTeamFilterChanged(int teamId)
    {
        selectedTeamId = teamId;
        FilterMatches();
    }
}