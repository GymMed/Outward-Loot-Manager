using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OutwardLootManager.Drop.Serialization
{
    public class ItemDropChanceSerializable
    {
        [XmlAttribute("ItemID")]
        public int ItemID { get; set; }

        [XmlAttribute("DropChance")]
        [DefaultValue(10)]
        public int DropChance { get; set; } = 10;

        [XmlAttribute("MinDropCount")]
        [DefaultValue(1)]
        public int MinDropCount { get; set; } = 1;

        [XmlAttribute("MaxDropCount")]
        [DefaultValue(1)]
        public int MaxDropCount { get; set; } = 1;

        [XmlAttribute("MinDiceRollValue")]
        [DefaultValue(0)]
        public int MinDiceRollValue { get; set; } = 0;

        [XmlAttribute("MaxDiceRollValue")]
        [DefaultValue(0)]
        public int MaxDiceRollValue { get; set; } = 0;

        [XmlAttribute("ChanceReduction")]
        [DefaultValue(0)]
        public int ChanceReduction { get; set; } = 0;
    }
}
