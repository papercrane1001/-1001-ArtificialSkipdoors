using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;

using VanillaPsycastsExpanded.Skipmaster;
using System.Reflection.Emit;

namespace _1001_ArtificialSkipdoors
{
    //[HarmonyPatch(typeof(Skipdoor), "SpawnSetup")]
    [HarmonyPatch(typeof(Skipdoor), "SpawnSetup")]
    public static class SkipDoor_SpawnSetup_Transpiler
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Skipdoor), "SpawnSetup");
        }
        public static IEnumerable<CodeInstruction> SkipDoor_SpawnSetup_Actual_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Log.Message("ASTranspiler Running");
            List<CodeInstruction> instructionList = instructions.ToList();
            
            for (int i = 0; i < instructionList.Count; i++)
            {
                if (instructionList[i].opcode == OpCodes.Ldarg_0 && i + 2 < instructionList.Count)
                {
                    // Look for the "ldarg.0" instruction, which loads the "this" argument
                    // Check if the subsequent instructions match the pattern for accessing "this.Pawn"
                    if (instructionList[i + 1].opcode == OpCodes.Ldfld && instructionList[i + 2].operand is FieldInfo fieldInfo && fieldInfo.Name == "Pawn")
                    {
                        Log.Message("if's passed");
                        // Insert your null check and return logic
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldfld, fieldInfo);
                        yield return new CodeInstruction(OpCodes.Brfalse, instructionList[i]);
                        yield return instructionList[i];
                    }
                    else
                    {
                        yield return instructionList[i];
                    }
                }
                else
                {
                    yield return instructionList[i];
                }
            }
        }
    }

    
    
}
