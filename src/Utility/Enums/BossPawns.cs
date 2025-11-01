using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Enums
{
    public enum BossPawns
    {
        Elite_Krypteia_Warrior,
        Elite_Krypteia_Witch,
        Elite_Obsidian_Elemental,
    }

    public static class BossPawnsHelper
    {
        public static readonly Dictionary<BossPawns, string> Names = new()
        {
            { BossPawns.Elite_Krypteia_Warrior, "Elite Krypteia Warrior" },
            { BossPawns.Elite_Krypteia_Witch, "Elite Krypteia Witch" },
            { BossPawns.Elite_Obsidian_Elemental, "Elite Obsidian Elemental" },
        };

        public static readonly Dictionary<BossPawns, string> NamesLoc = new()
        {
            { BossPawns.Elite_Krypteia_Warrior, "name_unpc_balira_01" },
            { BossPawns.Elite_Krypteia_Witch, "name_unpc_balira_01" },
            { BossPawns.Elite_Obsidian_Elemental, "Wildlife_ObsidianElemental" },
        };

        public static readonly Dictionary<BossPawns, string> WikiLocations = new()
        {
            { BossPawns.Elite_Krypteia_Warrior, "Tower of Regrets Arena" },
            { BossPawns.Elite_Krypteia_Witch, "Tower of Regrets Arena" },
            { BossPawns.Elite_Obsidian_Elemental, "Burning Tree Arena" },
        };

        public static readonly Dictionary<BossPawns, string> GameLocations = new()
        {
            { BossPawns.Elite_Krypteia_Warrior, "Unknown Arena" },
            { BossPawns.Elite_Krypteia_Witch, "Unknown Arena" },
            { BossPawns.Elite_Obsidian_Elemental, "Unknown Arena" },
        };

        // Reverse lookup
        public static bool TryGetEnum(string name, out BossPawns boss)
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
