using HarmonyLib;
using RandomColors.utils;

namespace RandomColors.Patchs;

[HarmonyPatch(typeof(GrabbableObject))]
public class PatchGrabbableObject
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    private static void PatchStart(GrabbableObject __instance)
    {
        if (!RandomColorsPlugin.instance.affectItemEntry.Value) return;
        if (!RandomColorsPlugin.instance.affectFlashLight.Value &&
            (__instance.gameObject.name.Contains("Flashlight") ||
             __instance.gameObject.name.Contains("LaserPointer"))) return;

        UtilsFunctions.ChangeGameObject(__instance.gameObject);
    }
}