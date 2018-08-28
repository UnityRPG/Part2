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

    public Conversation activeConversation { get; set; }

    ConversationNode currentNode = null;

    void Update()
    {
        if (activeConversation != null)
        {
            currentNode = activeConversation.GetRootNode();
        }
        else
        {
            currentNode = null;
        }

        if (currentNode != null)
        {
            NPCTextField.text = currentNode.text;
        }
    }

}
