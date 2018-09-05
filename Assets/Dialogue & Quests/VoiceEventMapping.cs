using System;
using UnityEngine.Events;
using UnityEngine;

namespace RPG.Dialogue
{
    [Serializable]
    public struct VoiceEventMapping
    {
        public string name;
        public UnityEvent callback;
    }
}
