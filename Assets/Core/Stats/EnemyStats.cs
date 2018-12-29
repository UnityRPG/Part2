using System;
using UnityEngine;

namespace RPG.Core.Stats
{
    [Serializable]
    public class EnemyStats : CoreCharacterStats
    {
        public float[] hitsPerSecond;
    }
}