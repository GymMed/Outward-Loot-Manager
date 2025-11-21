using OutwardLootManager.Drop;
using OutwardLootManager.Managers;
using OutwardLootManager.Utility.Enums;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
            int emptyDropChance = payload.Get<int>("emptyDropChance", -1);

            int maxDiceValue = payload.Get<int>("maxDiceValue", 0);
            itemDropRate.MaxDiceValue = maxDiceValue;

            if(maxDiceValue < 1)
            {
                // this will force to recalculate maxDiceValue and item dice ranges on lootable.OnDeath event
                // DropTable.GenerateDrop is the culprit and it is normal behaviour, good for simplicity
                itemDropRate.CalculateChangeRequired = true;

                // if publisher doesn't provide emptyDrop chance and maxDiceValue we try to get emptyDropChance by ourselves
                // this allows us to only set drop chances for items and everything should be calculated by itself
                if(emptyDropChance < 0)
                {
                    emptyDropChance = GetNormalizedEmptyDropChanceFromItems(payload);
                }
            }

            if(emptyDropChance < 0)
            {
                emptyDropChance = 0;
            }

            itemDropRate.EmptyDropChance = emptyDropChance;

            int minNumberOfDrops = payload.Get<int>("minNumberOfDrops", 1);
            itemDropRate.MinNumberOfDrops = minNumberOfDrops;

            int maxNumberOfDrops = payload.Get<int>("maxNumberOfDrops", 1);
            itemDropRate.MaxNumberOfDrops = maxNumberOfDrops;
        }

        public static int GetNormalizedEmptyDropChanceFromItems(EventPayload payload)
        {
            // prio, publisher needs to provide one or another, not everything at once.
            if (TryGetNormalizedEmptyDropChanceFromItemDropChances(payload, out int dropChanceFromItems))
                return dropChanceFromItems;

            if (TryGetNormalizedEmptyDropChanceFromItemDropChance(payload, out int dropChanceFromItem))
                return dropChanceFromItem;

            if (TryGetNormalizedEmptyDropChanceFromPayload(payload, out int dropChance))
                return dropChance;

            return 0;
        }

        public static bool TryGetNormalizedEmptyDropChanceFromPayload(EventPayload payload, out int value)
        {
            int dropChance = payload.Get<int>("dropChance", -1);

            if(dropChance < 0 || dropChance > 100)
            {
                value = 0;
                return false;
            }

            value = 100 - Mathf.Clamp(dropChance, 0, dropChance);
            return true;
        }

        public static bool TryGetNormalizedEmptyDropChanceFromItemDropChance(EventPayload payload, out int value)
        {
            ItemDropChance itemDropChance = payload.Get<ItemDropChance>("itemDropChance", null);

            if(itemDropChance == null)
            {
                value = 0;
                return false;
            }

            if(itemDropChance.DropChance > 100)
            {
                value = 0;
                return true;
            }

            value = 100 - Mathf.Clamp(itemDropChance.DropChance, 0, itemDropChance.DropChance);
            return true;
        }

        public static bool TryGetNormalizedEmptyDropChanceFromItemDropChances(EventPayload payload, out int value)
        {
            List<ItemDropChance> dropChances = payload.Get<List<ItemDropChance>>("listOfItemDropChances", null);

            if (dropChances == null)
            {
                value = -1;
                return false;
            }

            int totalDropChances = 0;

            foreach(ItemDropChance drop in dropChances)
            {
                totalDropChances += Mathf.Clamp(drop.DropChance, 0, drop.DropChance);
            }

            if (totalDropChances > 100)
                value = 0;
            else
                value = 100 - totalDropChances;

            return true;
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

            // made it 100 instead of 10 because later it will be normalized from weights to percentage
            // with 10 it if emptyDropChance and maxDiceValue is not provided it will make emptyDropChance to 90
            int dropChance = payload.Get<int>("dropChance", 100);
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
