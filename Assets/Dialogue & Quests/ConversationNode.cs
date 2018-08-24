using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [System.Serializable]
    public class ConversationNode
    {
        public string UUID;
        public Vector2 position;
        public string text;
        public List<string> children = new List<string>();
    }
}