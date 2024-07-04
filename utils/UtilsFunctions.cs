using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RandomColors.utils;

public class UtilsFunctions
{
    public static void ChangeGameObject(GameObject gameObject)
    {
        Color? emessiveColorFound = null;

        var renderers = gameObject.GetComponentsInChildren<Renderer>(true).ToList();
        renderers.AddRange(gameObject.GetComponents<Renderer>());
        foreach (var renderer in renderers)
            if (renderer != null && renderer.material != null)
                foreach (var m in renderer.materials)
                {
                    var color = GetRandomColor(m.color.a);
                    m.color = color;
                    try
                    {
                        var emissiveColor = m.GetColor("_EmissiveColor");
                        if (!colorDark(emissiveColor))
                        {
                            Debug.Log($"{emissiveColor} IS DARK");
                            if (!emessiveColorFound.HasValue)
                                emessiveColorFound = color;
                            else
                                color = emessiveColorFound.Value;

                            color.a = emissiveColor.a;
                            m.SetColor("_EmissiveColor", color);
                        }
                    }
                    catch
                    {
                    }
                }


        if (!RandomColorsPlugin.instance.affectLightEntry.Value) return;
        var lights = gameObject.GetComponentsInChildren<Light>(true).ToList();
        lights.AddRange(gameObject.GetComponents<Light>());
        foreach (var light in lights)
            if (light != null && light.color != null)
            {
                if (emessiveColorFound.HasValue)
                    light.color = emessiveColorFound.Value;
                else
                    light.color = GetRandomColor(light.color.a);

                Debug.Log($"ITEM {gameObject.name} HAVE LIGHT {light.color}");
            }

        var flashlightItem = gameObject.GetComponent<FlashlightItem>();
        if (flashlightItem != null && emessiveColorFound.HasValue)
        {
            var material = new Material(flashlightItem.bulbLight);
            material.color = emessiveColorFound.Value;
            flashlightItem.bulbDark = material;
            flashlightItem.bulbLight = material;
        }
    }

    public static float RandomZeroToOne()
    {
        return Random.Range(0f, 1f);
    }

    public static Color GetRandomColor(float initialAlpha = 1f)
    {
        return new Color(RandomZeroToOne(),
            RandomZeroToOne(), RandomZeroToOne(), initialAlpha);
    }

    public static bool colorDark(Color color)
    {
        Debug.Log($"COLOR CATCHED {color.r} {color.g} {color.b}");
        if (color.r < 3f && color.g < 3f && color.b < 3f)
        {
            Debug.Log(true);
            return true;
        }

        return false;
    }
}