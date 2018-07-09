using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voice : MonoBehaviour
{
    // configuration parameters, consider SO
    [SerializeField] Conversation conversation;

    // private instance variables for state

    // cached references for readability

    // messages, then public methods, then private methods...
    private void Start()
    {
        print(conversation.getConvoAsString());
    }
}
