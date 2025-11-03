using OutwardLootManager.Drop;
using OutwardLootManager.Drop.Serialization;
using OutwardLootManager.Utility.Helpers.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OutwardLootManager.Managers
{
    public class LootRulesSerializer
    {
        private static LootRulesSerializer _instance;

        private LootRulesSerializer()
        {
            this.configPath = Path.Combine(OutwardModsCommunicator.Managers.PathsManager.ConfigPath, "Loot_Manager");
            this.xmlFilePath = Path.Combine(this.configPath, "PlayerCustomLoots.xml");
        }

        public static LootRulesSerializer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LootRulesSerializer();

                return _instance;
            }
        }

        public string configPath = "";
        public string xmlFilePath = "";

        public LootRulesFile Load(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    OutwardLootManager.LogSL($"LootRules file not found at: {path}");
                    return null;
                }

                XmlSerializer serializer = new(typeof(LootRulesFile));

                using FileStream fs = new(path, FileMode.Open, FileAccess.Read);
                return serializer.Deserialize(fs) as LootRulesFile;
            }
            catch (Exception ex)
            {
                OutwardLootManager.LogSL($"Failed to load LootRules file at '{path}': {ex.Message}");
                return null;
            }
        }

        public void LoadPlayerCustomLoots()
        {
            if (!File.Exists(xmlFilePath))
                return;

            LoadCustomLoots(xmlFilePath);
        }

        public void LoadCustomLoots(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    OutwardLootManager.LogSL($"LootRulesSerializer@LoadCustomLoots file not found at: {path}");
                    return;
                }

                LootRulesFile loot = this.Load(path);

                if (loot == null)
                    return;

                List<LootRule> lootRules = GetLootRules(loot);
                LootRuleRegistryManager.Instance.AppendLootRules(lootRules);
            }
            catch (Exception ex)
            {
                OutwardLootManager.LogSL($"LootRulesSerializer@LoadCustomLoots failed loading '{path}': {ex.Message}");
            }
        }

        public void SaveLootRulesToXml(string filePath, List<LootRule> lootRules)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var file = BuildLootRulesFile(lootRules);

                var serializer = new XmlSerializer(typeof(LootRulesFile));

                var xmlWriterSettings = new XmlWriterSettings
                {
                    Indent = true,
                    NewLineOnAttributes = false
                };

                using (var writer = XmlWriter.Create(filePath, xmlWriterSettings))
                {
                    serializer.Serialize(writer, file);
                }
            }
            catch (Exception ex)
            {
                OutwardLootManager.LogSL($"LootRulesSerializer@SaveLootRulesToXml failed saving '{filePath}': {ex.Message}");
            }
        }

        public List<LootRule> GetLootRules(LootRulesFile file)
        {
            List<LootRule> lootRules = new List<LootRule>();

            foreach(LootRuleSerializable loot in file.Rules)
            {
                List<ItemDropChance> itemsDropChances = new List<ItemDropChance>();

                foreach (ItemDropChanceSerializable itemDropChanceSerializable in loot.ItemDropRate.Drops)
                {
                    ItemDropChance itemDropChance = new ItemDropChance();
                    itemDropChance.DropChance = itemDropChanceSerializable.DropChance;
                    itemDropChance.ChanceReduction = itemDropChanceSerializable.ChanceReduction;
                    itemDropChance.ItemID = itemDropChanceSerializable.ItemID;
                    itemDropChance.MinDropCount = itemDropChanceSerializable.MinDropCount;
                    itemDropChance.MaxDropCount = itemDropChanceSerializable.MaxDropCount;
                    itemDropChance.MinDiceRollValue = itemDropChanceSerializable.MinDiceRollValue;
                    itemDropChance.MaxDiceRollValue = itemDropChanceSerializable.MaxDiceRollValue;

                    itemsDropChances.Add(itemDropChance);
                }

                ItemDropRate itemDropRate = new ItemDropRate(itemsDropChances);
                itemDropRate.EmptyDropChance = loot.ItemDropRate.EmptyDropChance;
                itemDropRate.MaxDiceValue = loot.ItemDropRate.MaxDiceValue;
                itemDropRate.MinNumberOfDrops = loot.ItemDropRate.MinDrops;
                itemDropRate.MaxNumberOfDrops = loot.ItemDropRate.MaxDrops;

                LootRule lootRule = new LootRule(loot.Id, loot.EnemyID, loot.EnemyName, AreaFamiliesHelpers.GetAreaFamilyByName(loot.AreaFamilyName), itemDropRate);

                lootRule.area = AreaHelpers.GetAreaEnumFromAreaName(loot.AreaName);
                lootRule.faction = loot.Faction;

                lootRule.exceptIds = loot.ExceptIds;
                lootRule.exceptNames = loot.ExceptNames;

                lootRule.isBoss = loot.IsBoss;
                lootRule.isBossPawn = loot.IsBossPawn;
                lootRule.isStoryBoss = loot.IsStoryBoss;
                lootRule.isUniqueArenaBoss = loot.IsUniqueArenaBoss;
                lootRule.isUniqueEnemy = loot.IsUniqueEnemy;

                lootRules.Add(lootRule);
            }

            return lootRules;
        }

        public LootRulesFile BuildLootRulesFile(List<LootRule> lootRules)
        {
            var file = new LootRulesFile
            {
                Rules = new List<LootRuleSerializable>()
            };

            foreach (LootRule lootRule in lootRules)
            {
                // Convert ItemDropRate -> ItemDropRateSerializable
                var itemDropRateSerializable = new ItemDropRateSerializable
                {
                    Drops = new List<ItemDropChanceSerializable>(),
                    EmptyDropChance = lootRule.itemDropRate.EmptyDropChance,
                    MaxDiceValue = lootRule.itemDropRate.MaxDiceValue,
                    MinDrops = lootRule.itemDropRate.MinNumberOfDrops,
                    MaxDrops = lootRule.itemDropRate.MaxNumberOfDrops
                };

                foreach (ItemDropChance drop in lootRule.itemDropRate.ListItemDropChance)
                {
                    var dropSerializable = new ItemDropChanceSerializable
                    {
                        ItemID = drop.ItemID,
                        DropChance = drop.DropChance,
                        ChanceReduction = drop.ChanceReduction,
                        MinDropCount = drop.MinDropCount,
                        MaxDropCount = drop.MaxDropCount,
                        MinDiceRollValue = drop.MinDiceRollValue,
                        MaxDiceRollValue = drop.MaxDiceRollValue
                    };

                    itemDropRateSerializable.Drops.Add(dropSerializable);
                }

                // Convert LootRule -> LootRuleSerializable
                var lootSerializable = new LootRuleSerializable
                {
                    Id = lootRule.id ?? null,
                    EnemyID = lootRule.enemyID ?? null,
                    EnemyName = lootRule.enemyName ?? "",
                    AreaFamilyName = lootRule.areaFamily?.FamilyName ?? "",
                    FactionName = lootRule.faction?.ToString() ?? "",
                    AreaName = lootRule.area?.ToString() ?? "",
                    ItemDropRate = itemDropRateSerializable,
                    ExceptIds = lootRule.exceptIds ?? null,
                    ExceptNames = lootRule.exceptNames ?? null,
                    IsBoss = lootRule.isBoss,
                    IsBossPawn = lootRule.isBossPawn,
                    IsStoryBoss = lootRule.isStoryBoss,
                    IsUniqueArenaBoss = lootRule.isUniqueArenaBoss,
                    IsUniqueEnemy = lootRule.isUniqueEnemy
                };

                file.Rules.Add(lootSerializable);
            }

            return file;
        }
    }
}
