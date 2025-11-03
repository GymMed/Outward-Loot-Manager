using OutwardLootManager.Utility.Data;
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
            RegisterEnum(StoryBossesHelper.Enemies, BossCategories.Story);
            RegisterEnum(UniqueArenaBossesHelper.Enemies, BossCategories.Arena);
            RegisterEnum(BossPawnsHelper.Enemies, BossCategories.Pawn);
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
            new(StringComparer.Ordinal); // case-insensitive

        // Generic registration helper
        private void RegisterEnum<T>(
            Dictionary<T, EnemyIdentificationGroupData> mapping,
            BossCategories category)
            where T : Enum
        {
            foreach (var kvp in mapping)
            {
                var enumKey = kvp.Key;
                var groupData = kvp.Value;

                // Create a BossID that represents this whole group
                var bossId = new BossID(category, groupData, enumKey);

                // Register each enemy name from the group as a lookup key
                foreach (var enemy in groupData.Enemies)
                {
                    if (!string.IsNullOrWhiteSpace(enemy.ID))
                        bossLookup[enemy.ID] = bossId;
                    //if (!string.IsNullOrWhiteSpace(enemy.DisplayName))
                    //    bossLookup[enemy.DisplayName] = bossId;

                    //if (!string.IsNullOrWhiteSpace(enemy.InternalName))
                    //    bossLookup[enemy.InternalName] = bossId;

                    //if (!string.IsNullOrWhiteSpace(enemy.LocKey))
                    //    bossLookup[enemy.LocKey] = bossId;
                }
            }
        }

        public bool TryGetBoss(string key, out BossID boss) =>
            bossLookup.TryGetValue(key, out boss);

        public bool IsBoss(Character character) =>
            bossLookup.ContainsKey(character.UID.Value);

        public bool IsBossOfCategory(string key, BossCategories category) =>
            TryGetBoss(key, out var boss) && boss.Category == category;

        public IEnumerable<BossID> GetBossesOfCategory(BossCategories category) =>
            bossLookup.Values.Where(b => b.Category == category);    }
}
