using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Dialogue;

namespace RPG.UI.Dialogue
{
    public class DialogueDisplay : MonoBehaviour
    {
        [SerializeField]
        Text NPCTextField;

        [SerializeField]
        Transform responseHolder;

        [SerializeField]
        GameObject responsePrefab;

        private Speaker speaker;

        private void Start()
        {
            ActivateUI(false);
        }

        private void ActivateUI(bool activate)
        {
            NPCTextField.gameObject.SetActive(activate);
            responseHolder.gameObject.SetActive(activate);

            var player = GameObject.FindGameObjectWithTag("Player");
            speaker = player.GetComponent<Speaker>();
            speaker.OnCurrentNodeChanged += UpdateDisplay;
        }

        private void UpdateDisplay()
        {
            var node = speaker.currentNode;

            ActivateUI(node != null);

            SetNPCText(node);

            ClearResponseObjects();

            if (node != null)
            {
                CreateResponsesForNode();
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

        private void CreateResponsesForNode()
        {
            foreach (var child in speaker.children)
            {
                var responseObject = Instantiate(responsePrefab, responseHolder);
                responseObject.GetComponent<Text>().text = child.text;
                responseObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    speaker.ChooseResponse(child);
                });
            }
        }
    }
}