using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(menuName = ("RPG/Stat Set"))]
    public class StatSet : ScriptableObject
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