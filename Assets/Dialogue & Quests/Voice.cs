using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voice : MonoBehaviour
{
    // TODO rename "enemy canvas" to "NPC canvas"

    // configuration parameters, consider SO
    [SerializeField] Conversation conversation;
    [SerializeField] Transform canvas;
    [SerializeField] GameObject speechBubblePrefab;

    // private instance variables for state

    // cached references for readability

    // messages, then public methods, then private methods...
    private void OnEnable()
    {
        Instantiate(speechBubblePrefab, canvas);
    }
}
