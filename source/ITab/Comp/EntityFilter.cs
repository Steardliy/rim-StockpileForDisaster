using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using System;

namespace StockpileForDisaster
{
    class EntityFilter : IEntityFilter, IExposable, IRescriction
    {
        private bool loaded = false;
        public bool IsLoaded
        {
            get
            {
                return loaded;
            }
        }
        private bool vaild = true;
        private HashSet<Thing> allowedEntityList = new HashSet<Thing>();
        private HashSet<SpecialEntityFilterDef> forbiddenSpFilterList = new HashSet<SpecialEntityFilterDef>();

        public ISlotGroupParent slotGroupParent;

        public virtual bool IsVaild()
        {
            return this.vaild;
        }
        public virtual void SetVaild(bool flag = true)
        {
            this.vaild = flag;
        }

        public virtual bool IsAllowed(Thing t)
        {
            return this.allowedEntityList.Contains(t);
        }
        public virtual bool IsAllowed(SpecialEntityFilterDef spDef)
        {
            return !this.forbiddenSpFilterList.Contains(spDef);
        }
        public virtual void SetAllow(Thing t, bool flag = true)
        {
            if (flag)
            {
                this.allowedEntityList.Add(t);
            }
            else
            {
                this.allowedEntityList.Remove(t);
            }
        }
        public virtual void SetAllow(SpecialEntityFilterDef spDef, bool flag = true)
        {
            if (!flag)
            {
                this.forbiddenSpFilterList.Add(spDef);
            }
            else
            {
                this.forbiddenSpFilterList.Remove(spDef);
            }
        }
        public virtual void SetAllowAll(EntityCategory root, bool flag = true)
        {
            if (flag)
            {
                foreach (Thing t in root.DescendantThings)
                {
                    this.allowedEntityList.Add(t);
                }
                this.forbiddenSpFilterList.Clear();
                
            }
            else
            {
                foreach (SpecialEntityFilterDef def in root.catDef.DescendantSpecialEntityFilterDefs)
                {
                    this.forbiddenSpFilterList.Add(def);
                }
                this.allowedEntityList.Clear();
                
            }
        }
        public virtual void UpdateForbiddenList(EntityCategory root)
        {
            this.allowedEntityList.RemoveWhere(thing => {
                return (thing == null) || thing.Destroyed;
            });
            this.forbiddenSpFilterList.RemoveWhere(spFilter => !root.catDef.DescendantSpecialEntityFilterDefs.Contains(spFilter));
        }

        public void ExposeData()
        {
            Scribe_Values.Look<bool>(ref this.vaild, "vaild", true);
            Scribe_Collections.Look<Thing>(ref this.allowedEntityList, false, "allowedEntityList", LookMode.Reference);
            Scribe_Collections.Look<SpecialEntityFilterDef>(ref this.forbiddenSpFilterList, "forbiddenSpFilterList", LookMode.Def);
            
            if (forbiddenSpFilterList == null)
                forbiddenSpFilterList = new HashSet<SpecialEntityFilterDef>();
            if (allowedEntityList == null)
                allowedEntityList = new HashSet<Thing>();

            this.loaded = true;
        }
        public ISlotGroupParent GetSlotGroupParent()
        {
            return this.slotGroupParent;
        }
        public IEnumerable<Thing> GetAllowedEntityList()
        {
            return this.allowedEntityList;
        }
        public IEnumerable<SpecialEntityFilterDef> GetforbiddenSpFilterList()
        {
            return this.forbiddenSpFilterList;
        }

        public void CopyFrom(IEntityFilter from)
        {
            if(from == null)
            {
                return;
            }
            this.vaild = from.IsVaild();
            this.allowedEntityList.Clear();
            foreach (var a in from.GetAllowedEntityList())
            {
                this.allowedEntityList.Add(a);
            }
            this.forbiddenSpFilterList.Clear();
            foreach (var a in from.GetforbiddenSpFilterList())
            {
                this.forbiddenSpFilterList.Add(a);
            }
        }
    }
}
