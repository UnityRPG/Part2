using UnityEngine;
using RPG.Characters;
using UnityEngine.SceneManagement;
using RPG.Core.Saving;

namespace RPG.SceneManagement
{
    public class TransitionPoint : MonoBehaviour
    {
        public string newSceneName;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerControl>())
            {
                FindObjectOfType<SaveLoad>().Save();
                SceneManager.LoadScene(newSceneName);
            }
        }
    }
}
