using HarmonyLib;
using RandomColors.utils;

namespace RandomColors.Patchs;

[HarmonyPatch(typeof(VehicleController))]
public class PatchVehiculeControler
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    private static void PatchStart(VehicleController __instance)
    {
        if (!RandomColorsPlugin.instance.AffectCruiserEntry.Value) return;

        UtilsFunctions.ChangeGameObject(__instance.gameObject);
    }
}