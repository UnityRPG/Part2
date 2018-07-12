using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCollision : MonoBehaviour // Not from quest
{
    [SerializeField] Quest questToComplete;

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<QuestJournal>().CompleteQuest(questToComplete);
    }

}
