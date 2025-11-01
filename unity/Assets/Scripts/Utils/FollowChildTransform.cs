using UnityEngine;

namespace Utils
{
    public class FollowChildTransform : MonoBehaviour
    {
        [SerializeField] private Transform child;

        void LateUpdate()
        {
            if (child == null) return;
            transform.SetPositionAndRotation(child.position, child.rotation);
            transform.localScale = child.localScale;
        }
    }
}