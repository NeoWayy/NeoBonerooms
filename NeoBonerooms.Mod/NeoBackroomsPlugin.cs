using BepInEx;
using BepInEx.Logging;

namespace NeoBonerooms.Mod
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class NeoBackroomsPlugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "org.neowayy.neobonerooms";
        public const string PLUGIN_NAME = "NeoBonerooms";
        public const string PLUGIN_VERSION = "1.0.0";

        internal static new ManualLogSource Logger;

        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
        }
    }
}
