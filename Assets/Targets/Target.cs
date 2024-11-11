using UnityEngine;
using System.Collections; // This is necessary for IEnumerator

public class Target : MonoBehaviour
{
    public delegate void TargetHit(Target target);
    public static event TargetHit OnTargetHit;

    [SerializeField]
    private float disappearDelay = 2f; // Time in seconds before the target disappears

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            OnTargetHit?.Invoke(this);
            StartCoroutine(DelayedDisappear()); // Use coroutine for delay
        }
    }

    private IEnumerator DelayedDisappear()
    {
        yield return new WaitForSeconds(disappearDelay);
        gameObject.SetActive(false);
    }
}