using BepInEx;
using BepInEx.Logging;
using System;
using System.Reflection;

namespace NeoBonerooms.Mod.Utilities
{
    public static class Logger<T> where T : BaseUnityPlugin
    {
        private static ManualLogSource _instance;

        public static ManualLogSource Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                var plugin = PluginSingleton<T>.Instance;
                if (plugin == null)
                    throw new InvalidOperationException($"Plugin instance for {typeof(T).FullName} not found.");

                var loggerProp = typeof(T).GetProperty("Logger", BindingFlags.Public | BindingFlags.Instance);
                if (loggerProp?.GetValue(plugin) is ManualLogSource log)
                    return _instance = log;

                var logField = typeof(T).GetField("Log", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (logField?.GetValue(plugin) is ManualLogSource logFieldVal)
                    return _instance = logFieldVal;

                _instance = Logger.CreateLogSource(typeof(T).Name);
                _instance.LogWarning($"No logger found on {typeof(T).Name}; using fallback logger.");
                return _instance;
            }
        }

        #region Standard Unity/Console Logs
        public static void Log(LogLevel level, object data) => Instance.Log(level, data);
        public static void Fatal(object data) => Instance.LogFatal(data);
        public static void Error(object data) => Instance.LogError(data);
        public static void Warning(object data) => Instance.LogWarning(data);
        public static void Message(object data) => Instance.LogMessage(data);
        public static void Info(object data) => Instance.LogInfo(data);
        public static void Debug(object data) => Instance.LogDebug(data);
        #endregion
    }
}