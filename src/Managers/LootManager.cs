using MapMagic;
using OutwardLootManager.Drop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OutwardLootManager.Managers
{
    public class LootManager
    {
        private static LootManager _instance;

        private LootManager()
        {
        }

        public static LootManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LootManager();

                return _instance;
            }
        }

        public void AddLootForIds(LootableOnDeath lootableOnDeath, List<string>ids, List<ItemDropChance>itemsChances)
        {
            Character character = lootableOnDeath.m_character;

            if(character == null)
            {
                OutwardLootManager.LogSL("LootManager@AddLootForFaction could not retrieve character!");
                return;
            }

            bool foundId = false;

            foreach(string id in ids)
            {
                if (id == character.UID.Value)
                    foundId = true;
            }

            if (!foundId)
                return;

            AddLootToLootableOnDeath(lootableOnDeath, itemsChances);
        }

        public void AddLootForFaction(LootableOnDeath lootableOnDeath, Character.Factions faction, List<ItemDropChance>itemsChances, List<string>exceptIds)
        {
            Character character = lootableOnDeath.m_character;

            if(character == null)
            {
                OutwardLootManager.LogSL("LootManager@AddLootForFaction could not retrieve character!");
                return;
            }

            if (character.Faction != faction)
                return;

            foreach(string exceptId in exceptIds)
            {
                if (exceptId == character.UID.Value)
                    return;
            }

            AddLootToLootableOnDeath(lootableOnDeath, itemsChances);
        }

        public void AddLootForArea(LootableOnDeath lootableOnDeath, AreaManager.AreaEnum area, List<ItemDropChance>itemsChances, List<string>exceptIds)
        {
            Character character = lootableOnDeath.m_character;

            if(character == null)
            {
                OutwardLootManager.LogSL("LootManager@AddLootForArea could not retrieve character!");
                return;
            }

            Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            Area currentArea = AreaManager.Instance.GetAreaFromSceneName(currentScene.name);

            if (currentArea.ID != AreaManager.Instance.GetArea(area).ID)
                return;

            foreach(string exceptId in exceptIds)
            {
                if (exceptId == character.UID.Value)
                    return;
            }

            AddLootToLootableOnDeath(lootableOnDeath, itemsChances);
        }

        public void AddLootToLootableOnDeath(LootableOnDeath lootOnDeath, List<ItemDropChance> itemDropChances)
        {
            Transform dropTablesTransform = GetOrCreatDropTableObject(lootOnDeath);

            if(string.IsNullOrEmpty( lootOnDeath.Character?.UID.Value ) )
            {
                OutwardLootManager.LogSL("LootManager@AddLootToLootableOnDeath Missing Item Container UID!");
                return;
            }

            lootOnDeath.m_lootable = true;
            string id = GetLootId(lootOnDeath);

            GameObject dropObj = new GameObject("LootManager_" + id);
            dropObj.transform.SetParent(dropTablesTransform, false);

            Dropable drop = dropObj.AddComponent<Dropable>();
            drop.m_targetContainer = lootOnDeath.Character.Inventory.Pouch;
            drop.m_uid = lootOnDeath.Character.UID + "_" + lootOnDeath.m_lootDroppers.Length.ToString(); 

            List<Dropable> dropList = new List<Dropable>(lootOnDeath.m_lootDroppers ?? new Dropable[0]);
            dropList.Add(drop);
            lootOnDeath.m_lootDroppers = dropList.ToArray();

            DropTable dropTable = dropObj.AddComponent<DropTable>();
            dropTable.UID = id;

            // drops the same ItemDropChance, only confuses, not needed
            //dropTable.MinNumberOfDrops = itemDropChance.MinDropCount;
            //dropTable.MaxNumberOfDrops = itemDropChance.MaxDropCount;
            dropTable.m_itemDrops = itemDropChances;
            // don't allow to not drop anything
            dropTable.m_emptyDropChance = 0;
            dropTable.m_dropAmount = new SimpleRandomChance();
            dropTable.m_calculateChangeRequired = false;
            dropTable.m_maxDiceValue = itemDropChances.First().DropChance;

            drop.Start();
            dropTable.Start();
        }

        public void AddLootToLootableOnDeath(LootableOnDeath lootOnDeath, ItemDropRate itemDropRate)
        {
            try 
            {
                if (lootOnDeath == null)
                {
                    OutwardLootManager.LogMessage("AddLootToLootableOnDeath: lootOnDeath is null!");
                    return;
                }

                if (lootOnDeath.Character == null)
                {
                    OutwardLootManager.LogMessage("AddLootToLootableOnDeath: lootOnDeath.Character is null!");
                    return;
                }

                if (lootOnDeath.Character.UID == null)
                {
                    OutwardLootManager.LogMessage("AddLootToLootableOnDeath: lootOnDeath.Character.UID is null!");
                    return;
                }

                Transform dropTablesTransform = GetOrCreatDropTableObject(lootOnDeath);
                lootOnDeath.m_lootable = true;

                string id = GetLootId(lootOnDeath);
                GameObject dropObj = new GameObject("LootManager_" + id);
                dropObj.transform.SetParent(dropTablesTransform, false);

                Dropable drop = dropObj.AddComponent<Dropable>();
                drop.m_targetContainer = lootOnDeath.Character.Inventory.Pouch;
                drop.m_uid = $"{lootOnDeath.Character.UID}_{(lootOnDeath.m_lootDroppers?.Length ?? 0)}";

                if (string.IsNullOrEmpty(drop.m_uid))
                {
                    OutwardLootManager.LogMessage($"AddLootToLootableOnDeath: Invalid UID for {dropObj.name}");
                    return;
                }

                List<Dropable> dropList = new List<Dropable>(lootOnDeath.m_lootDroppers ?? new Dropable[0]);
                dropList.Add(drop);
                lootOnDeath.m_lootDroppers = dropList.ToArray();

                DropTable dropTable = dropObj.AddComponent<DropTable>();
                dropTable.UID = id;

                dropTable.MinNumberOfDrops = itemDropRate.MinNumberOfDrops;
                dropTable.MaxNumberOfDrops = itemDropRate.MaxNumberOfDrops;
                dropTable.m_itemDrops = itemDropRate.ListItemDropChance;
                dropTable.m_emptyDropChance = itemDropRate.EmptyDropChance;
                dropTable.m_dropAmount = new SimpleRandomChance();
                dropTable.m_calculateChangeRequired = itemDropRate.CalculateChangeRequired;
                dropTable.m_maxDiceValue = itemDropRate.MaxDiceValue;

                //drop.m_mainDropTables.Add(dropTable);

                drop.Start();
                dropTable.Start();

                TryMakeLootable(lootOnDeath.Character);
            }
            catch(Exception ex)
            {
                OutwardLootManager.LogMessage($"LootManager@AddLootToLootableOnDeath We encountered a problem: \"{ex.Message}\"!");
            }
        }

        // only for bosses to add interaction
        public bool TryMakeLootable(Character character, bool _dropWeapons = false, bool _enablePouch = true, bool _forceIteractable = true, bool _loadedDead = false)
        {
            if(character?.Inventory?.m_inventoryPouch == null)
            {
                OutwardLootManager.LogMessage($"LootManager@TryMakeLootable can't make lootable! missing character?.Inventory?.m_invetoryPouch.");
                return false;
            }

            InteractionLoot lootInteraction = character.Inventory.m_inventoryPouch.transform.GetComponent<InteractionLoot>();

            // Only make lootable who isn't already
            if(!lootInteraction)
            {
                character.Inventory.MakeLootable(_dropWeapons, _enablePouch, _forceIteractable, _loadedDead);
            }
            
            return true;
        }

        public string GetLootId(LootableOnDeath lootOnDeath)
        {
            try {
                if (lootOnDeath == null)
                {
                    OutwardLootManager.LogMessage("LootManager@GetLootId: lootOnDeath is null!");
                    return "invalid_lootOnDeath";
                }

                if (lootOnDeath.Character == null)
                {
                    OutwardLootManager.LogMessage("LootManager@GetLootId: lootOnDeath.Character is null!");
                    return "invalid_character";
                }

                if (lootOnDeath.Character.UID == null)
                {
                    OutwardLootManager.LogMessage("LootManager@GetLootId: lootOnDeath.Character.UID is null!");
                    return "invalid_UID";
                }

                int totalDroppers = lootOnDeath.m_lootDroppers?.Length ?? 0;
                int currentLootDropper = totalDroppers < 1 ? 0 : totalDroppers - 1;
                int dropTablesCount = 0;

                if (lootOnDeath.m_lootDroppers != null && totalDroppers > 0)
                {
                    List<DropTable> dropTables = lootOnDeath.m_lootDroppers[currentLootDropper]?.m_mainDropTables;

                    if(dropTables != null )
                        dropTablesCount = dropTables.Count;
                }

                return $"{lootOnDeath.Character.UID}_{currentLootDropper.ToString()}_{dropTablesCount.ToString()}";
            }
            catch(Exception ex)
            {
                OutwardLootManager.LogMessage($"LootManager@GetLootId We encountered a problem: \"{ex.Message}\"!");
                return "Error_" + lootOnDeath?.Character?.UID;
            }
        }

        public bool HasDrops(LootableOnDeath lootOnDeath)
        {
            if (lootOnDeath == null)
                return false;

            Transform dropTablesTransform = lootOnDeath.transform.Find("DropTables");

            if (dropTablesTransform == null)
                return false;

            Dropable drop = dropTablesTransform.GetComponentInChildren<Dropable>();

            if (drop == null)
                return false;

            return true;
        }

        public Transform GetOrCreatDropTableObject(LootableOnDeath lootOnDeath)
        {
            Transform dropTablesTransform = lootOnDeath.transform.Find("DropTables");

            if (dropTablesTransform == null)
            {
                GameObject dropTablesObj = new GameObject("DropTables");
                dropTablesObj.transform.SetParent(lootOnDeath.transform);
                dropTablesTransform = dropTablesObj.transform;
            }

            return dropTablesTransform;
        }
    }
}
