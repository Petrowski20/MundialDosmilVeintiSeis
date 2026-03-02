
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MundialDosmilVeintiSeis.Models
{
    [Table("profile_leagues")]
    public class ProfileLeague : BaseModel
    {
        [PrimaryKey("profile_id", false)]
        [Column("profile_id")]
        public Guid ProfileId { get; set; }

        [PrimaryKey("league_id", false)]
        [Column("league_id")]
        public int LeagueId { get; set; }

        [Reference(typeof(PrivateLeague))]
        public PrivateLeague? League { get; set; }

        [Column("joined_at")]
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
