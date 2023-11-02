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
        public static IEnumerable<CodeInstruction> SkipDoor_SpawnSetup_Actual_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilg)
        {
            Log.Message("ASTranspiler Running");
            List<CodeInstruction> instructionList = instructions.ToList();
            bool found = false;
            
            for (int i = 0; i < instructionList.Count; i++)
            {
                if (instructionList[i].opcode == OpCodes.Ldarg_0 && i + 2 < instructionList.Count && found == false)
                {
                    Log.Message("Found 'this'");
                    if(instructionList[i + 1].operand is FieldInfo fieldInfo_pawn && fieldInfo_pawn.Name == "Pawn")
                    {
                        Log.Message("Found this.Pawn");
                        if (instructionList[i + 2].opcode == OpCodes.Call)
                        {
                            Log.Message("Found relevant method call, running patch");
                            found = true;

                            yield return new CodeInstruction(OpCodes.Ldarg_0);
                            yield return new CodeInstruction(OpCodes.Ldfld, fieldInfo_pawn);
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

    /*
    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        this.Pawn.Psycasts().AddMinHeatGiver(this);
        if (!respawningAfterLoad)
        {
            this.Pawn.psychicEntropy.TryAddEntropy(50f, this, true, true);
        }
    }
    */

}
