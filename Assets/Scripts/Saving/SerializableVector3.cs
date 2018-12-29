namespace RPG.Saving
{
    using UnityEngine;

    [System.Serializable]
    public struct SerializableVector3
    {
        readonly float x, y, z;

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public static implicit operator SerializableVector3(Vector3 vector)
        {
            return new SerializableVector3(vector);
        }

        public static implicit operator Vector3(SerializableVector3 vector)
        {
            return new Vector3
            {
                x = vector.x,
                y = vector.y,
                z = vector.z
            };
        }
    }
}