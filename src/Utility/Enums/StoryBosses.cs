using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Enums
{
    public enum StoryBosseses
    {
        Crimson_Avatar,
        Djinn,
        Forge_Master,
        Light_Mender,
        Plague_Doctor,
    }

    public static class StoryBossesHelper
    {
        public static readonly Dictionary<StoryBosseses, string> Names = new()
        {
            { StoryBosseses.Crimson_Avatar, "Crimson Avatar" },
            { StoryBosseses.Djinn, "Djinn" },
            { StoryBosseses.Forge_Master, "Forge Master" },
            { StoryBosseses.Light_Mender, "Light Mender" },
            { StoryBosseses.Plague_Doctor, "Plague Doctor" },
        };

        public static readonly Dictionary<StoryBosseses, string> NamesLoc = new()
        {
            { StoryBosseses.Crimson_Avatar, "name_unpc_Avatar" },
            { StoryBosseses.Djinn, "name_unpc_Caldera_Djinn" },
            { StoryBosseses.Forge_Master, "name_unpc_forgemaster_01" },
            //{ StoryBosseses.Forge_Master, "Boss_RustLich" },
            { StoryBosseses.Light_Mender, "name_unpc_GoldLich_01" },
            { StoryBosseses.Plague_Doctor, "name_unpc_JadeLich_01" },
        };

        public static readonly Dictionary<StoryBosseses, string> WikiLocations = new()
        {
            { StoryBosseses.Crimson_Avatar, "Scarlet Sanctuary" },
            { StoryBosseses.Djinn, "Oil Refinery" },
            { StoryBosseses.Forge_Master, "Forgotten Research Laboratory" },
            { StoryBosseses.Light_Mender, "Spire of Light" },
            { StoryBosseses.Plague_Doctor, "Dark Ziggurat" },
        };

        public static readonly Dictionary<StoryBosseses, string> GameLocations = new()
        {
            { StoryBosseses.Crimson_Avatar, "Scarlet Sanctuary" },
            { StoryBosseses.Djinn, "Oil Refinery" },
            { StoryBosseses.Forge_Master, "Forgotten Research Laboratory" },
            { StoryBosseses.Light_Mender, "Spire of Light" },
            { StoryBosseses.Plague_Doctor, "Dark Ziggurat Interior" },// have to use Interior because Dark Ziggurat is unfinished and can't load? is not provided in cheat menu too.
        };

        // Reverse lookup
        public static bool TryGetEnum(string name, out StoryBosseses boss)
        {
            foreach (var kvp in Names)
            {
                if (kvp.Value.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    boss = kvp.Key;
                    return true;
                }
            }
            boss = default;
            return false;
        }
    }
}
