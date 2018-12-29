using System;

namespace RPG.Core.Stats
{
    [Serializable]
    public class CoreCharacterStats
    {
        public float[] health;
        public float[] damagePerHit;
        public float[] specialDamage;
    }
}