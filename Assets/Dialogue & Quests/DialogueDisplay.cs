using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Dialogue;

public class DialogueDisplay : MonoBehaviour {

    [SerializeField]
    Text NPCTextField;

    [SerializeField]
    Transform responseHolder;

    [SerializeField]
    GameObject responsePrefab;

    Conversation activeConversation;

    ConversationNode currentNode = null;

    public void SetActiveConversation(Conversation conversation)
    {
        activeConversation = conversation;
        if (activeConversation != null)
        {
            currentNode = activeConversation.GetRootNode();
        }
        else
        {
            currentNode = null;
        }
        UpdateDisplayForNode(currentNode);
    }

    private void UpdateDisplayForNode(ConversationNode node)
    {
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
