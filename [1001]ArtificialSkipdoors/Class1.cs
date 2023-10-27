using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using RimWorld;
using VFECore;
using VanillaPsycastsExpanded;
using VanillaPsycastsExpanded.Skipmaster;

namespace _1001_ArtificialSkipdoors
{
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
}
