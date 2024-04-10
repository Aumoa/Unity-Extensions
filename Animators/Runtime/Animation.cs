using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Ayla.Animators.Runtime
{
    [RequireComponent(typeof(Animator))]
    public class Animation : MonoBehaviour
    {
        [SerializeField]
        private AnimationClip m_Clip;

        [SerializeField]
        private bool m_PlayOnAwake = true;

        public AnimationClip clip => m_Clip;

        private Animator m_Animator;
        private PlayableGraph m_PlayableGraph;
        private PlayableOutput m_PlayableOutput;
        private AnimationClipPlayable m_ClipPlayable;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_PlayableGraph = PlayableGraph.Create();
            m_PlayableOutput = AnimationPlayableOutput.Create(m_PlayableGraph, "Animation", m_Animator);
            RefreshClipPlayable();

            if (m_PlayOnAwake)
            {
                Play();
            }
        }

        public void Play()
        {
            if (m_ClipPlayable.IsValid())
            {
                m_PlayableGraph.Play();
            }
        }

        private void RefreshClipPlayable()
        {
            if (m_Clip)
            {
                m_ClipPlayable = AnimationClipPlayable.Create(m_PlayableGraph, m_Clip);
                m_PlayableOutput.SetSourcePlayable(m_ClipPlayable);
            }
            else
            {
                if (m_ClipPlayable.IsValid())
                {
                    m_ClipPlayable.Destroy();
                }
                m_ClipPlayable = default;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (m_PlayableGraph.IsValid())
            {
                RefreshClipPlayable();
            }
        }
#endif
    }
}
