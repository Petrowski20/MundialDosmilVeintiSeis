using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;

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

        [Column("is_used")]
        public bool IsUsed { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
