using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using HarmonyLib;
using JetBrains.Annotations;
//using TerrainMovement;
using UnityEngine;
using Verse;
using Verse.AI;

using System.Reflection.Emit;
using System.Linq;

namespace _1001_ArtificialSkipdoors
{
    [UsedImplicitly]
    public class ModEntry : Mod
    {
        public ModEntry(ModContentPack content) : base(content)
        {
            var har = new Harmony(Content.PackageIdPlayerFacing);
            //var vfeCoreAssembly = typeof(VFECore.DoorTeleporterExtension).Assembly;
            var vfeCoreAssembly = typeof(VFECore.ShieldUtility).Assembly;
            Log.Message($"[ArtificialSkipdoors] Loaded VFECore assembly: {vfeCoreAssembly.FullName}");
            try
            {
                System.Reflection.MethodInfo mOriginal =
                AccessTools.Method(typeof(VanillaPsycastsExpanded.Skipmaster.Skipdoor), "SpawnSetup");
                var mTranspiler = AccessTools.Method(
                    typeof(SkipDoor_SpawnSetup_Transpiler),
                    "SkipDoor_SpawnSetup_Actual_Transpiler");
                if (har == null) { Log.Message("har null"); }
                if (mOriginal == null) { Log.Message("mOriginal null"); }
                if (mTranspiler == null) { Log.Message("mTranspiler null"); }
                har.Patch(mOriginal, null, null, new HarmonyMethod(mTranspiler));

                System.Reflection.MethodInfo mOGUI =
                    AccessTools.Method(typeof(VanillaPsycastsExpanded.Skipmaster.Skipdoor), "GetDoorTeleporterGismoz");
                var mTrGUI = AccessTools.Method(
                    typeof(SkipDoor_GetDoorTeleporterGismoz_Transpiler),
                    "SkipDoor_GetDoorTeleporterGismoz_Actual_Transpiler");
                har.Patch(mOGUI, null, null, new HarmonyMethod(mTrGUI));
            }
            catch (Exception e)
            {
                Log.Error($"[ArtificialSkipdoors] Error patching methods: {e}");
            }
        }

        
    }
}
