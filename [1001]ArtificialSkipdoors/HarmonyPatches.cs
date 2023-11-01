﻿using System;
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
            bool found = false;
            
            for (int i = 0; i < instructionList.Count; i++)
            {
                if (instructionList[i].opcode == OpCodes.Ldarg_0 && i + 2 < instructionList.Count && found == false)
                {
                    Log.Message("Found 'this'");
                    // Look for the "ldarg.0" instruction, which loads the "this" argument
                    // Check if the subsequent instructions match the pattern for accessing "this.Pawn"
                    if(instructionList[i + 1].operand is FieldInfo fieldInfo_pawn && fieldInfo_pawn.Name == "Pawn")
                    {
                        Log.Message("Found this.Pawn");
                        //Log.Message("fieldInfo.Name: " + ((FieldInfo)instructionList[i + 2].operand).Name); // <-- threw an error
                        //if (instructionList[i+2].operand is FieldInfo fieldInfo_Psycasts)
                        //{
                        //    Log.Message("fieldInfo.Name: " + fieldInfo_pawn.Name);
                        //}
                        //if(fieldInfo.Name == "")
                        Log.Message("Point: " + instructionList[i + 2].operand.ToString()); // Should be VanillaPsycastsExpanded.Hediff_PsycastAbilities Psycasts(Verse.Pawn)

                        if (instructionList[i + 2].opcode == OpCodes.Call)
                        {
                            Log.Message("Found method call, running patch");
                            found = true;
                            //Log.Message(fieldInfo_Psycasts.Name);
                            
                            yield return new CodeInstruction(OpCodes.Ldarg_0);
                            Log.Message("First line ran");
                            yield return new CodeInstruction(OpCodes.Ldfld, fieldInfo_pawn);
                            Log.Message("Second line ran");
                            yield return new CodeInstruction(OpCodes.Brfalse, instructionList[i + 1]);
                            //yield return new CodeInstruction(OpCodes.Brfalse);
                            //Label label = AbstractShapeGenerator.
                            //yield return new CodeInstruction(OpCodes.Brfalse, null);

                            Log.Message("Third line ran");
                            yield return instructionList[i];
                        }
                        else
                        {
                            yield return instructionList[i];
                        }

                        //if (instructionList[i + 2].operand is FieldInfo fieldInfo_Psycasts)// && fieldInfo_Psycasts.Name != "psychicEntropy")
                        //{
                        //    Log.Message("Made fieldInfo_Psycasts");
                        //    if (instructionList[i+2].opcode == OpCodes.Call)
                        //    {
                        //        Log.Message("Found method call, running patch");
                        //        found = true;
                        //        //Log.Message(fieldInfo_Psycasts.Name);
                        //        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        //        yield return new CodeInstruction(OpCodes.Ldfld, fieldInfo_pawn);
                        //        yield return new CodeInstruction(OpCodes.Brfalse, instructionList[i]);
                        //        yield return instructionList[i];
                        //    }
                        //    else
                        //    {
                        //        yield return instructionList[i];
                        //    }
                            
                        //}
                        //else
                        //{
                        //    yield return instructionList[i];
                        //}
                    }
                    //instructionList[i + 2].operand is FieldInfo fieldInfo;
                    //if (instructionList[i + 1].opcode == OpCodes.Ldfld && instructionList[i + 2].operand is FieldInfo fieldInfo && fieldInfo.Name == "Pawn")
                    //{
                    //    Log.Message("if's passed");
                    //    // Insert your null check and return logic
                    //    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    //    yield return new CodeInstruction(OpCodes.Ldfld, fieldInfo);
                    //    yield return new CodeInstruction(OpCodes.Brfalse, instructionList[i]);
                    //    yield return instructionList[i];
                    //}
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
