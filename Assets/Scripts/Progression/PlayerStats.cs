using System;

namespace RPG.Progression
{
    [Serializable]
    public class PlayerStats : CoreCharacterStats
    {
        public float[] XPToLevelUp;
        public float[] XPPerEnemyKill;
    }
}