using OutwardLootManager.Managers;
using OutwardLootManager.Utility.Enums;
using OutwardLootManager.Utility.Helpers.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Drop
{
    public class LootRule
    {
        public string id = null;

        // Id has to be unique so it gets applied once on all criterias
        public string enemyID = null;

        public string enemyName = "";
        public AreaFamily areaFamily = null;

        public Character.Factions? faction;
        public AreaManager.AreaEnum? area;

        public ItemDropRate itemDropRate = null;

        public bool isBoss = false;
        public bool isBossPawn = false;
        public bool isStoryBoss = false;
        public bool isUniqueArenaBoss = false;
        public bool isUniqueEnemy = false;

        public LootRule(string id = null, string enemyId = null, string enemyName = "", AreaFamily areaFamily = null, ItemDropRate itemDropRate = null)
        {
            if(string.IsNullOrEmpty(id))
                this.id = UID.Generate().Value;
            else
                this.id = id;

            this.enemyID = enemyId;
            this.enemyName = enemyName;
            this.areaFamily = areaFamily;
            this.itemDropRate = itemDropRate;
        }

        public bool Matches(Character character)
        {
            if (character == null)
                return false;

            // ID match is strongest — if specified, ignore others
            if (!string.IsNullOrEmpty(enemyID))
                return character.UID.Value == enemyID;

            // Otherwise check other optional filters
            if (!string.IsNullOrEmpty(enemyName) &&
                !character.Name.Equals(enemyName, StringComparison.OrdinalIgnoreCase))
                return false;

            if (faction.HasValue && character.Faction != faction.Value)
                return false;

            if (area.HasValue)
            {
                var currentArea = AreaManager.Instance.CurrentArea;
                var ruleArea = AreaManager.Instance.GetArea(area.Value);

                if (currentArea == null || ruleArea == null || currentArea.ID != ruleArea.ID)
                    return false;
            }

            if (areaFamily != null)
            {
                return AreaFamiliesHelpers.DoesAreaFamilyMatch(areaFamily);
            }

            if(isBoss)
            {
                return BossRegistryManager.Instance.IsBoss(character.Name);
            }

            if(isUniqueArenaBoss)
            {
                return UniqueArenaBossesHelper.TryGetEnum(character.Name, out UniqueArenaBosses boss);
            }

            if(isStoryBoss)
            {
                return StoryBossesHelper.TryGetEnum(character.Name, out StoryBosseses boss);
            }

            if(isBossPawn)
            {
                return BossPawnsHelper.TryGetEnum(character.Name, out BossPawns boss);
            }

            if(isUniqueEnemy)
            {
                return UniqueEnemiesHelper.TryGetEnum(character.Name, out UniqueEnemies enemy);
            }

            return true;
        }
    }
}
