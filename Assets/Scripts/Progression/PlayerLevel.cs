using RPG.Progression;

namespace RPG.Progression
{
    public class PlayerLevel : CharacterLevel
    {
        override protected CoreCharacterStats GetStats()
        {
            return statSet.GetPlayer();
        }

    }
}
