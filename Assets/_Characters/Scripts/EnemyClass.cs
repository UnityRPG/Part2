using UnityEngine;
using RPG.Core.Stats;

namespace RPG.Characters
{
    public class EnemyClass : MonoBehaviour
    {
        [SerializeField] StatSet statSet;
        [SerializeField] string className;
        public int level;

        public EnemyStats GetEnemy()
        {
            return statSet.GetEnemy(className);
        }
    }
}