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

        public static void RegisterEvents()
        {
            // Emitter/Publishers has GUID
            EventBus.RegisterEvent(
                OutwardLootManager.GUID,
                EventBusPublisher.Event_AppendLootRule,
                EventRegistryParamsHelper.Get(EventRegistryParams.LootRuleId)
            );

            EventBus.RegisterEvent(
                OutwardLootManager.GUID,
                EventBusPublisher.Event_RemoveLootRule,
                EventRegistryParamsHelper.Get(EventRegistryParams.LootRuleId)
            );

            // listeners/subscribers has _*
            EventBus.RegisterEvent(
                OutwardLootManager.EVENT_BUS_ALL_GUID,
                EventBusSubscriber.Event_LoadCustomLoots,
                EventRegistryParamsHelper.Get(EventRegistryParams.LoadLootsXmlFilePath)
            );

            EventBus.RegisterEvent(
                OutwardLootManager.EVENT_BUS_ALL_GUID,
                EventBusSubscriber.Event_StoreLootToXml,
                EventRegistryParamsHelper.Get(EventRegistryParams.StoreLootsXmlFilePath)
            );

            EventBus.RegisterEvent(
                OutwardLootManager.EVENT_BUS_ALL_GUID,
                EventBusSubscriber.Event_AddLoot,
                EventRegistryParamsHelper.Combine(
                    EventRegistryParamsHelper.Get(EventRegistryParams.LootId),
                    EnvironmentConditionsParams,
                    UniqueEnemiesParams,
                    DropRateParams,
                    DropChanceProvideWaysParams,
                    DropChanceParams
                )
            );

            EventBus.RegisterEvent(
                OutwardLootManager.EVENT_BUS_ALL_GUID,
                EventBusSubscriber.Event_AddLootByEnemyId,
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
                EventRegistryParamsHelper.Combine(
                    EventRegistryParamsHelper.Get(EventRegistryParams.LootId),
                    EventRegistryParamsHelper.Get(EventRegistryParams.EnemyName),
                    DropRateParams,
                    DropChanceProvideWaysParams,
                    DropChanceParams
                )
            );

            EventBus.RegisterEvent(
                OutwardLootManager.EVENT_BUS_ALL_GUID,
                EventBusSubscriber.Event_AddLootForUniques,
                EventRegistryParamsHelper.Combine(
                    EventRegistryParamsHelper.Get(EventRegistryParams.LootId),
                    UniqueEnemiesParams,
                    DropRateParams,
                    DropChanceProvideWaysParams,
                    DropChanceParams
                )
            );
        }
    }
}
