using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Dialogue;

namespace RPG.UI.Dialogue
{
    public class DialogueDisplay : MonoBehaviour
    {
        [SerializeField]
        Text NPCTextField;

        [SerializeField]
        Transform responseHolder;

        [SerializeField]
        GameObject responsePrefab;

        Voice activeVoice;

        ConversationNode currentNode = null;

        private void Start()
        {
            ActivateUI(false);
        }

        private void ActivateUI(bool activate)
        {
            NPCTextField.gameObject.SetActive(activate);
            responseHolder.gameObject.SetActive(activate);
        }

        public void SetActiveVoice(Voice voice)
        {
            activeVoice = voice;
            if (activeVoice != null)
            {
                var conversation = activeVoice.GetConversation();
                currentNode = conversation.GetRootNode();
            }
            else
            {
                currentNode = null;
            }

            UpdateDisplayForNode(currentNode);
        }

        private void UpdateDisplayForNode(ConversationNode node)
        {
            ActivateUI(node != null);

            SetNPCText(node);

            ClearResponseObjects();

            if (node != null)
            {
                CreateResponsesForNode(node);
            }
        }

        private void SetNPCText(ConversationNode node)
        {
            NPCTextField.text = node != null ? node.text : "";
        }

        private void ClearResponseObjects()
        {
            foreach (Transform child in responseHolder)
            {
                Destroy(child.gameObject);
            }
        }

        private void CreateResponsesForNode(ConversationNode node)
        {
            foreach (var child in node.children)
            {
                var responseObject = Instantiate(responsePrefab, responseHolder);
                var childNode = activeVoice.GetConversation().GetNodeByUUID(child);
                responseObject.GetComponent<Text>().text = childNode.text;
                responseObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    ChooseResponse(childNode);
                });
            }
        }

        private void ChooseResponse(ConversationNode childNode)
        {
            if (childNode.actionToTrigger != "")
            {
                activeVoice.TriggerEventForAction(childNode.actionToTrigger);
            }
            if (childNode.children.Count == 0)
            {
                currentNode = null;
                activeVoice = null;
            }
            else
            {
                currentNode = activeVoice.GetConversation().GetNodeByUUID(childNode.children[0]);
            }
            UpdateDisplayForNode(currentNode);
        }
    }
}