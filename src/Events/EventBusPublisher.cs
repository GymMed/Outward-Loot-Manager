using OutwardLootManager.Managers;
using OutwardLootManager.Utility.Enums;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Events
{
    public static class EventBusPublisher
    {
        public const string Event_AppendLootRule = "LootRuleRegistryManager@AppendLootRule";
        public const string Event_RemoveLootRule = "LootRuleRegistryManager@RemoveLootRule";
        public const string Event_AddSceneLoopAction = "AddSceneLoopAction";

        public const string Scene_Tester_Listener = "gymmed.scene_tester_*";

        public static void SendAppendLootRule(string id)
        {
            var payload = new EventPayload
            {
                [EventRegistryParamsHelper.Get(EventRegistryParams.LootRuleId).key] = id
            };
            EventBus.Publish(OutwardLootManager.GUID, Event_AppendLootRule, payload);
        }

        public static void SendRemoveLootRule(string id)
        {
            var payload = new EventPayload
            {
                [EventRegistryParamsHelper.Get(EventRegistryParams.LootRuleId).key] = id
            };
            EventBus.Publish(OutwardLootManager.GUID, Event_RemoveLootRule, payload);
        }

        // Scene Tester
        public static void SendSceneLoopAction(Action function, HashSet<AreaManager.AreaEnum> areas)
        {
            var payload = new EventPayload
            {
                ["action"] = function,
                ["hashSetOfAreas"] = areas,
            };

            EventBus.Publish(Scene_Tester_Listener, Event_AddSceneLoopAction, payload);
        }

        public static void SendSceneLoopAction(string actionId, Action function, HashSet<AreaManager.AreaEnum> areas)
        {
            var payload = new EventPayload
            {
                ["actionId"] = actionId,
                ["action"] = function,
                ["hashSetOfAreas"] = areas,
            };

            EventBus.Publish(Scene_Tester_Listener, Event_AddSceneLoopAction, payload);
        }
    }
}
