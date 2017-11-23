using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace StockpileForDisaster
{
    class RestrictedEntityManager : MapComponent
    {
        private const int errorKey = 525455533;

        private bool sectionFlag = true;
        private HashSet<IRescriction> registeredStorageList = new HashSet<IRescriction>();
        private Dictionary<Thing, bool> tmpList;

        private static EntityFilter defaultSettings = new EntityFilter();
        
        public static void DefaultCopyFrom(IEntityFilter filter)
        {
            RestrictedEntityManager.defaultSettings.CopyFrom(filter);
        }
        public static void DefaultPasteTo(IEntityFilter filter)
        {
            if (filter != null)
            {
                filter.CopyFrom(RestrictedEntityManager.defaultSettings);
            }
        }

        public RestrictedEntityManager(Map map) : base(map) { }

        public void StartForbiddenSection(Pawn actor)
        {
#if DEBUG
            Log.Message("Registered objects=" + registeredStorageList.Count);
#endif
            if (!this.sectionFlag)
            {
                Log.ErrorOnce(DebugLog.GetMethodName() + "Section is not closed", errorKey);
                return;
            }
            this.sectionFlag = false;

            tmpList = new Dictionary<Thing, bool>();
            foreach(var a in this.MatchThings(actor))
            {
                tmpList.Add(a, a.IsForbidden(Faction.OfPlayer));
                a.SetForbidden(true, false);
            }
#if DEBUG
            Log.Message("Number of forbidden Things=" + tmpList.Count);
#endif
        }
        public void EndForbiddenSection(Pawn actor)
        {
            if (this.sectionFlag)
            {
                Log.ErrorOnce(DebugLog.GetMethodName() + "Section is not started", errorKey + 1);
                return;
            }
            this.sectionFlag = true;

            foreach(var a in tmpList)
            {
                a.Key.SetForbidden(a.Value, false);
            }
#if DEBUG
            Log.Message("Number of allowed Things=" + tmpList.Count);
#endif
        }

        public void Register(IRescriction target)
        {
            this.registeredStorageList.Add(target);
        }
        public void UnRegister(IRescriction target)
        {
            this.registeredStorageList.Remove(target);
        }

        protected virtual IEnumerable<Thing> MatchThings(Pawn actor)
        {   foreach (var a in registeredStorageList)
            {
                bool spForbidFlag = false;
                foreach (var b in a.GetforbiddenSpFilterList())
                {
                    if (b.Worker.Matches(actor))
                    {
                        spForbidFlag = true;
                    }
                }
                if (!a.IsVaild() && (!a.GetAllowedEntityList().Contains(actor) || spForbidFlag))
                {
                    foreach (var b in a.GetSlotGroupParent().GetSlotGroup().HeldThings)
                    {
                        yield return b;
                    }
                }
            }
        }
        public override void ExposeData()
        {
            defaultSettings.ExposeData();
            base.ExposeData();
        }
    }
}
