using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;
using static MundialDosmilVeintiSeis.Models.Enums;

namespace MundialDosmilVeintiSeis.Models
{
    [Table("profiles")]
    public class Profile : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Required]
        [Column("nickname")]
        public string Nickname { get; set; } = string.Empty;

        [Required]
        [Column("total_points")]
        public int TotalPoints { get; set; } = 0;

        [Required]
        [Column("role")]
        public UserRole Role { get; set; } = UserRole.USER;

        [Required]
        [Column("birth_date")]
        public DateTime? BirthDate { get; set; }

        [Required]
        [Column("city")]
        public string? City { get; set; }

        [Required]
        [Column("favorite_club_id")]
        public int? FavoriteClubId { get; set; }

        [Reference(typeof(FavoriteClub))]
        public FavoriteClub? FavoriteClub { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
