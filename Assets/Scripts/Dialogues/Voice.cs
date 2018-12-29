using System.Collections.Generic;
using UnityEngine;
using RPG.Questing;
using RPG.UI.Dialogue;

namespace RPG.Dialogue
{
    public class Voice : MonoBehaviour
    {
        // configuration parameters, consider SO
        [SerializeField] Conversation conversation;
        [SerializeField] [Tooltip("Optional")] string questId;
        [Space(15)]
        [SerializeField] Transform canvas;
        [SerializeField] GameObject speechBubblePrefab;
        public List<VoiceEventMapping> dialogueEventBindings;

        public Conversation GetConversation()
        {
            return conversation;
        }

        public void TriggerEventForAction(string action)
        {
            foreach (var mapping in dialogueEventBindings)
            {
                if (mapping.name == action)
                {
                    mapping.callback.Invoke();
                }
            }
        }

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
            }
        }

        public void TriggerQuestIfAny()
        {
            var journal = FindObjectOfType<Journal>();
            var quest = journal.GetQuestById(questId);
            if (quest == null) return;
            journal.AddQuest(quest);
        }

        public void CompleteQuestIfAny()
        {
            if (questId == null) { return; }
            var journal = FindObjectOfType<Journal>();
            var quest = journal.GetQuestById(questId);
            if (quest == null) return;
            journal.CompleteQuest(quest);
        }

        private void ShowDialog()
        {
            dialogDisplay.SetActiveVoice(this);
        }
    }
}