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

        private DisplayUI displayUI;

        internal static new ManualLogSource Logger;

        public void Awake()
        {
            displayUI = gameObject.AddComponent<DisplayUI>();

            Logger = base.Logger;
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
        }

        public void Update()
        {
            var lines = displayUI.lines;
            lines.Clear();

            if (scrGameControl.Instance == null || scrGameControl.Instance.localPlayerID == -1)
            {
                lines.Add("Welcome, Player!");
                return;
            }
        }
    }
}
