using UnityEngine;

public class CameraInicio : MonoBehaviour
{
    public float breathSpeed = 0.1f; // Speed of breathing
    public float maxRotationAngle = 0.1f; // Maximum rotation angle

    private Quaternion originalRotation;
    private bool inhaling = true;

    void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        // Calculate rotation angle based on time
        float rotationAngle = Mathf.Sin(Time.time * breathSpeed) * maxRotationAngle;

        // Apply rotation to simulate breathing
        float targetAngle = inhaling ? rotationAngle : -rotationAngle;
        Quaternion targetRotation = originalRotation * Quaternion.Euler(targetAngle, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f); // Smooth rotation

        // Change breathing direction when reaching the maximum or minimum rotation angle
        if (rotationAngle >= maxRotationAngle || rotationAngle <= -maxRotationAngle)
        {
            inhaling = !inhaling;
        }
    }
}
