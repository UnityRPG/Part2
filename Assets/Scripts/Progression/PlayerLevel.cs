using RPG.Progression;

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
