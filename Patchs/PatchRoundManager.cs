using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RandomColors.utils;
using UnityEngine;

namespace RandomColors.Patchs;

[HarmonyPatch(typeof(RoundManager))]
public class PatchRoundManager
{
    [HarmonyPatch(nameof(RoundManager.FinishedGeneratingLevelServerRpc))]
    [HarmonyPostfix]
    private static void PatchLoadLevel(RoundManager __instance)
    {
        if (!RandomColorsPlugin.instance.affectLightEntry.Value) return;

        //SUNLIGHT AFFECT
        var noAffectedId = new List<int>();

        if (!RandomColorsPlugin.instance.affectSunLightEntry.Value)
        {
            var animatedSun = Object.FindObjectOfType<animatedSun>();
            if (animatedSun != null)
            {
                noAffectedId.Add(animatedSun.directLight.gameObject.GetInstanceID());
                noAffectedId.Add(animatedSun.indirectLight.gameObject.GetInstanceID());
            }
        }

        //EVERY LIGHTS AFFECT

        var lights = Object.FindObjectsOfType<Light>(true).ToList();

        foreach (var light in lights)
        {
            if (noAffectedId.Contains(light.gameObject.GetInstanceID()) || light.name == "NightVision") return;
            light.color = UtilsFunctions.GetRandomColor(light.color.a);
        }
    }
}