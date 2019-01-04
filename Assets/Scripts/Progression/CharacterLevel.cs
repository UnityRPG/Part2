using RPG.Progression;
using UnityEngine;

namespace RPG.Progression
{
    abstract public class CharacterLevel : MonoBehaviour
    {
        [SerializeField] protected BaseStatsProgression statSet;
        public int level;

        abstract protected CoreBaseStats GetStats();

        public float GetBaseStat(string statId)
        {
            return health; //TODO: Fix;
        }

        public float health
        {
            get
            {
                return GetStats().health[level];
            }
        }
    }
}
