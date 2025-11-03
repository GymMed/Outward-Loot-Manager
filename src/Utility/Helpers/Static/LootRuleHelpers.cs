using OutwardLootManager.Drop;
using OutwardLootManager.Managers;
using OutwardLootManager.Utility.Enums;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Helpers.Static
{
    public static class LootRuleHelpers
    {
        public static bool TryToFillRuleWithEnemyId(LootRule lootRule, EventPayload payload, bool required = false)
        {
            (string key, Type type, string description) enemyIdParameter = EventRegistryParamsHelper.Get(EventRegistryParams.EnemyId);
            string enemyId = payload.Get<string>(enemyIdParameter.key, null);

            if (!string.IsNullOrEmpty(enemyId))
            {
                lootRule.enemyID = enemyId;
                return true;
            }

            if(required)
                OutwardLootManager.LogSL($"LootRuleHelpers@TryToFillRuleWithEnemyId didn't receive {enemyIdParameter.key} variable! Cannot add loot!");

            return false;
        }

        public static bool TryToFillRuleWithEnemyName(LootRule lootRule, EventPayload payload, bool required = false)
        {
            (string key, Type type, string description) enemyNameParameter = EventRegistryParamsHelper.Get(EventRegistryParams.EnemyName);
            string enemyName = payload.Get<string>(enemyNameParameter.key, null);

            if (!string.IsNullOrEmpty(enemyName))
            {
                lootRule.enemyName = enemyName;
                return true;
            }

            if(required)
                OutwardLootManager.LogSL($"LootRuleHelpers@TryToFillRuleWithEnemyName didn't receive {enemyNameParameter.key} variable! Cannot add loot!");

            return false;
        }

        public static bool TryToFillRuleWithId(LootRule lootRule, EventPayload payload)
        {
            (string key, Type type, string description) lootIdParameter = EventRegistryParamsHelper.Get(EventRegistryParams.LootId);
            string lootId = payload.Get<string>(lootIdParameter.key, null);

            if (!string.IsNullOrEmpty(lootId))
            {
                lootRule.id = lootId;
                return true;
            }

            return false;
        }

        public static bool FillRuleWithEnvironmentConditions(LootRule lootRule, EventPayload payload)
        {
            lootRule.areaFamily = payload.Get("areaFamily", (AreaFamily)null);
            lootRule.area = payload.Get("area", (AreaManager.AreaEnum?)null);
            lootRule.faction = payload.Get("faction", (Character.Factions?)null);

            return lootRule.areaFamily != null
                || lootRule.area.HasValue
                || lootRule.faction.HasValue;
        }

        public static bool FillRuleWithExceptions(LootRule lootRule, EventPayload payload)
        {
            bool filledIdExceptions = FillRuleWithIdExceptions(lootRule, payload);
            bool filledNameExceptions = FillRuleWithNameExceptions(lootRule, payload);

            return filledIdExceptions || filledNameExceptions;
        }

        public static bool FillRuleWithIdExceptions(LootRule lootRule, EventPayload payload)
        {
            (string key, Type type, string description) exceptIdsParameter = EventRegistryParamsHelper.Get(EventRegistryParams.ExceptIds);
            List<string> exceptIds = payload.Get<List<string>>(exceptIdsParameter.key, null);

            if (exceptIds == null)
                return false;

            lootRule.exceptIds = exceptIds;
            return true;
        }

        public static bool FillRuleWithNameExceptions(LootRule lootRule, EventPayload payload)
        {
            (string key, Type type, string description) exceptNamesParameter = EventRegistryParamsHelper.Get(EventRegistryParams.ExceptNames);
            List<string> exceptNames = payload.Get<List<string>>(exceptNamesParameter.key, null);

            if (exceptNames == null)
                return false;

            lootRule.exceptNames = exceptNames;
            return true;
        }

        public static bool FillRuleForStrongEnemyTypes(LootRule lootRule, EventPayload payload)
        {
            lootRule.isBoss = payload.Get<bool>("isForBosses", false);
            lootRule.isBossPawn = payload.Get<bool>("isForBossesPawns", false);
            lootRule.isStoryBoss = payload.Get<bool>("isForStoryBosses", false);
            lootRule.isUniqueArenaBoss = payload.Get<bool>("isForUniqueArenaBosses", false);
            lootRule.isUniqueEnemy = payload.Get<bool>("isForUniqueEnemies", false);

            if (lootRule.isBoss || lootRule.isBossPawn || lootRule.isStoryBoss || lootRule.isUniqueArenaBoss || lootRule.isUniqueEnemy)
                return true;

            return false;
        }

        public static bool GetItemDropRateAndFillInRule(LootRule lootRule, EventPayload payload)
        {
            int itemId = payload.Get<int>("itemId", -1);

            if (itemId > -1)
            {
                FillRuleWithDropRate(lootRule, payload, itemId);
                return true;
            }

            List<ItemDropChance> dropChances = payload.Get<List<ItemDropChance>>("listOfItemDropChances", null);

            if(dropChances != null)
            {
                FillRuleWithDropRate(lootRule, payload, dropChances);
                return true;
            }

            ItemDropChance itemDropChance = payload.Get<ItemDropChance>("itemDropChance", null);

            if(itemDropChance != null)
            {
                FillRuleWithDropRate(lootRule, payload, itemDropChance);
                return true;
            }

            return false;
        }

        public static void FillRuleWithDropRate(LootRule lootRule, EventPayload payload, ItemDropChance itemDropChance)
        {
            List<ItemDropChance> listItemDropChances = new List<ItemDropChance>();
            listItemDropChances.Add(itemDropChance);
            FillRuleWithDropRate(lootRule, payload, listItemDropChances);
        }

        public static void FillRuleWithDropRate(LootRule lootRule, EventPayload payload, int itemId = -1)
        {
            if(itemId < 0)
            {
                OutwardLootManager.LogSL($"LootRuleHelpers@FillRuleWithDropRate didn't receive itemId! Failed to add loot!");
                return;
            }

            ItemDropChance itemDropChance = GetItemDropChanceFromPayLoad(payload, itemId);
            List<ItemDropChance> listItemDropChances = new List<ItemDropChance>();
            listItemDropChances.Add(itemDropChance);
            FillRuleWithDropRate(lootRule, payload, listItemDropChances);
        }

        public static void FillRuleWithDropRate(LootRule lootRule, EventPayload payload, List<ItemDropChance>itemDrops)
        {
            ItemDropRate itemDropRate = new ItemDropRate(itemDrops);
            FillDropRate(itemDropRate, payload);

            lootRule.itemDropRate = itemDropRate;
            itemDropRate.ListItemDropChance =  itemDrops;
        }

        public static void FillDropRate(ItemDropRate itemDropRate, EventPayload payload)
        {

            int emptyDropChance = payload.Get<int>("emptyDropChance", 0);
            itemDropRate.EmptyDropChance = emptyDropChance;

            int maxDiceValue = payload.Get<int>("maxDiceValue", 1);
            itemDropRate.MaxDiceValue = maxDiceValue;

            int minNumberOfDrops = payload.Get<int>("minNumberOfDrops", 1);
            itemDropRate.MinNumberOfDrops = minNumberOfDrops;

            int maxNumberOfDrops = payload.Get<int>("maxNumberOfDrops", 1);
            itemDropRate.MaxNumberOfDrops = maxNumberOfDrops;
        }

        public static ItemDropChance GetItemDropChanceFromPayLoad(EventPayload payload, int itemId = -1)
        {
            if(itemId < 0)
            {
                OutwardLootManager.LogSL($"LootRuleHelpers@GetItemDropChanceFromPayLoad didn't receive itemId! Failed to add loot!");
                return null;
            }

            ItemDropChance itemDropChance = new ItemDropChance();

            itemDropChance.ItemID = itemId;

            int dropChance = payload.Get<int>("dropChance", 10);
            itemDropChance.DropChance = dropChance;

            int minDropCount = payload.Get<int>("minDropCount", 1);
            itemDropChance.MinDropCount = minDropCount;

            int maxDropCount = payload.Get<int>("maxDropCount", 1);
            itemDropChance.MaxDropCount = maxDropCount;

            int minDiceRollValue = payload.Get<int>("minDiceRollValue", 0);
            itemDropChance.MinDiceRollValue = minDiceRollValue;

            int maxDiceRollValue = payload.Get<int>("maxDiceRollValue", 0);
            itemDropChance.MaxDiceRollValue = maxDiceRollValue;

            int chanceReduction = payload.Get<int>("chanceReduction", 0);
            itemDropChance.ChanceReduction = chanceReduction;

            return itemDropChance;
        }
    }
}
