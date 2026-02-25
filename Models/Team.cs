using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;
using static MundialDosmilVeintiSeis.Models.Enums;

namespace MundialDosmilVeintiSeis.Models;

[Table("teams")]
public class Team : BaseModel
{
    [PrimaryKey("id")]
    [Column("id")]
    public int Id { get; set; }

    [Required(ErrorMessage ="El nombre del equipo es obligatorio.")]
    [StringLength(100)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage ="El ISO es obligatorio.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "ISO debe tener exactamente 3 caracteres")]
    [Column("iso_code")]
    public string IsoCode { get; set; } = string.Empty;

    [Required(ErrorMessage ="La letra del grupo es obligatoria.")]
    [Column("letter")]
    public GroupLetter? GroupLetter { get; set; }

    [Column("flag_emoji")]
    public string? FlagEmoji { get; set; }

    [Column("matches_played")]
    public int MatchesPlayed { get; set; } = 0;

    [Column("wins")]
    public int Wins { get; set; } = 0;

    [Column("draws")]
    public int Draws { get; set; } = 0;

    [Column("losses")]
    public int Losses { get; set; } = 0;

    [Column("goals_for")]
    public int GoalsFor { get; set; } = 0;

    [Column("goals_against")]
    public int GoalsAgainst { get; set; } = 0;

    [Column("goal_difference")]
    public int GoalDifference { get; set; } = 0;

    [Column("points")]
    public int Points { get; set; } = 0;

    [Column("is_eliminated")]
    public bool Eliminated { get; set; } = false;
}