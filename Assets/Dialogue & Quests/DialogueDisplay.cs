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
        NPCTextField.text = node != null ? node.text : "";

        foreach (Transform child in responseHolder)
        {
            Destroy(child.gameObject);
        }

        if (node != null)
        {
            foreach (var child in node.children)
            {
                var responseObject = Instantiate(responsePrefab, responseHolder);
                var childNode = activeConversation.GetNodeByUUID(child);
                responseObject.GetComponent<Text>().text = childNode.text;
                responseObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (childNode.children.Count == 0)
                    {
                        currentNode = null;
                        activeConversation = null;
                    }
                    else
                    {
                        currentNode = activeConversation.GetNodeByUUID(childNode.children[0]);
                    }
                    UpdateDisplayForNode(currentNode);
                });
            }
        }
    }


}
