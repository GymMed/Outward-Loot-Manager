using OutwardLootManager.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Analytics;

namespace OutwardLootManager.Utility.Helpers.Static
{
    // Made just for testing
    // Tries to transform wiki locations of enum enemies to game locations
    public static class LocalizationHelpers
    {
        public static void LogConstructedAreasForEnum<T>(Dictionary<T, string> locations) where T : Enum
        {
            string areaName = "";
            bool found = false;

            foreach(KeyValuePair<T, string> kvp in locations)
            {
                foreach(Area area in AreaManager.Instance.Areas)
                {
                    areaName = area.GetName();

                    if(kvp.Value.Equals(areaName, StringComparison.OrdinalIgnoreCase))
                    {
                        OutwardLootManager.LogMessage($"{{ {typeof(T).Name}.{kvp.Key.ToString()}, \"{areaName}\" }},");
                        found = true;
                        break;
                    }
                }

                if(found)
                    found = false;
                else
                    OutwardLootManager.LogMessage($"{{ {typeof(T).Name}.{kvp.Key.ToString()}, \"---{locations[kvp.Key]}---\" }},");
            }

        }

        public static string GetAllLocalizationsForEnum<T>(Dictionary<T, string> names) where T : Enum
        {
            string output = "";

            foreach (KeyValuePair<string, string> kvp in LocalizationManager.Instance.m_generalLocalization)
            {
                string enumName = GetUniqueEnumNameInLocalization<T>(kvp.Value, names);

                if (string.IsNullOrEmpty(enumName))
                    continue;

                // Use typeof(T).Name to insert enum type name dynamically
                output += $"{{ {typeof(T).Name}.{enumName}, \"{kvp.Key}\" }},\n";
            }

            return output;
        }

        public static string GetAllLocalizationsForUniques()
        {
            string output = "";

            output += GetSeparationForTypeAndLocalization<UniqueEnemies>(UniqueEnemiesHelper.Names);
            output += GetSeparationForTypeAndLocalization<UniqueArenaBosses>(UniqueArenaBossesHelper.Names);
            output += GetSeparationForTypeAndLocalization<BossPawns>(BossPawnsHelper.Names);
            output += GetSeparationForTypeAndLocalization<StoryBosseses>(StoryBossesHelper.Names);

            return output;
        }

        public static string GetSeparationForTypeAndLocalization<T>(Dictionary<T, string> names) where T : Enum
        {
            string output = "";

            output += $"{typeof(T).Name} Start \n";
            output += GetAllLocalizationsForEnum<T>(names);
            output += $"{typeof(T).Name} End \n";

            return output;
        }

        public static string GetUniqueEnumNameInLocalization<T>(string localizedName, Dictionary<T, string> names) where T : Enum
        {
            foreach (T enemy in Enum.GetValues(typeof(T)))
            {
                if (names.TryGetValue(enemy, out string enName))
                {
                    if (string.Equals(enName, localizedName, StringComparison.OrdinalIgnoreCase))
                    {
                        return enemy.ToString();
                    }
                }
            }

            return null;
        }
    }
}
