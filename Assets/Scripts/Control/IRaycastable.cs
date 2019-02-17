using UnityEngine;

namespace RPG.Control
{
    public interface IRaycastable
    {
        bool Raycast(GameObject owner);
    }
}