using OutwardLootManager.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Helpers.Static
{
    public static class OnSceneLoadLootHelpers
    {
        public static void AddLootsToFaction(Character.Factions faction, List<ItemDropChance> itemDropChances, List<string> exceptIds = null)
        {
            List<Character> characters = AiSquadHelpers.GetCharactersFromAiSquads();

            foreach (Character character in characters)
            {
                if (character.Faction == faction)
                {
                    if (CharacterHelpers.IsCharacterIdInList(character, exceptIds))
                        continue;

                    foreach (ItemDropChance itemDropChance in itemDropChances)
                    {
                        AddLootToCharacter(character, itemDropChance);
                    }
                }
            }
        }

        public static void AddLootToFaction(Character.Factions faction, ItemDropChance itemDropChance, List<string> exceptIds = null)
        {
            List<Character> characters = AiSquadHelpers.GetCharactersFromAiSquads();

            foreach (Character character in characters)
            {
                if (character.Faction == faction)
                {
                    if (CharacterHelpers.IsCharacterIdInList(character, exceptIds))
                        continue;

                    AddLootToCharacter(character, itemDropChance);
                }
            }
        }

        public static void AddLootToCharacter(Character character, ItemDropChance itemDropChance)
        {
            LootableOnDeath lootOnDeath = character.GetComponent<LootableOnDeath>();

            if (lootOnDeath == null)
            {
                OutwardLootManager.LogSL("LootManager@AddLootToCharacter received lootOnDeath null! Can't add loot to gameobject without LootableOnDeath component.");
                character.transform.GetOrAddComponent<LootableOnDeath>();
            }

            List<ItemDropChance> itemsDropChances = new List<ItemDropChance>();
            itemsDropChances.Add(itemDropChance);
            LootManager.Instance.AddLootToLootableOnDeath(lootOnDeath, itemsDropChances);
        }
    }
}
