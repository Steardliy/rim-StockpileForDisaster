using UnityEngine;
using Verse;

namespace StockpileForDisaster
{
    class TabRestrictUI
    {
        private static float[] viewHeight = { 0f, 0f };
        
        public static void DrawEntityListWindow(IEntityFilter filter, EntityCategory root, string label, string description, Rect baseRect, ref Vector2 scrollPosition, int count)
        {
            Widgets.DrawMenuSection(baseRect);

            TabRestrictUI.DrawDefaultSettingButton(filter, ref baseRect);
            TabRestrictUI.DrawHeadingCheckBox(filter, label, description, ref baseRect);

            Text.Font = GameFont.Tiny;
            
            float num = baseRect.width - 2f;
            Rect buttonRect1 = new Rect(baseRect.x + 1f, baseRect.y + 1f, num / 2f, 24f);
            if (Widgets.ButtonText(buttonRect1, "ClearAll".Translate(), true, false, true))
            {
                filter.SetAllowAll(root, false);
            }
            Rect buttonRect2 = new Rect(buttonRect1.xMax + 1f, buttonRect1.y, baseRect.xMax - 1f - (buttonRect1.xMax + 1f), 24f);
            if (Widgets.ButtonText(buttonRect2, "AllowAll".Translate(), true, false, true))
            {
                filter.SetAllowAll(root);
            }
            baseRect.yMin = buttonRect1.yMax;

            Text.Font = GameFont.Small;

            Rect viewRect = new Rect(0f, 0f, baseRect.width - 16f, TabRestrictUI.viewHeight[count]);
            Widgets.BeginScrollView(baseRect, ref scrollPosition, viewRect, true);
            float num2 = 2f;
            float num3 = num2;
            Rect listRect = new Rect(0f, num2, viewRect.width, 9999f);

            Listing_EntityListUI entityListUI = new Listing_EntityListUI(filter);
            entityListUI.Begin(listRect);
            entityListUI.CreateCheckBoxUI(root, 8, 0);
            entityListUI.End();

            if (Event.current.type == EventType.Layout)
            {
                TabRestrictUI.viewHeight[count] = num3 + entityListUI.CurHeight + 90f;
            }
            Widgets.EndScrollView();
        }
        private static void DrawDefaultSettingButton(IEntityFilter filter, ref Rect baseRect)
        {
            Text.Font = GameFont.Tiny;
            float btHeight = 24f;
            Rect buttonRect1 = new Rect(baseRect.x, baseRect.y - btHeight - 2f, 140f , 24f);
            if(Widgets.ButtonText(buttonRect1, "DefaultEntityListSettingsButton".Translate(), true, false, true))
            {
                RestrictedEntityManager.DefaultCopyFrom(filter);
            }
        }

        private static void DrawHeadingCheckBox(IEntityFilter filter, string label, string description, ref Rect baseRect)
        {
            Text.Font = GameFont.Medium;
            Rect rect = new Rect(baseRect.x, baseRect.y, baseRect.width, Text.LineHeight);
            Widgets.DrawHighlightIfMouseover(rect);
            if (!description.NullOrEmpty())
            {
                if (Mouse.IsOver(rect))
                {
                    GUI.DrawTexture(rect, TexUI.HighlightTex);
                }
                TooltipHandler.TipRegion(rect, description);
            }
            Text.Anchor = TextAnchor.MiddleCenter;
            
            Widgets.Label(rect, label);
            bool flag = filter.IsVaild();
            bool flag2 = flag;
            Widgets.Checkbox(new Vector2(baseRect.x + rect.width - 26f, baseRect.y + 4f), ref flag);
            if (flag != flag2)
            {
                filter.SetVaild(flag);
            }
            baseRect.y += rect.height;
            Text.Anchor = TextAnchor.UpperLeft;
        }
    }
}
