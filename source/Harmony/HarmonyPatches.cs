using Harmony;
using RimWorld;
using System.Reflection;
using Verse;
using Verse.AI;

namespace StockpileForDisaster.Harmony
{
    [StaticConstructorOnStartup]
    class HarmonyPatches
    {
        static HarmonyPatches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.steardliy.StockpileForDisaster");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

#if DEBUG
            Log.Message("StockpileForDisaster:Harmony is initialized");
#endif
        }
        [HarmonyPatch(typeof(Pawn_JobTracker))]
        [HarmonyPatch("DetermineNextJob")]
        static class DetermineNextJob_Fix
        {
            public static void Prefix(Pawn_JobTracker __instance, ThinkResult __result)
            {
                FieldInfo info = AccessTools.Field(typeof(Pawn_JobTracker), "pawn");
                Pawn actor = info.GetValue(__instance) as Pawn;
                if(actor.Faction != Faction.OfPlayer)
                {
                    return;
                }
#if DEBUG
                Log.Message(DebugLog.GetMethodName(2) + MethodBase.GetCurrentMethod().Name + " actor:" + actor.Name.ToStringShort);
#endif
                RestrictedEntityManager manager = actor.Map.GetComponent<RestrictedEntityManager>();
                manager.StartForbiddenSection(actor);
                
            }
            public static void Postfix(Pawn_JobTracker __instance, ThinkResult __result, ThinkTreeDef thinkTree)
            {
                FieldInfo info = AccessTools.Field(typeof(Pawn_JobTracker), "pawn");
                Pawn actor = info.GetValue(__instance) as Pawn;
                if (actor.Faction != Faction.OfPlayer)
                {
                    return;
                }

                RestrictedEntityManager manager = actor.Map.GetComponent<RestrictedEntityManager>();
                manager.EndForbiddenSection(actor);
            }
        }
    }
}
