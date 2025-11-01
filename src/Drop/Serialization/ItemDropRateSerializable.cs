using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OutwardLootManager.Drop.Serialization
{
    public class ItemDropRateSerializable
    {
        [XmlElement("EmptyDropChance")]
        [DefaultValue(0)]
        public int EmptyDropChance { get; set; } = 0;

        [XmlElement("MaxDiceValue")]
        [DefaultValue(1)]
        public int MaxDiceValue { get; set; } = 1;

        [XmlElement("MinDrops")]
        [DefaultValue(1)]
        public int MinDrops { get; set; } = 1;

        [XmlElement("MaxDrops")]
        [DefaultValue(1)]
        public int MaxDrops { get; set; } = 1;

        [XmlArray("ItemDrops")]
        [XmlArrayItem("ItemDrop")]
        public List<ItemDropChanceSerializable> Drops { get; set; } = new();
    }
}
