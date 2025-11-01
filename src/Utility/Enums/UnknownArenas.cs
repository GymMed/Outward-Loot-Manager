using OutwardLootManager.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Enums
{
    public enum UnknownArenas
    {
        Chersonese,
        Hallowed,
        Abrassar,
        AntiqueField,
        Emercar,
        Caldera
    }

    public static class UnknownArenasHelper
    {
        public static readonly Dictionary<UnknownArenas, UnknownArenaData> Arenas = new()
        {
            { UnknownArenas.Chersonese, new("ChersoneseDungeonsBosses", "Scene_Chersonese_DungeonsBosses", "Chersonese Arena", "Unknown Arena") },
            { UnknownArenas.Hallowed, new("HallowedDungeonsBosses", "Scene_Hallowed_DungeonsBosses", "Hallowed Marsh Arena", "Unknown Arena") },
            { UnknownArenas.Abrassar, new("AbrassarDungeonsBosses", "Scene_Abrassar_DungeonsBosses", "Abrassar Arena", "Unknown Arena") },
            { UnknownArenas.AntiqueField, new("AntiqueFieldDungeonsBosses", "Scene_Harmattan_DungeonsBosses", "Antique Field Arena", "Unknown Arena") },
            { UnknownArenas.Emercar, new("EmercarDungeonsBosses", "Scene_Emercar_DungeonsBosses", "Enmercar Arena", "Unknown Arena") },
            { UnknownArenas.Caldera, new("CalderaDungeonsBosses", "Scene_Caldera_DungeonsBosses", "Caldera Arena", "Unknown Arena") },
        };

        public static Area GetArea(UnknownArenas arena)
        {
            if (!Arenas.TryGetValue(arena, out UnknownArenaData data))
                return null;

            return AreaManager.Instance.Areas
                .FirstOrDefault(area => area.DefaultName.Equals(data.defaultName, StringComparison.OrdinalIgnoreCase));
        }

        public static List<AreaManager.AreaEnum> GetAllAreaEnums()
        {
            List<Area> allAreas = GetAllAreas();
            List<AreaManager.AreaEnum> areaEnums = new List<AreaManager.AreaEnum>();

            foreach(Area area in allAreas) 
            {
                areaEnums.Add((AreaManager.AreaEnum)area.ID);
            }

            return areaEnums;
        }

        public static List<Area> GetAllAreas()
        {
            var allAreas = new List<Area>();

            foreach (var kvp in Arenas)
            {
                var match = AreaManager.Instance.Areas
                    .FirstOrDefault(area => area.DefaultName.Equals(kvp.Value.defaultName, StringComparison.OrdinalIgnoreCase));

                if (match != null)
                    allAreas.Add(match);
            }

            return allAreas;
        }
    }

}
