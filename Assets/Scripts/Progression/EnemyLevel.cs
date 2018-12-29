using UnityEngine;
using RPG.Progression;

namespace RPG.Progression
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