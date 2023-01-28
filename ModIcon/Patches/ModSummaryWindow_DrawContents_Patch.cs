using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using Verse;
using static ModIcon.ModIconSettings;
using System.IO;

namespace ModIcon.Patches;

[StaticConstructorOnStartup]
[HarmonyPatch]
public static class ModSummaryWindow_DrawContents_Patch
{
    static MethodInfo drawContainerBox = AccessTools.Method(typeof(Widgets), "DrawBoxSolid");
    static ConstructorInfo constructLabelRect = AccessTools.DeclaredConstructor(typeof(Rect), new[] { typeof(Single), typeof(Single), typeof(Single), typeof(Single) });
    static FieldInfo modInfo = AccessTools.Inner(typeof(ModSummaryWindow), "<>c__DisplayClass17_3").GetField("mod");

    static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Inner(typeof(ModSummaryWindow), "<>c__DisplayClass17_3")
            .GetMethod("<DrawContents>b__7", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        foreach (var code in codes)
        {
            yield return code;
            // Log.Message("CodeInstruction: " + "opCode: " + code.opcode + " operand: " + code.operand);
            // Log.Message("CodeInstruction: " + code.ToStringSafe());
            if (ShowModIcons)
            {
                if (code.opcode == OpCodes.Call && code.operand == drawContainerBox)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_1);

                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, modInfo);

                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ModSummaryWindow_DrawContents_Patch), "DrawIcon"));
                }

                if (code.opcode == OpCodes.Call && code.operand == constructLabelRect)
                {
                    yield return new CodeInstruction(OpCodes.Ldloca, 0);

                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, modInfo);

                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ModSummaryWindow_DrawContents_Patch), "ResizeLabelRect"));
                }
            }
        }
    }

    private static string AboutPath(DirectoryInfo rootDir)
    {
        return string.Concat(new string[]
        {
            rootDir.FullName,
            Path.DirectorySeparatorChar.ToString(),
            "About",
            Path.DirectorySeparatorChar.ToString(),
            "ModIcon.png"
        });
    }

    private static void DrawIcon(Rect r, ModMetaData mod)
    {
        if (!File.Exists(AboutPath(mod.RootDir)))
        {
            return;
        }

        Texture2D tex = new Texture2D(0,0);
        tex.LoadImage(File.ReadAllBytes(AboutPath(mod.RootDir)));
        
        Rect iconRect = new Rect(r.x + 8f, r.y + 2f, 32f, 32f);
        GenUI.DrawTextureWithMaterial(iconRect, tex, null);
    }

    private static void ResizeLabelRect(ref Rect r, ModMetaData mod)
    {
        if (!File.Exists(AboutPath(mod.RootDir)))
        {
            return;
        }

        r.x += 36f;
        r.width -= 36f;
    }
}