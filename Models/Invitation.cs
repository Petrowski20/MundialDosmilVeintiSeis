using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;
using static MundialDosmilVeintiSeis.Models.Enums;

namespace MundialDosmilVeintiSeis.Models
{
    [Table("invitations")]
    public class Invitation : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("suggested_username")]
        public string? SuggestedUsername { get; set; }

        [Column("private_league_id")]
        public int? PrivateLeagueId { get; set; }

        [Reference(typeof(PrivateLeague))]
        public PrivateLeague? PrivateLeague { get; set; }

        [Column("is_used")]
        public bool IsUsed { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
