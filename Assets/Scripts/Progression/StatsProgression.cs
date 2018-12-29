using UnityEngine;

namespace RPG.Progression
{
    [CreateAssetMenu(menuName = ("RPG/Stats Progression Set"))]
    public class StatsProgression : ScriptableObject
    {
        [SerializeField] PlayerStats playerStats;
        [SerializeField] EnemyClass[] enemyClasses;

        public PlayerStats GetPlayer()
        {
            return playerStats;
        }

        public EnemyStats GetEnemy(string className)
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