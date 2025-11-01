using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Enums
{
    public enum UniqueArenaBosses
    {
        Ash_Giant_Highmonk,
        Brand_Squire,
        Breath_of_Darkness,
        Calixa_Boss,
        Concealed_Knight,
        Elite_Alpha_Tuanosaur,
        // Boss and pawn?
        Elite_Ash_Giant,
        Elite_Beast_Golem,
        Elite_Boozu,
        Elite_Burning_Man,
        Elite_Crescent_Shark,
        Elite_Crimson_Avatar,
        Elite_Gargoyle_Alchemist,
        Elite_Gargoyle_Mage,
        Elite_Gargoyle_Warrior,
        Elite_Mantis_Shrimp,
        Elite_Sublime_Shell,
        Elite_Torcrab,
        Grandmother,
        Immaculate_Dreamer,
        Immaculates_Bird,
        // Story too
        Light_Mender,
        // Story too
        Plague_Doctor,
        Troglodyte_Queen,
    }

    public static class UniqueArenaBossesHelper
    {
        public static readonly Dictionary<UniqueArenaBosses, string> Names = new()
        {
            { UniqueArenaBosses.Ash_Giant_Highmonk, "Ash Giant Highmonk" },
            { UniqueArenaBosses.Brand_Squire, "Elite Brand Squire" },
            { UniqueArenaBosses.Breath_of_Darkness, "Breath of Darkness" },
            { UniqueArenaBosses.Calixa_Boss, "Elite Calixa" },
            { UniqueArenaBosses.Concealed_Knight, "Concealed Knight: ???" },
            { UniqueArenaBosses.Elite_Alpha_Tuanosaur, "Elite Alpha Tuanosaur" },
            { UniqueArenaBosses.Elite_Ash_Giant, "Elite Ash Giant" },
            { UniqueArenaBosses.Elite_Beast_Golem, "Elite Beast Golem" },
            { UniqueArenaBosses.Elite_Boozu, "Elite Boozu" },
            { UniqueArenaBosses.Elite_Burning_Man, "Elite Burning Man" },
            { UniqueArenaBosses.Elite_Crescent_Shark, "Elite Crescent Shark" },
            { UniqueArenaBosses.Elite_Crimson_Avatar, "Elite Crimson Avatar" },
            { UniqueArenaBosses.Elite_Gargoyle_Alchemist, "Elite Gargoyle Alchemist" },
            { UniqueArenaBosses.Elite_Gargoyle_Mage, "Elite Gargoyle Mage" },
            { UniqueArenaBosses.Elite_Gargoyle_Warrior, "Elite Gargoyle Warrior" },
            { UniqueArenaBosses.Elite_Mantis_Shrimp, "Elite Mantis Shrimp" },
            { UniqueArenaBosses.Elite_Sublime_Shell, "Elite Sublime Shell" },
            { UniqueArenaBosses.Elite_Torcrab, "Elite Torcrab" },
            { UniqueArenaBosses.Grandmother, "Grandmother" },
            { UniqueArenaBosses.Immaculate_Dreamer, "Immaculate Dreamer" },
            { UniqueArenaBosses.Immaculates_Bird, "Immaculate's Bird" },
            { UniqueArenaBosses.Light_Mender, "Light Mender" },
            { UniqueArenaBosses.Plague_Doctor, "Plague Doctor" },
            { UniqueArenaBosses.Troglodyte_Queen, "Troglodyte Queen" },
        };

        public static readonly Dictionary<UniqueArenaBosses, string> NamesLoc = new()
        {
            { UniqueArenaBosses.Ash_Giant_Highmonk, "Giant_Priest" },
            { UniqueArenaBosses.Brand_Squire, "Bandit_Desert_Basic" },
            { UniqueArenaBosses.Breath_of_Darkness, "Elite_Dweller" },
            { UniqueArenaBosses.Calixa_Boss, "Cyrene" },
            { UniqueArenaBosses.Concealed_Knight, "name_unpc_unknown_01" },
            { UniqueArenaBosses.Elite_Alpha_Tuanosaur, "Wildlife_TuanosaurAlpha" },
            { UniqueArenaBosses.Elite_Ash_Giant, "Giant_Guard" },
            { UniqueArenaBosses.Elite_Beast_Golem, "Golem_Beast" },
            { UniqueArenaBosses.Elite_Boozu, "Wildlife_BladeDancer" },
            { UniqueArenaBosses.Elite_Burning_Man, "Undead_BurningMan" },
            { UniqueArenaBosses.Elite_Crescent_Shark, "Wildlife_CrescentShark" },
            { UniqueArenaBosses.Elite_Crimson_Avatar, "Undead_BurningMan" },
            { UniqueArenaBosses.Elite_Gargoyle_Alchemist, "Horror_Shell" },
            { UniqueArenaBosses.Elite_Gargoyle_Mage, "Horror_Shell" },
            { UniqueArenaBosses.Elite_Gargoyle_Warrior, "Horror_Shell" },
            { UniqueArenaBosses.Elite_Mantis_Shrimp, "Wildlife_Shrimp" },
            { UniqueArenaBosses.Elite_Sublime_Shell, "Nicolas" },
            { UniqueArenaBosses.Elite_Torcrab, "Wildlife_Torcrab" },
            { UniqueArenaBosses.Grandmother, "Undead_Ghost" },
            { UniqueArenaBosses.Immaculate_Dreamer, "Horror_Immaculate" },//EliteImmaculate
            { UniqueArenaBosses.Immaculates_Bird, "Horror_Immaculate" },
            { UniqueArenaBosses.Light_Mender, "Undead_LichGold" },
            { UniqueArenaBosses.Plague_Doctor, "Undead_LichJade" },
            { UniqueArenaBosses.Troglodyte_Queen, "Troglodyte_Mana" },
            //{ UniqueArenaBosses.Light_Mender, "name_unpc_GoldLich_01" },
        };

        //gotten through Area.GetName()
        public static readonly Dictionary<UniqueArenaBosses, string> WikiLocations = new()
        {
            { UniqueArenaBosses.Ash_Giant_Highmonk, "Giant's Village Arena (south)" },
            { UniqueArenaBosses.Brand_Squire, "Conflux Mountain Arena" },
            { UniqueArenaBosses.Breath_of_Darkness, "The Vault of Stone" },
            { UniqueArenaBosses.Calixa_Boss, "Levant Arena" },
            { UniqueArenaBosses.Concealed_Knight, "Shipwreck (Castaway)" },
            { UniqueArenaBosses.Elite_Alpha_Tuanosaur, "Ziggurat Passage Arena" },
            { UniqueArenaBosses.Elite_Ash_Giant, "Unknown Arena" },
            { UniqueArenaBosses.Elite_Beast_Golem, "Parched Shipwrecks Arena" },
            { UniqueArenaBosses.Elite_Boozu, "Mana Lake Arena" },
            { UniqueArenaBosses.Elite_Burning_Man, "Burning Tree Arena" },
            { UniqueArenaBosses.Elite_Crescent_Shark, "Electric Lab Arena" },
            { UniqueArenaBosses.Elite_Crimson_Avatar, "Vault of Stone Arena" },
            { UniqueArenaBosses.Elite_Gargoyle_Alchemist, "New Sirocco Arena" },
            { UniqueArenaBosses.Elite_Gargoyle_Mage, "New Sirocco Arena" },
            { UniqueArenaBosses.Elite_Gargoyle_Warrior, "New Sirocco Arena" },
            { UniqueArenaBosses.Elite_Mantis_Shrimp, "Voltaic Hatchery Arena" },
            { UniqueArenaBosses.Elite_Sublime_Shell, "Isolated Windmill Arena" },
            { UniqueArenaBosses.Elite_Torcrab, "River of Red Arena" },
            { UniqueArenaBosses.Grandmother, "Tower of Regrets Arena" },
            { UniqueArenaBosses.Immaculate_Dreamer, "Cabal of Wind Temple Arena" },//EliteImmaculate
            { UniqueArenaBosses.Immaculates_Bird, "Immaculate's Camp Arena" },
            { UniqueArenaBosses.Light_Mender, "Spire of Light" },
            { UniqueArenaBosses.Plague_Doctor, "Dark Ziggurat" },
            { UniqueArenaBosses.Troglodyte_Queen, "Blister Burrow Arena" },
        };

        public static readonly Dictionary<UniqueArenaBosses, string> GameLocations = new()
        {
            { UniqueArenaBosses.Ash_Giant_Highmonk, "Unknown Arena" },
            { UniqueArenaBosses.Brand_Squire, "Unknown Arena" },
            { UniqueArenaBosses.Breath_of_Darkness, "The Vault of Stone" },
            { UniqueArenaBosses.Calixa_Boss, "Unknown Arena" },
            { UniqueArenaBosses.Concealed_Knight, "Unknown Arena" },
            { UniqueArenaBosses.Elite_Alpha_Tuanosaur, "Unknown Arena" },
            { UniqueArenaBosses.Elite_Ash_Giant, "Unknown Arena" },
            { UniqueArenaBosses.Elite_Beast_Golem, "Unknown Arena" },
            { UniqueArenaBosses.Elite_Boozu, "Unknown Arena" },
            { UniqueArenaBosses.Elite_Burning_Man, "Unknown Arena" },
            { UniqueArenaBosses.Elite_Crescent_Shark, "Unknown Arena" },
            { UniqueArenaBosses.Elite_Crimson_Avatar, "Unknown Arena" },
            { UniqueArenaBosses.Elite_Gargoyle_Alchemist, "Unknown Arena" },
            { UniqueArenaBosses.Elite_Gargoyle_Mage, "Unknown Arena" },
            { UniqueArenaBosses.Elite_Gargoyle_Warrior, "Unknown Arena" },
            { UniqueArenaBosses.Elite_Mantis_Shrimp, "Unknown Arena" },
            { UniqueArenaBosses.Elite_Sublime_Shell, "Unknown Arena" },
            { UniqueArenaBosses.Elite_Torcrab, "Unknown Arena" },
            { UniqueArenaBosses.Grandmother, "Unknown Arena" },
            { UniqueArenaBosses.Immaculate_Dreamer, "Unknown Arena" },
            { UniqueArenaBosses.Immaculates_Bird, "Unknown Arena" },
            { UniqueArenaBosses.Light_Mender, "Spire of Light" },
            { UniqueArenaBosses.Plague_Doctor, "Dark Ziggurat Interior" },
            { UniqueArenaBosses.Troglodyte_Queen, "Unknown Arena" },
        };

        // Reverse lookup
        public static bool TryGetEnum(string name, out UniqueArenaBosses boss)
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
