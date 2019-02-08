using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Combat
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textMesh;

        public void Activate(float amount, Vector3 position)
        {
            transform.position = 
                Camera.main.WorldToScreenPoint(position);
            textMesh.text = amount.ToString("0.0");
        }
    }
}