using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] GameObject prefab;
        [SerializeField] Vector3 textOffset;

        public void Create(float amount, Vector3 location)
        {
            var instance = Instantiate(prefab);
            instance.transform.parent = transform;
            var damageText = instance.GetComponent<DamageText>();
            damageText.Activate(amount, location + textOffset);
        }
    }
}
