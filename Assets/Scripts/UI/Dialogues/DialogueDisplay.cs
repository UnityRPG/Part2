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
        RectTransform root;

        [SerializeField]
        Text NPCTextField;

        [SerializeField]
        Transform responseHolder;

        [SerializeField]
        ResponseDisplay responsePrefab;

        private Speaker speaker;

        private ConversationNode lastNode;

        private void Start()
        {
            ActivateUI(false);
        }

        private void ActivateUI(bool activate)
        {

            root.gameObject.SetActive(activate);

            var player = GameObject.FindGameObjectWithTag("Player");
            speaker = player.GetComponent<Speaker>();
            //speaker.OnCurrentNodeChanged += UpdateDisplay;
        }

        private void Update()
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            var node = speaker.currentNode;

            ActivateUI(node != null);

            if (node != lastNode)
            {
                SetNPCText(node);

                ClearResponseObjects();

                if (node != null)
                {
                    CreateResponsesForNode();
                }
                lastNode = node;
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
                responseObject.text = child.text;
                responseObject.onClick.AddListener(() =>
                {
                    speaker.ChooseResponse(child);
                });
            }
        }
    }
}