using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;
using RandomColors.Patchs;
using UnityEngine;

namespace RandomColors;

[BepInPlugin(GUID, NAME, VERSION)]
public class RandomColorsPlugin : BaseUnityPlugin
{
    private const string GUID = "wexop.randomcolors";
    private const string NAME = "RandomColors";
    private const string VERSION = "1.0.4";

    public static RandomColorsPlugin instance;

    public List<int> saveObjectColorList = new();

    public string[] themes =
    {
        "default",
        "colorful",
        "dark",
        "neon"
    };

    public ConfigEntry<bool> AffectCruiserEntry;
    public ConfigEntry<bool> affectEnemyEntry;
    public ConfigEntry<bool> affectFlashLight;
    public ConfigEntry<bool> affectItemEntry;
    public ConfigEntry<bool> affectLightEntry;
    public ConfigEntry<bool> affectSunLightEntry;
    public ConfigEntry<float> chanceRandomColorEntry;
    public ConfigEntry<string> themeChoiceEntry;

    private void Awake()
    {
        instance = this;

        Logger.LogInfo("RandomColors starting....");

        chanceRandomColorEntry = Config.Bind("General", "ChanceRandomColor", 100f,
            "Chance for anything that this mod affect to have random color. No need to restart the game :)");
        CreateFloatConfig(chanceRandomColorEntry);

        affectSunLightEntry = Config.Bind("General", "AffectSunLight", false,
            "Sun light have a random color every day. No need to restart the game :)");
        CreateBoolConfig(affectSunLightEntry);

        affectLightEntry = Config.Bind("General", "AffectLights", true,
            "Every lights in the game have a random color every day. No need to restart the game :)");
        CreateBoolConfig(affectLightEntry);

        affectFlashLight = Config.Bind("General", "AffectFlashLight", true,
            "Every flashlights object have a random color on spawn. No need to restart the game :)");
        CreateBoolConfig(affectFlashLight);

        affectItemEntry = Config.Bind("General", "AffectItems", true,
            "Every grabbable items in the game have a random color on spawn. No need to restart the game :)");
        CreateBoolConfig(affectItemEntry);

        affectEnemyEntry = Config.Bind("General", "AffectEnemies", true,
            "Every monsters in the game have a random color on spawn. No need to restart the game :)");
        CreateBoolConfig(affectEnemyEntry);

        AffectCruiserEntry = Config.Bind("General", "AffectCruiser", true,
            "Every cruiser in the game have a random color on spawn. No need to restart the game :)");
        CreateBoolConfig(AffectCruiserEntry);

        themeChoiceEntry = Config.Bind("General", "Theme", "default",
            "Choose your theme ! default : any rgb color, colorful : light colors, dark : dark colors, neon : affect emissive texture color. No need to restart the game :)");
        CreateDropdownConfig(themeChoiceEntry, themes);

        Harmony.CreateAndPatchAll(typeof(PatchRoundManager));
        Harmony.CreateAndPatchAll(typeof(PatchGrabbableObject));
        Harmony.CreateAndPatchAll(typeof(PatchEnemyAi));
        Harmony.CreateAndPatchAll(typeof(PatchFlashLight));
        try
        {
            Harmony.CreateAndPatchAll(typeof(PatchVehiculeControler));
        }
        catch
        {
            Logger.LogWarning("COULD NOT LOAD VEHICULE CONTROLLER, THIS CAN HAPPEN IF YOU ARE ON VERSION < 55");
        }

        Logger.LogInfo("RandomColors is patched!");
    }

    private void CreateBoolConfig(ConfigEntry<bool> configEntry)
    {
        var exampleSlider = new BoolCheckBoxConfigItem(configEntry, new BoolCheckBoxOptions
        {
            RequiresRestart = false
        });
        LethalConfigManager.AddConfigItem(exampleSlider);
    }

    private void CreateFloatConfig(ConfigEntry<float> configEntry, float min = 0f, float max = 100f)
    {
        var exampleSlider = new FloatSliderConfigItem(configEntry, new FloatSliderOptions
        {
            RequiresRestart = false,
            Min = min,
            Max = max
        });
        LethalConfigManager.AddConfigItem(exampleSlider);
    }

    private void CreateDropdownConfig(ConfigEntry<string> configEntry, string[] values)
    {
        var exampleSlider = new TextDropDownConfigItem(configEntry, new TextDropDownOptions
        {
            RequiresRestart = false,
            Values = values
        });
        LethalConfigManager.AddConfigItem(exampleSlider);
    }

    public void SaveObjectColor(GameObject gameObject)
    {
        var id = gameObject.GetInstanceID();
        if (!saveObjectColorList.Contains(id)) saveObjectColorList.Add(id);
    }
}