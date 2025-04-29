using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 1f;
    private Vector3 locationOffset;
    private Quaternion initialRotation;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not assigned in FollowPlayer script.");
            enabled = false;
            return;
        }

        locationOffset = transform.position - target.position;
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + locationOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.rotation = initialRotation;
    }
}
