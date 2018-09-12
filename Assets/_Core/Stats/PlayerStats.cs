using System;

namespace RPG.Core.Stats
{
    [Serializable]
    public class PlayerStats : CoreCharacterStats
    {
        public float[] XPToLevelUp;
        public float[] XPPerEnemyKill;
    }
}