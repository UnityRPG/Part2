using System;
using UnityEngine;

namespace RPG.Control
{
    abstract class RaycasterCallbackBase<T> : RaycasterBase
    {
        public Action<T> callback { get; private set; }

        public RaycasterCallbackBase(Texture2D cursorTexture, Action<T> callback) : base(cursorTexture)
        {
            this.callback = callback;
        }
    }
}