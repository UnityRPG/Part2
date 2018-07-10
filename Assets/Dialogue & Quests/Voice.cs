using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;

public class Voice : MonoBehaviour
{
    // TODO rename "enemy canvas" to "NPC canvas"

    // configuration parameters, consider SO
    [SerializeField] Conversation conversation;
    [SerializeField] [Tooltip("Optional")]Quest quest;
    [Space(15)]
    [SerializeField] Transform canvas;
    [SerializeField] GameObject speechBubblePrefab;

    // private instance variables for state

    // cached references for readability
    Text dialogBox; // TODO consider singleton

    // messages, then public methods, then private methods...
    private void Start()
    {
        Instantiate(speechBubblePrefab, canvas);
        RegisterForMouseClicks();
        dialogBox = GameObject.FindWithTag("DialogBox").GetComponent<Text>(); // TODO yuck
    }

    private void RegisterForMouseClicks()
    {
        var cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.onMouseOverVoice += OnClick;
    }

    private void OnClick(Voice voice)
    {
        if (Input.GetMouseButtonDown(0))  // "Down" so we only get one event
        {
            dialogBox.text = conversation.getConvoAsString();
            GiveQuest();
        }
    }

    private void GiveQuest()
    {
        print(quest.GetQuestName());
    }
}
