using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.Core.Saving
{
    public class SaveItem : MonoBehaviour
    {

        public void Clicked()
        {
            var TextComponent = GetComponentInChildren<Text>();
            var SaveLoad = FindObjectOfType<SaveLoad>();
            SaveLoad.Load(TextComponent.text);
        }
    }
}