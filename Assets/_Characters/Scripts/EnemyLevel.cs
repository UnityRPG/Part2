using UnityEngine;
using RPG.Core.Stats;

namespace RPG.Characters
{
    public class EnemyLevel : CharacterLevel
    {
        [SerializeField] string className;

        override public CoreCharacterStats GetStats()
        {
            return statSet.GetEnemy(className);
        }
    }
}