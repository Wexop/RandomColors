﻿using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RandomColors.utils;

public class UtilsFunctions
{
    public static void ChangeGameObject(GameObject gameObject)
    {
        if (!CanChangeColor()) return;

        Color? emessiveColorFound = null;
        Material materialEmissive = null;

        var renderers = gameObject.GetComponentsInChildren<Renderer>(true).ToList();
        renderers.AddRange(gameObject.GetComponents<Renderer>());
        foreach (var renderer in renderers)
            if (renderer != null && renderer.material != null && !renderer.gameObject.name.Contains("MapDot"))
                foreach (var m in renderer.materials)
                {
                    if (!m.HasColor("_Color")) continue;
                    var color = GetRandomColor(m.color.a);

                    if (gameObject.name.Contains("RadMech"))
                    {
                        if (emessiveColorFound.HasValue) color = emessiveColorFound.Value;
                        else
                            emessiveColorFound = color;
                    }


                    try
                    {
                        m.color = color;
                        var emissiveColor = m.GetColor("_EmissiveColor");
                        if (emissiveColor != null && !colorDark(emissiveColor))
                        {
                            if (!emessiveColorFound.HasValue)
                                emessiveColorFound = color;
                            else
                                color = emessiveColorFound.Value;

                            color.a = emissiveColor.a;
                            m.SetColor("_EmissiveColor", color);
                            RandomColorsPlugin.instance.SaveObjectColor(renderer.gameObject);
                            if (materialEmissive == null) materialEmissive = new Material(m);
                        }
                        else if (RandomColorsPlugin.instance.themeChoiceEntry.Value == "neon" && emissiveColor != null)
                        {
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
                {
                    light.color = emessiveColorFound.Value;
                    RandomColorsPlugin.instance.SaveObjectColor(light.gameObject);
                }
                else
                {
                    light.color = GetRandomColor(light.color.a);
                }
            }

        var flashlightItem = gameObject.GetComponent<FlashlightItem>();
        if (flashlightItem != null && emessiveColorFound.HasValue && materialEmissive != null)
        {
            RandomColorsPlugin.instance.SaveObjectColor(flashlightItem.gameObject);
            flashlightItem.bulbLight = materialEmissive;
        }
    }

    public static float RandomZeroToOne()
    {
        return Random.Range(0f, 1f);
    }

    public static float RandomLightColorFloat()
    {
        return Random.Range(0.75f, 1f);
    }

    public static float RandomDarkColorFloat()
    {
        return Random.Range(0f, 0.25f);
    }

    public static Color GetRandomColor(float initialAlpha = 1f)
    {
        var theme = RandomColorsPlugin.instance.themeChoiceEntry.Value;

        var baseColor = new Color(RandomZeroToOne(),
            RandomZeroToOne(), RandomZeroToOne(), initialAlpha);

        if (theme == "colorful") baseColor[Random.Range(0, 4)] = RandomLightColorFloat();
        if (theme == "dark")
            baseColor = new Color(RandomDarkColorFloat(), RandomDarkColorFloat(), RandomDarkColorFloat());

        return baseColor;
    }

    public static bool colorDark(Color color)
    {
        //Debug.Log($"COLOR CATCHED {color.r} {color.g} {color.b}");
        if (color.r < 2f && color.g < 2f && color.b < 2f) return true;

        return false;
    }

    public static bool CanChangeColor()
    {
        var random = Random.Range(0f, 100f);
        return RandomColorsPlugin.instance.chanceRandomColorEntry.Value > random;
    }
}