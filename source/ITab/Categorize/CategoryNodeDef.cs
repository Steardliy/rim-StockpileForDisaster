using System;
using System.Collections.Generic;
using Verse;

namespace StockpileForDisaster
{
    class CategoryNodeDef : Def
    {
        public List<ThingDef> childThingDefs = new List<ThingDef>();
        public List<SpecialEntityFilterDef> childSpecialEntityFilters = new List<SpecialEntityFilterDef>();

        protected List<SpecialThingFilterWorker> workerInt = null;

        public List<Type> workerClass = null;
        public List<SpecialThingFilterWorker> Worker
        {
            get
            {
                if (this.workerInt == null && workerClass != null)
                {
                    this.workerInt = new List<SpecialThingFilterWorker>();
                    foreach(var worker in this.workerClass)
                    {
                        this.workerInt.Add((SpecialThingFilterWorker)Activator.CreateInstance(worker));
                    }
                }
                return workerInt;
            }
        }
    }
}
