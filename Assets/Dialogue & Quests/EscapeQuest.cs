using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeQuest : MonoBehaviour
{
    [SerializeField] QuestConfig questConfig;

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<QuestJournal>().CompleteQuest(questConfig);
    }
}