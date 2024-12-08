using UnityEngine;

public class TargetHinged : MonoBehaviour
{
public delegate void TargetHit(TargetHinged target);
    public static event TargetHit OnTargetHit;

    private bool isHit = false; // Track whether this target has been hit already
    private bool isRotating = false; // Track whether the target is currently rotating
    private float rotationDuration = 1.0f; // Duration to complete the rotation
    private float rotationStartTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow") && !isHit)
        {
            StartRotation();
            OnTargetHit?.Invoke(this); // Notify the GameManager that this target was hit
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
                isHit = true;
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
        // Assuming the pivot point is at (0, -0.5, 0) relative to the target's local position to simulate rotation around the bottom
        Vector3 pivotPoint = new Vector3(0f, -0.5f, 0f);
        Quaternion rotation = Quaternion.Euler(rotationAngle, 0, 0);
        transform.SetPositionAndRotation(pivotPoint + rotation * (transform.position - pivotPoint), rotation * transform.rotation);
    }

    public bool IsHit()
    {
        return isHit;
    }

    public void SetHitStatus(bool status)
    {
        isHit = status;
    }
}