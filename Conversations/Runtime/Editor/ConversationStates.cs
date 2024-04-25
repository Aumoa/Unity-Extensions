using System;

namespace Ayla.Conversations
{
    [Serializable]
    internal class ConversationStates
    {
        public Sequence selectedAsset { get; private set; }

        public event Action SelectedAssetChanged;

        public void SelectAsset(Sequence sequence)
        {
            if (selectedAsset != sequence)
            {
                selectedAsset = sequence;
                SelectedAssetChanged?.Invoke();
            }
        }
    }
}