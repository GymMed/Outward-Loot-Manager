using OutwardLootManager.Drop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Managers
{
    public class LootStoreManager
    {
        private static LootStoreManager _instance;

        private LootStoreManager()
        {
        }

        public static LootStoreManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LootStoreManager();

                return _instance;
            }
        }

        Dictionary<string, List<ItemDropRate>> idsLoots = new Dictionary<string, List<ItemDropRate>>();

        Dictionary<string, List<ItemDropRate>> namesLoots = new Dictionary<string, List<ItemDropRate>>();

        Dictionary<AreaFamily, Dictionary<string, List<ItemDropRate>>> areaFamiliesLoots = new Dictionary<AreaFamily, Dictionary<string, List<ItemDropRate>>>(); 

        Dictionary<AreaManager.AreaEnum, Dictionary<string, List<ItemDropRate>>> areasLoots = new Dictionary<AreaManager.AreaEnum, Dictionary<string, List<ItemDropRate>>>();

        Dictionary<Character.Factions, Dictionary<string, List<ItemDropRate>>> factionsLoots = new Dictionary<Character.Factions, Dictionary<string, List<ItemDropRate>>>();

        public void StoreLootsById(string id, List<ItemDropRate> itemDropRates)
        {
            foreach(ItemDropRate itemDropRate in itemDropRates)
            {
                StoreLootById(id, itemDropRate);
            }
        }

        public void StoreLootById(string id, ItemDropRate itemDropRate)
        {
            if (!idsLoots.ContainsKey(id))
            {
                List<ItemDropRate> itemDropRates = new List<ItemDropRate>();
                itemDropRates.Add(itemDropRate);
                idsLoots.Add(id, itemDropRates);
                return;
            }

            idsLoots.TryGetValue(id, out List<ItemDropRate> itemDrops);
            itemDrops.Add(itemDropRate);
        }

        public void StoreLootsByName(string name, List<ItemDropRate> itemDropRates)
        {
            foreach(ItemDropRate itemDropRate in itemDropRates)
            {
                StoreLootByName(name, itemDropRate);
            }
        }

        public void StoreLootByName(string name, ItemDropRate itemDropRate)
        {
            if (!namesLoots.ContainsKey(name))
            {
                List<ItemDropRate> itemDropRates = new List<ItemDropRate>();
                itemDropRates.Add(itemDropRate);
                namesLoots.Add(name, itemDropRates);
                return;
            }

            namesLoots.TryGetValue(name, out List<ItemDropRate> itemDrops);
            itemDrops.Add(itemDropRate);
        }

        public void StoreLootsByAreaAndId(AreaManager.AreaEnum area, string id, List<ItemDropRate> itemDropRates)
        {
            foreach(ItemDropRate itemDropRate in itemDropRates)
            {
                StoreLootByAreaAndId(area, id, itemDropRate);
            }
        }

        public void StoreLootByAreaAndId(AreaManager.AreaEnum area, string id, ItemDropRate itemDropRates)
        {
            if (!areasLoots.TryGetValue(area, out var idLoots))
            {
                idLoots = new Dictionary<string, List<ItemDropRate>>();
                areasLoots[area] = idLoots;
            }

            if (!idLoots.TryGetValue(id, out var itemDrops))
            {
                itemDrops = new List<ItemDropRate>();
                idLoots[id] = itemDrops;
            }

            itemDrops.Add(itemDropRates);
        }

        public void StoreLootsByFaction(Character.Factions faction, string id, List<ItemDropRate> itemDropRates)
        {
            foreach(ItemDropRate itemDropRate in itemDropRates)
            {
                StoreLootByFaction(faction, id, itemDropRate);
            }
        }

        public void StoreLootByFaction(Character.Factions faction, string id, ItemDropRate itemDropRate)
        {
            if (!factionsLoots.TryGetValue(faction, out var idLoots))
            {
                idLoots = new Dictionary<string, List<ItemDropRate>>();
                factionsLoots[faction] = idLoots;
            }

            if (!idLoots.TryGetValue(id, out var itemDrops))
            {
                itemDrops = new List<ItemDropRate>();
                idLoots[id] = itemDrops;
            }

            itemDrops.Add(itemDropRate);
        }
    }
}
