using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // The player or object to follow
    [SerializeField] private float smoothSpeed = 5f; // Speed of the camera movement
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -5); // Initial offset from the player

    private void LateUpdate()
    {
        if (target == null) return;

        // Calculate the rotated offset based on the player's rotation
        Vector3 rotatedOffset = target.rotation * offset;

        // Calculate the desired position of the camera
        Vector3 desiredPosition = target.position + rotatedOffset;

        // Smoothly move the camera to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Make the camera look at the player
        transform.LookAt(target);
    }
}
