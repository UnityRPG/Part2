using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] GameObject objectToDestroy;

        public void DestroyText()
        {
            Destroy(objectToDestroy);
        }
    }
}