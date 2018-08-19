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
        var SaveLoad = FindObjectOfType<SaveLoad>();
        foreach (GameObject item in GetComponent<Transform>())
        {
            Destroy(item);
        }
        foreach (var save in SaveLoad.GetSaveFileList())
        {
            var item = Instantiate(SaveItemPrefab, GetComponent<Transform>());
            var textComponent = item.GetComponentInChildren<Text>();
            textComponent.text = save;
        }
    }

}
