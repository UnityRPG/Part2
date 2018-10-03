using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Core
{
    public class UILoader : MonoBehaviour
    {

        [SerializeField] string sceneName;

        // Use this for initialization
        void Start()
        {
            var uiScene = SceneManager.GetSceneByName(sceneName);
            if (!uiScene.isLoaded)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }

    }
}
