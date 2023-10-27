﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using VFECore;
using VanillaPsycastsExpanded;
using VanillaPsycastsExpanded.Skipmaster;
using VFECore.Abilities;

namespace _1001_ArtificialSkipdoors
{
    public class NVPEDoorBuilding : Building
    {
        public NVPESkipDoor door;
        //public Pawn pawn;
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            //pawn = new Pawn();
            base.SpawnSetup(map, respawningAfterLoad);
            //Skipdoor Sdoor = (Skipdoor)ThingMaker.MakeThing(VPE_DefOf.VPE_Skipdoor, null);

            door = (NVPESkipDoor)ThingMaker.MakeThing(NVPE_DefOf.NVPESkipDoorDef, null);

            //door = new NVPESkipDoor();
            //door.Label = "skipdoor";

            //door.Pawn = pawn;
            Find.WindowStack.Add(new Dialog_RenameDoorTeleporter(door));
            GenSpawn.Spawn(door, Position, map, WipeMode.Vanish);
        }

        public override void PostMake()
        {
            base.PostMake();
            //door = (Skipdoor)ThingMaker.MakeThing(VPE_DefOf.VPE_Skipdoor, null);
            //Find.WindowStack.Add(new Dialog_RenameDoorTeleporter(door));
            //GenSpawn.Spawn(door, Position, Map, WipeMode.Vanish);
        }
        public override void Tick()
        {
            base.Tick();
            //if(door == null)
            //{
            //    door = (Skipdoor)ThingMaker.MakeThing(VPE_DefOf.VPE_Skipdoor, null);
            //    Find.WindowStack.Add(new Dialog_RenameDoorTeleporter(door));
            //    GenSpawn.Spawn(door, Position, Map, WipeMode.Vanish);
            //}
        }
    }
    public class NVPEDoorPlaceWorker : PlaceWorker
    {
        public override void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
        {
            base.PostPlace(map, def, loc, rot);
            //Skipdoor door = new Skipdoor()
            //Skipdoor door = WorldComponent_DoorTeleporterManager.Instance.DoorTeleporters.
            Skipdoor door = (Skipdoor)ThingMaker.MakeThing(VPE_DefOf.VPE_Skipdoor, null);
            Find.WindowStack.Add(new Dialog_RenameDoorTeleporter(door));
            GenSpawn.Spawn(door, loc, map, WipeMode.Vanish);


            //NVPEDoorTeleporter door = new NVPEDoorTeleporter();
            //WorldComponent_DoorTeleporterManager.Instance.DoorTeleporters.Add(door);
        }
    }

    public class NVPESkipDoor : Skipdoor
    {
        //public NVPESkipDoor(Skipdoor source)
        //{
        //    this = source as NVPESkipDoor;
        //}
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            //base.SpawnSetup(map, respawningAfterLoad);
            #region Thing
            map.listerThings.Add(this);
            map.thingGrid.Register(this);
            map.gasGrid.Notify_ThingSpawned(this);
            map.mapTemperature.Notify_ThingSpawned(this);
            if (map.IsPlayerHome)
            {
                EverSeenByPlayer = true;
            }

            if (Find.TickManager != null)
            {
                Find.TickManager.RegisterAllTickabilityFor(this);
            }

            DirtyMapMesh(map);
            if (def.drawerType != DrawerType.MapMeshOnly)
            {
                map.dynamicDrawManager.RegisterDrawable(this);
            }

            map.tooltipGiverList.Notify_ThingSpawned(this);
            if (def.CanAffectLinker)
            {
                map.linkGrid.Notify_LinkerCreatedOrDestroyed(this);
                map.mapDrawer.MapMeshDirty(Position, MapMeshFlag.Things, regenAdjacentCells: true, regenAdjacentSections: false);
            }

            if (def.pathCost != 0 || def.passability == Traversability.Impassable)
            {
                map.pathing.RecalculatePerceivedPathCostUnderThing(this);
            }

            if (def.AffectsReachability)
            {
                map.reachability.ClearCache();
            }

            map.coverGrid.Register(this);
            if (def.category == ThingCategory.Item)
            {
                map.listerHaulables.Notify_Spawned(this);
                map.listerMergeables.Notify_Spawned(this);
            }

            map.attackTargetsCache.Notify_ThingSpawned(this);
            (map.regionGrid.GetValidRegionAt_NoRebuild(Position)?.Room)?.Notify_ContainedThingSpawnedOrDespawned(this);
            StealAIDebugDrawer.Notify_ThingChanged(this);
            IHaulDestination haulDestination = this as IHaulDestination;
            if (haulDestination != null)
            {
                map.haulDestinationManager.AddHaulDestination(haulDestination);
            }

            if (this is IThingHolder && Find.ColonistBar != null)
            {
                Find.ColonistBar.MarkColonistsDirty();
            }

            if (def.category == ThingCategory.Item)
            {
                SlotGroup slotGroup = Position.GetSlotGroup(map);
                if (slotGroup != null && slotGroup.parent != null)
                {
                    slotGroup.parent.Notify_ReceivedThing(this);
                }
            }

            if (def.receivesSignals)
            {
                Find.SignalManager.RegisterReceiver(this);
            }

            if (!respawningAfterLoad)
            {
                QuestUtility.SendQuestTargetSignals(questTags, "Spawned", this.Named("SUBJECT"));
            }
            #endregion

            #region ThingWithComps
            //if (comps != null)
            //{
            //    for (int i = 0; i < comps.Count; i++)
            //    {
            //        comps[i].PostSpawnSetup(respawningAfterLoad);
            //    }
            //}
            #endregion

            #region DoorTeleporter
            WorldComponent_DoorTeleporterManager.Instance.DoorTeleporters.Add(this);
            DoorTeleporterMaterials mat = doorTeleporterMaterials[def];
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                background1 = new RenderTexture(mat.backgroundTex.width, mat.backgroundTex.height, 0);
                background2 = new RenderTexture(mat.backgroundTex.width, mat.backgroundTex.height, 0);
                backgroundMat = new Material(ShaderDatabase.TransparentPostLight);
                RecacheBackground();
            });
            #endregion

            #region Skipdoor

            #endregion
        }
    }

    
    //public class NVPEAbility_Skipdoor : VFECore.Abilities.Ability
    //{
    //    public override void Cast(params GlobalTargetInfo[] targets)
    //    {
    //        base.Cast(targets);
    //        foreach(GlobalTargetInfo target in targets)
    //        {
    //            Skipdoor skipdoor = (Skipdoor)ThingMaker.MakeThing(VPE_DefOf.VPE_Skipdoor, null);
    //            //skipdoor.Pawn = this.pawn;
    //            Find.WindowStack.Add(new Dialog_RenameDoorTeleporter(skipdoor));
    //            GenSpawn.Spawn(skipdoor, target.Cell, target.Map, WipeMode.Vanish);
    //        }
    //    }
    //}
}
