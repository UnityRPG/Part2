using System;

namespace RPG.Progression
{
    [Serializable]
    public class PlayerBaseStats : CoreBaseStats
    {
        public float[] XPToLevelUp;
        public float[] XPPerEnemyKill;
    }
}