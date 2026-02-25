using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace MundialDosmilVeintiSeis.Models;

[Table("predictions")]
public class Prediction : BaseModel
{
    [PrimaryKey("id")]
    [Column("id")]
    public int Id { get; set; }

    [Required(ErrorMessage ="El código de perfil es obligatorio.")]
    [Column("profile_id")]
    public Guid ProfileId { get; set; }

    [Required(ErrorMessage ="El código de partido es obligatorio.")]
    [Column("match_id")]
    public Guid MatchId { get; set; }

    [Required(ErrorMessage ="Los goles locales son obligatorios.")]
    [Range(0, 20, ErrorMessage = "Los goles deben estar entre 0 y 20.")]
    [Column("pred_home_goals")]
    public int PredHomeGoals { get; set; }

    [Required(ErrorMessage = "Los goles visitantes son obligatorios.")]
    [Range(0, 20, ErrorMessage = "Los goles deben estar entre 0 y 20.")]
    [Column("pred_away_goals")]
    public int PredAwayGoals { get; set; }

    [Column("pred_winner_id")]
    public int? PredWinnerId { get; set; }

    [Column("points_earned")]
    public int PointsEarned { get; set; } = 0;

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}