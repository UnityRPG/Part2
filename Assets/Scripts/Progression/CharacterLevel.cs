using RPG.Progression;
using UnityEngine;

namespace RPG.Characters
{
    abstract public class CharacterLevel : MonoBehaviour
    {
        [SerializeField] protected StatsProgression statSet;
        public int level;

        abstract protected CoreCharacterStats GetStats();

        public float health
        {
            get
            {
                return GetStats().health[level];
            }
        }
    }
}
