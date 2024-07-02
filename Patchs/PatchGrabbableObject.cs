﻿using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace RandomColors.Patchs;

[HarmonyPatch(typeof(GrabbableObject))]
public class PatchGrabbableObject
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    private static void PatchStart(GrabbableObject __instance)
    {
        if (RandomColorsPlugin.instance.affectItemEntry.Value)
        {
            var materials = __instance.gameObject.GetComponentsInChildren<Renderer>().ToList();
            materials.Add(__instance.gameObject.GetComponent<Renderer>());
            foreach (var material in materials)
                if (material != null && material.material != null)
                    material.material.color = RandomColorsPlugin.instance.GetRandomColor();
        }
    }
}