using HarmonyLib;
using RandomColors.utils;

namespace RandomColors.Patchs;

[HarmonyPatch(typeof(EnemyAI))]
public class PatchEnemyAi
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    private static void PatchStart(EnemyAI __instance)
    {
        if (!RandomColorsPlugin.instance.affectEnemyEntry.Value ||
            __instance.enemyType.enemyName.Contains("Locust Bees")) return;

        UtilsFunctions.ChangeGameObject(__instance.gameObject);
    }
}