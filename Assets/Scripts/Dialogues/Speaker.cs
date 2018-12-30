using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class Speaker : MonoBehaviour
    {
        ConversationSource conversationSource;

        public ConversationNode currentNode { get; private set; }

        public event Action OnCurrentNodeChanged;

        public void SetActiveConversation(ConversationSource conversationSource)
        {
            this.conversationSource = conversationSource;
            if (this.conversationSource != null)
            {
                var conversation = this.conversationSource.GetConversation();
                currentNode = conversation.GetRootNode();
            }
            else
            {
                currentNode = null;
            }

            OnCurrentNodeChanged();
        }

        public IEnumerable<ConversationNode> children
        {
            get
            {
                foreach (var child in currentNode.children)
                {
                    yield return conversationSource.GetConversation().GetNodeByUUID(child);
                }
            }
        }

        public void ChooseResponse(ConversationNode childNode)
        {
            if (childNode.actionToTrigger != "")
            {
                conversationSource.TriggerEventForAction(childNode.actionToTrigger);
            }
            if (childNode.children.Count == 0)
            {
                currentNode = null;
                conversationSource = null;
            }
            else
            {
                currentNode = conversationSource.GetConversation().GetNodeByUUID(childNode.children[0]);
            }
            OnCurrentNodeChanged();
        }
    }
}