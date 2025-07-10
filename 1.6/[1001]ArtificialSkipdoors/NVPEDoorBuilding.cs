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
        public Skipdoor pdoor;
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            pdoor = (Skipdoor)ThingMaker.MakeThing(VPE_DefOf.VPE_Skipdoor, null);
            if(pdoor == null) { Log.Message("Sdoor null"); }
            //Find.WindowStack.Add(new Dialog_RenameDoorTeleporter(pdoor));
            GenSpawn.Spawn(pdoor, Position + new IntVec3(0, 0, -1).RotatedBy(Rotation), map, WipeMode.Vanish);
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);
            pdoor.Destroy();
        }
    }


}
