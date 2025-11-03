using OutwardLootManager.Drop;
using OutwardLootManager.Drop.Serialization;
using OutwardLootManager.Managers;
using OutwardLootManager.Utility.Enums;
using OutwardLootManager.Utility.Helpers.Static;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Events
{
    public static class EventBusSubscriber
    {
        public const string Event_AddLoot = "AddLoot";
        public const string Event_AddLootByEnemyName = "AddLootByEnemyName";
        public const string Event_AddLootByEnemyId = "AddLootByEnemyId";
        public const string Event_AddLootForUniques = "AddLootForUniques";

        public const string Event_StoreLootToXml = "LootRulesSerializer@SaveLootRulesToXml";
        public const string Event_LoadCustomLoots = "LootRulesSerializer@LoadCustomLoots";

        public static string SceneUniquesActionId = UID.Generate();

        public const string Event_FinishedSceneLoopAction = "FinishedSceneLoopAction";

        public static void AddSubscribers()
        {
            EventBus.Subscribe(OutwardLootManager.EVENT_BUS_ALL_GUID, Event_AddLoot, AddLoot);
            EventBus.Subscribe(OutwardLootManager.EVENT_BUS_ALL_GUID, Event_AddLootByEnemyName, AddLootByEnemyName);
            EventBus.Subscribe(OutwardLootManager.EVENT_BUS_ALL_GUID, Event_AddLootByEnemyId, AddLootByEnemyId);
            EventBus.Subscribe(OutwardLootManager.EVENT_BUS_ALL_GUID, Event_AddLootForUniques, AddLootForUniques);
            EventBus.Subscribe(OutwardLootManager.EVENT_BUS_ALL_GUID, Event_StoreLootToXml, StoreLootsToXml);
            EventBus.Subscribe(OutwardLootManager.EVENT_BUS_ALL_GUID, Event_LoadCustomLoots, LoadLootsXml);
        }

        public static void AddSceneTesterSubscribers()
        {
            EventBus.Subscribe("gymmed.scene_tester", Event_FinishedSceneLoopAction, EventBusSubscriber.FinishedSceneLoopAction);
        }

        public static void AddLoot(EventPayload payload)
        {
            if (payload == null) return;

            LootRule lootRule = new LootRule();
            LootRuleHelpers.TryToFillRuleWithId(lootRule, payload);

            bool hasRetrievedChances = LootRuleHelpers.GetItemDropRateAndFillInRule(lootRule, payload);

            if(!hasRetrievedChances)
            {
                OutwardLootManager.LogSL($"EventBusSubscriber@AddLoot didn't receive " +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.ItemId).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.ItemDropChance).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.ListOfItemDropChances).key}! Cannot add loot!");
                return;
            }
            bool hasEnvironmentCondition = LootRuleHelpers.FillRuleWithEnvironmentConditions(lootRule, payload);
            bool isSetForUnique = LootRuleHelpers.FillRuleForStrongEnemyTypes(lootRule, payload);
            bool hasEnemyName = LootRuleHelpers.TryToFillRuleWithEnemyName(lootRule, payload);
            bool hasEnemyId = LootRuleHelpers.TryToFillRuleWithEnemyId(lootRule, payload);

            if(!isSetForUnique && !hasEnemyId && !hasEnemyName && !hasEnvironmentCondition)
            {
                OutwardLootManager.LogSL($"EventBusSubscriber@AddLoot didn't receive " +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.EnemyName).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.EnemyId).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.Faction).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.AreaFamily).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.AreaEnum).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForBosses).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForBossesPawns).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForStoryBosses).key}" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForUniqueArenaBosses).key}" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForUniqueEnemies).key}" +
                    $". Cannot add loot!");
                return;
            }

            bool hasExceptions = LootRuleHelpers.FillRuleWithExceptions(lootRule, payload);

            LootRuleRegistryManager.Instance.AppendLootRule(lootRule);
        }

        public static void AddLootByEnemyName(EventPayload payload)
        {
            if (payload == null) return;

            LootRule lootRule = new LootRule();
            LootRuleHelpers.TryToFillRuleWithId(lootRule, payload);

            if (!LootRuleHelpers.TryToFillRuleWithEnemyName(lootRule, payload, true))
                return;

            bool hasRetrievedChances = LootRuleHelpers.GetItemDropRateAndFillInRule(lootRule, payload);

            if(!hasRetrievedChances)
            {
                OutwardLootManager.LogSL($"EventBusSubscriber@AddLootByEnemyName didn't receive " +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.ItemId).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.ItemDropChance).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.ListOfItemDropChances).key}! Cannot add loot!");
                return;
            }
            LootRuleHelpers.FillRuleWithEnvironmentConditions(lootRule, payload);

            bool hasIdExceptions = LootRuleHelpers.FillRuleWithIdExceptions(lootRule, payload);

            LootRuleRegistryManager.Instance.AppendLootRule(lootRule);
        }

        public static void AddLootByEnemyId(EventPayload payload)
        {
            if (payload == null) return;

            LootRule lootRule = new LootRule();
            LootRuleHelpers.TryToFillRuleWithId(lootRule, payload);

            if (!LootRuleHelpers.TryToFillRuleWithEnemyId(lootRule, payload, true))
                return;

            bool hasRetrievedChances = LootRuleHelpers.GetItemDropRateAndFillInRule(lootRule, payload);

            if(!hasRetrievedChances)
            {
                OutwardLootManager.LogSL($"EventBusSubscriber@AddLootByEnemyId didn't receive " +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.ItemId).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.ItemDropChance).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.ListOfItemDropChances).key}! Cannot add loot!");
                return;
            }
            LootRuleHelpers.FillRuleWithEnvironmentConditions(lootRule, payload);

            LootRuleRegistryManager.Instance.AppendLootRule(lootRule);
        }

        public static void AddLootForUniques(EventPayload payload)
        {
            if (payload == null) return;

            LootRule lootRule = new LootRule();
            LootRuleHelpers.TryToFillRuleWithId(lootRule, payload);

            bool hasRetrievedChances = LootRuleHelpers.GetItemDropRateAndFillInRule(lootRule, payload);

            if(!hasRetrievedChances)
            {
                OutwardLootManager.LogSL($"EventBusSubscriber@AddLootForUniques didn't receive " +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.ItemId).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.ItemDropChance).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.ListOfItemDropChances).key}! Cannot add loot!");
                return;
            }
            LootRuleHelpers.FillRuleWithEnvironmentConditions(lootRule, payload);
            bool isSetForUnique = LootRuleHelpers.FillRuleForStrongEnemyTypes(lootRule, payload);

            if (!isSetForUnique)
            {
                OutwardLootManager.LogSL($"EventBusSubscriber@AddLootForUniques didn't receive " +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForBosses).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForBossesPawns).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForStoryBosses).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForUniqueArenaBosses).key}/" +
                    $"{EventRegistryParamsHelper.Get(EventRegistryParams.IsForUniqueEnemies).key}! Cannot add loot!");
                return;
            }

            bool hasExceptions = LootRuleHelpers.FillRuleWithExceptions(lootRule, payload);

            LootRuleRegistryManager.Instance.AppendLootRule(lootRule);
        }

        public static void LoadLootsXml(EventPayload payload)
        {
            if (payload == null) return;

            (string key, Type type, string description) xmlFilePathParameter = EventRegistryParamsHelper.Get(EventRegistryParams.LoadLootsXmlFilePath);
            string lootsXmlFilePath = payload.Get<string>(xmlFilePathParameter.key, null);

            if(string.IsNullOrEmpty(lootsXmlFilePath))
            {
                OutwardLootManager.LogSL($"EventBusSubscriber@LoadLootsXml didn't receive {xmlFilePathParameter.key} variable! Cannot add custom loots!");
                return;
            }

            LootRulesSerializer.Instance.LoadCustomLoots(lootsXmlFilePath);
        }

        public static void StoreLootsToXml(EventPayload payload)
        {
            if (payload == null) return;

            (string key, Type type, string description) xmlFilePathParameter = EventRegistryParamsHelper.Get(EventRegistryParams.StoreLootsXmlFilePath);
            string lootsXmlFilePath = payload.Get<string>(xmlFilePathParameter.key, null);

            if(string.IsNullOrEmpty(lootsXmlFilePath))
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string xmlFilePath = Path.Combine(LootRulesSerializer.Instance.configPath, "LootRules-" + timestamp + ".xml");
                OutwardLootManager.LogSL($"EventBusSubscriber@StoreLootsToXml didn't receive {xmlFilePathParameter.key} variable! For output path will use: \"{xmlFilePath}\"!");

                LootRulesSerializer.Instance.SaveLootRulesToXml(xmlFilePath, LootRuleRegistryManager.Instance.lootRules);
            }
            else
                LootRulesSerializer.Instance.SaveLootRulesToXml(lootsXmlFilePath, LootRuleRegistryManager.Instance.lootRules);
        }

        public static void FinishedSceneLoopAction(EventPayload payload)
        {
            if (payload == null) return;

            string actionId = payload.Get<string>("actionId", null);

            if(string.IsNullOrEmpty(actionId))
            {
                OutwardLootManager.LogSL($"EventBusSubscriber@FinishedSceneLoopAction didn't receive actionId variable!");
                return;
            }

            //not our action! Stop executing!
            if(SceneUniquesActionId != actionId)
                return;

            SceneLoopActionHelpers.FinishedAction();
        }
    }
}
