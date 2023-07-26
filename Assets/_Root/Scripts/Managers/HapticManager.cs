using UnityEngine;
using Lofelt.NiceVibrations;

namespace _Root.Scripts.Managers
{
    public class HapticManager : MonoBehaviour
    {
        #region Singleton

        public static HapticManager Instance;
        
                private void Awake()
                {
                    if (Instance != null && Instance != this) 
                    {
                        Destroy((this));
                        return;
                    }
        
                    Instance = this;
                }

        #endregion

        private bool _isHapticSupported = false;

        private void Start()
        {
            _isHapticSupported = DeviceCapabilities.isVersionSupported;
        }

        public void PlayRigidHaptic()
        {
            if (_isHapticSupported)
            {
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.RigidImpact);
            }
        }

        public void PlaySoftHaptic()
        {
            if (_isHapticSupported)
            {
#if UNITY_IOS
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
#elif UNITY_ANDROID
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
#endif
            }
        }

        public void PlayLightHaptic()
        {
            if (_isHapticSupported)
            {
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
            }
        }

        public void PlaySelectionHaptic()
        {
            if (_isHapticSupported)
            {
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
            }
        }

        public void PlayFailureHaptic()
        {
            if (_isHapticSupported)
            {
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
            }
        }

        public void PlaySuccessHaptic()
        {
            if (_isHapticSupported)
            {
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
            }
        }

        public void PlayWarningHaptic()
        {
            if (_isHapticSupported)
            {
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
            }
        }

        public void PlayHeavyHaptic()
        {
            if (_isHapticSupported)
            {
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
            }
        }
        
    }
}
