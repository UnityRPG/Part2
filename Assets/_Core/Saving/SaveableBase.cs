using UnityEngine;

public abstract class SaveableBase : MonoBehaviour {
    [SerializeField]
    string _UniqueIdentifier;

    public string UniqueIdentifier
    {
        get { return _UniqueIdentifier; }
    }

    public abstract object CaptureState();
    public abstract void RestoreState(object state);
}
