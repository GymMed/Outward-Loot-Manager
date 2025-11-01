using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Helpers.Static
{
    public static class AreaFamiliesHelpers
    {
        public static AreaFamily GetAreaFamilyByKeyWord(string keyword)
        {
            foreach(AreaFamily family in AreaManager.AreaFamilies)
            {
                foreach(string familyKeyword in family.FamilyKeywords)
                {
                    if (familyKeyword.Equals(keyword, StringComparison.OrdinalIgnoreCase))
                        return family;
                }
            }

            return null;
        }

        public static AreaFamily GetAreaFamilyByName(string name)
        {
            foreach(AreaFamily family in AreaManager.AreaFamilies)
            {
                if (family.FamilyName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return family;
            }

            return null;
        }

        public static bool DoesAreaFamilyMatch(AreaFamily family)
        {
            AreaFamily areaFamily = GetActiveAreaFamily();

            if(areaFamily == null || family == null)
                return false;

            if (areaFamily.FamilyName == family.FamilyName)
                return true;

            return false;
        }

        public static AreaFamily GetActiveAreaFamily()
        {
            foreach(AreaFamily areaFamily in AreaManager.AreaFamilies)
            {
                foreach(string familyKeyWord in areaFamily.FamilyKeywords)
                {
                    if (SceneManagerHelper.ActiveSceneName.Contains(familyKeyWord))
                        return areaFamily;
                }
            }

            return null;
        }
    }

    /*
    public static readonly AreaFamily[] AreaFamilies = new AreaFamily[]
    {
        new AreaFamily("Cierzo", new string[]
        {
            "Cierzo",
            "Cherso"
        }),
        new AreaFamily("Monsoon", new string[]
        {
            "Monsoon",
            "Hallow",
            "Marsh"
        }),
        new AreaFamily("Levant", new string[]
        {
            "Levant",
            "Abrassar",
            "Desert"
        }),
        new AreaFamily("Berg", new string[]
        {
            "Emercar",
            "Berg"
        }),
        new AreaFamily("Harmattan", new string[]
        {
            "Harmattan",
            "AntiqueField"
        }),
        new AreaFamily("Sirocco", new string[]
        {
            "Sirocco",
            "Caldera"
        }),
        new AreaFamily("Test", new string[]
        {
            "Test"
        })
    };*/
}
