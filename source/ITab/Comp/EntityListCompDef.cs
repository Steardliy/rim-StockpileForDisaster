using System.Collections.Generic;
using System.Linq;
using Verse;

namespace StockpileForDisaster
{
    class EntityListCompDef: CompProperties
    {
        public EntityCategoryPrisetDef categoryPriset = null;
        private CategoryNodeDef nodeDef = null;
        public List<EntityCategoryDef> canDisplayCategorys = null;
        public EntityListCompTypeDef listType = null;

        private EntityCategoryDef categoryRootDefInt = null;
        
        public EntityCategoryDef categoryRootDef
        {
            get
            {
#if DEBUG
                Log.Message(DebugLog.GetMethodName());
#endif
                if (this.categoryRootDefInt == null)
                {
                    this.CreateRootCategoryDef();
                }
                return this.categoryRootDefInt;
            }
        }


        private void CreateRootCategoryDef()
        {
            EntityCategoryDef result = new EntityCategoryDef(nodeDef);

            if (this.categoryPriset != null && !this.categoryPriset.categories.NullOrEmpty())
            {
                foreach(var a in this.categoryPriset.categories)
                {
                    result.AddChildCategoryDef(a);
                }
            } else if (!this.canDisplayCategorys.NullOrEmpty())
            {
                foreach (var a in this.canDisplayCategorys)
                {
                    result.AddChildCategoryDef(a);
                }
            }
            OtherThingDefs(result);

            this.categoryRootDefInt = result;
        }
        private void OtherThingDefs(EntityCategoryDef preDef)
        {
            IEnumerable<ThingDef> otherThingDefs = DefDatabase<ThingDef>.AllDefs.Where(thing => thing.category == ThingCategory.Pawn);
            
            foreach (var a in otherThingDefs.Except(preDef.DescendantThingDefs))
            {
                preDef.childThingDefs.Add(a);
            }
#if DEBUG
            if (otherThingDefs != null)
            {
                Log.Message(DebugLog.GetMethodName() + "Number of undefined objects=" + otherThingDefs.Count());
            }
#endif
        }

        public EntityListCompDef()
        {
            LongEventHandler.ExecuteWhenFinished(this.CreateRootCategoryDef);
        }
    }
}
