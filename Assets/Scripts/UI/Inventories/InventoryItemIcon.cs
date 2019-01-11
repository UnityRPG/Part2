using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Inventories;

public class InventoryItemIcon : MonoBehaviour
{
    Image _iconImage;

    private void Awake() {
        _iconImage = GetComponent<Image>();
        SetItem(null);
    }

    public void SetItem(InventoryItem item)
    {
        if (item == null)
        {
            _iconImage.enabled = false;
        }
        else
        {
            _iconImage.enabled = true;
            _iconImage.sprite = item.icon;
        }
    }
}
