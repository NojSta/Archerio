using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject notch;
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

        // Check if currentInteractor and notch are valid, and if arrow is close enough to the notch
        // Check if the arrow in hand is close enough to the notch to attach
        if (arrowInHand && currentInteractor != null &&
            Vector3.Distance(currentInteractor.transform.position, notch.transform.position) < putArrowRange)
        {
            AttachArrowToNotch();
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

    private void AttachArrowToNotch()
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
