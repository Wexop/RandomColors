using System.Linq;
using UnityEngine;

namespace RandomColors.utils;

public class UtilsFunctions
{
    public static void ChangeGameObject(GameObject gameObject)
    {
        var materials = gameObject.GetComponentsInChildren<Renderer>(true).ToList();
        materials.AddRange(gameObject.GetComponents<Renderer>());
        foreach (var material in materials)
            if (material != null && material.material != null)
                foreach (var m in material.materials)
                    m.color = GetRandomColor(material.material.color.a);

        if (!RandomColorsPlugin.instance.affectLightEntry.Value) return;
        var lights = gameObject.GetComponentsInChildren<Light>(true).ToList();
        lights.AddRange(gameObject.GetComponents<Light>());
        foreach (var light in lights)
            if (light != null && light.color != null)
                light.color = GetRandomColor(light.color.a);
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
}