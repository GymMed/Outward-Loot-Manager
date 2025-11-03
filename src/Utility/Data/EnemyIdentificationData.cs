using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Data
{
    public class EnemyIdentificationData
    {
        public string DisplayName;
        public string InternalName;
        public string LocKey;
        public string ID;
        public string WikiLocation;
        public string GameLocation;
        public string SceneName;

        public EnemyIdentificationData(string Name, string m_name, string m_nameLoc, string id, string wikiLocation, string gameLocation, string sceneName = "")
        {
            this.DisplayName = Name;
            this.InternalName = m_name;
            this.LocKey = m_nameLoc;
            this.ID = id;
            this.WikiLocation = wikiLocation;
            this.GameLocation = gameLocation;
            this.SceneName = sceneName;
        }

        public bool Matches(Character character, params Func<EnemyIdentificationData, Character, bool>[] comparers)
        {
            // if no custom comparers provided, fallback to default(only match by id because it is the only one reliable for all)
            if (comparers == null || comparers.Length == 0)
            {
                return
                    string.Equals(ID, character.UID.Value, StringComparison.OrdinalIgnoreCase);// ||
                    //string.Equals(DisplayName, character.Name, StringComparison.OrdinalIgnoreCase) ||
                    //string.Equals(InternalName, character.m_name, StringComparison.OrdinalIgnoreCase) ||
                    //string.Equals(LockKey, character.m_nameLocKey, StringComparison.OrdinalIgnoreCase);
            }

            // use custom comparers if given
            return comparers.Any(c => c(this, character));
        }
    }
}
