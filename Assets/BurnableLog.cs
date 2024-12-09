using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BurnableLog : MonoBehaviour
{
    // Reference to a single GameObject with the HingeSwing script attached
    public GameObject hingeSwingObject;

    // Reference to the fire GameObjects (fire VFX or similar objects)
    public List<GameObject> fireObjects;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure all fire objects are initially disabled
        foreach (var fireObject in fireObjects)
        {
            if (fireObject != null)
            {
                fireObject.SetActive(false); // Disable the fire objects initially
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<Arrow>(out Arrow arrow))
        {
            if (arrow.IsOnFire())
            {
                Debug.Log("Arrow is on fire!");


                if (arrow.IsOnFire())
                {
                    Debug.Log("Arrow is on fire!");
                    // Enable all fire objects
                    foreach (var fireObject in fireObjects)
                    {
                        if (fireObject != null)
                        {
                            fireObject.SetActive(true); // Enable each fire object
                        }
                    }

                    if (hingeSwingObject != null)
                    {
                        if (hingeSwingObject.TryGetComponent<HingeSwing>(out HingeSwing hingeSwingScript))
                        {
                            StartCoroutine(StopSwingAndDropAfterDelay(3f, hingeSwingScript));
                        }
                        else
                        {
                            Debug.LogWarning("HingeSwing script not found on the hingeSwingObject.");
                        }
                    }


                }
            }
        }
    }

    private IEnumerator StopSwingAndDropAfterDelay(float delay, HingeSwing hingeSwingScript)
    {
        // Wait for the specified delay (3 seconds in this case)
        yield return new WaitForSeconds(delay);

        // Now call StopSwingAndDrop after the delay
        hingeSwingScript.StopSwingAndDrop();
    }
        
}
