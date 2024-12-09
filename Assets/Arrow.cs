using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform tip;
    public GameObject fireEffectPrefab; // Reference to the fire effect prefab
    private bool _isOnFire = false; // Flag to check if the arrow is on fire

    private bool _inAir = false;
    private Vector3 _lastPosition = Vector3.zero;
    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        PullInteraction.PullActionReleased += Release;
        Stop();
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= Release;
    }

    private void Release(float value)
    {
        PullInteraction.PullActionReleased -= Release;
        gameObject.transform.parent = null;
        _inAir = true;
        SetPhysics(true);

        Vector3 force = transform.forward * value;
        _rigidBody.AddForce(force, ForceMode.Impulse);
        StartCoroutine(RotateWithVelocity());
        _lastPosition = tip.position;
    }

    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (_inAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(_rigidBody.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("FireSource") && !_isOnFire)
        {
            IgniteArrow(); // Ignite the arrow if it touches a fire source
        }
    }

    void FixedUpdate()
    {
        if (_inAir)
        {
            CheckCollision();
            _lastPosition = tip.position;
        }
    }

    private void CheckCollision()
    {
        if (Physics.Linecast(_lastPosition, tip.position, out RaycastHit hitInfo))
        {
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("FireSource") && !_isOnFire)
            {
                print("TOUCHED FIRE");
                IgniteArrow();
            }
            else if (hitInfo.transform.gameObject.layer != 8)
            {
                if (hitInfo.transform.TryGetComponent(out Rigidbody body))
                {
                    _rigidBody.interpolation = RigidbodyInterpolation.None;
                    transform.parent = hitInfo.transform;
                    body.AddForce(_rigidBody.velocity, ForceMode.Impulse);
                }
                Stop();
            }
        }
    }

    private void IgniteArrow()
    {
        _isOnFire = true;
        Instantiate(fireEffectPrefab, tip.position, Quaternion.identity, tip); // Attach fire effect to arrow tip
        // Add additional fire-related logic here, e.g., play a sound or apply damage effects
    }

    private void Stop()
    {
        _inAir = false;
        SetPhysics(false);
    }

    private void SetPhysics(bool usePhysics)
    {
        _rigidBody.useGravity = usePhysics;
        _rigidBody.isKinematic = !usePhysics;
    }
}
