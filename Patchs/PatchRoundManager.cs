using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace RandomColors.Patchs;

[HarmonyPatch(typeof(RoundManager))]
public class PatchRoundManager
{
    [HarmonyPatch(nameof(RoundManager.FinishedGeneratingLevelServerRpc))]
    [HarmonyPostfix]
    private static void PatchLoadLevel(RoundManager __instance)
    {
        if (RandomColorsPlugin.instance.affectLightEntry.Value)
        {
            var lights = Object.FindObjectsOfType<Light>().ToList();

            foreach (var light in lights)
                light.color = RandomColorsPlugin.instance.GetRandomColor(light.color.a);
        }
    }
}