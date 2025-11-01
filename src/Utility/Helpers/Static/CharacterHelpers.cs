using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Helpers.Static
{
    public static class CharacterHelpers
    {
        public static bool IsCharacterIdInList(Character character, List<string> ids = null)
        {
            if (ids == null)
                return false;

            foreach (string id in ids)
            {
                if (character.UID.Value == id)
                    return true;
            }

            return false;
        }
    }
}
