using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(menuName = ("RPG/Conversation"))]
    public class Conversation : ScriptableObject
    {
        [TextArea] [SerializeField] string openingGambit;
        [TextArea] [SerializeField] string playerResponse;

        public string getConvoAsString()
        {
            return "NPC says: " + openingGambit + "\nYou reply: " + playerResponse;
        }
    }
}