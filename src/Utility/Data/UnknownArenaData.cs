using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Data
{
    public class UnknownArenaData
    {
        public string scene;
        public string nameLoc;
        public string defaultName;
        // retrieved through method
        public string getName;

        public UnknownArenaData(string scene, string nameLoc, string defaultName, string getName)
        {
            this.scene = scene;
            this.nameLoc = nameLoc;
            this.defaultName = defaultName;
            this.getName = getName;
        }
    }
}
