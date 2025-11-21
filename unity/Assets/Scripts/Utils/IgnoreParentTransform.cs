using UnityEngine;

public class IgnoreParentTransform : MonoBehaviour
{
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;
    private Vector3 initialLocalScale;

    void Awake()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
        initialLocalScale = transform.localScale;
    }

    void LateUpdate()
    {
        if (transform.parent != null)
        {
            // Ignore parent's scale
            Vector3 parentScale = transform.parent.lossyScale;
            transform.localScale = new Vector3(
                initialLocalScale.x / parentScale.x,
                initialLocalScale.y / parentScale.y,
                initialLocalScale.z / parentScale.z
            );

            // Ignore parent's rotation
            transform.rotation = initialLocalRotation;

            // Ignore parent's movement
            Vector3 worldPos = transform.position;
            Vector3 parentPos = transform.parent.position;
            transform.position = worldPos; // don't follow parent move
        }
    }
}