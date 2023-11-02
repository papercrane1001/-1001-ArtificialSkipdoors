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
    [HarmonyPatch(typeof(Skipdoor), "SpawnSetup")]
    public static class SkipDoor_SpawnSetup_Transpiler
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Skipdoor), "SpawnSetup");
        }
        public static IEnumerable<CodeInstruction> SkipDoor_SpawnSetup_Actual_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilg)
        {
            Log.Message("ASTranspiler Running");
            List<CodeInstruction> instructionList = instructions.ToList();
            bool found = false;

            FieldInfo pawnInfo = AccessTools.Field(typeof(Skipdoor), nameof(Skipdoor.Pawn));
            
            for (int i = 0; i < instructionList.Count; i++)
            {
                if (instructionList[i].opcode == OpCodes.Ldarg_0 && i + 2 < instructionList.Count && found == false &&
                    instructionList[i + 1].LoadsField(pawnInfo) &&
                    instructionList[i + 2].opcode == OpCodes.Call)
                {
                    Log.Message("Found relevant method call, running patch");
                    found = true;

                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, pawnInfo);
                    Label label = ilg.DefineLabel();

                    yield return new CodeInstruction(OpCodes.Brtrue, label);
                    yield return new CodeInstruction(OpCodes.Ret);
                    yield return new CodeInstruction(OpCodes.Nop).WithLabels(label);

                    yield return instructionList[i];
                        
                }
                else
                {
                    yield return instructionList[i];
                }
            }
        }
    }

}
