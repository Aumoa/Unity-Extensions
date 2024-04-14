using UnityEngine;

namespace Ayla.Timers.Runtime.Channels
{
    public class ChannelMixerUpdater : MonoBehaviour
    {
        [SerializeField]
        private ChannelMixer m_Mixer;

        private void OnDestroy()
        {
            ChannelUpdateSystem.UnregisterUpdater(this);
        }

        private void OnEnable()
        {
            ChannelUpdateSystem.RegisterUpdater(this);
        }

        private void OnDisable()
        {
            ChannelUpdateSystem.UnregisterUpdater(this);
        }

        protected virtual void OnTimeUpdate()
        {
            UpdateChannelMixer(Time.unscaledDeltaTime);
        }

        protected void UpdateChannelMixer(double deltaTime)
        {
            m_Mixer.UpdateTimer(deltaTime);
        }

        internal void DispatchUpdateChannel()
        {
            OnTimeUpdate();
        }

        public static void Create<T>(ChannelMixer mixer) where T : ChannelMixerUpdater
        {
            var gameObject = new GameObject($"Channel Mixer Updater ({mixer.name})", typeof(T));
            var component = gameObject.GetComponent<T>();
            component.m_Mixer = mixer;
            gameObject.hideFlags |= HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor;
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}