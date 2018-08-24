using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RPG.Dialogue
{
    [CreateAssetMenu(menuName = ("RPG/Conversation"))]
    public class Conversation : ScriptableObject
    {
        [TextArea] [SerializeField] string openingGambit;
        [TextArea] [SerializeField] string playerResponse;
        [SerializeField] List<ConversationNode> _nodes;
        public delegate void ValidateDelegate();
        public event ValidateDelegate onValidated;

        public List<ConversationNode> nodes
        {
            get
            {
                return _nodes;
            }
        }

        void OnValidate()
        {
            Debug.Log("validate");
            string defaultUUID = null;
            var UUIDs = new HashSet<string>();
            foreach (var node in _nodes)
            {
                while (UUIDs.Contains(node.UUID))
                {
                    node.UUID = System.Guid.NewGuid().ToString();
                    EditorUtility.SetDirty(this);
                }

                if (defaultUUID == null)
                {
                    defaultUUID = node.UUID;
                }

                UUIDs.Add(node.UUID);
            }
            foreach (var node in _nodes)
            {
                foreach (var child in node.children)
                {
                    if (!UUIDs.Contains(child))
                    {
                        int i = node.children.IndexOf(child);
                        node.children[i] = defaultUUID;
                        EditorUtility.SetDirty(this);
                    }
                }
            }

            onValidated();
        }

        public string getConvoAsString()
        {
            return "NPC says: " + openingGambit + "\nYou reply: " + playerResponse;
        }
    }
}