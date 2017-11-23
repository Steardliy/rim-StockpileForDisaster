using System.Collections.Generic;
using Verse;

namespace StockpileForDisaster
{
    interface IEntityFilter
    {
        bool IsVaild();
        void SetVaild(bool flag = true);

        bool IsAllowed(Thing t);
        bool IsAllowed(SpecialEntityFilterDef t);
        void SetAllow(Thing t, bool flag = true);
        void SetAllow(SpecialEntityFilterDef t, bool flag = true);
        void SetAllowAll(EntityCategory root, bool flag = true);

        IEnumerable<Thing> GetAllowedEntityList();
        IEnumerable<SpecialEntityFilterDef> GetforbiddenSpFilterList();
        void CopyFrom(IEntityFilter from);
    }
}
