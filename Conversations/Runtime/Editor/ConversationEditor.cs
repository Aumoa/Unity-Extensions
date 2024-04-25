using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Ayla.Conversations
{
    internal class ConversationEditor : EditorWindow
    {
        [SerializeField]
        private ConversationStates m_States;

        private bool m_Initialized;
        private TimelineView m_TimelineView;

        private void OnEnable()
        {
            m_States ??= new ConversationStates();
        }

        private void OnDisable()
        {
            CleanupResources();
        }

        private void OnGUI()
        {
            InitializeComponentsOnDemand();

            var position = new Rect(Vector2.zero, this.position.size);
            m_TimelineView.Draw(position);
        }

        private void InitializeComponentsOnDemand()
        {
            if (m_Initialized == false)
            {
                m_TimelineView = new TimelineView(m_States);
                m_Initialized = true;
            }
        }

        private void CleanupResources()
        {
            m_TimelineView?.Dispose();
            m_Initialized = false;
        }

        [OnOpenAsset]
        private static bool OnOpenAsset(int instanceId)
        {
            if (EditorUtility.InstanceIDToObject(instanceId) is Sequence sequence)
            {
                GetWindow<ConversationEditor>().m_States.SelectAsset(sequence);
                return true;
            }

            return false;
        }
    }
}