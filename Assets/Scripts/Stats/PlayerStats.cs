using System;

namespace RPG.Stats
{
    [Serializable]
    public class PlayerStats : CoreCharacterStats
    {
        public float[] XPToLevelUp;
        public float[] XPPerEnemyKill;
    }
}