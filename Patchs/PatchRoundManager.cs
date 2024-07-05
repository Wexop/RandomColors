using System.Collections.Generic;
using HarmonyLib;
using RandomColors.utils;
using UnityEngine;
using Enumerable = System.Linq.Enumerable;

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

        var lights = Enumerable.ToList(Object.FindObjectsOfType<Light>(true));

        foreach (var light in lights)
        {
            if (noAffectedId.Contains(light.gameObject.GetInstanceID()) || light.name == "NightVision" ||
                RandomColorsPlugin.instance.saveObjectColorList.Contains(light.gameObject.GetInstanceID())) return;
            light.color = UtilsFunctions.GetRandomColor(light.color.a);
        }
    }
}