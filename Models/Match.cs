using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;
using static MundialDosmilVeintiSeis.Models.Enums;

namespace MundialDosmilVeintiSeis.Models;

[Table("matches")]
public class Match : BaseModel
{
    [PrimaryKey("id")]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("home_team_id")]
    public int HomeTeamId { get; set; }
    public Team? HomeTeam { get; set; }

    [Column("away_team_id")]
    public int AwayTeamId { get; set; }
    public Team? AwayTeam { get; set; }

    [Required(ErrorMessage ="La fecha y hora del partido es obligatoria.")]
    [Column("match_date")]
    public DateTimeOffset MatchDate { get; set; }

    [Column("stage")]
    public MatchStage Stage { get; set; } = MatchStage.GROUP_STAGE;

    [Column("group_letter")]
    public GroupLetter? GroupLetter { get; set; }

    [Column("status")]
    public MatchStatus Status { get; set; } = MatchStatus.PENDING;

    [Column("home_goals")]
    public int? HomeGoals { get; set; }

    [Column("away_goals")]
    public int? AwayGoals { get; set; }

    [Column("penalties_home")]
    public int? PenaltiesHome { get; set; }

    [Column("penalties_away")]
    public int? PenaltiesAway { get; set; }

    [Column("penalties_winner_id")]
    public int? PenaltiesWinnerId { get; set; }

    [Column("is_knockout")]
    public bool IsKnockout { get; set; } = false;

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}