using UnityEngine;

namespace RPG.Core.Stats
{
    [CreateAssetMenu(menuName = ("RPG/Stat Set"))]
    public class StatSet : ScriptableObject
    {
        [SerializeField] PlayerLevel[] playerLevels;
        [SerializeField] EnemyClass[] enemyClasses;
    }
}