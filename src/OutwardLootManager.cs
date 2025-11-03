using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SideLoader;
using OutwardModsCommunicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using OutwardModsCommunicator.EventBus;
using OutwardLootManager.Events;
using OutwardLootManager.Managers;
using OutwardLootManager.Drop;
using OutwardLootManager.Utility.Helpers.Static;
using OutwardLootManager.Utility.Enums;
using UnityEngine;

namespace OutwardLootManager
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(OutwardModsCommunicator.OMC.GUID, BepInDependency.DependencyFlags.SoftDependency)]
    public class OutwardLootManager : BaseUnityPlugin
    {
        // Choose a GUID for your project. Change "myname" and "mymodpack".
        public const string GUID = "gymmed.loot_manager";
        // Choose a NAME for your project, generally the same as your Assembly Name.
        public const string NAME = "Loot Manager";
        // Increment the VERSION when you release a new version of your mod.
        public const string VERSION = "0.0.1";

        public const string EVENT_BUS_ALL_GUID = "gymmed.loot_manager_*";

        // Choose prefix for log messages for quicker search and readablity
        public static string prefix = "[Loot-Manager]";

        internal static ManualLogSource Log;

        // If you need settings, define them like so:
        //public static ConfigEntry<bool> ExampleConfig;

        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            Log = this.Logger;
            LogMessage($"Hello world from {NAME} {VERSION}!");

            // Any config settings you define should be set up like this:
            //ExampleConfig = Config.Bind("ExampleCategory", "ExampleSetting", false, "This is an example setting.");

            // Harmony is for patching methods. If you're not patching anything, you can comment-out or delete this line.
            new Harmony(GUID).PatchAll();

            EventBusRegister.RegisterEvents();
            EventBusSubscriber.AddSubscribers();
        }

        public static void AddNewItemDrops(LootableOnDeath lootableOnDeath)
        {
            /*
            ItemDropChance tsarDropChance = new ItemDropChance();
            tsarDropChance.DropChance = 100;
            tsarDropChance.ItemID = 6200010;
            tsarDropChance.MinDropCount = 1;
            tsarDropChance.MaxDropCount = 3;
            tsarDropChance.ChanceReduction = 0;

            ItemDropChance itemDropChance = new ItemDropChance();
            itemDropChance.DropChance = 100;
            itemDropChance.ItemID = 2000160;
            itemDropChance.MinDropCount = 1;
            itemDropChance.MaxDropCount = 2;
            itemDropChance.ChanceReduction = 0;

            List<ItemDropChance> itemDrops = new List<ItemDropChance>();
            itemDrops.Add(tsarDropChance);
            itemDrops.Add(itemDropChance);

            LootManager.Instance.AddLootToLootableOnDeath(lootableOnDeath, itemDrops);
            */
            try
            {
                List<LootRule> lootRules = LootRuleRegistryManager.Instance.GetMatchingRules(lootableOnDeath.Character);

                foreach (LootRule lootRule in lootRules)
                {
                    LootManager.Instance.AddLootToLootableOnDeath(lootableOnDeath, lootRule.itemDropRate);
                }
            }
            catch(Exception ex)
            {
                LogMessage($"We encountered a problem: \"{ex.Message}\"!");
            }
        }

        // Update is called once per frame. Use this only if needed.
        // You also have all other MonoBehaviour methods available (OnGUI, etc)
        internal void Update()
        {
        }

        //  Log message with prefix
        public static void LogMessage(string message)
        {
            Log.LogMessage($"{OutwardLootManager.prefix} {message}");
        }

        // Log message through side loader, helps to see it
        // if you are using UnityExplorer and want to see live logs
        public static void LogSL(string message)
        {
            SL.Log($"{OutwardLootManager.prefix} {message}");
        }

        // Gets mod dll location
        public static string GetProjectLocation()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        [HarmonyPatch(typeof(LootableOnDeath), nameof(LootableOnDeath.OnDeath))]
        public class LootableOnDeath_OnDeath
        {
            static void Prefix(LootableOnDeath __instance)
            {
#if DEBUG
                LogMessage($"{__instance.Character.UID.Value} called me on death! {__instance.Character.Name}");
#endif
                OutwardLootManager.AddNewItemDrops(__instance);
            }
        }

        [HarmonyPatch(typeof(ResourcesPrefabManager), nameof(ResourcesPrefabManager.Load))]
        public class ResourcesPrefabManager_Load
        {
            static void Postfix(ResourcesPrefabManager __instance)
            {
#if DEBUG
                // provide class and method separated by @ for easier live debugging
                LogSL("ResourcesPrefabManager@Load called!");
#endif
                LootRulesSerializer.Instance.LoadPlayerCustomLoots();
                //SceneLoopActionHelpers.StartSceneTesting();
            }
        }
    }
}
