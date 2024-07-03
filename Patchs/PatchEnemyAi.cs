using System.Linq;
using HarmonyLib;
using UnityEngine;

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

        var materials = __instance.gameObject.GetComponentsInChildren<Renderer>().ToList();
        materials.Add(__instance.gameObject.GetComponent<Renderer>());
        foreach (var material in materials)
            if (material != null && material.material != null)
                material.material.color = RandomColorsPlugin.instance.GetRandomColor(material.material.color.a);
    }
}