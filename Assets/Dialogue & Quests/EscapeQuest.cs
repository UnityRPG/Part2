using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeQuest : MonoBehaviour
{
    [SerializeField] QuestConfig questConfig;

    private void OnCollisionEnter(Collision collision)
    {
        FindObjectOfType<QuestJournal>().CompleteQuest(questConfig);
    }
}