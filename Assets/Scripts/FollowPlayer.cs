using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;      // Drag your Player here
    public float smoothSpeed = 0.125f;
    public Vector3 offset;        // Example: (0, 10, -5) for top-down

    void LateUpdate() {
        Vector3 desiredPosition = target.position + offset;
        // Smoothly move from current position to desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target); // Keep the player centered
    }
}