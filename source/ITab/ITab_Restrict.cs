using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace StockpileForDisaster
{
    class ITab_Restrict : ITab
    {
        protected static ITab_RestrictDef tabDef;
        protected static ThingWithComps preSelObj;
        private Vector2[] scrollPosition = { default(Vector2), default(Vector2) };

        public ITab_Restrict()
        {
            LongEventHandler.ExecuteWhenFinished(() =>
            {
                tabDef = DefDatabase<ITab_RestrictDef>.GetNamed("tabSettings");
                this.size = new Vector2(tabDef.areaSize.x, tabDef.areaSize.y);
                this.labelKey = "ITabRestrict".Translate();
                this.tutorTag = "Restrict";
            });
        }
        private IEnumerable<EntityListComp> SelectedEntityList
        {
            get
            {
                ThingWithComps selObj = base.SelObject as ThingWithComps;
                if (selObj == null)
                {
                    Log.Error(DebugLog.GetMethodName() + "This Class does not inheriting ThingWithComps Class");
                    return null;
                }
                if(preSelObj != selObj)
                {
                    foreach (var comp in selObj.GetComps<EntityListComp>())
                    {
                        comp.UpdateThingsList();
                    }
                }
                preSelObj = selObj;
                return selObj.GetComps<EntityListComp>();
            }
        }
        protected override void FillTab()
        {
            if (tabDef == null)
            {
                Log.Error(DebugLog.GetMethodName() + "tabDef is null");
                return;
            }

            Rect tabRect = new Rect(0f, 0f, ITab_Restrict.tabDef.areaSize.x, ITab_Restrict.tabDef.areaSize.y).ContractedBy(10f);
            GUI.BeginGroup(tabRect);
            Text.Font = GameFont.Small;

            Vector2 curPos = new Vector2(0f, ITab_Restrict.tabDef.topMargine);
            float listWidth = (tabRect.width - ITab_Restrict.tabDef.centerMargine) / tabDef.listNumber;
            int count = 0;
            foreach (var comp in SelectedEntityList)
            {
                Rect baseRect = new Rect(curPos.x, curPos.y, listWidth, tabRect.height - ITab_Restrict.tabDef.topMargine);
                TabRestrictUI.DrawEntityListWindow(comp.filter, comp.entityCategoryRoot, comp.LabelCap, comp.Description, baseRect, ref this.scrollPosition[count], count);
                curPos.x += (listWidth + ITab_Restrict.tabDef.centerMargine);
                count++;
            }
            GUI.EndGroup();
        }
        public override void OnOpen()
        {
            foreach (var comp in SelectedEntityList)
            {
                comp.UpdateThingsList();
            }
            base.OnOpen();
        }
    }
}
