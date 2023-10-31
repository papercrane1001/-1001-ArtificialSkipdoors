using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;

using VanillaPsycastsExpanded.Skipmaster;

namespace _1001_ArtificialSkipdoors
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("rimworld.OneThousandOne.ArtificialSkipdoors");

            System.Reflection.MethodInfo mOriginal = AccessTools.Method(typeof(Skipdoor), "SpawnSetup");
            var mPrefix = AccessTools.Method(typeof(HarmonyPatches), "SpawnSetup_Postfix");

            harmony.Patch(mOriginal, null, new HarmonyMethod(mPrefix));
        }
        //public static bool SpawnSetup_Postfix(ref Map map, ref bool respawningAfterLoad)
        public static bool SpawnSetup_Prefix(ref bool __result, ref Skipdoor __instance, Map map, bool respawningAfterLoad)//Wrong order?  
        {
            //System.Reflection.MethodInfo mCalculateGrowth = AccessTools.Method(typeof(Verse.Pawn_AgeTracker), "CalculateGrowth");
            //mCalculateGrowth.Invoke(__instance, new object[] { 60 * +60000 });

            System.Reflection.MethodInfo mSpawnSetup = AccessTools.Method(typeof(Skipdoor), "SpawnSetup");
            mSpawnSetup.Invoke(__instance, new object[] { map, respawningAfterLoad });

            NVPESkipDoor check = __instance as NVPESkipDoor;
            if (check == null)
            {
                Log.Message("HarPing1");
                return true;
            }
            else
            {
                //__instance.SpawnSetup()
                Log.Message("HarPing2");
                return false;
            }
        }
    }
}
