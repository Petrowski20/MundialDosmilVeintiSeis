using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MundialDosmilVeintiSeis.Models
{
    [Table("private_leagues")]
    public class PrivateLeague : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;
    }
}
