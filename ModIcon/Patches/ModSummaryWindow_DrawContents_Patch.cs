using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace ModIcon.Patches;

[StaticConstructorOnStartup]
[HarmonyPatch]
public static class ModSummaryWindow_DrawContents_Patch
{
    static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Inner(typeof(ModSummaryWindow), "<>c__DisplayClass17_3")
            .GetMethod("<DrawContents>b__7", BindingFlags.NonPublic | BindingFlags.Instance);
    }
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {

        var codes = new List<CodeInstruction>(instructions);
        var x = AccessTools.Method(typeof(Widgets), "DrawBoxSolid");
        var y = AccessTools.DeclaredConstructor(typeof(Rect), new [] { typeof(Single) ,typeof(Single) ,typeof(Single) , typeof(Single)});

        var bla = AccessTools.Inner(typeof(ModSummaryWindow), "<>c__DisplayClass17_3").GetField("mod");
        foreach (var code in codes)
        {
            yield return code;
            // Log.Message("CodeInstruction: " + "opCode: " + code.opcode + " operand: " + code.operand);
            // Log.Message("CodeInstruction: " + code.ToStringSafe());
            if (code.opcode == OpCodes.Call && code.operand == x)
            {
                
                yield return new CodeInstruction(OpCodes.Ldarg_1);
                
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Ldfld, bla);
                
                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ModSummaryWindow_DrawContents_Patch), "DrawIcon"));
            }

            if (code.opcode == OpCodes.Call && code.operand == y)
            {
                yield return new CodeInstruction(OpCodes.Ldloca, 0);
                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ModSummaryWindow_DrawContents_Patch), "ResizeLabelRect"));
            }

        }
    }
    
    
    private static void DrawIcon(Rect r, ModMetaData mod)
    {
        Rect iconRect = new Rect(r.x + 8f, r.y + 2f, 32f, 32f);
        //TODO: load in a path to render a small icon here
        GenUI.DrawTextureWithMaterial(iconRect, mod.PreviewImage, null);
        // Widgets.DrawBoxSolid(iconRect, Color.red);
    }

    private static void ResizeLabelRect(ref Rect r)
    {
        r.x += 36f;
        r.width -= 36f;
    }
}