using Verse;

namespace SaM;

public class WallSign_PlaceWorker : PlaceWorker
{
    public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map,
        Thing thingToIgnore = null, Thing thing = null)

    {
        var c = loc;

        var support = c.GetEdifice(map);
        if (support?.def?.graphicData == null ||
            (support.def.graphicData.linkFlags & (LinkFlags.Rock | LinkFlags.Wall)) == 0)
        {
            return "SaM_Placeworker_OnSupport".Translate();
        }

        c = loc + rot.Opposite.FacingCell;
        if (!c.Walkable(map))
        {
            return "SaM_Placeworker_Walkable".Translate();
        }

        var currentBuildings = loc.GetThingList(map);
        foreach (var building in currentBuildings)
        {
            if (building?.def?.defName != "SaM_WallSign")
            {
                continue;
            }

            return "SaM_Placeworker_Existing".Translate();
        }

        return AcceptanceReport.WasAccepted;
    }
}