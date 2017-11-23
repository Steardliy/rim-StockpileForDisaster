using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace StockpileForDisaster
{
    [StaticConstructorOnStartup]
    static class CopyPasteRestrictionSetting
    {
        private const int gizmoGroupKey = 525455533;

        private static IEntityFilter storedSetting;
        private static Texture2D copyGizmoIcon = ContentFinder<Texture2D>.Get("UI/GizmoIcon/CopyRestrictionSettings");
        private static Texture2D pasteGizmoIcon = ContentFinder<Texture2D>.Get("UI/GizmoIcon/PasteRestrictionSettings");

        public static IEnumerable<Gizmo> CopyPasteRestrictSettingsGizmo(IEntityFilter filter)
        {
            yield return CopyGizmo(filter);
            yield return PasteGizmo(filter);
        }
        public static void CopyFrom(IEntityFilter filter)
        {
            storedSetting = filter;
        }
        public static void PasteTo(IEntityFilter filter)
        {
            if (filter != null)
            {
                filter.CopyFrom(storedSetting);
            }
        }
        private static Gizmo CopyGizmo(IEntityFilter filter)
        {
            Command_Action copyGizmo = new Command_Action();
            copyGizmo.icon = copyGizmoIcon;
            copyGizmo.action = () => CopyPasteRestrictionSetting.CopyFrom(filter);
            copyGizmo.defaultIconColor = Color.white;
            copyGizmo.defaultLabel = "CopyRestrictionLabel".Translate();
            copyGizmo.defaultDesc = "CopyRestrictionDesc".Translate();
            copyGizmo.groupKey = gizmoGroupKey;

            return copyGizmo;
        }
        private static Gizmo PasteGizmo(IEntityFilter filter)
        {
            Command_Action pasteGizmo = new Command_Action();
            pasteGizmo.icon = pasteGizmoIcon;
            pasteGizmo.action = () => CopyPasteRestrictionSetting.PasteTo(filter);
            pasteGizmo.defaultIconColor = Color.white;
            pasteGizmo.defaultLabel = "PasteRestrictionLabel".Translate();
            pasteGizmo.defaultDesc = "PasteRestrictionDesc".Translate();
            pasteGizmo.groupKey = gizmoGroupKey + 1;

            return pasteGizmo;
        }
    }
}
