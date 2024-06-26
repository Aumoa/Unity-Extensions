using Ayla.Timers.Runtime.Channels;
using UnityEngine;

namespace Ayla.Timers.Runtime.Animators
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorChannelUpdater : MonoBehaviour
    {
        [SerializeField]
        private ChannelBinder m_ChannelBinder;

        private Animator m_Animator;

        public Channel channel => m_ChannelBinder.channel;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        private void Update()
        {
            if (channel)
            {
                m_Animator.speed = (float)channel.scale;
            }
        }

        public void SetChannel(ChannelMixer mixer, int channelIndex)
        {
            m_ChannelBinder.mixer = mixer;
            m_ChannelBinder.channelIndex = channelIndex;
        }

        public void SetChannel(ChannelMixer mixer, string channelName)
        {
            SetChannel(mixer, mixer.channels.FindIndex(p => p.name == channelName));
        }
    }
}