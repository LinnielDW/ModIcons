using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace ModIcons;

[StaticConstructorOnStartup]
public class ModIconPatches
{
    static ModIconPatches()
    {
        var harmony = new Harmony("com.arquebus.rimworld.mod.modIcon");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}

public class ModIcons: Mod
{
    ModIconSettings settings;
    public ModIcons(ModContentPack content) : base(content)
    {
        settings = GetSettings<ModIconSettings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        settings.DoSettingsWindowContents(inRect);
    }
    
    public override string SettingsCategory()
    {
        if ("ModIcon_Settings_Category".CanTranslate())
        {
            return "ModIcon_Settings_Category".Translate();
        }
        else
        {
            return "Mod Icons";
        }
    }
}