using UnityEngine;

namespace Entity
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target Settings")]
        public Transform target;         // The player or character to follow

        [Header("Camera Offset Settings")]
        // Set the desired offset from the target.
        // For a 2.5D look, you might want a fixed z or y value.
        public Vector3 offset = new Vector3(0, 5, -10);

        [Header("Smooth Settings")]
        // How quickly the camera moves towards the target position.
        public float smoothSpeed = 0.125f;

        void LateUpdate()
        {
            // Check if the target has been assigned.
            if (target == null)
                return;

            // Calculate the desired position: the target's position plus an offset.
            Vector3 desiredPosition = target.position + offset;

            // Smoothly interpolate between the current camera position and the desired position.
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position.
            transform.position = smoothedPosition;

            // Optionally, have the camera always look at the target.
            transform.LookAt(target);
        }
    }
}