using UnityEngine;
using UnityEngine.UI;
using RPG.Saving;

namespace RPG.UI.Saving
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