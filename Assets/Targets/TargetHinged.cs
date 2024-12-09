using UnityEngine;

public class TargetHinged : MonoBehaviour
{
    public delegate void TargetHit(TargetHinged target);
    public static event TargetHit OnTargetHit;

    private bool isRotating = false;
    private float rotationDuration = 1.0f; // Duration of rotation
    private float rotationStartTime;
    private bool isHit = false; // Track whether this target has been hit already

    private Quaternion startRotation;
    private Quaternion endRotation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow") && !isHit)
        {
            // Make the arrow a child of the target to ensure it rotates with it
            other.transform.SetParent(transform);

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
                // Interpolate the rotation from start to end
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, fractionOfDuration);
            }
            else
            {
                // Ensure exact final rotation and stop rotating
                transform.rotation = endRotation;
                isRotating = false;
            }
        }
    }

    private void StartRotation()
    {
        isRotating = true;
        rotationStartTime = Time.time;

        startRotation = transform.rotation;
        Vector3 targetEulerAngles = new Vector3(transform.eulerAngles.x + 90f, transform.eulerAngles.y, transform.eulerAngles.z);
        endRotation = Quaternion.Euler(targetEulerAngles);
    }
}