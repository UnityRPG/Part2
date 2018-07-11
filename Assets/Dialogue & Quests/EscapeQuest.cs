using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeQuest : Quest
{
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<QuestJournal>().CompleteQuest();
    }
}