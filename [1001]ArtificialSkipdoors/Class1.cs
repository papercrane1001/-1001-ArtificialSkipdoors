using System;
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



using Prepatcher;

namespace _1001_ArtificialSkipdoors
{
    public class NVPEDoorBuilding : Building
    {
        //public NVPESkipDoor door;
        //public Pawn pawn;
        public Skipdoor pdoor;
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            //pawn = new Pawn();
            base.SpawnSetup(map, respawningAfterLoad);
            //Skipdoor Sdoor = (Skipdoor)ThingMaker.MakeThing(VPE_DefOf.VPE_Skipdoor, null);

            //door = (NVPESkipDoor)ThingMaker.MakeThing(NVPE_DefOf.NVPESkipDoorDef, null);
            //Find.WindowStack.Add(new Dialog_RenameDoorTeleporter(door));
            //GenSpawn.Spawn(door, Position + new IntVec3(1, 0, 0), map, WipeMode.Vanish);
            Log.Message("Ping1");

            pdoor = (Skipdoor)ThingMaker.MakeThing(VPE_DefOf.VPE_Skipdoor, null);
            if(pdoor == null) { Log.Message("Sdoor null"); }
            Log.Message("Ping2");
            Find.WindowStack.Add(new Dialog_RenameDoorTeleporter(pdoor));
            GenSpawn.Spawn(pdoor, Position + new IntVec3(-1, 0, 0), map, WipeMode.Vanish);
            Log.Message("Ping3");
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
}
