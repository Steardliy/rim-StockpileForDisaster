using Verse;

namespace StockpileForDisaster
{
    public class SpecialThingFilterWorker_Colonist : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            Pawn pawn = t as Pawn;
            if (pawn != null && pawn.IsColonist)
            {
                return true;
            }
            return false;
        }
    }
}