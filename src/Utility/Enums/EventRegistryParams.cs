using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Enums
{
    public enum EventRegistryParams
    {
        LootId,
        ItemId,
        DropChance,
        MinDropCount,
        MaxDropCount,
        MinDiceRollValue,
        MaxDiceRollValue,
        EnemyId,
        EnemyName,
        AreaEnum,
        AreaFamily,
        Faction,
        ExceptIds,
        ExceptNames,
        IsForBosses,
        IsForBossesPawns,
        IsForStoryBosses,
        IsForUniqueArenaBosses,
        IsForUniqueEnemies,
        ListOfItemDropChances,
        ItemDropChance,
        MinNumberOfDrops,
        MaxNumberOfDrops,
        EmptyDropChance,
        MaxDiceValue,
        // separated
        LoadLootsXmlFilePath,
        StoreLootsXmlFilePath,
        LootRuleId,
    }

    public static class EventRegistryParamsHelper
    {
        private static readonly Dictionary<EventRegistryParams, (string key, Type type, string description)> _registry
            = new()
            {
                [EventRegistryParams.LootId] = ("lootId", typeof(int), "Optional. You will need loot id if you planning to remove loot later."),
                [EventRegistryParams.ItemId] = ("itemId", typeof(int), "Required if itemDropChance/listOfItemDropChances is not provided. Loot item ID."),
                [EventRegistryParams.DropChance] = ("dropChance", typeof(int), "Optional. Default is 10. Determines chance of dropping item. You can provide ItemDropChance instead if you like."),
                [EventRegistryParams.MinDropCount] = ("minDropCount", typeof(int), "Optional. Default is 1. Provides minimum amount of items could be dropped. You can provide ItemDropChance instead if you like."),
                [EventRegistryParams.MaxDropCount] = ("dropChance", typeof(int), "Optional. Default is 1. Provides maximum amount of items could be dropped. You can provide ItemDropChance instead if you like."),
                [EventRegistryParams.MinDiceRollValue] = ("minDiceRollValue", typeof(int), "Optional. Default is 0. Sets the lowest dice roll value at which item drop chances begin to count. Use together with 'maxDiceRollValue' and 'maxDiceValue'. You can provide ItemDropChance instead if you like."),
                [EventRegistryParams.MaxDiceRollValue] = ("maxDiceRollValue", typeof(int), "Optional. Default is 0. Sets the highest dice roll value considered when calculating item drop chances. Use together with 'minDiceRollValue' and 'maxDiceValue'. You can provide ItemDropChance instead if you like."),
                [EventRegistryParams.EnemyId] = ("enemyId", typeof(string), "Default null. Determines if drop should be appliead for enemy. You can get this from UnityExplorer mod or logs."),
                [EventRegistryParams.EnemyName] = ("enemyName", typeof(string), "Default null. Determines if drop should be appliead for enemy. You can get this from UnityExplorer mod or logs."),
                [EventRegistryParams.AreaEnum] = ("area", typeof(AreaManager.AreaEnum?), "Optional. Default nullable. Determines if drop should be appliead for specific area. You can get this from AreaManager.AreaEnum enum."),
                [EventRegistryParams.AreaFamily] = ("areaFamily", typeof(AreaFamily), "Optional. Default null. Determines if drop should be appliead for specific area family(region). You can get this from AreaManager.AreaFamilies variable."),
                [EventRegistryParams.Faction] = ("faction", typeof(Character.Factions?), "Optional. Default nullable. Determines if drop should be appliead for specific faction. You can get this from Character.Factions enum."),
                [EventRegistryParams.ExceptIds] = ("listExceptIds", typeof(List<string>), "Optional. Default null. List of enemy ids that will not receive loot. You can get this from Character.UID.Value ."),
                [EventRegistryParams.ExceptNames] = ("listExceptNames", typeof(List<string>), "Optional. Default null. List of enemy names that will not receive loot. You can get this from Character.Name ."),
                [EventRegistryParams.IsForBosses] = ("isForBosses", typeof(bool), "Optional. Default false. Determines if drop should be appliead for all game bosses and pawns."),
                [EventRegistryParams.IsForBossesPawns] = ("isForBossPawns", typeof(bool), "Optional. Default false. Should drop be applied for bosses pawns?"),
                [EventRegistryParams.IsForStoryBosses] = ("isForStoryBosses", typeof(bool), "Optional. Default false. Should drop be applied for story bosses?"),
                [EventRegistryParams.IsForUniqueArenaBosses] = ("isForUniqueArenaBosses", typeof(bool), "Optional. Default false. Should drop be applied for unique arena bosses?"),
                [EventRegistryParams.IsForUniqueEnemies] = ("isForUniqueEnemies", typeof(bool), "Optional. Default false. Should drop be applied for unique enemies?"),
                [EventRegistryParams.ListOfItemDropChances] = ("listOfItemDropChances", typeof(bool), "Optional. Default null. Provide your created list of your ItemDropChance instances to be dropped."),
                [EventRegistryParams.ItemDropChance] = ("itemDropChance", typeof(bool), "Optional. Default null. Provide your created ItemDropChance instance to be dropped."),
                [EventRegistryParams.MinNumberOfDrops] = ("minNumberOfDrops", typeof(int), "Optional. Default is 1. Determines minimum amout of drops for same provided items(ItemDropChance)."),
                [EventRegistryParams.MaxNumberOfDrops] = ("maxNumberOfDrops", typeof(int), "Optional. Default is 1. Determines maximum amout of drops for same provided items(ItemDropChance)."),
                [EventRegistryParams.EmptyDropChance] = ("emptyDropChance", typeof(int), "Optional. Default is 0. Defines the percentage chance for a drop to be empty. Used together with 'maxDiceValue'."),
                [EventRegistryParams.MaxDiceValue] = ("maxDiceValue", typeof(int), "Optional. Default 1. Is the limit of dice rolls on ItemDropChance. It determines which item should be added in drop."),
                // not loot
                [EventRegistryParams.LoadLootsXmlFilePath] = ("filePath", typeof(int), "Required. Used for loading custom loots from xml file."),
                [EventRegistryParams.StoreLootsXmlFilePath] = ("filePath", typeof(int), "Optional. Default \"BepInEx/config/gymmed.Mods_Communicator/Loot_Manager\".Used for storing custom loots from xml file."),
                // separated for publishers
                [EventRegistryParams.LootRuleId] = ("lootRuleId", typeof(int), "Provides loot rule id.")
            };

        public static (string key, Type type, string description) Get(EventRegistryParams param) => _registry[param];

        public static (string key, Type type, string description)[] Combine(
            params object[] items)
        {
            var list = new List<(string key, Type type, string description)>();

            foreach (var item in items)
            {
                if (item is ValueTuple<string, Type, string> single)
                {
                    list.Add(single);
                }
                else if (item is ValueTuple<string, Type, string>[] array)
                {
                    list.AddRange(array);
                }
                else
                {
                    throw new ArgumentException(
                        $"Unsupported item type: {item?.GetType().FullName}");
                }
            }

            return list.ToArray();
        }

    }
}
