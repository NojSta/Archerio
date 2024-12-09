using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject[] notches; // Array to hold multiple notches
    public Transform quiverPosition;      // Set a position behind the player's back in the Inspector
    public float grabRange = 0.2f;        // The range to detect the hand reaching behind the back
    public float putArrowRange = 0.2f;

    private XRGrabInteractable bow;
    private XRBaseInteractor currentInteractor; // Reference to the VR hand grabbing the arrow
    private bool arrowInHand = false;
    private GameObject currentArrow = null;

    private void Start()
    {
        bow = GetComponent<XRGrabInteractable>();
        PullInteraction.PullActionReleased += NotchEmpty;
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= NotchEmpty;
    }

    private void Update()
    {
        // Check if an interactor (hand) is near the quiver position and spawn the arrow
        if (!arrowInHand && TryGetHandNearQuiver(out currentInteractor))
        {
            SpawnArrowInHand();
        }

        // Check if currentInteractor and notches are valid, and if arrow is close enough to any notch
        if (arrowInHand && currentInteractor != null)
        {
            GameObject closestNotch = GetClosestNotch();

            if (closestNotch != null)
            {
                // If the closest notch is within range, attach the arrow
                if (Vector3.Distance(currentInteractor.transform.position, closestNotch.transform.position) < putArrowRange)
                {
                    AttachArrowToNotch(closestNotch);
                }
            }
        }

        // If the bow is no longer held and there's an unnotched arrow, destroy it
        if (bow != null && !bow.isSelected && currentArrow != null && !arrowInHand)
        {
            Destroy(currentArrow);
            NotchEmpty(1f);
        }
    }

    private bool TryGetHandNearQuiver(out XRBaseInteractor interactor)
    {
        interactor = null;

        var interactors = FindObjectsOfType<XRBaseInteractor>();

        foreach (var xrInteractor in interactors)
        {
            if (Vector3.Distance(xrInteractor.transform.position, quiverPosition.position) < grabRange)
            {
                interactor = xrInteractor;
                return true;
            }
        }
        return false;
    }

    private void SpawnArrowInHand()
    {
        currentArrow = Instantiate(arrowPrefab, currentInteractor.transform);
        currentArrow.transform.localPosition = Vector3.zero;
        arrowInHand = true;
    }

    private GameObject GetClosestNotch()
    {
        GameObject closestNotch = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject notch in notches)
        {
            float distance = Vector3.Distance(currentInteractor.transform.position, notch.transform.position);
            if (distance < closestDistance && distance < putArrowRange)
            {
                closestDistance = distance;
                closestNotch = notch;
            }
        }

        return closestNotch;
    }

    private void AttachArrowToNotch(GameObject notch)
    {
        currentArrow.transform.SetParent(notch.transform, worldPositionStays: false);
        currentArrow.transform.localPosition = Vector3.zero;
        currentArrow.transform.localRotation = Quaternion.identity;
        arrowInHand = false;
    }

    private void NotchEmpty(float value)
    {
        arrowInHand = false;
        currentArrow = null;
    }
}
