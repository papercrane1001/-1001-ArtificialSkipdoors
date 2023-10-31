using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prepatcher;
using Verse;
using RimWorld;
using HarmonyLib;
using Mono.Cecil;

namespace _1001_ArtificialSkipdoors
{
    
    //public static class FreePatching
    //{
    //    [FreePatch]
    //    static void RewriteAssembly(ModuleDefinition module)
    //    {
    //        //var type = module.GetType($"{nameof(TestAssemblyTarget)}.{nameof(RewriteTarget)}");
    //        //var method = type.FindMethod(nameof(RewriteTarget.Method));

    //        //foreach (var inst in method.Body.Instructions)
    //        //    if (inst.OpCode == OpCodes.Ldc_I4_0)
    //        //        inst.OpCode = OpCodes.Ldc_I4_1;

    //        var type = 
    //            module.GetType($"{nameof(VanillaPsycastsExpanded.Skipmaster)}.{nameof(VanillaPsycastsExpanded.Skipmaster.Skipdoor)}");
    //        var method = type.FindMethod(nameof(VanillaPsycastsExpanded.Skipmaster.Skipdoor.SpawnSetup));

    //        foreach(var inst in method.Body.Instructions)
    //        {

    //        }
    //    }
    //}
}
