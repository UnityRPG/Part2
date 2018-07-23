using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
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
        Text dialogBox;

        // messages, then public methods, then private methods...
        void Start()
        {
            Instantiate(speechBubblePrefab, canvas);
            dialogBox = GameObject.FindWithTag("DialogBox").GetComponent<Text>();
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
            dialogBox.text = conversation.getConvoAsString();
            StartCoroutine(ExpireDialog());
        }

        IEnumerator ExpireDialog()
        {
            yield return new WaitForSeconds(DIALOG_LIFETIME);
            dialogBox.text = "";
        }
    }
}