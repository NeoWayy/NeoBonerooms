using BepInEx;
using BepInEx.Bootstrap;
using System;
using System.Linq;

namespace NeoBonerooms.Mod.Utilities
{
    public static class PluginSingleton<T> where T : BaseUnityPlugin
    {
        private static T? _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var plugins = Chainloader.PluginInfos.Values;
                    _instance = plugins.Select(x => x.Instance).OfType<T>().SingleOrDefault();

                    if (_instance == null)
                        throw new InvalidOperationException($"Could not locate an instance of plugin type {typeof(T).FullName}");
                }
                return _instance;
            }
            set
            {
                if (_instance == value)
                    return;

                if (_instance != null)
                    throw new InvalidOperationException($"Instance for {typeof(T).FullName} has already been set.");

                _instance = value;
            }
        }

        public static void Initialize()
        {
            _instance = Chainloader.PluginInfos.Values
                .Select(x => x.Instance)
                .OfType<T>()
                .SingleOrDefault();
        }
    }
}
