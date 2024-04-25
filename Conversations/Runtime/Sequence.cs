using UnityEngine;

namespace Ayla.Conversations
{
    [CreateAssetMenu(fileName = "Sequence.asset", menuName = "Ayla/Conversations/Sequence")]
    public class Sequence : ScriptableObject
    {
        public Block[] m_Blocks;
    }
}