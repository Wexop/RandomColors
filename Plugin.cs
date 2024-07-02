using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;
using RandomColors.Patchs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RandomColors;

[BepInPlugin(GUID, NAME, VERSION)]
public class RandomColorsPlugin : BaseUnityPlugin
{
    private const string GUID = "wexop.randomcolors";
    private const string NAME = "RandomColors";
    private const string VERSION = "1.0.0";

    public static RandomColorsPlugin instance;
    public ConfigEntry<bool> affectEnemyEntry;
    public ConfigEntry<bool> affectItemEntry;
    public ConfigEntry<bool> affectLightEntry;

    private void Awake()
    {
        instance = this;

        Logger.LogInfo("RandomColors starting....");

        affectLightEntry = Config.Bind("General", "AffectLights", true,
            "Every lights in the game have a random color every day. No need to restart the game :)");
        CreateBoolConfig(affectLightEntry);

        affectItemEntry = Config.Bind("General", "AffectItems", true,
            "Every grabbable items in the game have a random color on spawn. No need to restart the game :)");
        CreateBoolConfig(affectItemEntry);

        affectEnemyEntry = Config.Bind("General", "AffectEnemies", true,
            "Every monsters in the game have a random color on spawn. No need to restart the game :)");
        CreateBoolConfig(affectEnemyEntry);

        Harmony.CreateAndPatchAll(typeof(PatchRoundManager));
        Harmony.CreateAndPatchAll(typeof(PatchGrabbableObject));
        Harmony.CreateAndPatchAll(typeof(PatchEnemyAi));
        Logger.LogInfo("RandomColors is patched!");
    }

    public float RandomZeroToOne()
    {
        return Random.Range(0f, 1f);
    }

    public Color GetRandomColor()
    {
        return new Color(RandomZeroToOne(),
            RandomZeroToOne(), RandomZeroToOne());
    }

    private void CreateBoolConfig(ConfigEntry<bool> configEntry)
    {
        var exampleSlider = new BoolCheckBoxConfigItem(configEntry, new BoolCheckBoxOptions
        {
            RequiresRestart = false
        });
        LethalConfigManager.AddConfigItem(exampleSlider);
    }
}