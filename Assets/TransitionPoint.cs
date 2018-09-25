using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class TransitionPoint : MonoBehaviour
    {
        public string newSceneName;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerControl>())
            {
                SceneManager.LoadScene(newSceneName);
            }
        }
    }
}
