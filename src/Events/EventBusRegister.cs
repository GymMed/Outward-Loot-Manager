using OutwardLootManager.Utility.Enums;
using OutwardLootManager.Utility.Helpers.Static;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Events
{
    public static class EventBusRegister
    {
        private static readonly (string key, Type type, string description)[] EnvironmentConditionsParams =
        {
            EventRegistryParamsHelper.Get(EventRegistryParams.AreaFamily),
            EventRegistryParamsHelper.Get(EventRegistryParams.Faction),
            EventRegistryParamsHelper.Get(EventRegistryParams.AreaEnum),
        };

        private static readonly (string key, Type type, string description)[] UniqueEnemiesParams =
        {
            EventRegistryParamsHelper.Get(EventRegistryParams.IsForBosses),
            EventRegistryParamsHelper.Get(EventRegistryParams.IsForBossesPawns),
            EventRegistryParamsHelper.Get(EventRegistryParams.IsForStoryBosses),
            EventRegistryParamsHelper.Get(EventRegistryParams.IsForUniqueArenaBosses),
            EventRegistryParamsHelper.Get(EventRegistryParams.IsForUniqueEnemies),
        };

        private static readonly (string key, Type type, string description)[] DropRateParams =
        {
            EventRegistryParamsHelper.Get(EventRegistryParams.EmptyDropChance),
            EventRegistryParamsHelper.Get(EventRegistryParams.MaxDiceValue),
            EventRegistryParamsHelper.Get(EventRegistryParams.MinNumberOfDrops),
            EventRegistryParamsHelper.Get(EventRegistryParams.MaxNumberOfDrops),
        };

        private static readonly (string key, Type type, string description)[] DropChanceProvideWaysParams =
        {
            EventRegistryParamsHelper.Get(EventRegistryParams.ItemDropChance),
            EventRegistryParamsHelper.Get(EventRegistryParams.ListOfItemDropChances),
        };

        private static readonly (string key, Type type, string description)[] DropChanceParams =
        {
            EventRegistryParamsHelper.Get(EventRegistryParams.ItemId),
            EventRegistryParamsHelper.Get(EventRegistryParams.DropChance),
            EventRegistryParamsHelper.Get(EventRegistryParams.MinDropCount),
            EventRegistryParamsHelper.Get(EventRegistryParams.MaxDropCount),
            EventRegistryParamsHelper.Get(EventRegistryParams.MinDiceRollValue),
            EventRegistryParamsHelper.Get(EventRegistryParams.MaxDiceValue),
            // Chance reduction missing and can be provided but not needed?
        };

        private static readonly (string key, Type type, string description)[] ExceptionsParams =
        {
            EventRegistryParamsHelper.Get(EventRegistryParams.ExceptIds),
            EventRegistryParamsHelper.Get(EventRegistryParams.ExceptNames),
        };

        public static void RegisterEvents()
        {
            // Emitter/Publishers has GUID
            EventBus.RegisterEvent(
                OutwardLootManager.GUID,
                EventBusPublisher.Event_AppendLootRule,
                "Publishes event on appending loot rule to loot manager.",
                EventRegistryParamsHelper.Get(EventRegistryParams.LootRuleId)
            );

            EventBus.RegisterEvent(
                OutwardLootManager.GUID,
                EventBusPublisher.Event_RemoveLootRule,
                "Publishes event on removing loot rule from loot manager.",
                EventRegistryParamsHelper.Get(EventRegistryParams.LootRuleId)
            );

            // listeners/subscribers has _*
            EventBus.RegisterEvent(
                OutwardLootManager.EVENT_BUS_ALL_GUID,
                EventBusSubscriber.Event_LoadCustomLoots,
                "Listens for event, when to load custom loot rules from xml.",
                EventRegistryParamsHelper.Get(EventRegistryParams.LoadLootsXmlFilePath)
            );

            EventBus.RegisterEvent(
                OutwardLootManager.EVENT_BUS_ALL_GUID,
                EventBusSubscriber.Event_StoreLootToXml,
                "Listens for event, when to store loot manager loot rules to xml.",
                EventRegistryParamsHelper.Get(EventRegistryParams.StoreLootsXmlFilePath)
            );

            EventBus.RegisterEvent(
                OutwardLootManager.EVENT_BUS_ALL_GUID,
                EventBusSubscriber.Event_AddLoot,
                "Very abstract way to add loot rules. It allows you pass all the possible variables to determine if loot should be applied to a character.",
                EventRegistryParamsHelper.Combine(
                    EventRegistryParamsHelper.Get(EventRegistryParams.LootId),
                    EnvironmentConditionsParams,
                    UniqueEnemiesParams,
                    DropRateParams,
                    DropChanceProvideWaysParams,
                    DropChanceParams,
                    ExceptionsParams
                )
            );

            EventBus.RegisterEvent(
                OutwardLootManager.EVENT_BUS_ALL_GUID,
                EventBusSubscriber.Event_AddLootByEnemyId,
                "Add loot rule for specific enemy id. If enemy id validates it will not check other requirements!",
                EventRegistryParamsHelper.Combine(
                    EventRegistryParamsHelper.Get(EventRegistryParams.LootId),
                    EventRegistryParamsHelper.Get(EventRegistryParams.EnemyId),
                    DropRateParams,
                    DropChanceProvideWaysParams,
                    DropChanceParams
                )
            );

            EventBus.RegisterEvent(
                OutwardLootManager.EVENT_BUS_ALL_GUID,
                EventBusSubscriber.Event_AddLootByEnemyName,
                $"Add loot rule for specific enemy name. Simple way to add loot rule for specific characters. If using for bosses or unique enemies make sure to attach atleast one of the variables: " +
                $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForUniqueEnemies).key}/" +
                $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForBosses).key}/" +
                $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForStoryBosses).key}/" +
                $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForUniqueArenaBosses).key}/" +
                $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForBossesPawns).key} otherwise it will not be applied.",
                EventRegistryParamsHelper.Combine(
                    EventRegistryParamsHelper.Get(EventRegistryParams.LootId),
                    EventRegistryParamsHelper.Get(EventRegistryParams.EnemyName),
                    DropRateParams,
                    DropChanceProvideWaysParams,
                    DropChanceParams,
                    EventRegistryParamsHelper.Get(EventRegistryParams.ExceptIds)
                )
            );

            EventBus.RegisterEvent(
                OutwardLootManager.EVENT_BUS_ALL_GUID,
                EventBusSubscriber.Event_AddLootForUniques,
                "Add loot rule for unique enemy groups." +
                EventRegistryParamsHelper.Combine(
                    EventRegistryParamsHelper.Get(EventRegistryParams.LootId),
                    UniqueEnemiesParams,
                    DropRateParams,
                    DropChanceProvideWaysParams,
                    DropChanceParams,
                    ExceptionsParams
                )
            );
        }
    }
}
