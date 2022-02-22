using System;
using ZHaptics.ConfigManager;
using ZHaptics.Haptics;
using ZHaptics.Haptics.EffectManagers;
using BepInEx;
using BepInEx.IL2CPP;
using UnityEngine;
using ZHaptics.Haptics.Patterns;
using ZHaptics.Helpers;

namespace ZHaptics
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        private static Plugin _instance = null;
        private static readonly object _padlock = new object();

        public static Plugin Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Plugin();
                    }
                    return _instance;
                }
            }
        }

        public ConnectionManager Haptics;

        public bool Disabled { get; private set; } = false;

        /**
         * The first call made on application start
         */
        private void RootInit()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public void OnApplicationQuit()
        {
            Log.LogInfo("Application closing");

            StopServices();
        }

        private void InitializeManagers()
        {
            Haptics = new ConnectionManager();
        }

        private void StartServices()
        {
            Log.LogInfo("Starting Services");

            Haptics.Start();
        }

        private void StopServices()
        {
            Log.LogInfo("Stopping Services");

            Haptics.Stop();
        }

        public void OnUpdate()
        {
            EffectLoopRegistry.Update();
        }

        // public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        // {
        //     EffectLoopRegistry.LevelInit();
        // }

        public void Disable()
        {
            if (Disabled)
                return;

            // Harmony.UnpatchAll();

            Disabled = true;
        }

        public void OnFixedUpdate()
        {
            MovementHelper.OnFixedUpdate();
            StaminaHelper.OnFixedUpdate();
        }

        public override void Load()
        {
            Log.LogInfo("Application initializing");

            RootInit();

            FileHelpers.EnforceDirectory();
            ConfigLoader.InitConfig();
            ConfigManager.DynConfig.UpdateConfig(ConfigManager.DynConfig.SceneMode.General);

            Patreon.Run(); // (●'◡'●)

            InitializeManagers();

            StartServices();

            PatternManager.LoadPatterns();
            EffectEventsDispatcher.Init();

            Log.LogInfo("Successfully started");
        }
    }
}
