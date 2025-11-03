using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OutwardLootManager.Drop.Serialization
{
    public class LootRuleSerializable
    {
        [XmlElement("ID")]
        [DefaultValue(null)]
        public string Id { get; set; } = null;

        [XmlElement("EnemyID")]
        [DefaultValue(null)]
        public string EnemyID { get; set; } = null;

        [XmlElement("EnemyName")]
        [DefaultValue("")]
        public string EnemyName { get; set; } = "";

        [XmlElement("AreaFamily")]
        [DefaultValue("")]
        public string AreaFamilyName { get; set; } = "";

        [XmlElement("Faction")]
        [DefaultValue("")]
        public string FactionName { get; set; } = "";

        // Helper property to get enum
        [XmlIgnore]
        public Character.Factions? Faction =>
            string.IsNullOrWhiteSpace(FactionName)
                ? (Character.Factions?)null
                : Enum.TryParse<Character.Factions>(FactionName, out var f) ? f : null;

        [XmlElement("Area")]
        [DefaultValue("")]
        public string AreaName { get; set; } = "";

        [XmlElement("ItemDropRate")]
        public ItemDropRateSerializable ItemDropRate { get; set; }

        [XmlArray("ExceptIds")]
        [XmlArrayItem("ExceptId")]
        [DefaultValue(null)]
        public List<string> ExceptIds { get; set; } = null;

        [XmlArray("ExceptNames")]
        [XmlArrayItem("ExceptName")]
        [DefaultValue(null)]
        public List<string> ExceptNames { get; set; } = null;

        [XmlElement("IsForBosses")]
        [DefaultValue(false)]
        public bool IsBoss { get; set; } = false;

        [XmlElement("IsForBossesPawns")]
        [DefaultValue(false)]
        public bool IsBossPawn { get; set; } = false;

        [XmlElement("IsForStoryBosses")]
        [DefaultValue(false)]
        public bool IsStoryBoss { get; set; } = false;

        [XmlElement("IsForUniqueArenaBosses")]
        [DefaultValue(false)]
        public bool IsUniqueArenaBoss { get; set; } = false;

        [XmlElement("IsForUniqueEnemies")]
        [DefaultValue(false)]
        public bool IsUniqueEnemy { get; set; } = false;
    }
}
