using Verse;

namespace StockpileForDisaster
{
    public class SpecialThingFilterWorker_PackAnimal : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            Pawn pawn = t as Pawn;
            if (pawn != null && pawn.RaceProps.packAnimal)
            {
                return true;
            }
            return false;
        }
    }
}
