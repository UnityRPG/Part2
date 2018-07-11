using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// common data and behaviour for all quests
// TODO consider persistence across scene loads
public class Quest : MonoBehaviour
{
    [SerializeField] string questName;

    public string GetQuestAsString()
    {
        return questName;
        // TODO consider using the gameObject name
    }
	
}
