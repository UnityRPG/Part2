using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Dialogue
{
    public class ResponseDisplay : MonoBehaviour {
        [SerializeField] Text displayText;
        [SerializeField] Button button;

        public string text
        {
            set
            {
                displayText.text = value;
            }
        }

        public Button.ButtonClickedEvent onClick => button.onClick;
    }
}