using RimWorld;
using System.Collections.Generic;
using Verse;

namespace StockpileForDisaster
{
    interface IRescriction
    {
        bool IsVaild();

        ISlotGroupParent GetSlotGroupParent();
        IEnumerable<Thing> GetAllowedEntityList();
        IEnumerable<SpecialEntityFilterDef> GetforbiddenSpFilterList();
    }
}
