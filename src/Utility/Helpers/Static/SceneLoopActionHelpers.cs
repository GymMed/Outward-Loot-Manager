using Epic.OnlineServices.Logging;
using NodeCanvas.Tasks.Actions;
using OutwardLootManager.Events;
using OutwardLootManager.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OutwardLootManager.Utility.Helpers.Static
{
    public static class SceneLoopActionHelpers
    {
        public static List<Character> allScenesCharacters = new List<Character>();

        public static void FinishedAction()
        {
            OutwardLootManager.LogMessage($"Total characters found in scene: {allScenesCharacters.Count}");

            TryLoggingCharacterForEnum(UniqueEnemiesHelper.NamesLoc, UniqueEnemiesHelper.WikiLocations, UniqueEnemiesHelper.GameLocations);
            TryLoggingCharacterForEnum(UniqueArenaBossesHelper.NamesLoc, UniqueArenaBossesHelper.WikiLocations, UniqueArenaBossesHelper.GameLocations);
            TryLoggingCharacterForEnum(StoryBossesHelper.NamesLoc, StoryBossesHelper.WikiLocations, StoryBossesHelper.GameLocations);
            TryLoggingCharacterForEnum(BossPawnsHelper.NamesLoc, BossPawnsHelper.WikiLocations, BossPawnsHelper.GameLocations);
        }

        public static void TryLoggingCharacterForEnum<T>(Dictionary<T, string> names, Dictionary<T, string> wikiLocations, Dictionary<T, string> gameLocations)where T : Enum
        {
            string uniqueStudCaseName = "";
            string uniqueName = "";

            foreach(KeyValuePair<T, string> kvp in names)
            {
                foreach(Character character in allScenesCharacters)
                {
                    uniqueStudCaseName = kvp.Key.ToString().Replace("_", "");
                    uniqueName = kvp.Key.ToString().Replace("_", " ");

                    // trying to retrieve only needed character info but they are programmed inconsistent
                    if (character.m_nameLocKey.Equals(kvp.Value) || 
                        character.Name.Equals(uniqueStudCaseName) || 
                        character.m_name.Equals(uniqueStudCaseName) ||
                        character.Name.Equals(uniqueName) ||
                        character.m_name.Equals(uniqueName)) 
                    {
                        string wikiLocation = wikiLocations.TryGetValue(kvp.Key, out var wl) ? wl : "N/A";
                        string gameLocation = gameLocations.TryGetValue(kvp.Key, out var gl) ? gl : "N/A";

                        //OutwardLootManager.LogMessage($"Name: {character.Name} m_name: {character.m_name} m_nameLoc: {character.m_nameLocKey} id: {character.UID.Value} wikiLocation: {wikiLocation} gameLocation: {gameLocation}");
                        OutwardLootManager.LogMessage($"{{ {typeof(T).Name}.{kvp.Key.ToString()}, new(\"{character.Name}\", \"{character.m_name}\", \"{character.m_nameLocKey}\", \"{character.UID.Value}\", \"{wikiLocation}\", \"{gameLocation}\");");
                        break;
                    }
                }
            }
        }

        public static void StartSceneTesting()
        {
            EventBusSubscriber.AddSceneTesterSubscribers();

            // Locations and enums built from wiki.
            HashSet<AreaManager.AreaEnum> uniqueEnemiesAreas = AreaHelpers.GetUniqueAreasFromEnumDictionary(UniqueEnemiesHelper.GameLocations);
            HashSet<AreaManager.AreaEnum> uniqueArenaBossesAreas = AreaHelpers.GetUniqueAreasFromEnumDictionary(UniqueArenaBossesHelper.GameLocations);
            HashSet<AreaManager.AreaEnum> storyBossesAreas = AreaHelpers.GetUniqueAreasFromEnumDictionary(StoryBossesHelper.GameLocations);
            HashSet<AreaManager.AreaEnum> bossPawnsAreas = AreaHelpers.GetUniqueAreasFromEnumDictionary(BossPawnsHelper.GameLocations);

            HashSet<AreaManager.AreaEnum> unknownArenas = UnknownArenasHelper.GetAllAreaEnums().ToHashSet();

            HashSet<AreaManager.AreaEnum> allEnemiesLocations = new HashSet<AreaManager.AreaEnum>();

            allEnemiesLocations.UnionWith(uniqueEnemiesAreas);
            allEnemiesLocations.UnionWith(uniqueArenaBossesAreas);
            allEnemiesLocations.UnionWith(storyBossesAreas);
            allEnemiesLocations.UnionWith(bossPawnsAreas);
            allEnemiesLocations.UnionWith(unknownArenas);

            AddEnemyIdentifierLogger(allEnemiesLocations);
        }

        public static void AddEnemyIdentifierLogger(HashSet<AreaManager.AreaEnum> areas)
        {
            allScenesCharacters = new List<Character>();

            Action function = () =>
            {
                string areaName = AreaManager.Instance.CurrentArea.GetName();
                OutwardLootManager.LogMessage($"Current Area GetName:{areaName}");

                Transform AISquadManagerTransfrom = AISquadManager.Instance.transform;

                if (AISquadManagerTransfrom == null)
                    return;

                Character[] allSceneEnemyCharacters = AISquadManagerTransfrom.GetComponentsInChildren<Character>();

                foreach(Character character in allSceneEnemyCharacters)
                {
                    allScenesCharacters.Add(character); 
                    OutwardLootManager.LogMessage($"Name: {character.Name} m_name: {character.m_name} m_nameLoc: {character.m_nameLocKey} id: {character.UID.Value}");
                }
                OutwardLootManager.LogMessage($"End of Area:{areaName}");
            };

            EventBusPublisher.SendSceneLoopAction(EventBusSubscriber.SceneUniquesActionId, function, areas);
        }
    }
}
