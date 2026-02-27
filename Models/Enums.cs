using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MundialDosmilVeintiSeis.Models
{
    public class Enums
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum MatchStatus
        {
            PENDING,
            IN_PLAY,
            FINISHED
        }
        [JsonConverter(typeof(StringEnumConverter))]
        public enum MatchStage
        {
            GROUP_STAGE,
            ROUND_OF_32,
            ROUND_OF_16,
            QUARTER_FINALS,
            SEMI_FINALS,
            FINAL
        }
        [JsonConverter(typeof(StringEnumConverter))]
        public enum GroupLetter
        {
            A, B, C, D, E, F, G, H, I, J, K, L
        }
    }
}
