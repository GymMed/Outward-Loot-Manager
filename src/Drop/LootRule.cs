using OutwardLootManager.Managers;
using OutwardLootManager.Utility.Enums;
using OutwardLootManager.Utility.Extensions;
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

        public List<string> exceptNames = null;
        public List<string> exceptIds = null;

        public LootRule(string id = null, string enemyId = null, string enemyName = "", 
            AreaFamily areaFamily = null, ItemDropRate itemDropRate = null, 
            List<string> exceptNames = null, List<string> exceptIds = null)
        {
            if (string.IsNullOrEmpty(id))
                this.id = UID.Generate().Value;
            else
                this.id = id;

            this.enemyID = enemyId;
            this.enemyName = enemyName;
            this.areaFamily = areaFamily;
            this.itemDropRate = itemDropRate;
            this.exceptNames = exceptNames;
            this.exceptIds = exceptIds;
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

            if (exceptIds != null && exceptIds.Contains(character.UID.Value))
                return false;

            if (exceptNames != null && exceptNames.Contains(character.Name))
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
                if (!AreaFamiliesHelpers.DoesAreaFamilyMatch(areaFamily))
                    return false;
            }

            if(isBoss)
            {
                if (!BossRegistryManager.Instance.IsBoss(character))
                    return false;
                return true;
            }
            else
            {
                if (BossRegistryManager.Instance.IsBoss(character))
                    return false;
            }

            if(isUniqueArenaBoss)
            {
                if (!UniqueArenaBossesHelper.Enemies.TryGetEnum(character, out UniqueArenaBosses boss))
                    return false;
            }
            else
            {
                if (BossRegistryManager.Instance.IsBossOfCategory(character.UID.Value, BossCategories.Arena))
                    return false;
            }

            if(isStoryBoss)
            {
                if (!StoryBossesHelper.Enemies.TryGetEnum(character, out StoryBosseses boss))
                    return false;
            }
            else
            {
                if (BossRegistryManager.Instance.IsBossOfCategory(character.UID.Value, BossCategories.Story))
                    return false;
            }

            if(isBossPawn)
            {
                if (!BossPawnsHelper.Enemies.TryGetEnum(character, out BossPawns boss))
                    return false;
            }
            else
            {
                if (BossRegistryManager.Instance.IsBossOfCategory(character.UID.Value, BossCategories.Pawn))
                    return false;
            }

            if(isUniqueEnemy)
            {
                if (!UniqueEnemiesHelper.Enemies.TryGetEnum(character, out UniqueEnemies enemy))
                    return false;
            }
            else
            {
                if (UniqueEnemiesHelper.Enemies.TryGetEnum(character, out UniqueEnemies enemy))
                    return false;
            }

            return true;
        }
    }
}
