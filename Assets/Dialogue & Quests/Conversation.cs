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

        public List<ConversationNode> nodes
        {
            get
            {
                return _nodes;
            }
        }

        public Conversation()
        {
            _nodes = new List<ConversationNode>();
            _nodes.Add(new ConversationNode());
        }

        public string getConvoAsString()
        {
            return "NPC says: " + openingGambit + "\nYou reply: " + playerResponse;
        }
    }
}