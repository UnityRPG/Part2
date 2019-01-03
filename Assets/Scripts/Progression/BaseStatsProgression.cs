using UnityEngine;

namespace RPG.Progression
{
    [CreateAssetMenu(menuName = ("RPG/Stats Progression Set"))]
    public class BaseStatsProgression : ScriptableObject
    {
        [SerializeField] PlayerBaseStats playerStats;
        [SerializeField] EnemyClass[] enemyClasses;

        public PlayerBaseStats GetPlayer()
        {
            return playerStats;
        }

        public EnemyBaseStats GetEnemy(string className)
        {
            foreach (var enemyClass in enemyClasses)
            {
                if (enemyClass.className == className)
                {
                    return enemyClass.enemyStats;
                }
            }

            return null;
        }
    }
}