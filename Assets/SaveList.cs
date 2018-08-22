using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core.Saving;
using UnityEngine.UI;

public class SaveList : MonoBehaviour {

    [SerializeField]
    GameObject SaveItemPrefab;

    void OnEnable()
    {
        Debug.Log("loading");
        var SaveLoad = FindObjectOfType<SaveLoad>();
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
