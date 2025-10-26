using BepInEx;
using BepInEx.Logging;
using Steamworks;
using HarmonyLib;
using System;
using NeoBonerooms.Mod.Utilities;

namespace NeoBonerooms.Mod
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInProcess("Bonerooms.exe")]
    public class NeoBackroomsPlugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "org.neowayy.neobonerooms";
        public const string PLUGIN_NAME = "NeoBonerooms";
        public const string PLUGIN_VERSION = "1.0.0";

        private DisplayUI displayUI;

        string playerSteamName = "N/A";

        public NeoBackroomsPlugin()
        {
            PluginSingleton<NeoBackroomsPlugin>.Instance = this;
        }

        public void Awake()
        {
            var harmony = new Harmony(PLUGIN_GUID);

            displayUI = gameObject.AddComponent<DisplayUI>();

            try
            {
                if (!SteamClient.IsValid)
                {
                    SteamClient.Init(2719940, true); // The Bonerooms App ID
                }
                playerSteamName = SteamClient.Name;
                Logger.LogInfo($"Steam name loaded: {playerSteamName}");
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to get Steam name: {e}");
            }

            harmony.PatchAll();
        }

        public void Update()
        {
            var lines = displayUI.lines;
            lines.Clear();

            if (scrGameControl.Instance == null || scrGameControl.Instance.localPlayerID == -1)
            {
                lines.Add($"Welcome, {playerSteamName}!");
                return;
            }
        }
    }
}
