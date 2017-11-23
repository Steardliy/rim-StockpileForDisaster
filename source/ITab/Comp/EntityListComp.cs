using RimWorld;
using System.Collections.Generic;
using Verse;

namespace StockpileForDisaster
{
    class EntityListComp : ThingComp
    {
        private EntityListCompDef propDef;
        public EntityFilter filter = new EntityFilter();
        public string label = "Undefine";
        public string description = "none.";

        public string LabelCap
        {
            get
            {
                return this.label.CapitalizeFirst();
            }
        }
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        private EntityCategory entityCategoryRootInt = null;
        public EntityCategory entityCategoryRoot
        {
            get
            {
                if(entityCategoryRootInt == null)
                {
                    this.CreateRootCategory();
                }
                return entityCategoryRootInt;
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var a in CopyPasteRestrictionSetting.CopyPasteRestrictSettingsGizmo(this.filter))
            {
                yield return a;
            }
        }

        public void UpdateThingsList()
        {
            EntityCategory root = entityCategoryRoot;
            
            foreach (var a in root.DescendantEntityCategoriesAndThis)
            {
                a.childThings = new List<Thing>();
                foreach (var b in a.catDef.childThingDefs)
                {
                    a.childThings.AddRange(Find.VisibleMap.listerThings.ThingsOfDef(b));
#if DEBUG
                    Log.Message(DebugLog.GetMethodName() + "Found Things.Count=" + a.childThings.Count);
#endif
                }
                if (a.catDef.Worker != null)
                {
                    foreach (var c in a.catDef.Worker)
                    {
                        a.childThings.RemoveAll(thing => !c.Matches(thing));
                    }
                }
            }
            this.filter.UpdateForbiddenList(root);
        }

        private void CreateRootCategory()
        {
            this.entityCategoryRootInt = new EntityCategory(propDef.categoryRootDef);
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            this.propDef = this.props as EntityListCompDef;
            if (this.propDef == null)
            {
                Log.Error(DebugLog.GetMethodName() + "EntityListCompDef is NULL");
            }

            this.filter.slotGroupParent = this.parent as ISlotGroupParent;

            parent.Map.GetComponent<RestrictedEntityManager>().Register(this.filter);

            if(propDef.listType != null)
            {
                this.label = propDef.listType.label;
                this.description = propDef.listType.description;
            }
            if (!filter.IsLoaded)
            {
                RestrictedEntityManager.DefaultPasteTo(filter);
            }
            base.PostSpawnSetup(respawningAfterLoad);
        }
        public override void PostDeSpawn(Map map)
        {
            map.GetComponent<RestrictedEntityManager>().UnRegister(this.filter);
            base.PostDeSpawn(map);
        }

        public override void PostExposeData()
        {
            filter.ExposeData();
            base.PostExposeData();
        }
    }
}
