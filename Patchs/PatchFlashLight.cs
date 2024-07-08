using HarmonyLib;
using UnityEngine;

namespace RandomColors.Patchs;

[HarmonyPatch(typeof(FlashlightItem))]
public class PatchFlashLight
{
    [HarmonyPatch(nameof(FlashlightItem.PocketItem))]
    [HarmonyPostfix]
    private static void PatchTurnOnFlashLight(FlashlightItem __instance)
    {
        if (!__instance.isBeingUsed || !RandomColorsPlugin.instance.affectFlashLight.Value) return;

        var lightColor = __instance.gameObject.GetComponentInChildren<Light>()?.color;

        if (!lightColor.HasValue) return;

        var helmetLight =
            GetChildGameObject(GameNetworkManager.Instance.localPlayerController.gameObject, "HelmetLights");

        var lights = helmetLight.GetComponentsInChildren<Light>(true);

        foreach (var l in lights) l.color = lightColor.Value;
    }

    public static GameObject GetChildGameObject(GameObject fromGameObject, string withName)
    {
        var ts = fromGameObject.GetComponentsInChildren<Transform>(true);
        foreach (var t in ts)
            if (t.gameObject.name == withName)
                return t.gameObject;
        return null;
    }
}