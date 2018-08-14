using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableCharacter : SaveableBase
{
    [System.Serializable]
    class CharacterState
    {
        public Vector3 position;
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
    }
}
