using OutwardLootManager.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Managers
{
    public class BossRegistryManager
    {
        private static BossRegistryManager _instance;

        private BossRegistryManager()
        {
            RegisterEnum(StoryBossesHelper.Names, BossCategories.Story);
            RegisterEnum(UniqueArenaBossesHelper.Names, BossCategories.Arena);
            RegisterEnum(BossPawnsHelper.Names, BossCategories.Pawn);
        }

        public static BossRegistryManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BossRegistryManager();

                return _instance;
            }
        }

        private readonly Dictionary<string, BossID> bossLookup =
            new(StringComparer.OrdinalIgnoreCase); // case-insensitive

        // Generic registration helper
        private void RegisterEnum<T>(Dictionary<T, string> mapping, BossCategories category) where T : Enum
        {
            foreach (var kvp in mapping)
            {
                bossLookup[kvp.Value] = new BossID(category, kvp.Value, kvp.Key);
            }
        }

        // Try get a boss by name
        public bool TryGetBoss(string name, out BossID boss)
            => bossLookup.TryGetValue(name, out boss);

        // Check if name is any boss
        public bool IsBoss(string name) => bossLookup.ContainsKey(name);

        // Check if name is a boss of a specific category
        public bool IsBossOfCategory(string name, BossCategories category)
            => TryGetBoss(name, out var boss) && boss.Category == category;

        // Get all bosses of a specific category
        public IEnumerable<BossID> GetBossesOfCategory(BossCategories category)
            => bossLookup.Values.Where(b => b.Category == category);
    }
}
