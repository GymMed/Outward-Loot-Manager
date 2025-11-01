using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OutwardLootManager.Drop.Serialization
{
    [XmlRoot("Loots")]
    public class LootRulesFile
    {
        [XmlElement("LootRule")]
        public List<LootRuleSerializable> Rules { get; set; } = new();
    }
}
