using UnityEngine;

namespace Interface
{
    public interface ITransfer
    {
        void MoveTo(Transform parent, Vector3 position, bool destroyedAtEnd = false);
    }
}
