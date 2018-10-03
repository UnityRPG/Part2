using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core.Saving;

namespace RPG.CameraUI
{
    public class PauseMenu : MonoBehaviour
    {

        [SerializeField]
        GameObject mainMenu;
        [SerializeField]
        GameObject loadMenu;
        [SerializeField]
        GameObject saveConfirmationMenu;

        GameObject currentlyActiveMenu;

        SaveLoad SaveSystem;

        void Start()
        {
            mainMenu.SetActive(false);
            loadMenu.SetActive(false);
            saveConfirmationMenu.SetActive(false);
            SaveSystem = FindObjectOfType<SaveLoad>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (currentlyActiveMenu == null)
                {
                    OpenMenu();
                }
                else
                {
                    HideMenu();
                }
            }
        }

        private void OpenMenu()
        {
            Time.timeScale = 0;
            currentlyActiveMenu = mainMenu;
            currentlyActiveMenu.SetActive(true);
        }

        public void HideMenu()
        {
            currentlyActiveMenu.SetActive(false);
            currentlyActiveMenu = null;
            Time.timeScale = 1;
        }

        public void ShowLoadMenu()
        {
            ShowMenu(loadMenu);
        }

        public void ShowMainMenu()
        {
            ShowMenu(mainMenu);
        }

        void ShowMenu(GameObject newMenu)
        {
            currentlyActiveMenu.SetActive(false);
            currentlyActiveMenu = newMenu;
            currentlyActiveMenu.SetActive(true);
        }

        public void Save()
        {
            SaveSystem.Save();
            ShowMenu(saveConfirmationMenu);
        }
    }
}
