using UnityEngine;
using RPG.Core.Stats;

namespace RPG.Characters
{
    public class EnemyLevel : CharacterLevel
    {
        [SerializeField] string className;

        override protected CoreCharacterStats GetStats()
        {
            return statSet.GetEnemy(className);
        }
    }
}