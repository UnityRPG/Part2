using UnityEngine;
using RPG.Inventories;
using RPG.Control;

namespace RPG.Questing.Completion
{
    public class Deliver : QuestCompletion
    {
        // TODO allow to specify required object
        // TODO quest not availalbe until player has special item?

        // TODO consdier other completion criteria with Rick
        void OnTriggerEnter(Collider other)
        {
            PlayerControl player = other.GetComponent<PlayerControl>();
            if (!player) { return; }
            if (PlayerHasRequiredObject(player))
            {
                CompleteQuest();
            }
        }

        private bool PlayerHasRequiredObject(PlayerControl player)
        {
            bool playerIsCarryingSpecialItem = player.GetComponent<Inventory>().IsPlayerCarrying();
            return playerIsCarryingSpecialItem;
        }
    }
}