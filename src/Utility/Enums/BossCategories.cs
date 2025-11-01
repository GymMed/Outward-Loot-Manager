using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Enums
{
    public enum BossCategories
    {
        Story,
        Arena,
        Pawn
    }

    public struct BossID
    {
        public BossCategories Category;
        public string Name;
        public Enum EnumValue;

        public BossID(BossCategories category, string name, Enum enumValue = null)
        {
            Category = category;
            Name = name;
            EnumValue = enumValue;
        }

        public override string ToString() => $"{Category}:{Name}";
    }
}
