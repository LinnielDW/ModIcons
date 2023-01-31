using System;
using UnityEngine;
using Verse;

namespace ModIcons;

public class ModIconSettings : ModSettings
{
    
    public static bool ShowModIcons = true;
    public override void ExposeData()
    {
        Scribe_Values.Look(ref ShowModIcons, "ShowModIcons", true);
        base.ExposeData();
    }

    public void DoSettingsWindowContents(Rect inRect)
    {
        string label = "ModIcon_Settings_ShowModIcons".CanTranslate() ? "ModIcon_Settings_ShowModIcons".Translate() : "Show Mod Icons";
        string tooltip = "ModIcon_Settings_ShowModIconsTooltip".CanTranslate() ? "ModIcon_Settings_ShowModIconsTooltip".Translate() : "Shows mod icons in the loading screen.\n\nRequires restart in order to take effect.";
        Listing_Standard listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled(label,ref ShowModIcons, tooltip);
        listingStandard.End();
    }

}