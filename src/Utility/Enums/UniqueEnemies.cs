using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardLootManager.Utility.Enums
{
    public enum UniqueEnemies
    {
        Accursed_Wendigo,
        Altered_Gargoyle,
        Ancestral_General,
        Ancestral_Soldier,
        Bloody_Alexis,
        Calygrey_Hero,
        Chromatic_Arcane_Elemental,
        Cracked_Gargoyle,
        Elemental_Parasite,
        Executioner_Bug,
        Ghost_of_Vanasse,
        Giant_Horror,
        Glacial_Tuanosaur,
        Golden_Matriarch,
        Grandmother_Medyse,
        Greater_Grotesque,
        Guardian_of_the_Compass,
        Kazite_Admiral,
        Lightning_Dancer,
        Liquid_Cooled_Golem,
        Luke_the_Pearlescent,
        Mad_Captains_Bones,
        Matriarch_Myrmitaur,
        Quartz_Elemental,
        Razorhorn_Stekosaur,
        Royal_Manticore,
        Rusted_Enforcer,
        Sandrose_Horror,
        She_Who_Speaks,
        That_Annoying_Troglodyte,
        The_Crusher,
        The_First_Cannibal,
        The_Last_Acolyte,
        Thunderbolt_Golem,
        Titanic_Guardian_Mk_7,
        Troglodyte_Archmage,
        Tyrant_of_the_Hive,
        Vile_Illuminator,
        Virulent_Hiveman,
        Volcanic_Gastrocin,
    }

    public static class UniqueEnemiesHelper
    {
        // wiki names
        public static readonly Dictionary<UniqueEnemies, string> Names = new()
        {
            { UniqueEnemies.Accursed_Wendigo, "Accursed Wendigo" },
            { UniqueEnemies.Altered_Gargoyle, "Altered Gargoyle" },
            { UniqueEnemies.Ancestral_General, "Ancestral General" },
            { UniqueEnemies.Ancestral_Soldier, "Ancestral Soldier" },
            { UniqueEnemies.Bloody_Alexis, "Bloody Alexis" },
            { UniqueEnemies.Calygrey_Hero, "Calygrey Hero" },
            { UniqueEnemies.Chromatic_Arcane_Elemental, "Chromatic Arcane Elemental" },
            { UniqueEnemies.Cracked_Gargoyle, "Cracked Gargoyle" },
            { UniqueEnemies.Elemental_Parasite, "Elemental Parasite" },
            { UniqueEnemies.Executioner_Bug, "Executioner Bug" },
            { UniqueEnemies.Ghost_of_Vanasse, "Ghost of Vanasse" },
            { UniqueEnemies.Giant_Horror, "Giant Horror" },
            { UniqueEnemies.Glacial_Tuanosaur, "Glacial Tuanosaur" }, //TuanosaurIce" },
            { UniqueEnemies.Golden_Matriarch, "Golden Matriarch" },
            { UniqueEnemies.Grandmother_Medyse, "Grandmother Medyse" },
            { UniqueEnemies.Greater_Grotesque, "Greater Grotesque" },
            { UniqueEnemies.Guardian_of_the_Compass, "Guardian of the Compass" },
            { UniqueEnemies.Kazite_Admiral, "Kazite Admiral" },
            { UniqueEnemies.Lightning_Dancer, "Lightning Dancer" },
            { UniqueEnemies.Liquid_Cooled_Golem, "Liquid-Cooled Golem" },
            { UniqueEnemies.Luke_the_Pearlescent, "Luke the Pearlescent" },
            { UniqueEnemies.Mad_Captains_Bones, "Mad Captain's Bones" },
            { UniqueEnemies.Matriarch_Myrmitaur, "Matriarch Myrmitaur" },
            { UniqueEnemies.Quartz_Elemental, "Quartz Elemental" },
            { UniqueEnemies.Razorhorn_Stekosaur, "Razorhorn Stekosaur" },
            { UniqueEnemies.Royal_Manticore, "Royal Manticore" },
            { UniqueEnemies.Rusted_Enforcer, "Rusted Enforcer" },
            { UniqueEnemies.Sandrose_Horror, "Sandrose Horror" },
            { UniqueEnemies.She_Who_Speaks, "She Who Speaks" },
            { UniqueEnemies.That_Annoying_Troglodyte, "That Annoying Troglodyte" },
            { UniqueEnemies.The_Crusher, "The Crusher" },
            { UniqueEnemies.The_First_Cannibal, "The First Cannibal" },
            { UniqueEnemies.The_Last_Acolyte, "The Last Acolyte" },
            { UniqueEnemies.Thunderbolt_Golem, "Thunderbolt Golem" },
            { UniqueEnemies.Titanic_Guardian_Mk_7, "Titanic Guardian Mk-7 Golem" },
            { UniqueEnemies.Troglodyte_Archmage, "Troglodyte Archmage" },
            { UniqueEnemies.Tyrant_of_the_Hive, "Tyrant of the Hive" },
            { UniqueEnemies.Vile_Illuminator, "Vile Illuminator" },
            { UniqueEnemies.Virulent_Hiveman, "Virulent Hiveman" },
            { UniqueEnemies.Volcanic_Gastrocin, "Volcanic Gastrocin" },
        };

        // useless
        public static readonly Dictionary<UniqueEnemies, string> NamesLoc = new()
        {
            { UniqueEnemies.Accursed_Wendigo, "Unique_DefEd_Wendigo" },
            { UniqueEnemies.Altered_Gargoyle, "Unique_DefEd_AlteredGargoyle" },
            { UniqueEnemies.Ancestral_General, "Unique_DefEd_AncestorBig" },
            { UniqueEnemies.Ancestral_Soldier, "Unique_DefEd_AncestorSmall" },
            { UniqueEnemies.Bloody_Alexis, "Unique_DefEd_ArmoredThug" },
            { UniqueEnemies.Calygrey_Hero, "Elite_Calygrey" },
            { UniqueEnemies.Chromatic_Arcane_Elemental, "Unique_DefEd_ChromaElemental" },
            { UniqueEnemies.Cracked_Gargoyle, "Unique_DefEd_CrackedGargoyle" },
            { UniqueEnemies.Elemental_Parasite, "Wildlife_CrescentShark" },// crazy name...
            { UniqueEnemies.Executioner_Bug, "Unique_DefEd_ExecutionerBug" },
            { UniqueEnemies.Ghost_of_Vanasse, "Undead_GhostVanasse" },
            { UniqueEnemies.Giant_Horror, "Giant_Horror" },
            { UniqueEnemies.Glacial_Tuanosaur, "Unique_DefEd_GlacialTuano" },//TuanosaurIce" },
            { UniqueEnemies.Golden_Matriarch, "Unique_DefEd_GoldenMatriarch" },
            { UniqueEnemies.Grandmother_Medyse, "Unique_DefEd_GrandmotherMedyse" },
            { UniqueEnemies.Greater_Grotesque, "Unique_DefEd_GreaterGrotestue" },
            { UniqueEnemies.Guardian_of_the_Compass, "Golem_Basic2" },
            { UniqueEnemies.Kazite_Admiral, "Unique_DefEd_KaziteCaptain" },
            { UniqueEnemies.Lightning_Dancer, "Unique_DefEd_LightningDancer" },
            { UniqueEnemies.Liquid_Cooled_Golem, "Unique_DefEd_LiquidGolem" },
            { UniqueEnemies.Luke_the_Pearlescent, "Bandit_Standard_Captain2" },
            { UniqueEnemies.Mad_Captains_Bones, "Undead_Skeleton2" },
            { UniqueEnemies.Matriarch_Myrmitaur, "Elite_Myrm" },
            { UniqueEnemies.Quartz_Elemental, "Unique_DefEd_QuartzElemental" },
            { UniqueEnemies.Razorhorn_Stekosaur, "Unique_DefEd_BlackSteko" },
            { UniqueEnemies.Royal_Manticore, "Wildlife_Manticore2" },
            { UniqueEnemies.Rusted_Enforcer, "Unique_DefEd_RustyGolem" },
            { UniqueEnemies.Sandrose_Horror, "Unique_DefEd_SandroseHorror" },
            { UniqueEnemies.She_Who_Speaks, "Unique_DefEd_BossDweller" },
            { UniqueEnemies.That_Annoying_Troglodyte, "Unique_DefEd_AnnoyingTrog" },
            { UniqueEnemies.The_Crusher, "Unique_DefEd_DesertCrusher" },
            { UniqueEnemies.The_First_Cannibal, "Wildlife_Wendigo2" },
            { UniqueEnemies.The_Last_Acolyte, "Unique_DefEd_LastAcolyte" },
            { UniqueEnemies.Thunderbolt_Golem, "Unique_DefEd_ProtypeForgeGolem" },
            { UniqueEnemies.Titanic_Guardian_Mk_7, "Boss_DepracatedTitanticGolem" },
            { UniqueEnemies.Troglodyte_Archmage, "Unique_DefEd_TrogMage" },
            { UniqueEnemies.Tyrant_of_the_Hive, "Undead_Hivelord2" },
            { UniqueEnemies.Vile_Illuminator, "Unique_DefEd_VileIlluminator" },
            { UniqueEnemies.Virulent_Hiveman, "Unique_DefEd_VirulentHiveman" },
            { UniqueEnemies.Volcanic_Gastrocin, "Unique_DefEd_VolcanicSlug" },
        };

        //gotten through Area.GetName()
        public static readonly Dictionary<UniqueEnemies, string> WikiLocations = new()
        {
            { UniqueEnemies.Accursed_Wendigo , "Corrupted Tombs" },
            { UniqueEnemies.Altered_Gargoyle , "Ziggurat Passage" },
            { UniqueEnemies.Ancestral_General , "Necropolis" },
            { UniqueEnemies.Ancestral_Soldier , "Necropolis" },
            { UniqueEnemies.Bloody_Alexis , "Undercity Passage" },
            { UniqueEnemies.Calygrey_Hero , "Steam Bath Tunnels" },
            { UniqueEnemies.Chromatic_Arcane_Elemental , "Compromised Mana Transfer Station" },
            { UniqueEnemies.Cracked_Gargoyle , "Ark of the Exiled" },
            { UniqueEnemies.Elemental_Parasite , "Crumbling Loading Docks" },
            { UniqueEnemies.Executioner_Bug , "The Slide" },
            { UniqueEnemies.Ghost_of_Vanasse , "Chersonese" },
            { UniqueEnemies.Giant_Horror , "Crumbling Loading Docks" },
            { UniqueEnemies.Glacial_Tuanosaur , "Conflux Chambers" },
            { UniqueEnemies.Golden_Matriarch , "Voltaic Hatchery" },
            { UniqueEnemies.Grandmother_Medyse , "Sulphuric Caverns" },
            { UniqueEnemies.Greater_Grotesque , "Lost Golem Manufacturing Facility" },
            { UniqueEnemies.Guardian_of_the_Compass , "The Walled Garden" },
            { UniqueEnemies.Kazite_Admiral , "Abandoned Living Quarters" },
            { UniqueEnemies.Lightning_Dancer , "Ancient Foundry" },
            { UniqueEnemies.Liquid_Cooled_Golem , "Destroyed Test Chambers" },
            { UniqueEnemies.Luke_the_Pearlescent , "Ruins of Old Levant" },
            { UniqueEnemies.Mad_Captains_Bones , "Pirates' Hideout" },
            { UniqueEnemies.Matriarch_Myrmitaur , "Myrmitaur’s Haven" },
            { UniqueEnemies.Quartz_Elemental , "The Grotto of Chalcedony" },
            { UniqueEnemies.Razorhorn_Stekosaur , "Reptilian Lair" },
            { UniqueEnemies.Royal_Manticore , "Enmerkar Forest" },
            { UniqueEnemies.Rusted_Enforcer , "Ghost Pass" },
            { UniqueEnemies.Sandrose_Horror , "Sand Rose Cave" },
            { UniqueEnemies.She_Who_Speaks , "The Vault of Stone" },
            { UniqueEnemies.That_Annoying_Troglodyte , "Jade Quarry" },
            { UniqueEnemies.The_Crusher , "Ancestor’s Resting Place" },
            { UniqueEnemies.The_First_Cannibal , "Face of the Ancients" },
            { UniqueEnemies.The_Last_Acolyte , "Stone Titan Caves" },
            { UniqueEnemies.Thunderbolt_Golem , "Electric Lab" },
            { UniqueEnemies.Titanic_Guardian_Mk_7 , "Ruined Warehouse" },
            { UniqueEnemies.Troglodyte_Archmage , "Blister Burrow" },
            { UniqueEnemies.Tyrant_of_the_Hive , "Forest Hives" },
            { UniqueEnemies.Vile_Illuminator , "Cabal of Wind Temple" },
            { UniqueEnemies.Virulent_Hiveman , "Ancient Hive" },
            { UniqueEnemies.Volcanic_Gastrocin , "The Eldest Brother" },
        };

        public static readonly Dictionary<UniqueEnemies, string> GameLocations = new()
        {
            { UniqueEnemies.Accursed_Wendigo , "Corrupted Tombs" },
            { UniqueEnemies.Altered_Gargoyle , "Ziggurat Passage" },
            { UniqueEnemies.Ancestral_General , "Necropolis" },
            { UniqueEnemies.Ancestral_Soldier , "Necropolis" },
            { UniqueEnemies.Bloody_Alexis , "Undercity Passage" },
            { UniqueEnemies.Calygrey_Hero , "Steam Bath Tunnels" },
            { UniqueEnemies.Chromatic_Arcane_Elemental , "Compromised Mana Transfer Station" },
            { UniqueEnemies.Cracked_Gargoyle , "Ark of the Exiled" },
            { UniqueEnemies.Elemental_Parasite , "Crumbling Loading Docks" },
            { UniqueEnemies.Executioner_Bug , "The Slide" },
            { UniqueEnemies.Ghost_of_Vanasse , "Chersonese" },
            { UniqueEnemies.Giant_Horror , "Crumbling Loading Docks" },
            { UniqueEnemies.Glacial_Tuanosaur , "Conflux Chambers" },
            { UniqueEnemies.Golden_Matriarch , "Voltaic Hatchery" },
            { UniqueEnemies.Grandmother_Medyse , "Sulphuric Caverns" },
            { UniqueEnemies.Greater_Grotesque , "Lost Golem Manufacturing Facility" },
            { UniqueEnemies.Guardian_of_the_Compass , "Abrassar" },//"---The Walled Garden---" },
            { UniqueEnemies.Kazite_Admiral , "Abandoned Living Quarters" },
            { UniqueEnemies.Lightning_Dancer , "Ancient Foundry" },
            { UniqueEnemies.Liquid_Cooled_Golem , "Destroyed Test Chambers" },
            { UniqueEnemies.Luke_the_Pearlescent , "Abrassar" },//"---Ruins of Old Levant---" },
            { UniqueEnemies.Mad_Captains_Bones , "Chersonese Misc. Dungeons" },//"---Pirates' Hideout---" },
            { UniqueEnemies.Matriarch_Myrmitaur , "Myrmitaur’s Haven" },
            { UniqueEnemies.Quartz_Elemental , "The Grotto of Chalcedony" },
            { UniqueEnemies.Razorhorn_Stekosaur , "Reptilian Lair" },
            { UniqueEnemies.Royal_Manticore , "Enmerkar Forest" },
            { UniqueEnemies.Rusted_Enforcer , "Ghost Pass" },
            { UniqueEnemies.Sandrose_Horror , "Sand Rose Cave" },
            { UniqueEnemies.She_Who_Speaks , "The Vault of Stone" },
            { UniqueEnemies.That_Annoying_Troglodyte , "Jade Quarry" },
            { UniqueEnemies.The_Crusher , "Ancestor’s Resting Place" },
            { UniqueEnemies.The_First_Cannibal , "Face of the Ancients" },
            { UniqueEnemies.The_Last_Acolyte , "Stone Titan Caves" },
            { UniqueEnemies.Thunderbolt_Golem , "Electric Lab" },
            { UniqueEnemies.Titanic_Guardian_Mk_7 , "Ruined Warehouse" },
            { UniqueEnemies.Troglodyte_Archmage , "Blister Burrow" },
            { UniqueEnemies.Tyrant_of_the_Hive , "Forest Hives" },
            { UniqueEnemies.Vile_Illuminator , "Cabal of Wind Temple" },
            { UniqueEnemies.Virulent_Hiveman , "Ancient Hive" },
            { UniqueEnemies.Volcanic_Gastrocin , "The Eldest Brother" },
        };

        // Reverse lookup
        public static bool TryGetEnum(string name, out UniqueEnemies boss)
        {
            foreach (var kvp in Names)
            {
                if (kvp.Value.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    boss = kvp.Key;
                    return true;
                }
            }
            boss = default;
            return false;
        }
    }

}
