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
            var UUIDs = new HashSet<string>();
            foreach (var node in _nodes)
            {
                while (UUIDs.Contains(node.UUID) || node.UUID == "")
                {
                    node.UUID = System.Guid.NewGuid().ToString();
                    EditorUtility.SetDirty(this);
                }

                UUIDs.Add(node.UUID);
            }
            foreach (var node in _nodes)
            {
                var childrenCopy = node.children.ToArray();
                foreach (var child in childrenCopy)
                {
                    if (!UUIDs.Contains(child))
                    {
                        node.children.Remove(child);
                        EditorUtility.SetDirty(this);
                    }
                }
            }

            onValidated?.Invoke();
        }

        public void AddNode(Vector2 position)
        {
            Undo.RecordObject(this, "Add node.");

            var node = new ConversationNode();
            node.position = position;
            nodes.Add(node);

            OnValidate();
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

            OnValidate();
        }

        public string getConvoAsString()
        {
            return "NPC says: " + openingGambit + "\nYou reply: " + playerResponse;
        }
    }
}