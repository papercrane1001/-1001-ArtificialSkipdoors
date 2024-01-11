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
using VFECore;

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
            //Log.Message("ASTranspiler Running");
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

    [HarmonyPatch]
    public static class HarmonyPatches
    {
        public static IEnumerable<Gizmo> GetJustRenameGizmo(Skipdoor door)
        {
            DoorTeleporterExtension extension = door.def.GetModExtension<DoorTeleporterExtension>();
            DoorTeleporterMaterials doorMaterials = DoorTeleporter.doorTeleporterMaterials[door.def];

            if (doorMaterials.RenameIcon != null)
            {
                yield return new Command_Action 
                {
                    defaultLabel = extension.renameLabelKey.Translate(),
                    defaultDesc = extension.renameDescKey.Translate(),
                    icon = doorMaterials.RenameIcon,
                    action = delegate
                    {
                        Find.WindowStack.Add(new Dialog_RenameDoorTeleporter(door));
                    }
                };
            }
        }
    }

    [HarmonyPatch(typeof(Skipdoor), "GetDoorTeleporterGismoz")]
    public static class SkipDoor_GetDoorTeleporterGismoz_Transpiler
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Skipdoor), "GetDoorTeleporterGismoz");
        }


        public static IEnumerable<CodeInstruction> SkipDoor_GetDoorTeleporterGismoz_Actual_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilg)
        {
            List<CodeInstruction> instructionList = instructions.ToList();
            bool found = false;
            FieldInfo pawnInfo = AccessTools.Field(typeof(Skipdoor), nameof(Skipdoor.Pawn));

            for (int i = 0; i < instructionList.Count; i++){
                if (
                    //i == 1)
                    found == false &&
                    instructionList[i].opcode == OpCodes.Newobj
                    )

                {
                    found = true;
                    MethodInfo myRenameGizmo = AccessTools.Method(
                        typeof(HarmonyPatches), nameof(HarmonyPatches.GetJustRenameGizmo));
                    
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, pawnInfo);
                    Label label = ilg.DefineLabel();

                    yield return new CodeInstruction(OpCodes.Brtrue, label);

                    yield return new CodeInstruction(OpCodes.Call, myRenameGizmo);

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


    /*
     * public override IEnumerable<Gizmo> GetDoorTeleporterGismoz()
		{
			DoorTeleporterExtension extension = this.def.GetModExtension<DoorTeleporterExtension>();
			DoorTeleporterMaterials doorMaterials = DoorTeleporter.doorTeleporterMaterials[this.def];
			bool flag = doorMaterials.DestroyIcon != null;
			if (flag)
			{
				yield return new Command_Action
				{
					defaultLabel = extension.destroyLabelKey.Translate(),
					defaultDesc = extension.destroyDescKey.Translate(this.Pawn.NameFullColored),
					icon = doorMaterials.DestroyIcon,
					action = delegate()
					{
						this.Destroy(DestroyMode.Vanish);
					}
				};
			}
			bool flag2 = doorMaterials.RenameIcon != null;
			if (flag2)
			{
				yield return new Command_Action
				{
					defaultLabel = extension.renameLabelKey.Translate(),
					defaultDesc = extension.renameDescKey.Translate(),
					icon = doorMaterials.RenameIcon,
					action = delegate()
					{
						Find.WindowStack.Add(new Dialog_RenameDoorTeleporter(this));
					}
				};
			}
			yield break;
		}
    */
}
