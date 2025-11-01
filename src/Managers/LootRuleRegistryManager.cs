using OutwardLootManager.Drop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutwardLootManager.Utility.Extensions;
using OutwardModsCommunicator.EventBus;
using OutwardLootManager.Events;

namespace OutwardLootManager.Managers
{
    public class LootRuleRegistryManager
    {
        private static LootRuleRegistryManager _instance;

        private LootRuleRegistryManager()
        {
        }

        public static LootRuleRegistryManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LootRuleRegistryManager();

                return _instance;
            }
        }

        public List<LootRule> lootRules = new List<LootRule>();

        public void AppendLootRules(List<LootRule> lootRules)
        {
            foreach (LootRule rule in lootRules)
            {
                AppendLootRule(rule);
            }
        }

        public void AppendLootRule(LootRule rule)
        {
            lootRules.Add(rule);
            EventBusPublisher.SendAppendLootRule(rule.id);
        }

        public void RemoveLootRuleById(string id)
        {
            var rulesToRemove = lootRules.Where(rule => rule.id == id).ToList();

            foreach (var rule in rulesToRemove)
            {
                RemoveLootRule(rule);
            }
        }

        public void RemoveLootRule(LootRule rule)
        {
            lootRules.Remove(rule);
            EventBusPublisher.SendRemoveLootRule(rule.id);
        }

        public List<LootRule> GetMatchingRules(Character character)
        {
            List<LootRule> output = new List<LootRule>();

            foreach(LootRule lootRule in lootRules)
            {
                if(lootRule.Matches(character))
                    output.Add(lootRule);
            }

            return output;
        }
    }
}
