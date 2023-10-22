using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using System.Reflection;
using UnityModManagerNet;

namespace GoldenDragonAestheticRemoval
{
    public class Main
    {
        public static bool Enabled;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            return true;
        }

        /// <summary>
        /// We cannot modify blueprints until after the game has loaded them. We patch BlueprintsCache.Init
        /// to initialize our modifications as soon as the game blueprints have loaded.
        /// </summary>
        [HarmonyPatch(typeof(BlueprintsCache))]
        public static class BlueprintsCache_Patches
        {
            public static bool loaded = false;

            [HarmonyPriority(Priority.First)]
            [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
            public static void Postfix()
            {
                if (loaded) return;
                loaded = true;

                var goldenDragonClassBlueprint = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("daf1235b6217787499c14e4e32142523");
                goldenDragonClassBlueprint.AdditionalVisualSettings.Entries = new BlueprintClassAdditionalVisualSettingsProgression.Entry[0];
            }
        }
    }
}
