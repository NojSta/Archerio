using UnityEngine;

public class HingeSwing : MonoBehaviour
{
    public float motorForce = 100f;
    public float motorSpeed = 50f;  // Set motor speed (target velocity)
    public float springForce = 50f;
    public float damperForce = 5f;
    public float limitMin = -45f;
    public float limitMax = 45f;

    public LineRenderer lineRenderer;  // Reference to LineRenderer
    public Transform movingPart; // Moving part (to visualize with LineRenderer)
    public Transform hingeAnchor; // Anchor position of the hinge

    private HingeJoint hingeJoint;
    private bool isReversing = false;

    void Start()
    {
        hingeJoint = GetComponent<HingeJoint>();

        // Set hinge joint limits
        JointLimits limits = hingeJoint.limits;
        limits.min = limitMin;
        limits.max = limitMax;
        hingeJoint.limits = limits;
        hingeJoint.useLimits = true;

        // Motor Setup
        JointMotor motor = hingeJoint.motor;
        motor.force = motorForce;
        motor.targetVelocity = motorSpeed;  // Initial velocity
        hingeJoint.motor = motor;

        // Spring Setup
        JointSpring spring = hingeJoint.spring;
        spring.spring = springForce;  // Force to restore the hinge to neutral
        spring.damper = damperForce;  // Damping to reduce oscillation speed
        hingeJoint.spring = spring;

        // Enable motor and spring
        hingeJoint.useMotor = true;
        hingeJoint.useSpring = true;

        // Initialize LineRenderer
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;  // 2 points: anchor and moving part

            // Set the line width to a smaller value
            lineRenderer.startWidth = 0.01f;  // Start width of the line
            lineRenderer.endWidth = 0.01f;    // End width of the line
        }

        StopSwingAndDrop();
    }

    void Update()
    {
        // Alternate the motor target velocity to create the oscillating effect
        JointMotor motor = hingeJoint.motor;

        // Reverse the direction of the motor when it hits the limit
        if (hingeJoint.angle >= limitMax || hingeJoint.angle <= limitMin)
        {
            isReversing = !isReversing;
            motor.targetVelocity = isReversing ? -motorSpeed : motorSpeed;
            hingeJoint.motor = motor;
        }

        // Update LineRenderer positions
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, hingeAnchor.position);  // Set anchor point
            lineRenderer.SetPosition(1, movingPart.position);   // Set moving part position
        }
    }

    public void StopSwingAndDrop()
    {
        // Disable the hinge motor and spring
        hingeJoint.useMotor = false;
        hingeJoint.useSpring = false;

        // Remove the LineRenderer line
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;  // Alternatively, you can destroy it: Destroy(lineRenderer.gameObject);
        }

        // Enable gravity on the object
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
        }
    }
}
