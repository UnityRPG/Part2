﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Characters; // So we can detect by type

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
		[SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Texture2D talkCursor = null;
		[SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        const int POTENTIALLY_WALKABLE_LAYER = 8;
        float maxRaycastDepth = 100f; // Hard coded value

        Rect currentScreenRect;

        public delegate void OnMouseOverEnemy(EnemyAI enemy);
		public event OnMouseOverEnemy onMouseOverEnemy;

        public delegate void OnMouseOverVoice(Voice voice);
        public event OnMouseOverVoice onMouseOverVoice;

		public delegate void OnMouseOverTerrain(Vector3 destination);
        public event OnMouseOverTerrain onMouseOverPotentiallyWalkable;

		void Update()
        {
            currentScreenRect = new Rect(0, 0, Screen.width, Screen.height);

            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Impliment UI interaction
            }
            else
            {
                PerformRaycasts();
            }
        }

        private void PerformRaycasts()
		{
            if (currentScreenRect.Contains(Input.mousePosition))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // Specify layer priorities below, order matters
                if (InteractWithVoice(ray)) { return; }
                if (InteractWithEnemyAI(ray)) { return; } 
                if (NavigateToWalkable(ray)) { return; }
            }
		}

        private bool InteractWithVoice(Ray ray)
        {
            // Consider making generic RaycastFor<Type>() method
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            var gameObjectHit = hitInfo.collider.gameObject;
            var voiceHit = gameObjectHit.GetComponent<Voice>();
            if (voiceHit)
            {
                Cursor.SetCursor(talkCursor, cursorHotspot, CursorMode.Auto);
                voiceHit.VoiceClicked();
                return true;
            }
            return false;
        }

	    private bool InteractWithEnemyAI(Ray ray)
		{
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            var gameObjectHit = hitInfo.collider.gameObject;
            var enemyHit = gameObjectHit.GetComponent<EnemyAI>();
            if (enemyHit)
            {
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverEnemy(enemyHit);
                return true;
            }
            return false;
		}

        private bool NavigateToWalkable(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer);
            if (potentiallyWalkableHit)
            {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverPotentiallyWalkable(hitInfo.point);
                return true;
            }
            return false;
        }
    }
}