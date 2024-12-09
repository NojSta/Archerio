using UnityEngine;

public class TargetHinged : MonoBehaviour
{
    public delegate void TargetHit(TargetHinged target);
    public static event TargetHit OnTargetHit;

    private bool isRotating = false;
    private float rotationDuration = 1.0f; // Duration of rotation
    private float rotationStartTime;
    private bool isHit = false; // Track whether this target has been hit already

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow") && !isHit)
        {
            StartRotation();
            isHit = true; // Indicate that target has been hit, preventing multiple triggers
            OnTargetHit?.Invoke(this); // Notify that this target was hit
        }
    }

    private void Update()
    {
        if (isRotating)
        {
            // Calculate the time since the rotation started
            float timeSinceStart = Time.time - rotationStartTime;
            // Calculate the fraction of the total duration that has passed
            float fractionOfDuration = timeSinceStart / rotationDuration;

            if (fractionOfDuration < 1.0f)
            {
                // Interpolate the rotation from 90 to 180 degrees
                float currentRotation = Mathf.Lerp(90f, 180f, fractionOfDuration);
                RotateTarget(currentRotation);
            }
            else
            {
                // Ensure exact final rotation and stop rotating
                RotateTarget(180f);
                isRotating = false;
            }
        }
    }

    private void StartRotation()
    {
        isRotating = true;
        rotationStartTime = Time.time;
    }

    private void RotateTarget(float rotationAngle)
    {
        // Rotate the parent (pivot) GameObject
        transform.rotation = Quaternion.Euler(rotationAngle, 0, 0);
    }
}