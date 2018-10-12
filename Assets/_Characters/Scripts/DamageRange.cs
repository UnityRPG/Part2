using System;
using UnityEngine;

namespace RPG.Characters
{
    [Serializable]
    public struct DamageRange
    {

        public float min, max;

        public DamageRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public static DamageRange FromPercentage(float damage, float percentRange)
        {
            var min = Mathf.Clamp(damage - damage * 0.5f * percentRange, 0, float.PositiveInfinity);
            var max = Mathf.Clamp(damage + damage * 0.5f * percentRange, 0, float.PositiveInfinity);
            return new DamageRange(min, max);
        }
    }
}
