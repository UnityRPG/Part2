using RPG.Core.Stats;

namespace RPG.Characters
{
    public class PlayerLevel : CharacterLevel
    {
        override public CoreCharacterStats GetStats()
        {
            return statSet.GetPlayer();
        }

    }
}
