using UnityEngine;

namespace RPG.Control
{
    public interface IRaycastable
    {

        int priority { get; }
        CursorType cursor { get; }
        bool HandleRaycast(PlayerControl playerControl);
    }

    public enum CursorType
    {
        Walk,
        Attack,
        Pickup,
        Talk,
        None,
        Default
    }
}