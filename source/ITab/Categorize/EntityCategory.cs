using System.Collections.Generic;
using System.Linq;
using Verse;

namespace StockpileForDisaster
{
    class EntityCategory : TreeNode
    {
        public EntityCategoryDef catDef;

        public List<Thing> childThings = new List<Thing>();
        public List<EntityCategory> childCategories = new List<EntityCategory>();
        public string LabelCap
        {
            get
            {
                return this.catDef.LabelCap;
            }
        }
        public string Description
        {
            get
            {
                return this.catDef.description;
            }
        }

        private EntityCategory() { }
        public EntityCategory(EntityCategoryDef def)
        {
            catDef = def;
            foreach(var a in def.GetChildCategoryDef())
            {
                this.childCategories.Add(new EntityCategory(a));
            }
            if (this.catDef.label.NullOrEmpty() && this.catDef.childThingDefs != null)
            {
                ThingDef tmpDef = this.catDef.childThingDefs.First();
                if (tmpDef != null)
                {
                    this.catDef.label = tmpDef.label;
                }
            }
        }
        public IEnumerable<EntityCategory> DescendantEntityCategoriesAndThis
        {
            get
            {
                yield return this;
                foreach(var a in DescendantEntityCategories)
                {
                    yield return a;
                }
            }
        }
        public IEnumerable<EntityCategory> DescendantEntityCategories
        {
            get
            {
                foreach (var a in childCategories)
                {
                    yield return a;
                    foreach (var b in a.DescendantEntityCategories)
                    {
                        yield return b;
                    }
                }
            }
        }
        public IEnumerable<Thing> DescendantThings
        {
            get
            {
                foreach (var a in childThings)
                {
                    yield return a;
                }
                foreach (var a in childCategories)
                {
                    foreach (var b in a.DescendantThings)
                    {
                        yield return b;
                    }
                }
            }
        }
    }
}
