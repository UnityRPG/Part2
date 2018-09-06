using System;

namespace RPG.Core.Stats
{
    [Serializable]
    public class PlayerLevel : CoreLevel
    {
        public float XPToLevelUp;
        public float XPPerEnemyKill;
    }
}