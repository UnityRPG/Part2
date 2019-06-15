﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using UnityEngine.UI;

namespace RPG.UI.Saving
{
    public class SaveList : MonoBehaviour
    {

        [SerializeField]
        GameObject SaveItemPrefab;

        void OnEnable()
        {
            var SaveLoad = FindObjectOfType<SaveLoad>();
            if (SaveLoad == null) return;
            foreach (RectTransform item in GetComponent<Transform>())
            {
                Destroy(item.gameObject);
            }
            foreach (var save in SaveLoad.GetSaveFileList())
            {
                var item = Instantiate(SaveItemPrefab, GetComponent<Transform>());
                var textComponent = item.GetComponentInChildren<Text>();
                textComponent.text = save;
            }
        }
    }
}
