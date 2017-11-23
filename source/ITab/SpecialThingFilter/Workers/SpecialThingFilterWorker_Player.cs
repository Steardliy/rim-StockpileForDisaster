using RimWorld;
using Verse;

namespace StockpileForDisaster
{
    public class SpecialThingFilterWorker_Player : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            Pawn pawn = t as Pawn;
            if (pawn != null && (pawn.Faction == Faction.OfPlayer))
            {
                return true;
            }
            return false;
        }
    }
}