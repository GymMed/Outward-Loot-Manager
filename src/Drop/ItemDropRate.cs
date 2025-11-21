using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Drop
{
    // simple wrapper for List<ItemDropChance> for more DropTable options
    public class ItemDropRate
    {
        List<ItemDropChance> listItemDropChance = new List<ItemDropChance>();
        int emptyDropChance = 0;
        // gets compared with emptyDropChance in DropTable@IsEmpty and DropTable@GenerateDrop
        int maxDiceValue = 1;
        int minNumberOfDrops = 1;
        int maxNumberOfDrops = 1;
        bool calculateChangeRequired = true;

        public ItemDropRate(ItemDropChance itemDropChance)
        {
            this.ListItemDropChance.Add(itemDropChance);
        }

        public ItemDropRate(List<ItemDropChance> itemsDropChances)
        {
            this.ListItemDropChance = itemsDropChances;
        }

        public ItemDropRate(int itemId, int dropChance = 10, int minDropCount = 1, int maxDropCount = 1, int chanceReduction = 0)
        {
            this.AddItemDrop(itemId, dropChance, minDropCount, maxDropCount, chanceReduction);
        }

        public ItemDropRate(int itemId, int dropChance = 10, 
            int minDropCount = 1, int maxDropCount = 1, int minDiceRollValue = 0, int maxDiceRollValue = 0,
            int emptyDropChance = 0, int maxDiceValue = 1, bool calculateChangeRequired = true)
        {
            this.EmptyDropChance = emptyDropChance;
            this.MaxDiceValue = maxDiceValue;
            this.CalculateChangeRequired = calculateChangeRequired;

            this.AddItemDrop(itemId, dropChance, minDropCount, maxDropCount, minDiceRollValue, maxDiceRollValue);
        }

        public List<ItemDropChance> ListItemDropChance { get => listItemDropChance; set => listItemDropChance = value; }
        public int EmptyDropChance { get => emptyDropChance; set => emptyDropChance = value; }
        public int MaxDiceValue { get => maxDiceValue; set => maxDiceValue = value; }
        public int MinNumberOfDrops { get => minNumberOfDrops; set => minNumberOfDrops = value; }
        public int MaxNumberOfDrops { get => maxNumberOfDrops; set => maxNumberOfDrops = value; }
        public bool CalculateChangeRequired { get => calculateChangeRequired; set => calculateChangeRequired = value; }

        public void AddItemDrop(ItemDropChance itemDropChance)
        {
            this.ListItemDropChance.Add(itemDropChance);
        }

        public void AddItemDrop(int itemId, int dropChance = 10, int minDropCount = 1, int maxDropCount = 1, int chanceReduction = 0)
        {
            ItemDropChance itemDropChance = new ItemDropChance();

            itemDropChance.ItemID = itemId;
            itemDropChance.DropChance = dropChance;
            itemDropChance.MinDropCount = minDropCount;
            itemDropChance.MaxDropCount = maxDropCount;
            itemDropChance.ChanceReduction = chanceReduction;

            this.ListItemDropChance.Add(itemDropChance);
        }

        public void AddItemDrop(int itemId, int dropChance = 10, 
            int minDropCount = 1, int maxDropCount = 1, int minDiceRollValue = 0, int maxDiceRollValue = 0)
        {
            ItemDropChance itemDropChance = new ItemDropChance();

            itemDropChance.ItemID = itemId;
            itemDropChance.DropChance = dropChance;
            itemDropChance.MinDropCount = minDropCount;
            itemDropChance.MaxDropCount = maxDropCount;
            itemDropChance.MinDiceRollValue = minDiceRollValue;
            itemDropChance.MaxDiceRollValue = maxDiceRollValue;

            this.ListItemDropChance.Add(itemDropChance);
        }
    }
}
