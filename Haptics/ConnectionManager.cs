using System;
using Bhaptics.Tact;
using UnityEngine;
using ZHaptics.ConfigManager;
using BepInEx;

namespace ZHaptics.Haptics
{
    // TODO: use HapticPlayerManager
    public class ConnectionManager
    {
        private HapticPlayerManager HapticPlayerManager = null;
        private PlayerResponse Status = null;

        private bool UseFake = false;
        private FakeInstance _fakeInstance = new FakeInstance();

        public IHapticPlayer Player
        {
            get
            {
                if (UseFake)
                    return _fakeInstance;
                return HapticPlayerManager.GetHapticPlayer();
            }
        }

        public void Start()
        {
            HapticPlayerManager = HapticPlayerManager.Instance();
            HapticPlayerManager.BhapticsPlayerConnectionChange += ConnectionStatus;
            Player.StatusReceived += PlayerStatus;

            ConnectionStatus(HapticPlayerManager.Connected);
        }

        public void Stop()
        {
            if (Plugin.Instance.Disabled)
                return;
            
            Debug.LogWarning("Disabling bHaptics support, please restart the game with bHaptics player running.");
            
            UseFake = true;
            HapticPlayerManager.Dispose();
            
            Plugin.Instance.Disable();
        }

        private void PlayerStatus(PlayerResponse res)
        {
            Status = res;
        }

        private void ConnectionStatus(bool state)
        {
            if (!state)
                Stop();
        }
    }
}