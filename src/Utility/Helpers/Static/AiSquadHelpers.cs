using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Helpers.Static
{
    public static class AiSquadHelpers
    {
        public static List<Character> GetCharactersFromAiSquads()
        {
            List<Character> characters = new List<Character>();

            foreach (AISquad squad in AISquadManager.Instance.m_allSquads.Values)
            {
                foreach (AISquadMember squadMember in squad.Members)
                {
                    characters.Add(squadMember.Character);
                }
            }

            return characters;
        }
    }
}
