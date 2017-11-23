using RimWorld;
using Verse;

namespace StockpileForDisaster
{
    public class SpecialThingFilterWorker_UnHaulable : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            Pawn pawn = t as Pawn;
            bool visble;
            if (pawn != null && pawn.training != null && !pawn.training.CanAssignToTrain(DefDatabase<TrainableDef>.GetNamed("Haul"), out visble).Accepted)
            {
                return true;
            }
            return false;
        }
    }
}