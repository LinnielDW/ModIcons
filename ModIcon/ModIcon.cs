using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace ModIcon;

[StaticConstructorOnStartup]
public class ModIconPatches
{
    static ModIconPatches()
    {
        var harmony = new Harmony("com.arquebus.rimworld.mod.modIcon");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}

public class ModIcon: Mod
{
    ModIconSettings settings;
    public ModIcon(ModContentPack content) : base(content)
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