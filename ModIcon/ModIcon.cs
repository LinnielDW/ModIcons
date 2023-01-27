using System.Reflection;
using HarmonyLib;
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