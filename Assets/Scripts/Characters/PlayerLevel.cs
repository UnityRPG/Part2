using RPG.Stats;

namespace RPG.Characters
{
    public class PlayerLevel : CharacterLevel
    {
        override protected CoreCharacterStats GetStats()
        {
            return statSet.GetPlayer();
        }

    }
}
