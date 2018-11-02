using UnityEngine;
using UnityEngine.UI;
using RPG.InventorySystem;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] Image iconImage;

    public void SetItem(InventoryItem item)
    {
        if (item == null)
        {
            iconImage.enabled = false;
        }
        else 
        {
            iconImage.enabled = true;
            iconImage.sprite = item.icon;
        }
    }
}
