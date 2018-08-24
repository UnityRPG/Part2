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
                for (int i = 0; i < node.children.Count; ++i)
                {
                    var child = node.children[i];
                    if (!UUIDs.Contains(child))
                    {
                        node.children[i] = defaultUUID;
                        EditorUtility.SetDirty(this);
                    }
                }
            }

            onValidated?.Invoke();
        }

        public ConversationNode GetNodeByUUID(string UUID)
        {
            foreach (var node in _nodes)
            {
                if (node.UUID == UUID)
                {
                    return node;
                }
            }
            return null;
        }

        public void DeleteNode(ConversationNode node)
        {
            Undo.RecordObject(this, "Delete node.");
            _nodes.Remove(node);
        }

        public string getConvoAsString()
        {
            return "NPC says: " + openingGambit + "\nYou reply: " + playerResponse;
        }
    }
}