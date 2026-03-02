using static MundialDosmilVeintiSeis.Models.Enums;

namespace MundialDosmilVeintiSeis.Models
{
    public class Dtos
    {
        public class RegisterDto
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Nickname { get; set; } = string.Empty;
            public DateTime? BirthDate { get; set; }
            public string City { get; set; } = string.Empty;
            public int FavoriteClubId { get; set; }
            public Guid InvitationId { get; set; }
            public int PrivateLeagueId { get; set; }
        }
    }
}
