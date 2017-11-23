using RimWorld;
using Verse;

namespace StockpileForDisaster
{
    public class SpecialThingFilterWorker_Psychopath : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            Pawn pawn = t as Pawn;
            if (pawn != null && pawn.RaceProps.Humanlike && pawn.story.traits.HasTrait(TraitDefOf.Psychopath))
            {
                return true;
            }
            return false;
        }
    }
}
