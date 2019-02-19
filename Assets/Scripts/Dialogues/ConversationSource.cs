using System.Collections.Generic;
using UnityEngine;
using RPG.Questing;
using RPG.Control;

namespace RPG.Dialogue
{
    public class ConversationSource : MonoBehaviour, IRaycastable
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
        Speaker speaker;

        int IRaycastable.priority => 8;

        CursorType IRaycastable.cursor => CursorType.Talk;

        // messages, then public methods, then private methods...
        void Start()
        {
            Instantiate(speechBubblePrefab, canvas);
            speaker = GameObject.FindGameObjectWithTag("Player").GetComponent<Speaker>();
        }

        public void VoiceClicked()
        {
            ShowDialog();
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
            speaker.SetActiveConversation(this);
        }

        bool IRaycastable.HandleRaycast(PlayerControl playerControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShowDialog();
            }
            return true;
        }
    }
}