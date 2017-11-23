using System;
using Verse;

namespace StockpileForDisaster
{
    class SpecialEntityFilterDef : Def
    {

        public bool defaultForbidden = false;
        public Type workerClass = null;

        private SpecialThingFilterWorker workerInt = null;
        public SpecialThingFilterWorker Worker
        {
            get
            {
                if (this.workerInt == null)
                {
                    this.workerInt = (SpecialThingFilterWorker)Activator.CreateInstance(this.workerClass);
                }
                return this.workerInt;
            }
        }
        public static SpecialEntityFilterDef Named(string defName)
        {
            return DefDatabase<SpecialEntityFilterDef>.GetNamed(defName, true);
        }
    }
}
