﻿using System;
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
            //Log.Message("Ping0");
            var har = new Harmony(Content.PackageIdPlayerFacing);
            System.Reflection.MethodInfo mOriginal = AccessTools.Method(typeof(VanillaPsycastsExpanded.Skipmaster.Skipdoor), "SpawnSetup");
            var mTranspiler = AccessTools.Method(
                typeof(SkipDoor_SpawnSetup_Transpiler),
                "SkipDoor_SpawnSetup_Actual_Transpiler");
            if(har == null) { Log.Message("har null"); }
            if(mOriginal == null) { Log.Message("mOriginal null"); }
            if(mTranspiler == null) { Log.Message("mTranspiler null"); }
            har.Patch(mOriginal, null, null, new HarmonyMethod(mTranspiler));
        }

        
    }


    //[UsedImplicitly]
    //[StaticConstructorOnStartup]
    //public static class Setup
    //{
    //    static Setup()
    //    {
    //        var harmony = new Harmony(Content.PackageIdPlayerFacing);
    //        harmony.PatchAll();
    //    }
    //}
}