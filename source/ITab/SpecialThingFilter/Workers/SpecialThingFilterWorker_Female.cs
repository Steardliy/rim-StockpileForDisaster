using Verse;

namespace StockpileForDisaster
{
    public class SpecialThingFilterWorker_Female : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            Pawn pawn = t as Pawn;
            if (pawn != null && pawn.gender == Gender.Female)
            {
                return true;
            }
            return false;
        }
    }
}
