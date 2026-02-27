using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace MundialDosmilVeintiSeis.Models
{
    [Table("favorite_clubs")]
    public class FavoriteClub : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("league")]
        public string? League { get; set; }

        [Column("logo_url")]
        public string? LogoUrl { get; set; }
    }
}
