using RPG.Progression;

namespace RPG.Progression
{
    public class PlayerLevel : CharacterLevel
    {
        override protected CoreBaseStats GetStats()
        {
            return statSet.GetPlayer();
        }

    }
}
