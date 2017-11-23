using System.Collections.Generic;
using System.Linq;
using Verse;

namespace StockpileForDisaster
{
    class EntityCategoryDef : CategoryNodeDef
    {
        public EntityCategoryDef parent = null;
        protected List<EntityCategoryDef> childCategoryDefs = new List<EntityCategoryDef>();

        public IEnumerable<ThingDef> DescendantThingDefs
        {
            get
            {
                foreach (var a in childThingDefs)
                {
                    yield return a;
                }
                foreach (var a in childCategoryDefs)
                {
                    foreach (var b in a.DescendantThingDefs)
                    {
                        yield return b;
                    }
                }
            }
        }
        public IEnumerable<SpecialEntityFilterDef> DescendantSpecialEntityFilterDefs
        {
            get
            {
                foreach (var a in childSpecialEntityFilters)
                {
                    yield return a;
                }
                foreach (var a in childCategoryDefs)
                {
                    foreach (var b in a.DescendantSpecialEntityFilterDefs)
                    {
                        yield return b;
                    }
                }
            }
        }

        public void AddChildCategoryDef(EntityCategoryDef other)
        {
            childCategoryDefs.Add(other);
        }
        public void RemoveChildCategoryDef(EntityCategoryDef other)
        {
            childCategoryDefs.Remove(other);
        }
        public IEnumerable<EntityCategoryDef> GetChildCategoryDef()
        {
            foreach (var a in childCategoryDefs)
            {
                yield return a;
            }
        }

        public EntityCategoryDef() { }
        public EntityCategoryDef(CategoryNodeDef nodeDef)
        {
            if (nodeDef != null)
            {
                this.childThingDefs = nodeDef.childThingDefs;
                this.childSpecialEntityFilters = nodeDef.childSpecialEntityFilters;
                this.workerClass = nodeDef.workerClass;
                this.label = nodeDef.label;
                this.description = nodeDef.description;
            }
        }

        public override void PostLoad()
        {
            LongEventHandler.ExecuteWhenFinished(() =>
            {
                if (parent != null)
                {
                    parent.AddChildCategoryDef(this);
                }
            });
            base.PostLoad();
        }
    }
}
