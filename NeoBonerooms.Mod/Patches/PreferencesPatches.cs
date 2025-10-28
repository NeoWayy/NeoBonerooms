using HarmonyLib;

namespace NeoBonerooms.Mod.Patches
{
    [HarmonyPatch(typeof(Preferences), MethodType.Constructor)]
    public static class PreferencesPatches
    {
        public static void Postfix(Preferences __instance)
        {
            __instance.serverName = "NeoBonerooms";
        }
    }
}
