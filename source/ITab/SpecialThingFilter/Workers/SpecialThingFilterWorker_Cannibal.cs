using RimWorld;
using Verse;

namespace StockpileForDisaster
{
    public class SpecialThingFilterWorker_Cannibal : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            Pawn pawn = t as Pawn;
            if (pawn != null && pawn.RaceProps.Humanlike && pawn.story.traits.HasTrait(TraitDefOf.Cannibal))
            {
                return true;
            }
            return false;
        }
    }
}