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
    private const string VERSION = "1.0.2";

    public static RandomColorsPlugin instance;

    public List<int> saveObjectColorList = new();
    public ConfigEntry<bool> AffectCruiserEntry;
    public ConfigEntry<bool> affectEnemyEntry;
    public ConfigEntry<bool> affectFlashLight;
    public ConfigEntry<bool> affectItemEntry;
    public ConfigEntry<bool> affectLightEntry;
    public ConfigEntry<bool> affectSunLightEntry;

    private void Awake()
    {
        instance = this;

        Logger.LogInfo("RandomColors starting....");

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

        Harmony.CreateAndPatchAll(typeof(PatchRoundManager));
        Harmony.CreateAndPatchAll(typeof(PatchGrabbableObject));
        Harmony.CreateAndPatchAll(typeof(PatchEnemyAi));
        Harmony.CreateAndPatchAll(typeof(PatchVehiculeControler));
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

    public void SaveObjectColor(GameObject gameObject)
    {
        var id = gameObject.GetInstanceID();
        if (!saveObjectColorList.Contains(id)) saveObjectColorList.Add(id);
    }
}