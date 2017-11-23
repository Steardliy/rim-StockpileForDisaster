using Verse;

namespace StockpileForDisaster
{
    public class SpecialThingFilterWorker_Male : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            Pawn pawn = t as Pawn;
            if(pawn != null && pawn.gender == Gender.Male)
            {
                return true;
            }
            return false;
        }
    }
}
