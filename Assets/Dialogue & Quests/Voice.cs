using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RPG.Questing;

namespace RPG.Dialogue
{
    public class Voice : MonoBehaviour
    {
        // configuration parameters, consider SO
        [SerializeField] Conversation conversation;
        [SerializeField] [Tooltip("Optional")] Quest quest;
        [Space(15)]
        [SerializeField] Transform canvas;
        [SerializeField] GameObject speechBubblePrefab;

        const float DIALOG_LIFETIME = 5.0f;

        // private instance variables for state

        // cached references for readability
        DialogueDisplay dialogDisplay;

        // messages, then public methods, then private methods...
        void Start()
        {
            Instantiate(speechBubblePrefab, canvas);
            dialogDisplay = FindObjectOfType<DialogueDisplay>();
        }

        public void VoiceClicked()
        {
            if (Input.GetMouseButtonDown(0))  // "Down" so we only get one event
            {
                ShowDialog();
                TriggerQuestIfAny();
            }
        }

        private void TriggerQuestIfAny()
        {
            if (!quest) { return; }
            FindObjectOfType<Journal>().AddQuest(quest);
        }

        private void ShowDialog()
        {
            dialogDisplay.activeConversation = conversation;
        }
    }
}