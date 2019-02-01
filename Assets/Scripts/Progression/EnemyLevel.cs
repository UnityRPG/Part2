using UnityEngine;

namespace RPG.Progression
{
    public class EnemyLevel : CharacterLevel
    {
        [SerializeField] string className;

        override protected CoreBaseStats GetStats()
        {
            return statSet.GetEnemy(className);
        }
    }
}