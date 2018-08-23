using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public bool HasCycle()
        {
            if (_nodes.Count == 0) return false;

            bool[] visited = new bool[_nodes.Count];
            var nextNodes = new Queue<int>();
            nextNodes.Enqueue(0);
            do
            {
                int index = nextNodes.Dequeue();
                if (visited[index])
                {
                    return true;
                }
                visited[index] = true;
                foreach (int child in _nodes[index].children)
                {
                    nextNodes.Enqueue(child);
                }
            } while (nextNodes.Count > 0);

            return false;
        }

        public string getConvoAsString()
        {
            return "NPC says: " + openingGambit + "\nYou reply: " + playerResponse;
        }
    }
}