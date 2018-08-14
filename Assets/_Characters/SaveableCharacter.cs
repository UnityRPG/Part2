namespace RPG.Characters
{
    using UnityEngine;
    using RPG.Saving;

    public class SaveableCharacter : SaveableBase
    {
        [System.Serializable]
        class CharacterState
        {
            public SerializableVector3 position;
        }

        public override object CaptureState()
        {
            var state = new CharacterState
            {
                position = GetComponent<Transform>().position
            };
            return state;
        }

        public override void RestoreState(object State)
        {
            var characterState = (CharacterState)State;
            GetComponent<Transform>().position = characterState.position;
            print((Vector3)characterState.position);
        }
    }
}