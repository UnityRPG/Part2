using UnityEngine;

namespace RPG.Core.Stats
{
    [CreateAssetMenu(menuName = ("RPG/Stat Set"))]
    public class StatSet : ScriptableObject
    {
        [SerializeField] PlayerStats playerStats;
        [SerializeField] EnemyClass[] enemyClasses;
    }
}